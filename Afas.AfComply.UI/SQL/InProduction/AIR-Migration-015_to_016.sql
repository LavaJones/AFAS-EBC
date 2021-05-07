USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateAIR-SetLine15]
	@employerId INT,
	@yearId SMALLINT
AS

	-- clear emp monthly first.
	UPDATE [air].[emp].[monthly_detail]
	SET
		monthly_hours = -1,
		offer_of_coverage_code = NULL,
		mec_offered = 0,
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL,
		enrolled = 0,
		monthly_status_id = 7,
		insurance_type_id = NULL,
		hra_flex_contribution = NULL
	FROM
		[air].[emp].[monthly_detail] emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now reset with our values.
	UPDATE [air].[emp].[monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		share_lowest_cost_monthly_premium = i.monthlycost - ic.amount,
		enrolled = eioe.CoverageInForce,
		insurance_type_id = eioe.InsuranceId,
		hra_flex_contribution = eioe.HraFlexContribution
	FROM
		[air].[emp].[monthly_detail] emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[plan_year] py ON (py.plan_year_id = eioe.PlanYearId)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = eioe.InsuranceId AND i.plan_year_id = py.plan_year_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.insurance_id = i.insurance_id AND ic.ins_cont_id = eioe.InsuranceContributionId)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now clear appr monthly.
	UPDATE [air].[appr].[employee_monthly_detail]
	SET
		monthly_hours = -1,
		offer_of_coverage_code = NULL,
		mec_offered = 0,
		share_lowest_cost_monthly_premium = NULL,
		safe_harbor_code = NULL,
		enrolled = 0,
		monthly_status_id = 7,
		insurance_type_id = NULL,
		hra_flex_contribution = NULL
	FROM
		[air].[appr].[employee_monthly_detail] emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now reset with our values.
	UPDATE [air].[appr].[employee_monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		share_lowest_cost_monthly_premium = i.monthlycost - ic.amount,
		enrolled = eioe.CoverageInForce,
		insurance_type_id = eioe.InsuranceId,
		hra_flex_contribution = eioe.HraFlexContribution
	FROM
		[air].[appr].[employee_monthly_detail] emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[plan_year] py ON (py.plan_year_id = eioe.PlanYearId)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = eioe.InsuranceId AND i.plan_year_id = py.plan_year_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.insurance_id = i.insurance_id AND ic.ins_cont_id = eioe.InsuranceContributionId)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-SetLine15] TO [air-user] AS [dbo]
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

	-- TODO: Grab all ACA Full time not in AIR.

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employerId, @yearId
				PRINT '1 *** Cleanup the overwritten line 15 entries. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
				PRINT '2 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '3 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].dbo.[spUpdateAir-Set1GCodes] @employerId, @yearId
				PRINT '4 *** Setting 1G flags now that all of the data is ready. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '5 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP]
	@employerId INT,
	@yearId SMALLINT
AS

	-- insert these folks into emp first.
	-- We let the Line15, InsuranceChangeEvents and MonthlyHoursAndStatus clean things up.
	INSERT INTO [air].[emp].[monthly_detail] (
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
		EmployeeId,
		TimeFrameId,
		EmployerId,
		-2 AS monthly_hours,
		NULL AS offer_of_coverage_code,
		0 AS mec_offered,
		NULL AS share_lowest_cost_monthly_premium,
		NULL AS safe_harbor_code,
		0 AS enrolled,
		7 AS monthly_status_id,
		NULL AS insurance_type_id,
		NULL AS hra_flex_contribution
	FROM
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe
		INNER JOIN [air].[emp].[employee] ee ON (ee.employee_id = eioe.EmployeeId)
		INNER JOIN [aca].[dbo].[employee] adee ON (adee.employee_id = ee.employee_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		adee.aca_status_id = 5	-- full time
			AND
		(
			adee.terminationDate IS NULL
				OR
			adee.terminationDate >= DATEFROMPARTS(@yearId, 1, 1) -- Jan of the year
		)
			AND
		eioe.EmployeeId NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				[air].[emp].[monthly_detail] emd
				INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @yearId
		)

	-- now into appr.
	INSERT INTO [air].[appr].[employee_monthly_detail] (
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
		EmployeeId,
		TimeFrameId,
		EmployerId,
		-2 AS monthly_hours,
		NULL AS offer_of_coverage_code,
		0 AS mec_offered,
		NULL AS share_lowest_cost_monthly_premium,
		NULL AS safe_harbor_code,
		0 AS enrolled,
		7 AS monthly_status_id,
		NULL AS insurance_type_id,
		NULL AS hra_flex_contribution
	FROM
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe
		INNER JOIN [air].[emp].[employee] ee ON (ee.employee_id = eioe.EmployeeId)
		INNER JOIN [aca].[dbo].[employee] adee ON (adee.employee_id = ee.employee_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		adee.aca_status_id = 5	-- full time
			AND
		(
			adee.terminationDate IS NULL
				OR
			adee.terminationDate >= DATEFROMPARTS(@yearId, 1, 1) -- Jan of the year
		)
			AND
		eioe.EmployeeId NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				[air].[appr].[employee_monthly_detail] emd
				INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @yearId
		)

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] TO [air-user] AS [dbo]
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

			EXECUTE [air].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employerId, @yearId
				PRINT '1 *** Grab any potential full time coded employees that did not finish their IMP. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employerId, @yearId
				PRINT '2 *** Cleanup the overwritten line 15 entries. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
				PRINT '3 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '4 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].dbo.[spUpdateAir-Set1GCodes] @employerId, @yearId
				PRINT '5 *** Setting 1G flags now that all of the data is ready. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '6 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/28/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_employee_yearly_detail]
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
PRINT '12: Delete Appr Employee Yearly Detail';
DELETE air.emp.yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Insert Appr Employee Yearly Detail';
INSERT INTO air.emp.yearly_detail (
		employee_id,
		year_id,
		employer_id,
		annual_offer_of_coverage_code,
		annual_share_lowest_cost_monthly_premium,
		annual_safe_harbor_code,
		enrolled,
		insurance_type_id,
		must_supply_ci_info
	)
SELECT DISTINCT
	emd.employee_id,
	t.year_id,
	emd.employer_id,
	cc.offer_of_coverage_code,
	slcmp.share_lowest_cost_monthly_premium,
	sh.safe_harbor_code,
	ISNULL(enr.enrolled, 0) AS enrolled, 
	ISNULL(it1.insurance_type_id, 0) + ISNULL(it2.insurance_type_id, 0) AS insurance_type_id,
	IIF(
				(it2.employee_id IS NOT NULL) OR (it3.employee_id IS NOT NULL),
				1,
				0
			) AS must_supply_ci_info
FROM
	air.emp.monthly_detail emd 
	INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.offer_of_coverage_code) AS offer_of_coverage_code,
				t.year_id
			FROM
				air.emp.monthly_detail emd 
				INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
			GROUP BY
				emd.employee_id,
				t.year_id
			HAVING
				(COUNT(emd.offer_of_coverage_code) = 12)
					AND
				(COUNT(DISTINCT emd.offer_of_coverage_code) = 1)
		) cc ON (emd.employee_id = cc.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.share_lowest_cost_monthly_premium) AS share_lowest_cost_monthly_premium,
				t.year_id
			FROM
				air.emp.monthly_detail emd 
				INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
			GROUP BY
				emd.employee_id,
				t.year_id
			HAVING
				(COUNT(emd.share_lowest_cost_monthly_premium) = 12)
					AND 
				(COUNT(DISTINCT emd.share_lowest_cost_monthly_premium) = 1)
		) slcmp ON (emd.employee_id = slcmp.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.safe_harbor_code) AS safe_harbor_code,
				t.year_id
			FROM
				air.emp.monthly_detail emd 
				INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
			GROUP BY
				emd.employee_id,
				t.year_id
			HAVING
				(COUNT(emd.safe_harbor_code) = 12)
					AND
				(COUNT(DISTINCT emd.safe_harbor_code) = 1)
		) sh ON (emd.employee_id = sh.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				enrolled 
			FROM
				air.emp.monthly_detail 
			WHERE
				enrolled = 1
		) enr ON (emd.employee_id = enr.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
			FROM
				air.emp.monthly_detail
			WHERE
				(insurance_type_id = 1)
					AND
				(enrolled = 1)
		) it1 ON (emd.employee_id = it1.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
			FROM
				air.emp.monthly_detail 
			WHERE
				(insurance_type_id = 2)
					AND
				(enrolled = 1)
		) it2 ON (emd.employee_id = it2.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
				FROM
					air.emp.monthly_detail
				WHERE
					(insurance_type_id IN (1,2))
						AND
					(enrolled = 0)
						AND 
					(employee_id IN(SELECT DISTINCT employee_id FROM air.emp.covered_individual))
		) it3 ON (emd.employee_id = it3.employee_id)
WHERE
	(emd.employer_id = @employer_id)
		AND
	(t.year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000))

-- OLD 1G logic is elsewhere, gc5

PRINT '12: Update Employee _1095C Status';
EXECUTE air.etl.spUpdate_1095C_status @employer_id, @year_id, @employee_id

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

	-- emp hours first.
	UPDATE air.emp.monthly_detail
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

	-- now that we have real hours, reset the emp monthly_status_id
	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month and not covered?
				WHEN 
					ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id
						AND
					[air].[etl].[ufnDoesTerminationMonthHaveCoverage](ee.employee_id, emd.time_frame_id, ee.terminationDate) = 0
				THEN 4
								
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)), 0) > emd.time_frame_id THEN 5 
		
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
				THEN 1 

			END
	FROM 
		air.emp.monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId
	
	-- now the emp 1 codes.
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

	-- now emp 2 codes
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

	-- now appr hours.
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

	-- now that we have real appr hours, reset the monthly_status_id
	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month and not covered?
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

	-- now reset the appr 1 codes.
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

	-- now reset the appr 2 codes.
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

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR]
	@employerId INT,
	@yearId INT,
	@employeeId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;
	
	-- needs feature toggles on the database side. gc5

			EXECUTE [air].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employerId, @yearId
				PRINT '1 *** Grab any potential full time coded employees that did not finish their IMP. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employerId, @yearId
				PRINT '2 *** Cleanup the overwritten line 15 entries. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
				PRINT '3 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '4 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.etl.spInsert_employee_yearly_detail @employerId, @yearId, @employeeId
				PRINT '5 *** End Insert Insert Employee Yearly Detail - redux ***'
				PRINT ''
				PRINT ''

			EXECUTE air.appr.spInsert_employee_yearly_detail_init @employerId, @yearId, @employeeId, 'IRSTransmissionETL'
				PRINT '6 *** End Insert Appr Employee Yearly Detail - redux ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].dbo.[spUpdateAir-Set1GCodes] @employerId, @yearId
				PRINT '7 *** Setting 1G flags now that all of the data is ready. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '8 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/23/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spETL_Build]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL,
