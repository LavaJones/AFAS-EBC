USE [aca]
ALTER TABLE dbo.employee DROP CONSTRAINT fk_employee_stateID
ALTER TABLE dbo.employee ALTER COLUMN state_id integer NULL
ALTER TABLE dbo.employer ADD DBAName varchar(100) NULL
GO
ALTER PROCEDURE [dbo].[INSERT_new_employer]
	@name varchar(50),
	@add varchar(50),
	@city varchar(50),
	@stateID int,
	@zip varchar(15),
	@logo varchar(50),
	@b_add varchar(50),
	@b_city varchar(50),
	@b_stateID int,
	@b_zip varchar(15),
	@empTypeID int,
	@ein varchar(50),
	@dbaName varchar(100),
	@empid int OUTPUT
AS

BEGIN TRY
	INSERT INTO [employer](
		name,
		[address],
		city,
		state_id,
		zip,
		img_logo,
		bill_address,
		bill_city,
		bill_state,
		bill_zip,
		employer_type_id, 
		ein,
		DBAName)
	VALUES(
		@name,
		@add,
		@city,
		@stateID,
		@zip,
		@logo,
		@b_add,
		@b_city,
		@b_stateID,
		@b_zip,
		@empTypeID,
		@ein,
		@dbaName)

SELECT @empid = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_employer]
       @employerID int,
       @name varchar(50),
       @address varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @logo varchar(50),
       @ein varchar(50),
       @employerTypeId int,
	   @dbaName varchar(100)
AS
BEGIN TRY
       UPDATE employer
       SET
             name = @name, 
             [address] = @address,
             city = @city,
             state_id = @stateID, 
             zip = @zip,  
             img_logo = @logo,
             ein = @ein,
             employer_type_id = @employerTypeId,
			 DBAName = @dbaName
       WHERE
             employer_id = @employerID;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[DELETE_payroll_import_row]
	@rowID int
AS
BEGIN TRY
	DELETE FROM import_payroll
	WHERE rowid=@rowID;

END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[DELETE_payroll_import]
	@batchID int, 
	@modBy varchar(50), 
	@modOn datetime
AS
BEGIN
	BEGIN TRANSACTION
		BEGIN TRY
			--Archive any payroll records that were in the ACT system related to the batch being removed.  
			INSERT INTO payroll_archive (row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, modBy, modOn, history)
			SELECT row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, @modBy, @modOn, history
			FROM payroll
			WHERE batch_id=@batchID;

			--Remove the actual payroll records related to the batch id. 
			DELETE FROM payroll
			WHERE batch_id=@batchID;

			-- Remove the payroll records with batch id that are stuck in the payroll import table. 
			-- Note we are not archiving these records as they were never used to average any employees hours. 
			DELETE FROM import_payroll
			WHERE batchid=@batchID;

			UPDATE batch 
			SET
				delBy=@modBy, 
				delOn=@modOn
			WHERE
				batch_id=@batchID;
		
			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			exec dbo.INSERT_ErrorLogging
		END CATCH
END
GO
alter Table [aca].[dbo].[plan_year] add 
	[default_meas_start] [datetime] NULL,
	[default_meas_end] [datetime] NULL,
	[default_admin_start] [datetime] NULL,
	[default_admin_end] [datetime] NULL,
	[default_open_start] [datetime] NULL,
	[default_open_end] [datetime] NULL,
	[default_stability_start] [datetime] NULL,
	[default_stability_end] [datetime] NULL;

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/19/2014>
-- Description:	<This stored procedure is meant to create a new Pay Roll record.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_new_plan_year]
	@employerID int,
	@description varchar(50),
	@startDate datetime,
	@endDate datetime,
	@notes varchar(max),
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@default_Meas_Start datetime,
	@default_Meas_End datetime,
	@default_Admin_Start datetime,
	@default_Admin_End datetime,
	@default_Open_Start datetime,
	@default_Open_End datetime,
	@default_Stability_Start datetime,
	@default_Stability_End datetime,

	@planyearid int OUTPUT
AS

BEGIN
	INSERT INTO [plan_year](
		employer_id,
		[description],
		startDate,
		endDate,
		notes,
		history,
		modOn,
		modBy, 
		default_Meas_Start,
		default_Meas_End,
		default_Admin_Start,
		default_Admin_End,
		default_Open_Start,
		default_Open_End,
		default_Stability_Start,
		default_Stability_End)
	VALUES(
		@employerID,
		@description,
		@startDate,
		@endDate,
		@notes,
		@history,
		@modOn,
		@modBy,
		@default_Meas_Start,
		@default_Meas_End,
		@default_Admin_Start,
		@default_Admin_End,
		@default_Open_Start,
		@default_Open_End,
		@default_Stability_Start,
		@default_Stability_End)

