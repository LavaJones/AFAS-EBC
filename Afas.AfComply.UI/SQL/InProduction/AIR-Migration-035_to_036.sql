USE air
GO
IF OBJECT_ID('[log].[empEmployee]') is not null
	BEGIN
		DROP TABLE [log].[empEmployee]
		DROP TABLE [log].[aleEmployer]
		DROP TRIGGER [emp].[insertEmpEmployee]
		DROP TRIGGER [emp].[deleteEmpEmployee]
		DROP TRIGGER [ale].[insertAleEmployer]
		DROP TRIGGER [ale].[deleteAleEmployer]
	END 
GO
CREATE TABLE [log].[empEmployee] (
	[empEmployeeLogId] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[first_name] [nvarchar](50) NOT NULL,
	[middle_name] [nvarchar](50) NULL,
	[last_name] [nvarchar](50) NOT NULL,
	[name_suffix] [nvarchar](50) NULL,
	[address] [nvarchar](50) NOT NULL,
	[city] [nvarchar](50) NOT NULL,
	[state_code] [nchar](2) NOT NULL,
	[zipcode] [nchar](5) NOT NULL,
	[zipcode_ext] [nchar](4) NULL,
	[telephone] [nvarchar](12) NULL,
	[ssn] [nvarchar](50) NOT NULL,
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](50) NULL
CONSTRAINT [PK_emEmployee] PRIMARY KEY CLUSTERED
(
	[empEmployeeLogId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [log].[aleEmployer] (
	[aleEmployerLogId] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[ein] [nchar](9) NOT NULL,
	[name] [varchar](75) NOT NULL,
	[employer_control_name] [nvarchar](75) NULL,
	[address] [nvarchar](100) NOT NULL,
	[city] [nvarchar](50) NOT NULL,
	[state_code] [nchar](2) NOT NULL,
	[zipcode] [nchar](5) NOT NULL,
	[zipcode_ext] [nchar](4) NULL,
	[contact_first_name] [nvarchar](50) NOT NULL,
	[contact_middle_name] [nvarchar](50) NULL,
	[contact_last_name] [nvarchar](50) NOT NULL,
	[contact_name_suffix] [nvarchar](10) NULL,
	[contact_telephone] [nchar](10) NOT NULL,
	[dge_ein] [nchar](9) NULL,
	[test_id] [nchar](3) NULL,
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](50) NULL
CONSTRAINT [PK_aleEmployer] PRIMARY KEY CLUSTERED
(
	[aleEmployerLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TRIGGER [emp].[insertEmpEmployee]
ON [emp].[employee]
FOR INSERT
AS
BEGIN TRY
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
      ,OBJECT_NAME(@@PROCID)
	FROM inserted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
CREATE TRIGGER [emp].[deleteEmpEmployee]
ON [emp].[employee]
FOR DELETE
AS
BEGIN TRY
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
      ,OBJECT_NAME(@@PROCID)
	FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
CREATE TRIGGER [ale].[insertAleEmployer]
ON [ale].[employer]
FOR INSERT
AS
BEGIN TRY
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
      ,OBJECT_NAME(@@PROCID)
	FROM inserted

END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH
GO
CREATE TRIGGER [ale].[deleteAleEmployer]
ON [ale].[employer]
FOR DELETE
AS
BEGIN TRY
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
      ,OBJECT_NAME(@@PROCID)
	FROM deleted
END TRY
BEGIN CATCH
	EXEC aca.dbo.INSERT_ErrorLogging
END CATCH