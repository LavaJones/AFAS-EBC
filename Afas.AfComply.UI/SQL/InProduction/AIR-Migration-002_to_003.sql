USE [aca]
GO
GRANT SELECT ON dbo.ufnGetEmployeeInsurance TO  [aca-user] AS [dbo]
GO

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

	IF @monthly_status_id = 5 RETURN '2A' --(OR COBRA)
	IF @offered_coverage = 1 AND @enrolled = 1 AND @aca_status IN(4) AND @monthly_status_id IN(5) RETURN '2A'
	IF @offered_coverage = 1 AND @enrolled = 1 AND @monthly_status_id = 4 AND DAY(@termination_date) < air.gen.ufnGetDayCountInMonth(@termination_date) RETURN '2B'
	IF @offered_coverage = 1 AND @enrolled = 1 AND @monthly_status_id IN(1,2,3,4,6,7) RETURN '2C'
	IF @offered_coverage = 1 AND @enrolled = 0 AND @monthly_status_id = 2 RETURN '2B'
	IF @offered_coverage = 1 AND @enrolled = 0 AND @monthly_status_id IN(1,3,6,7) RETURN @ash_code
	IF @offered_coverage = 0 AND @monthly_status_id IN(3,6)  RETURN '2D'

	RETURN NULL

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--______________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/11/2015
-- Description:	Returns the monthly employee hours.
--______________________________________________________________________________________________________________________________
--______________________________________________________________________________________________________________________________
ALTER FUNCTION [etl].[ufnEmployeePayPeriodDetails] (
		@employer_id int,
		@year_id INT
	) RETURNS TABLE AS
--______________________________________________________________________________________________________________________________
RETURN
WITH
	payroll_details AS
	(
		SELECT DISTINCT 
			p.employer_id,
			p.employee_id,
			ee.hireDate,
			ee.terminationDate,
			p.sdate,
			p.edate,  
			SUM(p.act_hours) AS act_hours_in_period,
			v.month_counter,  
			MONTH(DATEADD(MONTH, v.month_counter - 1, sdate)) AS month_id,
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			YEAR(DATEADD(MONTH, v.month_counter - 1, CONVERT(DATETIME,'2016-01-01 00:00:00', 102))) AS year_id, 
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			air.etl.ufnGetTimeFrameID(YEAR(DATEADD(MONTH, v.month_counter - 1, CONVERT(DATETIME,'2016-01-01 00:00:00', 102))), MONTH(DATEADD(MONTH, v.month_counter - 1, sdate))) AS time_frame_id,
			DATEDIFF(dd, p.sdate, p.edate) + 1 AS total_days_in_period,
			CASE
				WHEN (MONTH(DATEADD(MONTH, v.month_counter - 1,sdate)) = MONTH(ee.hireDate)) AND (YEAR(ee.hireDate) = @year_id) AND DAY(p.sdate) < DAY(ee.hireDate)
					THEN DATEDIFF(dd, ee.hireDate, IIF(MONTH(p.edate) > MONTH(p.sdate), CAST(@year_id AS NVARCHAR(4)) + '-' + CAST(MONTH(ee.hireDate) AS NVARCHAR(2)) + '-' + CAST(air.gen.ufnGetDayCountInMonth(ee.hireDate) AS NVARCHAR(2)), p.edate) + 1)
				WHEN MONTH(DATEADD(MONTH, v.month_counter - 1, p.sdate)) = MONTH(ee.terminationDate) AND (YEAR(ee.terminationDate) = @year_id) 
					THEN DATEDIFF(dd,
							CASE
								WHEN MONTH(p.sdate) = MONTH(ee.terminationDate) THEN p.sdate 
								ELSE CONVERT(DATE, CAST(YEAR(ee.terminationDate) AS VARCHAR(4)) + '-' + CAST(MONTH(ee.terminationDate) AS VARCHAR(2)) + '-01') 
							END,
							IIF(edate > ee.terminationDate, ee.terminationDate, edate)  + 1)
				WHEN v.month_counter = 1
					THEN
						CASE
							WHEN LAST_VALUE(v.month_counter) OVER (PARTITION BY p.sdate ORDER BY p.edate) = 1
								THEN (DATEPART(dd, p.edate)) - (DATEPART(dd, p.sdate) - 1)
							ELSE air.gen.ufnGetDayCountInMonth(p.sdate) - (DATEPART(dd, p.sdate) - 1) 
						END
				WHEN v.month_counter < LAST_VALUE(v.month_counter) OVER (PARTITION BY p.employee_id ORDER BY p.sdate) 
					THEN air.gen.ufnGetDayCountInMonth(CAST(DATEPART(MONTH, DATEADD(MONTH, v.month_counter - 1, sdate)) AS VARCHAR(4)) + '/' + '1' + '/' + CAST(DATEPART(YEAR, DATEADD(MONTH, v.month_counter - 1, sdate)) AS VARCHAR(4)))	
				ELSE
					(DATEPART(dd, edate)) 	
			END AS days_from_period_in_month, 
			LAST_VALUE(v.month_counter) OVER (PARTITION BY p.sdate ORDER BY p.edate) AS month_count
		FROM
			aca.dbo.payroll p
			INNER JOIN aca.dbo.employee ee ON (p.employee_id = ee.employee_id)
			INNER JOIN (
				SELECT
					number AS month_counter
				FROM
					air.gen.number
				WHERE
					(number > 0)) v ON (v.month_counter - 1 <= DATEDIFF(MONTH, p.sdate, p.edate))
		WHERE
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.edate > CONVERT(DATETIME,'2014-12-31 00:00:00', 102)) 
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.sdate < CONVERT(DATETIME,'2016-01-01 00:00:00', 102)) 
				AND
			(ee.employer_id = @employer_id) 
				AND
			(p.sdate < ISNULL(terminationDate, '2016-12-31 00:00:00'))
				AND
			(p.sdate > ISNULL(hireDate, '2014-01-01 00:00:00'))
		GROUP BY
			p.employer_id,
			p.employee_id,
			v.month_counter,
			p.sdate,
			p.edate,
			ee.terminationDate,
			ee.hireDate
	)

