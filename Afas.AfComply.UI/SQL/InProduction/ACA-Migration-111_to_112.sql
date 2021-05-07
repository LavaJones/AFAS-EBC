USE [aca]
GO
CREATE TYPE BULK_Tax_year_1095c_approval AS TABLE
(
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NOT NULL
)
GO
GRANT SELECT ON BULK_Tax_year_1095c_approval TO [aca-user] as [dbo]
GO