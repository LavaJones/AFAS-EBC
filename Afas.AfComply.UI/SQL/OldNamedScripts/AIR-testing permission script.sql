USE air
GO
GRANT EXECUTE ON [log].[aleEmployer] TO [air-user] AS [DBO]
GO
GRANT SELECT ON [log].[covered_individual] TO [air-user] AS [DBO]
GO
GRANT SELECT ON [log].[covered_individual_monthly_detail] TO [air-user] AS [DBO]
GO
GRANT SELECT ON [log].[empEmployee] TO [air-user] AS [DBO]
GO
GRANT SELECT ON [log].[employee_monthly_detail] TO [air-user] AS [DBO]
GO
GRANT SELECT ON [log].[employee_yearly_detail] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [tr].[INSERT_UPDATE_header] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [br].[INSERT_UPDATE_manifest] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [fdf].[INSERT_UPDATE_1094C] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [fdf].[INSERT_UPDATE_1095C] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [sr].[INSERT_UPDATE_request_error] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [sr].[INSERT_UPDATE_status_request] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_submission_statuses] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_receipt_statuses] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employer_submissions] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employer_status_request] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employer_status_error] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employer_manifest] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employeeID_status_error] TO [air-user] AS [DBO]
GO
GRANT EXECUTE ON [dbo].[sp_AIR_INSERT_output_detail_receipt] TO [air-user] AS [DBO]
GO