USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [etl].[ufnDoesMonthHaveOffer] (
	@employeeId INT,
	@timeFrameId INT
)
RETURNS BIT
AS
BEGIN

	-- more work is needed to determine the outcome of the coverage
	DECLARE @offerInForce BIT
	SELECT
		@offerInForce = emd.mec_offered
	FROM
		[air].[emp].[monthly_detail] emd
	WHERE
		emd.employee_id = @employeeId
			AND
		emd.time_frame_id = @timeFrameId

	return @offerInForce

END

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns 1 if the month in question has an offer of coverage in force.' , @level0type=N'SCHEMA',@level0name=N'etl', @level1type=N'FUNCTION',@level1name=N'ufnDoesMonthHaveOffer'
GO

GRANT EXECUTE ON [etl].[ufnDoesMonthHaveOffer] TO [air-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [etl].[ufnGetMecCode] (
		@employee_id INT,
		@timeframe_id SMALLINT, 
		@offered_to_employee BIT, 
		@offered_to_spouse BIT, 
		@offered_to_dependents BIT, 
		@min_value BIT, 
		@mainland BIT, 
		@effective_date DATE,	-- here for legacy reasons, no longer used. gc5
		@termination_date DATE,	-- here for legacy reasons, no longer used. gc5
		@aca_status TINYINT,
		@spouseConditional BIT
	)
RETURNS CHAR(2)
AS
BEGIN


IF
	(
		[etl].[ufnDoesMonthHaveOffer](@employee_id, @timeframe_id) = 0
	)

	RETURN '1H'

ELSE

	BEGIN

		RETURN 
		CASE
			WHEN  @min_value = 1 THEN
				CASE
					WHEN @offered_to_employee = 1 THEN
						CASE
							WHEN @spouseConditional = 1 AND @offered_to_dependents = 0 THEN '1J'
							WHEN @spouseConditional = 1 AND @offered_to_dependents = 1 THEN '1K'
							WHEN @mainland = 1 AND @offered_to_spouse = 1 AND @offered_to_dependents = 1 THEN '1A'
							WHEN @offered_to_spouse = 1 AND @offered_to_dependents = 1 THEN '1E'
							WHEN @offered_to_spouse = 1 AND @offered_to_dependents = 0 THEN '1D'
							WHEN @offered_to_spouse = 0 AND @offered_to_dependents = 1 THEN '1C'
							ELSE
								'1B'
						END
				END
			ELSE
				CASE
					WHEN @offered_to_employee = 1 THEN '1F'
					ELSE '1H' -- it is not offered to the employees then. :( gc5
				END
			END
	END

RETURN NULL

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
		air.emp.monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.emp.monthly_detail emd 
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
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month and not covered?
				--WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				WHEN 
					ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id
						AND
					[air].[etl].[ufnDoesTerminationMonthHaveCoverage](ee.employee_id, emd.time_frame_id, ee.terminationDate) = 0
				THEN 4
								
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours and not tagged full time in the demographics?
				WHEN
					ISNULL(emd.monthly_hours, 0) = 0
						AND
					ee.aca_status_id NOT IN (5)
				THEN 7

				--: Are there no monthly hours and tagged full time in the demographics? Let the ACA status determine state. gc5
				WHEN
					ISNULL(emd.monthly_hours, 0) = 0
						AND
					ee.aca_status_id = 5
				THEN 1
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.emp.monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now appr.

	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.appr.employee_monthly_detail emd 
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
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month and not covered?
				--WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				WHEN 
					ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id
						AND
					[air].[etl].[ufnDoesTerminationMonthHaveCoverage](ee.employee_id, emd.time_frame_id, ee.terminationDate) = 0
				THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours and not tagged full time in the demographics?
				WHEN
					ISNULL(emd.monthly_hours, 0) = 0
						AND
					ee.aca_status_id NOT IN (5)
				THEN 7

				--: Are there no monthly hours and tagged full time in the demographics? Let the ACA status determine state. gc5
				WHEN
					ISNULL(emd.monthly_hours, 0) = 0
						AND
					ee.aca_status_id = 5
				THEN 1
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.appr.employee_monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now reset the 1 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.employee_id,
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
	
	-- now the emp ones.
	UPDATE [emp].[monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.employee_id,
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		 [emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now reset the 2 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		safe_harbor_code = [etl].[ufnGetSafeHarborCode](
				emd.monthly_status_id,
				ISNULL(emd.mec_offered, 0),
				emd.enrolled,
				ee.terminationDate,
				ee.aca_status_id,
				ec.ash_code
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

	-- now emp.
	UPDATE [emp].[monthly_detail]
	SET
		safe_harbor_code = [etl].[ufnGetSafeHarborCode](
				emd.monthly_status_id,
				ISNULL(emd.mec_offered, 0),
				emd.enrolled,
				ee.terminationDate,
				ee.aca_status_id,
				ec.ash_code
			)
	FROM
		[emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [etl].[spInsert_employee_monthly_detail]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '7: Delete Employee Monthly Detail'
DELETE air.emp.monthly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(time_frame_id IN(SELECT time_frame_id FROM air.gen.time_frame WHERE year_id = @year_id))
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '7: Insert Employee Monthly Detail';
WITH
	employees_in_payroll_during_year AS
	(
		SELECT DISTINCT
			p.employee_id,
			p.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			classification_id,
			1 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.payroll p
			CROSS JOIN air.gen.time_frame  t
			INNER JOIN aca.dbo.employee ee ON (p.employee_id = ee.employee_id)
		WHERE 
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.edate > CONVERT(DATETIME,'2014-12-31 00:00:00', 102)) 
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.sdate < CONVERT(DATETIME,'2016-01-01 00:00:00', 102)) 
				AND
			(p.employer_id = @employer_id)
				AND
			(t.year_id = @year_id)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000))
	),
	employees_in_ci_not_in_payroll_in_year AS
	(
		SELECT DISTINCT
			ee.employee_id,
			ee.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			ee.classification_id,
			0 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.employee ee
			INNER JOIN air.emp.covered_individual ci ON (ee.employee_id = ci.employee_id)
			CROSS JOIN air.gen.time_frame  t
			LEFT OUTER JOIN employees_in_payroll_during_year p ON (ee.employee_id = p.employee_id) 
		WHERE 
			(ee.employer_id = @employer_id)
				AND
			(t.year_id = @year_id)
				AND
			(p.employee_id IS NULL)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000))
	),
	employees_in_year AS
	(
		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_payroll_during_year

		UNION

		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_ci_not_in_payroll_in_year
	),
	employees_monthly_aggregates AS
	(
		SELECT
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			CASE
				 --: Is employee In Termination Month?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ey.terminationDate), MONTH(ey.terminationDate)),0) = ey.time_frame_id THEN 4
				--: Is employee Terminated and not in Termination Month?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ey.terminationDate), MONTH(ey.terminationDate)),1000) < ey.time_frame_id THEN 5 
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ey.hireDate), MONTH(ey.hireDate)),0) > ey.time_frame_id THEN 5 
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ey.initialMeasurmentEnd), MONTH(ey.initialMeasurmentEnd)), 0) >= ey.time_frame_id THEN 3 
				--: Is employee In Administrative Period?
				WHEN ISNULL(
					air.etl.ufnGetTimeFrameID(
						IIF(
								YEAR(ey.hireDate) = @year_id,
								YEAR(ey.hireDate),
								@year_id + 1
							), 
						MONTH(ey.hireDate)
					),
					0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@year_id), MONTH(ey.hireDate)) AND ey.time_frame_id THEN 6
				--: Are there no monthly hours?
				WHEN ISNULL(mh.monthly_hours,0) = 0 THEN 7
				--: Is full-time according to hours?
				WHEN mh.monthly_hours > 129.99 THEN 1
				--: Is part-time according to hours? 
				WHEN mh.monthly_hours < 130 THEN 2 
			END AS monthly_status_id, 
			ey.hireDate,
			ey.terminationDate,
			ey.initialMeasurmentEnd,
			mi.slcmp AS share_lowest_cost_monthly_premium, 
			CASE
				WHEN mi.offeredOn IS NOT NULL THEN 1
				ELSE 0 
			END AS offered_coverage, 
			ISNULL(accepted,0) AS enrolled,
			mi.offeredOn,
			mi.acceptedOn,
			mi.effectiveDate,
			mi.minValue,
			mi.offSpouse,
			mi.offDependent,
			mi.insurance_type_id, 
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			ey.aca_status_id,
			mi.monthly_flex,
			mi.SpouseConditional
		FROM
			employees_in_year ey
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyHours(@employer_id, @year_id) mh ON (ey.employee_id = mh.employee_id) AND (ey.time_frame_id = mh.time_frame_id)
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyInsurance(@employer_id, @year_id) mi ON (ey.employee_id = mi.employee_id) AND ((ey.time_frame_id = mi.time_frame_id))
			LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ey.classification_id = ec.classification_id)
		GROUP BY
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			calculated_sumer_hours,
			mh.time_frame_id,
			hireDate,
			ey.initialMeasurmentEnd,
			mi.slcmp,
			ey.terminationDate,
			offeredOn,
			acceptedOn,
			accepted,
			effectiveDate,
			mi.minValue,
			mi.offSpouse, 
			mi.offDependent,
			mi.insurance_type_id,
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			aca_status_id,
			mi.monthly_flex,
			mi.SpouseConditional
	)