SELECT
	employer_id,
	employee_id,
	hireDate,
	terminationDate,
	sdate,
	edate,
	act_hours_in_period,
	month_counter, 
	month_id,
	year_id,
	time_frame_id,
	total_days_in_period,
	days_from_period_in_month,
	month_count,
	CONVERT(DECIMAL(18,2),act_hours_in_period/total_days_in_period * days_from_period_in_month) AS hours_from_pay_period_in_month 
FROM
	payroll_details
--______________________________________________________________________________________________________________________________
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/8/2016 
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER FUNCTION [gen].[ufnGetDayCountInMonth] (@date DATE)
RETURNS INT
AS
BEGIN
    RETURN 
    CASE
		WHEN MONTH(@date) IN (1, 3, 5, 7, 8, 10, 12) THEN 31
		WHEN MONTH(@date) IN (4, 6, 9, 11) THEN 30
		ELSE 
			CASE 
				WHEN
						(YEAR(@date) % 4 = 0 AND YEAR(@date) % 100 <> 0)
							OR
						(YEAR(@date) % 400  = 0)
					THEN 29
				ELSE 28
			END
    END

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/12/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER FUNCTION [etl].[ufnGetMecCode] (
		@timeframe_id SMALLINT, 
		@offered_to_employee BIT, 
		@offered_to_spouse BIT, 
		@offered_to_dependents BIT, 
		@min_value BIT, 
		@mainland BIT, 
		@effective_date DATE, 
		@termination_date DATE, 
		@aca_status TINYINT
	)
RETURNS CHAR(2)
AS
BEGIN

-- Note still needs the 1K/1J logic. gc5

IF (@effective_date) IS NULL RETURN '1H'

IF (@timeframe_id <= air.etl.ufnGetTimeFrameID(YEAR(@effective_date), MONTH(@effective_date))) AND DAY(@effective_date) > 1 RETURN '1H'

IF (@termination_date IS NOT NULL AND @timeframe_id >= air.etl.ufnGetTimeFrameID(YEAR(@termination_date), MONTH(@termination_date))) AND DAY(@termination_date) < air.gen.ufnGetDayCountInMonth(@termination_date) RETURN '1H' 
ELSE
	BEGIN
		RETURN 
		CASE
			WHEN  @min_value = 1 THEN
				CASE
					WHEN @offered_to_employee = 1 THEN
						CASE
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/16/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER FUNCTION [etl].[ufnGetMonthFromTimeFrame] (@time_frame_id SMALLINT)
RETURNS TINYINT
AS
BEGIN

DECLARE @return TINYINT

SELECT
	@return = t.month_id
FROM
	air.gen.time_frame t
WHERE
	(t.time_frame_id = @time_frame_id)

RETURN @return

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/30/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER FUNCTION [etl].[ufnGetTimeFrameID] (@year_id SMALLINT, @month_id SMALLINT)
RETURNS INT
AS
BEGIN

DECLARE @return SMALLINT

SELECT
	@return = t.time_frame_id
FROM
	air.gen.time_frame t
WHERE
	(t.year_id = @year_id)
		AND
	(t.month_id = @month_id)

RETURN @return

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--______________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/27/2016
-- Description:	Returns the monthly employee hours.
--______________________________________________________________________________________________________________________________
--______________________________________________________________________________________________________________________________
ALTER FUNCTION [etl].[ufnEmployeeSummerAvgDetails] (
		@employer_id int,
		@year_id INT
	) RETURNS TABLE AS
--______________________________________________________________________________________________________________________________
RETURN
WITH
	summer_averages AS
	(
		SELECT DISTINCT
			p.employer_id,
			p.employee_id,
			ee.hireDate,
			ee.terminationDate,
			p.sdate, 
			p.edate,  
			SUM(p.act_hours) AS act_hours_in_period,
			v.month_counter,  
			MONTH(DATEADD(MONTH, v.month_counter - 1,sdate)) AS month_id,
			YEAR(DATEADD(MONTH, v.month_counter - 1,sdate)) AS year_id, 
			air.etl.ufnGetTimeFrameID(YEAR(DATEADD(MONTH, v.month_counter - 1, p.sdate)), MONTH(DATEADD(MONTH, v.month_counter - 1, p.sdate))) AS time_frame_id,
			DATEDIFF(dd, p.sdate, p.edate) + 1 AS total_days_in_period,
			CASE
				WHEN (MONTH(DATEADD(MONTH, v.month_counter - 1, p.sdate)) = MONTH(ee.hireDate)) AND (YEAR(ee.hireDate) = @year_id) AND DAY(p.sdate) < DAY(ee.hireDate)
					THEN DATEDIFF(dd, ee.hireDate, p.edate) + 1
				WHEN MONTH(DATEADD(MONTH, v.month_counter - 1, p.sdate)) = MONTH(ee.terminationDate) AND (YEAR(ee.terminationDate) = @year_id) 
					THEN DATEDIFF(
						dd,
						CASE
							WHEN MONTH(p.sdate) = MONTH(ee.terminationDate) 
								THEN p.sdate 
							ELSE CONVERT(DATE, CAST(YEAR(ee.terminationDate) AS VARCHAR(4)) + '-' + CAST(MONTH(ee.terminationDate) AS VARCHAR(2)) + '-01') 
						END,
						IIF(p.edate > ee.terminationDate, ee.terminationDate, p.edate)  + 1)
				WHEN v.month_counter = 1
					THEN 
						CASE
							WHEN LAST_VALUE(v.month_counter) OVER (PARTITION BY p.sdate ORDER BY p.edate) = 1
								THEN (DATEPART(dd, p.edate)) - (DATEPART(dd, p.sdate) - 1)
							ELSE air.gen.ufnGetDayCountInMonth(p.sdate) - (DATEPART(dd, p.sdate) - 1) 
						END
				WHEN v.month_counter < LAST_VALUE(v.month_counter) OVER (PARTITION BY p.employee_id ORDER BY sdate) 
					THEN air.gen.ufnGetDayCountInMonth(CAST(DATEPART(MONTH, DATEADD(MONTH, v.month_counter - 1, sdate)) AS VARCHAR(4)) + '/' + '1' + '/' + CAST(DATEPART(YEAR, DATEADD(MONTH, v.month_counter - 1, sdate)) AS VARCHAR(4)))	
				ELSE (DATEPART(dd, p.edate)) 	
			END AS days_from_period_in_month, 
			LAST_VALUE(v.month_counter) OVER (PARTITION BY p.sdate ORDER BY p.edate) AS month_count
		FROM
			aca.dbo.payroll_summer_averages (NOLOCK) p
			INNER JOIN aca.dbo.employee ee (NOLOCK) ON (p.employee_id = ee.employee_id)
			INNER JOIN (SELECT
							number AS month_counter
						FROM
							air.gen.number
						WHERE
							(number > 0)) v ON (v.month_counter - 1 <= DATEDIFF(MONTH, p.sdate, p.edate))
		WHERE
			(ee.employer_id = @employer_id) 
				AND
			-- to reduce confusion we are leaving the vendor's clients data in place for 2016 reporting, 2015 measuring. gc5
			(ISNULL(ee.terminationDate, '2015-09-30') > p.sdate)
		GROUP BY
			p.employer_id,
			p.employee_id,
			v.month_counter,
			p.sdate,
			p.edate,
			ee.terminationDate,
			ee.hireDate
	)

