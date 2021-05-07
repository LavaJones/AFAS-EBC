USE [air-demo]
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
			MAX(CAST(ISNULL(ei.hra_flex_contribution,0)AS DECIMAL(10,2))) AS monthly_flex,
			i.monthlycost,
			ee.terminationDate,
			ee.hireDate,
			i.minValue,
			i.offSpouse,
			i.offDependent, 
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
			(py.startDate BETWEEN '2016-01-01 00:00:00' AND '2016-12-31 23:59:59')
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
--__________________________________________________________________________________________________________________________________________________________________


GO
