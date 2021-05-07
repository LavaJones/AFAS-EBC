USE [air]
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
		@aca_status IN (4, 8) -- 4 is COBRA, 8 is retired.
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP]
	@employerId INT,
	@yearId SMALLINT,
	@employeeId INT = NULL
AS

	-- now into appr.
	-- We let the Line15, InsuranceChangeEvents and MonthlyHoursAndStatus clean things up.
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
		INNER JOIN [air].[emp].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eioe.EmployeeId)
		INNER JOIN [aca].[dbo].[employee] adee WITH (NOLOCK) ON (adee.employee_id = ee.employee_id)
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
				INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @yearId
		)
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(eioe.EmployeeId BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));


GO

GRANT EXECUTE ON [appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [appr].[spUpdateAIR-SetLine15]
	@employerId INT,
	@yearId SMALLINT,
	@employeeId INT = NULL
AS

	-- clear appr monthly.
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
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

	-- now reset with our values.
	UPDATE [air].[appr].[employee_monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		share_lowest_cost_monthly_premium =
			CASE
				WHEN ic.contribution_id = '$' THEN i.monthlycost - ic.amount
				WHEN ic.contribution_id = '%' THEN i.monthlycost - (i.monthlycost * (ic.amount/100))
				ELSE NULL
			END,
		enrolled = eioe.CoverageInForce,
		insurance_type_id = i.insurance_type_id,
		hra_flex_contribution = eioe.HraFlexContribution
	FROM
		[air].[appr].[employee_monthly_detail] emd
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK) ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[plan_year] py WITH (NOLOCK) ON (py.plan_year_id = eioe.PlanYearId)
		INNER JOIN [aca].[dbo].[insurance] i WITH (NOLOCK) ON (i.insurance_id = eioe.InsuranceId AND i.plan_year_id = py.plan_year_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic WITH (NOLOCK) ON (ic.insurance_id = i.insurance_id AND ic.ins_cont_id = eioe.InsuranceContributionId)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

GO

GRANT EXECUTE ON [appr].[spUpdateAIR-SetLine15] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [appr].[spUpdateAIR-InsuranceChangeEvents]
	@employerId int,
	@yearId smallint,
	@employeeId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE [air].[appr].[employee_monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		enrolled = eioe.CoverageInForce
	FROM
		[air].[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK) 
			ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		eioe.TaxYearId = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eioe.EmployeeId BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

	SET NOCOUNT OFF;
	
END

GO

GRANT EXECUTE ON [appr].[spUpdateAIR-InsuranceChangeEvents] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [appr].[spUpdateAIR-MonthlyHoursAndStatus]
	@employerId int,
	@yearId smallint,
	@employeeId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	-- appr hours.
	UPDATE [air].[appr].[employee_monthly_detail]
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.appr.employee_monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) AS MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea WITH (NOLOCK) ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (SELECT ee.employee_id FROM [aca].[dbo].[employee] ee WITH (NOLOCK) WHERE ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours AS MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea WITH (NOLOCK) ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (SELECT ee.employee_id FROM [aca].[dbo].[employee] ee WITH (NOLOCK) WHERE ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf WITH (NOLOCK) ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

	-- now that we have real appr hours, reset the monthly_status_id
	UPDATE air.appr.employee_monthly_detail
	SET
		monthly_status_id = [air].[etl].[ufnGetDerivedMonthlyStatus](
				emd.employee_id,
				emd.time_frame_id, 
				ee.terminationDate, 
				ee.hireDate,
				ee.initialMeasurmentEnd,
				ee.aca_status_id,
				emd.monthly_hours
			)
	FROM 
		air.appr.employee_monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
	WHERE
		emd.employer_id =  @employerId
			AND 
		tf.year_id = @yearId
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

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
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic WITH (NOLOCK) ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i WITH (NOLOCK) ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK) ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

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
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec WITH (NOLOCK) ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

END

GO

GRANT EXECUTE ON [appr].[spUpdateAIR-MonthlyHoursAndStatus] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [appr].[spUpdateAIR-Set1GCodes]
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

	-- appr pass.
	UPDATE [air].[appr].[employee_yearly_detail]
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
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

GRANT EXECUTE ON [appr].[spUpdateAIR-Set1GCodes] TO [air-user] AS DBO
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
				PRINT '1 *** End Insert Employer ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_ale_dge @employer_id, @year_id
				PRINT '1A *** End Insert Dge ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spUpdate_employer @employer_id, @year_id
				PRINT '2 *** End Update Employer ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spUpdate_ale_dge @employer_id, @year_id
				PRINT '2A *** End Update Employer ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_employee @employer_id, @employee_id
				PRINT '3 *** End Insert Employee ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spUpdate_employee @employer_id, @employee_id
				PRINT '4 *** End Update Employee ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_covered_individuals @employer_id, @year_id, @employee_id
				PRINT '5*** End Insert Covered Individuals***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_covered_individuals_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '6 *** End Insert Covered Individuals Monthly ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '7 *** End Insert Employee Monthly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_ale_monthly_detail @employer_id, @year_id, @aag_indicator, @_4980H_transition_relief_indicator
				PRINT '8 *** End Insert Ale Monthly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '9 *** End Insert Ale Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE air.appr.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '10 *** End Insert Appr Employee Monthly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employer_id, @year_id, @employee_id
				PRINT '11A *** Grab any potential full time coded employees that did not finish their IMP. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employer_id, @year_id, @employee_id
				PRINT '11B *** Grab any potential full time coded employees that did not finish their IMP. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '12A *** Cleanup the overwritten line 15 entries. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '12B *** Cleanup the overwritten line 15 entries. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employer_id, @year_id, @employee_id
				PRINT '13A *** Updating the insurance information based on change events. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-InsuranceChangeEvents] @employer_id, @year_id, @employee_id
				PRINT '13B *** Updating the insurance information based on change events. (appr) ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '14A *** Updating hours from the MP/IMP and redetermining the monthly status. (emp) ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[appr].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '14B *** Updating hours from the MP/IMP and redetermining the monthly status. (appr) ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '15 *** End Insert Insert Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.appr.spInsert_employee_yearly_detail_init @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '16 *** End Insert Appr Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '17A *** Setting 1G flags now that all of the data is ready. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '17B *** Setting 1G flags now that all of the data is ready. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employer_id, @year_id, @employee_id
				PRINT '18 *** Flagging the forms that should be presented on the 1095 screens. ***'
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
ALTER PROCEDURE [dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP]
	@employerId INT,
	@yearId SMALLINT,
	@employeeId INT = NULL
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
		INNER JOIN [air].[emp].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eioe.EmployeeId)
		INNER JOIN [aca].[dbo].[employee] adee WITH (NOLOCK) ON (adee.employee_id = ee.employee_id)
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
				INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @yearId
		)
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(eioe.EmployeeId BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-SetLine15]
	@employerId INT,
	@yearId SMALLINT,
	@employeeId INT = NULL
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
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

	-- now reset with our values.
	UPDATE [air].[emp].[monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		share_lowest_cost_monthly_premium =
			CASE
				WHEN ic.contribution_id = '$' THEN i.monthlycost - ic.amount
				WHEN ic.contribution_id = '%' THEN i.monthlycost - (i.monthlycost * (ic.amount/100))
				ELSE NULL
			END,
		enrolled = eioe.CoverageInForce,
		insurance_type_id = i.insurance_type_id,
		hra_flex_contribution = eioe.HraFlexContribution
	FROM
		[air].[emp].[monthly_detail] emd
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK) ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[plan_year] py WITH (NOLOCK) ON (py.plan_year_id = eioe.PlanYearId)
		INNER JOIN [aca].[dbo].[insurance] i WITH (NOLOCK) ON (i.insurance_id = eioe.InsuranceId AND i.plan_year_id = py.plan_year_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic WITH (NOLOCK) ON (ic.insurance_id = i.insurance_id AND ic.ins_cont_id = eioe.InsuranceContributionId)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-InsuranceChangeEvents]
	@employerId int,
	@yearId smallint,
	@employeeId INT = NULL
AS
BEGIN

	UPDATE [air].[emp].[monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		enrolled = eioe.CoverageInForce
	FROM
		[air].[emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK) 
			ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		eioe.TaxYearId = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eioe.EmployeeId BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-MonthlyHoursAndStatus]
	@employerId int,
	@yearId smallint,
	@employeeId INT = NULL
AS
BEGIN

	-- emp hours first.
	UPDATE [air].[emp].[monthly_detail]
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.emp.monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) AS MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea WITH (NOLOCK) ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (SELECT ee.employee_id FROM [aca].[dbo].[employee] ee WITH (NOLOCK) WHERE ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours AS MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea WITH (NOLOCK) ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (SELECT ee.employee_id FROM [aca].[dbo].[employee] ee WITH (NOLOCK) WHERE ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf WITH (NOLOCK) ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

	-- now that we have real hours, reset the emp monthly_status_id
	UPDATE [air].[emp].[monthly_detail]
	SET
		monthly_status_id = [air].[etl].[ufnGetDerivedMonthlyStatus](
				emd.employee_id,
				emd.time_frame_id, 
				ee.terminationDate, 
				ee.hireDate,
				ee.initialMeasurmentEnd,
				ee.aca_status_id,
				emd.monthly_hours
			)
	FROM 
		air.emp.monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));
	
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
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic WITH (NOLOCK) ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i WITH (NOLOCK) ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK) ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

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
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec WITH (NOLOCK) ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));


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

	-- start with emp.
	UPDATE [air].[emp].[yearly_detail]
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
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

