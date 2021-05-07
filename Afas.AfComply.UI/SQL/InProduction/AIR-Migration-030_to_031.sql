USE [air]
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
BEGIN

DECLARE @attempts tinyint
SET @attempts = 1
WHILE @attempts <= 3

	BEGIN

		BEGIN TRANSACTION

		BEGIN TRY

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
				(yd.employee_id = @employee_id);

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

			COMMIT

			BREAK

		END TRY

		BEGIN CATCH

			ROLLBACK

			-- if not one of the locks, throw the original error. gc5
			IF ERROR_NUMBER() NOT IN (1204, 1205, 1222 )
				THROW

			SET @attempts = @attempts + 1

			CONTINUE

		END CATCH

	END

END

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
BEGIN

DECLARE @attempts tinyint
SET @attempts = 1
WHILE @attempts <= 3

	BEGIN

		BEGIN TRANSACTION

		BEGIN TRY

			UPDATE air.appr.employee_yearly_detail
			SET
				submittal_ready = 0,
				_1095C = 0
			FROM
				air.appr.employee_yearly_detail yd WITH (NOLOCK)
			WHERE
				(yd.employee_id = @employee_id)
					AND 
				(yd.year_id = @year_id)
					AND
				(yd.employer_id = @employer_id);

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
						(
							employee_id IN (
								SELECT
									DISTINCT eci.employee_id
								FROM
									[air].[emp].[covered_individual] eci WITH (NOLOCK)
								WHERE
									eci.employer_id = @employer_id
							)
						)
				)
					AND 
				(yd.employer_id = @employer_id)
					AND
				(yd.year_id = @year_id)
					AND 
				(yd.employee_id = @employee_id);

			COMMIT

			BREAK

		END TRY

		BEGIN CATCH

			ROLLBACK

			-- if not one of the locks, throw the original error. gc5
			IF ERROR_NUMBER() NOT IN (1204, 1205, 1222 )
				THROW

			SET @attempts = @attempts + 1

			CONTINUE

		END CATCH

	END

END

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
BEGIN

	UPDATE air.appr.employee_yearly_detail
	SET
		submittal_ready = 0,
		_1095C = 0
	FROM
		[air].[appr].[employee_yearly_detail] yd
	WHERE
		(yd.employee_id = @employee_id)
			AND 
		(yd.year_id = @year_id)
			AND
		(yd.employer_id = @employer_id);

	UPDATE [air].[appr].[employee_yearly_detail]
	SET
		submittal_ready = 1,
		_1095C = 1
	FROM
		[air].[appr].[employee_yearly_detail] eyd
	WHERE
		eyd.[employee_id] IN (
			SELECT
				DISTINCT employee_id
			FROM
				[air].[appr].[employee_monthly_detail] emd
			WHERE
				(
					(emd.[monthly_status_id] = 1)
						OR
					(emd.[insurance_type_id] = 2 AND emd.[enrolled] = 1)
				)
					AND
				(emd.[employee_id] = @employee_id)
		)
			OR
		eyd.[employee_id] IN (
			SELECT DISTINCT
				emd.[employee_id]
			FROM
				[air].[appr].[employee_monthly_detail] emd
			WHERE
				(emd.[enrolled] = 0)
					AND
				(emd.[employee_id] IN (
						SELECT
							DISTINCT eci.[employee_id]
						FROM
							[air].[emp].[covered_individual] eci
						WHERE
							eci.[employer_id] = @employer_id
								AND
							eci.[employee_id] = @employee_id
					)
				)
		)
			AND 
		(eyd.[employer_id] = @employer_id)
			AND
		(eyd.[year_id] = @year_id)
			AND 
		(eyd.[employee_id] = @employee_id);

END

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
				(employee_id IN (
						SELECT DISTINCT eci.employee_id FROM air.emp.covered_individual eci WITH (NOLOCK) WHERE eci.employer_id = @employer_id
					)
				)
		)
			AND 
		(yd.employer_id = @employer_id)
			AND
		(yd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO



















GO