INSERT INTO air.emp.monthly_detail (
		employee_id,
		time_frame_id,
		employer_id,
		monthly_hours,
		offer_of_coverage_code,
		mec_offered,
		share_lowest_cost_monthly_premium,
		safe_harbor_code,
		enrolled,
		monthly_status_id,
		insurance_type_id,
		hra_flex_contribution
	)
SELECT
	employee_id,
	time_frame_id,
	@employer_id,
	monthly_hours, 
	air.etl.ufnGetMecCode(employee_id,time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id, SpouseConditional) AS offer_of_coverage_code,
	minValue, 
	IIF(
				air.etl.ufnGetMonthFromTimeFrame(time_frame_id) >= MONTH(terminationDate),
				NULL, 
				share_lowest_cost_monthly_premium
			), 
	air.etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code,
	enrolled,
	monthly_status_id,
	insurance_type_id,
	monthly_flex
FROM
	employees_monthly_aggregates

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [etl].[ufnDoesTerminationMonthHaveCoverage] (
	@employeeId INT,
	@timeFrameId INT,
	@terminationDate DATETIME
)
RETURNS BIT
AS
BEGIN

	IF (@terminationDate IS NULL)
		-- short circuit and return
		RETURN NULL

	IF (air.etl.ufnGetTimeFrameID(YEAR(@terminationDate), MONTH(@terminationDate)) = @timeFrameId)

		-- more work is needed to determine the outcome of the coverage
		DECLARE @coverageInForce BIT
		SELECT
			@coverageInForce = emd.enrolled
		FROM
			[emp].[monthly_detail] emd
		WHERE
			emd.employee_id = @employeeId
				AND
			emd.time_frame_id = @timeFrameId

		return @coverageInForce

	-- default answer
	RETURN NULL

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
		air.emp.monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.emp.monthly_detail emd 
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
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month and not covered?
				--WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				WHEN 
					ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id
						AND
					[air].[etl].[ufnDoesTerminationMonthHaveCoverage](ee.employee_id, emd.time_frame_id, ee.terminationDate) = 0
				THEN 4
								
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours and not tagged full time in the demographics?
				WHEN
					ISNULL(emd.monthly_hours, 0) = 0
						AND
					ee.aca_status_id NOT IN (5)
				THEN 7

				--: Are there no monthly hours and tagged full time in the demographics? Let the ACA status determine state. gc5
				WHEN
					ISNULL(emd.monthly_hours, 0) = 0
						AND
					ee.aca_status_id = 5
				THEN 1
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is measuring part-time according to hours and not tagged full time in the demographics? 
				WHEN
					emd.monthly_hours < 130
						AND
					ee.aca_status_id NOT IN (5)
				THEN 2 

				--: Is part-time according to hours and tagged full time in the demographics? Let the ACA status determine state. gc5 
				WHEN
					emd.monthly_hours < 130
						AND
					ee.aca_status_id = 5
				THEN 2 

			END
	FROM 
		air.emp.monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now appr.

	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.appr.employee_monthly_detail emd 
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
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month and not covered?
				--WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				WHEN 
					ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id
						AND
					[air].[etl].[ufnDoesTerminationMonthHaveCoverage](ee.employee_id, emd.time_frame_id, ee.terminationDate) = 0
				THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours and not tagged full time in the demographics?
				WHEN
					ISNULL(emd.monthly_hours, 0) = 0
						AND
					ee.aca_status_id NOT IN (5)
				THEN 7

				--: Are there no monthly hours and tagged full time in the demographics? Let the ACA status determine state. gc5
				WHEN
					ISNULL(emd.monthly_hours, 0) = 0
						AND
					ee.aca_status_id = 5
				THEN 1
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.appr.employee_monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now reset the 1 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.employee_id,
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
	
	-- now the emp ones.
	UPDATE [emp].[monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.employee_id,
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		 [emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now reset the 2 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		safe_harbor_code = [etl].[ufnGetSafeHarborCode](
				emd.monthly_status_id,
				ISNULL(emd.mec_offered, 0),
				emd.enrolled,
				ee.terminationDate,
				ee.aca_status_id,
				ec.ash_code
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

	-- now emp.
	UPDATE [emp].[monthly_detail]
	SET
		safe_harbor_code = [etl].[ufnGetSafeHarborCode](
				emd.monthly_status_id,
				ISNULL(emd.mec_offered, 0),
				emd.enrolled,
				ee.terminationDate,
				ee.aca_status_id,
				ec.ash_code
			)
	FROM
		[emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-Set1GCodes]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL,
@user_name NVARCHAR(100)=NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None Currently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

	-- used to be part 12 in the employer yearly detail but it was removing values it should not be
	-- based on outdated information. gc5

	-- appr first.

	UPDATE air.appr.employee_yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.appr.employee_yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		-- yet where enrolled in self funded insurance (type = 2)
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(insurance_type_id = 2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	UPDATE air.appr.employee_monthly_detail
	SET
		offer_of_coverage_code = '1G',
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL
	FROM
		air.emp.monthly_detail emd
		INNER JOIN [air].[emp].[yearly_detail] eyd ON (eyd.employee_id = emd.employee_id)
	WHERE
		eyd._1095C = 1
			AND
		eyd. is_1G = 1
			AND
		(eyd.year_id = @year_id)
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- now appr first.

	UPDATE air.emp.yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.emp.yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		-- yet where enrolled in self funded insurance (type = 2)
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(insurance_type_id = 2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	UPDATE air.emp.monthly_detail
	SET
		offer_of_coverage_code = '1G',
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL
	FROM
		air.emp.monthly_detail emd
		INNER JOIN [air].[emp].[yearly_detail] eyd ON (eyd.employee_id = emd.employee_id)
	WHERE
		eyd._1095C = 1
			AND
		eyd. is_1G = 1
			AND
		(eyd.year_id = @year_id)
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-Set1GCodes]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL,
@user_name NVARCHAR(100)=NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None Currently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

	-- used to be part 12 in the employer yearly detail but it was removing values it should not be
	-- based on outdated information. gc5

	-- appr first.

	UPDATE air.appr.employee_yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.appr.employee_yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		-- yet where enrolled in self funded insurance (type = 2)
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(insurance_type_id = 2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	UPDATE air.appr.employee_monthly_detail
	SET
		offer_of_coverage_code = '1G',
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL
	FROM
		air.emp.monthly_detail emd
		INNER JOIN [air].[emp].[yearly_detail] eyd ON (eyd.employee_id = emd.employee_id)
	WHERE
		eyd.employee_id = emd.employee_id
			AND
		eyd._1095C = 1
			AND
		eyd. is_1G = 1
			AND
		(eyd.year_id = @year_id)
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- now appr first.

	UPDATE air.emp.yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.emp.yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		-- yet where enrolled in self funded insurance (type = 2)
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(insurance_type_id = 2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	UPDATE air.emp.monthly_detail
	SET
		offer_of_coverage_code = '1G',
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL
	FROM
		air.emp.monthly_detail emd
		INNER JOIN [air].[emp].[yearly_detail] eyd ON (eyd.employee_id = emd.employee_id)
	WHERE
		eyd.employee_id = emd.employee_id
			AND
		eyd._1095C = 1
			AND
		eyd. is_1G = 1
			AND
		(eyd.year_id = @year_id)
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spUpdateAIR-Set1GCodes]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL,
@user_name NVARCHAR(100)=NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None Currently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

	-- used to be part 12 in the employer yearly detail but it was removing values it should not be
	-- based on outdated information. gc5

	-- appr first.

	UPDATE air.appr.employee_yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.appr.employee_yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		-- yet where enrolled in self funded insurance (type = 2)
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(insurance_type_id = 2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	UPDATE air.appr.employee_monthly_detail
	SET
		offer_of_coverage_code = '1G',
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL
	FROM
		air.emp.monthly_detail emd
		INNER JOIN [air].[emp].[yearly_detail] eyd ON (eyd.employee_id = emd.employee_id)
	WHERE
		emd.employee_id IN (
			SELECT
				employee_id
			FROM
				air.appr.employee_yearly_detail eyd
			WHERE
				eyd._1095C = 1
					AND
				eyd. is_1G = 1
					AND
				(eyd.year_id = @year_id)
		)
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- now emp.

	UPDATE air.emp.yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.emp.yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		-- yet where enrolled in self funded insurance (type = 2)
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(insurance_type_id = 2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	UPDATE air.emp.monthly_detail
	SET
		offer_of_coverage_code = '1G',
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL
	FROM
		air.emp.monthly_detail emd
		INNER JOIN [air].[emp].[yearly_detail] eyd ON (eyd.employee_id = emd.employee_id)
	WHERE
		emd.employee_id IN (
			SELECT
				employee_id
			FROM
				air.emp.yearly_detail eyd
			WHERE
				eyd._1095C = 1
					AND
				eyd. is_1G = 1
					AND
				(eyd.year_id = @year_id)
		)
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-Set1GCodes]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL,
@user_name NVARCHAR(100)=NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None Currently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

	-- used to be part 12 in the employer yearly detail but it was removing values it should not be
	-- based on outdated information. gc5

	-- appr first.

	UPDATE air.appr.employee_yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.appr.employee_yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		-- yet where enrolled in self funded insurance (type = 2)
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(insurance_type_id = 2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- regs say the 1G is for all rows and to kill other values.
	UPDATE [appr].[employee_monthly_detail]
	SET
		offer_of_coverage_code = '1G',
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [gen].time_frame tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employee_id IN (
			SELECT
				eyd.employee_id
			FROM
				[appr].[employee_yearly_detail] eyd
			WHERE
				eyd.employer_id = @employer_id
					AND
				eyd.year_id = @year_id
					AND
				eyd.is_1G = 1
					AND
				eyd._1095C = 1
		)
			AND
		tf.year_id = @year_id

	-- now emp.

	UPDATE air.emp.yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.emp.yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		-- yet where enrolled in self funded insurance (type = 2)
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(insurance_type_id = 2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- regs say the 1G is for all rows and to kill other values.
	UPDATE [emp].[monthly_detail]
	SET
		offer_of_coverage_code = '1G',
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL
	FROM
		[emp].[monthly_detail] emd
		INNER JOIN [gen].time_frame tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employee_id IN (
			SELECT
				eyd.employee_id
			FROM
				[emp].[yearly_detail] eyd
			WHERE
				eyd.employer_id = @employer_id
					AND
				eyd.year_id = @year_id
					AND
				eyd.is_1G = 1
					AND
				eyd._1095C = 1
		)
			AND
		tf.year_id = @year_id

GO
