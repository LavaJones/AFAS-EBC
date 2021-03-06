USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_current_employers_tax_year_transmission_status]    Script Date: 2/3/2017 4:01:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SELECT_current_employers_tax_year_transmission_status]
      @taxYearId int,
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
			cetyt.TransmissionStatusId,
			cetyt.StartDate,
			cetyt.EndDate,
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
      FROM
            employer AS e 
            INNER JOIN [state] as s ON e.state_id = s.state_id
            INNER JOIN (
                SELECT
                        etyt.EmployerId, 
                        etyt.EmployerTaxYearTransmissionId,
						cetyts.TransmissionStatusId,
						cetyts.StartDate,
						cetyts.EndDate
                FROM @currentEmployerTaxYearTransmissionStatuses as cetyts 
					INNER JOIN EmployerTaxYearTransmission AS etyt ON cetyts.EmployerTaxYearTransmissionId = etyt.EmployerTaxYearTransmissionId
            ) AS cetyt ON e.employer_id = cetyt.EmployerId

END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
