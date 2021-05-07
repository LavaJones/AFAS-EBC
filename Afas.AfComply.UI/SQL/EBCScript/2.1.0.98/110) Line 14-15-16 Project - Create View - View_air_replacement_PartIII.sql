USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_PartIII]    Script Date: 12/12/2017 12:14:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_air_replacement_PartIII]
AS
SELECT tax_year, employee_id, dependent_id, MAX(CONVERT(int, all12)) AS all12, MAX(CONVERT(int, jan)) AS jan, MAX(CONVERT(int, feb)) AS feb, MAX(CONVERT(int, mar)) AS mar, 
                  MAX(CONVERT(int, apr)) AS apr, MAX(CONVERT(int, may)) AS may, MAX(CONVERT(int, jun)) AS jun, MAX(CONVERT(int, jul)) AS jul, MAX(CONVERT(int, aug)) AS aug, 
                  MAX(CONVERT(int, sep)) AS sep, MAX(CONVERT(int, oct)) AS oct, MAX(CONVERT(int, nov)) AS nov, MAX(CONVERT(int, dec)) AS dec
FROM     dbo.insurance_coverage
WHERE EntityStatusID=1
GROUP BY tax_year, employee_id, dependent_id


GO


