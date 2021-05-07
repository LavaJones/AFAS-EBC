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
CREATE PROCEDURE dbo.SELECT_employer_transmission_statuses_by_tax_year
	-- Add the parameters for the stored procedure here
	@employerId int,
	@taxYearId int,
	@entityStatusId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		etts.TransmissionStatusId,
		etts.StartDate,
		etts.EndDate
	FROM EmployerTaxYearTransmissionStatus etts INNER JOIN EmployerTaxYearTransmission ett 	 
	ON ett.EmployerTaxYearTransmissionId = etts.EmployerTaxYearTransmissionId
	WHERE ett.EmployerId = @employerId
		AND ett.TaxYearId = @taxYearId
	ORDER BY etts.StartDate

END
GO
