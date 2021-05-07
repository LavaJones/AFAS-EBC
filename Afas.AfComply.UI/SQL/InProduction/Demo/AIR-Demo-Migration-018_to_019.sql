USE [air-demo]
GO

GRANT EXECUTE ON [etl].[ufnGetTimeFrameID] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [appr].[spInsert_employee_monthly_detail]
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
--None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '11: Delete Appr Employee Monthly Detail'
DELETE [air-demo].appr.employee_monthly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(time_frame_id IN(SELECT time_frame_id FROM [air-demo].gen.time_frame WHERE year_id = @year_id))
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '11: Insert Appr Employee Monthly Detail';

INSERT INTO [air-demo].appr.employee_monthly_detail (
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
		modified_by
	)
SELECT
	emd.employee_id,
	emd.time_frame_id,
	emd.employer_id,
	emd.monthly_hours,
	emd.offer_of_coverage_code,
	emd.mec_offered,
	emd.share_lowest_cost_monthly_premium, 
	emd.safe_harbor_code,
	emd.enrolled,
	emd.monthly_status_id,
	emd.insurance_type_id,
	@user_name
FROM
	[air-demo].emp.monthly_detail emd 
WHERE
	(emd.employer_id = @employer_id)
		AND
	(emd.time_frame_id IN(SELECT time_frame_id FROM [air-demo].gen.time_frame WHERE year_id = @year_id))
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [appr].[spInsert_employee_monthly_detail] TO [air-user] AS DBO
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
ALTER PROCEDURE [appr].[spInsert_employee_yearly_detail]
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
DELETE [air-demo].appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Insert Appr Employee Yearly Detail';
INSERT INTO [air-demo].appr.employee_yearly_detail (
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
	[air-demo].appr.employee_monthly_detail emd 
	INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.offer_of_coverage_code) AS offer_of_coverage_code,
				t.year_id
			FROM
				[air-demo].appr.employee_monthly_detail emd 
				INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
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
				[air-demo].appr.employee_monthly_detail emd 
				INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
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
				[air-demo].appr.employee_monthly_detail emd 
				INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
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
				[air-demo].appr.employee_monthly_detail 
			WHERE
				enrolled = 1
			) enr ON (emd.employee_id = enr.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
			FROM
				[air-demo].appr.employee_monthly_detail
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
				[air-demo].appr.employee_monthly_detail 
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
				[air-demo].appr.employee_monthly_detail
			WHERE
				(insurance_type_id IN (1,2))
					AND
				(enrolled = 0)
					AND 
				(employee_id IN(SELECT DISTINCT employee_id FROM [air-demo].emp.covered_individual))
			) it3 ON (emd.employee_id = it3.employee_id)
