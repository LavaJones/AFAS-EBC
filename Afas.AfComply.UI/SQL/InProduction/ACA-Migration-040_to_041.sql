USE [aca]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_employee_classification_by_plan_year_and_employer]
	@employerId INT,
	@planYearId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ins.insurance_description,
		ins.insurance_id,
		ins.amount,
		ec.[description] AS classification_description,
		ec.classification_id
	FROM [employee_classification] ec 
		LEFT OUTER JOIN (
			SELECT
				i.insurance_id, 
				ic.classification_id,
				ic.amount,
				i.[description] AS insurance_description
			FROM [insurance] i 
				INNER JOIN [insurance_contribution] ic ON i.insurance_id = ic.insurance_id
			WHERE i.plan_year_id = @planYearId) ins 
		ON ec.classification_id = ins.classification_id
	WHERE ec.employer_id = @employerId

END
GO

GRANT EXECUTE ON [dbo].[SELECT_employee_classification_by_plan_year_and_employer] TO [aca-user] AS [dbo]
GO

