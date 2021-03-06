USE [air-demo]
GO
CREATE ROLE [air-user]
GO
GRANT EXECUTE ON [ale].[spGet_submittal_ready_ales] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [ale].[spUpdateDistrictName] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spBUILD_APPROVED] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spGet_1094_5_C_upstream_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spGet_employee_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spGet_non_submittal_ready_ales] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spGet_self_insured_without_ci_information] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spGet_submittal_ready_ales] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spGet_submittal_ready_employees] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spInsert_ale_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spInsert_ale_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spInsert_employee_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spInsert_employee_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spInsert_employee_yearly_detail_init] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [appr].[spUpdate_1095C_status] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [br].[spGet_manifest_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [br].[spGet_upstream_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [br].[spInsert_br_output_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spGet_ci_coding] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spGet_employee_1095C] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spGet_employee_coding] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spGet_employee_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spGet_employees_per_employer] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spGet_submittal_ready_employees] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spInsert_employee_1095C] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spUpdate_ci_coding] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spUpdate_employee_coding] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spETL_Build] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spETL_ShortBuild] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spGetEmployerByIdOrName] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_ale_dge] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_ale_employer] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_ale_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_ale_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_covered_individuals] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_covered_individuals_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_employee] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_employee_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_employee_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spUpdate_1095C_status] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spUpdate_ale_dge] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spUpdate_covered_individuals] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spUpdate_employee] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spUpdate_employer] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [fdf].[spGet_1094_5_C_upstream_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [fdf].[spGet_employee_1095C_line_num] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [fdf].[spGetFirstEmployeeInTransmission] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [fdf].[spInsert_1094_5_C] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [fdf].[spInsert_1094_5_C_Correction] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [sr].[spGet_open_submittals] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [sr].[spGetAcceptedStatusXML] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [sr].[spInsert_request_error] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [sr].[spInsert_status_request] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_64s] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_acaui_business_header] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_acaui_business_header_no_namespaces] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_Check_64s] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_Envelope_From_64] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_transmission_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_transmitted_ales] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGetWSSE] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spInsert_fault] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spInsert_request] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spUpdate_header_transmitted_xml] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [ale].[dge] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [ale].[employer] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [ale].[monthly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [ale].[other_ale_group_members] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [ale].[yearly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [appr].[ale_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [appr].[ale_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [appr].[employee_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [appr].[employee_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [br].[manifest] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [br].[output_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [br].[status_code] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [br].[vendor] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [emp].[covered_individual] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [emp].[covered_individual_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [emp].[employee] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [emp].[employee_1095C] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [emp].[monthly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [emp].[monthly_status] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [emp].[yearly_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [fdf].[_1094C] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [fdf].[_1095C] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [gen].[month] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [gen].[number] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [gen].[state_code] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [gen].[time_frame] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [il].[_1094C_upstream] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [il].[_1095C_upstream] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [il].[_4980H_code] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [il].[error_code] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [il].[error_code_mapping] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [il].[mec_code] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [sr].[output_detail] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [sr].[output_error_type] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [sr].[request_error] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [sr].[status_code] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [sr].[status_request] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [tr].[fault] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [tr].[header] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [tr].[message_type] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [tr].[transmitter] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[aca_column] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[aca_function] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[aca_stored_proc] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[aca_table] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[aca_to_air_mapping] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[air_column] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[air_function] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[air_schema] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[air_stored_proc] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [utility].[air_table] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_merged_covered_individuals] TO [air-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[View_qa_covered_vs_enrolled] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [ale].[spGet_submittal_ready_ales] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [br].[spGet_manifest_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [br].[spGet_upstream_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [br].[spInsert_br_output_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spGet_employee_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [emp].[spGet_submittal_ready_employees] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_ale_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_ale_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_employee] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_employee_monthly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_employee_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spInsert_employee_yearly_detail] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spUpdate_employee] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [etl].[spUpdate_employer] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [fdf].[spGet_1094_5_C_upstream_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [fdf].[spInsert_1094_5_C] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [sr].[spGet_open_submittals] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_64s] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_acaui_business_header] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_acaui_business_header_no_namespaces] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_Check_64s] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_Envelope_From_64] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spGet_transmission_xml] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spInsert_fault] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spInsert_request] TO [air-user] AS [dbo]
GO
GRANT EXECUTE ON [tr].[spUpdate_header_transmitted_xml] TO [air-user] AS [dbo]
GO
