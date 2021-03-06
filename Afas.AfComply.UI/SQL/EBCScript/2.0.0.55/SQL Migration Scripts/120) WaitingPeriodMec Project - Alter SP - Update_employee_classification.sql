USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[UPDATE_employee_classification]    Script Date: 9/6/2017 2:17:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure is meant to update the EMPLOYER Classification.>
-- Modifications:
--		9-6-2017 TLW
--			- Added the waiting period id and entity status id
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_employee_classification]
	@classID int,
	@description varchar(50), 
	@ashCode varchar(2),
	@modBy varchar(50), 
	@modOn datetime, 
	@history varchar(max),
	@waitingPeriodID int,
	@entityStatusID int,
	@ooc varchar(2)
AS
BEGIN
	UPDATE employee_classification
	SET
		[description] = @description,
		ash_code = @ashCode,
		ModifiedDate = @modOn,
		ModifiedBy = @modBy,
		history=@history,
		WaitingPeriodID=@waitingPeriodID,
		EntityStatusID=@entityStatusID,
		Ooc=@ooc
	WHERE
		classification_id=@classID;

END





































