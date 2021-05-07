USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_SELECT_receipt_statuses]    Script Date: 3/17/2017 2:16:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/17/2017>
-- Description:	<This stored procedure is meant to return all status possibilities for the br schema.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_receipt_statuses]
AS

BEGIN
	SELECT
		*
	FROM
		air.br.status_code
END




GO

