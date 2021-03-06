USE [aca]
GO

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
	FROM air.emp.monthly_status ms 
				INNER JOIN dbo.IRSHoldingImport ihi 
					ON ms.status_description = ihi.[Measured ACA Status]

	UPDATE 
		air.appr.employee_monthly_detail 
			SET offer_of_coverage_code = CAST(imp.Line14OfferOfCoverageCode AS varchar(50)), 
			safe_harbor_code = imp.Line16SafeHarborCode, 
			share_lowest_cost_monthly_premium = CASE
													WHEN imp.Line15Premium = '' THEN NULL
													ELSE CAST(imp.Line15Premium AS int)
													END,
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
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SELECT_current_employer_tax_year_transmission_status]
      @employerId int = 0,
      @taxYearId int = 0,
      @entityStatusId int = 1,
      @employerResourceId uniqueidentifier = NULL
AS
BEGIN TRY

      SET NOCOUNT ON;
      
      DECLARE @employerTaxYearTransmissionStatusId INT = 0;
 
      IF @employerId > 0
      BEGIN

            SELECT TOP 1
                  @employerTaxYearTransmissionStatusId = etyts.EmployerTaxYearTransmissionStatusId
            FROM EmployerTaxYearTransmission AS etyt INNER JOIN EmployerTaxYearTransmissionStatus AS etyts 
            ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
            WHERE etyt.EmployerId = @employerId
                  AND etyt.TaxYearId = @taxYearId
                  AND etyts.EntityStatusId = @entityStatusId
                  AND etyts.EndDate IS NULL 

            IF @employerTaxYearTransmissionStatusId = 0
            BEGIN
                  SELECT TOP 1
                        @employerTaxYearTransmissionStatusId = etyts.EmployerTaxYearTransmissionStatusId
                  FROM EmployerTaxYearTransmission AS etyt INNER JOIN EmployerTaxYearTransmissionStatus AS etyts 
                  ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
                  WHERE etyt.EmployerId = @employerId
                        AND etyt.TaxYearId = @taxYearId
                        AND etyts.EntityStatusId = 1
                  ORDER BY etyts.EmployerTaxYearTransmissionStatusId DESC
            END

      END
      ELSE
      BEGIN
      
            SELECT TOP 1
                  @employerTaxYearTransmissionStatusId = etyts.EmployerTaxYearTransmissionStatusId
            FROM EmployerTaxYearTransmission AS etyt 
                  INNER JOIN EmployerTaxYearTransmissionStatus AS etyts ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
                  INNER JOIN (SELECT employer_id FROM employer WHERE ResourceId = @employerResourceId) AS e ON e.employer_id = etyt.EmployerId        
            WHERE etyt.TaxYearId = @taxYearId
                  AND etyts.EntityStatusId = @entityStatusId
                  AND etyts.EndDate IS NULL 

            IF @employerTaxYearTransmissionStatusId = 0
            BEGIN
                  SELECT TOP 1
                        @employerTaxYearTransmissionStatusId = etyts.EmployerTaxYearTransmissionStatusId
                  FROM EmployerTaxYearTransmission AS etyt 
                        INNER JOIN EmployerTaxYearTransmissionStatus AS etyts ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
                        INNER JOIN (SELECT employer_id FROM employer WHERE ResourceId = @employerResourceId) AS e ON e.employer_id = etyt.EmployerId
                  WHERE etyt.TaxYearId = @taxYearId
                        AND etyts.EntityStatusId = @entityStatusId
                  ORDER BY etyts.EmployerTaxYearTransmissionStatusId DESC
            END
                  
      END
      
      SELECT
            etyt.EmployerId,
            etyts.EmployerTaxYearTransmissionStatusId,
            etyts.EmployerTaxYearTransmissionId,
            etyts.TransmissionStatusId,
            etyts.ResourceId,
            etyts.EntityStatusId,
            etyts.StartDate,
            etyts.EndDate,
            etyts.CreatedBy,
            etyts.CreatedDate,
            etyts.ModifiedBy,
            etyts.ModifiedDate
      FROM EmployerTaxYearTransmission AS etyt 
            INNER JOIN EmployerTaxYearTransmissionStatus AS etyts ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
      WHERE EmployerTaxYearTransmissionStatusId = @employerTaxYearTransmissionStatusId

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
