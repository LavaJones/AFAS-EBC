USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_1094Part2]
AS
SELECT
	ROW_NUMBER() OVER (ORDER BY er.employer_id, tya.ale) AS [ApprovalId],
	er.employer_id AS [EmployerId],
	tya.ale AS [IsAle],
	1 AS [EntityStatusId],
	'SYSTEM' AS [CreatedBy],
	GETDATE() AS [CreatedDate],
	'SYSTEM' AS [ModifiedBy],
	GETDATE() AS [ModifiedDate],
	NEWID() AS [ResourceId],
	COUNT(DISTINCT a1fp2.employeeID) AS Form1095Count
FROM	
	[dbo].[Approved1095FinalPart2] a1fp2
	INNER JOIN [dbo].[employee] ee ON (a1fp2.employeeID = ee.employee_id)
	INNER JOIN [dbo].[employer] er ON (ee.employer_id = er.employer_id)
	INNER JOIN [dbo].[tax_year_approval] tya ON (er.employer_id = tya.employer_id)
WHERE
	a1fp2.EntityStatusId = 1
		AND
	(
		(a1fp2.MonthId = 0 AND a1fp2.Receiving1095C = 1)
			OR
		(a1fp2.MonthId <> 0 AND a1fp2.Receiving1095C = 1)
	)
GROUP BY
	er.employer_id, tya.ale

GO


