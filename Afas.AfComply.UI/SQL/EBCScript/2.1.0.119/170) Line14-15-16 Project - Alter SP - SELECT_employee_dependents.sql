USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employee_dependents]    Script Date: 12/14/2017 8:24:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2-24-2016>
-- Description:	<This stored procedure is meant to return all dependents and existing employee has.>
-- Modifications:Modified date 6/30/2017, modified by GN. Replaced "*" with columns. 
--		12/14/2017 TW
--			- Added the entity status id. 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employee_dependents](
	@employeeID int
	)
AS
BEGIN
	SELECT [dependent_id]
		  ,[employee_id]
		  ,[fName]
		  ,[mName]
		  ,[lName]
		  ,[ssn]
		  ,[dob]
		  ,[ResourceId]
    FROM [aca].[dbo].[employee_dependents]
	WHERE employee_id=@employeeID AND EntityStatusID=1;
END