SELECT
	employer_id,
	employee_id,
	hireDate,
	terminationDate,
	sdate,
	edate,
	act_hours_in_period,
	month_counter, 
	month_id,
	year_id,
	time_frame_id,
	total_days_in_period,
	days_from_period_in_month,
	month_count,
	CONVERT(DECIMAL(18,2),act_hours_in_period/total_days_in_period * days_from_period_in_month) AS calculated_summer_period_hours
FROM
	summer_averages
--______________________________________________________________________________________________________________________________
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--______________________________________________________________________________________________________________________________
-- Author:           Scott Harvey
-- Create date: 1/18/2016
-- Description:      Returns the monthly employee hours.
--______________________________________________________________________________________________________________________________
--______________________________________________________________________________________________________________________________
ALTER FUNCTION [etl].[ufnEmployeeMonthlyHours] (@employer_id int, @year_id INT) RETURNS TABLE AS
--______________________________________________________________________________________________________________________________
RETURN
WITH
	employee AS
	(
		SELECT
			ee.employee_id,
			t.time_frame_id,
			ee.employer_id                   
		FROM
			aca.dbo.employee ee
			CROSS JOIN air.gen.time_frame t
		WHERE
			(t.year_id = @year_id)
				AND
			(ee.employer_id = @employer_id)
		GROUP BY 
			ee.employee_id,
			t.time_frame_id,
			ee.employer_id
	),
	payroll_hours AS
	(
		SELECT
			ee.employee_id,
			ee.time_frame_id,
			ee.employer_id, 
			SUM(ISNULL(pd.hours_from_pay_period_in_month,0)) AS payroll_monthly_hours
		FROM
			employee ee 
			LEFT OUTER JOIN air.etl.ufnEmployeePayPeriodDetails(@employer_id,@year_id) pd ON (ee.time_frame_id = pd.time_frame_id) AND (ee.employee_id = pd.employee_id)
		GROUP BY
			ee.employee_id,
			ee.time_frame_id,
			ee.employer_id
	),
	summer_hours AS
	(
		SELECT
			ee.employee_id,
			ee.time_frame_id,
			ee.employer_id,
			SUM(ISNULL(sh.calculated_summer_period_hours,0)) AS calculated_sumer_hours
		FROM
			employee ee 
			LEFT OUTER JOIN air.etl.ufnEmployeeSummerAvgDetails(@employer_id,@year_id) sh ON (ee.time_frame_id = sh.time_frame_id) AND (ee.employee_id = sh.employee_id)
		GROUP BY
			ee.employee_id,
			ee.time_frame_id,
			ee.employer_id
	)
SELECT
	ph.employee_id,
	ph.time_frame_id,
	ph.employer_id,
	ph.payroll_monthly_hours,
	sh.calculated_sumer_hours,
    ISNULL(ph.payroll_monthly_hours,0) + ISNULL(sh.calculated_sumer_hours,0) AS monthly_hours
FROM
	payroll_hours ph
    LEFT OUTER JOIN summer_hours sh ON (ph.employee_id = sh.employee_id) AND (ph.time_frame_id = sh.time_frame_id)
GO

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
			MAX(CAST(ISNULL(ei.hra_flex_contribution/12,0)AS DECIMAL(10,2))) AS monthly_flex,
			i.monthlycost,
			ee.terminationDate,
			ee.hireDate,
			i.minValue,
			i.offSpouse,
			i.offDependent, 
			i.insurance_type_id,
			it.name,
			ic.contribution_id, 
			air.etl.ufnGetMonthFromTimeFrame(month_counter) AS month_id,
			@year_id AS year_id, 
			v.month_counter AS time_frame_id,
			LAST_VALUE(air.etl.ufnGetMonthFromTimeFrame(month_counter)) OVER (PARTITION BY ei.employee_id ORDER BY ei.effectiveDate) AS effective_month_count,
			DATEDIFF(MONTH, py.startDate, py.endDate) + 1 AS plan_month_count,
			ee.aca_status_id
		FROM
			aca.dbo.employee_insurance_offer ei 
			INNER JOIN aca.dbo.employee ee ON (ei.employee_id = ee.employee_id)
			INNER JOIN aca.dbo.plan_year py ON (ei.plan_year_id = py.plan_year_id)
			INNER JOIN aca.dbo.insurance i ON (ei.insurance_id = i.insurance_id)
			INNER JOIN aca.dbo.insurance_type it ON (i.insurance_type_id = it.insurance_type_id)
			INNER JOIN aca.dbo.insurance_contribution ic ON (ei.ins_cont_id = ic.ins_cont_id)
			INNER JOIN (SELECT number AS month_counter FROM air.gen.number WHERE (number > 0)) v ON (v.month_counter BETWEEN air.etl.ufnGetTimeFrameID(@year_id,1) AND air.etl.ufnGetTimeFrameID(YEAR(py.endDate),MONTH(py.endDate)))
		WHERE
			(ei.employer_id = @employer_id)  
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(py.startDate BETWEEN '2016-01-01 00:00:00' AND '2016-12-31 23:59:59')
				AND
			v.month_counter >= air.etl.ufnGetTimeFrameID(YEAR(effectiveDate),
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
			MAX(CAST(ISNULL(ei.hra_flex_contribution/12,0)AS DECIMAL(10,2))) AS monthly_flex,
			i.monthlycost,
			ee.terminationDate,
			ee.hireDate,
			i.minValue,
			i.offSpouse,
			i.offDependent, 
			i.insurance_type_id,
			it.name,
			ic.contribution_id, 
			air.etl.ufnGetMonthFromTimeFrame(month_counter) AS month_id,
			@year_id AS year_id, 
			v.month_counter AS time_frame_id,
			LAST_VALUE(air.etl.ufnGetMonthFromTimeFrame(month_counter)) OVER (PARTITION BY ei.employee_id ORDER BY ei.effectiveDate) - IIF(YEAR(ei.effectiveDate) =  @year_id, MONTH(ei.effectiveDate)-1,0) AS effective_month_count,
			DATEDIFF(MONTH, py.startDate, py.endDate) + 1 AS plan_month_count,
			ee.aca_status_id			
		FROM
			aca.dbo.employee_insurance_offer ei
			INNER JOIN aca.dbo.employee ee ON (ei.employee_id = ee.employee_id)
			INNER JOIN aca.dbo.plan_year py ON (ei.plan_year_id = py.plan_year_id)
			INNER JOIN aca.dbo.insurance i ON (ei.insurance_id = i.insurance_id)
			INNER JOIN aca.dbo.insurance_type it ON (i.insurance_type_id = it.insurance_type_id)
			INNER JOIN aca.dbo.insurance_contribution ic ON (ei.ins_cont_id = ic.ins_cont_id)
			INNER JOIN (SELECT number AS month_counter FROM air.gen.number WHERE (number > 0)) v ON (v.month_counter BETWEEN air.etl.ufnGetTimeFrameID(YEAR(ei.effectiveDate),IIF(DAY(effectiveDate) = 1, MONTH(effectiveDate), MONTH(effectiveDate)+1)) AND air.etl.ufnGetTimeFrameID(@year_id,MONTH(CAST(@year_id AS VARCHAR(4)) + '-12-31 00:00:00')))
		WHERE
			(ei.employer_id = @employer_id)  
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(py.startDate BETWEEN '2016-01-01 00:00:00' AND '2016-12-31 00:00:00') 
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
						air.etl.ufnGetTimeFrameID(YEAR(ISNULL(terminationDate,'2017-01-01')), MONTH(ISNULL(terminationDate,'2017-01-01'))), 
						-- reducing confusion by setting dates for 2016 reporting, 2015 measurement. gc5
						air.etl.ufnGetTimeFrameID(YEAR('2016-12-01'), 12)) -- not sure why this is not 12-31. gc5
				)
				AND
		-- reducing confusion by setting dates for 2016 reporting, 2015 measurement. gc5
		(time_frame_id BETWEEN (air.etl.ufnGetTimeFrameID(YEAR('2016-01-01'), 1)) AND air.etl.ufnGetTimeFrameID(YEAR('2016-12-01'), 12))
		AND
		(time_frame_id >= air.etl.ufnGetTimeFrameID(YEAR(hireDate), IIF(DAY(hireDate) = 1, MONTH(hireDate), MONTH(hireDate)+1)))
		-- took out the CAST and correct paranth. ltv
--__________________________________________________________________________________________________________________________________________________________________
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
ALTER PROCEDURE [etl].[spInsert_ale_employer]
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
PRINT '1: Insert Employer';
INSERT INTO air.ale.employer 
					(employer_id, ein, name, [address], city, state_code, zipcode, 
						contact_first_name, contact_last_name, contact_telephone)
SELECT DISTINCT	er.employer_id, 
				air.etl.ufnStripCharactersFromString(er.ein,@strip_characters,1,0), 
				 UPPER(air.etl.ufnStripCharactersFromString(er.name,@strip_characters, 1,0)),
				UPPER(air.etl.ufnStripCharactersFromString(er.[address],@strip_characters, 1,0)),
				UPPER(air.etl.ufnStripCharactersFromString(er.city,@strip_characters, 1,0)),
				UPPER(s.abbreviation),
				SUBSTRING(er.zip,1,5), 
				UPPER(air.etl.ufnStripCharactersFromString(u.fname,@strip_characters, 1,0)) AS contact_first_name, 
				UPPER(air.etl.ufnStripCharactersFromString(u.lname,@strip_characters, 1,1)) AS contact_last_name,
				UPPER(air.etl.ufnStripCharactersFromString(u.phone,@strip_characters,1,1)) AS  contact_telephone
FROM
	aca.dbo.employer er
	INNER JOIN aca.dbo.tax_year_approval tya ON (er.employer_id = tya.employer_id)
	INNER JOIN aca.dbo.[state] s ON (er.state_id = s.state_id)
	INNER JOIN aca.dbo.[user] u ON (er.employer_id = u.employer_id)
	LEFT OUTER JOIN ale.employer em ON (er.employer_id = em.employer_id)
WHERE
	(em.employer_id IS NULL)
		AND
	(er.employer_id = @employer_id)
		AND
	(u.irsContact = 1);
