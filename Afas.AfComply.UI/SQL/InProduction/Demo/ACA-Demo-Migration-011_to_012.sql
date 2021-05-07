USE [aca-demo]
GO

GRANT SELECT ON [dbo].[ErrorLog] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[INSERT_ErrorLogging] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[SELECT_errorLog] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[UPDATE_floater] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[DELETE_Alert] TO [aca-user] AS [dbo]
GO

ALTER PROCEDURE [dbo].[REMOVE_EMPLOYER_FROM_ACT]
       @employerID int
AS
BEGIN TRY
delete FROM [aca].[dbo].[employee_dependents] where employee_id in (select employee_id FROM [aca].[dbo].[employee] where employer_id = @employerID);

delete FROM [aca].[dbo].[insurance_coverage] where employee_id in (select employee_id FROM [aca].[dbo].[employee] where employer_id = @employerID);

delete FROM [aca].[dbo].[import_insurance_coverage] where [employer_id] = @employerID;

delete FROM [aca].[dbo].[import_payroll] where employerid = @employerID;

delete FROM [aca].[dbo].[payroll] where employer_id = @employerID;

delete FROM [aca].[dbo].[employee_insurance_offer] where employer_id = @employerID;

delete FROM [aca].[dbo].[employee] where employer_id = @employerID;

delete FROM [aca].[dbo].[import_employee] where employerID = @employerID;

DELETE FROM [aca].[dbo].[hr_status] where employer_id = @employerID;

DELETE FROM [aca].[dbo].[gross_pay_type] where employer_id = @employerID;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO
