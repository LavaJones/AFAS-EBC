USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[SELECT_plan_year_insurance_plan]    Script Date: 9/5/2017 10:42:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to return all insurance plans related to a plan year.>
-- Changes:
--     - 9/5/2017 TLW
--     - Change the column names for ModifiedOn and ModifiedBy to match AF's naming convention. 
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
      ,[ModifiedDate]
      ,[ModifiedBy]
      ,[history]
      ,[insurance_type_id]
      ,[ResourceId]
      ,[SpouseConditional]
	  ,[mec]
	  ,[CreatedBy]
	  ,[CreatedDate]
	  ,[EntityStatusID]
	  ,[fullyPlusSelfInsured]
	FROM [insurance]
	WHERE plan_year_ID=@planyearID;
END



GO


