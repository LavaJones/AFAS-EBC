USE [air]
GO
/****** Object:  Trigger [emp].[deleteMonthlyLog]    Script Date: 3/27/2017 4:48:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [emp].[deleteMonthlyLog]
ON [emp].[covered_individual_monthly_detail]
FOR DELETE
AS
BEGIN TRY
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

	INSERT INTO [log].[covered_individual_monthly_detail] ([cim_id]
      ,[covered_individual_id]
      ,[time_frame_id]
      ,[covered_indicator]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	 SELECT [cim_id]
      ,[covered_individual_id]
      ,[time_frame_id]
      ,[covered_indicator]
      ,'1'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [emp].[insertMonthlyLog]
ON [emp].[covered_individual_monthly_detail]
FOR INSERT 
AS
BEGIN TRY
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

	INSERT INTO [log].[covered_individual_monthly_detail] ([cim_id]
      ,[covered_individual_id]
      ,[time_frame_id]
      ,[covered_indicator]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	 SELECT [cim_id]
      ,[covered_individual_id]
      ,[time_frame_id]
      ,[covered_indicator]
      ,'0'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	FROM inserted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [emp].[deleteLog]
ON [emp].[covered_individual]
FOR DELETE
AS
BEGIN TRY
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

	INSERT INTO [log].[covered_individual]	([employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[ssn]
      ,[birth_date]
      ,[annual_coverage_indicator]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	  SELECT [employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[ssn]
      ,[birth_date]
      ,[annual_coverage_indicator]
      ,1
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [emp].[insertLog]
ON [emp].[covered_individual]
FOR INSERT
AS
BEGIN TRY
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

	INSERT INTO [log].[covered_individual]	([employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[ssn]
      ,[birth_date]
      ,[annual_coverage_indicator]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	  SELECT [employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[ssn]
      ,[birth_date]
      ,[annual_coverage_indicator]
      ,0
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry,255)
	 FROM inserted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [ale].[deleteAleEmployer]
ON [ale].[employer]
FOR DELETE
AS
BEGIN TRY
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
	
	INSERT INTO [log].[aleEmployer] ([employer_id]
      ,[ein]
      ,[name]
      ,[employer_control_name]
      ,[address]
      ,[city]
      ,[state_code]
      ,[zipcode]
      ,[zipcode_ext]
      ,[contact_first_name]
      ,[contact_middle_name]
      ,[contact_last_name]
      ,[contact_name_suffix]
      ,[contact_telephone]
      ,[dge_ein]
      ,[test_id]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	SELECT [employer_id]
      ,[ein]
      ,[name]
      ,[employer_control_name]
      ,[address]
      ,[city]
      ,[state_code]
      ,[zipcode]
      ,[zipcode_ext]
      ,[contact_first_name]
      ,[contact_middle_name]
      ,[contact_last_name]
      ,[contact_name_suffix]
      ,[contact_telephone]
      ,[dge_ein]
      ,[test_id]
      ,1
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [ale].[insertAleEmployer]
ON [ale].[employer]
FOR INSERT
AS
BEGIN TRY
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

	INSERT INTO [log].[aleEmployer] ([employer_id]
      ,[ein]
      ,[name]
      ,[employer_control_name]
      ,[address]
      ,[city]
      ,[state_code]
      ,[zipcode]
      ,[zipcode_ext]
      ,[contact_first_name]
      ,[contact_middle_name]
      ,[contact_last_name]
      ,[contact_name_suffix]
      ,[contact_telephone]
      ,[dge_ein]
      ,[test_id]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	SELECT [employer_id]
      ,[ein]
      ,[name]
      ,[employer_control_name]
      ,[address]
      ,[city]
      ,[state_code]
      ,[zipcode]
      ,[zipcode_ext]
      ,[contact_first_name]
      ,[contact_middle_name]
      ,[contact_last_name]
      ,[contact_name_suffix]
      ,[contact_telephone]
      ,[dge_ein]
      ,[test_id]
      ,0
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	FROM inserted

END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [emp].[deleteEmpEmployee]
ON [emp].[employee]
FOR DELETE
AS
BEGIN TRY
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

	INSERT INTO [log].[empEmployee] ([employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[address]
      ,[city]
      ,[state_code]
      ,[zipcode]
      ,[zipcode_ext]
      ,[telephone]
      ,[ssn]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	SELECT [employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[address]
      ,[city]
      ,[state_code]
      ,[zipcode]
      ,[zipcode_ext]
      ,[telephone]
      ,[ssn]
      ,0
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [emp].[insertEmpEmployee]
ON [emp].[employee]
FOR INSERT
AS
BEGIN TRY
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

	INSERT INTO [log].[empEmployee] ([employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[address]
      ,[city]
      ,[state_code]
      ,[zipcode]
      ,[zipcode_ext]
      ,[telephone]
      ,[ssn]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	SELECT [employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[address]
      ,[city]
      ,[state_code]
      ,[zipcode]
      ,[zipcode_ext]
      ,[telephone]
      ,[ssn]
      ,0
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	FROM inserted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [appr].[deleteEmployeeMonthlyLog]
ON [appr].[employee_monthly_detail]
FOR DELETE
AS
BEGIN TRY
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
	
	INSERT INTO [log].[employee_monthly_detail] ([employee_id]
      ,[time_frame_id]
      ,[employer_id]
      ,[monthly_hours]
      ,[offer_of_coverage_code]
      ,[mec_offered]
      ,[share_lowest_cost_monthly_premium]
      ,[safe_harbor_code]
      ,[enrolled]
      ,[monthly_status_id]
      ,[insurance_type_id]
      ,[hra_flex_contribution]
      ,[create_date]
      ,[modified_date]
      ,[modified_by]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	SELECT [employee_id]
      ,[time_frame_id]
      ,[employer_id]
      ,[monthly_hours]
      ,[offer_of_coverage_code]
      ,[mec_offered]
      ,[share_lowest_cost_monthly_premium]
      ,[safe_harbor_code]
      ,[enrolled]
      ,[monthly_status_id]
      ,[insurance_type_id]
      ,[hra_flex_contribution]
      ,[create_date]
      ,[modified_date]
      ,[modified_by]
      ,'1'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	  FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [appr].[insertEmployeeMonthlyLog]
ON [appr].[employee_monthly_detail]
FOR INSERT
AS
BEGIN TRY
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

	INSERT INTO [log].[employee_monthly_detail] ([employee_id]
      ,[time_frame_id]
      ,[employer_id]
      ,[monthly_hours]
      ,[offer_of_coverage_code]
      ,[mec_offered]
      ,[share_lowest_cost_monthly_premium]
      ,[safe_harbor_code]
      ,[enrolled]
      ,[monthly_status_id]
      ,[insurance_type_id]
      ,[hra_flex_contribution]
      ,[create_date]
      ,[modified_date]
      ,[modified_by]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	SELECT [employee_id]
      ,[time_frame_id]
      ,[employer_id]
      ,[monthly_hours]
      ,[offer_of_coverage_code]
      ,[mec_offered]
      ,[share_lowest_cost_monthly_premium]
      ,[safe_harbor_code]
      ,[enrolled]
      ,[monthly_status_id]
      ,[insurance_type_id]
      ,[hra_flex_contribution]
      ,[create_date]
      ,[modified_date]
      ,[modified_by]
      ,'0'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	  FROM inserted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [appr].[deleteEmployeeYearlyLog]
ON [appr].[employee_yearly_detail]
FOR DELETE
AS
BEGIN TRY
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
      ,LEFT(@Qry, 255)
	FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [appr].[insertEmployeeYearlyLog]
ON [appr].[employee_yearly_detail]
FOR INSERT
AS
BEGIN TRY
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
      ,LEFT(@Qry, 255)
	FROM inserted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH


