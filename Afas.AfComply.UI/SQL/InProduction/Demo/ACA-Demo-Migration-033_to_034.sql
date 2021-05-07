USE [aca-demo]
GO

UPDATE [dbo].[insurance_carrier_import_template]
SET
	[subscriber] = 1,
	[employee_dependent_link] = 2,
	[ssn] = 3
WHERE
	[carrier_id] = 13
GO
