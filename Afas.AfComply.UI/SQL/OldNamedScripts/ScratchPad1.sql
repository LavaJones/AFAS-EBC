USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[RESET_EMPLOYER]    Script Date: 9/14/2016 3:41:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
      @employerID int
AS

BEGIN TRY
      --Equivalencies
BEGIN TRAN
      SET ROWCOUNT 1000
      delete1:

      DELETE dbo.equivalency
            where employer_id=@employerID

      IF @@ROWCOUNT > 0 GOTO delete1
      SET ROWCOUNT 0
COMMIT TRAN


--Plan Year Archives/Insurance Offers
BEGIN TRAN
SET ROWCOUNT 1000
      delete2:
DELETE
  dbo.employee_insurance_offer
  where employer_id=@employerID;
IF @@ROWCOUNT > 0 GOTO delete2
SET ROWCOUNT 0
COMMIT TRAN

--Plan Year Archives/Insurance Offers
BEGIN TRAN
SET ROWCOUNT 1000
      delete3:
DELETE
  dbo.employee_insurance_offer_archive
  where employer_id=@employerID;
IF @@ROWCOUNT > 0 GOTO delete3
SET ROWCOUNT 0
COMMIT TRAN


--Insurance Contributions
BEGIN TRAN
SET ROWCOUNT 1000
      delete5:
DELETE
dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));
IF @@ROWCOUNT > 0 GOTO delete5
SET ROWCOUNT 0
COMMIT TRAN


--Insurance Plans
BEGIN TRAN
SET ROWCOUNT 1000
      delete6:
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);
IF @@ROWCOUNT > 0 GOTO delete6
SET ROWCOUNT 0
COMMIT TRAN


--Payroll Summer Averages.
BEGIN TRAN
SET ROWCOUNT 1000
      delete7:
DELETE
  dbo.payroll_summer_averages
  where employer_id=@employerID;
IF @@ROWCOUNT > 0 GOTO delete7
SET ROWCOUNT 0
COMMIT TRAN 


--Employee Import Alerts.
BEGIN TRAN
SET ROWCOUNT 1000
      delete8:
DELETE
  dbo.import_employee
  WHERE employerID=@employerID
IF @@ROWCOUNT > 0 GOTO delete8
SET ROWCOUNT 0
COMMIT TRAN 


--Employee Alert Archives. Alerts that have been deleted by users.
BEGIN TRAN
SET ROWCOUNT 1000
      delete9:
DELETE dbo.alert_archive
      WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete9
SET ROWCOUNT 0
COMMIT TRAN 


--Payroll Import Alerts. Alerts that have been deleted by users.
BEGIN TRAN
SET ROWCOUNT 1000
      delete10:
DELETE dbo.payroll_archive
      WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete10
SET ROWCOUNT 0
COMMIT TRAN 


--Payroll Import Alerts.
BEGIN TRAN
SET ROWCOUNT 1000
      delete11:
DELETE
  dbo.import_payroll
  WHERE employerid=@employerID
IF @@ROWCOUNT > 0 GOTO delete11
SET ROWCOUNT 0
COMMIT TRAN 


--Insurance Carrier Import Alerts
BEGIN TRAN
SET ROWCOUNT 1000
      delete12:
DELETE
  dbo.import_insurance_coverage
  WHERE employer_id=@employerID;
IF @@ROWCOUNT > 0 GOTO delete12
SET ROWCOUNT 0
COMMIT TRAN


--All Carrier Import Coverage
BEGIN TRAN
SET ROWCOUNT 1000
      delete13:
DELETE
  dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);
IF @@ROWCOUNT > 0 GOTO delete13
SET ROWCOUNT 0
COMMIT TRAN


--All Payroll. 
BEGIN TRAN
SET ROWCOUNT 1000
      delete14:
DELETE 
  [aca].[dbo].[payroll]
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete14
SET ROWCOUNT 0
COMMIT TRAN

--All Employees.
BEGIN TRAN
SET ROWCOUNT 1000
      delete15:
DELETE  
  FROM dbo.employee
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete15
SET ROWCOUNT 0
COMMIT TRAN

--All Employee dependents.
BEGIN TRAN
SET ROWCOUNT 1000
      delete4:
DELETE 
      dbo.employee_dependents
      WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);
IF @@ROWCOUNT > 0 GOTO delete4
SET ROWCOUNT 0
COMMIT TRAN

--All HR Status Codes
BEGIN TRAN
SET ROWCOUNT 1000
      delete16:
DELETE
  dbo.hr_status
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete16
SET ROWCOUNT 0
COMMIT TRAN


--All Gross Pay Filters
BEGIN TRAN
SET ROWCOUNT 1000
      delete17:
DELETE
  dbo.gross_pay_filter
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete17
SET ROWCOUNT 0
COMMIT TRAN


--All Gross Pay Codes
BEGIN TRAN
SET ROWCOUNT 1000
      delete18:
DELETE
  dbo.gross_pay_type
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete18
SET ROWCOUNT 0
COMMIT TRAN


--All Measurement Periods.
BEGIN TRAN
SET ROWCOUNT 1000
      delete19:
DELETE
  dbo.measurement
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete19
SET ROWCOUNT 0
COMMIT TRAN 


--All Plan Years.
BEGIN TRAN
SET ROWCOUNT 1000
      delete20:
DELETE
  dbo.plan_year
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete20
SET ROWCOUNT 0
COMMIT TRAN 

--All Batch rows.
BEGIN TRAN
SET ROWCOUNT 1000
      delete22:
DELETE
  dbo.batch
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete22
SET ROWCOUNT 0
COMMIT TRAN 


END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH

Go