SELECT @planyearid = SCOPE_IDENTITY();
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to update the plan year.>
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_plan_year]
	@planyearID int,
	@description varchar(50),
	@sDate datetime,
	@eDate datetime,
	@notes varchar(max),
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@default_Meas_Start datetime,
	@default_Meas_End datetime,
	@default_Admin_Start datetime,
	@default_Admin_End datetime,
	@default_Open_Start datetime,
	@default_Open_End datetime,
	@default_Stability_Start datetime,
	@default_Stability_End datetime
AS
BEGIN
	UPDATE plan_year
	SET
		description=@description,
		startDate = @sDate,
		endDate = @eDate,
		notes = @notes,
		history = @history,
		modOn = @modOn,
		modBy = @modBy,
		default_Meas_Start=@default_Meas_Start,
		default_Meas_End=@default_Meas_End,
		default_Admin_Start=@default_Admin_Start,
		default_Admin_End=@default_Admin_End,
		default_Open_Start=@default_Open_Start,
		default_Open_End=@default_Open_End,
		default_Stability_Start=@default_Stability_Start,
		default_Stability_End=@default_Stability_End
	WHERE
		plan_year_id=@planyearID;

END
GO

----------------------------------------------------
--Create Table ArchiveFileInfo
----------------------------------------------------
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ArchiveFileInfo](
	[ArchiveFileInfoId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployerId] [bigint] NOT NULL,
	[EmployerGuid] [uniqueidentifier] NOT NULL,
	[ArchivedTime] [datetime] NOT NULL,
	[FileName] [nvarchar](256) NOT NULL,
	[SourceFilePath] [nvarchar](256) NOT NULL,
	[ArchiveFilePath] [nvarchar](256) NOT NULL,
	[ArchiveReason] [nvarchar](256) NULL,
	--Standard Items
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ArchiveFileInfo] PRIMARY KEY NONCLUSTERED 
(
	[ArchiveFileInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ArchiveFileInfo] ADD  CONSTRAINT [DF_ArchiveFileInfo_resourceId]  DEFAULT (newid()) FOR [ResourceId]
GO

ALTER TABLE [dbo].[ArchiveFileInfo]  WITH CHECK ADD  CONSTRAINT [FK_ArchiveFileInfo_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[ArchiveFileInfo] CHECK CONSTRAINT [FK_ArchiveFileInfo_EntityStatus]
GO

GRANT SELECT ON [dbo].[ArchiveFileInfo] TO [aca-user] AS [dbo]
---------------------------------------------------
--Create Stored Procs For ArchiveFileInfo
---------------------------------------------------
--Select
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/23/2016
-- Description: Select a Single Row of 
-- =============================================
Create PROCEDURE [dbo].[SELECT_ArchiveFileInfo]
      @ArchiveFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[ArchiveFileInfo] 
      WHERE [ArchiveFileInfoId] = @ArchiveFileInfoId;
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
--Select for employer
GO
GRANT EXECUTE ON [dbo].[SELECT_ArchiveFileInfo] TO [aca-user] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/23/2016
-- Description: Select a Single Row of 
-- =============================================
Create PROCEDURE [dbo].[SELECT_ArchiveFileInfo_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[ArchiveFileInfo] 
      WHERE [EmployerId] = @EmployerId;
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
--Insert
END CATCH
GO
GRANT EXECUTE ON [dbo].[SELECT_ArchiveFileInfo_ForEmployer] TO [aca-user] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 9/23/2016
-- Description:	Insert a new Import for coversion into the table
-- =============================================
Create PROCEDURE [dbo].[INSERT_ArchiveFileInfo]
	@EmployerGuid uniqueidentifier,
	@FileName nvarchar(256),
	@SourceFilePath nvarchar(256),
	@ArchiveFilePath nvarchar(256),
	@ArchiveReason nvarchar(256),
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @EmployerId bigint;
	select @EmployerId = [employer_id] from employer where [ResourceId] = @EmployerGuid;

	INSERT INTO [dbo].[ArchiveFileInfo](
		[EmployerId], [EmployerGuid], [ArchivedTime], [FileName], [SourceFilePath], [ArchiveFilePath], [ArchiveReason],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate])
	VALUES (@EmployerId, @EmployerGuid, GETDATE(), @FileName, @SourceFilePath, @ArchiveFilePath, @ArchiveReason,
		1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE());

	SELECT @insertedID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
GRANT EXECUTE ON [dbo].[INSERT_ArchiveFileInfo] TO [aca-user] AS [dbo]
GO