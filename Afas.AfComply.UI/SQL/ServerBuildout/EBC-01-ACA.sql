-- start of ACA-Migration-001_to_002.sql

USE [aca]
GO
CREATE ROLE [aca-user]
GO
GRANT EXECUTE ON [dbo].[ap_AIR_SELECT_employer_employee_ids] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ARCHIVE_employee_plan_year] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DEACTIVATE_user] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_classification] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_dependent] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_employee_1095c_approval] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_employee_import] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_employee_import_row] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_employer_demographic_alerts] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_employer_gross_pay_filter] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_employer_payroll_alerts] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_equivalency] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_insurance] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_insurance_carrier_batch_import] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_insurance_carrier_import_row] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_insurance_contribution] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_insurance_coverage_editable_row] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_payroll_import] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_payroll_import_row] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[DELETE_payroll_summer_average] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_employer_alert] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_import_insurance_carrier_report] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_import_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_1095_tax_year_approval] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_batch] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_classification] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_editable_insurance_coverage] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_employee_type] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_employer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_equivalency] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_gross_pay] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_gross_pay_filter] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_hr_status] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_insurance_contribution] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_insurance_offer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_insurance_plan] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_insurnace_coverage] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_invoice] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_measurement] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_payroll_summer_avg] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_plan_year] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_registration] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_new_user] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_PlanYear_Missing_insurance_offers] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_UPDATE_employee_dependent] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_UPDATE_employer_irs_submission_approval] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[MERGE_gross_pay_description] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[REMOVE_EMPLOYER_FROM_ACT] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[RESET_EMPLOYER] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT__payroll_batch] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_activities] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_aca_status] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_alert_types] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_employee_types] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_employer_types] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_employers] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_fees] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_initial_measurements] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_insurance_carriers] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_insurance_types] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_months] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_states] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_terms] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_users] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_all_vendors] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_contribution_types] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_details] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_all_individual_coverage] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_coverage] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_dependent_coverage] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_dependents] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_editable_individual_coverage] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_gross_pay_count] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_insurance_offer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_payroll_sum] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employee_payroll_summer_avg] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_alerts] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_autoupload] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_batch_top25] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_billing] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_billing_count] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_check_dates] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_classifications] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_employee_count] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_employee_export] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_employees] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_employees_in_insurance_carrier_table] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_employees_Tax_Year_Approved] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_equivalencies] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_gross_pay_filters] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_gross_pay_types] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_hr_status] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_insurance_alerts] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_insurance_coverage_import_alerts] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_invoices] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_irs_submission] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_measurements] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_payroll_duplicates] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_payroll_summer_avg] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_plan_years] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_users] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_equivalency_units] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_Import_employer_employees] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_import_employer_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_insurance_contributions] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_insurance_coverage_template] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_measurement_types] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_open_invoices] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_past_due_measurement_periods] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_payroll_batch] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_plan_year_insurance_plan] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_planyear_measurement] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_positions] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_single_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_specific_measurements] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_vendor] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_ETL_ShortBuild] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_INSERT_approved_monthly_detail] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_4980H_codes] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employee_LINE3_coverage] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employer_employee_ids] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employer_employees_in_yearly_detail] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_mec_codes] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_monthly_detail] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_status_codes] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_Time_Frame_Months] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_UPDATE_approve_monthly_detail] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_UPDATE_approved_monthly_detail] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_UPDATE_covered_individual] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[TRANSFER_import_existing_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[TRANSFER_import_existing_insurance_carrier_imports] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[TRANSFER_import_new_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[TRANSFER_import_new_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[TRANSFER_insurance_change_event] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_AVG_MONTHLY_HOURS] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_class] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_classification] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_LINEIII_DOB] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_LINEIII_Months] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_LINEIII_SSN] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_plan_year] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_plan_year_meas] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_plan_year_meas_id] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employee_ssn] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employer_measurement] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employer_setup] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_employer_su_fee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_equivalency] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_gp_description] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_hr_status] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_import_insurance_carrier] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_import_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_insurance_contribution] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_insurance_coverage_import] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_insurance_offer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_insurance_plan] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_invoice] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_measurement] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_plan_year] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_reset_pwd] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_user] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_user_billing_contact] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_user_floating] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[VALIDATE_user] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[aca_status] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[affordability_safe_harbor] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[alert] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[alert_archive] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[alert_type] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[batch] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[contribution] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[employee] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[employee_classification] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[employee_dependents] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[employee_insurance_offer] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[employee_insurance_offer_archive] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[employee_type] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[employer] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[employer_type] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[equiv_activity] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[equiv_detail] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[equiv_position] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[equivalency] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[equivalency_type] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[fee] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[gross_pay_filter] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[gross_pay_type] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[hr_status] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[import_employee] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[import_insurance_coverage] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[import_insurance_coverage_archive] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[import_payroll] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[initial_measurement] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[insurance] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[insurance_carrier] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[insurance_carrier_import_template] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[insurance_contribution] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[insurance_coverage] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[insurance_coverage_editable] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[insurance_type] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[invoice] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[measurement] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[measurement_type] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[month] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[payroll] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[payroll_archive] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[payroll_summer_averages] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[payroll_vendor] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[plan_year] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[state] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[tax_year] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[tax_year_1095c_approval] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[tax_year_approval] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[term] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[unit] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[user] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_employer_alerts] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_import_employee] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_import_payroll] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_insurance_offer] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_summer_window] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_billContact_alerts] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_billing_contact] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_irs_alerts] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_irs_contact] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_import_carrier] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_PlanYear_Insurance] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_missingInsuranceType_alerts] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_insurance_type] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts_union] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_alerts] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_dependent_insurance_coverage] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_employee_insurance_coverage] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_all_insurance_coverage] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_Avg_Hours_Ongoing] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_Avg_Hours_Ongoing_limbo] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_Avg_Hours_Ongoing_meas] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_Employee_Export] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_employer_equivalency] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_insurance_alert_details] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_Insurance_Contributions] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_payroll] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_payroll_summer_avg] TO [aca-user] AS [dbo]
GO

-- end of ACA-Migration-001_to_002.sql

-- start of ACA-Migration-005_to_006.sql

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

-- end of ACA-Migration-005_to_006.sql

-- start of ACA-Migration-007_to_008.sql

ALTER TABLE [dbo].[employee] ALTER COLUMN [dob] [datetime] NULL
GO

-- AFcomply carrier templates removed. gc5

-- end of ACA-Migration-007_to_008.sql

-- start of ACA-Migration-008_to_009.sql

DROP VIEW [dbo].[View_employer_alerts]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_employer_alerts]
AS
SELECT
	[alert].[alert_id],
	[alert].[alert_type_id],
	[alert].[employer_id],
	[alert_type].[name],
	[alert_type].[image_url],
	[alert_type].[table_name]
FROM [dbo].[alert] as [alert] LEFT OUTER JOIN
	[dbo].[alert_type] ON [alert].[alert_type_id] = [alert_type].[alert_type_id]
GO

DROP VIEW [dbo].[View_PlanYear_Insurance]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_PlanYear_Insurance]
AS
SELECT
	[planYear].[plan_year_id],
	[planYear].[employer_id],
	[planYear].[description],
	[planYear].[startDate],
	[planYear].[endDate],
	[planYear].[notes],
	[planYear].[history],
	[planYear].[modOn],
	[planYear].[modBy],
	[dbo].[insurance].[insurance_id],
	[dbo].[insurance].[description] AS Expr1,
	[dbo].[insurance].[monthlycost],
	[dbo].[insurance].[minValue],
	[dbo].[insurance].[offSpouse],
	[dbo].[insurance].[offDependent],
    [dbo].[insurance].[modOn] AS Expr2,
    [dbo].[insurance].[modBy] AS Expr3,
    [dbo].[insurance].[history] AS Expr4,
    [dbo].[insurance].[insurance_type_id]
FROM [dbo].[plan_year] as [planYear] RIGHT OUTER JOIN
	[dbo].[insurance] ON [planYear].[plan_year_id] = [dbo].[insurance].[plan_year_id]
GO

GRANT SELECT ON [dbo].[View_PlanYear_Insurance] TO [aca-user] AS [dbo]
GO

DROP VIEW [dbo].[View_payroll_summer_avg]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_payroll_summer_avg]
AS
SELECT
	[payrollSummerAvg].[row_id],
	[payrollSummerAvg].[employer_id],
	[payrollSummerAvg].[plan_year_id],
	[payrollSummerAvg].[batch_id],
	[payrollSummerAvg].[employee_id],
	[payrollSummerAvg].[gp_id],
	[payrollSummerAvg].[act_hours],
	[payrollSummerAvg].[sdate],
	[payrollSummerAvg].[edate],
	[payrollSummerAvg].[cdate],
	[payrollSummerAvg].[modBy],
	[payrollSummerAvg].[modOn],
	[payrollSummerAvg].[history],
	[dbo].[gross_pay_type].[description]
FROM [dbo].[payroll_summer_averages] as [payrollSummerAvg] LEFT OUTER JOIN
	[dbo].[gross_pay_type] ON [payrollSummerAvg].[gp_id] = [dbo].[gross_pay_type].[gross_pay_id]
GO

GRANT SELECT ON [dbo].[View_payroll_summer_avg] TO [aca-user] AS [dbo]
GO

DROP VIEW[dbo].[View_Insurance_Contributions]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Insurance_Contributions]
AS
SELECT
	[dbo].[insurance_contribution].[ins_cont_id],
	[dbo].[insurance_contribution].[insurance_id],
	[dbo].[insurance_contribution].[contribution_id],
	[dbo].[insurance_contribution].[classification_id],
    [dbo].[insurance_contribution].[amount],
    [dbo].[insurance_contribution].[modBy],
    [dbo].[insurance_contribution].[modOn],
    [dbo].[insurance_contribution].[history],
    [dbo].[employee_classification].[description]
FROM [dbo].[employee_classification] LEFT OUTER JOIN
	[dbo].[insurance_contribution] ON [dbo].[employee_classification].[classification_id] = [dbo].[insurance_contribution].[classification_id]
GO

GRANT SELECT ON [dbo].[View_Insurance_Contributions] TO [aca-user] AS [dbo]
GO

-- end of ACA-Migration-008_to_009.sql

-- start of ACA-Migration-009_to_010.sql

UPDATE dbo.term SET [description] = 'An employee in a position for which the customary annual employment is six months or less. The reference to customary means that by the nature of the position an employee in this position typically works for a period of six months or less, and that period should begin each calendar year in approximately the same part of the year, such as summer or winter. In certain unusual instances, the employee can still be considered a seasonal employee even if the seasonal employment is extended in a particular year beyond its customary duration (regardless of whether the customary duration is six months or is less than six months).'
WHERE name = 'Seasonal Employees:'

UPDATE dbo.term SET [description] = 'The term stability period means a period selected by an applicable large employer member that immediately follows, and is associated with, a standard measurement period or an initial measurement period (and, if elected by the employer, the administrative period associated with that standard measurement period or initial measurement period), and is used by the applicable large employer member as part of the look-back measurement method.'
WHERE name = 'Stability period:'

UPDATE dbo.term SET [description] = 'The term standard measurement period means a period of at least three but not more than 12 consecutive months that is used by an applicable large employer member as part of the look-back measurement method.'
WHERE name = 'Standard Measurement Period:'

UPDATE dbo.term SET [description] = 'The term variable hour employee means an employee if, based on the facts and circumstances at the employee''s start date, the applicable large employer member cannot determine whether the employee is reasonably expected to be employed on average at least 30 hours of service per week during the initial measurement period because the employee''s hours are variable or otherwise uncertain.'
WHERE name = 'Variable Hour Employee:'

UPDATE dbo.term SET [description] = 'An employer may use one or more of the affordability safe harbors if it offers its full-time employees (and dependents) the opportunity to enroll in minimum essential coverage under a health plan that provides minimum value with respect to the self-only coverage offered to the employees. Use of any of the safe harbors is optional for an applicable large employer member, and an applicable large employer member may choose to apply the safe harbors for any reasonable category of employees, provided it does so on a uniform and consistent basis for all employees in a category. Reasonable categories generally include specified job categories, nature of compensation (hourly or salary), geographic location, and similar bona fide business criteria.'
WHERE name = 'AFFORDABILITY SAFE HARBORS '

UPDATE dbo.term SET [description] = 'Under the Form W-2 safe harbor, an employer may determine the affordability of its health coverage by reference only to an employee’s wages from that employer, instead of by reference to the employee’s household income. Wages for this purpose is the amount that is required to be reported in Box 1 of the employee’s Form W-2. An employer satisfies the Form W-2 safe harbor with respect to an employee if the employee’s required contribution for the calendar year for the employer’s lowest cost self-only coverage that provides minimum value during the entire calendar year (excluding COBRA or other continuation coverage except with respect to an active employee eligible for continuation coverage) does not exceed 9.5% (as adjusted yearly for inflation; in 2016 9.66%) of that employee’s Form W–2 wages from the employer for the calendar year. To be eligible for the Form W-2 safe harbor, the employee’s required contribution must remain a consistent amount or percentage of all Form W–2 wages during the calendar year (or during the plan year for plans with non-calendar year plan years). Thus, an applicable large employer is not permitted to make discretionary adjustments to the required employee contribution for a pay period. A periodic contribution that is based on a consistent percentage of all Form W–2 wages may be subject to a dollar limit specified by the employer. Employers determine whether the Form W-2 safe harbor applies after the end of the calendar year and on an employee-by-employee basis, taking into account W-2 wages and employee contributions. For an employee who was not offered coverage for an entire calendar year, the Form W-2 safe harbor is applied by: Adjusting the employee’s Form W-2 wages to reflect the period when the employee was offered coverage; and Comparing the adjusted wage amount to the employee’s share of the cost for the employer’s lowest cost self-only coverage that provides minimum value for the periods when coverage was offered. Specifically, the amount of the employee’s compensation for purposes of the Form W-2 safe harbor is determined by multiplying the wages for the calendar year by a fraction equal to the number of calendar months for which coverage was offered over the number of calendar months in the employee’s period of employment with the employer during the calendar year. For this purpose, if coverage is offered during at least one day during the calendar month, or the employee is employed for at least one day during the calendar month, the entire calendar month is counted in determining the applicable fraction.'
WHERE name = 'Form W-2 Safe Harbor'

UPDATE dbo.term SET [description] = 'The rate of pay safe harbor was designed to allow employers to prospectively satisfy affordability. For hourly employees, the rate of pay safe harbor allows an employer to: Take the lower of the hourly employee’s rate of pay as of the first day of the coverage period (generally, the first day of the plan year) or the employee’s lowest hourly rate of pay during the calendar month; Multiply that rate by 130 hours per month (the benchmark for full-time status for a month); and Determine affordability for the calendar month based on the resulting monthly wage amount. Specifically, the employee’s monthly contribution amount (for the self-only cost of the employer’s lowest cost coverage that provides minimum value) is affordable for a calendar month if it is equal to or lower than 9.5% (as adjusted yearly for inflation; in 2016 9.66%) of the computed monthly wages (that is, the employee’s applicable hourly rate of pay multiplied by 130 hours). An employer may use the rate of pay safe harbor even if an hourly employee’s rate of pay is reduced during the year. For salaried employees, monthly salary as of the first day of the coverage period would be used instead of hourly salary multiplied by 130 hours. However, if the monthly salary is reduced, including due to a reduction in work hours, the rate of pay safe harbor may not be used.'
WHERE name = 'Rate of Pay Safe Harbor'

UPDATE dbo.term SET [description] = 'An employer may also rely on a design-based safe harbor using the federal poverty level (FPL) for a single individual. The FPL safe harbor provides employers with a predetermined maximum amount of employee contribution that in all cases will result in the coverage being deemed affordable. Employer-provided coverage is considered affordable under the FPL safe harbor if the employee’s required contribution for the calendar month for the lowest cost self-only coverage that provides minimum value does not exceed 9.5% (as adjusted yearly for inflation; in 2016 9.66%) of the FPL for a single individual for the applicable calendar year, divided by 12. Employers may use any of the poverty guidelines in effect within six months before the first day of the plan year for purposes of this safe harbor.'
WHERE name = 'Federal Poverty Line Safe Harbor'

-- end of ACA-Migration-009_to_010.sql

-- start of ACA-Migration-010_to_011.sql

