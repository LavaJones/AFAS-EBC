USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_SELECT_employer_status_request]    Script Date: 3/17/2017 2:13:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/29/2016>
-- Description:	<This stored procedure is meant to return all status request rows for a specific receipt id.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_status_request]
	@receiptID varchar(50)
AS

BEGIN
	SELECT * FROM air.sr.status_request
	WHERE air.sr.status_request.receipt_id=@receiptID;
END













































GO

