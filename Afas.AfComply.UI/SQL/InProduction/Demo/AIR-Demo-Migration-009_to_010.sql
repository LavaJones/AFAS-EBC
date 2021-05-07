USE [air-demo]
GO
/****** Object:  UserDefinedFunction [etl].[ufnEmployeeMonthlyInsurance]    Script Date: 2/1/2017 11:13:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--__________________________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/12/2016
-- Description:	Returns the monthly employee coverage.
--__________________________________________________________________________________________________________________________________________________________________
--__________________________________________________________________________________________________________________________________________________________________
ALTER FUNCTION [etl].[ufnEmployeeMonthlyInsurance] (@employer_id INT, @year_id SMALLINT) RETURNS TABLE AS
--__________________________________________________________________________________________________________________________________________________________________
RETURN
WITH
	employee_insurance AS
	(
		SELECT DISTINCT 
			ei.employer_id,
			ei.employee_id,
			ei.plan_year_id,
			py.startDate,
			py.endDate,
			ei.offeredOn,
			ei.acceptedOn,
			ei.accepted,
			ei.effectiveDate,
			ic.amount,
			MAX(CAST(ISNULL(ei.hra_flex_contribution,0)AS DECIMAL(10,2))) AS monthly_flex,
			i.monthlycost,
			ee.terminationDate,
			ee.hireDate,
			i.minValue,
			i.offSpouse,
			i.offDependent,
			i.SpouseConditional,
			i.insurance_type_id,
			it.name,
			ic.contribution_id, 
			[air-demo].etl.ufnGetMonthFromTimeFrame(month_counter) AS month_id,
			@year_id AS year_id, 
			v.month_counter AS time_frame_id,
			LAST_VALUE([air-demo].etl.ufnGetMonthFromTimeFrame(month_counter)) OVER (PARTITION BY ei.employee_id ORDER BY ei.effectiveDate) AS effective_month_count,
			DATEDIFF(MONTH, py.startDate, py.endDate) + 1 AS plan_month_count,
			ee.aca_status_id
		FROM
			[aca-demo].dbo.employee_insurance_offer ei 
			INNER JOIN [aca-demo].dbo.employee ee ON (ei.employee_id = ee.employee_id)
			INNER JOIN [aca-demo].dbo.plan_year py ON (ei.plan_year_id = py.plan_year_id)
			INNER JOIN [aca-demo].dbo.insurance i ON (ei.insurance_id = i.insurance_id)
			INNER JOIN [aca-demo].dbo.insurance_type it ON (i.insurance_type_id = it.insurance_type_id)
			INNER JOIN [aca-demo].dbo.insurance_contribution ic ON (ei.ins_cont_id = ic.ins_cont_id)
			INNER JOIN (SELECT number AS month_counter FROM [air-demo].gen.number WHERE (number > 0)) v ON (v.month_counter BETWEEN [air-demo].etl.ufnGetTimeFrameID(@year_id,1) AND [air-demo].etl.ufnGetTimeFrameID(YEAR(py.endDate),MONTH(py.endDate)))
		WHERE
			(ei.employer_id = @employer_id)  
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			((py.startDate BETWEEN '2015-01-01 00:00:00' AND '2015-12-31 23:59:59'))
				AND
			v.month_counter >= [air-demo].etl.ufnGetTimeFrameID(YEAR(effectiveDate),
			IIF(DAY(effectiveDate) = 1, MONTH(effectiveDate), MONTH(effectiveDate)+1))
		GROUP BY
			ei.employer_id,
			ei.employee_id,
			ei.plan_year_id,
			py.startDate,
			py.endDate,
			ee.initialMeasurmentEnd,
			ei.offeredOn,
			ei.acceptedOn,
			ei.accepted,
			ei.effectiveDate,
			ic.amount,
			i.monthlycost,
			ee.terminationDate,
			ee.hireDate,
			i.minValue,
			i.offSpouse,
			i.offDependent,
			i.insurance_type_id,
			i.SpouseConditional, 
			it.name,
			ic.contribution_id,
			v.month_counter,
			ee.aca_status_id

		UNION

		SELECT DISTINCT 
			ei.employer_id,
			ei.employee_id,
			ei.plan_year_id,
			py.startDate,
			py.endDate,
			ei.offeredOn,
			ei.acceptedOn,
			ei.accepted,
			ei.effectiveDate,
			ic.amount,
			MAX(CAST(ISNULL(ei.hra_flex_contribution,0)AS DECIMAL(10,2))) AS monthly_flex,
			i.monthlycost,
			ee.terminationDate,
			ee.hireDate,
			i.minValue,
			i.offSpouse,
			i.offDependent,
			i.SpouseConditional, 
			i.insurance_type_id,
			it.name,
			ic.contribution_id, 
			[air-demo].etl.ufnGetMonthFromTimeFrame(month_counter) AS month_id,
			@year_id AS year_id, 
			v.month_counter AS time_frame_id,
			LAST_VALUE([air-demo].etl.ufnGetMonthFromTimeFrame(month_counter)) OVER (PARTITION BY ei.employee_id ORDER BY ei.effectiveDate) - IIF(YEAR(ei.effectiveDate) =  @year_id, MONTH(ei.effectiveDate)-1,0) AS effective_month_count,
			DATEDIFF(MONTH, py.startDate, py.endDate) + 1 AS plan_month_count,
			ee.aca_status_id			
		FROM
			[aca-demo].dbo.employee_insurance_offer ei
			INNER JOIN [aca-demo].dbo.employee ee ON (ei.employee_id = ee.employee_id)
			INNER JOIN [aca-demo].dbo.plan_year py ON (ei.plan_year_id = py.plan_year_id)
			INNER JOIN [aca-demo].dbo.insurance i ON (ei.insurance_id = i.insurance_id)
			INNER JOIN [aca-demo].dbo.insurance_type it ON (i.insurance_type_id = it.insurance_type_id)
			INNER JOIN [aca-demo].dbo.insurance_contribution ic ON (ei.ins_cont_id = ic.ins_cont_id)
			INNER JOIN (SELECT number AS month_counter FROM [air-demo].gen.number WHERE (number > 0)) v ON (v.month_counter BETWEEN [air-demo].etl.ufnGetTimeFrameID(YEAR(ei.effectiveDate),IIF(DAY(effectiveDate) = 1, MONTH(effectiveDate), MONTH(effectiveDate)+1)) AND [air-demo].etl.ufnGetTimeFrameID(@year_id,MONTH(CAST(@year_id AS VARCHAR(4)) + '-12-31 00:00:00')))
		WHERE
			(ei.employer_id = @employer_id)  
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			((py.startDate BETWEEN '2016-01-01 00:00:00' AND '2016-12-31 23:59:59'))
		GROUP BY
			ei.employer_id,
			ei.employee_id,
			ei.plan_year_id,
			py.startDate,
			py.endDate,
			ee.initialMeasurmentEnd,
			ei.offeredOn,
			ei.acceptedOn,
			ei.accepted,
			ei.effectiveDate,
			ic.amount,
			i.monthlycost,
			ee.terminationDate,
			ee.hireDate,
			i.minValue,
			i.offSpouse,
			i.offDependent,
			i.SpouseConditional,
			i.insurance_type_id, 
			it.name,
			ic.contribution_id,
			v.month_counter,
			ee.aca_status_id
	)

SELECT DISTINCT
	employer_id,
	employee_id,
	plan_year_id,
	startDate,
	endDate, 
	offeredOn,
	acceptedOn,
	accepted,
	effectiveDate,
	monthlycost AS init_amount,
	amount AS employer_contribution,
	IIF(contribution_id = '$', IIF(monthlycost-(ISNULL(monthly_flex,0) + ISNULL(amount,0)) < 0, 0, monthlycost-(ISNULL(monthly_flex,0)+ ISNULL(amount,0))), IIF(monthlycost - (monthlycost * IIF(amount > 0, (amount/100),1))-ISNULL(monthly_flex,0) <0,0, monthlycost - (monthlycost * IIF(amount > 0, (amount/100),1))-ISNULL(monthly_flex,0))) AS slcmp,
	monthly_flex,
	terminationDate,
	hireDate AS hire_date,
	minValue,
	offSpouse,
	offDependent,
	insurance_type_id,
	SpouseConditional, 
	name,
	contribution_id,
	month_id,
	year_id,
	time_frame_id,
	effective_month_count,
	plan_month_count	
FROM
	employee_insurance
WHERE
	(
		time_frame_id <= 
			IIF(
						aca_status_id NOT IN (4),
						-- reducing confusion by setting dates for 2016 reporting, 2015 measurement. gc5
						[air-demo].etl.ufnGetTimeFrameID(YEAR(ISNULL(terminationDate,'2016-01-01')), MONTH(ISNULL(terminationDate,'2016-12-31'))), 
						-- reducing confusion by setting dates for 2016 reporting, 2015 measurement. gc5
						[air-demo].etl.ufnGetTimeFrameID(YEAR('2016-12-01'), 12)) -- not sure why this is not 12-31. gc5
				)
				AND
		-- reducing confusion by setting dates for 2016 reporting, 2015 measurement. gc5
		(time_frame_id BETWEEN ([air-demo].etl.ufnGetTimeFrameID(YEAR('2016-01-01'), 1)) AND [air-demo].etl.ufnGetTimeFrameID(YEAR('2016-12-31'), 12))
		AND
		(time_frame_id >= [air-demo].etl.ufnGetTimeFrameID(YEAR(hireDate), IIF(DAY(hireDate) = 1, MONTH(hireDate), MONTH(hireDate)+1)))
		-- took out the CAST and correct paranth. ltv

GO

ALTER FUNCTION [etl].[ufnGetMecCode] (
		@timeframe_id SMALLINT, 
		@offered_to_employee BIT, 
		@offered_to_spouse BIT, 
		@offered_to_dependents BIT, 
		@min_value BIT, 
		@mainland BIT, 
		@effective_date DATE, 
		@termination_date DATE, 
		@aca_status TINYINT,
		@spouseConditional BIT
	)
RETURNS CHAR(2)
AS
BEGIN

-- Note still needs the 1K/1J logic. gc5

IF (@effective_date) IS NULL RETURN '1H'

IF (@timeframe_id <= [air-demo].etl.ufnGetTimeFrameID(YEAR(@effective_date), MONTH(@effective_date))) AND DAY(@effective_date) > 1 RETURN '1H'

IF (@termination_date IS NOT NULL AND @timeframe_id >= [air-demo].etl.ufnGetTimeFrameID(YEAR(@termination_date), MONTH(@termination_date))) AND DAY(@termination_date) < [air-demo].gen.ufnGetDayCountInMonth(@termination_date) RETURN '1H' 
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
					ELSE '1H'
				END
			END
	END

RETURN NULL

END
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
			SpouseConditional
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
			SpouseConditional
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
	[air-demo].etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id, SpouseConditional) AS offer_of_coverage_code,
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