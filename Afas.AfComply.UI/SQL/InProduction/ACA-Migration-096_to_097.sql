USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_employee_covered_individuals]
	@employeeId int,
	@taxYearId int
AS
BEGIN TRY
	
	-- IRS routines. gc5

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovals
	SELECT DISTINCT
		ty1a.tax_year,
		ty1a.employer_id,
		ty1a.employee_id,
		ty1a.get1095C
	FROM
		[aca].[dbo].[tax_year_1095c_approval] ty1a
	WHERE
		ty1a.employee_id = @employeeId
			AND
		ty1a.tax_year = @taxYearId
			AND
		ty1a.get1095C = 1;

	-- pivot back to the employerId from the above table, should always be one entry for the employer. gc5
	DECLARE @employerId INT;
	SELECT
		@employerId = employer_id 
	FROM
		@SanitizedTaxYear1095CApprovals sty1a
	WHERE
		sty1a.employee_id = @employeeId;

	DECLARE @EmployeeCoveredIndividuals TABLE (
		employee_id int NOT NULL,
		covered_individual_id int NOT NULL,
		PersonFirstNm varchar(max),
		PersonMiddleNm varchar(max),
		PersonLastNm varchar(max),
		SuffixNm varchar(max),
		SSN varchar(max),
		DOB date,
		CoveredIndividualAnnualInd varchar(1),
		JanuaryInd varchar(1),
		FebruaryInd varchar(1),
		MarchInd varchar(1),
		AprilInd varchar(1),
		MayInd varchar(1),
		JuneInd varchar(1),
		JulyInd varchar(1),
		AugustInd varchar(1),
		SeptemberInd varchar(1),
		OctoberInd varchar(1),
		NovemberInd varchar(1),
		DecemberInd varchar(1)
	)

	-- grab all of the employees and dependents into the table, all bit flags are empty.
	-- we filter through the approval table for this insert then use the in memory table to drive all other updates.
	INSERT INTO @EmployeeCoveredIndividuals(
		employee_id,
		covered_individual_id,
		PersonFirstNm,
		PersonMiddleNm,
		PersonLastNm,
		SuffixNm,
		SSN,
		DOB,
		CoveredIndividualAnnualInd
	)
	SELECT DISTINCT-- distinct because the CoveredIndividualDetails UFN below returns one row per month.
		ci.employee_id,
		ci.covered_individual_id,
		ci.first_name,
		ci.middle_name,
		ci.last_name,
		ci.name_suffix,
		ci.ssn,
		ci.birth_date,
		'0' as CoveredIndividualAnnualInd	-- hard coded to null since the eyd table does not seem to update correctly.
	FROM
		[air].[emp].[employee] air_ee
		INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (ty1a.employee_id = air_ee.employee_id AND ty1a.tax_year = @taxYearId)
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(air_ee.employee_id = ci.employee_id)
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		air_ee.employee_id = @employeeId
			AND
		ty1a.get1095C = 1;

	-- work around the faulty annual coverage indicator.
	UPDATE @EmployeeCoveredIndividuals
	SET
		CoveredIndividualAnnualInd = '1'
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN (SELECT
						ci.covered_individual_id,
						SUM(CASE WHEN ci.covered_indicator = 1 THEN 1 ELSE 0 END) AS total_covered_indicator
					FROM
						[aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
					GROUP BY
						ci.covered_individual_id
			) ci ON (ci.covered_individual_id = ed.covered_individual_id)
	WHERE ci.total_covered_indicator = 12;

	--January values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		JanuaryInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 1;

	--Feburary values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		FebruaryInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 2;
	
	--March values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		MarchInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 3;

	--April values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		AprilInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 4;

	--May values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		MayInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 5;

	--Jun values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		JuneInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 6;

	--Jul values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		JulyInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 7;

	--Aug values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		AugustInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 8;

	--Sep values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		SeptemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 9;

	--Oct values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		OctoberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 10;

	--Nov values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		NovemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 11;

	--Dec values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		DecemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 12;

	SELECT
		ed.*
	FROM
		@EmployeeCoveredIndividuals ed;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/7/2016>
-- Description:	<This stored procedure is meant to return all LINE 3 info.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_SELECT_employee_LINE3_coverage]
	@employeeID int
AS

BEGIN

-- we need the employer id for the table function below, grab it now. gc5
DECLARE @employerId INT
SELECT
	@employerId = ee.[employer_id]
FROM
	[aca].[dbo].[employee] ee
WHERE
	ee.[employee_id] = @employeeID;

