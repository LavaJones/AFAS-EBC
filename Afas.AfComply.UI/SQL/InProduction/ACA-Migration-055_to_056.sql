USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/16/2016>
-- Description:	<This stored procedure is meant to return all editable rows for a specific individuals.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employee_editable_individual_coverage]
	@employeeID int, 
	@taxYear int
AS

BEGIN
	SELECT
		ice.[row_id],
		ice.[employee_id],
		ice.[employer_id],
		ice.[dependent_id],
		ice.[tax_year],
		ice.[Jan],
		ice.[Feb],
		ice.[Mar],
		ice.[Apr],
		ice.[May],
		ice.[Jun],
		ice.[Jul],
		ice.[Aug],
		ice.[Sept],
		ice.[Oct],
		ice.[Nov],
		ice.[Dec],
		ice.[ResourceId]
	FROM
		[aca].[dbo].[insurance_coverage_editable] ice
	WHERE
		ice.[employee_id] = @employeeID
			AND
		ice.[tax_year] = @taxYear;

END

GO

