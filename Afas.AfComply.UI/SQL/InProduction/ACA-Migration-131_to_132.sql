USE aca
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 05/02/2016
-- Description:	returns a tax_year_1095c_correction_exception
-- =============================================
CREATE PROCEDURE SELECT_tax_year_1095c_correction_exception 
	-- Add the parameters for the stored procedure here
	@tax_year INT,
	@employer_id INT,
	@employee_id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		TaxYear1095cCorrectionExceptionId,
		tax_year,
		employer_id,
		employee_id,
		Justification,
		CreatedBy,
		ModifiedBy
	FROM [dbo].[tax_year_1095c_correction_exception]
	WHERE employer_id = @employer_id
		AND tax_year = @tax_year
		AND employee_id = @employee_id
END
GO

GRANT EXECUTE ON [dbo].[SELECT_tax_year_1095c_correction_exception] TO [aca-user] AS [DBO]
GO

