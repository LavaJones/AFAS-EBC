USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employee_dependent_coverage]    Script Date: 12/20/2016 1:28:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/23/2016>
-- Description:	<This stored procedure is meant to return all covered dependents.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employee_dependent_coverage]
	@employeeID int, 
	@taxYear int
AS

BEGIN
	SELECT
		ed.dependent_id,
		ed.employee_id,
		ed.fName AS Fname,
		ed.lName AS Lname,
		ed.mName AS Mi,
		ed.dob,
		ice.Jan,
		ice.Feb,
		ice.Mar,
		ice.Apr,
		ice.May,
		ice.Jun,
		ice.Jul,
		ice.Aug,
		ice.Sept,
		ice.Oct,
		ice.Nov,
		ice.[Dec]
	FROM dbo.employee_dependents ed INNER JOIN dbo.insurance_coverage_editable ice ON ed.dependent_id = ice.dependent_id
	WHERE ed.employee_id = @employeeID AND ice.tax_year=@taxYear;
END

