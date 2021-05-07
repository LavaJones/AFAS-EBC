USE [aca]
GO

ALTER TABLE [dbo].[ArchiveFileInfo] ALTER COLUMN EmployerId integer NOT NULL
GO
ALTER TABLE [dbo].[ArchiveFileInfo]  WITH NOCHECK ADD CONSTRAINT [FK_ArchiveFileInfo_employer] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[NightlyCalculation] WITH NOCHECK ADD CONSTRAINT [FK_NightlyCalculation_employer] FOREIGN KEY([employerId])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[NightlyCalculation] ADD CONSTRAINT PK_CalculationId PRIMARY KEY (CalculationId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_alert_ResourceId')
	DROP INDEX ak_alert_ResourceId ON [dbo].[alert]
GO
CREATE UNIQUE INDEX ak_alert_ResourceId ON [dbo].[alert] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_ArchiveFileInfo_ResourceId')
	DROP INDEX ak_ArchiveFileInfo_ResourceId ON [dbo].[ArchiveFileInfo]
GO
CREATE UNIQUE INDEX ak_ArchiveFileInfo_ResourceId ON [dbo].[ArchiveFileInfo] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_batch_ResourceId')
	DROP INDEX ak_batch_ResourceId ON [dbo].[batch]
GO
CREATE UNIQUE INDEX ak_batch_ResourceId ON [dbo].[batch] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_BreakInService_ResourceId')
	DROP INDEX ak_BreakInService_ResourceId ON [dbo].[BreakInService]
GO
CREATE UNIQUE INDEX ak_BreakInService_ResourceId ON [dbo].[BreakInService] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_ResourceId')
	DROP INDEX ak_employee_ResourceId ON [dbo].[employee]
GO
CREATE UNIQUE INDEX ak_employee_ResourceId ON [dbo].[employee] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_classification_ResourceId')
	DROP INDEX ak_employee_classification_ResourceId ON [dbo].[employee_classification]
GO
CREATE UNIQUE INDEX ak_employee_classification_ResourceId ON [dbo].[employee_classification] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_type_ResourceId')
	DROP INDEX ak_employee_type_ResourceId ON [dbo].[employee_type]
GO
CREATE UNIQUE INDEX ak_employee_type_ResourceId ON [dbo].[employee_type] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employer_ResourceId')
	DROP INDEX ak_employer_ResourceId ON [dbo].[employer]
GO
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
CREATE UNIQUE INDEX ak_employer_ResourceId ON [dbo].[employer] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_equivalency_ResourceId')
	DROP INDEX ak_equivalency_ResourceId ON [dbo].[equivalency]
GO
CREATE UNIQUE INDEX ak_equivalency_ResourceId ON [dbo].[equivalency] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_gross_pay_type_ResourceId')
	DROP INDEX ak_gross_pay_type_ResourceId ON [dbo].[gross_pay_type]
GO
CREATE UNIQUE INDEX ak_gross_pay_type_ResourceId ON [dbo].[gross_pay_type] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_hr_status_ResourceId')
	DROP INDEX ak_hr_status_ResourceId ON [dbo].[hr_status]
GO
CREATE UNIQUE INDEX ak_hr_status_ResourceId ON [dbo].[hr_status] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_import_employee_ResourceId')
	DROP INDEX ak_import_employee_ResourceId ON [dbo].[import_employee]
GO
CREATE UNIQUE INDEX ak_import_employee_ResourceId ON [dbo].[import_employee] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_import_payroll_ResourceId')
	DROP INDEX ak_import_payroll_ResourceId ON [dbo].[import_payroll]
GO
CREATE UNIQUE INDEX ak_import_payroll_ResourceId ON [dbo].[import_payroll] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_measurement_ResourceId')
	DROP INDEX ak_measurement_ResourceId ON [dbo].[measurement]
GO
CREATE UNIQUE INDEX ak_measurement_ResourceId ON [dbo].[measurement] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_measurementBreakInService_ResourceId')
	DROP INDEX ak_measurementBreakInService_ResourceId ON [dbo].[measurementBreakInService]
GO
CREATE UNIQUE INDEX ak_measurementBreakInService_ResourceId ON [dbo].[measurementBreakInService] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_payroll_ResourceId')
	DROP INDEX ak_payroll_ResourceId ON [dbo].[payroll]
GO
CREATE UNIQUE INDEX ak_payroll_ResourceId ON [dbo].[payroll] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_payroll_summer_averages_ResourceId')
	DROP INDEX ak_payroll_summer_averages_ResourceId ON [dbo].[payroll_summer_averages]
GO
CREATE UNIQUE INDEX ak_payroll_summer_averages_ResourceId ON [dbo].[payroll_summer_averages] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_plan_year_ResourceId')
	DROP INDEX ak_plan_year_ResourceId ON [dbo].[plan_year]
GO
CREATE UNIQUE INDEX ak_plan_year_ResourceId ON [dbo].[plan_year] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_user_ResourceId')
	DROP INDEX ak_user_ResourceId ON [dbo].[user]
GO
CREATE UNIQUE INDEX ak_user_ResourceId ON [dbo].[user] (ResourceId)
GO
--this is a stub
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_alert_archive_ResourceId')
	DROP INDEX ak_alert_archive_ResourceId ON [dbo].[alert_archive]
GO
CREATE UNIQUE INDEX ak_alert_archive_ResourceId ON [dbo].[alert_archive] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_dependents_ResourceId')
	DROP INDEX ak_employee_dependents_ResourceId ON [dbo].[employee_dependents]
GO
CREATE UNIQUE INDEX ak_employee_dependents_ResourceId ON [dbo].[employee_dependents] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_insurance_offer_ResourceId')
	DROP INDEX ak_employee_insurance_offer_ResourceId ON [dbo].[employee_insurance_offer]
GO
CREATE UNIQUE INDEX ak_employee_insurance_offer_ResourceId ON [dbo].[employee_insurance_offer] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_employee_insurance_offer_archive_ResourceId')
	DROP INDEX ak_employee_insurance_offer_archive_ResourceId ON [dbo].[employee_insurance_offer_archive]
GO
CREATE UNIQUE INDEX ak_employee_insurance_offer_archive_ResourceId ON [dbo].[employee_insurance_offer_archive] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_gross_pay_filter_ResourceId')
	DROP INDEX ak_gross_pay_filter_ResourceId ON [dbo].[gross_pay_filter]
GO
CREATE UNIQUE INDEX ak_gross_pay_filter_ResourceId ON [dbo].[gross_pay_filter] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_import_insurance_coverage_ResourceId')
	DROP INDEX ak_import_insurance_coverage_ResourceId ON [dbo].[import_insurance_coverage]
GO
CREATE UNIQUE INDEX ak_import_insurance_coverage_ResourceId ON [dbo].[import_insurance_coverage] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_import_insurance_coverage_archive_ResourceId')
	DROP INDEX ak_import_insurance_coverage_archive_ResourceId ON [dbo].[import_insurance_coverage_archive]
GO
CREATE UNIQUE INDEX ak_import_insurance_coverage_archive_ResourceId ON [dbo].[import_insurance_coverage_archive] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_insurance_ResourceId')
	DROP INDEX ak_insurance_ResourceId ON [dbo].[insurance]
GO
CREATE UNIQUE INDEX ak_insurance_ResourceId ON [dbo].[insurance] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_insurance_coverage_ResourceId')
	DROP INDEX ak_insurance_coverage_ResourceId ON [dbo].[insurance_coverage]
GO
CREATE UNIQUE INDEX ak_insurance_coverage_ResourceId ON [dbo].[insurance_coverage] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_insurance_coverage_editable_ResourceId')
	DROP INDEX ak_insurance_coverage_editable_ResourceId ON [dbo].[insurance_coverage_editable]
GO
CREATE UNIQUE INDEX ak_insurance_coverage_editable_ResourceId ON [dbo].[insurance_coverage_editable] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_invoice_ResourceId')
	DROP INDEX ak_invoice_ResourceId ON [dbo].[invoice]
GO
CREATE UNIQUE INDEX ak_invoice_ResourceId ON [dbo].[invoice] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_NightlyCalculation_ResourceId')
	DROP INDEX ak_NightlyCalculation_ResourceId ON [dbo].[NightlyCalculation]
GO
CREATE UNIQUE INDEX ak_NightlyCalculation_ResourceId ON [dbo].[NightlyCalculation] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_payroll_archive_ResourceId')
	DROP INDEX ak_payroll_archive_ResourceId ON [dbo].[payroll_archive]
GO
CREATE UNIQUE INDEX ak_payroll_archive_ResourceId ON [dbo].[payroll_archive] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_tax_year_1095c_approval_ResourceId')
	DROP INDEX ak_tax_year_1095c_approval_ResourceId ON [dbo].[tax_year_1095c_approval]
GO
CREATE UNIQUE INDEX ak_tax_year_1095c_approval_ResourceId ON [dbo].[tax_year_1095c_approval] (ResourceId)
GO
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_tax_year_approval_ResourceId')
	DROP INDEX ak_tax_year_approval_ResourceId ON [dbo].[tax_year_approval]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UploadedFileInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UploadedFileInfo]