WHERE
	(emd.employer_id = @employer_id)
		AND
	(t.year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- old 1G logic is now in its own procedure. gc5


GO

GRANT EXECUTE ON [appr].[spInsert_employee_yearly_detail] TO [air-user] AS DBO
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
ALTER PROCEDURE [appr].[spInsert_employee_yearly_detail_init]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT,
@user_name NVARCHAR(100)=NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None Currently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Delete Appr Employee Yearly Detail';
DELETE [air-demo].appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Insert Appr Employee Yearly Detail';
INSERT INTO [air-demo].appr.employee_yearly_detail (
	employee_id,
	year_id,
	employer_id,
	annual_offer_of_coverage_code,
	annual_share_lowest_cost_monthly_premium,
	annual_safe_harbor_code,
	enrolled,
	_1095C
)
SELECT DISTINCT
	employee_id,
	year_id,
	employer_id,
	annual_offer_of_coverage_code,
	annual_share_lowest_cost_monthly_premium,
	annual_safe_harbor_code,
	enrolled,
	_1095C
FROM
	[air-demo].emp.yearly_detail  eyd 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [appr].[spInsert_employee_yearly_detail_init] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 4/17/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [appr].[spUpdate_1095C_status]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters
@employer_id INT, 
@year_id INT,
@employee_id INT = NULL

-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
UPDATE [air-demo].appr.employee_yearly_detail
SET
	submittal_ready = 0,
	_1095C = 0
FROM
	[air-demo].appr.employee_yearly_detail yd
WHERE
	(yd.employer_id = @employer_id)
		AND
	(yd.year_id = @year_id)
		AND 
	-- note: this will break when there is more than 10 million lives in the database.
	(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

UPDATE [air-demo].appr.employee_yearly_detail
SET
	submittal_ready = 1,
	_1095C = 1
FROM
	[air-demo].appr.employee_yearly_detail yd
WHERE
	yd.employee_id IN (
			SELECT DISTINCT employee_id
			FROM [air-demo].appr.employee_monthly_detail md 
			WHERE
				((monthly_status_id = 1) OR (insurance_type_id = 2 AND enrolled = 1))
					AND
				(md.employee_id = employee_id)
	)
		OR
	yd.employee_id IN (
		SELECT DISTINCT employee_id
		FROM [air-demo].appr.employee_monthly_detail
		WHERE
			(enrolled = 0)
				AND
			(employee_id IN(SELECT DISTINCT employee_id FROM [air-demo].emp.covered_individual))
	)
		AND 
	(yd.employer_id = @employer_id)
		AND
	(yd.year_id = @year_id)
		AND 
	-- note: this will break when there is more than 10 million lives in the database.
	(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

GRANT EXECUTE ON [appr].[spUpdate_1095C_status] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-SetLine15]
	@employerId INT,
	@yearId SMALLINT
AS

	-- clear emp monthly first.
	UPDATE [air-demo].[emp].[monthly_detail]
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
		[air-demo].[emp].[monthly_detail] emd
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now reset with our values.
	UPDATE [air-demo].[emp].[monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		share_lowest_cost_monthly_premium = i.monthlycost - ic.amount,
		enrolled = eioe.CoverageInForce,
		insurance_type_id = i.insurance_type_id,
		hra_flex_contribution = eioe.HraFlexContribution
	FROM
		[air-demo].[emp].[monthly_detail] emd
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[plan_year] py ON (py.plan_year_id = eioe.PlanYearId)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = eioe.InsuranceId AND i.plan_year_id = py.plan_year_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.insurance_id = i.insurance_id AND ic.ins_cont_id = eioe.InsuranceContributionId)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now clear appr monthly.
	UPDATE [air-demo].[appr].[employee_monthly_detail]
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
		[air-demo].[appr].[employee_monthly_detail] emd
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now reset with our values.
	UPDATE [air-demo].[appr].[employee_monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		share_lowest_cost_monthly_premium = i.monthlycost - ic.amount,
		enrolled = eioe.CoverageInForce,
		insurance_type_id = i.insurance_type_id,
		hra_flex_contribution = eioe.HraFlexContribution
	FROM
		[air-demo].[appr].[employee_monthly_detail] emd
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[plan_year] py ON (py.plan_year_id = eioe.PlanYearId)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = eioe.InsuranceId AND i.plan_year_id = py.plan_year_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.insurance_id = i.insurance_id AND ic.ins_cont_id = eioe.InsuranceContributionId)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-SetLine15] TO [air-user] AS DBO
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

	UPDATE [air-demo].appr.employee_yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		[air-demo].appr.employee_yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				[air-demo].appr.employee_monthly_detail emd 
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
				[air-demo].appr.employee_monthly_detail emd 
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

	UPDATE [air-demo].emp.yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		[air-demo].emp.yearly_detail eyd
	WHERE
		-- if they where not full time anytime during the period
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				[air-demo].emp.monthly_detail emd 
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
				[air-demo].emp.monthly_detail emd 
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

GRANT EXECUTE ON [dbo].[spUpdateAIR-Set1GCodes] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [etl].[ufnGetDerivedMonthlyStatus] (
		@employee_id INT,
		@time_frame_id SMALLINT, 
		@termination_date DATE, 
		@hire_date DATE,
		@initial_measurement_period_end_date DATE,
		@aca_status INT,
		@measured_hours DECIMAL(18,2)
	)
RETURNS INT
AS
BEGIN

	-- breaks at time frame 1000
	DECLARE @terminationTimeFrame INT
	SELECT
		@terminationTimeFrame = ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(@termination_date), MONTH(@termination_date)), 1000)

	-- Are we terminated in the past?
	IF  @terminationTimeFrame < @time_frame_id
	
		RETURN 5 -- not an employee
	
	-- Are we terminated this month _and_ have no enrolled coverage?
	IF
		@terminationTimeFrame = @time_frame_id
			AND
		[air-demo].[etl].[ufnDoesTerminationMonthHaveCoverage](@employee_id, @time_frame_id, @termination_date) = 0

		RETURN 4	-- in termination month

	DECLARE @hireDateTimeFrame INT
	SELECT
		@hireDateTimeFrame = [air-demo].etl.ufnGetTimeFrameID(YEAR(@hire_date), MONTH(@hire_date))

	-- Are we hired in the future?
	IF @hireDateTimeFrame > @time_frame_id

		RETURN 5 -- not an employee

	DECLARE @initialMeasurementPeriodTimeFrame INT
	SELECT
		@initialMeasurementPeriodTimeFrame = [air-demo].[etl].[ufnGetTimeFrameID](YEAR(@initial_measurement_period_end_date), MONTH(@initial_measurement_period_end_date))
		
	-- Are we part time and still in our initial measurement period?
	IF
		(
			(@time_frame_id <= @initialMeasurementPeriodTimeFrame)
				AND
			(@aca_status = 2)	-- flagged as part time on the demographics
		)

		RETURN 3	-- limited assesssment period.
		
	-- Are we full time and still not in a full stability period?
	IF
		(
			(@time_frame_id <= @initialMeasurementPeriodTimeFrame)
				AND
			(@aca_status = 5)	-- flagged as full time on the demographics
		)

		RETURN 1	-- assume full time, demographics override

	DECLARE @endOfInitialMeasurementPeriodAdmin INT
	SELECT
		@endOfInitialMeasurementPeriodAdmin = @hireDateTimeFrame + 13	-- longest period is 13 months rounded up to next full month.

	-- Are we in the IMP admin period and we are flagged as part time?
	IF
		(
			(@time_frame_id > @initialMeasurementPeriodTimeFrame)
				AND
			(@time_frame_id <= @endOfInitialMeasurementPeriodAdmin)
				AND
			(@aca_status = 2)	-- flagged as part time on the demographics
		)

		RETURN 6	-- admin period.

	DECLARE @measuredHours DECIMAL(18,2)
	SELECT
		@measuredHours = ISNULL(@measured_hours, 0)

	-- no hours and not hired in this tax reporting period, give up.
	IF @measuredHours = 0

		RETURN 7	-- unknown status.

	IF @measuredHours > 129.99
	
		RETURN 1	-- measures full time.
		
	IF @measuredHours < 130
	
		RETURN 2	-- measures part time 

	RETURN NULL

END

GO

