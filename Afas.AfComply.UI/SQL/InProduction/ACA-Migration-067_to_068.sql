USE [aca]
GO

CREATE INDEX [IDX_ArchiveFileInfo_EmployerId] ON [dbo].[ArchiveFileInfo]([EmployerId])
GO

CREATE INDEX [IDX_BreakInService_EntityStatusId] ON [dbo].[BreakInService]([EntityStatusId])
GO

CREATE INDEX [IDX_Employee_EmployeeTypeId] ON [dbo].[employee]([employee_type_id])
GO

CREATE INDEX [IDX_Employee_HrStatusId] ON [dbo].[employee]([HR_status_id])
GO

CREATE INDEX [IDX_Employee_EmployerId] ON [dbo].[employee]([employer_id])
GO

CREATE INDEX [IDX_Employee_StateId] ON [dbo].[employee]([state_id])
GO

CREATE INDEX [IDX_Employee_PlanYearId] ON [dbo].[employee]([plan_year_id])
GO

CREATE INDEX [IDX_Employee_LimboPlanYearId] ON [dbo].[employee]([limbo_plan_year_id])
GO

CREATE INDEX [IDX_Employee_MeasurementPlanYearId] ON [dbo].[employee]([meas_plan_year_id])
GO

CREATE INDEX [IDX_Employee_ClassificationId] ON [dbo].[employee]([classification_id])
GO

CREATE INDEX [IDX_Employee_AcaStatusId] ON [dbo].[employee]([aca_status_id])
GO

CREATE INDEX [IDX_EmployeeClassification_EmployerId] ON [dbo].[employee_classification]([employer_id])
GO

CREATE INDEX [IDX_EmployeeClassification_AshCode] ON [dbo].[employee_classification]([ash_code])
GO

