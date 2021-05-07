USE [aca]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye Kolokolo
-- Create date: 04/14/2017
-- Description:	bulk inactivates records in the tax_year_1095c_correction table based on a list of given employees and tax year
-- =============================================
CREATE PROCEDURE [dbo].[BULK_INACTIVATE_tax_year_1095c_correction]
	@employerId int,
	@modifiedBy nvarchar(50)
AS

BEGIN TRY

	UPDATE [tax_year_1095c_correction]
	SET EntityStatusId = 2,
		ModifiedBy = @modifiedBy,
		ModifiedDate = GETDATE()
	FROM [dbo].[tax_year_1095c_correction]
	WHERE employer_id = @employerId
	AND Corrected = 1
	AND Transmitted = 1
	AND EntityStatusId = 1

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [BULK_INACTIVATE_tax_year_1095c_correction] TO [aca-user] as [dbo]
GO