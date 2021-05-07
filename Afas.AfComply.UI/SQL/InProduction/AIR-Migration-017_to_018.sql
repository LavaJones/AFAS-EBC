USE [air]
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


