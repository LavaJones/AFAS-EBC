USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_SELECT_submission_statuses]    Script Date: 3/17/2017 2:15:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/15/2017>
-- Description:	<This stored procedure is meant to return all AIR submission status possibilities.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_submission_statuses]
AS

BEGIN
	SELECT
		*
	FROM
		air.sr.status_code
END



GO

