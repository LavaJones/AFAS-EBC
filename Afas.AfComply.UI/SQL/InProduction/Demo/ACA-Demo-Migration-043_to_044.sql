USE [aca-demo];

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
GO
/****** Object:  StoredProcedure [dbo].[sp_AIR_ETL_ShortBuild]    Script Date: 1/31/2017 4:20:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_AIR_ETL_ShortBuild]
	@employerID int,
	@taxYear int,
	@employeeID int
AS

BEGIN TRY
	-- Extract Employee Info through AIR Process.
	EXEC dbo.PrepareAcaForIRSStaging @employerID, @taxYear
	
	EXEC [air-demo].etl.spETL_Build
		@employerid,
		@taxYear,
		@employeeID
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO
CREATE PROCEDURE [dbo].[sp_AIR_ADD_insuranceCoverage]
	@taxYear int
AS
BEGIN TRY
	INSERT INTO 
	dbo.insurance_coverage (
		tax_year, 
		carrier_id, 
		employee_id, 
		dependent_id, 
		all12, 
		jan, 
		feb, 
		mar, 
		apr, 
		may, 
		jun, 
		jul, 
		aug, 
		sep, 
		oct, 
		nov, 
		[dec], 
		history)
SELECT @taxYear,
		13,
		ed.employee_id,
		ed.dependent_id,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		'Input added by System'
FROM 
	dbo.insurance_coverage ic 
		RIGHT OUTER JOIN dbo.employee_dependents ed 
			ON ed.dependent_id = ic.dependent_id
WHERE ed.dependent_id NOT IN (SELECT 
								dependent_id 
							  FROM 
								dbo.insurance_coverage 
							  WHERE dependent_id is not null)
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
GRANT EXECUTE ON [dbo].[sp_AIR_ADD_insuranceCoverage] TO [aca-user] AS [dbo]
GO
ALTER PROCEDURE [dbo].[sp_AIR_UPDATE_approved_monthly_detail]
	@employeeID int,
	@timeFrameID int,
	@employerID int,
	@hours decimal(18,2),
	@ooc nchar(2),
	@mec bit, 
	@lcmp decimal,
	@ash nchar(2),
	@enrolled bit,
	@monthlyStatusID int,
	@insuranceTypeID int,
	@modBy varchar(50),
	@modOn datetime
AS

BEGIN TRY

DECLARE @employeeID2 int;
SET @employeeID2=0;

UPDATE [air-demo].[appr].employee_monthly_detail
	SET
		offer_of_coverage_code=@ooc,
		mec_offered=@mec,
		share_lowest_cost_monthly_premium=@lcmp,
		safe_harbor_code=@ash,
		enrolled=@enrolled,
		monthly_status_id=@monthlyStatusID,
		insurance_type_id=@insuranceTypeID,
		modified_by=@modBy,
		modified_date=@modOn
	WHERE
		employee_id=@employeeID AND
		time_frame_id=@timeFrameID

IF @@ROWCOUNT = 0
	BEGIN
		/*************************************************************
		****** Changed this to look at monthly detail as some employees 
		existedin the system, but had no Monthly Details to write too. 
		*************************************************************/
		--SELECT @employeeID2=employee_id FROM [air].[emp].employee 
		--WHERE employee_id=@employeeID;
		SELECT @employeeID2=employee_id FROM [air].appr.employee_monthly_detail
		WHERE employee_id=@employeeID;

		PRINT 'EMPLOYEE ID: ' + CONVERT(varchar, @employeeID2);

		IF @employeeID2 = 0
			BEGIN
				-- Extract Employee Info through AIR Process.
				EXEC [air-demo].etl.spETL_Build
					@employerid, 
					2016, 
					@employeeID
			END

		UPDATE [air-demo].[appr].employee_monthly_detail
			SET
				offer_of_coverage_code=@ooc,
				mec_offered=@mec,
				share_lowest_cost_monthly_premium=@lcmp,
				safe_harbor_code=@ash,
				enrolled=@enrolled,
				monthly_status_id=@monthlyStatusID,
				insurance_type_id=@insuranceTypeID,
				modified_by=@modBy,
				modified_date=@modOn
			WHERE
				employee_id=@employeeID AND
				time_frame_id=@timeFrameID
	END

END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
