USE [aca]
GO

/****** Object:  View [dbo].[View_1094Part3]    Script Date: 3/12/2018 4:22:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









ALTER VIEW [dbo].[View_1094Part3]
AS


   
SELECT 
       count(*) as NoOfEmployees, 
       EmployerId, 
       Receiving1095C,
       TaxYear, 
	   OfferedInsurance , 
       MonthId  
from
(
       select distinct
                      f.employeeID as EmployeeID,
                      f.employerID as EmployerId,
                     CASE 
					When p2.Receiving1095C < mp.Receiving1095C Then cast(mp.Receiving1095C as bit)
					when p2.Receiving1095C < all12.Receiving1095C Then cast(all12.Receiving1095C as bit)
					Else cast(p2.Receiving1095C as bit)
			End AS Receiving1095C, 
                      p2.TaxYear,
                      CASE
						WHEN p2.Line14 = '1A' Then 1 
						WHEN p2.Line14 = '1C' Then 1
						WHEN p2.Line14 = '1E' Then 1
						WHEN p2.Line14 = '1F' Then 1
						WHEN p2.Line14 = '1H' and p2.line16 = '2E' then 1 
                        when p2.Offered < all12.Offered Then all12.Offered
                        Else p2.Offered
                      End AS OfferedInsurance,
                      p2.MonthId

       FROM 
              [aca].[dbo].[Approved1095FinalPart2] p2
       join 
              [aca].[dbo].[Approved1095Final] f 
              on p2.Approved1095Final_ID=f.Approved1095FinalId  


       left JOIN 
              (
              select 
                      employee_id,
                      Receiving1095C, 
                      tax_year, 
                      month_id
              from 
                      (
                      SELECT 
                             employer_id,
                             employee_id,
                             terminationDate,
							 Case 
                                When terminationDate <> null and PeriodStartDate > terminationDate Then terminationDate 
                                Else PeriodStartDate
                             End AS PeriodStartDate,
                            -- PeriodStartDate,
                            Case 
                                When terminationDate <> null and PeriodEndDate > terminationDate Then terminationDate 
                                Else PeriodEndDate
                             End AS PeriodEndDate,

                             TypeOfPeriod
                      FROM 
                             (
                                    select distinct
                                           dbo.employer.employer_id,
                                           dbo.employee.employee_id,
										   dbo.employee.terminationDate,
                                           dbo.employee.hireDate AS PeriodStartDate,
                                           Case 
                                                  When dbo.initial_measurement.months <= 11 Then DATEADD(month, 2, dbo.employee.initialMeasurmentEnd) 
                                                  Else DATEADD(month, 1, dbo.employee.initialMeasurmentEnd)
                                           End AS PeriodEndDate,
                                           'IMP' as TypeOfPeriod
                                    From
                                           dbo.employer          
                                    INNER JOIN
                                           [dbo].[initial_measurement]
                                           ON dbo.initial_measurement.initial_measurement_id = dbo.employer.initial_measurement_id
                                    INNER JOIN
                                           dbo.employee
                                           ON dbo.employee.employer_id = dbo.employer.employer_id 
                                    Where 
                                           aca_status_id = 5

                             UNION

                                    select distinct
                                           dbo.employer.employer_id,
                                           dbo.employee.employee_id,
										   dbo.employee.terminationDate,
                                           Case 
                                                  When dbo.initial_measurement.months <= 11 Then DATEADD(month, 2, dbo.employee.initialMeasurmentEnd) 
                                                  Else DATEADD(month, 1, dbo.employee.initialMeasurmentEnd)
                                           End AS InitialStabilityPeriodStartDate, 
                                           Case 
                                                  When dbo.initial_measurement.months <= 11 Then DATEADD(month, (dbo.initial_measurement.months + 2), dbo.employee.initialMeasurmentEnd) 
                                                  Else DATEADD(month, 13, dbo.employee.initialMeasurmentEnd)
                                           End AS InitialStabilityPeriodEndDate,
                                           'ISP' as TypeOfPeriod
                                    From
                                           dbo.employer          
                                    INNER JOIN
                                           [dbo].[initial_measurement]
                                           ON dbo.initial_measurement.initial_measurement_id = dbo.employer.initial_measurement_id
                                    INNER JOIN
                                           dbo.employee
                                           ON dbo.employee.employer_id = dbo.employer.employer_id 
                                    INNER JOIN
                                           [dbo].[EmployeeMeasurementAverageHours] 
                                           ON 
                                                  dbo.EmployeeMeasurementAverageHours.EmployeeId = dbo.employee.employee_id
                                                  And
                                                  dbo.EmployeeMeasurementAverageHours.IsNewHire = 1 
                                                  And 
                                                  dbo.EmployeeMeasurementAverageHours.MonthlyAverageHours >= 130.0
                                    WHERE 
                                           dbo.EmployeeMeasurementAverageHours.EntityStatusId = 1) as a
                             ) as impisp
                      inner join 
                             (
                                    select 1 as Receiving1095C, tax_year, month_id, DATEFROMPARTS(tax_year, month_id, 1) as tym from [dbo].[tax_year_month] CROSS JOIN [dbo].[tax_year]
                             ) as tyme
                             ON CAST(impisp.PeriodEndDate AS DATE) >= CAST(tyme.tym AS DATE) AND CAST(impisp.PeriodStartDate AS DATE) <= CAST(tyme.tym AS DATE)
       ) as mp 
       ON 
              mp.employee_id = p2.employeeID
              and mp.tax_year = p2.TaxYear
              and mp.month_id = p2.MonthId

       join
       (
              -- Select the all 12 into individual months and join them in
              select 
              p2.employeeID, 
              p2.TaxYear, month_id, 
              p2.Receiving1095C, 
              p2.Offered 
              from 
                      [aca].[dbo].[Approved1095FinalPart2] p2
              join 
                      [aca].[dbo].[Approved1095Final] f 
                      on p2.Approved1095Final_ID=f.Approved1095FinalId  
              CROSS JOIN 
                      [dbo].[tax_year_month] mon
              where 
                      not (p2.Line14 = '1H' and p2.Line16 = '2D') and
					  not (p2.line14 = '1B') and not (p2.line14 = '1J') and
					  not (p2.Line14 = '1D') and
					  not (p2.Line14 = '1H' and p2.Line16 = '2B') and
					  not (p2.Line14 = '1H' and p2.Line16 = '2A') and   
                      p2.MonthId = 0
                      and 
                      p2.EntityStatusId=1 
                      and 
                      f.EntityStatusId=1
       ) as all12
       ON 
              all12.employeeID = p2.employeeID
              and all12.TaxYear = p2.TaxYear
              and all12.month_id = p2.MonthId
       where 
              not (p2.Line14 = '1H' and p2.Line16 = '2D') and 
			  not (p2.Line14 = '1H' and p2.Line16 = '2B') and
			  not (p2.Line14 = '1H' and p2.Line16 = '2A') and 
			  not (p2.Line14 = '1H' and P2.Line16 = '2E') and
              p2.MonthId <> 0
              and 
              p2.EntityStatusId=1 
              and 
              f.EntityStatusId=1
) as stuffs
where Receiving1095C = 1
group by EmployerID, TaxYear, MonthId, Receiving1095C, OfferedInsurance













GO


