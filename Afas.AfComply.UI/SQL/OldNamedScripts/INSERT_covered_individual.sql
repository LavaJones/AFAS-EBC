USE aca
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 04/25/2016
-- Description:	inserts a new record in the [air].[emp].[covered_individual] and return covered individual
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_covered_individual]
	-- Add the parameters for the stored procedure here
	   @employeeID int,
	   @employerID int,
	   @dependentID int,
	   @taxYear int,
	   @annual_coverage_indicator bit
AS
BEGIN TRY

	IF @dependentID = 0
	BEGIN

		INSERT INTO [air].[emp].[covered_individual] (
			[covered_individual_id]
			,[employee_id]
			,[employer_id]
			,[first_name]
			,[middle_name]
			,[last_name]
			,[ssn]
			,[birth_date]
			,[annual_coverage_indicator]
		)
		SELECT ice.row_id,
			ice.employee_id,
			ice.employer_id,
			e.fName,
			e.mName,
			e.lName,
			e.ssn,
			e.dob,
			@annual_coverage_indicator
		FROM [aca].[dbo].[employee] e 
			INNER JOIN [aca].[dbo].[insurance_coverage_editable] ice ON e.employee_id = ice.employee_id
		WHERE ice.row_id NOT IN (SELECT [covered_individual_id] FROM [air].[emp].[covered_individual] WHERE tax_year = @taxYear)
			AND ice.employer_id = @employerID
			AND ice.employee_id = @employeeID
			AND ice.tax_year = @taxYear
			AND ice.dependent_id IS NULL

	END
	ELSE
	BEGIN
			
		INSERT INTO [air].[emp].[covered_individual] (
			[covered_individual_id]
			,[employee_id]
			,[employer_id]
			,[first_name]
			,[middle_name]
			,[last_name]
			,[ssn]
			,[birth_date]
			,[annual_coverage_indicator]
		)
		SELECT ice.row_id,
			ice.employee_id,
			ice.employer_id,
			ed.fName,
			ed.mName,
			ed.lName,
			ed.ssn,
			ed.dob,
			@annual_coverage_indicator
		FROM [aca].[dbo].[employee_dependents] ed 
			INNER JOIN [aca].[dbo].[insurance_coverage_editable] ice ON ed.employee_id = ice.employee_id AND ed.dependent_id = ice.dependent_id
		WHERE ice.row_id NOT IN (SELECT [covered_individual_id] FROM [air].[emp].[covered_individual] WHERE tax_year = @taxYear)
			AND ice.employer_id = @employerID
			AND ice.employee_id = @employeeID
			AND ice.tax_year = @taxYear
			AND ice.dependent_id = @dependentID
		
	END

	SELECT ci.*
	FROM [aca].[dbo].[insurance_coverage_editable] ice INNER JOIN [air].[emp].[covered_individual] ci ON ice.row_id = ci.covered_individual_id
	WHERE ice.employer_id = @employerID
		AND ice.employee_id = @employeeID
		AND ice.tax_year = @taxYear
		AND (ice.dependent_id = @dependentID OR (ice.dependent_id IS NULL AND @dependentID = 0))

END TRY

BEGIN CATCH

	EXEC [dbo].[INSERT_ErrorLogging]

END CATCH
GO

GRANT EXECUTE ON [dbo].[INSERT_covered_individual] TO [aca-user] AS [dbo]
GO