(
	[UploadedFileInfoId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployerId] [int] NOT NULL,
	[UploadedByUser] [nvarchar](50) NOT NULL,
	[UploadTime] [datetime] NOT NULL,
	[UploadSourceDescription] [nvarchar](50) NOT NULL,
	[UploadTypeDescription] [nvarchar](50) NOT NULL,
	[FileTypeDescription] [nvarchar](50) NOT NULL,
	[FileName] [nvarchar](256) NOT NULL,
	[Processed] [bit] NOT NULL,
	[FailedProcessing] [bit] NOT NULL,
	[ArchiveFileInfoId] [bigint] NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL 
	CONSTRAINT [DF_UploadedFileInfo_resourceId] DEFAULT (newid()),
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,

	CONSTRAINT [PK_UploadedFileInfo] PRIMARY KEY NONCLUSTERED 
	(
	[UploadedFileInfoId] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
	ON [PRIMARY]
) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[UploadedFileInfo]  WITH CHECK ADD  CONSTRAINT [FK_UploadedFileInfo_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[UploadedFileInfo]  WITH CHECK ADD  CONSTRAINT [FK_UploadedFileInfo_Employer] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[employer] ([employer_id])
GO

ALTER TABLE [dbo].[UploadedFileInfo]  WITH CHECK ADD  CONSTRAINT [FK_UploadedFileInfo_Archive] FOREIGN KEY([ArchiveFileInfoId])
REFERENCES [dbo].[ArchiveFileInfo] ([ArchiveFileInfoId])
GO

ALTER TABLE [dbo].[UploadedFileInfo] CHECK CONSTRAINT [FK_UploadedFileInfo_EntityStatus]
GO

ALTER TABLE [dbo].[UploadedFileInfo] CHECK CONSTRAINT [FK_UploadedFileInfo_Employer]
GO

ALTER TABLE [dbo].[UploadedFileInfo] CHECK CONSTRAINT [FK_UploadedFileInfo_Archive]
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_UploadedFileInfo_ResourceId')
	DROP INDEX ak_UploadedFileInfo_ResourceId ON [dbo].[UploadedFileInfo]
GO
CREATE UNIQUE INDEX ak_UploadedFileInfo_ResourceId ON [dbo].[UploadedFileInfo] (ResourceId)
GO

IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo] AS SET NOCOUNT ON;')
GO
SET ANSI_PADDING OFF
GO
----------------UploadedFileInfo Procs----------------------------------------
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select a Single Row of 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo]
      @UploadedFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo_ForEmployer' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo_ForEmployer] AS SET NOCOUNT ON;')
