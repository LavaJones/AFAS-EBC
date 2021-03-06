USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[DELETE_insurance]    Script Date: 9/6/2017 8:28:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/17/2015>
-- Description:	<This stored procedure will DELETE an Employers Insurance Plan from the database.>
-- Changes:
--		- 9/6/2017 TLW
--			This was changed to alter the Entity Status ID to the delete status instead of hard deleting the row from the database. 
-- =============================================
ALTER PROCEDURE [dbo].[DELETE_insurance]
	@insuranceID int
AS
BEGIN
UPDATE insurance
	SET
		EntityStatusID=3
	WHERE
		insurance_id=@insuranceID;


END








