GRANT EXECUTE ON [etl].[ufnGetDerivedMonthlyStatus] TO [air-user] AS DBO
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
	UPDATE [air-demo].emp.monthly_detail
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
					   [aca-demo].dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN [aca-demo].dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
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
		INNER JOIN [air-demo].gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now that we have real hours, reset the emp monthly_status_id
	UPDATE 
		[air-demo].emp.monthly_detail
	SET
		monthly_status_id = [air-demo].[etl].[ufnGetDerivedMonthlyStatus](
				emd.employee_id,
				emd.time_frame_id, 
				ee.terminationDate, 
				ee.hireDate,
				ee.initialMeasurmentEnd,
				ee.aca_status_id,
				emd.monthly_hours
			)
	FROM 
		[air-demo].emp.monthly_detail emd
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId
	
	-- now the emp 1 codes.
	UPDATE [emp].[monthly_detail]
	SET
		offer_of_coverage_code = [air-demo].etl.ufnGetMecCode(
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
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
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
		LEFT OUTER JOIN [aca-demo].dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

	-- now appr hours.
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
					   [aca-demo].dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN [aca-demo].dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
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
		INNER JOIN [air-demo].gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now that we have real appr hours, reset the monthly_status_id
	UPDATE 
		[air-demo].appr.employee_monthly_detail
	SET
		monthly_status_id = [air-demo].[etl].[ufnGetDerivedMonthlyStatus](
				emd.employee_id,
				emd.time_frame_id, 
				ee.terminationDate, 
				ee.hireDate,
				ee.initialMeasurmentEnd,
				ee.aca_status_id,
				emd.monthly_hours
			)
	FROM 
		[air-demo].appr.employee_monthly_detail emd
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now reset the appr 1 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		offer_of_coverage_code = [air-demo].etl.ufnGetMecCode(
				emd.employee_id,
				emd.time_frame_id,
				emd.mec_offered,
				i.offSpouse,
				i.offDependent,
				i.minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				ee.terminationDate,
				ee.aca_status_id,
				i.SpouseConditional
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
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
		LEFT OUTER JOIN [aca-demo].dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

END

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-MonthlyHoursAndStatus] TO [air-user] AS DBO
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
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe
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
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe
			ON (eioe.EmployeeId = emd.employee_id AND eioe.TimeFrameId = emd.time_frame_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		eioe.TaxYearId = @yearId;

	SET NOCOUNT OFF;
	
END

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-InsuranceChangeEvents] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP]
	@employerId INT,
	@yearId SMALLINT
AS

	-- insert these folks into emp first.
	-- We let the Line15, InsuranceChangeEvents and MonthlyHoursAndStatus clean things up.
	INSERT INTO [air-demo].[emp].[monthly_detail] (
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
		INNER JOIN [air-demo].[emp].[employee] ee ON (ee.employee_id = eioe.EmployeeId)
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
				[air-demo].[emp].[monthly_detail] emd
				INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @yearId
		)

	-- now into appr.
	INSERT INTO [air-demo].[appr].[employee_monthly_detail] (
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
		INNER JOIN [air-demo].[emp].[employee] ee ON (ee.employee_id = eioe.EmployeeId)
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
				[air-demo].[appr].[employee_monthly_detail] emd
				INNER JOIN [air-demo].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @yearId
		)


GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] TO [air-user] AS DBO
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

			EXECUTE [air-demo].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employerId, @yearId
				PRINT '1 *** Grab any potential full time coded employees that did not finish their IMP. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air-demo].[dbo].[spUpdateAIR-SetLine15] @employerId, @yearId
				PRINT '2 *** Cleanup the overwritten line 15 entries. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air-demo].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
				PRINT '3 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air-demo].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '4 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE [air-demo].etl.spInsert_employee_yearly_detail @employerId, @yearId, @employeeId
				PRINT '5 *** End Insert Insert Employee Yearly Detail - redux ***'
				PRINT ''
				PRINT ''

			EXECUTE [air-demo].appr.spInsert_employee_yearly_detail_init @employerId, @yearId, @employeeId, 'IRSTransmissionETL'
				PRINT '6 *** End Insert Appr Employee Yearly Detail - redux ***'
				PRINT ''
				PRINT ''

			EXECUTE [air-demo].dbo.[spUpdateAir-Set1GCodes] @employerId, @yearId
				PRINT '7 *** Setting 1G flags now that all of the data is ready. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air-demo].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '8 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

END

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 9/11/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spUpdate_employer]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @strip_characters NVARCHAR(100) = '&|*|#|-|;|:|,|''|.|=|(|)'
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '2: Update Employer';
UPDATE [air-demo].ale.employer 
		SET	ein = [air-demo].etl.ufnStripCharactersFromString(er.ein,@strip_characters,1,0),
			name = UPPER([air-demo].etl.ufnStripCharactersFromString(er.name,@strip_characters, 1,0)),
			[address] = UPPER([air-demo].etl.ufnStripCharactersFromString(er.[address],@strip_characters, 1,0)),
			city = UPPER([air-demo].etl.ufnStripCharactersFromString(er.city,@strip_characters, 1,0)),
			state_code = UPPER(s.abbreviation),
			zipcode = SUBSTRING(er.zip,1,5),
			contact_first_name = UPPER([air-demo].etl.ufnStripCharactersFromString(u.fname,@strip_characters, 1,0)), 
			contact_last_name= UPPER([air-demo].etl.ufnStripCharactersFromString(u.lname,@strip_characters, 1,0)), 
			contact_telephone= UPPER([air-demo].etl.ufnStripCharactersFromString(u.phone,@strip_characters, 1,1)),
			dge_ein = [air-demo].etl.ufnStripCharactersFromString(tya.dge_ein,@strip_characters, 1,0)
FROM
	[aca-demo].dbo.employer er
	INNER JOIN [aca-demo].dbo.tax_year_approval tya ON (er.employer_id = tya.employer_id)
	INNER JOIN [aca-demo].dbo.[state] s ON (er.state_id = s.state_id)
	INNER JOIN ale.employer er1  ON (er.employer_id = er1.employer_id)
	INNER JOIN  [aca-demo].dbo.[user] u ON (er.employer_id = u.employer_id) 
