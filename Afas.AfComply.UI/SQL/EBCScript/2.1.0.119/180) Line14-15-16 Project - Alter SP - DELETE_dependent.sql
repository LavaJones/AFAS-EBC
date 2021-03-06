USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[DELETE_dependent]    Script Date: 12/14/2017 8:30:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/29/2016>
-- Description:	<This stored procedure will DELETE an Employees Dependent.>
--		
-- =============================================
ALTER PROCEDURE [dbo].[DELETE_dependent]
	@employeeID int,
	@dependentID int
AS
BEGIN
UPDATE employee_dependents
SET
	EntityStatusID=2
WHERE 
	employee_id=@employeeID AND
	dependent_id=@dependentID;
END

