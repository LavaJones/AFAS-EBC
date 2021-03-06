USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[UPDATE_insurance_plan]    Script Date: 9/5/2017 2:06:44 PM ******/
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
--		9/5/2017 TLW
--			- Added Mec
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
	@insuranceTypeID int,
	@mec bit,
	@fullyPlusSelf bit
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
		ModifiedDate = @modOn,
		ModifiedBy = @modBy,
		insurance_type_id=@insuranceTypeID,
		Mec=@mec,
		fullyPlusSelfInsured=@fullyPlusSelf
	WHERE
		insurance_id=@insuranceID;

END