WHERE
	(er.employer_id = @employer_id)
		AND
	(u.irsContact = 1) 
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [etl].[spUpdate_employer] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 9/11/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spUpdate_employee]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@employee_id INT = NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @strip_characters NVARCHAR(100) = '&|*|#|-|;|:|,|''|.|=|(|)'
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '4: Update Employee';
UPDATE [air-demo].emp.employee  
		SET employer_id = ee.employer_id, 
			first_name = UPPER([air-demo].etl.ufnStripCharactersFromString(ee.fName,@strip_characters,1,0)), 
			last_name = UPPER([air-demo].etl.ufnStripCharactersFromString(ee.lName,@strip_characters,1,0)), 
			[address] = UPPER([air-demo].etl.ufnStripCharactersFromString(ee.[address],@strip_characters,1,0)), 
			city = UPPER([air-demo].etl.ufnStripCharactersFromString(ee.city,@strip_characters,1,0)), 
			state_code = UPPER(s.abbreviation), 
			zipcode = UPPER([air-demo].etl.ufnStripCharactersFromString(SUBSTRING(ee.zip,1,5),@strip_characters,1,0)), 
			ssn = ee.ssn,
			telephone = UPPER([air-demo].etl.ufnStripCharactersFromString(contact_telephone,@strip_characters,1,1))
FROM
	[aca-demo].dbo.employee ee
	INNER JOIN [air-demo].ale.employer er ON (ee.employer_id = er.employer_id)
	INNER JOIN [air-demo].emp.employee em ON (ee.employer_id = em.employer_id) AND (ee.employee_id = em.employee_id)
	INNER JOIN [aca-demo].dbo.state s ON (ee.state_id = s.state_id)
WHERE
	(ee.employer_id = @employer_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [etl].[spUpdate_employee] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/17/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_covered_individuals]
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
DECLARE @characters_to_strip NVARCHAR(100) = '&|*|#|-|;|:|,|.|''|=|(|)'
-- ______________________________________________________________________________________________________________________________________________________
PRINT '5: Delete Covered Individuals'
DELETE [air-demo].emp.covered_individual 
FROM
	[air-demo].emp.covered_individual ci 
	INNER JOIN [air-demo].emp.employee ee ON (ci.employee_id = ee.employee_id)
WHERE
	(ee.employer_id = @employer_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '5: Insert Covered Individuals'
INSERT INTO [air-demo].emp.covered_individual 
		(covered_individual_id, employee_id, employer_id, first_name, middle_name, last_name, ssn, birth_date)
SELECT	DISTINCT first_row_id, employee_id, employer_id,
			UPPER([air-demo].etl.ufnStripCharactersFromString(fName,@characters_to_strip,1,0)), 
			UPPER([air-demo].etl.ufnStripCharactersFromString(mName,@characters_to_strip,1,0)), 
			UPPER([air-demo].etl.ufnStripCharactersFromString(lName,@characters_to_strip,1,0)), 
			ssn, IIF(ssn IS NULL, dob, NULL)
FROM
	[aca-demo].dbo.ufnGetEmployeeInsurance(@year_id) ei
WHERE
	(employer_id = @employer_id)
		AND
	(ISNULL(ssn,dob) IS NOT NULL)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000))
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [etl].[spInsert_covered_individuals] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/17/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_covered_individuals_monthly_detail]
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
PRINT '6: Delete Covered Individual Monthly Detail'
DELETE [air-demo].emp.covered_individual_monthly_detail 
FROM
	[air-demo].emp.covered_individual_monthly_detail cim
	INNER JOIN [air-demo].emp.covered_individual ci ON (cim.covered_individual_id = ci.covered_individual_id) 
	INNER JOIN [air-demo].emp.employee ee ON (ci.employee_id = ee.employee_id)
WHERE
	(time_frame_id IN(SELECT time_frame_id FROM [air-demo].gen.time_frame WHERE year_id = @year_id))
		AND
	(ee.employer_id = @employer_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '6: Insert Covered Individual Monthly Detail';
WITH
	insurance_coverage AS
	(
		SELECT
			first_row_id,
			employee_id,
			dependent_id,
			tax_year,
			[month],
			covered_indicator
		FROM (
				SELECT
					ic.first_row_id,
					ic.employee_id,
					dependent_id,
					tax_year,
					jan,
					feb,
					mar,
					apr,
					may,
					jun,
					jul,
					aug,
					sept,
					oct,
					nov,
					[dec]
				FROM
					[aca-demo].dbo.ufnGetEmployeeInsurance(@year_id) ic
					INNER JOIN [aca-demo].dbo.employee ee ON (ic.employee_id = ee.employee_id)
				WHERE
					(ee.employer_id = @employer_id)
						AND
					-- note: this will break when there is more than 10 million lives in the database.
					(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000))
			) imc
		UNPIVOT (
			covered_indicator FOR [month] IN 
				(jan, feb, mar, apr, may, jun, jul, aug, sept, oct, nov, [dec])
		) AS unpvt
	)

INSERT INTO [air-demo].emp.covered_individual_monthly_detail (
		covered_individual_id,
		time_frame_id,
		covered_indicator
	) 
SELECT
	first_row_id,
	time_frame_id,
	covered_indicator
FROM
	insurance_coverage ic
	INNER JOIN [air-demo].gen.[month] m ON (SUBSTRING(ic.[month],1,3) = SUBSTRING(m.name,1,3))
	INNER JOIN [air-demo].gen.time_frame t ON (ic.tax_year = t.year_id) AND (m.month_id = t.month_id)
WHERE
	first_row_id IN (
			SELECT DISTINCT covered_individual_id FROM [air-demo].emp.covered_individual
		);

-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

PRINT '6: Update Covered Individual Annual';
UPDATE emp.covered_individual
SET
	annual_coverage_indicator = cim.annual_coverage_indicator 
FROM
	emp.covered_individual ci
	INNER JOIN  [air-demo].emp.employee ee ON (ci.employee_id = ee.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				cim.covered_individual_id,
				MAX(CAST(cim.covered_indicator AS INT)) AS annual_coverage_indicator
			FROM
				[air-demo].emp.covered_individual_monthly_detail cim
			GROUP BY
				cim.covered_individual_id
			HAVING
				(COUNT(cim.covered_indicator) = 12)
					AND
				(COUNT(DISTINCT cim.covered_indicator) = 1)
		) cim ON (ci.covered_individual_id = cim.covered_individual_id)
