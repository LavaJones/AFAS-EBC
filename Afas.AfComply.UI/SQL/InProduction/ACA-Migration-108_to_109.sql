USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [log].[insurance_coverage_editable](
	[insuranceCoverageEditableLogId] [int] IDENTITY(1,1) NOT NULL,
	[row_id] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[dependent_id] [int] NULL,
	[tax_year] [int] NOT NULL,
	[Jan] [bit] NOT NULL,
	[Feb] [bit] NOT NULL,
	[Mar] [bit] NOT NULL,
	[Apr] [bit] NOT NULL,
	[May] [bit] NOT NULL,
	[Jun] [bit] NOT NULL,
	[Jul] [bit] NOT NULL,
	[Aug] [bit] NOT NULL,
	[Sept] [bit] NOT NULL,
	[Oct] [bit] NOT NULL,
	[Nov] [bit] NOT NULL,
	[Dec] [bit] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](max) NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [log].[insurance_coverage](
	[insuranceCoverageLogId] [int] IDENTITY(1,1) NOT NULL,
	[row_id] [int] NOT NULL,
	[tax_year] [int] NOT NULL,
	[carrier_id] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[dependent_id] [int] NULL,
	[all12] [bit] NOT NULL,
	[jan] [bit] NOT NULL,
	[feb] [bit] NOT NULL,
	[mar] [bit] NOT NULL,
	[apr] [bit] NOT NULL,
	[may] [bit] NOT NULL,
	[jun] [bit] NOT NULL,
	[jul] [bit] NOT NULL,
	[aug] [bit] NOT NULL,
	[sep] [bit] NOT NULL,
	[oct] [bit] NOT NULL,
	[nov] [bit] NOT NULL,
	[dec] [bit] NOT NULL,
	[history] [varchar](max) NULL,
	[ResourceId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](max) NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
CREATE TRIGGER [dbo].[insertInsuranceCoverageEditableLog]
ON [dbo].[insurance_coverage_editable]
FOR INSERT
AS
BEGIN
	DECLARE @count int
	SELECT @count = COUNT(*) FROM inserted 
	IF (@count <> 0) 
		DECLARE @ExecStr varchar(MAX), @Qry nvarchar(MAX)
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
		INSERT INTO [log].[insurance_coverage_editable]	([row_id]
		  ,[employee_id]
		  ,[employer_id]
		  ,[dependent_id]
		  ,[tax_year]
		  ,[Jan]
		  ,[Feb]
		  ,[Mar]
		  ,[Apr]
		  ,[May]
		  ,[Jun]
		  ,[Jul]
		  ,[Aug]
		  ,[Sept]
		  ,[Oct]
		  ,[Nov]
		  ,[Dec]
		  ,[ResourceId]
		  ,[deleted]
		  ,[modifiedDate]
		  ,[modifiedBy]
		  ,[storedProcedureName])
		SELECT [row_id]
		  ,[employee_id]
		  ,[employer_id]
		  ,[dependent_id]
		  ,[tax_year]
		  ,[Jan]
		  ,[Feb]
		  ,[Mar]
		  ,[Apr]
		  ,[May]
		  ,[Jun]
		  ,[Jul]
		  ,[Aug]
		  ,[Sept]
		  ,[Oct]
		  ,[Nov]
		  ,[Dec]
		  ,[ResourceId]
		  ,0
		  ,GETDATE()
		  ,'SYSTEM TRIGGER'
		  ,LEFT(@Qry,255)
		 FROM inserted
END
GO
CREATE TRIGGER [dbo].[deleteInsuranceCoverageEditableLog]
ON [dbo].[insurance_coverage_editable]
FOR DELETE
AS
BEGIN
	DECLARE @count int
	SELECT @count = COUNT(*) FROM deleted 
	IF (@count <> 0) 
		DECLARE @ExecStr varchar(MAX), @Qry nvarchar(MAX)
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
		INSERT INTO [log].[insurance_coverage_editable]	([row_id]
		  ,[employee_id]
		  ,[employer_id]
		  ,[dependent_id]
		  ,[tax_year]
		  ,[Jan]
		  ,[Feb]
		  ,[Mar]
		  ,[Apr]
		  ,[May]
		  ,[Jun]
		  ,[Jul]
		  ,[Aug]
		  ,[Sept]
		  ,[Oct]
		  ,[Nov]
		  ,[Dec]
		  ,[ResourceId]
		  ,[deleted]
		  ,[modifiedDate]
		  ,[modifiedBy]
		  ,[storedProcedureName])
		SELECT [row_id]
		  ,[employee_id]
		  ,[employer_id]
		  ,[dependent_id]
		  ,[tax_year]
		  ,[Jan]
		  ,[Feb]
		  ,[Mar]
		  ,[Apr]
		  ,[May]
		  ,[Jun]
		  ,[Jul]
		  ,[Aug]
		  ,[Sept]
		  ,[Oct]
		  ,[Nov]
		  ,[Dec]
		  ,[ResourceId]
		  ,0
		  ,GETDATE()
		  ,'SYSTEM TRIGGER'
		  ,LEFT(@Qry,255)
		 FROM deleted
END
GO
CREATE TRIGGER [dbo].[insertInsuranceCoverageLog]
ON [dbo].[insurance_coverage]
FOR INSERT
AS
BEGIN
	DECLARE @count int
	SELECT @count = COUNT(*) FROM inserted
	IF (@count <> 0) 
		DECLARE @ExecStr varchar(MAX), @Qry nvarchar(MAX)
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
		INSERT INTO [log].[insurance_coverage]	([row_id]
		  ,[tax_year]
		  ,[carrier_id]
		  ,[employee_id]
		  ,[dependent_id]
		  ,[all12]
		  ,[jan]
		  ,[feb]
		  ,[mar]
		  ,[apr]
		  ,[may]
		  ,[jun]
		  ,[jul]
		  ,[aug]
		  ,[sep]
		  ,[oct]
		  ,[nov]
		  ,[dec]
		  ,[history]
		  ,[ResourceId]
		  ,[deleted]
		  ,[modifiedDate]
		  ,[modifiedBy]
		  ,[storedProcedureName])
		SELECT [row_id]
		  ,[tax_year]
		  ,[carrier_id]
		  ,[employee_id]
		  ,[dependent_id]
		  ,[all12]
		  ,[jan]
		  ,[feb]
		  ,[mar]
		  ,[apr]
		  ,[may]
		  ,[jun]
		  ,[jul]
		  ,[aug]
		  ,[sep]
		  ,[oct]
		  ,[nov]
		  ,[dec]
		  ,[history]
		  ,[ResourceId]
		  ,0
		  ,GETDATE()
		  ,'SYSTEM TRIGGER'
		  ,LEFT(@Qry,255)
		 FROM inserted
END
GO
CREATE TRIGGER [dbo].[deleteInsuranceCoverageLog]
ON [dbo].[insurance_coverage]
FOR DELETE
AS
BEGIN
	DECLARE @count int
	SELECT @count = COUNT(*) FROM deleted
	IF (@count <> 0) 
		DECLARE @ExecStr varchar(MAX), @Qry nvarchar(MAX)
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
		INSERT INTO [log].[insurance_coverage]	([row_id]
		  ,[tax_year]
		  ,[carrier_id]
		  ,[employee_id]
		  ,[dependent_id]
		  ,[all12]
		  ,[jan]
		  ,[feb]
		  ,[mar]
		  ,[apr]
		  ,[may]
		  ,[jun]
		  ,[jul]
		  ,[aug]
		  ,[sep]
		  ,[oct]
		  ,[nov]
		  ,[dec]
		  ,[history]
		  ,[ResourceId]
		  ,[deleted]
		  ,[modifiedDate]
		  ,[modifiedBy]
		  ,[storedProcedureName])
		SELECT [row_id]
		  ,[tax_year]
		  ,[carrier_id]
		  ,[employee_id]
		  ,[dependent_id]
		  ,[all12]
		  ,[jan]
		  ,[feb]
		  ,[mar]
		  ,[apr]
		  ,[may]
		  ,[jun]
		  ,[jul]
		  ,[aug]
		  ,[sep]
		  ,[oct]
		  ,[nov]
		  ,[dec]
		  ,[history]
		  ,[ResourceId]
		  ,0
		  ,GETDATE()
		  ,'SYSTEM TRIGGER'
		  ,LEFT(@Qry,255)
		 FROM deleted
END