USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_PartIII_details]    Script Date: 12/12/2017 12:15:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[View_air_replacement_PartIII_details]
AS
SELECT tax_year, employee_id, employer_id, dependent_id, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec
FROM     dbo.View_air_replacement_PartIII_filtered_out_records_in_editable_table
UNION
SELECT tax_year, employee_id, employer_id, dependent_id, jan, feb, mar, apr, may, jun, jul, aug, sept, oct, nov, dec
FROM     dbo.insurance_coverage_editable WHERE EntityStatusID=1;


GO


