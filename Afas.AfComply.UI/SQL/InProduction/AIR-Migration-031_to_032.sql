USE [air]
GO

CREATE INDEX [IDX_ApprEmployeeMonthlyDetail_EmployerId] ON [appr].[employee_monthly_detail]([employer_id]) INCLUDE ([employee_id], [time_frame_id])
GO

CREATE INDEX [IDX_ApprEmployeeMonthlyDetail_MonthlyStatusId] ON [appr].[employee_monthly_detail]([monthly_status_id])
GO

CREATE INDEX [IDX_ApprEmployeeMonthlyDetail_InsuranceTypeId] ON [appr].[employee_monthly_detail]([insurance_type_id])
GO

CREATE INDEX [IDX_ApprEmployeeMonthlyDetail_EmployerId_MonthlyStatusId] ON [appr].[employee_monthly_detail]([employer_id], [monthly_status_id]) INCLUDE ([employee_id], [time_frame_id])
GO

CREATE INDEX [IDX_ApprEmployeeMonthlyDetail_TimeFrameId_EmployerId] ON [appr].[employee_monthly_detail]([time_frame_id], [employer_id])
GO

CREATE INDEX [IDX_EmployeeMonthlyDetail_EmployerId] ON [emp].[monthly_detail]([employer_id]) INCLUDE ([employee_id], [time_frame_id])
GO

CREATE INDEX [IDX_EmployeeMonthlyDetail_MonthlyStatusId] ON [emp].[monthly_detail]([monthly_status_id])
GO

CREATE INDEX [IDX_EmployeeMonthlyDetail_InsuranceTypeId] ON [emp].[monthly_detail]([insurance_type_id])
GO

CREATE INDEX [IDX_EmployeeMonthlyDetail_EmployerId_MonthlyStatusId] ON [emp].[monthly_detail]([employer_id], [monthly_status_id]) INCLUDE ([employee_id], [time_frame_id])
GO

CREATE INDEX [IDX_EmployeeMonthlyDetail_TimeFrameId_EmployerId] ON [emp].[monthly_detail]([time_frame_id], [employer_id])
GO

CREATE INDEX [IDX_ApprEmployeeYearlyDetail_EmployerId] ON [appr].[employee_yearly_detail]([employer_id]) INCLUDE ([employee_id])
GO

CREATE INDEX [IDX_ApprEmployeeYearlyDetail_InsuranceTypeId] ON [appr].[employee_yearly_detail]([insurance_type_id])
GO

CREATE INDEX [IDX_ApprEmployeeYearlyDetail_EmployerId__1095C] ON [appr].[employee_yearly_detail]([employer_id], [_1095c]) INCLUDE ([employee_id])
GO

CREATE INDEX [IDX_EmployeeYearlyDetail_EmployerId] ON [emp].[yearly_detail]([employer_id]) INCLUDE ([employee_id])
GO

CREATE INDEX [IDX_EmployeeYearlyDetail_InsuranceTypeId] ON [emp].[yearly_detail]([insurance_type_id])
GO

CREATE INDEX [IDX_EmployeeYearlyDetail_EmployerId__1095C] ON [emp].[yearly_detail]([employer_id], [_1095c]) INCLUDE ([employee_id])
GO

CREATE INDEX [IDX_EmployeeCoveredIndividual_EmployeeId] ON [emp].[covered_individual]([employee_id]) INCLUDE ([covered_individual_id])
GO

CREATE INDEX [IDX_Employee_EmployerId] ON [emp].[employee]([employer_id])
GO

CREATE INDEX [IDX_Employee_StateCode] ON [emp].[employee]([state_code])
GO

CREATE INDEX [IDX_Employee_LastName] ON [emp].[employee]([last_name])
GO
