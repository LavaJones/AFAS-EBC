USE aca
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 04/19/2017
-- Description:	Returns the current IRS Status of an employer for a given year
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_current_employer_tax_year_irs_status] 
	-- Add the parameters for the stored procedure here
	@employerId int,
	@taxYearId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT 
		ale_e.employer_id,
		ale_e.ein,
		ale_e.name,
		sr.receipt_id,
		sr.return_time_local,
		sc.status_code
	FROM air.ale.employer ale_e 
		INNER JOIN air.br.manifest m ON ale_e.ein = m.ein
		INNER JOIN air.sr.status_request sr ON sr.header_id = m.header_id
		INNER JOIN air.sr.status_code sc ON sc.status_code_id = sr.status_code_id
	WHERE ale_e.employer_id = @employerId
		AND YEAR(sr.return_time_local) = @taxYearId
	ORDER BY return_time_local

END
GO

GRANT EXECUTE ON [dbo].[SELECT_current_employer_tax_year_irs_status]  TO [aca-user] AS [dbo]
GO
