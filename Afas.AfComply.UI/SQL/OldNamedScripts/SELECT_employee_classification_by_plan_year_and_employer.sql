USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE SELECT_employee_classification_by_plan_year_and_employer
	-- Add the parameters for the stored procedure here
	@employerId INT,
	@planYearId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		ins.insurance_description,
		ins.insurance_id,
		ins.employee_contribution,
		ins.insurance_type,
		ec.[description] AS classification_description,
		ec.classification_id
	FROM [employee_classification] ec 
		LEFT OUTER JOIN (
			SELECT
				i.insurance_id, 
				it.[name] as insurance_type,
				ic.classification_id,
				convert(numeric(18,2),(i.monthlycost - ic.amount)) as employee_contribution,
				i.[description] AS insurance_description
			FROM [insurance] i 
				INNER JOIN [insurance_contribution] ic ON i.insurance_id = ic.insurance_id
				INNER JOIN [insurance_type] it ON it.insurance_type_id = i.insurance_type_id
			WHERE i.plan_year_id = @planYearId) ins 
		ON ec.classification_id = ins.classification_id
	WHERE ec.employer_id = @employerId

END
GO
