USE [air]
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

	SET NOCOUNT ON;

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

	SET NOCOUNT ON;

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

	-- now appr hours.
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

	-- now appr.
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

	-- clean everything out first.
	UPDATE air.appr.employee_yearly_detail
	SET
		submittal_ready = 0,
		_1095C = 0
	FROM
		air.appr.employee_yearly_detail yd
	WHERE
		(yd.employer_id = @employer_id)
			AND
		(yd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- toggle back on the correct bits.
	UPDATE air.appr.employee_yearly_detail
	SET
		submittal_ready = 1,
		_1095C = 1
	FROM
		air.appr.employee_yearly_detail yd
	WHERE
		yd.employee_id IN (
				SELECT DISTINCT employee_id
				FROM air.appr.employee_monthly_detail md 
				WHERE
					((monthly_status_id = 1) OR (insurance_type_id = 2 AND enrolled = 1))
						AND
					(md.employee_id = employee_id)
		)
			OR
		yd.employee_id IN (
			SELECT DISTINCT employee_id
			FROM air.appr.employee_monthly_detail
			WHERE
				(enrolled = 0)
					AND
				(employee_id IN(SELECT DISTINCT employee_id FROM air.emp.covered_individual))
		)
			AND 
		(yd.employer_id = @employer_id)
			AND
		(yd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

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

			EXECUTE [air].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employerId, @yearId, @employeeId
				PRINT '1 *** Grab any potential full time coded employees that did not finish their IMP. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employerId, @yearId, @employeeId
				PRINT '2 *** Cleanup the overwritten line 15 entries. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId, @employeeId
				PRINT '3 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId, @employeeId
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

			EXECUTE [air].dbo.[spUpdateAir-Set1GCodes] @employerId, @yearId, @employeeId
				PRINT '7 *** Setting 1G flags now that all of the data is ready. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employerId, @yearId, @employeeId
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
				EXECUTE air.etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '8 *** End Insert Insert Employee Yearly Detail ***'
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
			EXECUTE air.dbo.spUpdateAIR @employer_id, @year_id, @employee_id
				PRINT '11 *** Update items for the 2016 tax year. ***'
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
