USE [air-demo]
GO

DROP PROCEDURE [dbo].[spChangeEvent]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-InsuranceChangeEvents]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [air-demo].[emp].[monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		enrolled = eioe.CoverageInForce
	FROM
		[air-demo].[emp].[monthly_detail] emd
		INNER JOIN [aca-demo].[dbo].[EmployeeInsuranceOfferEditable] eioe
			ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		eioe.TaxYearId = @yearId;

	UPDATE [air-demo].[appr].[employee_monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		enrolled = eioe.CoverageInForce
	FROM
		[air-demo].[appr].[employee_monthly_detail] emd
		INNER JOIN [aca-demo].[dbo].[EmployeeInsuranceOfferEditable] eioe
			ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		eioe.TaxYearId = @yearId;

	SET NOCOUNT OFF;
	
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spUpdateAIR]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;
	
	-- needs feature toggles on the database side. gc5

			EXECUTE [air-demo].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
				PRINT '1 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air-demo].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '2 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air-demo].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '3 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spUpdateAIR-MonthlyHoursAndStatus]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE 
		[air-demo].emp.monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		[air-demo].emp.monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) as MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   [aca-demo].dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN [aca-demo].dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca-demo].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca-demo].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   [aca-demo].dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN [aca-demo].dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca-demo].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca-demo].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN [air-demo].gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		[air-demo].emp.monthly_detail
	SET
		monthly_status_id =
			CASE
				--: Is employee In Termination Month?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						[air-demo].etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN [air-demo].etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours?
				WHEN ISNULL(emd.monthly_hours,0) = 0 THEN 7
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		[air-demo].emp.monthly_detail emd
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca-demo].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now appr.

	UPDATE 
		[air-demo].appr.employee_monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		[air-demo].appr.employee_monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) as MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   [aca-demo].dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN [aca-demo].dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca-demo].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca-demo].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   [aca-demo].dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN [aca-demo].dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca-demo].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca-demo].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN [air-demo].gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		[air-demo].appr.employee_monthly_detail
	SET
		monthly_status_id =
			CASE
				--: Is employee In Termination Month?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						[air-demo].etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN [air-demo].etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours?
				WHEN ISNULL(emd.monthly_hours,0) = 0 THEN 7
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		[air-demo].appr.employee_monthly_detail emd
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca-demo].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now reset the 1 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		offer_of_coverage_code = [air-demo].etl.ufnGetMecCode(
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- this date may be a bit off. gc5
					WHEN eioe.CoverageInForce = 1 THEN DATEFROMPARTS(tf.year_id, tf.month_id, 1)
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca-demo].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca-demo].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca-demo].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca-demo].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
	
	UPDATE [emp].[monthly_detail]
	SET
		offer_of_coverage_code = [air-demo].etl.ufnGetMecCode(
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					WHEN eioe.CoverageInForce = 1 THEN DATEFROMPARTS(tf.year_id, tf.month_id, 1)
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		[emp].[monthly_detail] emd
		INNER JOIN [aca-demo].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca-demo].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca-demo].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca-demo].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId


END

GO