GO
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo for an Employer
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_ForEmployer]
      @employerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [EmployerId] = @employerId AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo_Unprocessed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo_Unprocessed] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo that are unprocessed
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_Unprocessed]
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [Processed] = 0  AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_EmployerIds_UploadedFileInfo_Unprocessed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_EmployerIds_UploadedFileInfo_Unprocessed] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/11/2016
-- Description: Select all The Employer Ids Rows of UploadedFileInfo that are unprocessed
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_EmployerIds_UploadedFileInfo_Unprocessed]
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT DISTINCT EmployerId FROM [dbo].[UploadedFileInfo] 
      WHERE [Processed] = 0 AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo_Failed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo_Failed] AS SET NOCOUNT ON;')

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/10/2016
-- Description: Select all Rows of UploadedFileInfo that Failed Processing
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_Failed]
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [FailedProcessing] = 1 AND [Processed] = 0;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_UploadedFileInfo_UnprocessedForEmployer' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_UploadedFileInfo_UnprocessedForEmployer] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo for an Employer that are unprocessed
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_UnprocessedForEmployer]
		@employerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[UploadedFileInfo] 
      WHERE [Processed] = 0 AND [EmployerId] = @employerId AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_UploadedFileInfo' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_UploadedFileInfo] AS SET NOCOUNT ON;')
