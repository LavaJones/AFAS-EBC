USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_SELECT_employeeID_status_error]    Script Date: 3/17/2017 2:12:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/27/2016>
-- Description:	<This stored procedure is meant to return all submissions to the IRS by EIN and Year.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employeeID_status_error]
	@utID varchar(100), 
	@recordID int
AS

BEGIN
	SELECT air.fdf._1095C.employee_id FROM air.fdf._1095C
	WHERE unique_transmission_id=@utID AND record_id=@recordID;
END














































GO