-- ______________________________________________________________________________________________________________________________________________________
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
INSERT INTO		air.ale.dge	(name, ein, [address], city, state_code, zipcode, contact_first_name, contact_last_name, contact_telephone) 
SELECT DISTINCT			UPPER(air.etl.ufnStripCharactersFromString(dge_name,@strip_characters,1,0)) AS name,
						air.etl.ufnStripCharactersFromString(dge_ein,@strip_characters,1,0), 
						UPPER(air.etl.ufnStripCharactersFromString(dge_address, @strip_characters,1,0)),
						UPPER(air.etl.ufnStripCharactersFromString(dge_city, @strip_characters,1,0)),
						s.abbreviation, 
						SUBSTRING(dge_zip,1,5), 
						UPPER(air.etl.ufnStripCharactersFromString(dge_contact_fname, @strip_characters,1,0)), 
						UPPER(air.etl.ufnStripCharactersFromString(dge_contact_lname, @strip_characters,1,0)), 
						UPPER(air.etl.ufnStripCharactersFromString(dge_phone,@strip_characters,1,1))
FROM
	aca.dbo.tax_year_approval tya
	INNER JOIN aca.dbo.[state] s ON (tya.state_id = s.state_id)
	LEFT OUTER JOIN air.ale.dge d ON (REPLACE(tya.dge_ein,'-','') = d.ein) 
WHERE
	(tya.employer_id = @employer_id)
		AND
	(d.ein IS NULL)
-- ______________________________________________________________________________________________________________________________________________________
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
UPDATE air.ale.employer 
		SET	ein = air.etl.ufnStripCharactersFromString(er.ein,@strip_characters,1,0),
			name = UPPER(air.etl.ufnStripCharactersFromString(er.name,@strip_characters, 1,0)),
			[address] = UPPER(air.etl.ufnStripCharactersFromString(er.[address],@strip_characters, 1,0)),
			city = UPPER(air.etl.ufnStripCharactersFromString(er.city,@strip_characters, 1,0)),
			state_code = UPPER(s.abbreviation),
			zipcode = SUBSTRING(er.zip,1,5),
			contact_first_name = UPPER(air.etl.ufnStripCharactersFromString(u.fname,@strip_characters, 1,0)), 
			contact_last_name= UPPER(air.etl.ufnStripCharactersFromString(u.lname,@strip_characters, 1,0)), 
			contact_telephone= UPPER(air.etl.ufnStripCharactersFromString(u.phone,@strip_characters, 1,1)),
			dge_ein = air.etl.ufnStripCharactersFromString(tya.dge_ein,@strip_characters, 1,0)
FROM
	aca.dbo.employer er
	INNER JOIN aca.dbo.tax_year_approval tya ON (er.employer_id = tya.employer_id)
	INNER JOIN aca.dbo.[state] s ON (er.state_id = s.state_id)
	INNER JOIN ale.employer er1  ON (er.employer_id = er1.employer_id)
	INNER JOIN  aca.dbo.[user] u ON (er.employer_id = u.employer_id) 
WHERE
	(er.employer_id = @employer_id)
		AND
	(u.irsContact = 1) 
-- ______________________________________________________________________________________________________________________________________________________
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
ALTER PROCEDURE [etl].[spUpdate_ale_dge]
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
PRINT '2A: Update Dge';
UPDATE		air.ale.dge	SET	name = UPPER(air.etl.ufnStripCharactersFromString(dge_name,@strip_characters,1,0)), 
							[address] =  UPPER(air.etl.ufnStripCharactersFromString(dge_address,@strip_characters,1,0)), 
							city =  UPPER(air.etl.ufnStripCharactersFromString(dge_city,@strip_characters,1,0)), 
							state_code = s.abbreviation, 
							zipcode = SUBSTRING(dge_zip,1,5),
							contact_first_name = UPPER(air.etl.ufnStripCharactersFromString(dge_contact_fname,@strip_characters,1,0)), 
							contact_last_name = UPPER(air.etl.ufnStripCharactersFromString(dge_contact_lname,@strip_characters,1,0)), 
							contact_telephone = UPPER(air.etl.ufnStripCharactersFromString(dge_phone,@strip_characters,1,1))
FROM
	air.ale.dge d
	INNER JOIN air.ale.employer er ON(d.ein = er.dge_ein)	
	INNER JOIN aca.dbo.tax_year_approval tya ON (er.employer_id = tya.employer_id)
	INNER JOIN aca.dbo.[state] s ON (tya.state_id = s.state_id) 
WHERE
	(tya.employer_id = @employer_id)