WHERE
	(cim.annual_coverage_indicator = 1)
		AND
	(ee.employer_id = @employer_id) 
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ci.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [etl].[spInsert_covered_individuals_monthly_detail] TO [air-user] AS DBO
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
DELETE [air-demo].emp.monthly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(time_frame_id IN(SELECT time_frame_id FROM [air-demo].gen.time_frame WHERE year_id = @year_id))
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
			[aca-demo].dbo.payroll p
			CROSS JOIN [air-demo].gen.time_frame  t
			INNER JOIN [aca-demo].dbo.employee ee ON (p.employee_id = ee.employee_id)
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
			[aca-demo].dbo.employee ee
			INNER JOIN [air-demo].emp.covered_individual ci ON (ee.employee_id = ci.employee_id)
			CROSS JOIN [air-demo].gen.time_frame  t
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
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ey.terminationDate), MONTH(ey.terminationDate)),0) = ey.time_frame_id THEN 4
				--: Is employee Terminated and not in Termination Month?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ey.terminationDate), MONTH(ey.terminationDate)),1000) < ey.time_frame_id THEN 5 
				--: Is employee Not Yet Hired?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ey.hireDate), MONTH(ey.hireDate)),0) > ey.time_frame_id THEN 5 
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL([air-demo].etl.ufnGetTimeFrameID(YEAR(ey.initialMeasurmentEnd), MONTH(ey.initialMeasurmentEnd)), 0) >= ey.time_frame_id THEN 3 
				--: Is employee In Administrative Period?
				WHEN ISNULL(
					[air-demo].etl.ufnGetTimeFrameID(
						IIF(
								YEAR(ey.hireDate) = @year_id,
								YEAR(ey.hireDate),
								@year_id + 1
							), 
						MONTH(ey.hireDate)
					),
					0) + 3 BETWEEN [air-demo].etl.ufnGetTimeFrameID(YEAR(@year_id), MONTH(ey.hireDate)) AND ey.time_frame_id THEN 6
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
			LEFT OUTER JOIN [air-demo].etl.ufnEmployeeMonthlyHours(@employer_id, @year_id) mh ON (ey.employee_id = mh.employee_id) AND (ey.time_frame_id = mh.time_frame_id)
			LEFT OUTER JOIN [air-demo].etl.ufnEmployeeMonthlyInsurance(@employer_id, @year_id) mi ON (ey.employee_id = mi.employee_id) AND ((ey.time_frame_id = mi.time_frame_id))
			LEFT OUTER JOIN [aca-demo].dbo.employee_classification	ec ON (ey.classification_id = ec.classification_id)
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

