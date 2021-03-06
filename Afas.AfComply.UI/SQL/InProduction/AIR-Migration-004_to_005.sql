USE [air]
GO
/****** Object:  StoredProcedure [dbo].[spUpdateAIR]    Script Date: 1/20/2017 11:58:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR]
	-- Add the parameters for the stored procedure here
	@employerId int,
	@yearId int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	UPDATE 
		air.appr.employee_monthly_detail 
	SET monthly_hours = ad.MonthlyAverageHours, 
		monthly_status_id = CASE
								WHEN ISNULL(ad.MonthlyAverageHours, 0) = 0 THEN 7
								WHEN ad.MonthlyAverageHours > 129.99 THEN 1
								WHEN ad.MonthlyAverageHours < 130 THEN 2
							END
	FROM 
		air.appr.employee_monthly_detail emd 
			INNER JOIN (
				SELECT
					MonthlyAverageHours, 
					eah.EmployeeId, 
					mea.employer_id
				FROM aca.dbo.EmployeeMeasurementAverageHours eah 
					INNER JOIN aca.dbo.measurement mea 
						ON eah.MeasurementId = mea.measurement_id
				WHERE  ((stability_start >= '2016-01-01 00:00:00.000' AND stability_start <= '2016-12-31 00:00:00.000')) AND (eah.EntityStatusId = 1)) ad 
					ON ad.EmployeeId = emd.employee_id 
						INNER JOIN air.gen.time_frame tf 
							ON emd.time_frame_id = tf.time_frame_id
	WHERE tf.year_id = @yearId AND emd.employer_id =  @employerId

	DECLARE @tempCode TABLE (
	employee_id int,
	time_frame_id int,
	offer_of_coverage_code varchar(10),
	safe_harbor_code varchar(10)
);
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
			(p.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
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
			(ee.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			(p.employee_id IS NULL)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
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
			emd.monthly_status_id, 
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
			mi.monthly_flex
		FROM
			employees_in_year ey
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyHours(@employerId, @yearId) mh ON (ey.employee_id = mh.employee_id) AND (ey.time_frame_id = mh.time_frame_id)
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyInsurance(@employerId, @yearId) mi ON (ey.employee_id = mi.employee_id) AND ((ey.time_frame_id = mi.time_frame_id))
			LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ey.classification_id = ec.classification_id)
			INNER JOIN air.appr.employee_monthly_detail emd ON (ey.employee_id = emd.employee_id)
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
			emd.monthly_status_id
	)
	INSERT INTO 
		@tempCode (
			employee_id, 
			time_frame_id, 
			offer_of_coverage_code, 
			safe_harbor_code)
	SELECT
		employee_id,
		time_frame_id,
		air.etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id) AS offer_of_coverage_code,
		air.etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code
	FROM
		employees_monthly_aggregates ema


	UPDATE air.appr.employee_monthly_detail
	SET offer_of_coverage_code = tc.offer_of_coverage_code, safe_harbor_code = tc.safe_harbor_code
	FROM air.appr.employee_monthly_detail emd INNER JOIN @tempCode tc
		ON emd.employee_id = tc.employee_id AND emd.time_frame_id = tc.time_frame_id
END