-- ______________________________________________________________________________________________________________________________________________________
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
ALTER PROCEDURE [etl].[spInsert_employee]
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
PRINT '3: Insert Employee'
	INSERT INTO air.emp.employee 
			(employee_id, employer_id, first_name, last_name, [address], 
				city, state_code, zipcode, telephone, ssn)
	SELECT DISTINCT	
			ee.employee_id, ee.employer_id, 
			UPPER(air.etl.ufnStripCharactersFromString(ee.fName,@strip_characters,1,0)), 
			UPPER(air.etl.ufnStripCharactersFromString(ee.lName,@strip_characters,1,0)),
			UPPER(air.etl.ufnStripCharactersFromString(ee.[address],@strip_characters,1,0)),
			UPPER(air.etl.ufnStripCharactersFromString(ee.city,@strip_characters,1,0)),
			UPPER(s.abbreviation), 
			UPPER(air.etl.ufnStripCharactersFromString(SUBSTRING(ee.zip,1,5),@strip_characters,1,0)), 
			UPPER(air.etl.ufnStripCharactersFromString(contact_telephone,@strip_characters,1,1)), 
			ee.ssn
	FROM
		aca.dbo.employee ee
		INNER JOIN air.ale.employer er ON (ee.employer_id = er.employer_id)
		INNER JOIN aca.dbo.state s ON (ee.state_id = s.state_id)
		LEFT OUTER JOIN air.emp.employee emp ON (ee.employee_id = emp.employee_id)
	WHERE
		(er.employer_id = @employer_id)
			AND
		(emp.employee_id IS NULL) 
			AND
		-- note: this will break when there is more than 10 million lives in the database.
		(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
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
UPDATE air.emp.employee  
		SET employer_id = ee.employer_id, 
			first_name = UPPER(air.etl.ufnStripCharactersFromString(ee.fName,@strip_characters,1,0)), 
			last_name = UPPER(air.etl.ufnStripCharactersFromString(ee.lName,@strip_characters,1,0)), 
			[address] = UPPER(air.etl.ufnStripCharactersFromString(ee.[address],@strip_characters,1,0)), 
			city = UPPER(air.etl.ufnStripCharactersFromString(ee.city,@strip_characters,1,0)), 
			state_code = UPPER(s.abbreviation), 
			zipcode = UPPER(air.etl.ufnStripCharactersFromString(SUBSTRING(ee.zip,1,5),@strip_characters,1,0)), 
			ssn = ee.ssn,
			telephone = UPPER(air.etl.ufnStripCharactersFromString(contact_telephone,@strip_characters,1,1))
FROM
	aca.dbo.employee ee
	INNER JOIN air.ale.employer er ON (ee.employer_id = er.employer_id)
	INNER JOIN air.emp.employee em ON (ee.employer_id = em.employer_id) AND (ee.employee_id = em.employee_id)
	INNER JOIN aca.dbo.state s ON (ee.state_id = s.state_id)
WHERE
	(ee.employer_id = @employer_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
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
DELETE air.emp.covered_individual 
FROM
	air.emp.covered_individual ci 
	INNER JOIN air.emp.employee ee ON (ci.employee_id = ee.employee_id)
WHERE
	(ee.employer_id = @employer_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '5: Insert Covered Individuals'
INSERT INTO air.emp.covered_individual 
		(covered_individual_id, employee_id, employer_id, first_name, middle_name, last_name, ssn, birth_date)
SELECT	DISTINCT first_row_id, employee_id, employer_id,
			UPPER(air.etl.ufnStripCharactersFromString(fName,@characters_to_strip,1,0)), 
			UPPER(air.etl.ufnStripCharactersFromString(mName,@characters_to_strip,1,0)), 
			UPPER(air.etl.ufnStripCharactersFromString(lName,@characters_to_strip,1,0)), 
			ssn, IIF(ssn IS NULL, dob, NULL)
FROM
	aca.dbo.ufnGetEmployeeInsurance(@year_id) ei
WHERE
	(employer_id = @employer_id)
		AND
	(ISNULL(ssn,dob) IS NOT NULL)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000))
-- ______________________________________________________________________________________________________________________________________________________
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
DELETE air.emp.covered_individual_monthly_detail 
FROM
	air.emp.covered_individual_monthly_detail cim
	INNER JOIN air.emp.covered_individual ci ON (cim.covered_individual_id = ci.covered_individual_id) 
	INNER JOIN air.emp.employee ee ON (ci.employee_id = ee.employee_id)
WHERE
	(time_frame_id IN(SELECT time_frame_id FROM air.gen.time_frame WHERE year_id = @year_id))
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
					aca.dbo.ufnGetEmployeeInsurance(@year_id) ic
					INNER JOIN aca.dbo.employee ee ON (ic.employee_id = ee.employee_id)
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

INSERT INTO air.emp.covered_individual_monthly_detail (
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
	INNER JOIN air.gen.[month] m ON (SUBSTRING(ic.[month],1,3) = SUBSTRING(m.name,1,3))
	INNER JOIN air.gen.time_frame t ON (ic.tax_year = t.year_id) AND (m.month_id = t.month_id)
WHERE
	first_row_id IN (
			SELECT DISTINCT covered_individual_id FROM air.emp.covered_individual
		);

-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

PRINT '6: Update Covered Individual Annual';
UPDATE emp.covered_individual
SET
	annual_coverage_indicator = cim.annual_coverage_indicator 
FROM
	emp.covered_individual ci
	INNER JOIN  air.emp.employee ee ON (ci.employee_id = ee.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				cim.covered_individual_id,
				MAX(CAST(cim.covered_indicator AS INT)) AS annual_coverage_indicator
			FROM
				air.emp.covered_individual_monthly_detail cim
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/8/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
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
			ee.classification_id,
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
			--: Is the employee Not Yet Hired?
			WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ey.hireDate), MONTH(ey.hireDate)),0) > ey.time_frame_id THEN 5
			--: Is employee In Initial Measurement?
			WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ey.initialMeasurmentEnd), MONTH(ey.initialMeasurmentEnd)), 0) >= ey.time_frame_id THEN 3
			--: Is employee In Administrative Period>
			WHEN ISNULL(
				air.etl.ufnGetTimeFrameID(
					IIF(
						YEAR(ey.hireDate) = @year_id,
						YEAR(ey.hireDate), @year_id + 1), 
						MONTH(ey.hireDate)
					),
				0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@year_id), MONTH(ey.hireDate)) AND ey.time_frame_id THEN 6
			--: Does the employee have monthly hours?
			WHEN ISNULL(mh.monthly_hours,0) = 0 THEN 7
			--: Does the employee measure full-time according to hours?
			WHEN mh.monthly_hours > 129.99 THEN 1
			--: Does the employee measure part-time according to hours?
			WHEN mh.monthly_hours < 130 THEN 2
		END AS monthly_status_id, 
		ey.hireDate,
		ey.terminationDate,
		ey.initialMeasurmentEnd,
		mi.slcmp AS share_lowest_cost_monthly_premium, 
		CASE
			WHEN offeredOn IS NOT NULL THEN 1 ELSE 0
		END AS offered_coverage,
		ISNULL(accepted, 0) AS enrolled,
		offeredOn,
		acceptedOn,
		effectiveDate,
		mi.minValue,
		mi.offSpouse,
		mi.offDependent,
		mi.insurance_type_id, 
		contribution_id,
		ec.ash_code,
		on_payroll,
		aca_status_id,
		mi.monthly_flex
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
		mi.monthly_flex
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
	air.etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id) AS offer_of_coverage_code,
	minValue, 
	IIF(air.etl.ufnGetMonthFromTimeFrame(time_frame_id) >= MONTH(terminationDate), NULL, share_lowest_cost_monthly_premium), 
	air.etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code,
	enrolled,
	monthly_status_id,
	insurance_type_id,
	monthly_flex
FROM
	employees_monthly_aggregates ema
-- ______________________________________________________________________________________________________________________________________________________
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
ALTER PROCEDURE [etl].[spUpdate_1095C_status]
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
--:None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
UPDATE
	air.emp.yearly_detail
SET
	submittal_ready = 0,
	_1095C = 0
FROM
	air.emp.yearly_detail yd
WHERE
	(yd.employer_id = @employer_id)
		AND
	(yd.year_id = @year_id)
		AND 
	-- note: this will break when there is more than 10 million lives in the database.
	(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

UPDATE
	air.emp.yearly_detail
SET
	submittal_ready = 1,
	_1095C = 1
FROM
	air.emp.yearly_detail yd
WHERE
	yd.employee_id IN (
			SELECT DISTINCT
				employee_id
			FROM
				air.emp.monthly_detail md 
			WHERE
				((monthly_status_id = 1)
					OR
				(insurance_type_id = 2 AND enrolled = 1))
					AND
				(md.employee_id = employee_id)
		)
		OR
	yd.employee_id IN (
			SELECT DISTINCT
				employee_id
			FROM
				air.emp.monthly_detail
			WHERE
				(enrolled = 0)
					AND
				(employee_id IN (
						SELECT DISTINCT
							employee_id
						FROM
							air.emp.covered_individual
					)
				)
		)
		AND 
	(yd.employer_id = @employer_id)
		AND
	(yd.year_id = @year_id)
		AND 
	-- note: this will break when there is more than 10 million lives in the database.
	(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
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
		ISNULL(enr.enrolled,0) AS enrolled, 
		ISNULL(it1.insurance_type_id,0) + ISNULL(it2.insurance_type_id,0) AS insurance_type_id,
		IIF((it2.employee_id IS NOT NULL) OR (it3.employee_id IS NOT NULL),1,0) AS must_supply_ci_info
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
				GROUP BY emd.employee_id, t.year_id
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
				FROM air.emp.monthly_detail 
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
				FROM air.emp.monthly_detail
				WHERE
					(insurance_type_id IN(1,2))
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

PRINT '12: Update Appr Employee Yearly Detail Part-Time 1G'; --:TO DO: MOVE THIS INTO THE INSERT STATEMENT.
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
	eyd.employee_id NOT IN (
			SELECT DISTINCT
				employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(monthly_status_id IN(1))
					AND
				(emd.employer_id = @employer_id)
		)
		AND
	eyd.employee_id IN(
			SELECT DISTINCT
				employee_id
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

PRINT '12: Update Appr Employee Monthly Detail Part-Time 1G ACTION COMMENTED OUT CURRENTLY UNTIL DECISION TO OVERWRITE INFORMATION IS MADE' 
--UPDATE air.emp.monthly_detail
--			SET share_lowest_cost_monthly_premium = NULL,
--				safe_harbor_code = NULL
--FROM	air.emp.monthly_detail emd
--WHERE	emd.employee_id NOT IN(SELECT DISTINCT employee_id
--							FROM air.emp.monthly_detail emd 
--							WHERE (monthly_status_id IN(1)) AND (emd.employer_id = @employer_id))
--			AND emd.employee_id IN(SELECT DISTINCT employee_id
--									FROM air.emp.monthly_detail emd 
--									WHERE (insurance_type_id = 2) AND (emd.employer_id = @employer_id))AND
--		(emd.employer_id = @employer_id) AND
--		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

PRINT '12: Update Employee _1095C Status';
EXECUTE air.etl.spUpdate_1095C_status @employer_id, @year_id, @employee_id
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
	air.ale.monthly_detail
WHERE
	(employer_id = @employer_id)
		AND
	(time_frame_id IN(SELECT time_frame_id FROM air.gen.time_frame WHERE year_id = @year_id));

-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '9: Insert Appr Ale Monthly Detail'
INSERT INTO
	air.ale.monthly_detail (
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
	air.emp.monthly_detail emd
	INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN air.emp.monthly_detail tec
		ON
			(emd.employee_id = tec.employee_id)
				AND
			(emd.time_frame_id = tec.time_frame_id)
				AND
			tec.monthly_status_id IN (1,2,3,4,6)
	LEFT OUTER JOIN air.emp.monthly_detail fte
		ON
			(emd.employee_id = fte.employee_id)
				AND
			(emd.time_frame_id = fte.time_frame_id)
				AND
			fte.monthly_status_id = 1
	LEFT OUTER JOIN air.emp.monthly_detail oec
		ON
			(emd.employee_id = oec.employee_id)
				AND
			(emd.time_frame_id = oec.time_frame_id)
				AND
			oec.monthly_status_id IN (2,3,4,6)
	LEFT OUTER JOIN air.emp.monthly_detail pte
		ON
			(emd.employee_id = pte.employee_id)
				AND
			(emd.time_frame_id = pte.time_frame_id)
				AND
			pte.monthly_status_id = 2
	LEFT OUTER JOIN air.emp.monthly_detail ft_cov
		ON
			(t.time_frame_id = ft_cov.time_frame_id)
				AND
			(emd.employee_id = ft_cov.employee_id)
				AND
			ft_cov.mec_offered = 1
				AND
			ft_cov.monthly_status_id = 1
	LEFT OUTER JOIN air.emp.monthly_detail pt_cov
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
	air.ale.yearly_detail
WHERE
	(employer_id = @employer_id)
			AND
	(year_id = @year_id);

-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '10: Insert Ale Yearly Detail'
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
	INNER JOIN air.gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employer_id ,
				MAX(total_employee_count) AS total_employee_count,
				t.year_id
			FROM
				air.ale.monthly_detail amd WITH (NOLOCK)
				INNER JOIN air.gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
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
				INNER JOIN air.gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
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
				INNER JOIN air.gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
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
				INNER JOIN air.gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
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
				INNER JOIN air.gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
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
				INNER JOIN air.gen.time_frame t ON (amd.time_frame_id = t.time_frame_id)
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
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/8/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
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
			mi.monthly_flex
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
			mi.monthly_flex
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
	air.etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id) AS offer_of_coverage_code,
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
	employees_monthly_aggregates ema
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

PRINT '12: Update Appr Employee Yearly Detail Part-Time 1G'; --:TO DO: MOVE THIS INTO THE INSERT STATEMENT.
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
	eyd.employee_id NOT IN (
			SELECT DISTINCT
				employee_id
			FROM
				air.emp.monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		eyd.employee_id IN (
				SELECT DISTINCT
					employee_id
				FROM
					air.emp.monthly_detail emd 
				WHERE
					(insurance_type_id =2)
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

PRINT '12: Update Appr Employee Monthly Detail Part-Time 1G ACTION COMMENTED OUT CURRENTLY UNTIL DECISION TO OVERWRITE INFORMATION IS MADE' 
--UPDATE air.emp.monthly_detail
--			SET share_lowest_cost_monthly_premium = NULL,
--				safe_harbor_code = NULL
--FROM	air.emp.monthly_detail emd
--WHERE	emd.employee_id NOT IN(SELECT DISTINCT employee_id
--							FROM air.emp.monthly_detail emd 
--							WHERE (monthly_status_id IN(1)) AND (emd.employer_id = @employer_id))
--			AND emd.employee_id IN(SELECT DISTINCT employee_id
--									FROM air.emp.monthly_detail emd 
--									WHERE (insurance_type_id = 2) AND (emd.employer_id = @employer_id))AND
--		(emd.employer_id = @employer_id) AND
--		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

PRINT '12: Update Employee _1095C Status';
EXECUTE air.etl.spUpdate_1095C_status @employer_id, @year_id, @employee_id
GO
-- ______________________________________________________________________________________________________________________________________________________
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
DELETE air.appr.employee_monthly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(time_frame_id IN(SELECT time_frame_id FROM air.gen.time_frame WHERE year_id = @year_id))
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '11: Insert Appr Employee Monthly Detail';

INSERT INTO air.appr.employee_monthly_detail (
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
	air.emp.monthly_detail emd 
WHERE
	(emd.employer_id = @employer_id)
		AND
	(emd.time_frame_id IN(SELECT time_frame_id FROM air.gen.time_frame WHERE year_id = @year_id))
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
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
DELETE air.appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Insert Appr Employee Yearly Detail';
INSERT INTO air.appr.employee_yearly_detail (
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
	air.appr.employee_monthly_detail emd 
	INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.offer_of_coverage_code) AS offer_of_coverage_code,
				t.year_id
			FROM
				air.appr.employee_monthly_detail emd 
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
				air.appr.employee_monthly_detail emd 
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
				air.appr.employee_monthly_detail emd 
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
				air.appr.employee_monthly_detail 
			WHERE
				enrolled = 1
			) enr ON (emd.employee_id = enr.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
			FROM
				air.appr.employee_monthly_detail
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
				air.appr.employee_monthly_detail 
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
				air.appr.employee_monthly_detail
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
	(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

PRINT '12: Update Appr Employee Yearly Detail Part-Time 1G';
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
	eyd.employee_id IN (
		SELECT DISTINCT
			emd.employee_id
		FROM
			air.appr.employee_monthly_detail emd 
		WHERE
			(insurance_type_id =2)
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

PRINT '12: Update Appr Employee Monthly Detail Part-Time 1G ACTION COMMENTED OUT CURRENTLY UNTIL DECISION TO OVERWRITE INFORMATION IS MADE' 
--UPDATE air.appr.employee_monthly_detail
--			SET share_lowest_cost_monthly_premium = NULL,
--				safe_harbor_code = NULL
--FROM	air.appr.employee_monthly_detail emd
--WHERE	emd.employee_id NOT IN(SELECT DISTINCT employee_id
--							FROM air.appr.employee_monthly_detail emd 
--							WHERE (monthly_status_id IN(1)) AND (emd.employer_id = @employer_id))
--			AND emd.employee_id IN(SELECT DISTINCT employee_id
--									FROM air.appr.employee_monthly_detail emd 
--									WHERE (insurance_type_id = 2) AND (emd.employer_id = @employer_id))AND
--		(emd.employer_id = @employer_id) AND
--		(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

PRINT '12: Update Employee _1095C Status';
EXECUTE air.appr.spUpdate_1095C_status @employer_id, @year_id, @employee_id
GO
-- ______________________________________________________________________________________________________________________________________________________
GO

-- must be last:!
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
--:WHEN NEEDED FOR TESTING
--DECLARE @employer_id INT=11
--DECLARE @year_id SMALLINT = 2015
--DECLARE @employee_id INT = NULL
--DECLARE @aag_indicator BIT = 0
--DECLARE @aag_code TINYINT = 2
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @ErrorMessage NVARCHAR(125)
DECLARE @dge_ein NCHAR(10)
DECLARE @_4980H_transition_relief_indicator BIT = 1
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
SELECT  @dge_ein = dge_ein, @_4980H_transition_relief_indicator = safeHarbor 
FROM	aca.dbo.tax_year_approval 
WHERE	(employer_id = @employer_id) AND (tax_year = @year_id)
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
			EXECUTE air.appr.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '12*** End Insert Appr Employee Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE aca.dbo.spUpdateAIR @employer_id, @year_id
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
-- ______________________________________________________________________________________________________________________________________________________
GO
