USE aca
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 04/20/2017
-- Description:	Returns number of employees that have been corrected but not transmitted
-- =============================================
CREATE PROCEDURE SELECT_NotTransmitCountPerEmployer
	-- Add the parameters for the stored procedure here
	@taxYear int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		e.employer_id, 
		e.name, 
		COUNT(c.Transmitted) as TransmitCount
	FROM employer e INNER JOIN tax_year_1095c_correction c ON e.employer_id = c.employer_id
	WHERE c.tax_year = @taxYear 
		AND c.Transmitted = 0 
		AND c.Corrected = 1
	GROUP BY e.[employer_id], e.[name] 
	ORDER BY TransmitCount desc

END
GO

GRANT EXECUTE ON [dbo].[SELECT_NotTransmitCountPerEmployer]  TO [aca-user] AS [dbo]
GO