DROP PROCEDURE [dbo].[spUpdateAIR]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/31/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spETL_ShortBuild]
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
	(tax_year = @year_id);
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
BEGIN TRY
	BEGIN TRAN ETL_BUILD
		BEGIN

			EXECUTE air.etl.spInsert_ale_employer @employer_id, @year_id
				PRINT '1 *** End Insert Employer ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_ale_dge @employer_id, @year_id
				PRINT '1A *** End Insert Dge ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spUpdate_employer @employer_id, @year_id
				PRINT '2 *** End Update Employer ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spUpdate_ale_dge @employer_id, @year_id
				PRINT '2A *** End Update Dge ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_employee @employer_id, @employee_id
				PRINT '3 *** End Insert Employee ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spUpdate_employee @employer_id, @employee_id
				PRINT '4 *** End Update Employee ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_covered_individuals @employer_id, @year_id, @employee_id
				PRINT '5 *** End Insert Covered Individuals ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_covered_individuals_monthly_detail @employer_id, @year_id
				PRINT '6 *** End Insert Covered Individuals Monthly ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '7 *** End Insert Employee Monthly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_ale_monthly_detail @employer_id, @year_id, @aag_indicator, @_4980H_transition_relief_indicator
				PRINT '9 *** End Insert Ale Monthly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '10 *** End Insert Ale Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employer_id, @year_id, @employee_id
				PRINT '11 *** Grab any potential full time coded employees that did not finish their IMP. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '12 *** Cleanup the overwritten line 15 entries. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employer_id, @year_id, @employee_id
				PRINT '13 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '14 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '15 *** End Insert Insert Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '17a *** Setting 1G flags now that all of the data is ready. ***'
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
-- ______________________________________________________________________________________________________________________________________________________

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [appr].[spDeleteCoveredIndividual]
	@rowid INT