--Insert
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Long, Vu; Ryan, Mccully
-- Create date: 9/19/2016
-- Description:	Insert a new Import for coversion into the table
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_UploadedFileInfo]
	@fileName varchar(256),
	@employerId bigint,
	@uploadTime datetime,
	@uploadSourceDescription nvarchar(50),
	@uploadTypeDescription nvarchar(50),
	@fileTypeDescription nvarchar(50),
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[UploadedFileInfo]
	([EntityStatusId], [Processed], [FailedProcessing], [EmployerId], [UploadTime], [UploadSourceDescription], [UploadTypeDescription], [FileTypeDescription], 
	[FileName], [UploadedByUser], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
	VALUES (1, 0, 0, @employerId, @uploadTime, @uploadSourceDescription, @uploadTypeDescription, @fileTypeDescription,
	@fileName, @CreatedBy, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

	SELECT @insertedID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_UploadedFileInfo_Processed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_UploadedFileInfo_Processed] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Sets the Specific File as Processed 
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_UploadedFileInfo_Processed]
       @UploadedFileInfoId int,	   
	   @modifiedBy nvarchar(50),
	   @processed bit,
	   @failed bit
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[UploadedFileInfo] 
		   set [Processed] = @processed, 
		   [FailedProcessing] = @failed,
		   [ModifiedBy] = @modifiedBy, 
		   [ModifiedDate] = GETDATE()
			where [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_UploadedFileInfo_Archived' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_UploadedFileInfo_Archived] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/3/2016
-- Description: Links to a specific File Archive
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_UploadedFileInfo_Archived]
       @UploadedFileInfoId int,	   
	   @modifiedBy nvarchar(50),
	   @archiveFileInfoId bigint
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[UploadedFileInfo] set [ArchiveFileInfoId] = @archiveFileInfoId, [ModifiedBy] = @modifiedBy, [ModifiedDate] = GETDATE()
			where [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_UploadedFileInfo_EntityStatus' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_UploadedFileInfo_EntityStatus] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Set the entity Status  
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_UploadedFileInfo_EntityStatus]
       @UploadedFileInfoId int,	   
	   @modifiedBy nvarchar(50),
	   @EntityStatus int
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[UploadedFileInfo] set [EntityStatusId] = @EntityStatus, [ModifiedBy] = @modifiedBy, [ModifiedDate] = GETDATE()
			where [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
--Grant Execute 
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo_ForEmployer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo_Unprocessed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployerIds_UploadedFileInfo_Unprocessed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo_Failed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_UploadedFileInfo_UnprocessedForEmployer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_UploadedFileInfo] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_UploadedFileInfo_Processed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_UploadedFileInfo_Archived] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_UploadedFileInfo_EntityStatus] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[UploadedFileInfo] TO [aca-user] AS [dbo]
GO


-----------------------------------------------------------------------------------------------------------------
--[StagingImport] Changes
-----------------------------------------------------------------------------------------------------------------

--GO
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--SET ANSI_PADDING ON
--GO
--DROP TABLE dbo.stagingImport
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StagingImport]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StagingImport](
	[StagingImportId] [bigint] IDENTITY(1,1) NOT NULL,
	[Original] [xml] NOT NULL,
	[UploadedFileInfoId] [bigint] NOT NULL,
	[Modify] [xml] NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StagingImport_resourceId] DEFAULT (newid()),
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	
 CONSTRAINT [PK_stagingImport] PRIMARY KEY NONCLUSTERED 
(
	[StagingImportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[StagingImport]  WITH CHECK ADD  CONSTRAINT [FK_StagingImport_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[StagingImport]  WITH CHECK ADD  CONSTRAINT [FK_StagingImport_UploadedFileInfo] FOREIGN KEY([UploadedFileInfoId])
REFERENCES [dbo].[UploadedFileInfo] ([UploadedFileInfoId])
GO

ALTER TABLE [dbo].[StagingImport] CHECK CONSTRAINT [FK_StagingImport_UploadedFileInfo]
GO

ALTER TABLE [dbo].[StagingImport] CHECK CONSTRAINT [FK_StagingImport_EntityStatus]
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'ak_stagingImport_ResourceId')
	DROP INDEX ak_stagingImport_ResourceId ON [dbo].[stagingImport]
GO
CREATE UNIQUE INDEX ak_stagingImport_ResourceId ON [dbo].[stagingImport] (ResourceId)
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_StagingImport' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_StagingImport] AS SET NOCOUNT ON;')
GO
SET ANSI_PADDING OFF
GO


------------------------StagingImport Procs---------------------------------


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select a Single Row of Import Staging
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_StagingImport]
      @StagingImportId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[StagingImport] 
      WHERE [StagingImportId] = @StagingImportId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_ActiveStagingImport' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_ActiveStagingImport] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select All Active Imports 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ActiveStagingImport]

AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[StagingImport] 
      WHERE [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_ActiveStagingImport_ForUpload' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_ActiveStagingImport_ForUpload] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/11/2016
-- Description: Select All Active Imports that are tied to this file
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ActiveStagingImport_ForUpload]
	@uploadedFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[StagingImport] 
      WHERE [EntityStatusId] = 1 AND [UploadedFileInfoId] = @uploadedFileInfoId;
END TRy
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_StagingImport_ForUpload' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_StagingImport_ForUpload] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/12/2016
-- Description: Select All Active Imports that are tied to this file
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_StagingImport_ForUpload]
	@uploadedFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[StagingImport] 
      WHERE [UploadedFileInfoId] = @uploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_StagingImport' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_StagingImport] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Log, Vu; Ryan, Mccully
