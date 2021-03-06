USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[INSERT_new_insurance_plan]    Script Date: 9/5/2017 12:18:23 PM ******/
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
--		9-5-2017 TLW
--				- Added the following parameters: mec	
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
	@mec bit,
	@fullyPlusSelf bit,
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
		CreatedBy,
		CreatedDate, 
		ModifiedDate,
		ModifiedBy, 
		history,
		insurance_type_id,
		Mec,
		EntityStatusID,
		fullyPlusSelfInsured)
	VALUES(
		@planyearID,
		@name,
		@monthlycost, 
		@minValue,
		@offSpouse,
		@SpouseConditional,
		@offDependent,
		@modBy,
		@modOn,
		@modOn,
		@modBy,
		@history,
		@insuranceTypeID,
		@mec,
		1,
		@fullyPlusSelf)

SELECT @insuranceid = SCOPE_IDENTITY();
END