INSERT INTO [air-demo].emp.monthly_detail (
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
	[air-demo].etl.ufnGetMecCode(employee_id,time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id, SpouseConditional) AS offer_of_coverage_code,
	minValue, 
	IIF(
				[air-demo].etl.ufnGetMonthFromTimeFrame(time_frame_id) >= MONTH(terminationDate),
				NULL, 
				share_lowest_cost_monthly_premium
			), 
	[air-demo].etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code,
	enrolled,
	monthly_status_id,
	insurance_type_id,
	monthly_flex
FROM
	employees_monthly_aggregates

GO

GRANT EXECUTE ON [etl].[spInsert_employee_monthly_detail] TO [air-user] AS DBO
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
DELETE [air-demo].emp.yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Insert Appr Employee Yearly Detail';
INSERT INTO [air-demo].emp.yearly_detail (
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
	[air-demo].emp.monthly_detail emd 
	INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.offer_of_coverage_code) AS offer_of_coverage_code,
				t.year_id
			FROM
				[air-demo].emp.monthly_detail emd 
				INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
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
				[air-demo].emp.monthly_detail emd 
				INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
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
				[air-demo].emp.monthly_detail emd 
				INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
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
				[air-demo].emp.monthly_detail 
			WHERE
				enrolled = 1
		) enr ON (emd.employee_id = enr.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
			FROM
				[air-demo].emp.monthly_detail
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
				[air-demo].emp.monthly_detail 
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
					[air-demo].emp.monthly_detail
				WHERE
					(insurance_type_id IN (1,2))
						AND
					(enrolled = 0)
						AND 
					(employee_id IN(SELECT DISTINCT employee_id FROM [air-demo].emp.covered_individual))
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
EXECUTE [air-demo].etl.spUpdate_1095C_status @employer_id, @year_id, @employee_id

GO

GRANT EXECUTE ON [etl].[spInsert_employee_yearly_detail] TO [air-user] AS DBO
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
ALTER PROCEDURE [etl].[spInsert_ale_monthly_detail]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@aag_indicator BIT = 0,
@_4980H_transition_relief_indicator BIT = 1
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '9: Delete Appr Ale Monthly Detail'
DELETE
	[air-demo].ale.monthly_detail
WHERE
	(employer_id = @employer_id)
		AND
	(time_frame_id IN(SELECT time_frame_id FROM [air-demo].gen.time_frame WHERE year_id = @year_id));

-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '9: Insert Appr Ale Monthly Detail'
INSERT INTO
	[air-demo].ale.monthly_detail (
			employer_id,
			time_frame_id,
			total_employee_count,
			full_time_employee_count_with_equivalents,
			full_time_employee_count, 
			other_employee_count,
			full_time_equivalents_count,
			min_offered_count_without_equivalents,
			min_offered_percent_without_equivalents,
			mec_offered_without_equivalents,
			min_offered_count_with_equivalents,
			min_offered_percent_with_equivalents,
			mec_offered_with_equivalents,
			aag_indicator,
			_4980H_transition_relief_indicator, 
			_4980H_transition_relief_code,
			self_insured, 
			min_offered_count_to_ft_equivalents,
			allocable_reduction,
			fte_with_equivalents_after_reduction, 
			min_offered_percent_after_reduction
		)
SELECT
	emd.employer_id,
	emd.time_frame_id,  
	COUNT(DISTINCT tec.employee_id) AS total_employee_count,
	IIF(
			SUM(pte.monthly_hours) > 0,
			CAST(COUNT(DISTINCT fte.employee_id) + SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)),
			0
		) AS full_time_employee_count_with_equivalents,
	COUNT(DISTINCT fte.employee_id) AS full_time_employee_count,
	COUNT(DISTINCT oec.employee_id) AS other_employee_count,
	IIF(
			SUM(pte.monthly_hours) > 0,
			CAST(SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)),
			0
		) AS full_time_equivalents_count, 
	CAST(COUNT(DISTINCT ft_cov.employee_id) AS DECIMAL(9,2)) AS min_offered_count_without_equivalents,
	IIF(
			SUM(fte.monthly_hours) > 0 AND COUNT(DISTINCT ft_cov.employee_id) > 0,
			CAST(CAST(COUNT(DISTINCT ft_cov.employee_id) AS DECIMAL(9,2)) / CAST(COUNT(DISTINCT fte.employee_id)AS DECIMAL(9,2)) AS DECIMAL(9,2)) * 100,
			0
		) AS min_offered_percent_without_equivalents, 
	IIF(
			SUM(pte.monthly_hours) > 0 AND COUNT(DISTINCT pt_cov.employee_id) > 0,
			IIF(
					CAST(CAST(COUNT(DISTINCT ft_cov.employee_id) AS DECIMAL(9,2)) / CAST(COUNT(DISTINCT fte.employee_id)AS DECIMAL(9,2)) AS DECIMAL(9,2)) >= .7,
					1,
					0
				),
			0
			) AS mec_offered_without_equivalents,
	CAST(COUNT(DISTINCT ft_cov.employee_id) + COUNT(DISTINCT pt_cov.employee_id) AS DECIMAL(9,2)) AS min_offered_count_with_equivalents,
	IIF(
				SUM(pte.monthly_hours) > 0 AND COUNT(DISTINCT pt_cov.employee_id) > 0,
				CAST((COUNT(DISTINCT ft_cov.employee_id) +  COUNT(DISTINCT pt_cov.employee_id)) / (COUNT(DISTINCT fte.employee_id) + (SUM(pte.monthly_hours)/130)) AS DECIMAL(18,2)) * 100,
				0
			) AS min_percent_offered_with_equivalents,
	IIF(
				SUM(pte.monthly_hours) > 0 AND COUNT(DISTINCT pt_cov.employee_id) > 0,
				IIF(CAST((COUNT(DISTINCT ft_cov.employee_id) + COUNT(DISTINCT pt_cov.employee_id)) / (COUNT(DISTINCT fte.employee_id) + (SUM(pte.monthly_hours)/130)) AS DECIMAL(18,2)) >= .7, 1, 0),
				0
			) AS mec_offered_with_equivalents,
	MAX(CAST(@aag_indicator AS TINYINT)) AS aag_indicator,
	MAX(CAST(@_4980H_transition_relief_indicator AS TINYINT)) AS _4980H_transition_relief_indicator,
	IIF(
				SUM(pte.monthly_hours) > 0,
				IIF(
						CAST(COUNT(DISTINCT fte.employee_id) + SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)) -
							IIF(
									CAST(COUNT(DISTINCT fte.employee_id) + SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)) > 100,
									80,
									30
								) > 99.99,
						'2',
						'1'
					),
				0
			) AS _4980H_transition_relief_code,
	IIF(
				MAX(tec.insurance_type_id) > 1,
				1,
				0
			) AS self_insured,
	COUNT(DISTINCT pt_cov.employee_id) AS min_offered_count_to_ft_equivalents,
	IIF(
				SUM(pte.monthly_hours) > 0,
				IIF(
							CAST(COUNT(DISTINCT fte.employee_id) + SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)) > 100, 
							80,
							30
						),
				0
			) AS allocable_reduction,
	IIF(
				SUM(pte.monthly_hours) > 0,
				CAST(COUNT(DISTINCT fte.employee_id) + SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)) - 
					IIF(
								CAST(COUNT(DISTINCT fte.employee_id) + SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)) > 100,
								80,
								30
							),
				0
			) AS ftec_after_reduction,
			
	IIF(
				SUM(pte.monthly_hours) > 0 AND COUNT(DISTINCT pt_cov.employee_id) > 0,
				CAST(CAST(COUNT(DISTINCT ft_cov.employee_id) + COUNT(DISTINCT pt_cov.employee_id) AS DECIMAL(9,2)) / (CAST(COUNT(DISTINCT fte.employee_id) + SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)) - 
					IIF(
								CAST(COUNT(DISTINCT fte.employee_id) + SUM(pte.monthly_hours)/130 AS DECIMAL(9,2)) > 100,
								80,
								30
							)
					) AS DECIMAL(9,2)) * 100,
				0
			) AS min_offered_percent_after_reduction
FROM
	[air-demo].emp.monthly_detail emd
	INNER JOIN [air-demo].gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN [air-demo].emp.monthly_detail tec
		ON
			(emd.employee_id = tec.employee_id)
				AND
			(emd.time_frame_id = tec.time_frame_id)
				AND
			tec.monthly_status_id IN (1,2,3,4,6)
	LEFT OUTER JOIN [air-demo].emp.monthly_detail fte
		ON
			(emd.employee_id = fte.employee_id)
				AND
			(emd.time_frame_id = fte.time_frame_id)
				AND
			fte.monthly_status_id = 1
	LEFT OUTER JOIN [air-demo].emp.monthly_detail oec
		ON
			(emd.employee_id = oec.employee_id)
				AND
			(emd.time_frame_id = oec.time_frame_id)
				AND
			oec.monthly_status_id IN (2,3,4,6)
	LEFT OUTER JOIN [air-demo].emp.monthly_detail pte
		ON
			(emd.employee_id = pte.employee_id)
				AND
			(emd.time_frame_id = pte.time_frame_id)
				AND
			pte.monthly_status_id = 2
	LEFT OUTER JOIN [air-demo].emp.monthly_detail ft_cov
		ON
			(t.time_frame_id = ft_cov.time_frame_id)
				AND
			(emd.employee_id = ft_cov.employee_id)
				AND
			ft_cov.mec_offered = 1
				AND
			ft_cov.monthly_status_id = 1
	LEFT OUTER JOIN [air-demo].emp.monthly_detail pt_cov
		ON
			(t.time_frame_id = pt_cov.time_frame_id)
				AND
			(emd.employee_id = pt_cov.employee_id)
				AND
			pt_cov.mec_offered = 1
				AND
			pt_cov.monthly_status_id = 2
