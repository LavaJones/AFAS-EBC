USE air
GO
ALTER TABLE [log].[empEmployee] ALTER COLUMN [storedProcedureName] varchar(MAX) NULL
ALTER TABLE [log].[aleEmployer] ALTER COLUMN [storedProcedureName] varchar(MAX) NULL
ALTER TABLE [log].[covered_individual] ALTER COLUMN [storedProcedureName] varchar(MAX) NULL
ALTER TABLE [log].[covered_individual_monthly_detail] ALTER COLUMN [storedProcedureName] varchar(MAX) NULL
ALTER TABLE [log].[employee_monthly_detail] ALTER COLUMN [storedProcedureName] varchar(MAX) NULL
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
      ,@Qry
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
      ,@Qry
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
      ,@Qry
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
      ,@Qry
	FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [emp].[insertLog]
ON [emp].covered_individual
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
      ,@Qry
	 FROM inserted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [emp].[deleteLog]
ON [emp].covered_individual
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
      ,@Qry
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
      ,@Qry
	FROM inserted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
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
      ,@Qry
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
      ,@Qry
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
      ,@Qry
	  FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO