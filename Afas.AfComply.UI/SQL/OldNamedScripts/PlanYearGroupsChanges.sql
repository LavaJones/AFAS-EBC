USE [aca];
GO

-----create the new table-----

GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlanYearGroup](
	[PlanYearGroupId] [bigint] IDENTITY(1,1) NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[Employer_id] [int] NOT NULL,
 CONSTRAINT [PK_PlanYearGroup_PlanYearGroupId] PRIMARY KEY NONCLUSTERED 
(
	[PlanYearGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PlanYearGroup]  WITH CHECK ADD  CONSTRAINT [FK_PlanYearGroup_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[PlanYearGroup] CHECK CONSTRAINT [FK_PlanYearGroup_EntityStatus]
GO

ALTER TABLE [dbo].[PlanYearGroup]  WITH NOCHECK ADD  CONSTRAINT [fk_PlanYearGroup_EmployerId] FOREIGN KEY([Employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO

ALTER TABLE [dbo].[PlanYearGroup] CHECK CONSTRAINT [fk_PlanYearGroup_EmployerId]
GO

------Prepopulate the data-------

GO
Insert into [dbo].[PlanYearGroup] 
		([EntityStatusId],
		[CreatedBy],
		[CreatedDate],
		[ModifiedBy],
		[ModifiedDate],
		[GroupName],
		[Employer_id]) 
	select 
		1,
		'system',
		GETDATE(),
		'system',
		GETDATE(), 
		[name],
		[employer_id] 
	from [dbo].[employer];
GO

------Alter existing tables------
---Alter Plan Year---

GO
alter table [dbo].[plan_year] add [PlanYearGroupId] [bigint] NOT NULL DEFAULT (0);
GO

UPDATE [dbo].[plan_year] 
	SET [dbo].[plan_year].[PlanYearGroupId] = g.[PlanYearGroupId]
	FROM [dbo].[plan_year] py 
	INNER JOIN [dbo].[PlanYearGroup] g 
	ON g.[Employer_id] = py.[employer_id];
GO

ALTER TABLE [dbo].[plan_year]  WITH NOCHECK ADD  CONSTRAINT [fk_plan_year_PlanYearGroupId] FOREIGN KEY([PlanYearGroupId])
REFERENCES [dbo].[PlanYearGroup] ([PlanYearGroupId])
GO

ALTER TABLE [dbo].[plan_year] CHECK CONSTRAINT [fk_plan_year_PlanYearGroupId]
GO

---Alter Employee---

--GO
--alter table [dbo].[employee] add [PlanYearGroupId] [bigint] NOT NULL DEFAULT (0);
--GO

--UPDATE [dbo].[employee] 
--	SET [dbo].[employee].[PlanYearGroupId] = py.[PlanYearGroupId]
--	FROM [dbo].[employee] e
--	INNER JOIN [dbo].[plan_year] py 
--	ON e.meas_plan_year_id = py.plan_year_id;
--GO

--ALTER TABLE [dbo].[employee]  WITH NOCHECK ADD  CONSTRAINT [fk_employee_PlanYearGroupId] FOREIGN KEY([PlanYearGroupId])
--REFERENCES [dbo].[PlanYearGroup] ([PlanYearGroupId])
--GO

--ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [fk_employee_PlanYearGroupId]
--GO

-----New Stored Procs-----
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 10/31/2016
-- Description:	Insert a new PlanYearGroup
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_PlanYearGroup]
	@CreatedBy nvarchar(50),
    @GroupName nvarchar(50),
	@Employer_id int,
	@insertedID int OUTPUT
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[PlanYearGroup]
	([EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate],
	[GroupName], [Employer_id])
	VALUES (1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE(),
	@GroupName, @Employer_id);

	SELECT @insertedID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Select a Single Row of [PlanYearGroup]
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_PlanYearGroup]
      @PlanYearGroupId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[PlanYearGroup] 
      WHERE [PlanYearGroupId] = @PlanYearGroupId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Select all Rows of [PlanYearGroup] belonging to a certain employer
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_PlanYearGroup_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[PlanYearGroup] 
      WHERE [Employer_Id] = @EmployerId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 10/31/2016
-- Description:	Update a PlanYearGroup
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_PlanYearGroup]
      @modifiedBy nvarchar(50),
      @PlanYearGroupId int,
      @GroupName nvarchar(50)
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [dbo].[PlanYearGroup] SET GroupName = @GroupName,
		ModifiedBy = @modifiedBy, ModifiedDate = GETDATE()
      WHERE PlanYearGroupId = @PlanYearGroupId
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/31/2016
-- Description: Set the entity Status  
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_PlanYearGroupStatus]
	   @EntityStatus int,
	   @PlanYearGroupId int,
	   @modifiedBy nvarchar(50)
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       Update [dbo].[PlanYearGroup] set [EntityStatusId] = @EntityStatus, [ModifiedBy] = @modifiedBy, [ModifiedDate] = GETDATE()
			where [PlanYearGroupId] = @PlanYearGroupId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO


-----Grant execute to all the new procs-----
GO
GRANT EXECUTE ON [dbo].[INSERT_PlanYearGroup] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_PlanYearGroup] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_PlanYearGroup_ForEmployer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_PlanYearGroup] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPDATE_PlanYearGroupStatus] TO [aca-user] AS [dbo]
GO


-----Modify Stored Procs-----

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
	@PlanYearGroupId bigint,
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
		default_Stability_End,
		PlanYearGroupId)
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
		@default_Stability_End,
		@PlanYearGroupId)

SELECT @planyearid = SCOPE_IDENTITY();
END
GO


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
	@default_Stability_End datetime,
	@PlanYearGroupId bigint
AS
BEGIN
	UPDATE [dbo].[plan_year]
	SET
		[description] = @description,
		[startDate] = @sDate,
		[endDate] = @eDate,
		[notes] = @notes,
		[history] = @history,
		[modOn] = @modOn,
		[modBy] = @modBy,
		[default_Meas_Start] = @default_Meas_Start,
		[default_Meas_End] = @default_Meas_End,
		[default_Admin_Start] = @default_Admin_Start,
		[default_Admin_End] = @default_Admin_End,
		[default_Open_Start] = @default_Open_Start,
		[default_Open_End] = @default_Open_End,
		[default_Stability_Start] = @default_Stability_Start,
		[default_Stability_End] = @default_Stability_End,
		[PlanYearGroupId] = @PlanYearGroupId
	WHERE
		[plan_year_id]=@planyearID;
	
--	UPDATE [dbo].[employee] 
--		SET [dbo].[employee].[PlanYearGroupId] = @PlanYearGroupId
--	where [meas_plan_year_id] = @planyearID;

END
GO

