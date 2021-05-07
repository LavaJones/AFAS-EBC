USE aca
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye Kolokolo
-- Create date: 04/14/2017
-- Description:	bulk deletes records in the tax_year_1095c_approval table based on a list of given employees and tax year
-- =============================================
CREATE PROCEDURE [dbo].[BULK_DELETE_tax_year_1095c_approval]
	@approvalDelete BULK_Tax_year_1095c_approval readonly
AS

BEGIN TRY

	DELETE FROM [tax_year_1095c_approval]
	WHERE ResourceId IN
		(SELECT tya.ResourceId
		FROM [dbo].[tax_year_1095c_approval] tya 
			INNER JOIN @approvalDelete ad 
		ON tya.tax_year = ad.tax_year 
			AND tya.employee_id = ad.employee_id)

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [BULK_DELETE_tax_year_1095c_approval] TO [aca-user] as [dbo]
GO