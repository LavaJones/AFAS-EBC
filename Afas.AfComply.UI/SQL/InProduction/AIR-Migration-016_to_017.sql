USE [air]
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
		insurance_type_id = i.insurance_type_id,
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
		insurance_type_id = i.insurance_type_id,
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

-- 2017 and 2018 values
ALTER TABLE [gen].[time_frame] DROP CONSTRAINT [CK_time_frame_year]
GO

ALTER TABLE [gen].[time_frame]  WITH CHECK ADD CONSTRAINT [CK_time_frame_year] CHECK (([year_id]>=(1920) AND [year_id]<=(2020)))
GO

INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (37, 2017, 1)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (38, 2017, 2)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (39, 2017, 3)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (40, 2017, 4)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (41, 2017, 5)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (42, 2017, 6)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (43, 2017, 7)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (44, 2017, 8)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (45, 2017, 9)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (46, 2017, 10)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (47, 2017, 11)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (48, 2017, 12)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (49, 2018, 1)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (50, 2018, 2)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (51, 2018, 3)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (52, 2018, 4)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (53, 2018, 5)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (54, 2018, 6)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (55, 2018, 7)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (56, 2018, 8)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (57, 2018, 9)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (58, 2018, 10)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (59, 2018, 11)
GO
INSERT [gen].[time_frame] ([time_frame_id], [year_id], [month_id]) VALUES (60, 2018, 12)
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [etl].[ufnGetDerivedMonthlyStatus] (
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
		@terminationTimeFrame = ISNULL(air.etl.ufnGetTimeFrameID(YEAR(@termination_date), MONTH(@termination_date)), 1000)

	-- Are we terminated in the past?
	IF  @terminationTimeFrame < @time_frame_id
	
		RETURN 5 -- not an employee
	
	-- Are we terminated this month _and_ have no enrolled coverage?
	IF
		@terminationTimeFrame = @time_frame_id
			AND
		[air].[etl].[ufnDoesTerminationMonthHaveCoverage](@employee_id, @time_frame_id, @termination_date) = 0

		RETURN 4	-- in termination month

	DECLARE @hireDateTimeFrame INT
	SELECT
		@hireDateTimeFrame = air.etl.ufnGetTimeFrameID(YEAR(@hire_date), MONTH(@hire_date))

	-- Are we hired in the future?
	IF @hireDateTimeFrame > @time_frame_id

		RETURN 5 -- not an employee

	DECLARE @initialMeasurementPeriodTimeFrame INT
	SELECT
		@initialMeasurementPeriodTimeFrame = [air].[etl].[ufnGetTimeFrameID](YEAR(@initial_measurement_period_end_date), MONTH(@initial_measurement_period_end_date))
		
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Determines what this month should be classified as basd on the passed information.' , @level0type=N'SCHEMA',@level0name=N'etl', @level1type=N'FUNCTION',@level1name=N'ufnGetDerivedMonthlyStatus'
GO

GRANT EXECUTE ON [etl].[ufnGetDerivedMonthlyStatus] TO [air-user] AS [dbo]
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

INSERT INTO [il].[mec_code] ([mec_code], [description]) VALUES ('1J', 'Description 2016 IRS 1J')
GO
INSERT INTO [il].[mec_code] ([mec_code], [description]) VALUES ('1K', 'Description 2016 IRS 1K')
GO

GRANT EXECUTE ON [etl].[ufnGetTimeFrameID] TO [air-user] AS [dbo]
GO