-- Create date: 9/19/2016
-- Description:	Insert a new Import for coversion into the table
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_StagingImport]
	@xmlOriginal XML,
	@uploadedFileInfoId int,
	@xmlModify XML,
	@CreatedBy nvarchar(50),
    @CreatedDate datetime2(7),
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[StagingImport]
	([EntityStatusId], [Original], [UploadedFileInfoId], [Modify], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
	VALUES (1, @xmlOriginal, @uploadedFileInfoId, @xmlModify, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

	SELECT @insertedID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_StagingImport_Reprocessed' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_StagingImport_Reprocessed] AS SET NOCOUNT ON;')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Log, Vu; Ryan, Mccully
-- Create date: 9/19/2016
-- Description:	Update an Existing Staging Import, deactivating the old row and adding a new row for this transformation.
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_StagingImport_Reprocessed]
	@stagingId int,	
	@xmlModify XML,
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @xmlOriginal XML;
	DECLARE @uploadedFileInfoId varchar(256);
	
	SELECT @xmlOriginal = [Modify], @uploadedFileInfoId = [UploadedFileInfoId]
	from [dbo].[StagingImport] WHERE StagingImportId = @stagingId;
	
	
	INSERT INTO [dbo].[StagingImport]
	([EntityStatusId], [Original], [UploadedFileInfoId], [Modify], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
	VALUES (1, @xmlOriginal, @uploadedFileInfoId, @xmlModify, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

	SELECT @insertedID = SCOPE_IDENTITY();

	UPDATE [dbo].[StagingImport] SET [EntityStatusId] = 2, [ModifiedBy] = @CreatedBy, [ModifiedDate] = GETDATE()
	where [StagingImportId] = @stagingId;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_StagingImportModified' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_StagingImportModified] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: We need to beable to set the finished XML once we have manipulated it.  
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_StagingImportModified]
       @stagingId int,
	   @modifiedBy nvarchar(50),
	   @xmlModify XML
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[StagingImport] set [Modify] = @xmlModify, [ModifiedBy] = @modifiedBy, [ModifiedDate] =  GETDATE()
			where [StagingImportId] = @stagingId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_StagingImportEntityStatus' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_StagingImportEntityStatus] AS SET NOCOUNT ON;')
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Set the entity Status  
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_StagingImportEntityStatus]
       @stagingId int,	   
	   @modifiedBy nvarchar(50),
	   @EntityStatus int
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[StagingImport] set [EntityStatusId] = @EntityStatus, [ModifiedBy] = @modifiedBy, [ModifiedDate] = GETDATE()
			where [StagingImportId] = @stagingId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
--Grant Execute 
GO
GRANT EXECUTE ON [dbo].[SELECT_StagingImport] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_ActiveStagingImport] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_ActiveStagingImport_ForUpload] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_StagingImport_ForUpload] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[INSERT_StagingImport] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_StagingImport_Reprocessed] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_StagingImportModified] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_StagingImportEntityStatus] TO [aca-user] AS [dbo]
GO
GRANT SELECT ON [dbo].[StagingImport] TO [aca-user] AS [dbo]
GO




IF NOT EXISTS (SELECT * FROM sys.types WHERE  is_table_type = 1 AND name = N'Bulk_import_employee')
BEGIN
-----------------------------------------------------------------------------------------------------------------
--Bulk Import Changes
-----------------------------------------------------------------------------------------------------------------
create type Bulk_import_employee as table
(
	rowid int,
	employeeTypeID int,
	hr_status_id int,
	hr_status_ext_id varchar(50),
	hr_description varchar(50),
	employerID int,
	planYearID int,
	fName varchar(50),
	mName varchar(50),
	lName varchar(50),
	[address] varchar(50),
	city varchar(50),
	stateID int,
	stateAbb varchar(2),
	zip varchar(5),
	hDate datetime,
	i_hDate varchar(8),
	cDate datetime,
	i_cDate varchar(8),
	ssn varchar(50),
	ext_employee_id varchar(50),
	tDate datetime,
	i_tDate varchar(8),
	dob datetime,
	i_dob varchar(8),
	impEnd datetime,
	batchid int,
	aca_status_id int,
	class_id int
);	
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_INSERT_import_employee' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_INSERT_import_employee] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/19/2016
-- Description:	Bulk Import Data to speed up uploading based on <Travis Wells> INSERT_import_employee
-- =============================================
ALTER PROCEDURE [dbo].[BULK_INSERT_import_employee]
	@batchID int,
	@employees Bulk_import_employee readonly
AS

BEGIN TRY
	INSERT INTO [import_employee](
	hr_status_ext_id,
	hr_description,
	employerID,
	fName, 
	mName,
	lName,
	[address],
	city,
	stateAbb,
	zip,
	i_hDate,
	i_cDate,
	ssn,
	ext_employee_id,
	i_tDate,
	i_dob,
	batchid) 	
	SELECT hr_status_ext_id, hr_description, employerID, fName, mName, lName,
	[address], city, stateAbb, zip, i_hDate, i_cDate, ssn, ext_employee_id, 
	i_tDate, i_dob, @batchID from @employees; 

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_INSERT_FULL_import_employee' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_INSERT_FULL_import_employee] AS SET NOCOUNT ON;')

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/6/2016
-- Description:	Bulk Import the full Data to speed up uploading based on <Travis Wells> INSERT_import_employee
-- =============================================
ALTER PROCEDURE [dbo].[BULK_INSERT_FULL_import_employee]
	@batchID int,
	@employees Bulk_import_employee readonly
