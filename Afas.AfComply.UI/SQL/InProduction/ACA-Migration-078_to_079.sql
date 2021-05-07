USE [aca]
GO
CREATE TYPE BULK_status AS TABLE (
	employeeId int,
	employerId int)

--GRANT SELECT ON BULK_status TO [aca-user] AS DBO
--GO
GRANT EXECUTE ON TYPE::dbo.BULK_status TO [aca-user] AS DBO 
Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.UPDATE_printedStatus 
	@printedStatus BULK_status readonly,
	@taxYear int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   UPDATE dbo.tax_year_1095c_approval
   SET printed = 1 
   FROM dbo.tax_year_1095c_approval tya 
			INNER JOIN 
		@printedStatus ps 
			ON (ps.employerId = tya.employer_id) 
				AND 
			   (ps.employeeId = tya.employee_id)
   WHERE tya.tax_year = @taxYear

END
GO

GRANT EXECUTE ON [dbo].[UPDATE_printedStatus] TO [aca-user] AS [dbo]
GO

