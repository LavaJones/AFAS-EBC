USE [aca]
GO

/****** Object:  View [dbo].[View_1094Part2]    Script Date: 3/14/2018 4:02:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






ALTER VIEW [dbo].[View_1094Part2]
AS
SELECT
	tya.employer_id,
	tya.tax_year,
	tya.ale AS [IsAle],
	c.EmployeeCount AS Form1095Count
FROM (SELECT b.name, COUNT(a.employeeId) AS EmployeeCount, b.ein, b.employer_id
from (
select distinct a.employerID, a.employeeId FROM dbo.Approved1095Final a 
	   INNER JOIN dbo.Approved1095FinalPart2 c ON a.Approved1095FinalId = c.Approved1095Final_ID
	   left join dbo.Approved1095FinalPart3 d on a.Approved1095FinalId = d.Approved1095Final_ID
WHERE (c.Receiving1095C = 1 or d.Approved1095Final_ID is not null) and c.EntityStatusId = 1 and a.EntityStatusId = 1
)as a INNER JOIN dbo.employer b ON a.EmployerId = b.employer_id
GROUP BY b.name, b.ein, b.employer_id
) as c 
	INNER JOIN [dbo].[tax_year_approval] tya ON (c.employer_id = tya.employer_id)
--WHERE
--	a1fp2.EntityStatusId = 1
--		AND
--	(
--		(a1fp2.MonthId = 0 AND a1fp2.Receiving1095C = 1)
--			OR
--		(a1fp2.MonthId <> 0 AND a1fp2.Receiving1095C = 1)
--	)
GROUP BY
	 tya.ale,tya.tax_year, c.EmployeeCount, tya.employer_id, tya.tax_year






GO


