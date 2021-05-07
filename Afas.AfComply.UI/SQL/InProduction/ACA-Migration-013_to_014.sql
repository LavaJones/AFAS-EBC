USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[RESET_EMPLOYER]    Script Date: 9/8/2016 10:28:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <03/13/2015>
-- Description:	<This stored procedure is meant to delete all employee_import records matching the batch id.>
-- Altered:
--				<11/30/2015> TLW
--					- Changed to handle new Foreign Key constraints. 
-- =============================================
ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
	@employerID int
AS
BEGIN TRY

--Equivalencies
DELETE 
  aca.dbo.equivalency
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  aca.dbo.employee_insurance_offer
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  aca.dbo.employee_insurance_offer_archive
  where employer_id=@employerID;

DELETE 
	aca.dbo.employee_dependents
	WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--Insurance Contributions
DELETE
 aca.dbo.insurance_contribution
 WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--Payroll Summer Averages. 
DELETE
  aca.dbo.payroll_summer_averages
  where employer_id=@employerID;

--Employee Import Alerts. 
DELETE
  aca.dbo.import_employee
  WHERE employerID=@employerID

--Employee Alert Archives. Alerts that have been deleted by users. 
DELETE aca.dbo.alert_archive
	WHERE employer_id=@employerID

--Payroll Import Alerts. Alerts that have been deleted by users. 
DELETE aca.dbo.payroll_archive
	WHERE employer_id=@employerID

--Payroll Import Alerts. 
DELETE
  aca.dbo.import_payroll
  WHERE employerid=@employerID

--Insurance Carrier Import Alerts
DELETE
  aca.dbo.import_insurance_coverage
  WHERE employer_id=@employerID;

--All Carrier Import Coverage
DELETE
  aca.dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--All Payroll. 
DELETE 
  [aca].[dbo].[payroll]
  WHERE employer_id=@employerID

--All Employees.
DELETE  
  FROM aca.dbo.employee
  WHERE employer_id=@employerID

--All HR Status Codes
DELETE
  aca.dbo.hr_status
  WHERE employer_id=@employerID

--All Gross Pay Filters
DELETE
  aca.dbo.gross_pay_filter
  WHERE employer_id=@employerID

--All Gross Pay Codes
DELETE
  aca.dbo.gross_pay_type
  WHERE employer_id=@employerID

--All Measurement Periods. 
DELETE
  aca.dbo.measurement
  WHERE employer_id=@employerID

--All Plan Years. 
DELETE
  aca.dbo.plan_year
  WHERE employer_id=@employerID

--All assigned Alerts. 
DELETE
  aca.dbo.alert
  WHERE employer_id=@employerID

--All Batch rows. 
DELETE
  aca.dbo.batch
  WHERE employer_id=@employerID

END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_employeeType]
       -- Add the parameters for the stored procedure here
       @employeeId int,
       @name varchar(50)
       
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    UPDATE [aca].[dbo].[employee_type] SET name = @name
       WHERE employee_type_id = @employeeId
       
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO