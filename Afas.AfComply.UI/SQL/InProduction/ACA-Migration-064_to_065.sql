USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return all employees who have had their 1095c approved for a specific tax year.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_employees_Tax_Year_Approved](
	@employerID int,
	@taxYear int
	)
AS
BEGIN
	SELECT
		[employee_id],
		[employee_type_id],
		[HR_status_id],
		[employer_id],
		[fName],
		[mName],
		[lName],
		[address],
		[city],
		[state_id],
		[zip],
		[hireDate],
		[currDate],
		[ssn],
		[ext_emp_id],
		[terminationDate],
		[dob],
		[initialMeasurmentEnd],
		[plan_year_id],
		[limbo_plan_year_id],
		[meas_plan_year_id],
		[modOn],
		[modBy],
		[plan_year_avg_hours],
		[limbo_plan_year_avg_hours],
		[meas_plan_year_avg_hours],
		[imp_plan_year_avg_hours],
		[classification_id],
		[aca_status_id],
		[ResourceId]
	FROM
		[dbo].[employee] ee
	WHERE
		ee.[employer_id] = @employerID
			AND
		ee.[employee_id] IN (
			SELECT
				ty1a.[employee_id]
			FROM [tax_year_1095c_approval] ty1a
			WHERE
				ty1a.[tax_year] = @taxYear
			);

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Jacob Turnbull>
-- Create date: <5/3/2016>
-- Description:	<This stored procedure will DELETE an Employee from the database. Only if the Employee ID is not used in other tables.>
-- Modifications:
--		- Added the Tax Year parameter. 2/19/2017 Travis Wells
-- =============================================
ALTER PROCEDURE [dbo].[DELETE_employee_1095c_approval]
	@employeeID int,
	@employerID int,
	@taxyear int
AS
BEGIN
DELETE [dbo].[tax_year_1095c_approval]
WHERE
	employee_id = @employeeID
		AND
	employer_id = @employerID
		AND
	tax_year = @taxyear;

END

GO

