USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_employer_employees_Tax_Year_Not_Approved] (
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
		[dbo].[employee]
	WHERE
		[employer_id] = @employerID


			AND
		[employee_id] NOT IN (
			SELECT DISTINCT
				ty1a.[employee_id]
			FROM
				[dbo].[tax_year_1095c_approval] ty1a
			WHERE
				ty1a.[tax_year] = @taxYear
					AND
				ty1a.employer_id = @employerID
		)
			AND
		[employee_id] IN (
			SELECT DISTINCT
				employee_id
			FROM
				[air].[appr].[employee_yearly_detail]
			WHERE
				_1095C=1
					AND
				employer_id=@employerID 
					AND
				year_id=@taxYear
		)
END
GO

GO
GRANT EXECUTE ON [dbo].[SELECT_employer_employees_Tax_Year_Not_Approved] TO [aca-user] AS [dbo] 
GO