/****** Object:  Table [dbo].[ErrorLog]    Script Date: 8/18/2016 1:06:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ErrorLog](
       [ErrorLogID] [int] IDENTITY(1,1) NOT NULL,
       [ErrorTime] [datetime] NOT NULL CONSTRAINT [DF_ErrorLog_ErrorTime]  DEFAULT (getdate()),
       [UserName] [sysname] NOT NULL,
       [ErrorNumber] [int] NOT NULL,
       [ErrorSeverity] [int] NULL,
       [ErrorState] [int] NULL,
       [ErrorProcedure] [nvarchar](126) NULL,
       [ErrorLine] [int] NULL,
       [ErrorMessage] [nvarchar](4000) NOT NULL,
CONSTRAINT [PK_ErrorLog_ErrorLogID] PRIMARY KEY CLUSTERED 
(
       [ErrorLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for ErrorLog records.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorLogID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time at which the error occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The user who executed the batch in which the error occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'UserName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The error number of the error that occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The severity of the error that occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorSeverity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The state number of the error that occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorState'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the stored procedure or trigger where the error occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorProcedure'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The line number at which the error occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The message text of the error that occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorMessage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Audit table tracking errors in the the CloneOfAW database that are caught by the CATCH block of a TRY...CATCH construct. Data is inserted by stored procedure dbo.uspLogError when it is executed from inside the CATCH block of a TRY...CATCH construct.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key (clustered) constraint' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'CONSTRAINT',@level2name=N'PK_ErrorLog_ErrorLogID'
GO

CREATE PROCEDURE [dbo].[INSERT_ErrorLogging] 

AS
BEGIN 
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       INSERT INTO dbo.ErrorLog ([UserName]
      ,[ErrorNumber]
      ,[ErrorSeverity]
      ,[ErrorState]
      ,[ErrorProcedure]
      ,[ErrorLine]
      ,[ErrorMessage])
       VALUES (SYSTEM_USER 
         ,ERROR_NUMBER() 
         ,ERROR_SEVERITY() 
         ,ERROR_STATE() 
         ,ERROR_PROCEDURE() 
         ,ERROR_LINE() 
         ,ERROR_MESSAGE());
END
GO



CREATE PROCEDURE [dbo].[SELECT_errorLog]
       @time datetime,
       @errorNumber int,
       @errorLine int,
       @errorProcedure varchar(50),
       @rowsPage int,
       @pageNumber int

AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    -- Insert statements for procedure here

       SELECT [ErrorLogID]
      ,[ErrorTime]
      ,[UserName]
      ,[ErrorNumber]
      ,[ErrorSeverity]
      ,[ErrorState]
      ,[ErrorProcedure]
      ,[ErrorLine]
      ,[ErrorMessage]
  FROM [aca].[dbo].[ErrorLog]
  WHERE ((@time is null) OR (ErrorTime = @time))
  AND  ((@errorNumber is null) OR (ErrorNumber = @errorNumber))
  AND   ((@errorLine is null) OR (ErrorLine = @errorLine))
  AND   ((@errorProcedure is null) OR (ErrorLine = @errorLine))
  ORDER BY Errortime DESC OFFSET ((@pageNumber  - 1) * @rowsPage) ROWS
  FETCH NEXT @rowsPage ROWS ONLY;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO




CREATE PROCEDURE [dbo].[UPDATE_floater]

       @setFloater bit,
       @userId int

AS
BEGIN TRY

SET NOCOUNT ON;

    UPDATE [aca].[dbo].[user] SET floater = @setFloater
       WHERE [user_id] = @userId AND Employer_id = 1
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO




CREATE PROCEDURE [dbo].[DELETE_Alert]
       @alertId int,
       @employerId int
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    DELETE FROM dbo.alert 
       WHERE alert_type_id = @alertId AND employer_id = @employerId
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO




ALTER PROCEDURE [dbo].[UPDATE_employer]
       @employerID int,
       @name varchar(50),
       @address varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @logo varchar(50),
       @ein varchar(50),
       @employerTypeId int
AS
BEGIN TRY
       UPDATE employer
       SET
             name = @name, 
             [address] = @address,
             city = @city,
             state_id = @stateID, 
             zip = @zip,  
             img_logo = @logo,
             ein = @ein,
             employer_type_id = @employerTypeId
       WHERE
             employer_id = @employerID;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO




ALTER PROCEDURE [dbo].[REMOVE_EMPLOYER_FROM_ACT]
       @employerID int
AS
BEGIN TRY
DELETE 
  aca.dbo.equivalency
  where employer_id=@employerID

DELETE  aca.dbo.employee_insurance_offer
  where employer_id=@employerID

--Insurance Contributions
DELETE
aca.dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--Summer Payroll Supplemental hours
DELETE aca.dbo.payroll_summer_averages
  WHERE employer_id=@employerID;

DELETE
  aca.dbo.import_employee
  WHERE employerID=@employerID

DELETE aca.dbo.alert_archive
       WHERE employer_id=@employerID

DELETE aca.dbo.payroll_archive
       WHERE employer_id=@employerID

DELETE
  aca.dbo.import_payroll
  WHERE employerid=@employerID

DELETE 
  [aca].[dbo].[payroll]
  WHERE employer_id=@employerID

DELETE  
  FROM aca.dbo.employee
  WHERE employer_id=@employerID

DELETE
  aca.dbo.hr_status
  WHERE employer_id=@employerID

DELETE
  aca.dbo.gross_pay_filter
  WHERE employer_id=@employerID

DELETE
  aca.dbo.gross_pay_type
  WHERE employer_id=@employerID

DELETE
  aca.dbo.measurement
  WHERE employer_id=@employerID

DELETE
  aca.dbo.plan_year
  WHERE employer_id=@employerID

DELETE
  aca.dbo.alert
  WHERE employer_id=@employerID

DELETE
  aca.dbo.[user]
  WHERE employer_id=@employerID

DELETE
  aca.dbo.employee_type
  WHERE employer_id=@employerID

DELETE 
  aca.dbo.invoice
  WHERE employer_id=@employerID

DELETE
  aca.dbo.batch
  WHERE employer_id=@employerID

--All Employee Classifications. 
DELETE
  aca.dbo.employee_classification
  WHERE employer_id=@employerID

DELETE 
  aca.dbo.employer
  WHERE employer_id=@employerID
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO

-- end of ACA-Migration-010_to_011.sql

-- start of ACA-Migration-011_to_012.sql

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

-- end of ACA-Migration-011_to_012.sql

-- start of ACA-Migration-012_to_013.sql
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityStatus](
      [EntityStatusId] [int] IDENTITY(1,1) NOT NULL,
      [EntityStatusName] [nvarchar](50) NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,
CONSTRAINT [XPKEntityStatus] PRIMARY KEY CLUSTERED
(
      [EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

SET IDENTITY_INSERT EntityStatus ON
GO
INSERT into EntityStatus (EntityStatusId, EntityStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) VALUES (1, 'Active', 'SYSTEM', '2014-08-15', 'SYSTEM', '2014-08-15')
GO
INSERT into EntityStatus (EntityStatusId, EntityStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) VALUES (2, 'Inactive', 'SYSTEM', '2014-08-15', 'SYSTEM', '2014-08-15')
GO
INSERT into EntityStatus (EntityStatusId, EntityStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) VALUES (3, 'Deleted', 'SYSTEM', '2014-08-15', 'SYSTEM', '2014-08-15') 
GO
SET IDENTITY_INSERT EntityStatus OFF
GO

GRANT SELECT ON [dbo].[EntityStatus] TO [aca-user] AS [dbo]
GO

CREATE TABLE [dbo].[BreakInService](
      [BreakInServiceId] [bigint] IDENTITY(1,1) NOT NULL,
      [ResourceId] [uniqueidentifier] NOT NULL,
      [EntityStatusId] [int] NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,
      [StartDate] [datetime2](7) NULL,
      [EndDate] [datetime2](7) NULL,
      [Weeks] [int] NULL,
CONSTRAINT [PK_BreakInService_BreakInServiceId] PRIMARY KEY NONCLUSTERED
(
      [BreakInServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
  
ALTER TABLE [dbo].[BreakInService] ADD  DEFAULT (newid()) FOR [resourceId]
GO

ALTER TABLE [dbo].[BreakInService]  WITH CHECK ADD  CONSTRAINT [FK_BreakInService_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
 
ALTER TABLE [dbo].[BreakInService] CHECK CONSTRAINT [FK_BreakInService_EntityStatus]
GO

GRANT SELECT ON [dbo].[BreakInService] TO [aca-user] AS [dbo]
GO
 
CREATE TABLE [dbo].[measurementBreakInService](
      [measurementBreakInserviceId] [bigint] IDENTITY(1,1) NOT NULL,
      [EntityStatusId] [int] NOT NULL,
      [resourceId] [uniqueidentifier] NOT NULL,
      [measurement_id] [int] NOT NULL,
      [BreakInServiceId] [bigint] NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,
CONSTRAINT [PK_measurementBreakInService_measurementBreakInServiceId] PRIMARY KEY NONCLUSTERED
(
      [measurementBreakInserviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
 GO
 
ALTER TABLE [dbo].[measurementBreakInService] ADD  DEFAULT (newid()) FOR [resourceId]
GO
 
ALTER TABLE [dbo].[measurementBreakInService]  WITH CHECK ADD  CONSTRAINT [FK_measurementBreakInService_BreakInService] FOREIGN KEY([BreakInServiceId])
REFERENCES [dbo].[BreakInService] ([BreakInServiceId])
ON DELETE CASCADE
GO
 
ALTER TABLE [dbo].[measurementBreakInService] CHECK CONSTRAINT [FK_measurementBreakInService_BreakInService]
GO
 
ALTER TABLE [dbo].[measurementBreakInService]  WITH CHECK ADD  CONSTRAINT [FK_measurementBreakInService_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
 
ALTER TABLE [dbo].[measurementBreakInService] CHECK CONSTRAINT [FK_measurementBreakInService_EntityStatus]
GO
 
ALTER TABLE [dbo].[measurementBreakInService]  WITH CHECK ADD  CONSTRAINT [FK_measurementBreakInService_measurement] FOREIGN KEY([measurement_id])
REFERENCES [dbo].[measurement] ([measurement_id])
GO
 
ALTER TABLE [dbo].[measurementBreakInService] CHECK CONSTRAINT [FK_measurementBreakInService_measurement]
GO

GRANT SELECT ON [dbo].[measurementBreakInService] TO [aca-user] AS [dbo]
GO

-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_BreakInService]
      @CreatedBy nvarchar(50),
      @CreatedDate datetime2(7),
      @startDate datetime2(7),
      @endDate datetime2(7),
      @breakInServiceId int,
      @measurementId int,
      @week int
     
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
      INSERT INTO [dbo].[BreakInService] (EntityStatusId, CreatedBy, CreatedDate, startDate, endDate, Weeks)
      VALUES (1,@CreatedBy,@CreatedDate,@startDate,@endDate,@week)
   
      SELECT @breakInServiceId = BreakInServiceId FROM [dbo].[BreakInService]
      WHERE (startDate = @startDate AND endDate = @endDate) AND (CreatedBy = @CreatedBy AND CreatedDate = @CreatedDate)
 
      INSERT INTO [dbo].[measurementBreakInService] (EntityStatusId, measurement_id, BreakInServiceId, CreatedBy, CreatedDate)
      VALUES (1, @measurementId, @breakInServiceId, @CreatedBy, @CreatedDate)
END
GO 
GRANT EXECUTE ON [dbo].[INSERT_BreakInService] TO [aca-user] AS [dbo]
GO

-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_BreakInService]
      @employerId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    SELECT employer_id, bis.StartDate, bis.EndDate, bis.Weeks FROM [dbo].[measurement] mea INNER JOIN [dbo].[measurementBreakInService] mbis
      ON mea.measurement_id = mbis.measurement_id INNER JOIN [dbo].[BreakInService] bis ON mbis.BreakInServiceId = bis.BreakInServiceId
      WHERE mea.employer_id = @employerId
END
GO
GRANT EXECUTE ON [dbo].[SELECT_BreakInService] TO [aca-user] AS [dbo]
GO

-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_BreakInServiceStatus]
      @EntityStatus int,
      @BreakInServiceId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [dbo].[BreakInService] SET EntityStatusId = @EntityStatus
      WHERE BreakInServiceId = @BreakInServiceId
END
GO
GRANT EXECUTE ON [dbo].[UPDATE_BreakInServiceStatus] TO [aca-user] AS [dbo]
GO

ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
      @employerID int
AS
BEGIN TRY
 
--Equivalencies
DELETE
  aca.dbo.equivalency
  where employer_id=@employerID;
 
--Plan Year Archives/Insurance Offers
DELETE
  aca.dbo.employee_insurance_offer
  where employer_id=@employerID;
 
--Plan Year Archives/Insurance Offers
DELETE
  aca.dbo.employee_insurance_offer_archive
  where employer_id=@employerID;
 
DELETE
      aca.dbo.employee_dependents
      WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);
 
--Insurance Contributions
DELETE
aca.dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));
 
--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);
 
--Payroll Summer Averages.
DELETE
  aca.dbo.payroll_summer_averages
  where employer_id=@employerID;
 
--Employee Import Alerts.
DELETE
  aca.dbo.import_employee
  WHERE employerID=@employerID
 
--Payroll Import Alerts. Alerts that have been deleted by users.
DELETE aca.dbo.payroll_archive
      WHERE employer_id=@employerID
 
--Payroll Import Alerts.
DELETE
  aca.dbo.import_payroll
  WHERE employerid=@employerID
 
--Insurance Carrier Import Alerts
DELETE
  aca.dbo.import_insurance_coverage
  WHERE employer_id=@employerID;
 
--All Carrier Import Coverage
DELETE
  aca.dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);
 
--All Payroll.
DELETE
  [aca].[dbo].[payroll]
  WHERE employer_id=@employerID
 
--All Employees.
DELETE 
  FROM aca.dbo.employee
  WHERE employer_id=@employerID
 
--All HR Status Codes
DELETE
  aca.dbo.hr_status
  WHERE employer_id=@employerID
 
--All Gross Pay Filters
DELETE
  aca.dbo.gross_pay_filter
  WHERE employer_id=@employerID
 
--All Gross Pay Codes
DELETE
  aca.dbo.gross_pay_type
  WHERE employer_id=@employerID
 
--All Batch rows.
DELETE
  aca.dbo.batch
  WHERE employer_id=@employerID
 
--All Employee Classifications.
DELETE
  aca.dbo.employee_classification
  WHERE employer_id=@employerID
 
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO

CREATE PROCEDURE [dbo].[UPDATE_employeeType]
      -- Add the parameters for the stored procedure here
      @employerId int,
      @name varchar(50)
     
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [aca].[dbo].[employee_type] SET name = @name
      WHERE employer_id = @employerId
     
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[UPDATE_employeeType] TO [aca-user] AS [dbo]
GO
 
CREATE PROCEDURE [dbo].[INSERT_employeeType]
      -- Add the parameters for the stored procedure here
      @employerId int,
      @name varchar(50)
     
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    INSERT INTO [aca].[dbo].[employee_type] (name,employer_id)
      VALUES (@name, @employerId)
     
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[INSERT_employeeType] TO [aca-user] AS [dbo]
GO
 
CREATE PROCEDURE [dbo].[DELETE_employeeType]
      -- Add the parameters for the stored procedure here
      @employeeId int
     
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    DELETE [aca].[dbo].[employee_type]
      WHERE employee_type_id = @employeeId
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[DELETE_employeeType] TO [aca-user] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[UPDATE_BreakInService]    Script Date: 9/6/2016 4:31:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_BreakInService]
      @modifiedBy nvarchar(50),
      @BreakInServiceId int,
      @startDate datetime2(7),
      @endDate datetime2(7),
      @weeks int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [dbo].[BreakInService] SET StartDate = @startDate, EndDate = @endDate, Weeks = @weeks,
      ModifiedBy = @modifiedBy, ModifiedDate = GETDATE()
      WHERE BreakInServiceId = @BreakInServiceId
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[UPDATE_BreakInService] TO [aca-user] AS [dbo]
GO
 
ALTER PROCEDURE [dbo].[UPDATE_BreakInServiceStatus]
      @EntityStatus int,
      @modifiedBy nvarchar(50),
      @BreakInServiceId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [dbo].[BreakInService] SET EntityStatusId = @EntityStatus, ModifiedBy = @modifiedBy, ModifiedDate = GETDATE()
      WHERE BreakInServiceId = @BreakInServiceId
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[UPDATE_BreakInServiceStatus] TO [aca-user] AS [dbo]
GO
 
ALTER PROCEDURE [dbo].[INSERT_BreakInService]
      @CreatedBy nvarchar(50),
      @CreatedDate datetime2(7),
      @startDate datetime2(7),
      @endDate datetime2(7),
      @measurementId int,
      @week int
     
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
      DECLARE @ModifiedBy nvarchar(50) = @CreatedBy;
      DECLARE @ModifiedDate datetime2(7) = @CreatedDate;
 
      INSERT INTO [dbo].[BreakInService] (EntityStatusId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, startDate, endDate, Weeks)
      VALUES (1,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@startDate,@endDate,@week)
      DECLARE @breakInServiceId int;
      SELECT @breakInServiceId = BreakInServiceId FROM [dbo].[BreakInService]
      WHERE (startDate = @startDate AND endDate = @endDate) AND (CreatedBy = @CreatedBy AND CreatedDate = @CreatedDate)
 
      INSERT INTO [dbo].[measurementBreakInService] (EntityStatusId, measurement_id, BreakInServiceId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
      VALUES (1, @measurementId, @breakInServiceId, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate)
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[INSERT_BreakInService] TO [aca-user] AS [dbo]
GO
 
ALTER PROCEDURE [dbo].[SELECT_BreakInService]
	@measurementId int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT bis.BreakInServiceId, bis.CreatedBy, bis.CreatedDate, bis.StartDate, bis.EndDate, bis.EntityStatusId, bis.ResourceId,
	 bis.ModifiedBy, bis.ModifiedDate FROM [dbo].[BreakInService] bis INNER JOIN [dbo].[measurementBreakInService] mbis
	  ON bis.BreakInServiceId = mbis.BreakInServiceId
	WHERE mbis.measurement_id = @measurementId AND bis.EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [dbo].[SELECT_BreakInService] TO [aca-user] AS [dbo]
GO
-- end of ACA-Migration-012_to_013.sql

-- start of ACA-Migration-013_to_014.sql
/****** Object:  StoredProcedure [dbo].[RESET_EMPLOYER]    Script Date: 9/8/2016 10:28:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <03/13/2015>
-- Description:	<This stored procedure is meant to delete all employee_import records matching the batch id.>
-- Altered:
--				<11/30/2015> TLW
--					- Changed to handle new Foreign Key constraints. 
-- =============================================
ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
	@employerID int
AS
BEGIN TRY

--Equivalencies
DELETE 
  aca.dbo.equivalency
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  aca.dbo.employee_insurance_offer
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  aca.dbo.employee_insurance_offer_archive
  where employer_id=@employerID;

DELETE 
	aca.dbo.employee_dependents
	WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--Insurance Contributions
DELETE
 aca.dbo.insurance_contribution
 WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--Payroll Summer Averages. 
DELETE
  aca.dbo.payroll_summer_averages
  where employer_id=@employerID;

--Employee Import Alerts. 
DELETE
  aca.dbo.import_employee
  WHERE employerID=@employerID

--Employee Alert Archives. Alerts that have been deleted by users. 
DELETE aca.dbo.alert_archive
	WHERE employer_id=@employerID

--Payroll Import Alerts. Alerts that have been deleted by users. 
DELETE aca.dbo.payroll_archive
	WHERE employer_id=@employerID

--Payroll Import Alerts. 
DELETE
  aca.dbo.import_payroll
  WHERE employerid=@employerID

--Insurance Carrier Import Alerts
DELETE
  aca.dbo.import_insurance_coverage
  WHERE employer_id=@employerID;

--All Carrier Import Coverage
DELETE
  aca.dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--All Payroll. 
DELETE 
  [aca].[dbo].[payroll]
  WHERE employer_id=@employerID

--All Employees.
DELETE  
  FROM aca.dbo.employee
  WHERE employer_id=@employerID

--All HR Status Codes
DELETE
  aca.dbo.hr_status
  WHERE employer_id=@employerID

--All Gross Pay Filters
DELETE
  aca.dbo.gross_pay_filter
  WHERE employer_id=@employerID

--All Gross Pay Codes
DELETE
  aca.dbo.gross_pay_type
  WHERE employer_id=@employerID

--All Measurement Periods. 
DELETE
  aca.dbo.measurement
  WHERE employer_id=@employerID

--All Plan Years. 
DELETE
  aca.dbo.plan_year
  WHERE employer_id=@employerID

--All assigned Alerts. 
DELETE
  aca.dbo.alert
  WHERE employer_id=@employerID

--All Batch rows. 
DELETE
  aca.dbo.batch
  WHERE employer_id=@employerID

END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_employeeType]
       -- Add the parameters for the stored procedure here
       @employeeId int,
       @name varchar(50)
       
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    UPDATE [aca].[dbo].[employee_type] SET name = @name
       WHERE employee_type_id = @employeeId
       
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO
-- end of ACA-Migration-013_to_014.sql

-- start of ACA-Migration-014_to_015.sql

ALTER TABLE dbo.employee DROP CONSTRAINT fk_employee_stateID
ALTER TABLE dbo.employee ALTER COLUMN state_id integer NULL
ALTER TABLE dbo.employer ADD DBAName varchar(100) NULL
GO
ALTER PROCEDURE [dbo].[INSERT_new_employer]
	@name varchar(50),
	@add varchar(50),
	@city varchar(50),
	@stateID int,
	@zip varchar(15),
	@logo varchar(50),
	@b_add varchar(50),
	@b_city varchar(50),
	@b_stateID int,
	@b_zip varchar(15),
	@empTypeID int,
	@ein varchar(50),
	@dbaName varchar(100),
	@empid int OUTPUT
AS

BEGIN TRY
	INSERT INTO [employer](
		name,
		[address],
		city,
		state_id,
		zip,
		img_logo,
		bill_address,
		bill_city,
		bill_state,
		bill_zip,
		employer_type_id, 
		ein,
		DBAName)
	VALUES(
		@name,
		@add,
		@city,
		@stateID,
		@zip,
		@logo,
		@b_add,
		@b_city,
		@b_stateID,
		@b_zip,
		@empTypeID,
		@ein,
		@dbaName)

SELECT @empid = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_employer]
       @employerID int,
       @name varchar(50),
       @address varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @logo varchar(50),
       @ein varchar(50),
       @employerTypeId int,
	   @dbaName varchar(100)
AS
BEGIN TRY
       UPDATE employer
       SET
             name = @name, 
             [address] = @address,
             city = @city,
             state_id = @stateID, 
             zip = @zip,  
             img_logo = @logo,
             ein = @ein,
             employer_type_id = @employerTypeId,
			 DBAName = @dbaName
       WHERE
             employer_id = @employerID;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[DELETE_payroll_import_row]
	@rowID int
AS
BEGIN TRY
	DELETE FROM import_payroll
	WHERE rowid=@rowID;

END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[DELETE_payroll_import]
	@batchID int, 
	@modBy varchar(50), 
	@modOn datetime
AS
BEGIN
	BEGIN TRANSACTION
		BEGIN TRY
			--Archive any payroll records that were in the ACT system related to the batch being removed.  
			INSERT INTO payroll_archive (row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, modBy, modOn, history)
			SELECT row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, @modBy, @modOn, history
			FROM payroll
			WHERE batch_id=@batchID;

			--Remove the actual payroll records related to the batch id. 
			DELETE FROM payroll
			WHERE batch_id=@batchID;

			-- Remove the payroll records with batch id that are stuck in the payroll import table. 
			-- Note we are not archiving these records as they were never used to average any employees hours. 
			DELETE FROM import_payroll
			WHERE batchid=@batchID;

			UPDATE batch 
			SET
				delBy=@modBy, 
				delOn=@modOn
			WHERE
				batch_id=@batchID;
		
			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			exec dbo.INSERT_ErrorLogging
		END CATCH
END
GO
alter Table [aca].[dbo].[plan_year] add 
	[default_meas_start] [datetime] NULL,
	[default_meas_end] [datetime] NULL,
	[default_admin_start] [datetime] NULL,
	[default_admin_end] [datetime] NULL,
	[default_open_start] [datetime] NULL,
	[default_open_end] [datetime] NULL,
	[default_stability_start] [datetime] NULL,
	[default_stability_end] [datetime] NULL;

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/19/2014>
-- Description:	<This stored procedure is meant to create a new Pay Roll record.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_new_plan_year]
	@employerID int,
	@description varchar(50),
	@startDate datetime,
	@endDate datetime,
	@notes varchar(max),
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@default_Meas_Start datetime,
	@default_Meas_End datetime,
	@default_Admin_Start datetime,
	@default_Admin_End datetime,
	@default_Open_Start datetime,
	@default_Open_End datetime,
	@default_Stability_Start datetime,
	@default_Stability_End datetime,

	@planyearid int OUTPUT
AS

BEGIN
	INSERT INTO [plan_year](
		employer_id,
		[description],
		startDate,
		endDate,
		notes,
		history,
		modOn,
		modBy, 
		default_Meas_Start,
		default_Meas_End,
		default_Admin_Start,
		default_Admin_End,
		default_Open_Start,
		default_Open_End,
		default_Stability_Start,
		default_Stability_End)
	VALUES(
		@employerID,
		@description,
		@startDate,
		@endDate,
		@notes,
		@history,
		@modOn,
		@modBy,
		@default_Meas_Start,
		@default_Meas_End,
		@default_Admin_Start,
		@default_Admin_End,
		@default_Open_Start,
		@default_Open_End,
		@default_Stability_Start,
		@default_Stability_End)

SELECT @planyearid = SCOPE_IDENTITY();
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to update the plan year.>
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_plan_year]
	@planyearID int,
	@description varchar(50),
	@sDate datetime,
	@eDate datetime,
	@notes varchar(max),
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@default_Meas_Start datetime,
	@default_Meas_End datetime,
	@default_Admin_Start datetime,
	@default_Admin_End datetime,
	@default_Open_Start datetime,
	@default_Open_End datetime,
	@default_Stability_Start datetime,
	@default_Stability_End datetime
AS
BEGIN
	UPDATE plan_year
	SET
		description=@description,
		startDate = @sDate,
		endDate = @eDate,
		notes = @notes,
		history = @history,
		modOn = @modOn,
		modBy = @modBy,
		default_Meas_Start=@default_Meas_Start,
		default_Meas_End=@default_Meas_End,
		default_Admin_Start=@default_Admin_Start,
		default_Admin_End=@default_Admin_End,
		default_Open_Start=@default_Open_Start,
		default_Open_End=@default_Open_End,
		default_Stability_Start=@default_Stability_Start,
		default_Stability_End=@default_Stability_End
	WHERE
		plan_year_id=@planyearID;

END
GO

----------------------------------------------------
--Create Table ArchiveFileInfo
----------------------------------------------------
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ArchiveFileInfo](
	[ArchiveFileInfoId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployerId] [bigint] NOT NULL,
	[EmployerGuid] [uniqueidentifier] NOT NULL,
	[ArchivedTime] [datetime] NOT NULL,
	[FileName] [nvarchar](256) NOT NULL,
	[SourceFilePath] [nvarchar](256) NOT NULL,
	[ArchiveFilePath] [nvarchar](256) NOT NULL,
	[ArchiveReason] [nvarchar](256) NULL,
	--Standard Items
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ArchiveFileInfo] PRIMARY KEY NONCLUSTERED 
(
	[ArchiveFileInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ArchiveFileInfo] ADD  CONSTRAINT [DF_ArchiveFileInfo_resourceId]  DEFAULT (newid()) FOR [ResourceId]
GO

ALTER TABLE [dbo].[ArchiveFileInfo]  WITH CHECK ADD  CONSTRAINT [FK_ArchiveFileInfo_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[ArchiveFileInfo] CHECK CONSTRAINT [FK_ArchiveFileInfo_EntityStatus]
GO

GRANT SELECT ON [dbo].[ArchiveFileInfo] TO [aca-user] AS [dbo]
---------------------------------------------------
--Create Stored Procs For ArchiveFileInfo
---------------------------------------------------
--Select
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/23/2016
-- Description: Select a Single Row of 
-- =============================================
Create PROCEDURE [dbo].[SELECT_ArchiveFileInfo]
      @ArchiveFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[ArchiveFileInfo] 
      WHERE [ArchiveFileInfoId] = @ArchiveFileInfoId;
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
--Select for employer
GO
GRANT EXECUTE ON [dbo].[SELECT_ArchiveFileInfo] TO [aca-user] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/23/2016
-- Description: Select a Single Row of 
-- =============================================
Create PROCEDURE [dbo].[SELECT_ArchiveFileInfo_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[ArchiveFileInfo] 
      WHERE [EmployerId] = @EmployerId;
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
--Insert
END CATCH
GO
GRANT EXECUTE ON [dbo].[SELECT_ArchiveFileInfo_ForEmployer] TO [aca-user] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 9/23/2016
-- Description:	Insert a new Import for coversion into the table
-- =============================================
Create PROCEDURE [dbo].[INSERT_ArchiveFileInfo]
	@EmployerGuid uniqueidentifier,
	@FileName nvarchar(256),
	@SourceFilePath nvarchar(256),
	@ArchiveFilePath nvarchar(256),
	@ArchiveReason nvarchar(256),
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @EmployerId bigint;
	select @EmployerId = [employer_id] from employer where [ResourceId] = @EmployerGuid;

	INSERT INTO [dbo].[ArchiveFileInfo](
		[EmployerId], [EmployerGuid], [ArchivedTime], [FileName], [SourceFilePath], [ArchiveFilePath], [ArchiveReason],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
	VALUES (@EmployerId, @EmployerGuid, GETDATE(), @FileName, @SourceFilePath, @ArchiveFilePath, @ArchiveReason,
		1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

	SELECT @insertedID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
GRANT EXECUTE ON [dbo].[INSERT_ArchiveFileInfo] TO [aca-user] AS [dbo]
GO

-- end of ACA-Migration-014_to_015.sql

-- start of ACA-Migration-015_to_016.sql
/****** Object:  Table [dbo].[NightlyCalculation1]    Script Date: 9/29/2016 9:58:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NightlyCalculation](
	[CalculationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[EmployerId] [int] NOT NULL,
	[BatchId] [bigint] NOT NULL,
	[ProcessStatus] [bit] NOT NULL,
	[ProcessFail] [bit] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[NightlyCalculation] ADD  CONSTRAINT [DF_NightlyCalculation_ResourceId]  DEFAULT (newid()) FOR [ResourceId]
GO
GRANT SELECT ON [dbo].[NightlyCalculation] TO [aca-user] AS [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_NightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_NightlyCalculation] AS SET NOCOUNT ON;')
GO
GRANT EXECUTE ON [dbo].[INSERT_NightlyCalculation] TO [aca-user] AS [dbo]
GO
ALTER PROCEDURE [dbo].[INSERT_NightlyCalculation]
	@EmployerId int,
	@CreatedBy nvarchar(50)
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[NightlyCalculation] (CreatedBy,
		CreatedDate,
		ModifiedBy,
		ModifiedDate,
		EmployerId,
		BatchId,
		ProcessStatus,
		ProcessFail)
	VALUES (@CreatedBy,
		GETDATE(),
		@CreatedBy,
		GETDATE(),
		@EmployerId,
		0,
		0,
		0)
   
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
--IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_NightlyCalculation' AND type = 'P') 
--	EXEC('CREATE PROCEDURE [dbo].[SELECT_NightlyCalculation] AS SET NOCOUNT ON;')
--GO
--GRANT EXECUTE ON [dbo].[SELECT_NightlyCalculation] TO [aca-user] AS [dbo]
--GO
--ALTER PROCEDURE [dbo].[SELECT_NightlyCalculation]
	
--AS
--BEGIN TRY
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	SET NOCOUNT ON;

--	SELECT CalculationId,
--		CreatedBy,
--		CreatedDate,
--		ModifiedBy,
--		ModifiedDate,
--		EmployerId,
--		BatchId,
--		ProcessStatus,
--		ProcessFail 
--	FROM [dbo].[NightlyCalculation]
--	WHERE ProcessStatus = 0 AND ProcessFail != 1;
	   
--END TRY
--BEGIN CATCH
--	EXEC INSERT_ErrorLogging
--END CATCH
--GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_FailNightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_FailNightlyCalculation] AS SET NOCOUNT ON;')
GO
GRANT EXECUTE ON [dbo].[UPDATE_FailNightlyCalculation] TO [aca-user] AS [dbo]
GO
ALTER PROCEDURE [dbo].[UPDATE_FailNightlyCalculation]
	@employerId as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[NightlyCalculation] SET processStatus = 0, processFail = 1
	WHERE EmployerId = @employerId

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_NightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_NightlyCalculation] AS SET NOCOUNT ON;')
GO
GRANT EXECUTE ON [dbo].[UPDATE_NightlyCalculation] TO [aca-user] AS [dbo]
GO
ALTER PROCEDURE [dbo].[UPDATE_NightlyCalculation]
	@employerId as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[NightlyCalculation] SET processStatus = 1, processFail = 0
	WHERE EmployerId = @employerId
	
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

-- end of ACA-Migration-015_to_016.sql

-- start of ACA-Migration-016_to_017.sql

-----------------------------------------------------
-- Create Tables
-----------------------------------------------------

GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EmployeeMeasurementAverageHours]
(
	[EmployeeMeasurementAverageHoursId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[MeasurementId] [int] NOT NULL,
	[WeeklyAverageHours] [numeric](18, 4) NULL,	
	[MonthlyAverageHours] [numeric](18, 4) NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL 
	CONSTRAINT [DF_EmployeeMeasurementAverageHours_resourceId] DEFAULT (newid()),
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,

	CONSTRAINT [PK_EmployeeMeasurementAverageHours] PRIMARY KEY NONCLUSTERED 
	(
	[EmployeeMeasurementAverageHoursId] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
	ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMeasurementAverageHours_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMeasurementAverageHours_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[employee] ([employee_id])
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMeasurementAverageHours_Measurement] FOREIGN KEY([MeasurementId])
REFERENCES [dbo].[measurement] ([measurement_id])
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] CHECK CONSTRAINT [FK_EmployeeMeasurementAverageHours_EntityStatus]
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] CHECK CONSTRAINT [FK_EmployeeMeasurementAverageHours_Employee]
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] CHECK CONSTRAINT [FK_EmployeeMeasurementAverageHours_Measurement]
GO


SET ANSI_PADDING OFF
GO


-------------------------------------------------------------
--Create Stored Procs
-------------------------------------------------------------

-- SELECT

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Select a Single Row of 
-- =============================================
Create PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours]
      @Id int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeMeasurementAverageHoursId] = @Id;
END
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Select a Single Row of 
-- =============================================
Create PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ByEmployeeMeasurement]
      @employeeId int,
	  @measurementId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeId] = @employeeId 
		AND [MeasurementId] = @measurementId
	    AND [EntityStatusId] = 1;
END
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Select all Rows of AverageHours for an Employee
-- =============================================
Create PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ForEmployee]
      @employeeId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeId] = @employeeId AND [EntityStatusId] = 1;
END
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Select all Rows of AverageHours for a Measurement Period
-- =============================================
Create PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ForMeasurement]
      @measurementId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [MeasurementId] = @measurementId AND [EntityStatusId] = 1;
END
GO


--INSERT / UPDATE 


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 10/6/2016
-- Description:	Upsert: update or insert AverageHours into the table
-- =============================================
Create PROCEDURE [dbo].[UPSERT_AverageHours]
	@employeeId int,
	@measurementId int,
	@weeklyAverageHours numeric(18, 4),
	@monthlyAverageHours numeric(18, 4),
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	MERGE [dbo].[EmployeeMeasurementAverageHours]  AS T  
	USING (
			SELECT @employeeId employeeId,
			@measurementId measurementId,
			@weeklyAverageHours weeklyAverageHours, 
			@monthlyAverageHours monthlyAverageHours, 
			@CreatedBy CreatedBy
		) AS S 
	ON T.EmployeeId = S.employeeId AND T.MeasurementId = S.measurementId
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[WeeklyAverageHours] = S.weeklyAverageHours,
		T.[MonthlyAverageHours] = S.monthlyAverageHours,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE(),
		@CreatedBy = T.[EmployeeMeasurementAverageHoursId]
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[EmployeeId], [MeasurementId], [WeeklyAverageHours], [MonthlyAverageHours],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employeeId, S.measurementId, S.weeklyAverageHours, S.monthlyAverageHours, 
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );

	SELECT @insertedID = SCOPE_IDENTITY();

END
GO

GO
create type Bulk_AverageHours as table
(
	EmployeeMeasurementAverageHoursId int,
	EmployeeId int,
	MeasurementId int,
	WeeklyAverageHours numeric(18, 4),
	MonthlyAverageHours numeric(18, 4)
);	

GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/7/2016
-- Description:	Bulk Insert or Update Calculation Averages
-- =============================================
CREATE PROCEDURE [dbo].[BULK_UPSERT_AverageHours]	
	@averages Bulk_AverageHours readonly,
	@CreatedBy nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	MERGE [dbo].[EmployeeMeasurementAverageHours]  AS T  
	USING (
			SELECT EmployeeId employeeId,
			MeasurementId measurementId,
			WeeklyAverageHours weeklyAverageHours, 
			MonthlyAverageHours monthlyAverageHours, 
			@CreatedBy CreatedBy From @averages
		) AS S 
	ON T.EmployeeId = S.employeeId AND T.MeasurementId = S.measurementId
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[WeeklyAverageHours] = S.weeklyAverageHours,
		T.[MonthlyAverageHours] = S.monthlyAverageHours,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE(),
		@CreatedBy = T.[EmployeeMeasurementAverageHoursId]
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[EmployeeId], [MeasurementId], [WeeklyAverageHours], [MonthlyAverageHours],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employeeId, S.measurementId, S.weeklyAverageHours, S.monthlyAverageHours, 
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );
END
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Set the entity Status
-- =============================================
Create PROCEDURE [dbo].[Update_EmployeeMeasurementAverageHours_EntityStatus]
	@employeeId int,
	@measurementId int,
	@modifiedBy nvarchar(50),
	@EntityStatus int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    UPDATE [dbo].[EmployeeMeasurementAverageHours] 
	SET EntityStatusId = @EntityStatus
	WHERE [MeasurementId] = @measurementId AND [EmployeeId]= @employeeId;
END
GO





-----------------------------------------------------------------------
-- Exising Tables
-----------------------------------------------------------------------

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_All_BreakInService_ForEmployer]
      @employerId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT bis.BreakInServiceId, bis.CreatedBy, bis.CreatedDate, bis.StartDate, bis.EndDate, 
		   bis.EntityStatusId, bis.ResourceId, bis.ModifiedBy, bis.ModifiedDate, mbis.measurement_id  
	  FROM 
	  [dbo].[BreakInService] bis 
	  INNER JOIN [dbo].[measurementBreakInService] mbis ON bis.BreakInServiceId = mbis.BreakInServiceId 
	  INNER JOIN [dbo].[Measurement] meas ON mbis.measurement_id = meas.measurement_id
      WHERE meas.employer_id = @employerId AND bis.EntityStatusId = 1;
END
GO

GO
create type Bulk_Employee_AverageHours as table
(
	employee_id int,
	pyAvg numeric(18,4),
	lpyAvg numeric(18,4),
	mpyAvg numeric(18,4), 
	impAvg numeric(18,4)
);	

GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/7/2016
-- Description:	Bulk Update Data to speed up uploading based on <Travis Wells> UPDATE_employee_AVG_MONTHLY_HOURS
-- =============================================
CREATE PROCEDURE [dbo].[BULK_UPDATE_employee_AVG_MONTHLY_HOURS]
	@employeeHours Bulk_Employee_AverageHours readonly
AS
BEGIN
	UPDATE employee
	SET
		plan_year_avg_hours=a.pyAvg,
		limbo_plan_year_avg_hours=a.lpyAvg,
		meas_plan_year_avg_hours=a.mpyAvg,
		imp_plan_year_avg_hours=a.impAvg
	FROM @employeeHours a
	JOIN employee b ON b.employee_id=a.employee_id;
END
GO



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/10/2016
-- Description:	Select all payroll for an employer based on <Travis Wells> [SELECT_employee_payroll]
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_payroll]
	@employerId int
AS
BEGIN
	SELECT * FROM View_payroll
	WHERE employer_id=@employerId 
	ORDER BY employee_id, edate;
END
GO


-- fix nuking ----------------------------------

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
      @employerID int
AS
BEGIN TRY

--Plan Year Archives/Insurance Offers
DELETE
  dbo.employee_insurance_offer
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  dbo.employee_insurance_offer_archive
  where employer_id=@employerID;

--All Carrier Import Coverage
DELETE
  dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--
DELETE
	dbo.insurance_coverage_editable
	where employer_id=@employerID;

--Insurance Carrier Import Alerts
DELETE
  dbo.import_insurance_coverage
  WHERE employer_id=@employerID;

--All Measurement Periods. 
CREATE TABLE #entityValue (value integer)

INSERT INTO #entityValue (value)
SELECT BreakInServiceId
FROM dbo.measurementBreakInService
WHERE measurement_id IN (SELECT measurement_id FROM [dbo].[measurement] WHERE employer_id = @employerId)

--Measurement Break In Service
DELETE dbo.measurementBreakInService WHERE measurement_id IN (SELECT measurement_id FROM [dbo].[measurement] WHERE employer_id = @employerId)

--Break in Service
UPDATE dbo.BreakInService SET EntityStatusId = 3 WHERE BreakInServiceId IN (SELECT Value FROM #entityValue)

DROP TABLE #entityValue

--Insurance Contributions
DELETE
dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

-- Tax Year
DELETE
	dbo.tax_year_1095c_approval
	where employer_id=@employerID;

--Payroll Summer Averages. 
DELETE
  dbo.payroll_summer_averages
  where employer_id=@employerID;

--All Payroll. 
DELETE 
  dbo.payroll
  WHERE employer_id=@employerID
  
--Payroll Import Alerts. Alerts that have been deleted by users. 
DELETE dbo.payroll_archive
      WHERE employer_id=@employerID

-- dependents
DELETE 
      dbo.employee_dependents
      WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

-- must clear out average hours before clearing measurement period 
DELETE dbo.EmployeeMeasurementAverageHours where MeasurementId in (Select measurement_id from dbo.measurement WHERE employer_id = @employerId);

--Measurement 
DELETE dbo.measurement WHERE employer_id = @employerId;

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--All Employees.
DELETE  
  FROM dbo.employee
  WHERE employer_id=@employerID
  
--All Gross Pay Filters
DELETE
  dbo.gross_pay_filter
  WHERE employer_id=@employerID

--Employee Alert Archives. Alerts that have been deleted by users. 
DELETE dbo.alert_archive
      WHERE employer_id=@employerID

--All Gross Pay Codes
DELETE
  dbo.gross_pay_type
  WHERE employer_id=@employerID

--All Plan Years. 
DELETE
  dbo.plan_year
  WHERE employer_id=@employerID

  -- tax year approval
DELETE
  dbo.tax_year_approval
  WHERE employer_id=@employerID

--All Batch rows. 
DELETE
  dbo.batch
  WHERE employer_id=@employerID

  -- Theses are not referenced at all in the spreadsheet
  
--Equivalencies
DELETE 
  dbo.equivalency
  where employer_id=@employerID;

--Employee Import Alerts. 
DELETE
  dbo.import_employee
  WHERE employerID=@employerID
  
--Payroll Import Alerts. 
DELETE
  dbo.import_payroll
  WHERE employerid=@employerID

--All HR Status Codes
DELETE
  dbo.hr_status
  WHERE employer_id=@employerID

END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH



-----------------------------



GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeMeasurementAverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeMeasurementAverageHours_ForEmployee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeMeasurementAverageHours_ForMeasurement] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeMeasurementAverageHours_ByEmployeeMeasurement] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPSERT_AverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_UPSERT_AverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Update_EmployeeMeasurementAverageHours_EntityStatus] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_All_BreakInService_ForEmployer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_UPDATE_employee_AVG_MONTHLY_HOURS] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_AverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_Employee_AverageHours] TO [aca-user] AS [dbo]
GO

-- end of ACA-Migration-016_to_017.sql

-- start of ACA-Migration-017_to_018.sql

ALTER TABLE [dbo].[ArchiveFileInfo] ALTER COLUMN EmployerId integer NOT NULL
GO
ALTER TABLE [dbo].[ArchiveFileInfo]  WITH NOCHECK ADD CONSTRAINT [FK_ArchiveFileInfo_employer] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[NightlyCalculation] WITH NOCHECK ADD CONSTRAINT [FK_NightlyCalculation_employer] FOREIGN KEY([employerId])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[NightlyCalculation] ADD CONSTRAINT PK_CalculationId PRIMARY KEY (CalculationId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_alert_ResourceId')
	DROP INDEX ak_alert_ResourceId ON [dbo].[alert]
GO
CREATE UNIQUE INDEX ak_alert_ResourceId ON [dbo].[alert] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_ArchiveFileInfo_ResourceId')
	DROP INDEX ak_ArchiveFileInfo_ResourceId ON [dbo].[ArchiveFileInfo]
GO
CREATE UNIQUE INDEX ak_ArchiveFileInfo_ResourceId ON [dbo].[ArchiveFileInfo] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_batch_ResourceId')
	DROP INDEX ak_batch_ResourceId ON [dbo].[batch]
GO
CREATE UNIQUE INDEX ak_batch_ResourceId ON [dbo].[batch] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_BreakInService_ResourceId')
	DROP INDEX ak_BreakInService_ResourceId ON [dbo].[BreakInService]
GO
CREATE UNIQUE INDEX ak_BreakInService_ResourceId ON [dbo].[BreakInService] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_ResourceId')
	DROP INDEX ak_employee_ResourceId ON [dbo].[employee]
GO
CREATE UNIQUE INDEX ak_employee_ResourceId ON [dbo].[employee] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_classification_ResourceId')
	DROP INDEX ak_employee_classification_ResourceId ON [dbo].[employee_classification]
GO
CREATE UNIQUE INDEX ak_employee_classification_ResourceId ON [dbo].[employee_classification] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_type_ResourceId')
	DROP INDEX ak_employee_type_ResourceId ON [dbo].[employee_type]
GO
CREATE UNIQUE INDEX ak_employee_type_ResourceId ON [dbo].[employee_type] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employer_ResourceId')
	DROP INDEX ak_employer_ResourceId ON [dbo].[employer]
GO
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
CREATE UNIQUE INDEX ak_employer_ResourceId ON [dbo].[employer] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_equivalency_ResourceId')
	DROP INDEX ak_equivalency_ResourceId ON [dbo].[equivalency]
GO
CREATE UNIQUE INDEX ak_equivalency_ResourceId ON [dbo].[equivalency] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_gross_pay_type_ResourceId')
	DROP INDEX ak_gross_pay_type_ResourceId ON [dbo].[gross_pay_type]
GO
CREATE UNIQUE INDEX ak_gross_pay_type_ResourceId ON [dbo].[gross_pay_type] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_hr_status_ResourceId')
	DROP INDEX ak_hr_status_ResourceId ON [dbo].[hr_status]
GO
CREATE UNIQUE INDEX ak_hr_status_ResourceId ON [dbo].[hr_status] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_import_employee_ResourceId')
	DROP INDEX ak_import_employee_ResourceId ON [dbo].[import_employee]
GO
CREATE UNIQUE INDEX ak_import_employee_ResourceId ON [dbo].[import_employee] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_import_payroll_ResourceId')
	DROP INDEX ak_import_payroll_ResourceId ON [dbo].[import_payroll]
GO
CREATE UNIQUE INDEX ak_import_payroll_ResourceId ON [dbo].[import_payroll] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_measurement_ResourceId')
	DROP INDEX ak_measurement_ResourceId ON [dbo].[measurement]
GO
CREATE UNIQUE INDEX ak_measurement_ResourceId ON [dbo].[measurement] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_measurementBreakInService_ResourceId')
	DROP INDEX ak_measurementBreakInService_ResourceId ON [dbo].[measurementBreakInService]
GO
CREATE UNIQUE INDEX ak_measurementBreakInService_ResourceId ON [dbo].[measurementBreakInService] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_payroll_ResourceId')
	DROP INDEX ak_payroll_ResourceId ON [dbo].[payroll]
GO
CREATE UNIQUE INDEX ak_payroll_ResourceId ON [dbo].[payroll] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_payroll_summer_averages_ResourceId')
	DROP INDEX ak_payroll_summer_averages_ResourceId ON [dbo].[payroll_summer_averages]
GO
CREATE UNIQUE INDEX ak_payroll_summer_averages_ResourceId ON [dbo].[payroll_summer_averages] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_plan_year_ResourceId')
	DROP INDEX ak_plan_year_ResourceId ON [dbo].[plan_year]
GO
CREATE UNIQUE INDEX ak_plan_year_ResourceId ON [dbo].[plan_year] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_user_ResourceId')
	DROP INDEX ak_user_ResourceId ON [dbo].[user]
GO
CREATE UNIQUE INDEX ak_user_ResourceId ON [dbo].[user] (ResourceId)
GO
--this is a stub
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_alert_archive_ResourceId')
	DROP INDEX ak_alert_archive_ResourceId ON [dbo].[alert_archive]
GO
CREATE UNIQUE INDEX ak_alert_archive_ResourceId ON [dbo].[alert_archive] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_dependents_ResourceId')
	DROP INDEX ak_employee_dependents_ResourceId ON [dbo].[employee_dependents]
GO
CREATE UNIQUE INDEX ak_employee_dependents_ResourceId ON [dbo].[employee_dependents] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_insurance_offer_ResourceId')
	DROP INDEX ak_employee_insurance_offer_ResourceId ON [dbo].[employee_insurance_offer]
GO
CREATE UNIQUE INDEX ak_employee_insurance_offer_ResourceId ON [dbo].[employee_insurance_offer] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_insurance_offer_archive_ResourceId')
	DROP INDEX ak_employee_insurance_offer_archive_ResourceId ON [dbo].[employee_insurance_offer_archive]
GO
CREATE UNIQUE INDEX ak_employee_insurance_offer_archive_ResourceId ON [dbo].[employee_insurance_offer_archive] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_gross_pay_filter_ResourceId')
	DROP INDEX ak_gross_pay_filter_ResourceId ON [dbo].[gross_pay_filter]
GO
CREATE UNIQUE INDEX ak_gross_pay_filter_ResourceId ON [dbo].[gross_pay_filter] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_import_insurance_coverage_ResourceId')
	DROP INDEX ak_import_insurance_coverage_ResourceId ON [dbo].[import_insurance_coverage]
GO
CREATE UNIQUE INDEX ak_import_insurance_coverage_ResourceId ON [dbo].[import_insurance_coverage] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_import_insurance_coverage_archive_ResourceId')
	DROP INDEX ak_import_insurance_coverage_archive_ResourceId ON [dbo].[import_insurance_coverage_archive]
GO
CREATE UNIQUE INDEX ak_import_insurance_coverage_archive_ResourceId ON [dbo].[import_insurance_coverage_archive] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_insurance_ResourceId')
	DROP INDEX ak_insurance_ResourceId ON [dbo].[insurance]
GO
CREATE UNIQUE INDEX ak_insurance_ResourceId ON [dbo].[insurance] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_insurance_coverage_ResourceId')
	DROP INDEX ak_insurance_coverage_ResourceId ON [dbo].[insurance_coverage]
GO
CREATE UNIQUE INDEX ak_insurance_coverage_ResourceId ON [dbo].[insurance_coverage] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_insurance_coverage_editable_ResourceId')
	DROP INDEX ak_insurance_coverage_editable_ResourceId ON [dbo].[insurance_coverage_editable]
GO
CREATE UNIQUE INDEX ak_insurance_coverage_editable_ResourceId ON [dbo].[insurance_coverage_editable] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_invoice_ResourceId')
	DROP INDEX ak_invoice_ResourceId ON [dbo].[invoice]
GO
CREATE UNIQUE INDEX ak_invoice_ResourceId ON [dbo].[invoice] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_NightlyCalculation_ResourceId')
	DROP INDEX ak_NightlyCalculation_ResourceId ON [dbo].[NightlyCalculation]
GO
CREATE UNIQUE INDEX ak_NightlyCalculation_ResourceId ON [dbo].[NightlyCalculation] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_payroll_archive_ResourceId')
	DROP INDEX ak_payroll_archive_ResourceId ON [dbo].[payroll_archive]
GO
CREATE UNIQUE INDEX ak_payroll_archive_ResourceId ON [dbo].[payroll_archive] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_tax_year_1095c_approval_ResourceId')
	DROP INDEX ak_tax_year_1095c_approval_ResourceId ON [dbo].[tax_year_1095c_approval]
GO
CREATE UNIQUE INDEX ak_tax_year_1095c_approval_ResourceId ON [dbo].[tax_year_1095c_approval] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_tax_year_approval_ResourceId')
	DROP INDEX ak_tax_year_approval_ResourceId ON [dbo].[tax_year_approval]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UploadedFileInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UploadedFileInfo]
(
	[UploadedFileInfoId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployerId] [int] NOT NULL,
	[UploadedByUser] [nvarchar](50) NOT NULL,
	[UploadTime] [datetime] NOT NULL,
	[UploadSourceDescription] [nvarchar](50) NOT NULL,
	[UploadTypeDescription] [nvarchar](50) NOT NULL,
	[FileTypeDescription] [nvarchar](50) NOT NULL,
	[FileName] [nvarchar](256) NOT NULL,
	[Processed] [bit] NOT NULL,
	[FailedProcessing] [bit] NOT NULL,
	[ArchiveFileInfoId] [bigint] NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL 
	CONSTRAINT [DF_UploadedFileInfo_resourceId] DEFAULT (newid()),
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,

	CONSTRAINT [PK_UploadedFileInfo] PRIMARY KEY NONCLUSTERED 
	(
	[UploadedFileInfoId] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
	ON [PRIMARY]
) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[UploadedFileInfo]  WITH CHECK ADD  CONSTRAINT [FK_UploadedFileInfo_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[UploadedFileInfo]  WITH CHECK ADD  CONSTRAINT [FK_UploadedFileInfo_Employer] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[employer] ([employer_id])
GO

ALTER TABLE [dbo].[UploadedFileInfo]  WITH CHECK ADD  CONSTRAINT [FK_UploadedFileInfo_Archive] FOREIGN KEY([ArchiveFileInfoId])
REFERENCES [dbo].[ArchiveFileInfo] ([ArchiveFileInfoId])
GO

ALTER TABLE [dbo].[UploadedFileInfo] CHECK CONSTRAINT [FK_UploadedFileInfo_EntityStatus]
GO

ALTER TABLE [dbo].[UploadedFileInfo] CHECK CONSTRAINT [FK_UploadedFileInfo_Employer]
GO

ALTER TABLE [dbo].[UploadedFileInfo] CHECK CONSTRAINT [FK_UploadedFileInfo_Archive]
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_UploadedFileInfo_ResourceId')
	DROP INDEX ak_UploadedFileInfo_ResourceId ON [dbo].[UploadedFileInfo]
GO
CREATE UNIQUE INDEX ak_UploadedFileInfo_ResourceId ON [dbo].[UploadedFileInfo] (ResourceId)
GO

IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo] AS SET NOCOUNT ON;')
GO
SET ANSI_PADDING OFF
GO
----------------UploadedFileInfo Procs----------------------------------------
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select a Single Row of 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo]
      @UploadedFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo_ForEmployer' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo_ForEmployer] AS SET NOCOUNT ON;')
GO
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo for an Employer
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_ForEmployer]
      @employerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [EmployerId] = @employerId AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo_Unprocessed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo_Unprocessed] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo that are unprocessed
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_Unprocessed]
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [Processed] = 0  AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_EmployerIds_UploadedFileInfo_Unprocessed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_EmployerIds_UploadedFileInfo_Unprocessed] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/11/2016
-- Description: Select all The Employer Ids Rows of UploadedFileInfo that are unprocessed
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_EmployerIds_UploadedFileInfo_Unprocessed]
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT DISTINCT EmployerId FROM [dbo].[UploadedFileInfo] 
      WHERE [Processed] = 0 AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo_Failed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo_Failed] AS SET NOCOUNT ON;')

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/10/2016
-- Description: Select all Rows of UploadedFileInfo that Failed Processing
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_Failed]
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [FailedProcessing] = 1 AND [Processed] = 0;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo_UnprocessedForEmployer' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo_UnprocessedForEmployer] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo for an Employer that are unprocessed
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_UnprocessedForEmployer]
		@employerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [Processed] = 0 AND [EmployerId] = @employerId AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_UploadedFileInfo' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_UploadedFileInfo] AS SET NOCOUNT ON;')
--Insert
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Long, Vu; Ryan, Mccully
-- Create date: 9/19/2016
-- Description:	Insert a new Import for coversion into the table
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_UploadedFileInfo]
	@fileName varchar(256),
	@employerId bigint,
	@uploadTime datetime,
	@uploadSourceDescription nvarchar(50),
	@uploadTypeDescription nvarchar(50),
	@fileTypeDescription nvarchar(50),
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[UploadedFileInfo]
	([EntityStatusId], [Processed], [FailedProcessing], [EmployerId], [UploadTime], [UploadSourceDescription], [UploadTypeDescription], [FileTypeDescription], 
	[FileName], [UploadedByUser], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
	VALUES (1, 0, 0, @employerId, @uploadTime, @uploadSourceDescription, @uploadTypeDescription, @fileTypeDescription,
	@fileName, @CreatedBy, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

	SELECT @insertedID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_UploadedFileInfo_Processed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_UploadedFileInfo_Processed] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Sets the Specific File as Processed 
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_UploadedFileInfo_Processed]
       @UploadedFileInfoId int,	   
	   @modifiedBy nvarchar(50),
	   @processed bit,
	   @failed bit
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[UploadedFileInfo] 
		   set [Processed] = @processed, 
		   [FailedProcessing] = @failed,
		   [ModifiedBy] = @modifiedBy, 
		   [ModifiedDate] = GETDATE()
			where [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_UploadedFileInfo_Archived' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_UploadedFileInfo_Archived] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/3/2016
-- Description: Links to a specific File Archive
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_UploadedFileInfo_Archived]
       @UploadedFileInfoId int,	   
	   @modifiedBy nvarchar(50),
	   @archiveFileInfoId bigint
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[UploadedFileInfo] set [ArchiveFileInfoId] = @archiveFileInfoId, [ModifiedBy] = @modifiedBy, [ModifiedDate] = GETDATE()
			where [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_UploadedFileInfo_EntityStatus' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_UploadedFileInfo_EntityStatus] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Set the entity Status  
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_UploadedFileInfo_EntityStatus]
       @UploadedFileInfoId int,	   
	   @modifiedBy nvarchar(50),
	   @EntityStatus int
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[UploadedFileInfo] set [EntityStatusId] = @EntityStatus, [ModifiedBy] = @modifiedBy, [ModifiedDate] = GETDATE()
			where [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
--Grant Execute 
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo_ForEmployer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo_Unprocessed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployerIds_UploadedFileInfo_Unprocessed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo_Failed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo_UnprocessedForEmployer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_UploadedFileInfo] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_UploadedFileInfo_Processed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_UploadedFileInfo_Archived] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_UploadedFileInfo_EntityStatus] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[UploadedFileInfo] TO [aca-user] AS [dbo]
GO


-----------------------------------------------------------------------------------------------------------------
--[StagingImport] Changes
-----------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StagingImport]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StagingImport](
	[StagingImportId] [bigint] IDENTITY(1,1) NOT NULL,
	[Original] [xml] NOT NULL,
	[UploadedFileInfoId] [bigint] NOT NULL,
	[Modify] [xml] NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StagingImport_resourceId] DEFAULT (newid()),
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	
 CONSTRAINT [PK_stagingImport] PRIMARY KEY NONCLUSTERED 
(
	[StagingImportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[StagingImport]  WITH CHECK ADD  CONSTRAINT [FK_StagingImport_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[StagingImport]  WITH CHECK ADD  CONSTRAINT [FK_StagingImport_UploadedFileInfo] FOREIGN KEY([UploadedFileInfoId])
REFERENCES [dbo].[UploadedFileInfo] ([UploadedFileInfoId])
GO

ALTER TABLE [dbo].[StagingImport] CHECK CONSTRAINT [FK_StagingImport_UploadedFileInfo]
GO

ALTER TABLE [dbo].[StagingImport] CHECK CONSTRAINT [FK_StagingImport_EntityStatus]
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_stagingImport_ResourceId')
	DROP INDEX ak_stagingImport_ResourceId ON [dbo].[stagingImport]
GO
CREATE UNIQUE INDEX ak_stagingImport_ResourceId ON [dbo].[stagingImport] (ResourceId)
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_StagingImport' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_StagingImport] AS SET NOCOUNT ON;')
GO
SET ANSI_PADDING OFF
GO


------------------------StagingImport Procs---------------------------------


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select a Single Row of Import Staging
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_StagingImport]
      @StagingImportId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[StagingImport] 
      WHERE [StagingImportId] = @StagingImportId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_ActiveStagingImport' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_ActiveStagingImport] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select All Active Imports 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ActiveStagingImport]

AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[StagingImport] 
      WHERE [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_ActiveStagingImport_ForUpload' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_ActiveStagingImport_ForUpload] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/11/2016
-- Description: Select All Active Imports that are tied to this file
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ActiveStagingImport_ForUpload]
	@uploadedFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[StagingImport] 
      WHERE [EntityStatusId] = 1 AND [UploadedFileInfoId] = @uploadedFileInfoId;
END TRy
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_StagingImport_ForUpload' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_StagingImport_ForUpload] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/12/2016
-- Description: Select All Active Imports that are tied to this file
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_StagingImport_ForUpload]
	@uploadedFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[StagingImport] 
      WHERE [UploadedFileInfoId] = @uploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_StagingImport' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_StagingImport] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Log, Vu; Ryan, Mccully
-- Create date: 9/19/2016
-- Description:	Insert a new Import for coversion into the table
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_StagingImport]
	@xmlOriginal XML,
	@uploadedFileInfoId int,
	@xmlModify XML,
	@CreatedBy nvarchar(50),
    @CreatedDate datetime2(7),
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[StagingImport]
	([EntityStatusId], [Original], [UploadedFileInfoId], [Modify], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
	VALUES (1, @xmlOriginal, @uploadedFileInfoId, @xmlModify, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

	SELECT @insertedID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_StagingImport_Reprocessed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_StagingImport_Reprocessed] AS SET NOCOUNT ON;')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Log, Vu; Ryan, Mccully
-- Create date: 9/19/2016
-- Description:	Update an Existing Staging Import, deactivating the old row and adding a new row for this transformation.
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_StagingImport_Reprocessed]
	@stagingId int,	
	@xmlModify XML,
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @xmlOriginal XML;
	DECLARE @uploadedFileInfoId varchar(256);
	
	SELECT @xmlOriginal = [Modify], @uploadedFileInfoId = [UploadedFileInfoId]
	from [dbo].[StagingImport] WHERE StagingImportId = @stagingId;
	
	
	INSERT INTO [dbo].[StagingImport]
	([EntityStatusId], [Original], [UploadedFileInfoId], [Modify], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
	VALUES (1, @xmlOriginal, @uploadedFileInfoId, @xmlModify, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

	SELECT @insertedID = SCOPE_IDENTITY();

	UPDATE [dbo].[StagingImport] SET [EntityStatusId] = 2, [ModifiedBy] = @CreatedBy, [ModifiedDate] = GETDATE()
	where [StagingImportId] = @stagingId;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_StagingImportModified' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_StagingImportModified] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: We need to beable to set the finished XML once we have manipulated it.  
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_StagingImportModified]
       @stagingId int,
	   @modifiedBy nvarchar(50),
	   @xmlModify XML
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[StagingImport] set [Modify] = @xmlModify, [ModifiedBy] = @modifiedBy, [ModifiedDate] =  GETDATE()
			where [StagingImportId] = @stagingId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_StagingImportEntityStatus' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_StagingImportEntityStatus] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Set the entity Status  
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_StagingImportEntityStatus]
       @stagingId int,	   
	   @modifiedBy nvarchar(50),
	   @EntityStatus int
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[StagingImport] set [EntityStatusId] = @EntityStatus, [ModifiedBy] = @modifiedBy, [ModifiedDate] = GETDATE()
			where [StagingImportId] = @stagingId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
--Grant Execute 
GO
GRANT EXECUTE ON [dbo].[SELECT_StagingImport] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_ActiveStagingImport] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_ActiveStagingImport_ForUpload] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_StagingImport_ForUpload] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_StagingImport] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_StagingImport_Reprocessed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_StagingImportModified] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_StagingImportEntityStatus] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[StagingImport] TO [aca-user] AS [dbo]
GO




IF NOT EXISTS (SELECT * FROM sys.types WHERE  is_table_type = 1 AND name = N'Bulk_import_employee')
BEGIN
-----------------------------------------------------------------------------------------------------------------
--Bulk Import Changes
-----------------------------------------------------------------------------------------------------------------
create type Bulk_import_employee as table
(
	rowid int,
	employeeTypeID int,
	hr_status_id int,
	hr_status_ext_id varchar(50),
	hr_description varchar(50),
	employerID int,
	planYearID int,
	fName varchar(50),
	mName varchar(50),
	lName varchar(50),
	[address] varchar(50),
	city varchar(50),
	stateID int,
	stateAbb varchar(2),
	zip varchar(5),
	hDate datetime,
	i_hDate varchar(8),
	cDate datetime,
	i_cDate varchar(8),
	ssn varchar(50),
	ext_employee_id varchar(50),
	tDate datetime,
	i_tDate varchar(8),
	dob datetime,
	i_dob varchar(8),
	impEnd datetime,
	batchid int,
	aca_status_id int,
	class_id int
);	
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_INSERT_import_employee' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_INSERT_import_employee] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/19/2016
-- Description:	Bulk Import Data to speed up uploading based on <Travis Wells> INSERT_import_employee
-- =============================================
ALTER PROCEDURE [dbo].[BULK_INSERT_import_employee]
	@batchID int,
	@employees Bulk_import_employee readonly
AS

BEGIN TRY
	INSERT INTO [import_employee](
	hr_status_ext_id,
	hr_description,
	employerID,
	fName, 
	mName,
	lName,
	[address],
	city,
	stateAbb,
	zip,
	i_hDate,
	i_cDate,
	ssn,
	ext_employee_id,
	i_tDate,
	i_dob,
	batchid) 	
	SELECT hr_status_ext_id, hr_description, employerID, fName, mName, lName,
	[address], city, stateAbb, zip, i_hDate, i_cDate, ssn, ext_employee_id, 
	i_tDate, i_dob, @batchID from @employees; 

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_INSERT_FULL_import_employee' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_INSERT_FULL_import_employee] AS SET NOCOUNT ON;')

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/6/2016
-- Description:	Bulk Import the full Data to speed up uploading based on <Travis Wells> INSERT_import_employee
-- =============================================
ALTER PROCEDURE [dbo].[BULK_INSERT_FULL_import_employee]
	@batchID int,
	@employees Bulk_import_employee readonly
AS

BEGIN TRY
	INSERT INTO [import_employee](
		employeeTypeID,	hr_status_id,	hr_status_ext_id,	hr_description,
		employerID,	planYearID,	fName,	mName,	lName,	[address],
		city,	stateID,	stateAbb,	zip,	hDate,	i_hDate,
		cDate,	i_cDate,	ssn,	ext_employee_id,	tDate,
		i_tDate,	dob,	i_dob,	impEnd,	aca_status_id,
		class_id, batchid) 	
	SELECT 
		employeeTypeID,	hr_status_id,	hr_status_ext_id,	hr_description,
		employerID,	planYearID,	fName,	mName,	lName,	[address],
		city,	stateID,	stateAbb,	zip,	hDate,	i_hDate,
		cDate,	i_cDate,	ssn,	ext_employee_id,	tDate,
		i_tDate,	dob,	i_dob,	impEnd,	aca_status_id,
		class_id, @batchID 
	from @employees; 
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_UPDATE_import_employee' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_UPDATE_import_employee] AS SET NOCOUNT ON;')



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/20/2016
-- Description:	Bulk UPDATE Data to speed up uploading based on <Travis Wells> UPDATE_import_employee
-- =============================================
ALTER PROCEDURE [dbo].[BULK_UPDATE_import_employee]
	@employees Bulk_import_employee readonly
AS
BEGIN TRY
	UPDATE import_employee 
	SET
		employeeTypeID=a.employeeTypeID,
		HR_status_id = a.HR_status_id,
		hr_status_ext_id = a.hr_status_ext_id,
		hr_description = a.hr_description,
		planYearID = a.planYearID,
		stateID = a.stateID,  
		hDate=a.hDate,
		i_hDate=a.i_hDate,
		cDate=a.cDate,
		tDate=a.tDate,
		i_tDate=a.i_tDate,
		dob=a.dob,
		i_dob=a.i_dob,
		impEnd=a.impEnd, 
		class_id=a.class_id,
		aca_status_id=a.aca_status_id
	FROM @employees a
	JOIN import_employee b ON b.rowid=a.rowid;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH








----------------------------------------------------------------------







GO
IF NOT EXISTS (SELECT * FROM sys.types WHERE  is_table_type = 1 AND name = N'Bulk_import_payroll')
BEGIN
create type Bulk_import_payroll as table
(
	rowid int,
	employerid int,
	batchid int,
	employee_id int,
	fname varchar(50),
	mname varchar(50),
	lname varchar(50),
	i_hours varchar(50),
	[hours] decimal(18, 4),
	i_sdate varchar(8),
	sdate datetime,
	i_edate varchar(50),
	edate datetime,
	ssn varchar(50),
	gp_description varchar(50),
	gp_ext_id varchar(50),
	gp_id int,
	i_cdate varchar(8),
	cdate datetime,
	modBy varchar(50),
	modOn datetime,
	ext_employee_id varchar(30)
);
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_INSERT_import_payroll' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_INSERT_import_payroll] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/19/2016
-- Description:	Bulk Import Data to speed up uploading based on <Travis Wells> INSERT_import_payroll
-- =============================================
ALTER PROCEDURE [dbo].[BULK_INSERT_import_payroll]
	@payroll Bulk_import_payroll readonly
AS

BEGIN TRY
	INSERT INTO [import_payroll](
	employerid,
	batchid,
	fname, 
	mname,
	lname,
	i_hours,
	i_sdate,
	i_edate,
	ssn,
	gp_description,
	gp_ext_id,
	i_cdate,
	modBy,
	modOn, 
	ext_employee_id) 	
	SELECT employerid,
	batchid, fname, mname, lname, i_hours, i_sdate, i_edate, ssn, gp_description, gp_ext_id,
	i_cdate, modBy, modOn, ext_employee_id
	from @payroll;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_UPDATE_import_payroll' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_UPDATE_import_payroll] AS SET NOCOUNT ON;')

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/20/2016
-- Description:	Bulk Import Data to speed up uploading based on <Travis Wells> UPDATE_import_payroll
-- =============================================
ALTER PROCEDURE [dbo].[BULK_UPDATE_import_payroll]
	@payroll Bulk_import_payroll readonly
AS

BEGIN TRY
	UPDATE import_payroll
	SET
		employee_id= a.employee_id,
		gp_id = a.gp_id,
		[hours] = a.[hours],
		sdate = a.sdate,
		edate = a.edate,
		cdate = a.cdate, 
		modBy = a.modBy,
		modOn = a.modOn	
	from @payroll a	
	JOIN import_payroll b ON b.rowid=a.rowid;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_TRANSFER_import_new_payroll' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_TRANSFER_import_new_payroll] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/20/2016
-- Description:	Bulk Import Data to speed up uploading based on <Travis Wells> TRANSFER_import_new_payroll
-- =============================================
ALTER PROCEDURE [dbo].[BULK_TRANSFER_import_new_payroll] 
	@history varchar(max),
	@payroll  Bulk_import_payroll readonly
AS

BEGIN TRY
	
	DECLARE @rowid int;
	DECLARE @employerid int;
	DECLARE @batchid int;
	DECLARE @employee_id int;
	DECLARE @hours decimal(18, 4);
	DECLARE @sdate datetime;
	DECLARE @edate datetime;
	DECLARE @gp_id int;
	DECLARE @cdate datetime;
	DECLARE @modBy varchar(50);
	DECLARE @modOn datetime;
	
	DECLARE MY_CURSOR CURSOR 
		LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
		SELECT rowid, employerid, batchid, employee_id, gp_id, [hours], sdate, edate, cdate, modBy, modOn
		FROM @payroll
	
	OPEN MY_CURSOR;
	FETCH NEXT FROM MY_CURSOR INTO @rowId, @employerID, @batchID, @employee_ID, @gp_id, @hours, @sdate, @edate, @cdate, @modBy, @modOn;
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		--loop through ever row passed in and try it individually. 
		Exec [TRANSFER_import_new_payroll] @rowId, @employerID, @batchID, @employee_ID, @gp_id, @hours, @sdate, @edate, @cdate, @modBy, @modOn, @history;
		FETCH NEXT FROM MY_CURSOR INTO @rowId, @employerID, @batchID, @employee_ID, @gp_id, @hours, @sdate, @edate, @cdate, @modBy, @modOn;
	END
	CLOSE MY_CURSOR;
	DEALLOCATE MY_CURSOR;
	
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
-----------------------------
GO
GRANT EXECUTE ON [dbo].[BULK_INSERT_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_INSERT_FULL_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_UPDATE_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_INSERT_import_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_UPDATE_import_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_TRANSFER_import_new_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_import_payroll] TO [aca-user] AS [dbo]

----------------------------------------------------------------------------------
--alter existing procedures
----------------------------------------------------------------------------------
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_FailNightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_FailNightlyCalculation] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UPDATE_FailNightlyCalculation]
	@employerId as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[NightlyCalculation] SET processFail = 1
	WHERE EmployerId = @employerId AND processStatus = 0 AND processFail = 0;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_NightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_NightlyCalculation] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UPDATE_NightlyCalculation]
	@employerId as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[NightlyCalculation] SET processStatus = 1
	WHERE EmployerId = @employerId AND processStatus = 0 AND processFail = 0;
	
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'TRANSFER_import_new_payroll' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[TRANSFER_import_new_payroll] AS SET NOCOUNT ON;')
GO
/****** Object:  StoredProcedure [dbo].[TRANSFER_import_new_payroll]    Script Date: 9/21/2016 10:26:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/17/2014>
-- Description:	<This stored procedure is meant transfer a new payroll record from the import table over to the current payroll table.
-- Changes:
--			5-27-2015 TLW
--				- Added the history parameter to record the initial data. 
-- =============================================
ALTER PROCEDURE [dbo].[TRANSFER_import_new_payroll] 
	@rowID int,
	@employerID int,
	@batchID int, 
	@employeeID int,
	@gpID int, 
	@hours numeric(10,4),
	@sdate datetime, 
	@edate datetime, 
	@cdate datetime, 
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(2000)
AS

BEGIN
	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	
	BEGIN TRANSACTION
		BEGIN TRY
			-- Step 1: Create a new EMPLOYEE based on the registration information.
			--			- Return the new Employee_ID
				EXEC INSERT_new_payroll 
					@employerID,
					@batchID, 
					@employeeID,
					@gpID, 
					@hours,
					@sdate, 
					@edate, 
					@cdate, 
					@modBy,
					@modOn,
					@history
	
			-- Step 2: DELETE the record from the import_employee table. 
				EXEC DELETE_payroll_import_row
					@rowID
			COMMIT
		END TRY
		BEGIN CATCH
			--this will happen if a constraint is missing, or something.
			ROLLBACK TRANSACTION
			EXEC INSERT_ErrorLogging
		END CATCH
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_employee_plan_year' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_employee_plan_year] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[UPDATE_employee_plan_year]
	@employerID int,
	@employeeTypeID int,
	@adminPlanYearID int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY

		UPDATE employee
		SET
			plan_year_id=@adminPlanYearID,
			limbo_plan_year_id=NULL,
			modOn=@modOn,
			modBy=@modBy
		WHERE
			employer_id=@employerID AND
			employee_type_id=@employeeTypeID AND
			limbo_plan_year_id=@adminPlanYearID;
		
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		exec dbo.INSERT_ErrorLogging
	END CATCH
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_new_registration' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_new_registration] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[INSERT_new_registration]
       @empTypeID int,
       @name varchar(50),
       @ein varchar(50),
       @add varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @userfname varchar(50),
       @userlname varchar(50),
       @useremail varchar(50),
       @userphone varchar(15),
       @username varchar(50),
       @password varchar(50),
       @active bit,
       @power bit,
       @billing bit,
       @b_add varchar(50),
       @b_city varchar(50),
       @b_stateID int,
       @b_zip varchar(15),
       @p_desc1 varchar(50),
       @p_start1 datetime,
       @p_desc2 varchar(50),
       @p_start2 datetime,
       @b_fname varchar(50),
       @b_lname varchar(50),
       @b_email varchar(50),
       @b_phone varchar(15), 
       @b_username varchar(50),
       @b_password varchar(50),
       @b_active bit, 
       @b_power bit,
       @b_billing bit,
       @dbaName varchar(100),
       @employerID int OUTPUT
AS

BEGIN
       DECLARE @userid int;
       DECLARE @planyearid int;
       DECLARE @lastModBy varchar(50);
       DECLARE @lastMod datetime;
       DECLARE @irsContact bit;
       DECLARE @irsContact2 bit;

       SET @irsContact = 1;
       SET @irsContact2 = 0;
       SET @lastModBy = 'Registration';
       SET @lastMod = GETDATE();

       /*************************************************************
       ******* Create a transaction that must fully complete ********
       **************************************************************/
       BEGIN TRANSACTION
             BEGIN TRY
                    DECLARE @default varchar(50);
                    SET @default = 'All Employees';

                    -- Step 1: Create a new EMPLOYER based on the registration information.
                    --                  - Return the new Employer_ID
                    EXEC INSERT_new_employer 
                           @name, 
                           @add, 
                           @city,
                           @stateID, 
                           @zip, 
                           '../images/logos/EBC_logo.gif',  
                           @b_add, 
                           @b_city, 
                           @b_stateID, 
                           @b_zip, 
                           @empTypeID,
                           @ein,
                           @dbaName,
                           @employerID OUTPUT;

                    -- Step 2: Create a new EMPLOYEE_TYPE for this specific employer.
                    EXEC INSERT_new_employee_type 
                           @employerID, 
                           @default;

                    -- Step 3: Create a new USER for this specific district.
                    EXEC INSERT_new_user 
                           @userfname, 
                           @userlname, 
                           @useremail, 
                           @userphone, 
                           @username, 
                           @password,
                           @employerID, 
                           @active, 
                           @power,
                           @lastModBy,
                           @lastMod,
                           @billing,
                           @irsContact,
                           @userid OUTPUT

                    IF (@b_username IS NOT NULL)
                           BEGIN
                                 EXEC INSERT_new_user 
                                        @b_fname, 
                                        @b_lname, 
                                        @b_email, 
                                        @b_phone, 
                                        @b_username, 
                                        @b_password,
                                        @employerID, 
                                        @b_active, 
                                        @b_power,
                                        @lastModBy,
                                        @lastMod,
                                        @b_billing,
                                        @irsContact2,
                                        @userid OUTPUT
                           END
       

                    -- Step 4: Create a new PLAN YEAR for this specific district.
                    DECLARE @p_end1 datetime;
                    DECLARE @history varchar(100);
                    DECLARE @p_notes varchar(100);
                    SET @p_end1 = DATEADD(dd, 364, @p_start1);
                    SET @p_notes = '';
                    SET @history = 'Plan created on: ' + CONVERT(varchar(19), GETDATE());
                    EXEC INSERT_new_plan_year 
                           @employerID, 
                           @p_desc1, 
                           @p_start1, 
                           @p_end1, 
                           @p_notes,
                           @history,
                           @lastMod,
                           @lastModBy,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           @planyearid OUTPUT; 

                    -- Step 5: Create a second PLAN YEAR for this specific district if needed.
                    IF (@p_start2 IS NOT NULL OR @p_start2 = '')
                           BEGIN
                                 DECLARE @p_end2 datetime;
                                 SET @p_end2 = DATEADD(dd, 364, @p_start2);
                                 SET @p_notes = '';
                                 SET @history = 'Plan created on: ' + CONVERT(varchar(19), GETDATE());
                                 EXEC INSERT_new_plan_year 
                                        @employerID, 
                                        @p_desc2, 
                                        @p_start2, 
                                        @p_end2, 
                                        @p_notes,
                                        @history,
                                        @lastMod,
                                        @lastModBy, 
                                        @planyearid OUTPUT; 
                           END

                    COMMIT
             END TRY
             BEGIN CATCH
                    --If something fails return 0.
                    SET @employerID = 0;
                    ROLLBACK TRANSACTION
                    exec dbo.INSERT_ErrorLogging
             END CATCH

END
GO

-- end of ACA-Migration-017_to_018.sql


GO
alter table [dbo].[EmployeeMeasurementAverageHours] add [IsNewHire] bit not null DEFAULT 0;
GO
-------------------------------------------------------------------------------------------------------------------------
-- New stored Procs
-------------------------------------------------------------------------------------------------------------------------
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan, McCully
-- Create date: 10/18/2016
-- Description:	This stored procedure is meant to update the measurement_plan_year_id and limbo_plan_year_id in reverse of the normal rollover period.
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_ROLLBACK_employee_plan_year_meas]
	@employerID int,
	@employeeTypeID int,
	@CurrPlanYearID int,
	@RollbackToPlanYearID int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY
	
		/***************************************************************************
		Step 1: Get the Current Measurement Period Start Date.
		***************************************************************************/
		DECLARE @measStart datetime;
		SELECT @measStart=meas_start FROM measurement WHERE plan_year_id=@CurrPlanYearID;
		

		/******************************************************************************************************************
		Step 2: UPDATE the Plan Year Columns 
		******************************************************************************************************************/
		UPDATE employee
		SET
			meas_plan_year_id=@RollbackToPlanYearID,
			limbo_plan_year_id=null,
			plan_year_id=null,
			modOn=@modOn,
			modBy=@modBy
		WHERE
			employer_id=@employerID AND
			employee_type_id=@employeeTypeID AND
			meas_plan_year_id=@CurrPlanYearID AND 
			hireDate < @measStart;
		
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		EXEC dbo.INSERT_ErrorLogging
	END CATCH
END
GO




GO
GRANT EXECUTE ON [dbo].[UPDATE_ROLLBACK_employee_plan_year_meas] TO [aca-user] AS [dbo]
GO




------------------------------------------------------------------------
-- Modify Previous Stored Procs and tables
------------------------------------------------------------------------
GO 
ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] ADD [TrendingWeeklyAverageHours] [numeric](18, 4) NULL
GO
ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] ADD [TrendingMonthlyAverageHours] [numeric](18, 4) NULL
GO
ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] ADD [TotalHours] [numeric](18, 4) NULL
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 10/6/2016
-- Description:	Upsert: update or insert AverageHours into the table
-- =============================================
ALTER PROCEDURE [dbo].[UPSERT_AverageHours]
	@employeeId int,
	@measurementId int,
	@weeklyAverageHours numeric(18, 4),
	@monthlyAverageHours numeric(18, 4),
	@trendingWeeklyAverageHours numeric(18, 4),
	@trendingMonthlyAverageHours numeric(18, 4),
	@totalHours numeric(18, 4),
	@isNewHire bit,
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	MERGE [dbo].[EmployeeMeasurementAverageHours]  AS T  
	USING (
			SELECT @employeeId employeeId,
			@measurementId measurementId,
			@weeklyAverageHours weeklyAverageHours, 
			@monthlyAverageHours monthlyAverageHours, 
			@trendingWeeklyAverageHours trendingWeeklyAverageHours,
			@trendingMonthlyAverageHours trendingMonthlyAverageHours,
			@totalHours totalHours,
			@isNewHire isNewHire,
			@CreatedBy CreatedBy
		) AS S 
	ON T.EmployeeId = S.employeeId AND T.MeasurementId = S.measurementId AND T.IsNewHire = S.isNewHire
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[WeeklyAverageHours] = S.weeklyAverageHours,
		T.[MonthlyAverageHours] = S.monthlyAverageHours,
		T.[TrendingWeeklyAverageHours] = S.trendingWeeklyAverageHours,
		T.[TrendingMonthlyAverageHours] = S.trendingMonthlyAverageHours,
		T.[TotalHours] = S.totalHours,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE(),
		@CreatedBy = T.[EmployeeMeasurementAverageHoursId]
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[EmployeeId], [MeasurementId], [WeeklyAverageHours], [MonthlyAverageHours],
		[TrendingWeeklyAverageHours], [TrendingMonthlyAverageHours], [TotalHours], [IsNewHire],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employeeId, S.measurementId, S.weeklyAverageHours, S.monthlyAverageHours, 
		  S.trendingWeeklyAverageHours, S.trendingMonthlyAverageHours, S.totalHours, S.isNewHire,
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );

	SELECT @insertedID = SCOPE_IDENTITY();

END
GO

GO
drop PROCEDURE [dbo].[BULK_UPSERT_AverageHours];
GO
Drop type Bulk_AverageHours;
GO
CREATE type Bulk_AverageHours as table
(
	EmployeeMeasurementAverageHoursId int,
	EmployeeId int,
	MeasurementId int,
	WeeklyAverageHours numeric(18, 4),
	MonthlyAverageHours numeric(18, 4),
	TrendingWeeklyAverageHours numeric(18, 4),
	TrendingMonthlyAverageHours numeric(18, 4),
	TotalHours numeric(18, 4),
	IsNewHire bit
);	

GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/7/2016
-- Description:	Bulk Insert or Update Calculation Averages
-- =============================================
CREATE PROCEDURE [dbo].[BULK_UPSERT_AverageHours]	
	@averages Bulk_AverageHours readonly,
	@CreatedBy nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	MERGE [dbo].[EmployeeMeasurementAverageHours]  AS T  
	USING (
			SELECT EmployeeId employeeId,
			MeasurementId measurementId,
			WeeklyAverageHours weeklyAverageHours, 
			MonthlyAverageHours monthlyAverageHours, 
			TrendingWeeklyAverageHours trendingWeeklyAverageHours, 
			TrendingMonthlyAverageHours trendingMonthlyAverageHours, 
			TotalHours totalHours,
			IsNewHire isNewHire,
			@CreatedBy CreatedBy From @averages
		) AS S 
	ON T.EmployeeId = S.employeeId AND T.MeasurementId = S.measurementId AND T.IsNewHire = S.isNewHire
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[WeeklyAverageHours] = S.weeklyAverageHours,
		T.[MonthlyAverageHours] = S.monthlyAverageHours,		
		T.[TrendingWeeklyAverageHours] = S.trendingWeeklyAverageHours,
		T.[TrendingMonthlyAverageHours] = S.trendingMonthlyAverageHours,
		T.[TotalHours] = S.totalHours,
		T.[IsNewHire] = S.isNewHire,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE(),
		@CreatedBy = T.[EmployeeMeasurementAverageHoursId]
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[EmployeeId], [MeasurementId], [WeeklyAverageHours], [MonthlyAverageHours],
		[TrendingWeeklyAverageHours], [TrendingMonthlyAverageHours], [TotalHours], [IsNewHire],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employeeId, S.measurementId, S.weeklyAverageHours, S.monthlyAverageHours, 
		  S.trendingWeeklyAverageHours, S.trendingMonthlyAverageHours, S.totalHours, S.isNewHire,
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );
END
GO

GO
GRANT EXECUTE ON [dbo].[BULK_UPSERT_AverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_AverageHours] TO [aca-user] AS [dbo]
GO

-- end of ACA-Migration-018_to_019.sql
--Migration script 19-20
-----create the new table-----


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlanYearGroup](
	[PlanYearGroupId] [bigint] IDENTITY(1,1) NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[GroupName] [nvarchar](75) NOT NULL,
	[Employer_id] [int] NOT NULL,
 CONSTRAINT [PK_PlanYearGroup_PlanYearGroupId] PRIMARY KEY NONCLUSTERED 
(
	[PlanYearGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PlanYearGroup]  WITH CHECK ADD  CONSTRAINT [FK_PlanYearGroup_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[PlanYearGroup] CHECK CONSTRAINT [FK_PlanYearGroup_EntityStatus]
GO

ALTER TABLE [dbo].[PlanYearGroup]  WITH NOCHECK ADD  CONSTRAINT [fk_PlanYearGroup_EmployerId] FOREIGN KEY([Employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO

ALTER TABLE [dbo].[PlanYearGroup] CHECK CONSTRAINT [fk_PlanYearGroup_EmployerId]
GO

------Prepopulate the data-------

GO
Insert into [dbo].[PlanYearGroup] 
		([EntityStatusId],
		[CreatedBy],
		[CreatedDate],
		[ModifiedBy],
		[ModifiedDate],
		[GroupName],
		[Employer_id]) 
	select 
		1,
		'system',
		GETDATE(),
		'system',
		GETDATE(), 
		[name],
		[employer_id] 
	from [dbo].[employer];
GO

------Alter existing tables------
---Alter Plan Year---

GO
alter table [dbo].[plan_year] add [PlanYearGroupId] [bigint] NOT NULL DEFAULT (0);
GO

UPDATE [dbo].[plan_year] 
	SET [dbo].[plan_year].[PlanYearGroupId] = g.[PlanYearGroupId]
	FROM [dbo].[plan_year] py 
	INNER JOIN [dbo].[PlanYearGroup] g 
	ON g.[Employer_id] = py.[employer_id];
GO

ALTER TABLE [dbo].[plan_year]  WITH NOCHECK ADD  CONSTRAINT [fk_plan_year_PlanYearGroupId] FOREIGN KEY([PlanYearGroupId])
REFERENCES [dbo].[PlanYearGroup] ([PlanYearGroupId])
GO

ALTER TABLE [dbo].[plan_year] CHECK CONSTRAINT [fk_plan_year_PlanYearGroupId]
GO

-----New Stored Procs-----
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 10/31/2016
-- Description:	Insert a new PlanYearGroup
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_PlanYearGroup' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_PlanYearGroup] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[INSERT_PlanYearGroup]
	@CreatedBy nvarchar(50),
    @GroupName nvarchar(50),
	@Employer_id int,
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[PlanYearGroup]
	([EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate],
	[GroupName], [Employer_id])
	VALUES (1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE(),
	@GroupName, @Employer_id);

	SELECT @insertedID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Select a Single Row of [PlanYearGroup]
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_PlanYearGroup' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_PlanYearGroup] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[SELECT_PlanYearGroup]
      @PlanYearGroupId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[PlanYearGroup] 
      WHERE [PlanYearGroupId] = @PlanYearGroupId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Select all Rows of [PlanYearGroup] belonging to a certain employer
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_PlanYearGroup_ForEmployer' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_PlanYearGroup_ForEmployer] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[SELECT_PlanYearGroup_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[PlanYearGroup] 
      WHERE [Employer_Id] = @EmployerId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 10/31/2016
-- Description:	Update a PlanYearGroup
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_PlanYearGroup' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_PlanYearGroup] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[UPDATE_PlanYearGroup]
      @modifiedBy nvarchar(50),
      @PlanYearGroupId int,
      @GroupName nvarchar(50)
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [dbo].[PlanYearGroup] SET GroupName = @GroupName,
		ModifiedBy = @modifiedBy, ModifiedDate = GETDATE()
      WHERE PlanYearGroupId = @PlanYearGroupId
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Set the entity Status  
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_PlanYearGroupStatus' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_PlanYearGroupStatus] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[UPDATE_PlanYearGroupStatus]
	   @EntityStatus int,
	   @PlanYearGroupId int,
	   @modifiedBy nvarchar(50)
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[PlanYearGroup] set [EntityStatusId] = @EntityStatus, [ModifiedBy] = @modifiedBy, [ModifiedDate] = GETDATE()
			where [PlanYearGroupId] = @PlanYearGroupId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'spGetFieldForIRS' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[spGetFieldForIRS] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[spGetFieldForIRS]
	 @employerID int,
	 @taxYear int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	WITH empA AS
	(
	SELECT employee_id,a.time_frame_id, employer_id, offer_of_coverage_code, monthly_status_id, monthly_hours
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
	WHERE employer_id = @employerID AND b.year_id = @taxYear
	),
	empB AS
	(
	SELECT a.employee_id, first_name, middle_name, last_name, name_suffix, a.[address], a.city, state_code, zipcode,
		c.dob
	FROM air.emp.employee a INNER JOIN empA b ON  a.employee_id = b.employee_id INNER JOIN aca.dbo.employee c
	ON a.employee_id = c.employee_id
	)
	SELECT DISTINCT d.name as [Employer Name], d.ein, d.[address] as [Employer Address], 
		d.city as [Employer City], d.state_code AS [Employer Code], d.zipcode AS [Employer Zip], 
		d.contact_telephone AS [Employer Telephone], e.name as months, f.year_id as years, 
		a.offer_of_coverage_code, a.monthly_hours, B.*, C.status_description, b.dob
	FROM empA a INNER JOIN empB B  ON a.employee_id = b.employee_id INNER JOIN air.emp.monthly_status c ON a.monthly_status_id = c.monthly_status_id
		INNER JOIN air.ale.employer d ON d.employer_id = a.employer_id INNER JOIN air.gen.[month] e ON a.monthly_status_id = e.month_id
		INNER JOIN air.gen.time_frame f ON a.time_frame_id = f.time_frame_id
		ORDER BY b.first_name
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
-----Grant execute to all the new procs-----
GRANT EXECUTE ON [dbo].[spGetFieldForIRS] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_PlanYearGroup] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_PlanYearGroup] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_PlanYearGroup_ForEmployer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_PlanYearGroup] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_PlanYearGroupStatus] TO [aca-user] AS [dbo]
GO


-----Modify Stored Procs-----

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/19/2014>
-- Description:	<This stored procedure is meant to create a new Pay Roll record.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_new_plan_year]
	@employerID int,
	@description varchar(50),
	@startDate datetime,
	@endDate datetime,
	@notes varchar(max),
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@default_Meas_Start datetime,
	@default_Meas_End datetime,
	@default_Admin_Start datetime,
	@default_Admin_End datetime,
	@default_Open_Start datetime,
	@default_Open_End datetime,
	@default_Stability_Start datetime,
	@default_Stability_End datetime,
	@PlanYearGroupId bigint,
	@planyearid int OUTPUT
