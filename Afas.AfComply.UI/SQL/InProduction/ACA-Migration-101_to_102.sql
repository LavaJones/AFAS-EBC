USE [aca]
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferEditable_TaxYearId_EmployerId_EmployeeId_A83C5] ON [aca].[dbo].[EmployeeInsuranceOfferEditable] ([TaxYearId], [EmployerId],[EmployeeId]) INCLUDE ([TimeFrameId], [OfferInForce], [CoverageInForce])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferEditable_EmployerId_EmployeeId_90A43] ON [aca].[dbo].[EmployeeInsuranceOfferEditable] ([EmployerId],[EmployeeId]) INCLUDE ([TimeFrameId])
GO

CREATE INDEX [IDX_TaxYear1095cApproval_Get1095C_TaxYear_EmployeeId_EmployerId] ON [aca].[dbo].[tax_year_1095c_approval] ([get1095C]) INCLUDE ([tax_year], [employee_id], [employer_id])
GO

CREATE INDEX [IDX_Employee_LastName] ON [aca].[dbo].[employee] ([lName])
GO

CREATE INDEX [IDX_Employee_TerminationDate_EmployerId] ON [aca].[dbo].[employee] ([terminationDate]) INCLUDE ([employer_id])
GO

CREATE INDEX [IDX_TaxYear1095cApproval_TaxYear_Printed_EmployerId] ON [aca].[dbo].[tax_year_1095c_approval] ([tax_year], [printed]) INCLUDE ([employer_id])
GO

CREATE INDEX [IDX_TaxYear1095cApproval_TaxYear_Get1095C] ON [aca].[dbo].[tax_year_1095c_approval] ([tax_year], [get1095C])
GO

CREATE INDEX [IDX_TaxYear1095cApproval_TaxYear_Get1095C_EmployeeId] ON [aca].[dbo].[tax_year_1095c_approval] ([tax_year], [get1095C]) INCLUDE ([employee_id])
GO
