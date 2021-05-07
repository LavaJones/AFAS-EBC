USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [appr].[spUpdate_1095C_status_for_employee]
	@employer_id INT, 
	@year_id INT,
	@employee_id INT

AS

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
		(yd.employee_id = @employee_id);

GO

GRANT EXECUTE ON [appr].[spUpdate_1095C_status_for_employee] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [appr].[spUpdate_1095C_status_for_employee]
	@employer_id INT, 
	@year_id INT,
	@employee_id INT

AS

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
		(yd.employee_id = @employee_id);

	UPDATE air.appr.employee_yearly_detail
	SET
		submittal_ready = 1,
		_1095C = 1
	FROM
		air.appr.employee_yearly_detail yd 
	WHERE
		yd.employee_id IN (
				SELECT DISTINCT
					employee_id
				FROM
					air.appr.employee_monthly_detail md 
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
				air.appr.employee_monthly_detail 
			WHERE
				(enrolled = 0)
					AND
				(employee_id IN(SELECT DISTINCT employee_id FROM air.emp.covered_individual ))
		)
			AND 
		(yd.employer_id = @employer_id)
			AND
		(yd.year_id = @year_id)
			AND 
		(yd.employee_id = @employee_id);

GO