WITH
cim AS
(
	SELECT DISTINCT -- distinct because the UFN below returns 12 rows per person
		ci.covered_individual_id,
		time_frame_id,
		CAST(covered_indicator AS INT) AS COV_IND
	FROM
		-- hard coded to 2016 to line up with other hard codes. gc5
		[aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, 2016) ci
	WHERE
		ci.employee_id = @employeeID       
),
cim_pivoted AS
(
	SELECT
		-- 2016 values replacing the 2015 values. gc5  [13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24]
		covered_individual_id, [25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36]
	FROM
		cim
	PIVOT (
		-- 2016 values replacing the 2015 values. gc5 ([13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24])
		MAX(COV_IND) FOR time_frame_id IN ([25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36]) 
	) as cip
)
SELECT DISTINCT -- distinct since the UFN returns 12 rows per person.
	ci.employee_id,
	ci.first_name,
	ci.last_name,
	ci.ssn,
	ci.birth_date,
	cip.* 
FROM
	cim_pivoted cip
    INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, 2016) ci ON (cip.covered_individual_id = ci.covered_individual_id)
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_employee_tax_year_dependents]
	@employeeId int,
	@taxYearId int
AS
BEGIN TRY
	
	-- print routines. gc5

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	-- does not filter on tax_year since it is used in many places to fix issues. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovals
	SELECT DISTINCT
		ty1a.tax_year,
		ty1a.employer_id,
		ty1a.employee_id,
		ty1a.get1095C
	FROM
		[aca].[dbo].[tax_year_1095c_approval] ty1a
	WHERE
		ty1a.employee_id = @employeeId
			AND
		ty1a.tax_year = @taxYearId
			AND
		ty1a.get1095C = 1;

	-- pivot back to the employerId from the above table, should always be one entry for the employer. gc5
	DECLARE @employerId INT;
	SELECT
		@employerId = employer_id 
	FROM
		@SanitizedTaxYear1095CApprovals sty1a
	WHERE
		sty1a.employee_id = @employeeId

	-- in memory table to build up all of the answers.
	DECLARE @EmployeeDependents TABLE (
		employee_id int NOT NULL,
		covered_individual_id int,
		Fname varchar(max),
		Mi varchar(max),
		Lname varchar(max),
		ssn varchar(max),
		dob date,
		Jan bit,
		Feb bit,
		Mar bit,
		Apr bit,
		May bit,
		Jun bit,
		Jul bit,
		Aug bit,
		Sep bit,
		Oct bit,
		Nov bit,
		[Dec] bit
	)

	-- grab all of the employees and dependents into the table, all bit flags are empty.
	-- we filter through the approval table for this insert then use the in memory table to drive all other updates.
	INSERT INTO @EmployeeDependents (
		employee_id,
		covered_individual_id,
		Fname,
		Mi,
		Lname,
		ssn,
		dob,
		Jan,
		Feb,
		Mar,
		Apr,
		May,
		Jun,
		Jul,
		Aug,
		Sep,
		Oct,
		Nov,
		[Dec]
	)
	SELECT DISTINCT -- distinct because the ufn below returns 12 rows per person.
		ci.employee_id,
		ci.covered_individual_id,
		ci.first_name AS Fname,
		ci.middle_name AS Mi,
		ci.last_name AS Lname,
		ci.ssn,
		ci.birth_date AS dob,
		0 AS Jan,
		0 AS Feb,
		0 AS Mar,
		0 AS Apr,
		0 AS May,
		0 AS Jun,
		0 AS Jul,
		0 AS Aug,
		0 AS Sep,
		0 AS Oct,
		0 AS Nov,
		0 AS [Dec]
	FROM
		[air].[emp].[employee] air_ee
		INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (ty1a.employee_id = air_ee.employee_id AND ty1a.tax_year = @taxYearId)
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(air_ee.employee_id = ci.employee_id)
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		air_ee.employee_id = @employeeId
			AND
		ty1a.get1095C = 1

	-- January values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Jan = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 1

	-- February values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Feb = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 2
	
	-- March values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Mar = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 3

	-- April values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Apr = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 4

	-- May values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		May = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 5

	-- June values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Jun = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 6

	-- July values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Jul = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 7

	-- August values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Aug = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 8

	-- September values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Sep = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 9

	-- October values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Oct = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 10

	-- November values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Nov = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 11

	-- December values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		[Dec] = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 12

	SELECT
		ed.*
	FROM
		@EmployeeDependents ed

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

GRANT EXECUTE ON [dbo].[SELECT_transmission_status_before_halt] TO [aca-user] AS [DBO]
GO

