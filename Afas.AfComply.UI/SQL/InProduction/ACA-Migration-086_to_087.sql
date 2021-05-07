USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_other_ale_group_members]    Script Date: 3/15/2017 8:59:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_other_ale_group_members] 
	-- Add the parameters for the stored procedure here
	@employerId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		name AS OtherALEBusinessNameLine1Txt,
		ein AS OtherALEEIN
	FROM air.ale.other_ale_group_members
	WHERE employer_id = @employerId

END
GO 

GRANT EXECUTE ON [dbo].[SELECT_other_ale_group_members] TO [aca-user] AS [dbo] 
GO

