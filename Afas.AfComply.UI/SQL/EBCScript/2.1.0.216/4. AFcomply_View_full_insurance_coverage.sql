USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[View_full_insurance_coverage]
AS

WITH CTE_PartIII
(
	tax_year,
	employee_id,
	dependent_id,
	all12,
	jan,
	feb,
	mar, 
	apr,
	may,
	jun,
	jul,
	aug, 
	sep,
	oct,
	nov,
	[dec]
)
AS
(
	SELECT
		ic.tax_year,
		ic.employee_id,
		ic.dependent_id,
		MAX(CONVERT(int, ic.all12)) AS all12,
		MAX(CONVERT(int, ic.jan)) AS jan,
		MAX(CONVERT(int, ic.feb)) AS feb,
		MAX(CONVERT(int, ic.mar)) AS mar, 
		MAX(CONVERT(int, ic.apr)) AS apr,
		MAX(CONVERT(int, ic.may)) AS may,
		MAX(CONVERT(int, ic.jun)) AS jun,
		MAX(CONVERT(int, ic.jul)) AS jul,
		MAX(CONVERT(int, ic.aug)) AS aug, 
		MAX(CONVERT(int, ic.sep)) AS sep,
		MAX(CONVERT(int, ic.oct)) AS oct,
		MAX(CONVERT(int, ic.nov)) AS nov,
		MAX(CONVERT(int, ic.[dec])) AS [dec]
	FROM
		dbo.insurance_coverage ic
	WHERE
		(ic.EntityStatusID = 1) -- ensure active. gc5
	GROUP BY
		ic.tax_year,
		ic.employee_id,
		ic.dependent_id
)
,
CTE_PartIII_Not_Edited
(
	tax_year,
	employee_id,
	employer_id,
	dependent_id,
	jan,
	feb,
	mar,
	apr,
	may,
	jun,
	jul,
	aug, 
	sep,
	oct,
	nov,
	[dec]
)
AS
(
	SELECT
		partIII.tax_year,
		partIII.employee_id,
		ee.employer_id,
		partIII.dependent_id,
		partIII.jan,
		partIII.feb,
		partIII.mar,
		partIII.apr,
		partIII.may,
		partIII.jun,
		partIII.jul,
		partIII.aug, 
		partIII.sep,
		partIII.oct,
		partIII.nov,
		partIII.[dec]
	FROM
		CTE_PartIII AS partIII
		-- this used to be an left outer, should be an inner since the employee
		-- record is associated with every employee and every dependent. gc5
		INNER JOIN dbo.employee ee ON (partIII.employee_id = ee.employee_id)
	WHERE
		(
			NOT EXISTS
			(
				SELECT
					ice.tax_year,
					ice.employee_id,
					ice.dependent_id
				FROM
					dbo.insurance_coverage_editable AS ice
				WHERE
					(	-- active ice for dependent match. gc5
						-- active dependent check happens elsewehere. gc5
						(ice.EntityStatusID = 1)
							AND
						(partIII.employee_id = ice.employee_id)
							AND
						(partIII.tax_year = ice.tax_year)
							AND
						(partIII.dependent_id = ice.dependent_id)
					)
						OR
					( -- active ice for employee match. gc5
						(ice.EntityStatusID = 1)
							AND
						(partIII.employee_id = ice.employee_id)
							AND
						(partIII.tax_year = ice.tax_year)
							AND
						(partIII.dependent_id IS NULL)
							AND
						(ice.dependent_id IS NULL)
					)
			)
		)
)
,
CTE_PartIII_Details
(
	tax_year,
	employee_id,
	employer_id,
	dependent_id,
	jan,
	feb,
	mar,
	apr,
	may,
	jun,
	jul,
	aug,
	sep,
	oct,
	nov,
	[dec]
)
AS
(
	SELECT
		partiii_not_edited.tax_year,
		partiii_not_edited.employee_id,
		partiii_not_edited.employer_id,
		partiii_not_edited.dependent_id,
		partiii_not_edited.jan,
		partiii_not_edited.feb,
		partiii_not_edited.mar,
		partiii_not_edited.apr,
		partiii_not_edited.may,
		partiii_not_edited.jun,
		partiii_not_edited.jul,
		partiii_not_edited.aug,
		partiii_not_edited.sep,
		partiii_not_edited.oct,
		partiii_not_edited.nov,
		partiii_not_edited.[dec]
	FROM
		CTE_PartIII_Not_Edited AS partiii_not_edited
	UNION
	SELECT
		ice.tax_year,
		ice.employee_id,
		ice.employer_id,
		ice.dependent_id,
		ice.jan,
		ice.feb,
		ice.mar,
		ice.apr,
		ice.may,
		ice.jun,
		ice.jul,
		ice.aug,
		ice.sept,
		ice.oct,
		ice.nov,
		ice.[dec]
	FROM
		dbo.insurance_coverage_editable ice
	WHERE
		(ice.EntityStatusID = 1) -- active ice record. gc5
)
SELECT
	ROW_NUMBER() OVER (ORDER BY ee.employer_id DESC) AS RowId,
	ee.employer_id,
	ee.employee_id,
	ee.fName AS employeeFName,
	ee.mName AS employeeMName, 
    ee.lName AS employeeLName,
	ee.ssn AS employeeSsn,
	ee.dob AS employeeDob, 
    ee_deps.dependent_id,
	ee_deps.fName AS dependentFName,
	ee_deps.mName AS dependentMName, 
    ee_deps.lName AS dependentLName,
	ee_deps.ssn AS dependentSsn,
	ee_deps.dob AS dependentDob,
    partiii_details.tax_year, 
    partiii_details.jan,
	partiii_details.feb,
	partiii_details.mar,
	partiii_details.apr,
	partiii_details.may, 
    partiii_details.jun,
	partiii_details.jul,
	partiii_details.aug,
	partiii_details.sep,
	partiii_details.oct, 
	partiii_details.nov,
	partiii_details.[dec]
FROM
	CTE_PartIII_Details partiii_details
	-- was left outer join, converted to inner as every employee must exist. gc5
    INNER JOIN dbo.employee ee ON (partiii_details.employee_id = ee.employee_id)
    LEFT OUTER JOIN dbo.employee_dependents ee_deps ON (partiii_details.dependent_id = ee_deps.dependent_id)
WHERE
	(
		(partiii_details.dependent_id IS NULL) -- grabs the employees
			OR
		(ee_deps.EntityStatusID = 1) -- grabs active dependents
	)

GO


