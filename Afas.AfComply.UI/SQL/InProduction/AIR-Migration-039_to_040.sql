USE AIR
GO
ALTER TABLE air.[log].employee_yearly_detail ALTER COLUMN [storedProcedureName]  varchar(MAX) NULL
GO
ALTER TRIGGER [appr].[insertEmployeeYearlyLog]
ON [appr].[employee_yearly_detail]
FOR INSERT
AS
BEGIN
	DECLARE @ExecStr varchar(50), @Qry nvarchar(MAX)
	DECLARE @inputbuffer TABLE
	 (
	  EventType nvarchar(30), 
	  Parameters int, 
	  EventInfo nvarchar(MAX)
	 )
 
	 SET @ExecStr = 'DBCC INPUTBUFFER(' + STR(@@SPID) + ')'
 
	 INSERT INTO @inputbuffer 
	 EXEC (@ExecStr)
 
	 SET @Qry = (SELECT EventInfo FROM @inputbuffer)

	INSERT INTO [log].[employee_yearly_detail] ([employee_id]
      ,[year_id]
      ,[employer_id]
      ,[annual_offer_of_coverage_code]
      ,[annual_share_lowest_cost_monthly_premium]
      ,[annual_safe_harbor_code]
      ,[enrolled]
      ,[submittal_ready]
      ,[submitted]
      ,[correction_ready]
      ,[ack_status_code_id]
      ,[_1095C]
      ,[plan_start_month]
      ,[create_date]
      ,[modified_date]
      ,[modified_by]
      ,[insurance_type_id]
      ,[must_supply_ci_info]
      ,[is_1G]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	SELECT [employee_id]
      ,[year_id]
      ,[employer_id]
      ,[annual_offer_of_coverage_code]
      ,[annual_share_lowest_cost_monthly_premium]
      ,[annual_safe_harbor_code]
      ,[enrolled]
      ,[submittal_ready]
      ,[submitted]
      ,[correction_ready]
      ,[ack_status_code_id]
      ,[_1095C]
      ,[plan_start_month]
      ,[create_date]
      ,[modified_date]
      ,[modified_by]
      ,[insurance_type_id]
      ,[must_supply_ci_info]
      ,[is_1G]
      ,'0'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,@Qry
	FROM inserted
END
GO
ALTER TRIGGER [appr].[deleteEmployeeYearlyLog]
ON [appr].[employee_yearly_detail]
FOR DELETE
AS
BEGIN
	DECLARE @ExecStr varchar(50), @Qry nvarchar(MAX)
	DECLARE @inputbuffer TABLE
	 (
	  EventType nvarchar(30), 
	  Parameters int, 
	  EventInfo nvarchar(MAX)
	 )
 
	 SET @ExecStr = 'DBCC INPUTBUFFER(' + STR(@@SPID) + ')'
 
	 INSERT INTO @inputbuffer 
	 EXEC (@ExecStr)
 
	 SET @Qry = (SELECT EventInfo FROM @inputbuffer)

	INSERT INTO [log].[employee_yearly_detail] ([employee_id]
      ,[year_id]
      ,[employer_id]
      ,[annual_offer_of_coverage_code]
      ,[annual_share_lowest_cost_monthly_premium]
      ,[annual_safe_harbor_code]
      ,[enrolled]
      ,[submittal_ready]
      ,[submitted]
      ,[correction_ready]
      ,[ack_status_code_id]
      ,[_1095C]
      ,[plan_start_month]
      ,[create_date]
      ,[modified_date]
      ,[modified_by]
      ,[insurance_type_id]
      ,[must_supply_ci_info]
      ,[is_1G]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	SELECT [employee_id]
      ,[year_id]
      ,[employer_id]
      ,[annual_offer_of_coverage_code]
      ,[annual_share_lowest_cost_monthly_premium]
      ,[annual_safe_harbor_code]
      ,[enrolled]
      ,[submittal_ready]
      ,[submitted]
      ,[correction_ready]
      ,[ack_status_code_id]
      ,[_1095C]
      ,[plan_start_month]
      ,[create_date]
      ,[modified_date]
      ,[modified_by]
      ,[insurance_type_id]
      ,[must_supply_ci_info]
      ,[is_1G]
      ,'1'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,@Qry
	FROM deleted
END