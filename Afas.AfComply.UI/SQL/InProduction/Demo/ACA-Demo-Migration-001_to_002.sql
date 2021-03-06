USE [aca-demo]
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