USE [aca-demo]
GO
/****** Object:  StoredProcedure [dbo].[RESET_EMPLOYER]    Script Date: 9/14/2016 3:41:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
      @employerID int
AS

BEGIN TRY
      --Equivalencies
BEGIN TRAN
      SET ROWCOUNT 1000
      delete1:

      DELETE dbo.equivalency
            where employer_id=@employerID

      IF @@ROWCOUNT > 0 GOTO delete1
      SET ROWCOUNT 0
COMMIT TRAN


--Plan Year Archives/Insurance Offers
BEGIN TRAN
SET ROWCOUNT 1000
      delete2:
DELETE
  dbo.employee_insurance_offer
  where employer_id=@employerID;
IF @@ROWCOUNT > 0 GOTO delete2
SET ROWCOUNT 0
COMMIT TRAN

--Plan Year Archives/Insurance Offers
BEGIN TRAN
SET ROWCOUNT 1000
      delete3:
DELETE
  dbo.employee_insurance_offer_archive
  where employer_id=@employerID;
IF @@ROWCOUNT > 0 GOTO delete3
SET ROWCOUNT 0
COMMIT TRAN


--Insurance Contributions
BEGIN TRAN
SET ROWCOUNT 1000
      delete5:
DELETE
dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));
IF @@ROWCOUNT > 0 GOTO delete5
SET ROWCOUNT 0
COMMIT TRAN


--Insurance Plans
BEGIN TRAN
SET ROWCOUNT 1000
      delete6:
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);
IF @@ROWCOUNT > 0 GOTO delete6
SET ROWCOUNT 0
COMMIT TRAN


--Payroll Summer Averages.
BEGIN TRAN
SET ROWCOUNT 1000
      delete7:
DELETE
  dbo.payroll_summer_averages
  where employer_id=@employerID;
IF @@ROWCOUNT > 0 GOTO delete7
SET ROWCOUNT 0
COMMIT TRAN 


--Employee Import Alerts.
BEGIN TRAN
SET ROWCOUNT 1000
      delete8:
DELETE
  dbo.import_employee
  WHERE employerID=@employerID
IF @@ROWCOUNT > 0 GOTO delete8
SET ROWCOUNT 0
COMMIT TRAN 


--Employee Alert Archives. Alerts that have been deleted by users.
BEGIN TRAN
SET ROWCOUNT 1000
      delete9:
DELETE dbo.alert_archive
      WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete9
SET ROWCOUNT 0
COMMIT TRAN 


--Payroll Import Alerts. Alerts that have been deleted by users.
BEGIN TRAN
SET ROWCOUNT 1000
      delete10:
DELETE dbo.payroll_archive
      WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete10
SET ROWCOUNT 0
COMMIT TRAN 


--Payroll Import Alerts.
BEGIN TRAN
SET ROWCOUNT 1000
      delete11:
DELETE
  dbo.import_payroll
  WHERE employerid=@employerID
IF @@ROWCOUNT > 0 GOTO delete11
SET ROWCOUNT 0
COMMIT TRAN 


--Insurance Carrier Import Alerts
BEGIN TRAN
SET ROWCOUNT 1000
      delete12:
DELETE
  dbo.import_insurance_coverage
  WHERE employer_id=@employerID;
IF @@ROWCOUNT > 0 GOTO delete12
SET ROWCOUNT 0
COMMIT TRAN


--All Carrier Import Coverage
BEGIN TRAN
SET ROWCOUNT 1000
      delete13:
DELETE
  dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);
IF @@ROWCOUNT > 0 GOTO delete13
SET ROWCOUNT 0
COMMIT TRAN


--All Payroll. 
BEGIN TRAN
SET ROWCOUNT 1000
      delete14:
DELETE 
  [aca].[dbo].[payroll]
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete14
SET ROWCOUNT 0
COMMIT TRAN

--All Employees.
BEGIN TRAN
SET ROWCOUNT 1000
      delete15:
DELETE  
  FROM dbo.employee
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete15
SET ROWCOUNT 0
COMMIT TRAN

--All Employee dependents.
BEGIN TRAN
SET ROWCOUNT 1000
      delete4:
DELETE 
      dbo.employee_dependents
      WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);
IF @@ROWCOUNT > 0 GOTO delete4
SET ROWCOUNT 0
COMMIT TRAN

--All HR Status Codes
BEGIN TRAN
SET ROWCOUNT 1000
      delete16:
DELETE
  dbo.hr_status
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete16
SET ROWCOUNT 0
COMMIT TRAN


--All Gross Pay Filters
BEGIN TRAN
SET ROWCOUNT 1000
      delete17:
DELETE
  dbo.gross_pay_filter
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete17
SET ROWCOUNT 0
COMMIT TRAN


--All Gross Pay Codes
BEGIN TRAN
SET ROWCOUNT 1000
      delete18:
DELETE
  dbo.gross_pay_type
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete18
SET ROWCOUNT 0
COMMIT TRAN


--All Measurement Periods.
BEGIN TRAN
SET ROWCOUNT 1000
      delete19:
DELETE
  dbo.measurement
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete19
SET ROWCOUNT 0
COMMIT TRAN 


--All Plan Years.
BEGIN TRAN
SET ROWCOUNT 1000
      delete20:
DELETE
  dbo.plan_year
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete20
SET ROWCOUNT 0
COMMIT TRAN 

--All Batch rows.
BEGIN TRAN
SET ROWCOUNT 1000
      delete22:
DELETE
  dbo.batch
  WHERE employer_id=@employerID
IF @@ROWCOUNT > 0 GOTO delete22
SET ROWCOUNT 0
COMMIT TRAN 


END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH

Go
