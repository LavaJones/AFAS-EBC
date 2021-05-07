USE aca
GO
CREATE TYPE BULK_Tax_year_1095c_approval AS TABLE
(
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NOT NULL
)
GO