CREATE INDEX [IDX_EmployeeDependents_EmployeeId] ON [dbo].[employee_dependents]([employee_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOffer_EmployerId] ON [dbo].[employee_insurance_offer]([employer_id]) INCLUDE ([offered], [accepted], [hra_flex_contribution])
GO

CREATE INDEX [IDX_EmployeeInsuranceOffer_EmployeeId] ON [dbo].[employee_insurance_offer]([employee_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOffer_PlanYearId] ON [dbo].[employee_insurance_offer]([plan_year_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOffer_InsuranceId] ON [dbo].[employee_insurance_offer]([insurance_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOffer_InsuranceContributionId] ON [dbo].[employee_insurance_offer]([ins_cont_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferArchive_EmployerId] ON [dbo].[employee_insurance_offer_archive]([employer_id]) INCLUDE ([offered], [accepted], [hra_flex_contribution])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferArchive_EmployeeId] ON [dbo].[employee_insurance_offer_archive]([employee_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferArchive_PlanYearId] ON [dbo].[employee_insurance_offer_archive]([plan_year_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferArchive_InsuranceId] ON [dbo].[employee_insurance_offer_archive]([insurance_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferArchive_InsuranceContributionId] ON [dbo].[employee_insurance_offer_archive]([ins_cont_id])
GO

CREATE INDEX [IDX_EmployeeType_EmployerId] ON [dbo].[employee_type]([employer_id])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferEditable_TaxYearId_EmployerId] ON [dbo].[EmployeeInsuranceOfferEditable]([TaxYearId], [EmployerId])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferEditable_EmployerId] ON [dbo].[EmployeeInsuranceOfferEditable]([EmployerId])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferEditable_EmployeeId] ON [dbo].[EmployeeInsuranceOfferEditable]([EmployeeId])
GO

CREATE INDEX [IDX_EmployeeInsuranceOfferEditable_TimeFrameId] ON [dbo].[EmployeeInsuranceOfferEditable]([TimeFrameId])
GO

CREATE INDEX [IDX_EmployeeMeasurementAverageHours_EmployeeId] ON [dbo].[EmployeeMeasurementAverageHours]([EmployeeId])
GO

CREATE INDEX [IDX_EmployeeMeasurementAverageHours_MeasurementId] ON [dbo].[EmployeeMeasurementAverageHours]([MeasurementId])
GO

CREATE INDEX [IDX_EmployeeMeasurementAverageHours_EntityStatusId] ON [dbo].[EmployeeMeasurementAverageHours]([EntityStatusId])
GO

CREATE INDEX [IDX_EmployeeMeasurementAverageHours_EntityStatusId_IsNewHire] ON [dbo].[EmployeeMeasurementAverageHours]([EntityStatusId], [IsNewHire]) INCLUDE ([EmployeeId], [MeasurementId], [MonthlyAverageHours])
GO

CREATE INDEX [IDX_Employer_StateId] ON [dbo].[employer]([state_id])
GO

CREATE INDEX [IDX_Employer_BillStateId] ON [dbo].[employer]([bill_state])
GO

CREATE INDEX [IDX_Employer_VendorId] ON [dbo].[employer]([vendor_id])
GO

CREATE INDEX [IDX_Employer_FeeId] ON [dbo].[employer]([fee_id])
GO

CREATE INDEX [IDX_EmployerTaxYearTransmission_EmployerId] ON [dbo].[EmployerTaxYearTransmission]([EmployerId])
GO

CREATE INDEX [IDX_EmployerTaxYearTransmission_TaxYearId] ON [dbo].[EmployerTaxYearTransmission]([TaxYearId])
GO

CREATE INDEX [IDX_EmployerTaxYearTransmission_EntityStatusId] ON [dbo].[EmployerTaxYearTransmission]([EntityStatusId])
GO

CREATE INDEX [IDX_EmployerTaxYearTransmission_EmployerTaxYearTransmissionId_EntityStatusId] ON [dbo].[EmployerTaxYearTransmission]([EmployerTaxYearTransmissionId], [EntityStatusId])
GO

CREATE INDEX [IDX_EmployerTaxYearTransmissionStatus_EmployerTaxYearTransmissionId] ON [dbo].[EmployerTaxYearTransmissionStatus]([EmployerTaxYearTransmissionId])
GO

CREATE INDEX [IDX_EmployerTaxYearTransmissionStatus_TransmissionStatusId] ON [dbo].[EmployerTaxYearTransmissionStatus]([TransmissionStatusId])
GO

CREATE INDEX [IDX_EmployerTaxYearTransmissionStatus_EntityStatusId] ON [dbo].[EmployerTaxYearTransmissionStatus]([EntityStatusId])
GO

CREATE INDEX [IDX_EmployerTaxYearTransmissionStatus_EmployerTaxYearTransmissionId_EntityStatusId] ON [dbo].[EmployerTaxYearTransmissionStatus]([EmployerTaxYearTransmissionId], [EntityStatusId]) INCLUDE (EmployerTaxYearTransmissionStatusId)
GO

CREATE INDEX [IDX_ImportEmployee_EmployerId] ON [dbo].[import_employee]([employerID])
GO

CREATE INDEX [IDX_ImportEmployee_BatchId] ON [dbo].[import_employee]([batchid])
GO

CREATE INDEX [IDX_ImportInsuranceCoverage_EmployerId] ON [dbo].[import_insurance_coverage]([employer_id])
GO

CREATE INDEX [IDX_ImportInsuranceCoverage_BatchId] ON [dbo].[import_insurance_coverage]([batch_id])
GO

CREATE INDEX [IDX_Insurance_PlanYearId] ON [dbo].[insurance]([plan_year_id])
GO

CREATE INDEX [IDX_Insurance_InsuranceTypeId] ON [dbo].[insurance]([insurance_type_id])
GO

CREATE INDEX [IDX_InsuranceContribution_InsuranceId] ON [dbo].[insurance_contribution]([insurance_id])
GO

CREATE INDEX [IDX_InsuranceContribution_ContributionId] ON [dbo].[insurance_contribution]([contribution_id])
GO

CREATE INDEX [IDX_InsuranceContribution_ClassificationId] ON [dbo].[insurance_contribution]([classification_id])
GO

CREATE INDEX [IDX_InsuranceCoverage_TaxYearId] ON [dbo].[insurance_coverage]([tax_year])
GO

CREATE INDEX [IDX_InsuranceCoverage_CarrierId] ON [dbo].[insurance_coverage]([carrier_id])
GO

CREATE INDEX [IDX_InsuranceCoverage_EmployeeId] ON [dbo].[insurance_coverage]([employee_id])
GO

CREATE INDEX [IDX_InsuranceCoverage_DependentId] ON [dbo].[insurance_coverage]([dependent_id])
GO

CREATE INDEX [IDX_InsuranceCoverage_TaxYearId_EmployeeId_DependentId] ON [dbo].[insurance_coverage]([tax_year], [employee_id], [dependent_id])
GO

CREATE INDEX [IDX_Measurement_PlanYearId] ON [dbo].[measurement]([plan_year_id])
GO

CREATE INDEX [IDX_MeasurementBreakInService_MeasurementId] ON [dbo].[measurementBreakInService]([measurement_id]) INCLUDE ([BreakInServiceId])
GO

CREATE INDEX [IDX_Payroll_EmployeeId] ON [dbo].[payroll]([employee_id]) INCLUDE ([row_id])
GO

CREATE INDEX [IDX_Payroll_EmployerId_BatchId] ON [dbo].[payroll]([employer_id], [batch_id]) INCLUDE ([row_id])
GO

CREATE INDEX [IDX_Payroll_BatchId] ON [dbo].[payroll]([batch_id]) INCLUDE ([row_id])
GO

CREATE INDEX [IDX_InsuranceCoverageEditable_TaxYearId] ON [dbo].[insurance_coverage_editable]([tax_year])
GO

CREATE INDEX [IDX_InsuranceCoverageEditable_EmployerId] ON [dbo].[insurance_coverage_editable]([employer_id])
GO

CREATE INDEX [IDX_InsuranceCoverageEditable_EmployeeId] ON [dbo].[insurance_coverage_editable]([employee_id])
GO

CREATE INDEX [IDX_InsuranceCoverageEditable_DependentId] ON [dbo].[insurance_coverage_editable]([dependent_id])
GO

CREATE INDEX [IDX_PlanYear_EmployerId] ON [dbo].[plan_year]([employer_id]) INCLUDE ([plan_year_id])
GO

CREATE INDEX [IDX_TaxYear1095CApproval_TaxYearId] ON [dbo].[tax_year_1095c_approval]([tax_year])
GO

CREATE INDEX [IDX_TaxYear1095CApproval_EmployeeId] ON [dbo].[tax_year_1095c_approval]([employee_id])
GO

CREATE INDEX [IDX_TaxYear1095CApproval_EmployerId] ON [dbo].[tax_year_1095c_approval]([employer_id])
GO

CREATE INDEX [IDX_TaxYear1095CApproval_TaxYearId_EmployerId] ON [dbo].[tax_year_1095c_approval]([tax_year], [employer_id]) INCLUDE ([employee_id])
GO

CREATE INDEX [IDX_TaxYearApproval_EmployerId] ON [dbo].[tax_year_approval]([employer_id])
GO

CREATE INDEX [IDX_TaxYearApproval_TaxYearId] ON [dbo].[tax_year_approval]([tax_year])
GO

CREATE INDEX [IDX_User_EmployerId] ON [dbo].[user]([employer_id])
GO

CREATE INDEX [IDX_User_EmployerId_Active] ON [dbo].[user]([employer_id], [active])
GO

CREATE INDEX [IDX_User_EmployerId_Active_IrsContact] ON [dbo].[user]([employer_id], [active], [irsContact]) INCLUDE ([fName], [lName], [phone])
GO
