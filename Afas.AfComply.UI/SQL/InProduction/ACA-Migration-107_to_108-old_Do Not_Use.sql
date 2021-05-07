USE aca
GO
CREATE TYPE BULK_Tax_year_1095c_correction AS TABLE
(
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[approvedBy] [varchar](50) NOT NULL,
	[approvedOn] [datetime2](7) NOT NULL,
	[get1095C] [bit] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
	[Corrected] [bit] NOT NULL,
	[Transmitted] [bit] NOT NULL,
	[EntityStatusId] [int] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL		

)
GO
CREATE PROCEDURE [dbo].[INSERT_tax_year_1095c_correction]
	@correctionInsert BULK_Tax_year_1095c_correction readonly
AS

BEGIN TRY
	INSERT INTO [dbo].[tax_year_1095c_correction] ([tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
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
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[Corrected]
      ,null
      ,null
      ,null
      ,[Transmitted]
      ,[EntityStatusId]
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM @correctionInsert
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GRANT SELECT ON BULK_Tax_year_1095c_correction TO [aca-user] as [dbo]