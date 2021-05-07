USE [air]
GO

-- exception to the rule of an index with all fields, this table drives so many things with transmission and printing anything we can do to speed up the reads is justified. gc5
CREATE INDEX [IDX_EmployeeMonthlyDetail_EmployerId_EmployeeId_TimeFrameId_MonthlyHours_AndManyMore] ON [air].[appr].[employee_monthly_detail] ([employer_id]) INCLUDE ([employee_id], [time_frame_id], [monthly_hours], [offer_of_coverage_code], [mec_offered], [share_lowest_cost_monthly_premium], [safe_harbor_code], [enrolled], [monthly_status_id])
GO

CREATE INDEX [IDX_CoveredIndividual_EmployerId_CoveredIndividualId_EmployeeId] ON [air].[emp].[covered_individual] ([employer_id]) INCLUDE ([covered_individual_id], [employee_id])
GO

CREATE INDEX [IDX_Manifest_paymentYear_Ein] ON [air].[br].[manifest] ([payment_year], [ein])
GO
