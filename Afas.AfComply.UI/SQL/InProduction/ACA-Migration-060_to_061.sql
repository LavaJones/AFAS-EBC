USE [aca]
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
		INNER JOIN dbo.hr_status hr ON (hr.HR_status_id = eeb.HR_status_id)
		INNER JOIN dbo.employee_classification ec ON (ec.classification_id = eeb.classification_id)
		INNER JOIN dbo.employee_type et ON (et.employee_type_id = eeb.employee_type_id)
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

DROP TABLE [dbo].[IRSHoldingImport]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IRSHoldingImport] (
	[EmployerId] [nvarchar](50) NULL,
	[FEIN] [nvarchar](50) NOT NULL,
	[EmployeeId] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[HireDate] [nvarchar](50) NOT NULL,
	[TerminationDate] [nvarchar](50) NOT NULL,
	[GettingA1095] [nvarchar](50) NOT NULL,
	[Year] [nvarchar](50) NOT NULL,
	[Month] [nvarchar](50) NOT NULL,
	[Line14OfferOfCoverageCode] [nvarchar](50) NULL,
	[Line15Premium] [nvarchar](50) NULL,
	[Line16SafeHarborCode] [nvarchar](50) NULL,
	[Measured Monthly Hours] [nvarchar](50) NULL,
	[Measured ACA Status] [nvarchar](50) NOT NULL,
	[HasAnOfferThisMonth] [nvarchar](50) NULL,
	[ShowsEnrolledThisMonth] [nvarchar](50) NULL,
	[Employee #] [nvarchar](50) NOT NULL,
	[HR Status Code] [nvarchar](50) NOT NULL,
	[HR Status Description] [nvarchar](50) NOT NULL,
	[ACA Status] [nvarchar](50) NOT NULL,
	[Employee Class] [nvarchar](50) NOT NULL,
	[Employee Type] [nvarchar](50) NOT NULL,
	[DateOfBirth] [nvarchar](50) NULL
) ON [PRIMARY]

GO
CREATE TRIGGER [dbo].[UpdatingInfo]
ON [dbo].[IRSHoldingImport] AFTER INSERT
AS
BEGIN TRY

	SET NOCOUNT ON;

	 DECLARE @monthlyStatus TABLE(
		employee_id int,
		employer_id int,
		monthly_status_id varchar(50),
		status_id int);

	INSERT INTO 
		@monthlyStatus (employee_id, 
						employer_id, 
						monthly_status_id, 
						status_id)
	SELECT 
		ihi.EmployeeId, 
		ihi.EmployerId, 
		ihi.[Measured Monthly Hours], 
		ms.monthly_status_id 
	FROM air.emp.monthly_status ms 
				INNER JOIN dbo.IRSHoldingImport ihi 
					ON ms.status_description = ihi.[Measured ACA Status]

	UPDATE 
		air.appr.employee_monthly_detail 
			SET offer_of_coverage_code = CAST(imp.Line14OfferOfCoverageCode AS varchar(50)), 
			safe_harbor_code = imp.Line16SafeHarborCode, 
			share_lowest_cost_monthly_premium = CAST(imp.Line15Premium AS int),
			create_date = GETDATE(), 
			modified_date = GETDATE(), 
			modified_by = 'IRS Import'
	FROM 
		air.appr.employee_monthly_detail emd 
			INNER JOIN (SELECT DISTINCT 
							ihi.[month], 
							ihi.[year], 
							ihi.Line14OfferOfCoverageCode, 
							ihi.[ACA Status], 
							ihi.EmployeeId,
							ihi.Name, 
							ihi.EmployerId, 
							ihi.Line15Premium, 
							ihi.Line16SafeHarborCode, 
							tf.time_frame_id as monthly 
						FROM [dbo].[IRSHoldingImport] ihi 
								INNER JOIN air.gen.[month] m
									ON ihi.[month] = m.month_id
								INNER JOIN air.gen.time_frame tf 
									ON m.month_id = tf.month_id AND tf.year_id = ihi.[year]) imp 
			ON emd.employee_id = CAST(imp.EmployeeId AS int) 
				AND 
			   emd.employer_id = CAST(imp.EmployerId AS int) 
				AND 
			   imp.monthly = emd.time_frame_id
					INNER JOIN @monthlyStatus ms 
						ON ms.employee_id = CAST(imp.EmployeeId AS int) 
							AND 
						   ms.employer_id = CAST(imp.EmployerId AS int) 
	

	TRUNCATE TABLE dbo.IRSHoldingImport

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GRANT SELECT ON [dbo].[IRSHoldingImport] TO [aca-user] AS [dbo]
GO

GRANT UPDATE ON [dbo].[IRSHoldingImport] TO [aca-user] AS [dbo]
GO

GRANT DELETE ON [dbo].[IRSHoldingImport] TO [aca-user] AS [dbo]
GO

GRANT INSERT ON [dbo].[IRSHoldingImport] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[SELECT_employee_with_employer_info] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[SELECT_insurance_combined_by_employer] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[SELECT_employers_in_transmission_status_for_tax_year] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[SELECT_employer_transmission_statuses_by_tax_year] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[SELECT_combined_date_data_by_employer] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[SELECT_employee_offer_and_coverage] TO [aca-user] AS [dbo]
GO