AS

BEGIN TRY
	INSERT INTO [plan_year](
		employer_id,
		[description],
		startDate,
		endDate,
		notes,
		history,
		modOn,
		modBy, 
		default_Meas_Start,
		default_Meas_End,
		default_Admin_Start,
		default_Admin_End,
		default_Open_Start,
		default_Open_End,
		default_Stability_Start,
		default_Stability_End,
		PlanYearGroupId)
	VALUES(
		@employerID,
		@description,
		@startDate,
		@endDate,
		@notes,
		@history,
		@modOn,
		@modBy,
		@default_Meas_Start,
		@default_Meas_End,
		@default_Admin_Start,
		@default_Admin_End,
		@default_Open_Start,
		@default_Open_End,
		@default_Stability_Start,
		@default_Stability_End,
		@PlanYearGroupId)

SELECT @planyearid = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to update the plan year.>
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_plan_year]
	@planyearID int,
	@description varchar(50),
	@sDate datetime,
	@eDate datetime,
	@notes varchar(max),
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@default_Meas_Start datetime,
	@default_Meas_End datetime,
	@default_Admin_Start datetime,
	@default_Admin_End datetime,
	@default_Open_Start datetime,
	@default_Open_End datetime,
	@default_Stability_Start datetime,
	@default_Stability_End datetime,
	@PlanYearGroupId bigint
AS
BEGIN TRY
	UPDATE [dbo].[plan_year]
	SET
		[description] = @description,
		[startDate] = @sDate,
		[endDate] = @eDate,
		[notes] = @notes,
		[history] = @history,
		[modOn] = @modOn,
		[modBy] = @modBy,
		[default_Meas_Start] = @default_Meas_Start,
		[default_Meas_End] = @default_Meas_End,
		[default_Admin_Start] = @default_Admin_Start,
		[default_Admin_End] = @default_Admin_End,
		[default_Open_Start] = @default_Open_Start,
		[default_Open_End] = @default_Open_End,
		[default_Stability_Start] = @default_Stability_Start,
		[default_Stability_End] = @default_Stability_End,
		[PlanYearGroupId] = @PlanYearGroupId
	WHERE
		[plan_year_id]=@planyearID;
	
--	UPDATE [dbo].[employee] 
--		SET [dbo].[employee].[PlanYearGroupId] = @PlanYearGroupId
--	where [meas_plan_year_id] = @planyearID;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

ALTER PROCEDURE [dbo].[sp_AIR_ETL_ShortBuild]
	@employerID int,
	@taxYear int,
	@employeeID int
AS

