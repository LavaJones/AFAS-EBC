USE [aca-demo]
ALTER TABLE dbo.IRSHoldingImport ADD FEIN NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN employee_id
ALTER TABLE dbo.IRSHoldingImport ADD EmployeeId NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN first_name
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN middle_name
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN last_name
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN name_suffix
ALTER TABLE dbo.IRSHoldingImport ADD Name NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD HireDate NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD TerminationDate NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD GettingA1095 NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN years
ALTER TABLE dbo.IRSHoldingImport ADD [Year] NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD [Month] NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN offer_of_coverage_code
ALTER TABLE dbo.IRSHoldingImport ADD Line14OfferOfCoverageCode NVARCHAR(50) NULL
ALTER TABLE dbo.IRSHoldingImport ADD Line15Premium NVARCHAR(50) NULL
ALTER TABLE dbo.IRSHoldingImport ADD Line16SafeHarborCode NVARCHAR(50) NULL
ALTER TABLE dbo.IRSHoldingImport ADD [Measured Monthly Hours] NVARCHAR(50) NULL
ALTER TABLE dbo.IRSHoldingImport ADD [Measured ACA Status] NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD [HasAnOfferThisMonth] NVARCHAR(50) NULL
ALTER TABLE dbo.IRSHoldingImport ADD ShowsEnrolledThisMonth NVARCHAR(50) NULL
ALTER TABLE dbo.IRSHoldingImport ADD [Employee #] NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD [HR Status Code] NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD [HR Status Description] NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD [ACA Status] NVARCHAR(50) NOT NUll
ALTER TABLE dbo.IRSHoldingImport ADD [Employee Class] NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport ADD [Employee Type] NVARCHAR(50) NOT NULL
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN [address]
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN [city]
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN [state_code]
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN [zipcode]
ALTER TABLE dbo.IRSHoldingIMport DROP COLUMN [dob]
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN [dob1]
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN months
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN monthly_hours
ALTER TABLE dbo.IRSHoldingImport DROP COLUMN status_description
GO
/****** Object:  Trigger [dbo].[UpdatingInfo]    Script Date: 2/9/2017 9:33:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdatingInfo]
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
	FROM [air-demo].emp.monthly_status ms 
				INNER JOIN dbo.IRSHoldingImport ihi 
					ON ms.status_description = ihi.[Measured ACA Status]



	UPDATE 
		[air-demo].appr.employee_monthly_detail 
			SET offer_of_coverage_code = CAST(imp.Line14OfferOfCoverageCode AS varchar(50)), 
			safe_harbor_code = imp.Line16SafeHarborCode, 
			share_lowest_cost_monthly_premium = CAST(imp.Line15Premium AS int),
			create_date = GETDATE(), 
			modified_date = GETDATE(), 
			modified_by = 'IRS Import'
	FROM 
		[air-demo].appr.employee_monthly_detail emd 
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
								INNER JOIN [air-demo].gen.[month] m
									ON ihi.[month] = m.month_id
								INNER JOIN [air-demo].gen.time_frame tf 
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