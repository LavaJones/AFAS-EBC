USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[DELETE_classification]    Script Date: 9/6/2017 3:31:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure will DELETE an Employee Classification from the database.>
-- Modifications:
--		9-6-2017 TLW
--			- Changed the DELETE to use the Entity Status ID instead of hard deleting the classification.
-- =============================================
ALTER PROCEDURE [dbo].[DELETE_classification]
	@classID int
AS
BEGIN
UPDATE employee_classification
SET
	EntityStatusID=3
WHERE classification_id=@classID;


END







































