USE [aca]
GO

/****** Object:  View [dbo].[View_full_insurance_coverage]    Script Date: 12/28/2017 1:51:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[View_full_insurance_coverage]
AS
SELECT 
              row_number() over (order by dbo.employee.employer_id desc) as RowId, dbo.employee.employer_id, dbo.employee.employee_id, dbo.employee.fName as employeeFName, dbo.employee.mName as employeeMName, 
              dbo.employee.lName as employeeLName, dbo.employee.ssn as employeeSsn, dbo.employee.dob as employeeDob, 
              dbo.employee_dependents.dependent_id, dbo.employee_dependents.fName as dependentFName, dbo.employee_dependents.mName as dependentMName, 
              dbo.employee_dependents.lName as dependentLName, dbo.employee_dependents.ssn as dependentSsn, dbo.employee_dependents.dob as dependentDob,
              dbo.View_air_replacement_PartIII_details.tax_year, 
              dbo.View_air_replacement_PartIII_details.jan, dbo.View_air_replacement_PartIII_details.feb, dbo.View_air_replacement_PartIII_details.mar, dbo.View_air_replacement_PartIII_details.apr, dbo.View_air_replacement_PartIII_details.may, 
              dbo.View_air_replacement_PartIII_details.jun, dbo.View_air_replacement_PartIII_details.jul, dbo.View_air_replacement_PartIII_details.aug, dbo.View_air_replacement_PartIII_details.sep, dbo.View_air_replacement_PartIII_details.oct, 
              dbo.View_air_replacement_PartIII_details.nov, dbo.View_air_replacement_PartIII_details.dec


FROM     dbo.View_air_replacement_PartIII_details
                                    LEFT OUTER JOIN
                  dbo.employee ON dbo.View_air_replacement_PartIII_details.employee_id = dbo.employee.employee_id
                                    LEFT OUTER JOIN
                  dbo.employee_dependents ON dbo.employee_dependents.dependent_id = dbo.View_air_replacement_PartIII_details.dependent_id
WHERE dbo.View_air_replacement_PartIII_details.dependent_id is null or dbo.employee_dependents.EntityStatusID = 1



GO


