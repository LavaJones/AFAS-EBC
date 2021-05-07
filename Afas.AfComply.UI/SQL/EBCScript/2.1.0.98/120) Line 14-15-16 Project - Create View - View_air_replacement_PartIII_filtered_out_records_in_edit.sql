USE [aca]
GO

/****** Object:  View [dbo].[View_air_replacement_PartIII_filtered_out_records_in_editable_table]    Script Date: 12/12/2017 12:15:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_air_replacement_PartIII_filtered_out_records_in_editable_table]
AS
SELECT partIII.tax_year, partIII.employee_id, dbo.employee.employer_id, partIII.dependent_id, partIII.jan, partIII.feb, partIII.mar, partIII.apr, partIII.may, partIII.jun, partIII.jul, partIII.aug, 
                  partIII.sep, partIII.oct, partIII.nov, partIII.dec
FROM     dbo.View_air_replacement_PartIII AS partIII LEFT OUTER JOIN
                  dbo.employee ON partIII.employee_id = dbo.employee.employee_id
WHERE  (NOT EXISTS
                      (SELECT tax_year, employee_id, dependent_id
                       FROM      dbo.insurance_coverage_editable AS e
                       WHERE   (EntityStatusID = 1) AND (partIII.employee_id = employee_id) AND (partIII.tax_year = tax_year) AND (partIII.dependent_id = dependent_id) OR
                                         (EntityStatusID = 1) AND (partIII.employee_id = employee_id) AND (partIII.tax_year = tax_year) AND (partIII.dependent_id IS NULL) AND (dependent_id IS NULL)))

GO

