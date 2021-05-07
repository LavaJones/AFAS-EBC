USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 10/29/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [emp].[spGet_employee_monthly_detail]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employee_id INT,
@year SMALLINT
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
SELECT
	e.employee_id,
	m.abbreviation AS [month],
	md.offer_of_coverage_code,
	md.share_lowest_cost_monthly_premium, 
	md.safe_harbor_code,
	safe_harbor_code,
	md.enrolled
FROM
	air.emp.employee e  (NOLOCK) 
	INNER JOIN air.emp.monthly_detail md (NOLOCK)  ON (e.employee_id = md.employee_id) 
	INNER JOIN air.gen.time_frame (NOLOCK) t ON (md.time_frame_id = t.time_frame_id)
	INNER JOIN air.gen.[month] (NOLOCK) m ON (t.month_id = m.month_id) 
WHERE
	(e.employee_id = @employee_id)
		AND
	(t.year_id = @year)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/18/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [emp].[spInsert_employee_1095C]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
 @employee_id INT, 
 @employer_id INT,
 @tax_year SMALLINT, 
 @first_name NVARCHAR(50), 
 @middle_name NVARCHAR(50) = NULL, 
 @last_name NVARCHAR(50), 
 @_1095C NVARCHAR(MAX)
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @Max_employee_1095C_id INT
-- ______________________________________________________________________________________________________________________________________________________
DELETE air.emp.employee_1095C
WHERE
	(employee_id = @employee_id)
		AND
	(tax_year = @tax_year);

SELECT
	@Max_employee_1095C_id = IIF(MAX(emp1095C_id) IS NULL, 0, MAX(emp1095C_id))
FROM air.emp.employee_1095C;

INSERT INTO air.emp.employee_1095C (
		emp1095C_id,
		employee_id,
		employer_id,
		tax_year,
		first_name,
		middle_name,
		last_name,
		_1095C
	)
VALUES (
		@Max_employee_1095C_id + 1,
		@employee_id,
		@employer_id,
		@tax_year,
		@first_name,
		@middle_name,
		@last_name,
		@_1095C
	);
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- alter date: 10/29/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [appr].[spGet_employee_monthly_detail]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employee_id INT,
@year SMALLINT
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
SELECT
	ee.employee_id,
	m.abbreviation AS [month],
	offer_of_coverage_code,
	share_lowest_cost_monthly_premium, 
	safe_harbor_code,
	IIF(enrolled=1,1,0) as enrolled
FROM
	air.emp.employee ee  
	INNER JOIN air.appr.employee_monthly_detail emd ON (ee.employee_id = emd.employee_id) 
	INNER JOIN air.gen.time_frame  t ON (emd.time_frame_id = t.time_frame_id)
	INNER JOIN air.gen.[month]  m ON (t.month_id = m.month_id) 
WHERE
	(ee.employee_id = @employee_id)
		AND
	(t.year_id = @year)

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/22/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [appr].[spGet_submittal_ready_employees]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@employee_id INT = NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
SELECT
	ee.employee_id,
	ee.first_name,
	ee.middle_name,
	ee.last_name,
	ee.name_suffix,
	eyd._1095C 
FROM
	air.emp.employee ee
	INNER JOIN air.appr.employee_yearly_detail eyd ON (ee.employee_id = eyd.employee_id)
