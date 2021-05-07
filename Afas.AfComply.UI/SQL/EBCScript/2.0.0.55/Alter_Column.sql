USE aca
GO
ALTER TABLE dbo.insurance ALTER COLUMN fullyPlusSelfInsured bit
GO
DROP INDEX IDX_EmployeeInsuranceOfferArchive_EmployerId on dbo.employee_insurance_offer_archive
GO
ALTER TABLE dbo.employee_insurance_offer_archive ALTER COLUMN offered bit
GO
CREATE NONCLUSTERED INDEX IDX_EmployeeInsuranceOfferArchive_EmployerId 
ON dbo.employee_insurance_offer_archive (employer_id)
INCLUDE (offered, accepted, hra_flex_contribution)
GO
