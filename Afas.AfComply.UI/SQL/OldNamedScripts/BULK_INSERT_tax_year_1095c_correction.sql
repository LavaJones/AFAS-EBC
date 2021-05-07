USE aca
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye Kolokolo
-- Create date: 04/14/2017
-- Description:	updates the transmitted bit for given employee and tax year
-- =============================================
CREATE PROCEDURE [dbo].[BULK_INSERT_tax_year_1095c_correction]
	@correctionInsert BULK_Tax_year_1095c_correction readonly
AS

BEGIN TRY
	INSERT INTO [dbo].[tax_year_1095c_correction] ([tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[ResourceId]
      ,[Corrected]
      ,[OriginalUniqueSubmissionId]
      ,[CorrectedUniqueSubmissionId]
      ,[CorrectedUniqueRecordId]
      ,[Transmitted]
      ,[EntityStatusId]
      ,[ModifiedBy]
      ,[ModifiedDate])
	SELECT [tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[ResourceId]
      ,[Corrected]
      ,[OriginalUniqueSubmissionId]
      ,[CorrectedUniqueSubmissionId]
      ,[CorrectedUniqueRecordId]
      ,[Transmitted]
      ,1
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM @correctionInsert
	WHERE [employee_id] NOT IN (
			SELECT
				ty1a.[employee_id]
			FROM [dbo].[tax_year_1095c_correction] ty1a INNER JOIN @correctionInsert ci ON ty1a.employee_id = ci.employee_id AND ty1a.tax_year = ci.tax_year
			WHERE ty1a.Corrected = 0
				AND ty1a.Transmitted = 0
				AND ty1a.EntityStatusId = 1
		);
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [BULK_INSERT_tax_year_1095c_correction] TO [aca-user] as [dbo]
GO