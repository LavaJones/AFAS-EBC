USE aca
GO

UPDATE dbo.tax_year_1095c_approval SET printed = 0 WHERE printed is null
GO
ALTER TABLE dbo.tax_year_1095c_approval ALTER COLUMN [printed] bit NOT NULL