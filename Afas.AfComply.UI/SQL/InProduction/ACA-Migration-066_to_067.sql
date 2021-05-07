USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/16/2016>
-- Description:	<This is meant to INSERT a new insurance coverage row.>	
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_new_editable_insurance_coverage]
	@employeeID int,
	@employerID int,
	@dependentID int,
	@taxYear int, 
	@jan bit, 
	@feb bit, 
	@mar bit, 
	@apr bit, 
	@may bit, 
	@jun bit, 
	@jul bit, 
	@aug bit, 
	@sep bit, 
	@oct bit, 
	@nov bit, 
	@dec bit
AS

BEGIN

DECLARE @attempts tinyint
SET @attempts = 1
WHILE @attempts <= 3

	BEGIN

		BEGIN TRANSACTION

		BEGIN TRY

			INSERT INTO [insurance_coverage_editable] (
					employee_id,
					employer_id,
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
				)
			VALUES(
					@employeeID,
					@employerID,
					@dependentID,
					@taxYear,
					@jan,
					@feb,
					@mar,
					@apr,
					@may,
					@jun,
					@jul,
					@aug,
					@sep,
					@oct,
					@nov,
					@dec
				)

			COMMIT

			BREAK

		END TRY

		BEGIN CATCH

			ROLLBACK

			EXEC [dbo].[INSERT_ErrorLogging]

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
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/17/2015>
-- Description:	<This stored procedure will DELETE an Employers Insurance Contribution from the database.>
-- =============================================
ALTER PROCEDURE [dbo].[DELETE_insurance_contribution]
	@contID int
AS
BEGIN

DECLARE @attempts tinyint
SET @attempts = 1
WHILE @attempts <= 3

	BEGIN

		BEGIN TRANSACTION

		BEGIN TRY

			DELETE [dbo].[insurance_contribution]
			WHERE
				ins_cont_id = @contID;

			COMMIT

			BREAK

		END TRY

		BEGIN CATCH

			ROLLBACK

			EXEC [dbo].[INSERT_ErrorLogging]

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
ALTER PROCEDURE [dbo].[spGetFieldForIRS]
	 @employerID int,
	 @taxYear int
AS
BEGIN TRY

	SET NOCOUNT ON;

WITH empA AS
	(
	SELECT
		ed.employer_id,
		ed.employee_id,
		ed.time_frame_id,
		eyd._1095C,
		ed.offer_of_coverage_code,
		ed.share_lowest_cost_monthly_premium,
		ed.safe_harbor_code,
		ed.monthly_status_id,
		ed.monthly_hours,
		ed.mec_offered,
		ed.enrolled
	FROM
		air.appr.employee_monthly_detail ed
		INNER JOIN air.gen.time_frame tf ON (tf.time_frame_id = ed.time_frame_id)
		INNER JOIN air.appr.employee_yearly_detail eyd ON (eyd.employee_id = ed.employee_id)
	WHERE
		ed.employer_id = @employerID
			AND
		tf.year_id = @taxYear
	),
	empB AS
	(
	SELECT
		eea.employee_id,
		eea.first_name,
		eea.middle_name,
		eea.last_name,
		eea.name_suffix,
		eeb.hireDate,
		eeb.terminationDate,
		eeb.dob,
		eeb.ext_emp_id,
		hr.ext_id AS HRStatusCode,
		hr.[name] AS HRStatusDescription,
		acas.[name] AS ACAStatus,
		ec.[description] AS EmployeeClass,
		et.[name] AS EmployeeType
	FROM
		air.emp.employee eea
		INNER JOIN empA em ON (eea.employee_id = em.employee_id) 
		INNER JOIN dbo.employee eeb ON (eea.employee_id = eeb.employee_id)
		INNER JOIN dbo.aca_status acas ON (acas.aca_status_id = eeb.aca_status_id)
		INNER JOIN dbo.employee_classification ec ON (ec.classification_id = eeb.classification_id)
		INNER JOIN dbo.employee_type et ON (et.employee_type_id = eeb.employee_type_id)
		LEFT JOIN dbo.hr_status hr ON (hr.HR_status_id = eeb.HR_status_id) -- this should always have a value, in practice it is missing more than we would expect. gc5
	)
	SELECT DISTINCT
		er.ein AS [FEIN],
		emb.employee_id AS [EmployeeId],
		ISNULL(emb.last_name, '') + ', ' + ISNULL(emb.first_name, '') + ' ' + ISNULL(emb.name_suffix, '') AS [Name],
		CONVERT(varchar(8), hireDate, 1) AS [HireDate],
		CONVERT(varchar(8), terminationDate, 1) AS [TerminationDate],
		CONVERT(varchar(8), dob, 1) AS [DateOfBirth],
		ema._1095C AS [GettingA1095],
		tf.year_id as [Year], 
		m.month_id as [Month],
		ema.offer_of_coverage_code AS [Line14OfferOfCoverageCode],
		ema.share_lowest_cost_monthly_premium AS [Line15Premium],
		ema.safe_harbor_code AS [Line16SafeHarborCode],
		ema.monthly_hours AS [Measured Monthly Hours],
		ms.status_description AS [Measured ACA Status],
		ema.mec_offered AS [HasAnOfferThisMonth],
		ema.enrolled AS [ShowsEnrolledThisMonth],
		emb.ext_emp_id AS [Employee #],
		HRStatusCode AS [HR Status Code],
		HRStatusDescription AS [HR Status Description],
		emb.ACAStatus AS [ACA Status],
		EmployeeClass AS [Employee Class],
		EmployeeType AS [Employee Type]
	FROM
		empA ema
		INNER JOIN empB emb ON (ema.employee_id = emb.employee_id) 
		INNER JOIN air.emp.monthly_status ms ON (ema.monthly_status_id = ms.monthly_status_id)
		INNER JOIN air.ale.employer er ON (er.employer_id = ema.employer_id) 
		INNER JOIN air.gen.time_frame tf ON (ema.time_frame_id = tf.time_frame_id)
		INNER JOIN air.gen.[month] m ON (tf.month_id = m.month_id)
	ORDER BY
		[Name], [Year], [Month]

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO
