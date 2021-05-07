USE [air]
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
PRINT '8a: Delete Ale Yearly Detail'
DELETE
	air.ale.yearly_detail
WHERE
	(employer_id = @employer_id)
			AND
	(year_id = @year_id);

-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '8b: Insert Ale Yearly Detail'
INSERT INTO air.ale.yearly_detail (
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
		(SELECT COUNT(DISTINCT employee_id) FROM air.appr.employee_yearly_detail YD WHERE (_1095C = 1) AND (employer_id = @employer_id)),
		(SELECT COUNT(DISTINCT employee_id) FROM air.emp.yearly_detail WHERE (employer_id = @employer_id)),
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
	air.ale.monthly_detail amd WITH (NOLOCK)
	INNER JOIN air.gen.time_frame t WITH (NOLOCK) ON (amd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employer_id ,
				MAX(total_employee_count) AS total_employee_count,
				t.year_id
			FROM
				air.ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN air.gen.time_frame t WITH (NOLOCK) ON (amd.time_frame_id = t.time_frame_id)
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
				air.ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN air.gen.time_frame t WITH (NOLOCK) ON (amd.time_frame_id = t.time_frame_id)
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
				air.ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN air.gen.time_frame t WITH (NOLOCK) ON (amd.time_frame_id = t.time_frame_id)
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
				air.ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN air.gen.time_frame t WITH (NOLOCK) ON (amd.time_frame_id = t.time_frame_id)
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
				air.ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN air.gen.time_frame t WITH (NOLOCK) ON (amd.time_frame_id = t.time_frame_id)
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
				air.ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN air.gen.time_frame t WITH (NOLOCK) ON (amd.time_frame_id = t.time_frame_id)
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
PRINT '9a: Delete Employee Monthly Detail'
DELETE air.emp.monthly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(time_frame_id IN(SELECT time_frame_id FROM air.gen.time_frame WHERE year_id = @year_id))
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '9b: Insert Employee Monthly Detail';
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
PRINT '9a: Delete Employee Monthly Detail'
DELETE air.emp.monthly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(time_frame_id IN(SELECT time_frame_id FROM air.gen.time_frame WHERE year_id = @year_id))
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '9b: Insert Employee Monthly Detail';
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
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK)
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
				[air].[emp].[monthly_detail] emd WITH (NOLOCK)
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
ALTER PROCEDURE [appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP]
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
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK)
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
				[air].[appr].[employee_monthly_detail] emd WITH (NOLOCK)
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
ALTER PROCEDURE [appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP]
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
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK)
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
				[air].[appr].[employee_monthly_detail] emd WITH (NOLOCK)
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
ALTER PROCEDURE [appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP]
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
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK)
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
				[air].[appr].[employee_monthly_detail] emd WITH (NOLOCK)
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
ALTER PROCEDURE [appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP]
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
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK)
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
				[air].[appr].[employee_monthly_detail] emd WITH (NOLOCK)
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

	UPDATE [air].[emp].[monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		enrolled = eioe.CoverageInForce
	FROM
		[air].[emp].[monthly_detail] emd WITH (NOLOCK)
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
ALTER PROCEDURE [appr].[spUpdateAIR-InsuranceChangeEvents]
	@employerId int,
	@yearId smallint,
	@employeeId INT = NULL
AS
BEGIN

	UPDATE [air].[appr].[employee_monthly_detail]
	SET
		mec_offered = eioe.OfferInForce,
		enrolled = eioe.CoverageInForce
	FROM
		[air].[appr].[employee_monthly_detail] emd WITH (NOLOCK)
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
		air.emp.monthly_detail emd WITH (NOLOCK) 
		INNER JOIN (
			-- this needs to honor the split plan years and ensure the hours line up per measurement period vs. all one number.
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
					   aca.dbo.EmployeeMeasurementAverageHours eah WITH (NOLOCK)
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
					   aca.dbo.EmployeeMeasurementAverageHours eah WITH (NOLOCK)
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
		air.emp.monthly_detail emd WITH (NOLOCK)
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
		[emp].[monthly_detail] emd WITH (NOLOCK)
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
		[emp].[monthly_detail] emd WITH (NOLOCK)
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

	-- appr hours.
	UPDATE [air].[appr].[employee_monthly_detail]
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.appr.employee_monthly_detail emd WITH (NOLOCK) 
		INNER JOIN (
			-- this needs to honor the split plan years and ensure the hours line up per measurement period vs. all one number.
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
					   aca.dbo.EmployeeMeasurementAverageHours eah WITH (NOLOCK)
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
					   aca.dbo.EmployeeMeasurementAverageHours eah WITH (NOLOCK)
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
		air.appr.employee_monthly_detail emd WITH (NOLOCK)
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
		[appr].[employee_monthly_detail] emd WITH (NOLOCK)
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
		[appr].[employee_monthly_detail] emd WITH (NOLOCK)
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
PRINT '14a: Delete Appr Employee Yearly Detail';
DELETE air.emp.yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '14b: Insert Appr Employee Yearly Detail';
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

PRINT '14c: Update Employee _1095C Status';
EXECUTE [air].[etl].[spUpdate_1095C_status] @employer_id, @year_id, @employee_id

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
PRINT '15a: Delete Appr Employee Yearly Detail';
DELETE air.appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '15b: Insert Appr Employee Yearly Detail';
INSERT INTO air.appr.employee_yearly_detail (
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
	air.emp.yearly_detail  eyd 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

-- ______________________________________________________________________________________________________________________________________________________

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
PRINT '15a: Delete Appr Employee Yearly Detail';
DELETE air.appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '15b: Insert Appr Employee Yearly Detail';
INSERT INTO air.appr.employee_yearly_detail (
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
	air.emp.yearly_detail  eyd 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

-- ______________________________________________________________________________________________________________________________________________________

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
PRINT '15a: Delete Appr Employee Yearly Detail';
DELETE air.appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '15b: Insert Appr Employee Yearly Detail';
INSERT INTO air.appr.employee_yearly_detail (
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
	air.emp.yearly_detail  eyd 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

-- ______________________________________________________________________________________________________________________________________________________

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
PRINT '15a: Delete Appr Employee Yearly Detail';
DELETE air.appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '15b: Insert Appr Employee Yearly Detail';
INSERT INTO air.appr.employee_yearly_detail (
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
	air.emp.yearly_detail  eyd 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

-- ______________________________________________________________________________________________________________________________________________________

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

			EXECUTE air.etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '8 *** End Insert Ale Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE air.appr.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '9 *** End Insert Appr Employee Monthly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employer_id, @year_id, @employee_id
				PRINT '10A *** Grab any potential full time coded employees that did not finish their IMP. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employer_id, @year_id, @employee_id
				PRINT '10B *** Grab any potential full time coded employees that did not finish their IMP. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '11A *** Cleanup the overwritten line 15 entries. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '11B *** Cleanup the overwritten line 15 entries. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employer_id, @year_id, @employee_id
				PRINT '12A *** Updating the insurance information based on change events. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-InsuranceChangeEvents] @employer_id, @year_id, @employee_id
				PRINT '12B *** Updating the insurance information based on change events. (appr) ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '13A *** Updating hours from the MP/IMP and redetermining the monthly status. (emp) ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[appr].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '13B *** Updating hours from the MP/IMP and redetermining the monthly status. (appr) ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE [air].[etl].[spInsert_employee_yearly_detail] @employer_id, @year_id, @employee_id
				PRINT '14 *** End Insert Insert Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE [air].[appr].[spInsert_employee_yearly_detail_init] @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '15 *** End Insert Appr Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '16A *** Setting 1G flags now that all of the data is ready. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '16B *** Setting 1G flags now that all of the data is ready. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employer_id, @year_id, @employee_id
				PRINT '17 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

		END	
	COMMIT TRAN BuildFormsTables;
	SELECT 'Successful';
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN ETL_BUILD
	EXEC aca.dbo.INSERT_ErrorLogging
	SELECT ERROR_PROCEDURE() AS ErrorProcedure, ERROR_MESSAGE() AS ErrorMessage;
END CATCH

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

			EXECUTE air.etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '8 *** End Insert Ale Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE air.appr.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '9 *** End Insert Appr Employee Monthly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employer_id, @year_id, @employee_id
				PRINT '10A *** Grab any potential full time coded employees that did not finish their IMP. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employer_id, @year_id, @employee_id
				PRINT '10B *** Grab any potential full time coded employees that did not finish their IMP. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '11A *** Cleanup the overwritten line 15 entries. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '11B *** Cleanup the overwritten line 15 entries. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employer_id, @year_id, @employee_id
				PRINT '12A *** Updating the insurance information based on change events. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-InsuranceChangeEvents] @employer_id, @year_id, @employee_id
				PRINT '12B *** Updating the insurance information based on change events. (appr) ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '13A *** Updating hours from the MP/IMP and redetermining the monthly status. (emp) ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[appr].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '13B *** Updating hours from the MP/IMP and redetermining the monthly status. (appr) ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE [air].[etl].[spInsert_employee_yearly_detail] @employer_id, @year_id, @employee_id
				PRINT '14 *** End Insert Insert Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE [air].[appr].[spInsert_employee_yearly_detail_init] @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '15 *** End Insert Appr Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '16A *** Setting 1G flags now that all of the data is ready. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '16B *** Setting 1G flags now that all of the data is ready. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employer_id, @year_id, @employee_id
				PRINT '17 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

		END	
	COMMIT TRAN BuildFormsTables;
	SELECT 'Successful';
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN ETL_BUILD
	EXEC aca.dbo.INSERT_ErrorLogging
	SELECT ERROR_PROCEDURE() AS ErrorProcedure, ERROR_MESSAGE() AS ErrorMessage;
END CATCH

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

			EXECUTE air.etl.spInsert_employee @employer_id, @employee_id
				PRINT '1 *** End Insert Employee ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spUpdate_employee @employer_id, @employee_id
				PRINT '2 *** End Update Employee ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_covered_individuals @employer_id, @year_id, @employee_id
				PRINT '3 *** End Insert Covered Individuals ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_covered_individuals_monthly_detail @employer_id, @year_id
				PRINT '4 *** End Insert Covered Individuals Monthly ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '5 *** End Insert Employee Monthly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE air.etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '6 *** End Insert Ale Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-ImportFullTimeStatusNotFinishedWithIMP] @employer_id, @year_id, @employee_id
				PRINT '7 *** Grab any potential full time coded employees that did not finish their IMP. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '8 *** Cleanup the overwritten line 15 entries. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employer_id, @year_id, @employee_id
				PRINT '9 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '10 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '11 *** End Insert Insert Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '12 *** Setting 1G flags now that all of the data is ready. ***'
				PRINT ''
				PRINT ''

		END	
	COMMIT TRAN BuildFormsTables;
	SELECT 'Successful';
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN ETL_BUILD
	EXEC aca.dbo.INSERT_ErrorLogging
	SELECT ERROR_PROCEDURE() AS ErrorProcedure, ERROR_MESSAGE() AS ErrorMessage;
END CATCH					
-- ______________________________________________________________________________________________________________________________________________________

GO
