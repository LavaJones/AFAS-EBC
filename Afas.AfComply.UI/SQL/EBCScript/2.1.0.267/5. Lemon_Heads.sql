USE [aca]
GO

/****** Object:  View [dbo].[View_1094Part4]    Script Date: 3/5/2018 11:01:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[View_1094Part4]
AS

SELECT
	oagm.Ale_Member_Id AS AleMemberId,
	oagm.employer_id AS EmployerId,
	oagm.ein AS Ein,
	oagm.name AS Name,
	oagm.ResourceId,
	oagm.CreatedBy,
	oagm.CreatedDate,
	oagm.ModifiedBy,
	oagm.ModifiedDate,
	oagm.EntityStatusId,
	tya.tax_year
FROM
	[dbo].[other_ale_group_members] oagm
	INNER JOIN [dbo].[tax_year_approval] tya ON
		(
			oagm.employer_id = tya.employer_id
				AND
			oagm.EntityStatusId = 1
				AND
			tya.ale = 1
		)




GO


