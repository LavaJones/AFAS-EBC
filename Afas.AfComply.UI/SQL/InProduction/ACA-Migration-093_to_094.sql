USE aca
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 03/21/2016
-- Description:	selects the transmission status before halt
-- =============================================
CREATE PROCEDURE SELECT_transmission_status_before_halt
	-- Add the parameters for the stored procedure here
	@employerId int,
	@taxYearId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1
		etyts.*
	FROM EmployerTaxYearTransmission AS etyt 
		INNER JOIN EmployerTaxYearTransmissionStatus AS etyts ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
		INNER JOIN (SELECT 
						t.EmployerId,
						MAX(s.CreatedDate) AS MaxCreatedDate 
					FROM EmployerTaxYearTransmission t 
						INNER JOIN EmployerTaxYearTransmissionStatus s ON s.EmployerTaxYearTransmissionId = t.EmployerTaxYearTransmissionId
					GROUP BY t.EmployerId
					) m ON m.EmployerId = etyt.EmployerId
	WHERE etyt.EmployerId = @employerId
		AND etyt.TaxYearId = @taxYearId
		AND etyts.EntityStatusId = 1 
		AND etyts.CreatedDate <> m.MaxCreatedDate
	ORDER BY etyts.EmployerTaxYearTransmissionStatusId DESC

END
GO