AS

BEGIN TRY
	INSERT INTO [import_employee](
		employeeTypeID,	hr_status_id,	hr_status_ext_id,	hr_description,
		employerID,	planYearID,	fName,	mName,	lName,	[address],
		city,	stateID,	stateAbb,	zip,	hDate,	i_hDate,
		cDate,	i_cDate,	ssn,	ext_employee_id,	tDate,
		i_tDate,	dob,	i_dob,	impEnd,	aca_status_id,
		class_id, batchid) 	
	SELECT 
		employeeTypeID,	hr_status_id,	hr_status_ext_id,	hr_description,
		employerID,	planYearID,	fName,	mName,	lName,	[address],
		city,	stateID,	stateAbb,	zip,	hDate,	i_hDate,
		cDate,	i_cDate,	ssn,	ext_employee_id,	tDate,
		i_tDate,	dob,	i_dob,	impEnd,	aca_status_id,
		class_id, @batchID 
	from @employees; 
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_UPDATE_import_employee' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_UPDATE_import_employee] AS SET NOCOUNT ON;')



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/20/2016
-- Description:	Bulk UPDATE Data to speed up uploading based on <Travis Wells> UPDATE_import_employee
-- =============================================
ALTER PROCEDURE [dbo].[BULK_UPDATE_import_employee]
	@employees Bulk_import_employee readonly
AS
BEGIN TRY
	UPDATE import_employee 
	SET
		employeeTypeID=a.employeeTypeID,
		HR_status_id = a.HR_status_id,
		hr_status_ext_id = a.hr_status_ext_id,
		hr_description = a.hr_description,
		planYearID = a.planYearID,
		stateID = a.stateID,  
		hDate=a.hDate,
		i_hDate=a.i_hDate,
		cDate=a.cDate,
		tDate=a.tDate,
		i_tDate=a.i_tDate,
		dob=a.dob,
		i_dob=a.i_dob,
		impEnd=a.impEnd, 
		class_id=a.class_id,
		aca_status_id=a.aca_status_id
	FROM @employees a
	JOIN import_employee b ON b.rowid=a.rowid;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH








----------------------------------------------------------------------







GO
IF NOT EXISTS (SELECT * FROM sys.types WHERE  is_table_type = 1 AND name = N'Bulk_import_payroll')
BEGIN
create type Bulk_import_payroll as table
(
	rowid int,
	employerid int,
	batchid int,
	employee_id int,
	fname varchar(50),
	mname varchar(50),
	lname varchar(50),
	i_hours varchar(50),
	[hours] decimal(18, 4),
	i_sdate varchar(8),
	sdate datetime,
	i_edate varchar(50),
	edate datetime,
	ssn varchar(50),
	gp_description varchar(50),
	gp_ext_id varchar(50),
	gp_id int,
	i_cdate varchar(8),
	cdate datetime,
	modBy varchar(50),
	modOn datetime,
	ext_employee_id varchar(30)
);
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_INSERT_import_payroll' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_INSERT_import_payroll] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/19/2016
-- Description:	Bulk Import Data to speed up uploading based on <Travis Wells> INSERT_import_payroll
-- =============================================
ALTER PROCEDURE [dbo].[BULK_INSERT_import_payroll]
	@payroll Bulk_import_payroll readonly
AS

BEGIN TRY
	INSERT INTO [import_payroll](
	employerid,
	batchid,
	fname, 
	mname,
	lname,
	i_hours,
	i_sdate,
	i_edate,
	ssn,
	gp_description,
	gp_ext_id,
	i_cdate,
	modBy,
	modOn, 
	ext_employee_id) 	
	SELECT employerid,
	batchid, fname, mname, lname, i_hours, i_sdate, i_edate, ssn, gp_description, gp_ext_id,
	i_cdate, modBy, modOn, ext_employee_id
	from @payroll;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_UPDATE_import_payroll' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_UPDATE_import_payroll] AS SET NOCOUNT ON;')

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/20/2016
-- Description:	Bulk Import Data to speed up uploading based on <Travis Wells> UPDATE_import_payroll
-- =============================================
ALTER PROCEDURE [dbo].[BULK_UPDATE_import_payroll]
	@payroll Bulk_import_payroll readonly
AS

BEGIN TRY
	UPDATE import_payroll
	SET
		employee_id= a.employee_id,
		gp_id = a.gp_id,
		[hours] = a.[hours],
		sdate = a.sdate,
		edate = a.edate,
		cdate = a.cdate, 
		modBy = a.modBy,
		modOn = a.modOn	
	from @payroll a	
	JOIN import_payroll b ON b.rowid=a.rowid;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'BULK_TRANSFER_import_new_payroll' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[BULK_TRANSFER_import_new_payroll] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 9/20/2016