WHERE
	(emd.employer_id = @employer_id)
		AND
	(t.year_id = @year_id)
GROUP BY
	emd.employer_id,
	emd.time_frame_id
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [etl].[spInsert_ale_monthly_detail] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/22/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_ale_yearly_detail]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@aag_code TINYINT = 2,
@_4980H_transition_relief_indicator BIT = 1
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '10: Delete Ale Yearly Detail'
DELETE
	[air-demo].ale.yearly_detail
WHERE
	(employer_id = @employer_id)
			AND
	(year_id = @year_id);

-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '10: Insert Ale Yearly Detail'
INSERT INTO [air-demo].ale.yearly_detail (
		employer_id,
		year_id,
		_1095C_count,
		total_employee_count_through_year,
		annual_fulltime_employee_count,
		annual_total_employee_count, 
		aag_code,
		annual_aag_indicator,
		_4980H_transition_relief_indicator,
		_4980H_transition_relief_code, 
		annual_mec_offer_indicator,
		self_insured
	)
	SELECT DISTINCT
		amd.employer_id,
		t.year_id, 
		(SELECT COUNT(DISTINCT employee_id) FROM [air-demo].appr.employee_yearly_detail YD WHERE (_1095C = 1) AND (employer_id = @employer_id)),
		(SELECT COUNT(DISTINCT employee_id) FROM [air-demo].emp.yearly_detail WHERE (employer_id = @employer_id)),
		eefte.fulltime_employee_count, eetec.total_employee_count, 
		IIF(
					amd.aag_indicator = 1,
					1,
					2
				) AS aag_code,
		mdaag.annual_aag_indicator,
		@_4980H_transition_relief_indicator AS _4980H_transition_relief_indicator,
		IIF(
					ee4980H._4980H_transition_relief_code IS NOT NULL, 
					IIF(
								ee4980H._4980H_transition_relief_code = 1,
								'A',
								'B'
							), 
					NULL
				),
		eemec.mec_offered_with_equivalents AS mec_offer_indicator, 
		IIF(
					eese.self_insured IS NULL,
					0,
					1
				) AS self_insured
FROM
	[air-demo].ale.monthly_detail amd WITH (NOLOCK)
	INNER JOIN [air-demo].gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employer_id ,
				MAX(total_employee_count) AS total_employee_count,
				t.year_id
			FROM
				[air-demo].ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN [air-demo].gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
			WHERE
				(employer_id = @employer_id)
					AND
				(t.year_id = @year_id)
			GROUP BY
				employer_id,
				t.year_id
			HAVING
				(COUNT(amd.total_employee_count) = 12) 
					AND
				(COUNT(DISTINCT amd.total_employee_count) = 1)
		) eetec ON (amd.employer_id = eetec.employer_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employer_id ,
				MAX(full_time_employee_count_with_equivalents) AS fulltime_employee_count,
				t.year_id
			FROM
				[air-demo].ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN [air-demo].gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
			WHERE
				(employer_id = @employer_id)
					AND
				(t.year_id = @year_id)
			GROUP BY
				employer_id,
				t.year_id
			HAVING
				(COUNT(amd.full_time_employee_count_with_equivalents) = 12) 
					AND
				(COUNT(DISTINCT amd.full_time_employee_count_with_equivalents) = 1)
		) eefte ON (amd.employer_id = eefte.employer_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employer_id,
				MAX(CAST(aag_indicator AS TINYINT)) AS annual_aag_indicator,
				t.year_id
			FROM
				[air-demo].ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN [air-demo].gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
			WHERE
				(employer_id = @employer_id)
					AND
				(t.year_id = @year_id)
			GROUP BY
				employer_id,
				t.year_id
			HAVING
				(COUNT(amd.aag_indicator) = 12) 
					AND
				(COUNT(DISTINCT amd.aag_indicator) = 1)
		) mdaag ON (amd.employer_id = mdaag.employer_id) AND mdaag.annual_aag_indicator = 1
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employer_id,
				MAX(CAST(mec_offered_with_equivalents AS INT)) AS mec_offered_with_equivalents,
				t.year_id
			FROM
				[air-demo].ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN [air-demo].gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
			WHERE
				(employer_id = @employer_id)
					AND
				(t.year_id = @year_id)
			GROUP BY
				employer_id,
				t.year_id
			HAVING
				(COUNT(amd.mec_offered_with_equivalents) = 12) 
					AND
				(COUNT(DISTINCT amd.mec_offered_with_equivalents) = 1)
		) eemec ON (amd.employer_id = eemec.employer_id) AND eemec.mec_offered_with_equivalents = 1
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employer_id,
				MAX(_4980H_transition_relief_code) AS _4980H_transition_relief_code
			FROM
				[air-demo].ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN [air-demo].gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
			WHERE
				(employer_id = @employer_id)
					AND
				(t.year_id = @year_id)
			GROUP BY
				employer_id,
				t.year_id
			HAVING
				(COUNT(amd._4980H_transition_relief_code) = 12) 
					AND
				(COUNT(DISTINCT amd._4980H_transition_relief_code) = 1)
		) ee4980H ON (amd.employer_id = ee4980H.employer_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employer_id,
				MAX(CAST(self_insured AS INT)) AS self_insured
			FROM
				[air-demo].ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN [air-demo].gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
			WHERE
				(employer_id = @employer_id)
					AND
				(t.year_id = @year_id)
			GROUP BY
				employer_id,
				t.year_id
			HAVING
				(COUNT(amd.self_insured) = 12) 
					AND
				(COUNT(DISTINCT amd.self_insured) = 1)
		) eese ON (amd.employer_id = eese.employer_id) AND eese.self_insured = 1
