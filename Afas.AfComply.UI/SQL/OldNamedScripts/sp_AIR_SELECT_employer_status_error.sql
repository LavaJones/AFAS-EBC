USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_SELECT_employer_status_error]    Script Date: 3/17/2017 2:13:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/29/2016>
-- Description:	<This stored procedure is meant to return all errors pertaining to a specific Status Request ID.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_status_error]
	@srID int
AS

BEGIN
	SELECT * FROM air.sr.request_error
	WHERE air.sr.request_error.status_request_id=@srID;
END














































GO

