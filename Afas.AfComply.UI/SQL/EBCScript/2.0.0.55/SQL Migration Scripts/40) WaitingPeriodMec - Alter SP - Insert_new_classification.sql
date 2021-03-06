USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[INSERT_new_classification]    Script Date: 8/30/2017 11:09:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure is meant to create a Classification.>
-- Changes: 
-- 	8-30-2017:
--		- Added the WaitingPeriodID to the stored procedure.
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_new_classification]
	@employerID int,
	@desc varchar(50),
	@ashCode varchar(2) NULL,
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max),
	@waitingPeriodID int NULL, 
	@ooc varchar(2) NULL
AS

BEGIN
	INSERT INTO [employee_classification](
		employer_id,
		[description],
		ash_code,
		ModifiedBy,
		ModifiedDate,
		history,
		WaitingPeriodID,
		CreatedDate,
		CreatedBy,
		EntityStatusID,
		Ooc)
	VALUES(
		@employerID,
		@desc,
		@ashCode,
		@modBy,
		@modOn,
		@history,
		@waitingPeriodID,
		@modOn,
		@modBy,
		1,
		@ooc)

END

















