WHERE
	(amd.employer_id = @employer_id)
		AND
	(t.year_id = @year_id)
GROUP BY
	t.year_id,
	amd.employer_id,
	amd._4980H_transition_relief_indicator,
	eefte.fulltime_employee_count,
	eetec.total_employee_count, 
	mdaag.annual_aag_indicator,
	amd.aag_indicator,
	eemec.mec_offered_with_equivalents,
	ee4980H._4980H_transition_relief_code,
	eese.self_insured;
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [etl].[spInsert_ale_yearly_detail] TO [air-user] AS DBO
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
	[aca-demo].dbo.tax_year_approval 
WHERE
	(employer_id = @employer_id)
		AND
	(tax_year = @year_id);
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
BEGIN TRY
	BEGIN TRAN ETL_BUILD
		BEGIN
			EXECUTE [air-demo].etl.spInsert_ale_employer @employer_id, @year_id
				PRINT '1*** End Insert Employer***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_ale_dge @employer_id, @year_id
				PRINT '1A*** End Insert Dge***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spUpdate_employer @employer_id, @year_id
				PRINT '2*** End Update Employer***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spUpdate_ale_dge @employer_id, @year_id
				PRINT '2A*** End Update Dge***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_employee @employer_id, @employee_id
				PRINT '3*** End Insert Employee***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spUpdate_employee @employer_id, @employee_id
				PRINT '4*** End Update Employee***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_covered_individuals @employer_id, @year_id, @employee_id
				PRINT '5*** End Insert Covered Individuals***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_covered_individuals_monthly_detail @employer_id, @year_id
				PRINT '6*** End Insert Covered Individuals Monthly***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '7*** End Insert Employee Monthly Detail***'
				PRINT ''
				PRINT ''
				EXECUTE [air-demo].etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '8*** End Insert Insert Employee Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_ale_monthly_detail @employer_id, @year_id, @aag_indicator, @_4980H_transition_relief_indicator
				PRINT '9*** End Insert Ale Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '10*** End Insert Ale Yearly Detail***'
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

GRANT EXECUTE ON [etl].[spETL_ShortBuild] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 04/07/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_ale_dge]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @strip_characters NVARCHAR(100) = '&|*|#|-|;|:|,|.|''|=|(|)'
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '1A: Insert Dge';
INSERT INTO	[air-demo].ale.dge	(
	name,
	ein,
	[address],
	city,
	state_code,
	zipcode,
	contact_first_name,
	contact_last_name,
	contact_telephone
) 
SELECT DISTINCT
	UPPER([air-demo].etl.ufnStripCharactersFromString(dge_name,@strip_characters,1,0)) AS name,
	[air-demo].etl.ufnStripCharactersFromString(dge_ein,@strip_characters,1,0), 
	UPPER([air-demo].etl.ufnStripCharactersFromString(dge_address, @strip_characters,1,0)),
	UPPER([air-demo].etl.ufnStripCharactersFromString(dge_city, @strip_characters,1,0)),
	s.abbreviation, 
	SUBSTRING(dge_zip,1,5), 
	UPPER([air-demo].etl.ufnStripCharactersFromString(dge_contact_fname, @strip_characters,1,0)), 
	UPPER([air-demo].etl.ufnStripCharactersFromString(dge_contact_lname, @strip_characters,1,0)), 
	UPPER([air-demo].etl.ufnStripCharactersFromString(dge_phone,@strip_characters,1,1))
FROM
	[aca-demo].dbo.tax_year_approval tya
	INNER JOIN [aca-demo].dbo.[state] s ON (tya.state_id = s.state_id)
	LEFT OUTER JOIN [air-demo].ale.dge d ON (REPLACE(tya.dge_ein,'-','') = d.ein) 
WHERE
	(tya.employer_id = @employer_id)
		AND
	(d.ein IS NULL)
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [etl].[spETL_ShortBuild] TO [air-user] AS DBO
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
	[aca-demo].dbo.tax_year_approval 
WHERE
	(employer_id = @employer_id)
		AND
	(tax_year = @year_id)
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
BEGIN TRY
	BEGIN TRAN ETL_BUILD
		BEGIN
			EXECUTE [air-demo].etl.spInsert_ale_employer @employer_id, @year_id
				PRINT '1*** End Insert Employer***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_ale_dge @employer_id, @year_id
				PRINT '1A*** End Insert Dge***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spUpdate_employer @employer_id, @year_id
				PRINT '2*** End Update Employer***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spUpdate_ale_dge @employer_id, @year_id
				PRINT '2A*** End Update Employer***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_employee @employer_id, @employee_id
				PRINT '3*** End Insert Employee***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spUpdate_employee @employer_id, @employee_id
				PRINT '4*** End Update Employee***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_covered_individuals @employer_id, @year_id, @employee_id
				PRINT '5*** End Insert Covered Individuals***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_covered_individuals_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '6*** End Insert Covered Individuals Monthly***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '7*** End Insert Employee Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '8*** End Insert Insert Employee Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_ale_monthly_detail @employer_id, @year_id, @aag_indicator, @_4980H_transition_relief_indicator
				PRINT '9*** End Insert Ale Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '10*** End Insert Ale Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].appr.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '11*** End Insert Appr Employee Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].appr.spInsert_employee_yearly_detail_init @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '12*** End Insert Appr Employee Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE [air-demo].dbo.spUpdateAIR @employer_id, @year_id, @employee_id
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

GRANT EXECUTE ON [etl].[spETL_Build] TO [air-user] AS DBO
GO