@aag_indicator BIT = 0,
@aag_code TINYINT = 2
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @ErrorMessage NVARCHAR(125)
DECLARE @dge_ein NCHAR(10)
DECLARE @_4980H_transition_relief_indicator BIT = 1
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
SELECT 
	@dge_ein = dge_ein,
	@_4980H_transition_relief_indicator = safeHarbor 
FROM
	aca.dbo.tax_year_approval 
WHERE
	(employer_id = @employer_id)
		AND
	(tax_year = @year_id)
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
BEGIN TRY
	BEGIN TRAN ETL_BUILD
		BEGIN
			EXECUTE air.etl.spInsert_ale_employer @employer_id, @year_id
				PRINT '1*** End Insert Employer***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_ale_dge @employer_id, @year_id
				PRINT '1A*** End Insert Dge***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spUpdate_employer @employer_id, @year_id
				PRINT '2*** End Update Employer***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spUpdate_ale_dge @employer_id, @year_id
				PRINT '2A*** End Update Employer***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_employee @employer_id, @employee_id
				PRINT '3*** End Insert Employee***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spUpdate_employee @employer_id, @employee_id
				PRINT '4*** End Update Employee***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_covered_individuals @employer_id, @year_id, @employee_id
				PRINT '5*** End Insert Covered Individuals***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_covered_individuals_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '6*** End Insert Covered Individuals Monthly***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '7*** End Insert Employee Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '8*** End Insert Insert Employee Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_ale_monthly_detail @employer_id, @year_id, @aag_indicator, @_4980H_transition_relief_indicator
				PRINT '9*** End Insert Ale Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '10*** End Insert Ale Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.appr.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '11*** End Insert Appr Employee Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.appr.spInsert_employee_yearly_detail_init @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '12*** End Insert Appr Employee Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.dbo.spUpdateAIR @employer_id, @year_id, @employee_id
				PRINT '13*** Update Hours and Monthly Status to be inline with ACA db.***'
				PRINT ''
				PRINT ''
		END	
	COMMIT TRAN BuildFormsTables;
	SELECT 'Successful';
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN ETL_BUILD
	SELECT ERROR_PROCEDURE() AS ErrorProcedure, ERROR_MESSAGE() AS ErrorMessage;
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/13/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER FUNCTION [etl].[ufnGetSafeHarborCode] (
		@monthly_status_id TINYINT, 
		@offered_coverage BIT, 
		@enrolled BIT, 
		@termination_date DATETIME, 
		@aca_status TINYINT, 
		@ash_code CHAR(2)
	)
RETURNS CHAR(2)
AS
BEGIN

	IF @monthly_status_id = 5 -- Employee is not an employee for this time frame.
		RETURN '2A' -- Not An Employee

	IF
		@offered_coverage = 1
			AND
		@enrolled = 1
			AND
		@aca_status IN (4) -- 4 is COBRA
			AND
		@monthly_status_id IN (5) -- Employee is not an employee for this time frame
		
		RETURN '2A' -- Not an Employee
	
	IF
		@offered_coverage = 1
			AND
		@enrolled = 1
			AND
		@monthly_status_id = 4 -- this time frame is your termination month
			AND
		DAY(@termination_date) < air.gen.ufnGetDayCountInMonth(@termination_date)
		
		RETURN '2B'	-- Part Time

	IF
		@offered_coverage = 1
			AND
		@enrolled = 1
			AND
		@monthly_status_id IN (1,2,3,4,6,7) -- Full time, PartTime, IMP, Termination Month, Admin, Unknown
	
		RETURN '2C'

	IF
		@offered_coverage = 1
			AND
		@enrolled = 0
			AND
		@monthly_status_id = 2 -- Part Time
		
		RETURN '2B' -- Part Time
	
	IF
		@offered_coverage = 1
			AND
		@enrolled = 0
			AND
		@monthly_status_id IN (1,3,6,7) -- Full Time, IMP, Admin, Unknown
		
		RETURN UPPER(@ash_code) -- employee class default.
	
	IF
		@offered_coverage = 0
			AND
		@monthly_status_id IN (3,6) -- IMP, Admin
	
		RETURN '2D' -- Limited Assessment Period

	RETURN NULL

END

GO
