USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_current_employer_tax_year_transmission_status]    Script Date: 3/29/2017 11:20:27 AM ******/
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
