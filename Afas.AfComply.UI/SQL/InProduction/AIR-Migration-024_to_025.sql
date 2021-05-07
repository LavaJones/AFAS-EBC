USE [air]
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

	-- clean everything out first.
	UPDATE air.appr.employee_yearly_detail
	SET
		submittal_ready = 0,
		_1095C = 0
	FROM
		air.appr.employee_yearly_detail yd WITH (NOLOCK)
	WHERE
		(yd.employer_id = @employer_id)
			AND
		(yd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- toggle back on the correct bits.
	UPDATE air.appr.employee_yearly_detail
	SET
		submittal_ready = 1,
		_1095C = 1
	FROM
		air.appr.employee_yearly_detail yd WITH (NOLOCK)
	WHERE
		yd.employee_id IN (
				SELECT DISTINCT
					employee_id
				FROM
					air.appr.employee_monthly_detail md WITH (NOLOCK)
				WHERE
					(
						(monthly_status_id = 1)
							OR
						(insurance_type_id = 2 AND enrolled = 1)
					)
						AND
					(md.employee_id = employee_id)
		)
			OR
		yd.employee_id IN (
			SELECT DISTINCT
				employee_id
			FROM
				air.appr.employee_monthly_detail WITH (NOLOCK)
			WHERE
				(enrolled = 0)
					AND
				(employee_id IN(SELECT DISTINCT employee_id FROM air.emp.covered_individual WITH (NOLOCK)))
		)
			AND 
		(yd.employer_id = @employer_id)
			AND
		(yd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO


