USE [aca-demo]
GO

/****** Object:  Table [dbo].[IRSHoldingImport]    Script Date: 12/15/2016 10:03:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IRSHoldingImport]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[IRSHoldingImport](
	[months] [nvarchar](50) NULL,
	[years] [nvarchar](50) NULL,
	[offer_of_coverage_code] [nvarchar](50) NULL,
	[monthly_hours] [nvarchar](50) NULL,
	[employee_id] [nvarchar](50) NULL,
	[first_name] [nvarchar](50) NULL,
	[middle_name] [nvarchar](50) NULL,
	[last_name] [nvarchar](50) NULL,
	[name_suffix] [nvarchar](50) NULL,
	[address] [nvarchar](100) NULL,
	[city] [nvarchar](50) NULL,
	[state_code] [nvarchar](50) NULL,
	[zipcode] [nvarchar](50) NULL,
	[dob] [nvarchar](50) NULL,
	[status_description] [varchar](max) NULL,
	[dob1] [nvarchar](50) NULL,
	[EmployerId] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_EmployeeCount' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[SELECT_EmployeeCount] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[SELECT_EmployeeCount]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT COUNT(ee.[employee_id]) AS CountOfEmployees, er.[name] 
   FROM [dbo].[employee] AS ee 
	INNER JOIN [dbo].[employer] AS er 
		ON ee.[employer_id] = er.[employer_id] 
   WHERE ee.[terminationDate] is null
   GROUP BY er.[name]
END
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'spGetFieldForIRS' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[spGetFieldForIRS] AS SET NOCOUNT ON;')
GO
ALTER PROCEDURE [dbo].[spGetFieldForIRS]
	 @employerID int,
	 @taxYear int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
WITH empA AS
	(
	SELECT employee_id, ed.time_frame_id, employer_id, offer_of_coverage_code, monthly_status_id, monthly_hours
	FROM [air-demo].emp.monthly_detail ed INNER JOIN [air-demo].gen.time_frame tf 
		ON tf.time_frame_id = ed.time_frame_id
	WHERE employer_id = @employerID AND tf.year_id = @taxYear
	),
	empB AS
	(
	SELECT eea.employee_id, first_name, middle_name, last_name, name_suffix, eea.[address], 
		   eea.city, state_code, zipcode, eeb.dob
	FROM [air-demo].emp.employee eea INNER JOIN empA em 
		ON  eea.employee_id = em.employee_id 
	INNER JOIN dbo.employee eeb
		ON eea.employee_id = eeb.employee_id
	)
	SELECT  DISTINCT er.name as [Employer Name], er.ein, er.[address] as [Employer Address], 
		er.city as [Employer City], er.state_code AS [Employer Code], er.zipcode AS [Employer Zip], 
		er.contact_telephone AS [Employer Telephone], m.name as months, tf.year_id as years, 
		ema.offer_of_coverage_code, ema.monthly_hours, emb.employee_id, emb.first_name, emb.middle_name, 
		emb.last_name, emb.name_suffix, emb.[address], emb.city, emb.state_code, emb.zipcode, emb.dob, ms.status_description, emb.dob
	FROM empA ema INNER JOIN empB emb  
			ON ema.employee_id = emb.employee_id 
		INNER JOIN [air-demo].emp.monthly_status ms 
			ON ema.monthly_status_id = ms.monthly_status_id
		INNER JOIN [air-demo].ale.employer er 
			ON er.employer_id = ema.employer_id 
		INNER JOIN [air-demo].gen.time_frame tf 
			ON ema.time_frame_id = tf.time_frame_id
		INNER JOIN [air-demo].gen.[month] m 
			ON tf.month_id = m.month_id
	ORDER BY emb.first_name

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
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

	INSERT INTO @monthlyStatus (employee_id, employer_id, monthly_status_id, status_id)
	SELECT ihi.employee_id, ihi.EmployerId, ihi.monthly_hours, ms.monthly_status_id FROM [air-demo].emp.monthly_status ms 
					INNER JOIN dbo.IRSHoldingImport ihi ON ms.status_description = ihi.status_description


	UPDATE [air-demo].appr.employee_monthly_detail SET offer_of_coverage_code = imp.offer_of_coverage_code, 
				monthly_hours = imp.monthly_hours, monthly_status_id = ms.status_id,
				create_date = GETDATE(), modified_date = GETDATE(), modified_by = 'IRS Import'
	FROM [air-demo].appr.employee_monthly_detail emd 
		INNER JOIN (SELECT DISTINCT ihi.[months], ihi.years, ihi.offer_of_coverage_code, ihi.monthly_hours, ihi.employee_id,
									ihi.first_name, ihi.middle_name, ihi.last_name, ihi.name_suffix, 
									ihi.[address], ihi.city, ihi.state_code, ihi.zipcode, ihi.dob, ihi.status_description, 
									ihi.dob1, ihi.EmployerId,  tf.time_frame_id as monthly 
						FROM [dbo].[IRSHoldingImport] ihi 
						INNER JOIN [air-demo].gen.[month] m
							ON ihi.months = m.name 
						INNER JOIN [air-demo].gen.time_frame tf 
							ON m.month_id = tf.month_id AND tf.year_id = ihi.years) imp 
			ON emd.employee_id = CAST(imp.employee_id AS int) 
			AND emd.employer_id = CAST(imp.EmployerId AS int) 
			AND imp.monthly = emd.time_frame_id
		INNER JOIN @monthlyStatus ms 
			ON ms.employee_id = CAST(imp.employee_id AS int) 
			AND ms.employer_id = CAST(imp.EmployerId AS int) 
	

	TRUNCATE TABLE dbo.IRSHoldingImport

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
GRANT EXECUTE ON [dbo].[spGetFieldForIRS] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeCount] TO [aca-user] AS [dbo]
GO


