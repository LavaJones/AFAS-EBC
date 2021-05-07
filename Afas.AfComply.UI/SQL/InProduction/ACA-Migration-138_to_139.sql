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
CREATE PROCEDURE StageForCorrectionRetransmission
	-- Add the parameters for the stored procedure here
	 @employerID int,
	 @taxYearID int,
	 @modifiedBy varchar
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE aca.dbo.tax_year_1095c_correction
	SET Transmitted = 0,
		ModifiedBy = @modifiedBy,
		ModifiedDate = GETDATE()
	WHERE employer_id = @employerID
	AND tax_year = @taxYearID
	AND Corrected = 1

END
GO

GRANT EXECUTE ON [dbo].[StageForCorrectionRetransmission] TO [aca-user] AS [dbo]
GO
