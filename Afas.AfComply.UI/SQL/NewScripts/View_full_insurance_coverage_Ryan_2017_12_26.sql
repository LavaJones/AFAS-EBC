USE [aca]
GO

/****** Object:  View [dbo].[View_employee_insurance_coverage]    Script Date: 12/26/2017 10:44:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_full_insurance_coverage]
AS
SELECT 
		dbo.employee.employer_id, dbo.employee.employee_id, dbo.employee.fName as employeeFName, dbo.employee.mName as employeeMName, 
		dbo.employee.lName as employeeLName, dbo.employee.ssn as employeeSsn, dbo.employee.dob as employeeDob, 
		dbo.employee_dependents.dependent_id, dbo.employee_dependents.fName as dependentFName, dbo.employee_dependents.mName as dependentMName, 
		dbo.employee_dependents.lName as dependentLName, dbo.employee_dependents.ssn as dependentSsn, dbo.employee_dependents.dob as dependentDob,
		dbo.insurance_coverage.row_id, dbo.insurance_coverage.tax_year, dbo.insurance_coverage.carrier_id, dbo.insurance_coverage.all12, 
		dbo.insurance_coverage.jan, dbo.insurance_coverage.feb, dbo.insurance_coverage.mar, dbo.insurance_coverage.apr, dbo.insurance_coverage.may, 
		dbo.insurance_coverage.jun, dbo.insurance_coverage.jul, dbo.insurance_coverage.aug, dbo.insurance_coverage.sep, dbo.insurance_coverage.oct, 
		dbo.insurance_coverage.nov, dbo.insurance_coverage.dec, dbo.insurance_coverage.history


FROM     dbo.insurance_coverage 
					LEFT OUTER JOIN
                  dbo.employee ON dbo.insurance_coverage.employee_id = dbo.employee.employee_id
					LEFT OUTER JOIN
                  dbo.employee_dependents ON dbo.employee_dependents.dependent_id = dbo.insurance_coverage.dependent_id

GO

--WHERE  (dbo.insurance_coverage.dependent_id IS NULL)


--GO

--CREATE VIEW [dbo].[View_dependent_insurance_coverage]
--AS
--SELECT dbo.employee_dependents.dependent_id, dbo.employee_dependents.employee_id, dbo.employee_dependents.fName, dbo.employee_dependents.mName, 
--                  dbo.employee_dependents.lName, dbo.employee_dependents.ssn, dbo.employee_dependents.dob, dbo.insurance_coverage.row_id, dbo.insurance_coverage.tax_year, 
--                  dbo.insurance_coverage.carrier_id, dbo.insurance_coverage.all12, dbo.insurance_coverage.jan, dbo.insurance_coverage.feb, dbo.insurance_coverage.mar, 
--                  dbo.insurance_coverage.apr, dbo.insurance_coverage.may, dbo.insurance_coverage.jun, dbo.insurance_coverage.jul, dbo.insurance_coverage.aug, 
--                  dbo.insurance_coverage.sep, dbo.insurance_coverage.oct, dbo.insurance_coverage.nov, dbo.insurance_coverage.dec, dbo.insurance_coverage.history
--FROM     dbo.employee_dependents RIGHT OUTER JOIN
--                  dbo.insurance_coverage ON dbo.employee_dependents.dependent_id = dbo.insurance_coverage.dependent_id


--GO

