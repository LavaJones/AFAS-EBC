USE [aca]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_employers_in_transmission_status_for_tax_year]
      @taxYearId int,
      @transmissionStatusId int,
      @entityStatusId int = 1
AS
BEGIN TRY

      SET NOCOUNT ON;

      DECLARE @execstatementsbatch nvarchar(max) = '';

      DECLARE @currentEmployerTaxYearTransmissionStatuses TABLE(
			EmployerId int,
            EmployerTaxYearTransmissionStatusId int,
            EmployerTaxYearTransmissionId int,
            TransmissionStatusId int,
            ResourceId uniqueidentifier,
            EntityStatusId int,
            StartDate datetime2,
            EndDate datetime2,
            CreatedBy nvarchar(50),
            CreatedDate datetime2,
            ModifiedBy nvarchar(50),
            ModifiedDate datetime2
      )

      SELECT @execstatementsbatch += 'Exec SELECT_current_employer_tax_year_transmission_status ' + CAST(employer_id AS varchar)  + ', ' + CAST(@taxYearId AS varchar)+ '; '
      FROM employer

      print(@execstatementsbatch)

      Insert into @currentEmployerTaxYearTransmissionStatuses EXEC(@execstatementsbatch)

      SELECT
            cetyt.EmployerTaxYearTransmissionId,
            e.employer_id,
            e.ResourceId,
            e.name,
            e.[address],
            e.city,
            s.abbreviation AS 'state',
            e.zip,
            e.bill_address,
            e.bill_city,
            e.bill_state,
            e.bill_zip
      FROM employer AS e 
            INNER JOIN [state] as s ON e.state_id = s.state_id
            INNER JOIN (
                SELECT
                    etyt.EmployerId, 
                    etyt.EmployerTaxYearTransmissionId
                FROM @currentEmployerTaxYearTransmissionStatuses as cetyts 
                    INNER JOIN EmployerTaxYearTransmission AS etyt 
				ON cetyts.EmployerTaxYearTransmissionId = etyt.EmployerTaxYearTransmissionId
                WHERE TransmissionStatusId = @transmissionStatusId
            ) AS cetyt ON e.employer_id = cetyt.EmployerId

END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