AS

	DELETE [emp].[covered_individual_monthly_detail]
	WHERE [covered_individual_id] = @rowId;

	DELETE [emp].[covered_individual]
	WHERE [covered_individual_id] = @rowId;

GO

GRANT EXECUTE ON [appr].[spDeleteCoveredIndividual] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-MonthlyHoursAndStatus]
	@employerId int,
	@yearId smallint,
	@employeeId INT = NULL
AS
BEGIN

	-- emp hours first.
	UPDATE [air].[emp].[monthly_detail]
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.emp.monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) AS MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea WITH (NOLOCK) ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (SELECT ee.employee_id FROM [aca].[dbo].[employee] ee WITH (NOLOCK) WHERE ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours AS MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea WITH (NOLOCK) ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (SELECT ee.employee_id FROM [aca].[dbo].[employee] ee WITH (NOLOCK) WHERE ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf WITH (NOLOCK) ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

	-- now that we have real hours, reset the emp monthly_status_id
	UPDATE [air].[emp].[monthly_detail]
	SET
		monthly_status_id = [air].[etl].[ufnGetDerivedMonthlyStatus](
				emd.employee_id,
				emd.time_frame_id, 
				ee.terminationDate, 
				ee.hireDate,
				ee.initialMeasurmentEnd,
				ee.aca_status_id,
				emd.monthly_hours
			)
	FROM 
		air.emp.monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));
	
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
				0,		-- mainland code has not been certified. gc5
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
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic WITH (NOLOCK) ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i WITH (NOLOCK) ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK) ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

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
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec WITH (NOLOCK) ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [appr].[spUpdateAIR-MonthlyHoursAndStatus]
	@employerId int,
	@yearId smallint,
	@employeeId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	-- appr hours.
	UPDATE [air].[appr].[employee_monthly_detail]
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.appr.employee_monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) AS MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea WITH (NOLOCK) ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (SELECT ee.employee_id FROM [aca].[dbo].[employee] ee WITH (NOLOCK) WHERE ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours AS MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea WITH (NOLOCK) ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (SELECT ee.employee_id FROM [aca].[dbo].[employee] ee WITH (NOLOCK) WHERE ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf WITH (NOLOCK) ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

	-- now that we have real appr hours, reset the monthly_status_id
	UPDATE air.appr.employee_monthly_detail
	SET
		monthly_status_id = [air].[etl].[ufnGetDerivedMonthlyStatus](
				emd.employee_id,
				emd.time_frame_id, 
				ee.terminationDate, 
				ee.hireDate,
				ee.initialMeasurmentEnd,
				ee.aca_status_id,
				emd.monthly_hours
			)
	FROM 
		air.appr.employee_monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
	WHERE
		emd.employer_id =  @employerId
			AND 
		tf.year_id = @yearId
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

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
				0,		-- mainland code has not been certified yet. gc5
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
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic WITH (NOLOCK) ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i WITH (NOLOCK) ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK) ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

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
		INNER JOIN [aca].[dbo].[employee] ee WITH (NOLOCK) ON (ee.employee_id = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec WITH (NOLOCK) ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(emd.employee_id BETWEEN ISNULL(@employeeId, 0) AND ISNULL(@employeeId, 10000000));

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spInsertNewCoveredIndividual]
	@rowid INT
AS

	INSERT INTO [air].[emp].[covered_individual] (
		[covered_individual_id],
		[employee_id],
		[employer_id],
		[first_name],
		[middle_name],
		[last_name],
		[name_suffix],
		[ssn],
		[birth_date],
		[annual_coverage_indicator]
		)
	SELECT
		ice.[row_id] AS [covered_individual_id],
		ice.employee_id,
		ice.employer_id,
		CASE
			WHEN ice.dependent_id IS NULL THEN ee.fName
			ELSE ed.fName
		END AS [first_name],
		NULL AS [middle_name],
		CASE
			WHEN ice.dependent_id IS NULL THEN ee.lName
			ELSE ed.lName
		END AS [last_name],
		NULL AS [name_suffix],
		CASE
			WHEN ice.dependent_id IS NULL THEN ee.ssn
			ELSE ed.ssn
		END AS [ssn],
		CASE
			WHEN ice.dependent_id IS NULL THEN ee.dob
			ELSE ed.dob
		END AS [birth_date],
		0 as [annual_coverage_indicator]
	FROM
		[aca].[dbo].[insurance_coverage_editable] ice
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = ice.employee_id)
		INNER JOIN [aca].[dbo].[employee_dependents] ed ON (ed.employee_id = ice.dependent_id)
	WHERE
		ice.row_id = @rowId;

	INSERT INTO [emp].[covered_individual_monthly_detail] (
			[covered_individual_id],
			[time_frame_id],
			[covered_indicator]
		)
	SELECT
		ice.row_id,
		tf.time_frame_id,
		0 AS [covered_indicator]
	FROM
		[aca].[dbo].[insurance_coverage_editable] ice
		CROSS JOIN [air].[gen].time_frame tf
	WHERE
		ice.row_id = @rowId
			AND
		tf.year_id = ice.tax_year;

GO

GRANT EXECUTE ON [dbo].[spInsertNewCoveredIndividual] TO [air-user] AS DBO
GO