BEGIN TRY
	-- Extract Employee Info through AIR Process.
	EXEC [air].etl.spETL_ShortBuild
		@employerid,
		@taxYear,
		@employeeID
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH

-- end Migration script 19-20
GO
--Start Migration script 20 - 21
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[INSERT_new_registration]
       @empTypeID int,
       @name varchar(50),
       @ein varchar(50),
       @add varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @userfname varchar(50),
       @userlname varchar(50),
       @useremail varchar(50),
       @userphone varchar(15),
       @username varchar(50),
       @password varchar(50),
       @active bit,
       @power bit,
       @billing bit,
       @b_add varchar(50),
       @b_city varchar(50),
       @b_stateID int,
       @b_zip varchar(15),
       @p_desc1 varchar(50),
       @p_start1 datetime,
       @p_desc2 varchar(50),
       @p_start2 datetime,
       @b_fname varchar(50),
       @b_lname varchar(50),
       @b_email varchar(50),
       @b_phone varchar(15), 
       @b_username varchar(50),
       @b_password varchar(50),
       @b_active bit, 
       @b_power bit,
       @b_billing bit,
       @dbaName varchar(100),
       @employerID int OUTPUT
AS

BEGIN
       DECLARE @userid int;
	   DECLARE @planyearGroupId int;
       DECLARE @planyearid int;
       DECLARE @lastModBy varchar(50);
       DECLARE @lastMod datetime;
       DECLARE @irsContact bit;
       DECLARE @irsContact2 bit;

       SET @irsContact = 1;
       SET @irsContact2 = 0;
       SET @lastModBy = 'Registration';
       SET @lastMod = GETDATE();

       /*************************************************************
       ******* Create a transaction that must fully complete ********
       **************************************************************/
       BEGIN TRANSACTION
             BEGIN TRY
                    DECLARE @default varchar(50);
                    SET @default = 'All Employees';

                    -- Step 1: Create a new EMPLOYER based on the registration information.
                    --                  - Return the new Employer_ID
                    EXEC INSERT_new_employer 
                           @name, 
                           @add, 
                           @city,
                           @stateID, 
                           @zip, 
                           '../images/logos/EBC_logo.gif',  
                           @b_add, 
                           @b_city, 
                           @b_stateID, 
                           @b_zip, 
                           @empTypeID,
                           @ein,
                           @dbaName,
                           @employerID OUTPUT;

                    -- Step 2: Create a new EMPLOYEE_TYPE for this specific employer.
                    EXEC INSERT_new_employee_type 
                           @employerID, 
                           @default;

                    -- Step 3: Create a new USER for this specific district.
                    EXEC INSERT_new_user 
                           @userfname, 
                           @userlname, 
                           @useremail, 
                           @userphone, 
                           @username, 
                           @password,
                           @employerID, 
                           @active, 
                           @power,
                           @lastModBy,
                           @lastMod,
                           @billing,
                           @irsContact,
                           @userid OUTPUT

                    IF (@b_username IS NOT NULL)
                           BEGIN
                                 EXEC INSERT_new_user 
                                        @b_fname, 
                                        @b_lname, 
                                        @b_email, 
                                        @b_phone, 
                                        @b_username, 
                                        @b_password,
                                        @employerID, 
                                        @b_active, 
                                        @b_power,
                                        @lastModBy,
                                        @lastMod,
                                        @b_billing,
                                        @irsContact2,
                                        @userid OUTPUT
                           END
       

                    -- Step 4: Create a new PLAN YEAR for this specific district.
					-- Step 4.1: Creat a Plan Year Group For this employer
					EXEC [INSERT_PlanYearGroup]
						@lastModBy,
						@name,
						@employerID,
						@planyearGroupId OUTPUT;

					-- Now insert the plan year
                    DECLARE @p_end1 datetime;
                    DECLARE @history varchar(100);
                    DECLARE @p_notes varchar(100);
                    SET @p_end1 = DATEADD(dd, 364, @p_start1);
                    SET @p_notes = '';
                    SET @history = 'Plan created on: ' + CONVERT(varchar(19), GETDATE());
                    EXEC INSERT_new_plan_year 
                           @employerID, 
                           @p_desc1, 
                           @p_start1, 
                           @p_end1, 
                           @p_notes,
                           @history,
                           @lastMod,
                           @lastModBy,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
						   @planyearGroupId,
                           @planyearid OUTPUT; 

                    -- Step 5: Create a second PLAN YEAR for this specific district if needed.
                    IF (@p_start2 IS NOT NULL OR @p_start2 = '')
                           BEGIN
                                 DECLARE @p_end2 datetime;
                                 SET @p_end2 = DATEADD(dd, 364, @p_start2);
                                 SET @p_notes = '';
                                 SET @history = 'Plan created on: ' + CONVERT(varchar(19), GETDATE());
                                 EXEC INSERT_new_plan_year 
                                        @employerID, 
                                        @p_desc2, 
                                        @p_start2, 
                                        @p_end2, 
                                        @p_notes,
                                        @history,
                                        @lastMod,
                                        @lastModBy, 
										null,
									   null,
									   null,
									   null,
									   null,
									   null,
									   null,
									   null,
									   @planyearGroupId,
                                        @planyearid OUTPUT; 
                           END

                    COMMIT
             END TRY
             BEGIN CATCH
                    --If something fails return 0.
                    SET @employerID = 0;
                    ROLLBACK TRANSACTION
                    exec dbo.INSERT_ErrorLogging
             END CATCH

END
--End Migration script 20 - 21
GO
--Start Migration script 21 - 22
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Select a Single Row of [PlanYearGroup]
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_PlanYearGroup' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_PlanYearGroup] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[SELECT_PlanYearGroup]
      @PlanYearGroupId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[PlanYearGroup] 
      WHERE [PlanYearGroupId] = @PlanYearGroupId 
		AND [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Select all Rows of [PlanYearGroup] belonging to a certain employer
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_PlanYearGroup_ForEmployer' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_PlanYearGroup_ForEmployer] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[SELECT_PlanYearGroup_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[PlanYearGroup] 
      WHERE [Employer_Id] = @EmployerId
		AND [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'spGetFieldForIRS' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[spGetFieldForIRS] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[spGetFieldForIRS]
	 @employerID int,
	 @taxYear int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	WITH empA AS
	(
	SELECT employee_id,a.time_frame_id, employer_id, offer_of_coverage_code, monthly_status_id, monthly_hours
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
	WHERE employer_id = @employerID AND b.year_id = @taxYear
	),
	empB AS
	(
	SELECT a.employee_id, first_name, middle_name, last_name, name_suffix, a.[address], a.city, state_code, zipcode,
		c.dob
	FROM air.emp.employee a INNER JOIN empA b ON  a.employee_id = b.employee_id INNER JOIN aca.dbo.employee c
	ON a.employee_id = c.employee_id
	)
	SELECT DISTINCT d.name as [Employer Name], d.ein, d.[address] as [Employer Address], 
		d.city as [Employer City], d.state_code AS [Employer Code], d.zipcode AS [Employer Zip], 
		d.contact_telephone AS [Employer Telephone], e.name as months, f.year_id as years, 
		a.offer_of_coverage_code, a.monthly_hours, B.*, C.status_description, b.dob
	FROM empA a INNER JOIN empB B  ON a.employee_id = b.employee_id INNER JOIN air.emp.monthly_status c ON a.monthly_status_id = c.monthly_status_id
		INNER JOIN air.ale.employer d ON d.employer_id = a.employer_id INNER JOIN air.gen.[month] e ON a.monthly_status_id = e.month_id
		INNER JOIN air.gen.time_frame f ON a.time_frame_id = f.time_frame_id
		ORDER BY b.first_name
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
--End Migration script 21 - 22
GO
--Start Migration script 22-23
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
ALTER TABLE dbo.employer ALTER COLUMN name VARCHAR(75) NULL
GO
ALTER TABLE dbo.employer ALTER COLUMN DBAName VARCHAR(75) NULL
GO
ALTER TABLE dbo.PlanYearGroup ALTER COLUMN GroupName NVARCHAR(75) NOT NULL
GO
ALTER TABLE dbo.equivalency ALTER COLUMN name VARCHAR(75) NULL
GO
ALTER TABLE dbo.equiv_detail ALTER COLUMN name VARCHAR(75) NOT NULL
GO
/****** Object:  StoredProcedure [dbo].[INSERT_new_employer]    Script Date: 11/29/2016 9:41:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[INSERT_new_employer]
      @name varchar(50),
      @add varchar(75),
      @city varchar(50),
      @stateID int,
      @zip varchar(15),
      @logo varchar(50),
      @b_add varchar(50),
      @b_city varchar(50),
      @b_stateID int,
      @b_zip varchar(15),
      @empTypeID int,
      @ein varchar(50),
      @dbaName varchar(100),
      @empid int OUTPUT
AS
 
BEGIN TRY
      INSERT INTO [employer](
            name,
            [address],
            city,
            state_id,
            zip,
            img_logo,
            bill_address,
            bill_city,
            bill_state,
            bill_zip,
            employer_type_id, 
            ein,
            DBAName)
      VALUES(
            @name,
            @add,
            @city,
            @stateID,
            @zip,
            @logo,
            @b_add,
            @b_city,
            @b_stateID,
            @b_zip,
            @empTypeID,
            @ein,
            @dbaName)
 
SELECT @empid = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[INSERT_new_equivalency]
      @employerID int,
      @name varchar(75),
      @gpID int,
      @every decimal(18,4),
      @unitID int,
      @credit decimal(18,4),
      @sdate datetime,
      @edate datetime,
      @notes varchar(1000),
      @modBy varchar(50),
      @modOn datetime, 
      @history varchar(max),
      @active bit,
      @equivTypeID int,
      @posID int,
      @actID int,
      @detID int,
      @equivalencyID int OUTPUT
AS
 
BEGIN TRY
      INSERT INTO [equivalency](
            employer_id,
            name,
            gpid,
            every,
            unit_id,
            credit,
            [start_date],
            end_date,
            notes,
            modBy,
            modOn,
            history,
            active,
            equivalency_type_id,
            position_id,
            activity_id,
            detail_id)
      VALUES(
            @employerID,
            @name,
            @gpID,
            @every,
            @unitID,
            @credit,
            @sdate,
            @edate,
            @notes,
            @modBy,
            @modOn,
            @history,
            @active,
            @equivTypeID,
            @posID,
            @actID,
            @detID)
 
SELECT @equivalencyID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[INSERT_UPDATE_employer_irs_submission_approval]
      @approvalID int,
      @employerID int,
      @taxYear int,
      @dge bit,
      @dgeName varchar(75),
      @dgeEIN varchar(50),
      @dgeAddress varchar(50),
      @dgeCity varchar(50),
      @dgeStateID int,
      @dgeZip varchar(50),
      @dgeFname varchar(50),
      @dgeLname varchar(50),
      @dgePhone varchar(50),
      @ale bit,
      @tr1 bit,
      @tr2 bit, 
      @tr3 bit,
      @tr4 bit,
      @tr5 bit,
      @tr bit,
      @tobacco bit,
      @unpaidLeave bit,
      @safeHarbor bit,
      @completedBy varchar(50),
      @completedOn datetime,
      @ebcApproved bit,
      @ebcApprovedBy varchar(50),
      @ebcApprovedOn datetime,
      @allowEditing bit,
      @approvalID_Final int OUTPUT
AS
 
BEGIN TRY
      SET @approvalID_Final = @approvalID;
 
      /************************************************************************************************************************
      Compare EmployerID and TAX YEAR to see if a record exists. 
      ************************************************************************************************************************/
IF @approvalID_Final<= 0
      BEGIN
            SELECT @approvalID_Final=approval_id FROM tax_year_approval
            WHERE employer_id=@employerID AND tax_year=@taxYear;
      END
 
IF @approvalID_Final <= 0
      BEGIN
            INSERT INTO [tax_year_approval](
                  employer_id,
                  tax_year,
                  dge,
                  dge_name,
                  dge_ein,
                  dge_address,
                  dge_city,
                  state_id,
                  dge_zip,
                  dge_contact_fname,
                  dge_contact_lname,
                  dge_phone,
                  ale,
                  tr_q1,
                  tr_q2,
                  tr_q3,
                  tr_q4,
                  tr_q5,
                  tr_qualified,
                  tobacco,
                  unpaidLeave,
                  safeHarbor,
                  completed_by,
                  completed_on,
                  ebc_approval,
                  ebc_approved_by,
                  ebc_approved_on,
                  allow_editing)
            VALUES(
                  @employerID,
                  @taxYear,
                  @dge,
                  @dgeName,
                  @dgeEIN,
                  @dgeAddress,
                  @dgeCity,
                  @dgeStateID,
                  @dgeZip,
                  @dgeFname,
                  @dgeLname,
                  @dgePhone,
                  @ale,
                  @tr1,
                  @tr2, 
                  @tr3,
                  @tr4,
                  @tr5,
                  @tr,
                  @tobacco,
                  @unpaidLeave,
                  @safeHarbor,
                  @completedBy,
                  @completedOn,
                  @ebcApproved,
                  @ebcApprovedBy,
                  @ebcApprovedOn,
                  @allowEditing)
      END
ELSE
      BEGIN
            UPDATE [tax_year_approval]
            SET
                  employer_id=@employerID,
                  tax_year=@taxYear,
                  dge=@dge,
                  dge_name=@dgeName,
                  dge_ein=@dgeEIN,
                  dge_address=@dgeAddress,
                  dge_city=@dgeCity,
                  state_id=@dgeStateID,
                  dge_zip=@dgeZip,
                  dge_contact_fname=@dgeFname,
                  dge_contact_lname=@dgeLname,
                  dge_phone=@dgePhone,
                  ale=@ale,
                  tr_q1=@tr1,
                  tr_q2=@tr2,
                  tr_q3=@tr3,
                  tr_q4=@tr4,
                  tr_q5=@tr5,
                  tr_qualified=@tr,
                  tobacco=@tobacco,
                  unpaidLeave=@unpaidLeave,
                  safeHarbor=@safeHarbor,
                  completed_by=@completedBy,
                  completed_on=@completedOn,
                  ebc_approval=@ebcApproved,
                  ebc_approved_by=@ebcApprovedBy,
                  ebc_approved_on=@ebcApprovedOn,
                  allow_editing=@allowEditing
            WHERE
                  approval_id=@approvalID_Final;
      END
 
IF @approvalID_Final <= 0
      BEGIN
            SET @approvalID_Final = SCOPE_IDENTITY();
      END
 
SELECT @approvalID_Final;
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_employer]
       @employerID int,
       @name varchar(75),
       @address varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @logo varchar(50),
       @ein varchar(50),
       @employerTypeId int,
         @dbaName varchar(100)
AS
BEGIN TRY
       UPDATE employer
       SET
             name = @name, 
             [address] = @address,
             city = @city,
             state_id = @stateID, 
             zip = @zip,  
             img_logo = @logo,
             ein = @ein,
             employer_type_id = @employerTypeId,
                  DBAName = @dbaName
       WHERE
             employer_id = @employerID;
 
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_equivalency]
      @equivalencyID int,
      @employerID int,
      @name varchar(75),
      @gpID int,
      @every decimal(18,4),
      @unitID int,
      @credit decimal(18,4),
      @sdate datetime,
      @edate datetime,
      @notes varchar(1000),
      @modBy varchar(50),
      @modOn datetime, 
      @history varchar(max),
      @active bit,
      @equivTypeID int,
      @posID int,
      @actID int,
      @detID int
AS
 
BEGIN TRY
      UPDATE [equivalency]
      SET
            employer_id = @employerID,
            name = @name,
            gpID = @gpID,
            every = @every,
            unit_id = @unitID,
            credit = @credit,
            [start_date] = @sdate,
            end_date = @edate,
            notes = @notes,
            modBy = @modBy,
            modOn = @modOn,
            history = @history,
            active = @active,
            equivalency_type_id = @equivTypeID,
            position_id = @posID,
            activity_id = @actID,
            detail_id = @detID
      WHERE
            equivalency_id = @equivalencyID
 
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_PlanYearGroup]
      @modifiedBy nvarchar(50),
      @PlanYearGroupId int,
      @GroupName nvarchar(75)
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
    UPDATE [dbo].[PlanYearGroup] SET GroupName = @GroupName,
            ModifiedBy = @modifiedBy, ModifiedDate = GETDATE()
      WHERE PlanYearGroupId = @PlanYearGroupId
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
--End Migration script 22-23