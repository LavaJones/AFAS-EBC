USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Ryan McCully>
-- Create date: <3/5/2017>
-- Description:	<This stored procedure is meant to return the count of PDfs per employer>
-- =============================================
Create PROCEDURE [dbo].[SELECT_PrintedCountPerEmployer]
@taxYear int
AS
BEGIN
SELECT tax.[employer_id], emp.[name], COUNT(tax.[printed]) as pdfCount
  FROM [dbo].[tax_year_1095c_approval] tax 
  inner join [dbo].[employer] emp on emp.[employer_id] = tax.[employer_id]
  Where [tax_year] = @taxYear AND [printed] = 1 Group By tax.[employer_id], emp.[name] ORDER BY pdfCount desc
END
GO

GRANT EXECUTE ON [dbo].[SELECT_PrintedCountPerEmployer] TO [aca-user] AS [dbo]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Ryan McCully>
-- Create date: <3/5/2017>
-- Description:	<This stored procedure is meant to return the count of unprinted PDfs per employer>
-- =============================================
Create PROCEDURE [dbo].[SELECT_NotPrintedCountPerEmployer]
@taxYear int
AS
BEGIN
SELECT tax.[employer_id], emp.[name], COUNT(tax.[printed]) as pdfCount
  FROM [dbo].[tax_year_1095c_approval] tax 
  inner join [dbo].[employer] emp on emp.[employer_id] = tax.[employer_id]
  Where [tax_year] = @taxYear AND [printed] = 0 Group By tax.[employer_id], emp.[name] ORDER BY pdfCount desc
END
GO

GRANT EXECUTE ON [dbo].[SELECT_NotPrintedCountPerEmployer] TO [aca-user] AS [dbo]
GO