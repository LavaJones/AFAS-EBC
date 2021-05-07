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
CREATE PROCEDURE dbo.SELECT_TransmissionHold 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT c.name, c.ein , a.* FROM dbo.EmployerTaxYearTransmission a 
	INNER JOIN dbo.EmployerTaxYearTransmissionStatus b 
		ON a.EmployerTaxYearTransmissionId = b.EmployerTaxYearTransmissionId
	INNER JOIN dbo.employer c
		ON a.EmployerId = c.employer_id
	WHERE b.TransmissionStatusId = 14 and a.TaxYearId = 2017
END
GO
GRANT EXECUTE ON dbo.SELECT_TransmissionHold TO [aca-user]
GO