-- Description:	Bulk Import Data to speed up uploading based on <Travis Wells> TRANSFER_import_new_payroll
-- =============================================
ALTER PROCEDURE [dbo].[BULK_TRANSFER_import_new_payroll] 
	@history varchar(max),
	@payroll  Bulk_import_payroll readonly
AS

BEGIN TRY
	
	DECLARE @rowid int;
	DECLARE @employerid int;
	DECLARE @batchid int;
	DECLARE @employee_id int;
	DECLARE @hours decimal(18, 4);
	DECLARE @sdate datetime;
	DECLARE @edate datetime;
	DECLARE @gp_id int;
	DECLARE @cdate datetime;
	DECLARE @modBy varchar(50);
	DECLARE @modOn datetime;
	
	DECLARE MY_CURSOR CURSOR 
		LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
		SELECT rowid, employerid, batchid, employee_id, gp_id, [hours], sdate, edate, cdate, modBy, modOn
		FROM @payroll
	
	OPEN MY_CURSOR;
	FETCH NEXT FROM MY_CURSOR INTO @rowId, @employerID, @batchID, @employee_ID, @gp_id, @hours, @sdate, @edate, @cdate, @modBy, @modOn;
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		--loop through ever row passed in and try it individually. 
		Exec [TRANSFER_import_new_payroll] @rowId, @employerID, @batchID, @employee_ID, @gp_id, @hours, @sdate, @edate, @cdate, @modBy, @modOn, @history;
		FETCH NEXT FROM MY_CURSOR INTO @rowId, @employerID, @batchID, @employee_ID, @gp_id, @hours, @sdate, @edate, @cdate, @modBy, @modOn;
	END
	CLOSE MY_CURSOR;
	DEALLOCATE MY_CURSOR;
	
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
-----------------------------
GO
GRANT EXECUTE ON [dbo].[BULK_INSERT_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_INSERT_FULL_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_UPDATE_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_import_employee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_INSERT_import_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_UPDATE_import_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_TRANSFER_import_new_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_import_payroll] TO [aca-user] AS [dbo]

