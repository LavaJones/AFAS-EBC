USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
      @employerID int
AS
BEGIN TRY

--Plan Year Archives/Insurance Offers
DELETE
  dbo.employee_insurance_offer_archive
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  dbo.employee_insurance_offer
  where employer_id=@employerID;
--All Carrier Import Coverage

DELETE
  dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--
DELETE
      dbo.insurance_coverage_editable
      where employer_id=@employerID;

--Insurance Carrier Import Alerts
DELETE
  dbo.import_insurance_coverage
  WHERE employer_id=@employerID;

--All Measurement Periods. 
CREATE TABLE #entityValue (value integer)

DELETE dbo.EmployeeInsuranceOfferEditable WHERE EmployerId = @employerID

INSERT INTO #entityValue (value)
SELECT BreakInServiceId
FROM dbo.measurementBreakInService
WHERE measurement_id IN (SELECT measurement_id FROM [dbo].[measurement] WHERE employer_id = @employerId)

--Measurement Break In Service
DELETE dbo.measurementBreakInService WHERE measurement_id IN (SELECT measurement_id FROM [dbo].[measurement] WHERE employer_id = @employerId)

--Break in Service
UPDATE dbo.BreakInService SET EntityStatusId = 3 WHERE BreakInServiceId IN (SELECT Value FROM #entityValue)

DROP TABLE #entityValue

--Insurance Contributions
DELETE
dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

-- Tax Year
DELETE
      dbo.tax_year_1095c_approval
      where employer_id=@employerID;

--Payroll Summer Averages. 
DELETE
  dbo.payroll_summer_averages
  where employer_id=@employerID;

--All Payroll. 
DELETE 
  dbo.payroll
  WHERE employer_id=@employerID
  
--Payroll Import Alerts. Alerts that have been deleted by users. 
DELETE dbo.payroll_archive
      WHERE employer_id=@employerID

-- dependents
DELETE 
      dbo.employee_dependents
      WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

-- must clear out average hours before clearing measurement period 
DELETE dbo.EmployeeMeasurementAverageHours where MeasurementId in (Select measurement_id from dbo.measurement WHERE employer_id = @employerId);

--Measurement 
DELETE dbo.measurement WHERE employer_id = @employerId;

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--All Employees.
DELETE  
  FROM dbo.employee
  WHERE employer_id=@employerID
  
--All Gross Pay Filters
DELETE
  dbo.gross_pay_filter
  WHERE employer_id=@employerID

--Employee Alert Archives. Alerts that have been deleted by users. 
DELETE dbo.alert_archive
      WHERE employer_id=@employerID

--All Gross Pay Codes
DELETE
  dbo.gross_pay_type
  WHERE employer_id=@employerID

--All Plan Years. 
DELETE
  dbo.plan_year
  WHERE employer_id=@employerID

  -- tax year approval
DELETE
  dbo.tax_year_approval
  WHERE employer_id=@employerID

--All Batch rows. 
DELETE
  dbo.batch
  WHERE employer_id=@employerID

  -- Theses are not referenced at all in the spreadsheet
  
--Equivalencies
DELETE 
  dbo.equivalency
  where employer_id=@employerID;

--Employee Import Alerts. 
DELETE
  dbo.import_employee
  WHERE employerID=@employerID
  
--Payroll Import Alerts. 
DELETE
  dbo.import_payroll
  WHERE employerid=@employerID

--All HR Status Codes
DELETE
  dbo.hr_status
  WHERE employer_id=@employerID

END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
