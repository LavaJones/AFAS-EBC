USE aca
GO
ALTER TABLE dbo.tax_year_1095c_correction DROP COLUMN [approvedBy]
GO
ALTER TABLE dbo.tax_year_1095c_correction DROP COLUMN [approvedOn]
GO
ALTER TABLE dbo.tax_year_1095c_correction DROP COLUMN [get1095c]
GO
DROP TYPE BULK_Tax_year_1095c_correction
GO
CREATE TYPE BULK_Tax_year_1095c_correction AS TABLE
(
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
	[Corrected] [bit] NOT NULL,
	[Transmitted] [bit] NOT NULL,
	[EntityStatusId] [int] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL		

)
GO
GRANT SELECT ON BULK_Tax_year_1095c_correction TO [aca-user] as [dbo]