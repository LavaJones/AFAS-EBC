select
	ee.employee_id,
	ee.employer_id,
	ee.fName,
	ee.lName,
	ee.hireDate,
	ee.terminationDate,
	ee.initialMeasurmentEnd,
	aas.name as ACAStatusName,
	emah.MonthlyAverageHours,
	emah.TrendingMonthlyAverageHours,
	emah.MeasurementId,
	emah.IsNewHire,
	py.startDate as PlanYearStartDate,
	py.endDate as PlanYearEndDate,
	m.meas_start as MeasurementStartDate,
	m.meas_end as MeasurementEndDate,
	im.months
from
	employee ee
	INNER JOIN aca_status aas on (aas.aca_status_id = ee.aca_status_id)
	INNER JOIN EmployeeMeasurementAverageHours emah on (emah.EmployeeId = ee.employee_id)
	INNER JOIN measurement m on (m.measurement_id = emah.MeasurementId)
	INNER JOIN plan_year py on (py.plan_year_id = m.plan_year_id)
	INNER JOIN employer er on (ee.employer_id = er.employer_id)
	INNER JOIN initial_measurement im on (im.initial_measurement_id = er.initial_measurement_id)
where
	--Filter to only Plan Years in the time period 
	((py.startDate >= '2016-01-01' AND py.startDate <= '2016-12-31') OR (py.endDate >= '2016-01-01' AND py.endDate <= '2016-12-31'))
	-- Filter out people termed before this time period
		AND
	(
	ee.terminationDate is NULL -- not termed
		OR
	ee.terminationDate > '2016-01-01' -- Per Susan, if they are termed before the start of the reporting period we dont have to report on them. 
	)
	--Filter outFT since we are only looking for IMP
		AND
	(aas.aca_status_id != 5) -- then we exclude them
-- We need to discuss this line a tad before putting it in -- AND aas.aca_status_id != 4 AND aas.aca_status_id != 8) -- NOT Retiree or COBRA

	--And filter to Ones where the IMP Stability Period overlaps with the Reporting period 	
		AND
	(ee.initialMeasurmentEnd <= py.startDate 
			AND 
		(
			( -- try to derive the IMP Stability Start Date and see if they overlap the reporting period
			DATEADD(month, 13, ee.hireDate) >= '2016-01-01'-- 13 months for IMP + admin, but this is wrong for 6 Month MP
				AND
			DATEADD(month, 13, ee.hireDate) <= '2016-12-31'
			)
				OR
			(-- try to derive the IMP Stability End Date and see if they overlap the reporting period
			DATEADD(month, 25, ee.hireDate) >= '2016-01-01'-- 25 because 13 months for IMP + admin and 12 Months for Stability lenght, but this is wrong
				AND
			DATEADD(month, 25, ee.hireDate) <= '2016-12-31'
			)
		)
	)
	-- We only want to look at the IMP hours, so we filter on New Hire
		AND 
	emah.IsNewHire = 1


	--I'm a little uncertain about this one.
