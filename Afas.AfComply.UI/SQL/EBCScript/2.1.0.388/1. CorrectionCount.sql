USE aca
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.SELECT_CorrectionCount
	@employerId as int,
	@taxYear as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT COUNT(Correction) as Corrections 
	FROM (SELECT [TaxYearEmployerTransmissionId] as Correction
			FROM [aca].[dbo].[TransmissionLinking]
			where EmployerId = @employerId and TaxYear = @taxYear
			GROUP BY TaxYearEmployerTransmissionId) as item
  
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
