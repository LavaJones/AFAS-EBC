USE [aca];
GO

ALTER TABLE [dbo].[employer] ADD IrsEnabled bit not null default 0;
GO


-- New Stored Proc to flip the bit
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UPDATE_employer_IRS_Enabled]
       @employerID int,
       @irsEnabled bit
AS
BEGIN TRY
       UPDATE 
			[dbo].[employer]
       SET 
			IrsEnabled = @irsEnabled
       WHERE
            employer_id = @employerID;
 
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO

-- Alter Existing stored procs
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[INSERT_new_employer]
      @name varchar(50),
      @add varchar(75),
      @city varchar(50),
      @stateID int,
      @zip varchar(15),
      @logo varchar(50),
      @b_add varchar(50),
      @b_city varchar(50),
      @b_stateID int,
      @b_zip varchar(15),
      @empTypeID int,
      @ein varchar(50),
      @dbaName varchar(100),
      @empid int OUTPUT
AS
 
BEGIN TRY
      INSERT INTO [employer](
            name,
            [address],
            city,
            state_id,
            zip,
            img_logo,
            bill_address,
            bill_city,
            bill_state,
            bill_zip,
            employer_type_id, 
            ein,
            DBAName,
			IrsEnabled)
      VALUES(
            @name,
            @add,
            @city,
            @stateID,
            @zip,
            @logo,
            @b_add,
            @b_city,
            @b_stateID,
            @b_zip,
            @empTypeID,
            @ein,
            @dbaName,
			0)
 
SELECT @empid = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/22/2014>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:		<removed select *>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer]
	@employerID varchar(100)
AS

BEGIN
	SELECT [employer_id]
      ,[name]
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[img_logo]
      ,[bill_address]
      ,[bill_city]
      ,[bill_state]
      ,[bill_zip]
      ,[employer_type_id]
      ,[ein]
      ,[initial_measurement_id]
      ,[import_demo]
      ,[import_payroll]
      ,[iei]
      ,[iec]
      ,[ftpei]
      ,[ftpec]
      ,[ipi]
      ,[ipc]
      ,[ftppi]
      ,[ftppc]
      ,[importProcess]
      ,[vendor_id]
      ,[autoUpload]
      ,[autoBill]
      ,[suBilled]
      ,[import_gp]
      ,[import_hr]
      ,[import_ec]
      ,[import_io]
      ,[import_ic]
      ,[import_pay_mod]
      ,[ResourceId]
      ,[DBAName]
      ,[IrsEnabled]
	FROM [employer]
	WHERE employer_id = @employerID;
END
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/16/2014>
-- Description:	<This stored procedure is meant to return all employers.>
-- Changes:		<removed select *>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_all_employers]
AS

BEGIN
	SELECT [employer_id]
      ,[name]
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[img_logo]
      ,[bill_address]
      ,[bill_city]
      ,[bill_state]
      ,[bill_zip]
      ,[employer_type_id]
      ,[ein]
      ,[initial_measurement_id]
      ,[import_demo]
      ,[import_payroll]
      ,[iei]
      ,[iec]
      ,[ftpei]
      ,[ftpec]
      ,[ipi]
      ,[ipc]
      ,[ftppi]
      ,[ftppc]
      ,[importProcess]
      ,[vendor_id]
      ,[autoUpload]
      ,[autoBill]
      ,[suBilled]
      ,[import_gp]
      ,[import_hr]
      ,[import_ec]
      ,[import_io]
      ,[import_ic]
      ,[import_pay_mod]
      ,[ResourceId]
      ,[DBAName]
      ,[IrsEnabled]
	FROM [employer]
	ORDER BY name;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:<Travis Wells>
-- Create date: <4/9/2016>
-- Description: <This function will transpose all insurance carriers rows for a single individual. If they have multiple rows, 
--the TRUE value will always show. >
-- EXAMPLE:
--          Jan   Feb   Mar   Apr   May   Jun   Jul   Aug   Sep   Oct   Nov   Dec
--Bob 1     1     1     0     0     0     1     1     1     1     1     1
--Bob 0     0     0     0     0     1     1     1     1     1     0     0
--____________________________________________________
--Bob 1     1     1     0     0     1     1     1     1     1     1     1     Final Results for AIR system.
-- =============================================
ALTER FUNCTION [dbo].[ufnGetEmployeeInsurance](@year_id SMALLINT)
RETURNS TABLE 
AS
RETURN 
SELECT
      row_id AS first_row_id,
      ic.employee_id,
      ee.employer_id, 
      ic.dependent_id, 
      tax_year,
      CASE WHEN ic.dependent_id IS NULL THEN ee.fName ELSE ed.fName end AS fName,
      CASE WHEN ic.dependent_id IS NULL THEN ee.mName ELSE ed.mName end AS mName,
      CASE WHEN ic.dependent_id IS NULL THEN ee.lName ELSE ed.lName end AS lName,
      CASE WHEN ic.dependent_id IS NULL THEN ee.ssn ELSE ed.ssn end AS ssn,
      CASE WHEN ic.dependent_id IS NULL THEN ee.dob ELSE ed.dob end AS dob,
      Jan,
      Feb,
      Mar,
      Apr,
      May,
      Jun,
      Jul,
      Aug,
      Sept,
      Oct,
      Nov,
      [Dec]
FROM  
      aca.dbo.insurance_coverage_editable ic
      INNER JOIN aca.dbo.employee ee ON (ic.employee_id = ee.employee_id)
      LEFT OUTER JOIN aca.dbo.employee_dependents ed ON (ic.dependent_id = ed.dependent_id)
WHERE
      [tax_year] = @year_id
GO

