USE [aca];

--Create new Table
CREATE TABLE [dbo].[other_ale_group_members]
(
	[Ale_Member_Id] [bigint] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[ein] [nchar](10) NOT NULL,
	[name] [varchar](50) NOT NULL,
--Standard Items
	[ResourceId] [uniqueidentifier] ROWGUIDCOL NOT NULL,
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
)
GO

ALTER TABLE [dbo].[other_ale_group_members]  WITH NOCHECK ADD 
CONSTRAINT [fk_other_ale_group_members_employerId] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO

ALTER TABLE [dbo].[other_ale_group_members] ADD 
CONSTRAINT [DF_other_ale_group_members_resourceId]  DEFAULT (newid()) FOR [ResourceId]
GO

ALTER TABLE [dbo].[other_ale_group_members]  WITH CHECK ADD 
CONSTRAINT [FK_other_ale_group_members_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO


--Stored Procs for ALE
--Select by employer
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 1/31/2017
-- Description: Select all ALE for an employer
-- =============================================
Create PROCEDURE [dbo].[SELECT_ALE_Members_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    
SELECT [Ale_Member_Id]
      ,[employer_id]
      ,[ein]
      ,[name]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
  FROM [dbo].[other_ale_group_members] Where [employer_id] = @EmployerId AND [EntityStatusId] = 1

END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
GRANT EXECUTE ON [dbo].[SELECT_ALE_Members_ForEmployer] TO [aca-user] AS [dbo]
GO


-- Deactivate ALE Row
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 1/31/2017
-- Description: Deactivate an ALE for an employer
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_ALE_Member]
      @AleMemberId int,
	  @modifiedBy nvarchar(50)
AS
BEGIN TRY
    
Update [dbo].[other_ale_group_members] set 
		[EntityStatusId] = 2, 
		ModifiedDate = GETDATE(), 
		ModifiedBy = @modifiedBy
	Where [Ale_Member_Id] = @AleMemberId

END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
GRANT EXECUTE ON [dbo].[DELETE_ALE_Member] TO [aca-user] AS [dbo]
GO

-- Insert Or Update

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 1/31/2017
-- Description:	Upsert: update or insert ALE Member into the table
-- =============================================
CREATE PROCEDURE [dbo].[UPSERT_Ale_Member]
	@AleMemberId bigint,
	@employerId int,
	@ein nchar(10),
	@name varchar(50),
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	MERGE [dbo].[other_ale_group_members]  AS T  
	USING (
			SELECT @AleMemberId Ale_Member_Id,
			@employerId employerId,
			@ein ein,
			@name name, 
			@CreatedBy CreatedBy
		) AS S 
	ON T.Ale_Member_Id = S.Ale_Member_Id AND T.employer_Id = S.employerId
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[ein] = S.ein,
		T.[name] = S.name,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE()
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[employer_id]
		  ,[ein]
		  ,[name]
		  ,[EntityStatusId],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employerId, 
		  S.ein,
		  S.name,
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );

	SELECT @insertedID = SCOPE_IDENTITY();

END
GO
GRANT EXECUTE ON [dbo].[UPSERT_Ale_Member] TO [aca-user] AS [dbo]
GO


--Add New Field to Insurance
ALTER TABLE [dbo].[insurance] ADD SpouseConditional BIT null default 0;
GO

-- Modify stored Procs to deal with extra field
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to create a new insurance plan.>
-- Changes:
--	     5-19-2015 TLW
--				- Added the following three parameters: minValue, offSpouse, offDependent.		
-- ============================================
ALTER PROCEDURE [dbo].[INSERT_new_insurance_plan]
	@planyearID int,
	@name varchar(50),
	@monthlycost money,
	@minValue bit,
	@offSpouse bit,
	@SpouseConditional bit,
	@offDependent bit,
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(max),
	@insuranceTypeID int,
	@insuranceid int OUTPUT
AS

BEGIN
	INSERT INTO [insurance](
		plan_year_id,
		[description],
		monthlycost,
		minValue,
		offSpouse,
		SpouseConditional,
		offDependent, 
		modOn,
		modBy, 
		history,
		insurance_type_id)
	VALUES(
		@planyearID,
		@name,
		@monthlycost, 
		@minValue,
		@offSpouse,
		@SpouseConditional,
		@offDependent,
		@modOn,
		@modBy,
		@history,
		@insuranceTypeID)

SELECT @insuranceid = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to update an insurance plan.>
-- Changes:
--		5-19-2015 TLW
--			- Added the following three parameters: minValue, offSpouse, offDependent.
--		10-28-2015 TLW
--			- Added InsuranceTypeID
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_insurance_plan]
	@insuranceID int,
	@planyearID int,
	@name varchar(50),
	@cost money,
	@minValue bit,
	@offSpouse bit,
	@SpouseConditional bit,
	@offDependent bit,
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@insuranceTypeID int

AS
BEGIN
	UPDATE insurance
	SET
		plan_year_id=@planyearID,
		[description] = @name,
		monthlycost = @cost,
		minValue=@minValue,
		offSpouse=@offSpouse,
		SpouseConditional = @SpouseConditional,
		offDependent=@offDependent,
		history = @history,
		modOn = @modOn,
		modBy = @modBy,
		insurance_type_id=@insuranceTypeID
	WHERE
		insurance_id=@insuranceID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to return all insurance plans related to a plan year.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_plan_year_insurance_plan](
	@planyearID int
	)
AS
BEGIN
	SELECT [insurance_id]
      ,[plan_year_id]
      ,[description]
      ,[monthlycost]
      ,[minValue]
      ,[offSpouse]
      ,[offDependent]
      ,[modOn]
      ,[modBy]
      ,[history]
      ,[insurance_type_id]
      ,[ResourceId]
      ,[SpouseConditional]
	FROM [insurance]
	WHERE plan_year_ID=@planyearID;
END

GO


-- This Script should have been run in all enviroments, but this is double checking.
IF EXISTS (SELECT * 
FROM sys.indexes
WHERE object_id = OBJECT_ID('[dbo].[employee_insurance_offer]') AND name='UN_employeeInsuranceOffer_Employee_PlanYear')
    ALTER TABLE [dbo].[employee_insurance_offer] DROP CONSTRAINT [UN_employeeInsuranceOffer_Employee_PlanYear]
GO
ALTER TABLE [dbo].[employee_insurance_offer] ADD CONSTRAINT [UN_employeeInsuranceOffer_Employee_PlanYear] UNIQUE NONCLUSTERED 
(
       [employee_id] ASC,
       [plan_year_id] ASC
)
GO