WHERE
	(ee.employer_id = @employer_id)
		AND
	(eyd.submittal_ready = 1)
		AND
	(eyd._1095C = 1)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(ee.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 4/17/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [appr].[spUpdate_1095C_status]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters
@employer_id INT, 
@year_id INT,
@employee_id INT = NULL

-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
UPDATE air.appr.employee_yearly_detail
SET
	submittal_ready = 0,
	_1095C = 0
FROM
	air.appr.employee_yearly_detail yd
WHERE
	(yd.employer_id = @employer_id)
		AND
	(yd.year_id = @year_id)
		AND 
	-- note: this will break when there is more than 10 million lives in the database.
	(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

UPDATE air.appr.employee_yearly_detail
SET
	submittal_ready = 1,
	_1095C = 1
FROM
	air.appr.employee_yearly_detail yd
WHERE
	yd.employee_id IN (
			SELECT DISTINCT employee_id
			FROM air.appr.employee_monthly_detail md 
			WHERE
				((monthly_status_id = 1) OR (insurance_type_id = 2 AND enrolled = 1))
					AND
				(md.employee_id = employee_id)
	)
		OR
	yd.employee_id IN (
		SELECT DISTINCT employee_id
		FROM air.appr.employee_monthly_detail
		WHERE
			(enrolled = 0)
				AND
			(employee_id IN(SELECT DISTINCT employee_id FROM air.emp.covered_individual))
	)
		AND 
	(yd.employer_id = @employer_id)
		AND
	(yd.year_id = @year_id)
		AND 
	-- note: this will break when there is more than 10 million lives in the database.
	(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

GRANT UPDATE ON [appr].[employee_monthly_detail] TO [air-user]
GO

GRANT UPDATE ON [emp].[covered_individual] TO [air-user]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [etl].[spReset_ale_employer]
	@employer_id INT
AS
BEGIN

	SET NOCOUNT ON;

	PRINT '1*** Delete Emp Yearly Detail***'
	DELETE
	FROM air.emp.yearly_detail 
	WHERE employer_id = @employer_id
	PRINT '1*** End Delete Emp Yearly Detail***'
	PRINT ''
	PRINT ''

	PRINT '2*** Delete Appr Monthly Detail***'
	DELETE
	FROM air.appr.employee_monthly_detail
	WHERE employer_id = @employer_id
	PRINT '2*** End Delete Appr Monthly Detail***'
	PRINT ''
	PRINT ''

	PRINT '3*** Delete Ale Yearly Detail***'
	DELETE
	FROM air.ale.yearly_detail
	WHERE employer_id = @employer_id
	PRINT '3*** End Delete Ale Yearly Detail***'
	PRINT ''
	PRINT ''

	PRINT '4*** Delete Ale Monthly Detail***'
	DELETE air.ale.monthly_detail
	WHERE employer_id = @employer_id
	PRINT '4*** End Delete Ale Monthly Detail***'
	PRINT ''
	PRINT ''

	PRINT '5*** Delete Covered Individual Monthly Detail***'
	DELETE air.emp.covered_individual_monthly_detail 
	FROM air.emp.covered_individual_monthly_detail cim
		INNER JOIN air.emp.covered_individual ci ON (cim.covered_individual_id = ci.covered_individual_id) 
		INNER JOIN  air.emp.employee ee ON (ci.employee_id = ee.employee_id)
	WHERE (ee.employer_id = @employer_id)
	PRINT '5*** End Delete Covered Individual Monthly Detail***'
	PRINT ''
	PRINT ''

	PRINT '6*** Delete Covered Individual***'
	DELETE air.emp.covered_individual 
	FROM air.emp.covered_individual ci 
			INNER JOIN  air.emp.employee ee ON(ci.employee_id = ee.employee_id)
	WHERE (ee.employer_id = @employer_id)
	PRINT '6*** End Delete Covered Individual***'
	PRINT ''
	PRINT ''

	PRINT '7*** Delete Employee***'
	DELETE air.emp.employee  
	FROM aca.dbo.employee ee
		INNER JOIN air.ale.employer er ON (ee.employer_id = er.employer_id)
		INNER JOIN air.emp.employee em ON (ee.employer_id = em.employer_id) AND (ee.employee_id = em.employee_id)
		INNER JOIN aca.dbo.state s ON (ee.state_id = s.state_id)
	WHERE (ee.employer_id = @employer_id)
	PRINT '7*** End Delete Employee***'
	PRINT ''
	PRINT ''

	PRINT '8*** Delete Ale Dge***'
	DELETE air.ale.dge	
	FROM air.ale.dge d
		INNER JOIN air.ale.employer er ON(d.ein = er.dge_ein)	
		INNER JOIN aca.dbo.tax_year_approval tya ON (er.employer_id = tya.employer_id)
		INNER JOIN aca.dbo.[state] s ON (tya.state_id = s.state_id) 
	WHERE (tya.employer_id = @employer_id)
	PRINT '8*** End Delete Ale Dge***'
	PRINT ''
	PRINT ''

	PRINT '9*** Delete Ale Employer***'
    DELETE air.ale.employer
	FROM aca.dbo.employer er INNER JOIN air.ale.employer em ON (er.employer_id = em.employer_id)
	WHERE er.employer_id = @employer_id
	PRINT '9*** End Delete Ale Employer***'
	PRINT ''
	PRINT ''


END
GO
GRANT EXECUTE ON [etl].[spReset_ale_employer] TO [air-user] AS [dbo]
GO
