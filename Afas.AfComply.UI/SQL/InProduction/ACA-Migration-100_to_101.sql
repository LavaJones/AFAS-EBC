USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[insertTaxYear1095cApprovalLog]
ON [dbo].[tax_year_1095c_approval]
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

	INSERT INTO [log].[tax_year_1095c_approval]	([tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[printed]
	  ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	  SELECT [tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[printed]
      ,0
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry,255)
	 FROM inserted
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER TRIGGER [dbo].[deleteTaxYear1095cApprovalLog]
ON [dbo].[tax_year_1095c_approval]
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

	INSERT INTO [log].[tax_year_1095c_approval]	([tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[printed]
	  ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	  SELECT [tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[printed]
      ,1
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,LEFT(@Qry, 255)
	FROM deleted
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
