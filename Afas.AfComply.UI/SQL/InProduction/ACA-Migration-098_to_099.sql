USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_other_ale_group_members] 
	@employerId int
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		oagm.name AS OtherALEBusinessNameLine1Txt,
		oagm.ein AS OtherALEEIN
	FROM
		[dbo].[other_ale_group_members] oagm
		-- hard coded since the proc does not pass it. gc5
		INNER JOIN [dbo].[tax_year_approval] tya ON (oagm.employer_id = tya.employer_id AND tya.tax_year = 2016)
	WHERE
		-- they have to marked as an ale.
		tya.ale = 1
			AND
		oagm.[employer_id] = @employerId
			AND 
		oagm.[EntityStatusId] = 1
	ORDER BY
		oagm.[Ale_Member_Id]

END

GO


