USE aca
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
CREATE PROCEDURE dbo.SELECT_1094CCorrectionCount
	@employerId as int,
	@taxYear as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT COUNT(tax_year_employer_transmissionId) as [1094CCorrections] 
	FROM (SELECT [tax_year_employer_transmissionId]
  FROM [aca].[dbo].[tax_year_employer_transmission] a LEFT OUTER JOIN dbo.TransmissionLinking b ON a.tax_year_employer_transmissionId = b.TaxYearEmployerTransmissionId 
  where (employer_id = @employerId and b.TaxYearEmployerTransmissionId is null) and a.TransmissionType = 'C' and tax_year = @taxYear
			GROUP BY tax_year_employer_transmissionId) as item
  
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO