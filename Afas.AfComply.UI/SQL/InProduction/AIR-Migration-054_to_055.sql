USE [air]
GO

ALTER TABLE [emp].[employee] DROP CONSTRAINT [FK_employee_employer]
GO

ALTER TABLE [emp].[employee] WITH CHECK ADD  CONSTRAINT [FK_employee_employer] FOREIGN KEY([employer_id]) REFERENCES [ale].[employer] ([employer_id])
GO

ALTER TABLE [emp].[employee] CHECK CONSTRAINT [FK_employee_employer]
GO

ALTER TABLE [emp].[covered_individual] DROP CONSTRAINT [FK_covered_individual_employee]
GO

ALTER TABLE [emp].[covered_individual]  WITH CHECK ADD  CONSTRAINT [FK_covered_individual_employee] FOREIGN KEY([employee_id]) REFERENCES [emp].[employee] ([employee_id])
GO

ALTER TABLE [emp].[covered_individual] CHECK CONSTRAINT [FK_covered_individual_employee]
GO

ALTER TABLE [emp].[covered_individual] DROP CONSTRAINT [CheckSSNDobBothNull]
GO

ALTER TABLE [emp].[covered_individual] DROP CONSTRAINT [CK_emp_CI_employer_id]
GO

ALTER TABLE [emp].[covered_individual_monthly_detail] DROP CONSTRAINT [FK_covered_individual_monthly_covered_individual]
GO

ALTER TABLE [emp].[covered_individual_monthly_detail]  WITH CHECK ADD  CONSTRAINT [FK_covered_individual_monthly_covered_individual] FOREIGN KEY([covered_individual_id]) REFERENCES [emp].[covered_individual] ([covered_individual_id])
GO

ALTER TABLE [emp].[covered_individual_monthly_detail] CHECK CONSTRAINT [FK_covered_individual_monthly_covered_individual]
GO

ALTER TABLE [emp].[covered_individual_monthly_detail] DROP CONSTRAINT [FK_covered_individual_monthly_time_frame]
GO

ALTER TABLE [emp].[covered_individual_monthly_detail]  WITH CHECK ADD  CONSTRAINT [FK_covered_individual_monthly_time_frame] FOREIGN KEY([time_frame_id]) REFERENCES [gen].[time_frame] ([time_frame_id])
GO

ALTER TABLE [emp].[covered_individual_monthly_detail] CHECK CONSTRAINT [FK_covered_individual_monthly_time_frame]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/17/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_covered_individuals]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

PRINT '5a: Delete Covered Individuals Monthly Details'
DELETE air.emp.covered_individual_monthly_detail 
FROM
	air.emp.covered_individual_monthly_detail cimd
	INNER JOIN air.emp.covered_individual ci ON (cimd.covered_individual_id = ci.covered_individual_id)
	INNER JOIN air.emp.employee ee ON (ci.employee_id = ee.employee_id)
WHERE
	(ee.employer_id = @employer_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

PRINT '5b: Delete Covered Individuals'
DELETE air.emp.covered_individual 
FROM
	air.emp.covered_individual ci 
	INNER JOIN air.emp.employee ee ON (ci.employee_id = ee.employee_id)
WHERE
	(ee.employer_id = @employer_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

-- ______________________________________________________________________________________________________________________________________________________
PRINT '5c: Insert Covered Individuals'
INSERT INTO air.emp.covered_individual (
	covered_individual_id, 
	employee_id, 
	employer_id, 
	first_name, 
	middle_name, 
	last_name, 
	ssn, 
	birth_date
)
SELECT DISTINCT
	first_row_id,
	employee_id,
	employer_id,
	UPPER(fName), 
	UPPER(mName), 
	UPPER(lName), 
	ssn,
	dob
FROM
	aca.dbo.ufnGetEmployeeInsurance(@year_id) ei
WHERE
	(employer_id = @employer_id)
		AND
	(ISNULL(ssn, dob) IS NOT NULL)	-- not liking this statement at all, leaving for the time being. gc5
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000))
-- ______________________________________________________________________________________________________________________________________________________
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/17/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_covered_individuals_monthly_detail]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--None presently
-- ______________________________________________________________________________________________________________________________________________________
PRINT '6: Delete Covered Individual Monthly Detail'
DELETE air.emp.covered_individual_monthly_detail 
FROM
	air.emp.covered_individual_monthly_detail cim
	INNER JOIN air.emp.covered_individual ci ON (cim.covered_individual_id = ci.covered_individual_id) 
	INNER JOIN air.emp.employee ee ON (ci.employee_id = ee.employee_id)
WHERE
	(time_frame_id IN(SELECT time_frame_id FROM air.gen.time_frame WHERE year_id = @year_id))
		AND
	(ee.employer_id = @employer_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '6: Insert Covered Individual Monthly Detail';
WITH
	insurance_coverage AS
	(
		SELECT
			first_row_id,
			employee_id,
			dependent_id,
			tax_year,
			[month],
			covered_indicator
		FROM (
				SELECT
					ic.first_row_id,
					ic.employee_id,
					dependent_id,
					tax_year,
					jan,
					feb,
					mar,
					apr,
					may,
					jun,
					jul,
					aug,
					sept,
					oct,
					nov,
					[dec]
				FROM
					aca.dbo.ufnGetEmployeeInsurance(@year_id) ic
					INNER JOIN aca.dbo.employee ee ON (ic.employee_id = ee.employee_id)
				WHERE
					(ee.employer_id = @employer_id)
						AND
					-- note: this will break when there is more than 10 million lives in the database.
					(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000))
			) imc
		UNPIVOT (
			covered_indicator FOR [month] IN 
				(jan, feb, mar, apr, may, jun, jul, aug, sept, oct, nov, [dec])
		) AS unpvt
	)

INSERT INTO air.emp.covered_individual_monthly_detail (
		covered_individual_id,
		time_frame_id,
		covered_indicator
	) 
SELECT
	first_row_id,
	time_frame_id,
	covered_indicator
FROM
	insurance_coverage ic
	INNER JOIN air.gen.[month] m ON (SUBSTRING(ic.[month],1,3) = SUBSTRING(m.name,1,3))
	INNER JOIN air.gen.time_frame t ON (ic.tax_year = t.year_id) AND (m.month_id = t.month_id)
WHERE
	first_row_id IN (
			SELECT DISTINCT covered_individual_id FROM air.emp.covered_individual
		);

-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

PRINT '6: Update Covered Individual Annual';
UPDATE emp.covered_individual
SET
	annual_coverage_indicator = cim.annual_coverage_indicator 
FROM
	emp.covered_individual ci
	INNER JOIN  air.emp.employee ee ON (ci.employee_id = ee.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				cim.covered_individual_id,
				MAX(CAST(cim.covered_indicator AS INT)) AS annual_coverage_indicator
			FROM
				air.emp.covered_individual_monthly_detail cim
			GROUP BY
				cim.covered_individual_id
			HAVING
				(COUNT(cim.covered_indicator) = 12)
					AND
				(COUNT(DISTINCT cim.covered_indicator) = 1)
		) cim ON (ci.covered_individual_id = cim.covered_individual_id)
WHERE
	(cim.annual_coverage_indicator = 1)
		AND
	(ee.employer_id = @employer_id) 
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ci.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/22/2015
-- Description:	Retired March 30th, no longer needed as it was incorrect.
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_ale_yearly_detail]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@aag_code TINYINT = 2,
@_4980H_transition_relief_indicator BIT = 1
-- ______________________________________________________________________________________________________________________________________________________
AS

PRINT 'Procedure is retired, no longer used.'

GO




