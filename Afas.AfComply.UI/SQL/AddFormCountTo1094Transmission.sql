USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_tax_year_submissions_employer]    Script Date: 5/18/2018 1:26:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/20/2017>
-- Description:	<This stored procedure is meant to return all taxYearEmployerSubmissions.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_tax_year_submissions_employer]
	@employerID int, 
	@taxYear int
AS

BEGIN
	SELECT 
	  er.[tax_year_employer_transmissionId]
      ,er.[tax_year]
      ,er.[employer_id]
      ,er.[ResourceId]
      ,er.[TransmissionType]
      ,er.[UniqueTransmissionId]
      ,er.[ReceiptId]
      ,er.[UniqueSubmissionId]
      ,er.[transmission_status_code_id]
      ,er.[OriginalReceiptId]
      ,er.[OriginalUniqueSubmissionId]
      ,er.[EntityStatusId]
      ,er.[CreatedBy]
      ,er.[CreatedDate]
      ,er.[ModifiedBy]
      ,er.[ModifiedDate]
      ,er.[BulkFile]
      ,er.[ManifestFile]
      ,er.[AckFile]
	  ,ee.form_count
	FROM [aca].[dbo].[tax_year_employer_transmission] er
	inner join
	(SELECT [tax_year_employer_transmissionId], count([employee_id]) as form_count
  FROM [aca].[dbo].[tax_year_employee_transmission] group by [tax_year_employer_transmissionId]) ee
	on ee.[tax_year_employer_transmissionId] = er.[tax_year_employer_transmissionId]
	WHERE er.employer_id = @employerID and er.tax_year=@taxYear
	;
END










































