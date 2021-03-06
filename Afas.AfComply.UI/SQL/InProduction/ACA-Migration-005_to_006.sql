USE [aca]
GO
UPDATE [dbo].[state] SET [abbreviation] = N'NJ' where [state_id] = 30
GO

ALTER TABLE [dbo].[alert] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[alert] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[alert] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Alert_ResourceId] ON [dbo].[alert]([ResourceId])
GO

ALTER TABLE [dbo].[alert_archive] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[alert_archive] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[alert_archive] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_AlertArchive_ResourceId] ON [dbo].[alert_archive]([ResourceId])
GO

ALTER TABLE [dbo].[batch] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[batch] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[batch] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Batch_ResourceId] ON [dbo].[batch]([ResourceId])
GO

ALTER TABLE [dbo].[employee] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[employee] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Employee_ResourceId] ON [dbo].[employee]([ResourceId])
GO

ALTER TABLE [dbo].[employee_classification] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[employee_classification] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[employee_classification] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_EmployeeClassification_ResourceId] ON [dbo].[employee_classification]([ResourceId])
GO

ALTER TABLE [dbo].[employee_dependents] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[employee_dependents] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[employee_dependents] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_EmployeeDependents_ResourceId] ON [dbo].[employee_dependents]([ResourceId])
GO

ALTER TABLE [dbo].[employee_insurance_offer] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[employee_insurance_offer] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[employee_insurance_offer] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_EmployeeInsuranceOffer_ResourceId] ON [dbo].[employee_insurance_offer]([ResourceId])
GO

ALTER TABLE [dbo].[employee_insurance_offer_archive] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_EmployeeInsuranceOfferArchive_ResourceId] ON [dbo].[employee_insurance_offer_archive]([ResourceId])
GO

ALTER TABLE [dbo].[employee_type] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[employee_type] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[employee_type] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_EmployeeType_ResourceId] ON [dbo].[employee_type]([ResourceId])
GO

ALTER TABLE [dbo].[employer] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[employer] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[employer] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Employer_ResourceId] ON [dbo].[employer]([ResourceId])
GO

ALTER TABLE [dbo].[equivalency] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[equivalency] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[equivalency] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Equivalency_ResourceId] ON [dbo].[equivalency]([ResourceId])
GO

ALTER TABLE [dbo].[gross_pay_filter] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[gross_pay_filter] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[gross_pay_filter] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_GrossPayFilter_ResourceId] ON [dbo].[gross_pay_filter]([ResourceId])
GO

ALTER TABLE [dbo].[gross_pay_type] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[gross_pay_type] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[gross_pay_type] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_GrossPayType_ResourceId] ON [dbo].[gross_pay_type]([ResourceId])
GO

ALTER TABLE [dbo].[hr_status] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[hr_status] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[hr_status] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_HrStatus_ResourceId] ON [dbo].[hr_status]([ResourceId])
GO

ALTER TABLE [dbo].[import_employee] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[import_employee] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[import_employee] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_ImportEmployee_ResourceId] ON [dbo].[import_employee]([ResourceId])
GO

ALTER TABLE [dbo].[import_insurance_coverage] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[import_insurance_coverage] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[import_insurance_coverage] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_ImportInsuranceCoverage_ResourceId] ON [dbo].[import_insurance_coverage]([ResourceId])
GO

ALTER TABLE [dbo].[import_insurance_coverage_archive] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[import_insurance_coverage_archive] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[import_insurance_coverage_archive] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_ImportInsuranceCoverageArchive_ResourceId] ON [dbo].[import_insurance_coverage_archive]([ResourceId])
GO

ALTER TABLE [dbo].[import_payroll] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[import_payroll] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[import_payroll] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_ImportPayroll_ResourceId] ON [dbo].[import_payroll]([ResourceId])
GO

ALTER TABLE [dbo].[insurance] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[insurance] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[insurance] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Insurance_ResourceId] ON [dbo].[insurance]([ResourceId])
GO

ALTER TABLE [dbo].[insurance_coverage] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[insurance_coverage] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[insurance_coverage] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_InsuranceCoverage_ResourceId] ON [dbo].[insurance_coverage]([ResourceId])
GO

ALTER TABLE [dbo].[insurance_coverage_editable] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[insurance_coverage_editable] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[insurance_coverage_editable] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_InsuranceCoverageEditable_ResourceId] ON [dbo].[insurance_coverage_editable]([ResourceId])
GO

ALTER TABLE [dbo].[invoice] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[invoice] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[invoice] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Invoice_ResourceId] ON [dbo].[invoice]([ResourceId])
GO

ALTER TABLE [dbo].[measurement] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[measurement] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[measurement] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Measurement_ResourceId] ON [dbo].[measurement]([ResourceId])
GO

ALTER TABLE [dbo].[payroll] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[payroll] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[payroll] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Payroll_ResourceId] ON [dbo].[payroll]([ResourceId])
GO

ALTER TABLE [dbo].[payroll_archive] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[payroll_archive] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[payroll_archive] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_PayrollArchive_ResourceId] ON [dbo].[payroll_archive]([ResourceId])
GO

ALTER TABLE [dbo].[payroll_summer_averages] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[payroll_summer_averages] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[payroll_summer_averages] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_PayrollSummerAverages_ResourceId] ON [dbo].[payroll_summer_averages]([ResourceId])
GO

ALTER TABLE [dbo].[plan_year] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[plan_year] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[plan_year] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_PlanYear_ResourceId] ON [dbo].[plan_year]([ResourceId])
GO

ALTER TABLE [dbo].[tax_year_1095c_approval] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[tax_year_1095c_approval] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[tax_year_1095c_approval] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_TaxYear1095CApproval_ResourceId] ON [dbo].[tax_year_1095c_approval]([ResourceId])
GO

ALTER TABLE [dbo].[tax_year_approval] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[tax_year_approval] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[tax_year_approval] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_TaxYearApproval_ResourceId] ON [dbo].[tax_year_approval]([ResourceId])
GO

ALTER TABLE [dbo].[user] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[user] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_User_ResourceId] ON [dbo].[user]([ResourceId])
GO

ALTER TABLE [dbo].[user] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[user] DROP CONSTRAINT [uc_Useremail]
GO
CREATE INDEX [IDX_User_Email] ON [dbo].[user] ([email])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT ALL
GO 