----------------------------------------------------------------------------------
--alter existing procedures
----------------------------------------------------------------------------------
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_FailNightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_FailNightlyCalculation] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UPDATE_FailNightlyCalculation]
	@employerId as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[NightlyCalculation] SET processFail = 1
	WHERE EmployerId = @employerId AND processStatus = 0 AND processFail = 0;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_NightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_NightlyCalculation] AS SET NOCOUNT ON;')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UPDATE_NightlyCalculation]
	@employerId as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[NightlyCalculation] SET processStatus = 1
	WHERE EmployerId = @employerId AND processStatus = 0 AND processFail = 0;
	
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'TRANSFER_import_new_payroll' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[TRANSFER_import_new_payroll] AS SET NOCOUNT ON;')
GO
/****** Object:  StoredProcedure [dbo].[TRANSFER_import_new_payroll]    Script Date: 9/21/2016 10:26:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/17/2014>
-- Description:	<This stored procedure is meant transfer a new payroll record from the import table over to the current payroll table.
-- Changes:
--			5-27-2015 TLW
--				- Added the history parameter to record the initial data. 
-- =============================================
ALTER PROCEDURE [dbo].[TRANSFER_import_new_payroll] 
	@rowID int,
	@employerID int,
	@batchID int, 
	@employeeID int,
	@gpID int, 
	@hours numeric(10,4),
	@sdate datetime, 
	@edate datetime, 
	@cdate datetime, 
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(2000)
AS

BEGIN
	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	
	BEGIN TRANSACTION
		BEGIN TRY
			-- Step 1: Create a new EMPLOYEE based on the registration information.
			--			- Return the new Employee_ID
				EXEC INSERT_new_payroll 
					@employerID,
					@batchID, 
					@employeeID,
					@gpID, 
					@hours,
					@sdate, 
					@edate, 
					@cdate, 
					@modBy,
					@modOn,
					@history
	
			-- Step 2: DELETE the record from the import_employee table. 
				EXEC DELETE_payroll_import_row
					@rowID
			COMMIT
		END TRY
		BEGIN CATCH
			--this will happen if a constraint is missing, or something.
			ROLLBACK TRANSACTION
			EXEC INSERT_ErrorLogging
		END CATCH
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_employee_plan_year' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_employee_plan_year] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[UPDATE_employee_plan_year]
	@employerID int,
	@employeeTypeID int,
	@adminPlanYearID int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY

		UPDATE employee
		SET
			plan_year_id=@adminPlanYearID,
			limbo_plan_year_id=NULL,
			modOn=@modOn,
			modBy=@modBy
		WHERE
			employer_id=@employerID AND
			employee_type_id=@employeeTypeID AND
			limbo_plan_year_id=@adminPlanYearID;
		
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		exec dbo.INSERT_ErrorLogging
	END CATCH
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_new_registration' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_new_registration] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[INSERT_new_registration]
       @empTypeID int,
       @name varchar(50),
       @ein varchar(50),
       @add varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @userfname varchar(50),
       @userlname varchar(50),
       @useremail varchar(50),
       @userphone varchar(15),
       @username varchar(50),
       @password varchar(50),
       @active bit,
       @power bit,
       @billing bit,
       @b_add varchar(50),
       @b_city varchar(50),
       @b_stateID int,
       @b_zip varchar(15),
       @p_desc1 varchar(50),
       @p_start1 datetime,
       @p_desc2 varchar(50),
       @p_start2 datetime,
       @b_fname varchar(50),
       @b_lname varchar(50),
       @b_email varchar(50),
       @b_phone varchar(15), 
       @b_username varchar(50),
       @b_password varchar(50),
       @b_active bit, 
       @b_power bit,
       @b_billing bit,
       @dbaName varchar(100),
       @employerID int OUTPUT
AS

BEGIN
       DECLARE @userid int;
       DECLARE @planyearid int;
       DECLARE @lastModBy varchar(50);
       DECLARE @lastMod datetime;
       DECLARE @irsContact bit;
       DECLARE @irsContact2 bit;

       SET @irsContact = 1;
       SET @irsContact2 = 0;
       SET @lastModBy = 'Registration';
       SET @lastMod = GETDATE();

       /*************************************************************
       ******* Create a transaction that must fully complete ********
       **************************************************************/
       BEGIN TRANSACTION
             BEGIN TRY
                    DECLARE @default varchar(50);
                    SET @default = 'All Employees';

                    -- Step 1: Create a new EMPLOYER based on the registration information.
                    --                  - Return the new Employer_ID
                    EXEC INSERT_new_employer 
                           @name, 
                           @add, 
                           @city,
                           @stateID, 
                           @zip, 
                           '../images/logos/EBC_logo.gif',  
                           @b_add, 
                           @b_city, 
                           @b_stateID, 
                           @b_zip, 
                           @empTypeID,
                           @ein,
                           @dbaName,
                           @employerID OUTPUT;

                    -- Step 2: Create a new EMPLOYEE_TYPE for this specific employer.
                    EXEC INSERT_new_employee_type 
                           @employerID, 
                           @default;

                    -- Step 3: Create a new USER for this specific district.
                    EXEC INSERT_new_user 
                           @userfname, 
                           @userlname, 
                           @useremail, 
                           @userphone, 
                           @username, 
                           @password,
                           @employerID, 
                           @active, 
                           @power,
                           @lastModBy,
                           @lastMod,
                           @billing,
                           @irsContact,
                           @userid OUTPUT

                    IF (@b_username IS NOT NULL)
                           BEGIN
                                 EXEC INSERT_new_user 
                                        @b_fname, 
                                        @b_lname, 
                                        @b_email, 
                                        @b_phone, 
                                        @b_username, 
                                        @b_password,
                                        @employerID, 
                                        @b_active, 
                                        @b_power,
                                        @lastModBy,
                                        @lastMod,
                                        @b_billing,
                                        @irsContact2,
                                        @userid OUTPUT
                           END
       

                    -- Step 4: Create a new PLAN YEAR for this specific district.
                    DECLARE @p_end1 datetime;
                    DECLARE @history varchar(100);
                    DECLARE @p_notes varchar(100);
                    SET @p_end1 = DATEADD(dd, 364, @p_start1);
                    SET @p_notes = '';
                    SET @history = 'Plan created on: ' + CONVERT(varchar(19), GETDATE());
                    EXEC INSERT_new_plan_year 
                           @employerID, 
                           @p_desc1, 
                           @p_start1, 
                           @p_end1, 
                           @p_notes,
                           @history,
                           @lastMod,
                           @lastModBy,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           @planyearid OUTPUT; 

                    -- Step 5: Create a second PLAN YEAR for this specific district if needed.
                    IF (@p_start2 IS NOT NULL OR @p_start2 = '')
                           BEGIN
                                 DECLARE @p_end2 datetime;
                                 SET @p_end2 = DATEADD(dd, 364, @p_start2);
                                 SET @p_notes = '';
                                 SET @history = 'Plan created on: ' + CONVERT(varchar(19), GETDATE());
                                 EXEC INSERT_new_plan_year 
                                        @employerID, 
                                        @p_desc2, 
                                        @p_start2, 
                                        @p_end2, 
                                        @p_notes,
                                        @history,
                                        @lastMod,
                                        @lastModBy, 
                                        @planyearid OUTPUT; 
                           END

                    COMMIT
             END TRY
             BEGIN CATCH
                    --If something fails return 0.
                    SET @employerID = 0;
                    ROLLBACK TRANSACTION
                    exec dbo.INSERT_ErrorLogging
             END CATCH

END
GO