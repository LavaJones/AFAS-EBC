USE [aca]
GO

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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SELECT_employee_with_employer_info]
	@employerID int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH  empB AS
      (
		  SELECT 
			a.employee_id, 
			a.employer_id,
			c.ResourceId,
			a.first_name AS Fname, 
			a.middle_name AS Mi, 
			a.last_name AS Lname, 
			a.ssn,
			a.name_suffix AS Suffix, 
			a.[address] AS [Address],
			a.city AS City, 
			a.state_code AS [State], 
			a.zipcode AS ZIP,
			c.dob
		FROM air.emp.employee a INNER JOIN aca.dbo.employee c ON a.employee_id = c.employee_id 
		WHERE a.employer_id = @employerID
      )
      SELECT DISTINCT 
		B.employee_id,
	    e.ResourceId as EmployerResourceId,
		d.name as BusinessNameLine1,
		d.[address] as BusinessAddressLine1Txt, 
		d.city as BusinessCityNm, 
		d.state_code AS BusinessUSStateCd,
		d.zipcode AS BusinessUSZIPCd, 
		d.ein as EIN,
        d.contact_telephone AS ContactPhoneNum,
		B.ResourceId AS EmployeeResourceId,
		B.Fname,
		B.Mi,
		B.Lname,
		B.Suffix,
		B.City,
		B.[State],
		B.[Address],
		B.ZIP,
		B.ssn,
		B.dob as PersonBirthDt
      FROM empB B INNER JOIN air.ale.employer d ON d.employer_id = B.employer_id
				  INNER JOIN employer e ON e.employer_id = B.employer_id		
      ORDER BY b.Fname
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH

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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SELECT_tax_years
AS
BEGIN

	SET NOCOUNT ON;

	SELECT *
	FROM tax_year
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.SELECT_employer_transmission_statuses_by_tax_year
	@employerId int,
	@taxYearId int,
	@entityStatusId int = 1
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		etts.TransmissionStatusId,
		etts.StartDate,
		etts.EndDate
	FROM EmployerTaxYearTransmissionStatus etts INNER JOIN EmployerTaxYearTransmission ett 	 
	ON ett.EmployerTaxYearTransmissionId = etts.EmployerTaxYearTransmissionId
	WHERE ett.EmployerId = @employerId
		AND ett.TaxYearId = @taxYearId
	ORDER BY etts.StartDate

END
GO
