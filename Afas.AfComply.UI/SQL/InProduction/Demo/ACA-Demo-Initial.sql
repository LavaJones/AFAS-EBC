USE [master]
GO

ALTER DATABASE [aca-demo] SET SINGLE_USER WITH ROLLBACK IMMEDIATE	
GO
ALTER DATABASE [aca-demo] SET  DISABLE_BROKER
GO
ALTER DATABASE [aca-demo] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [aca-demo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF )
GO
ALTER DATABASE [aca-demo] SET MULTI_USER
GO
ALTER DATABASE [aca-demo] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [aca-demo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [aca-demo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [aca-demo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [aca-demo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [aca-demo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [aca-demo] SET ARITHABORT OFF 
GO
ALTER DATABASE [aca-demo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [aca-demo] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [aca-demo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [aca-demo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [aca-demo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [aca-demo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [aca-demo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [aca-demo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [aca-demo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [aca-demo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [aca-demo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [aca-demo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [aca-demo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [aca-demo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [aca-demo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [aca-demo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [aca-demo] SET RECOVERY FULL 
GO
ALTER DATABASE [aca-demo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [aca-demo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [aca-demo] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'aca-demo', N'ON'
GO
USE [aca-demo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[ap_AIR_SELECT_employer_employee_ids]
	@employerID int
AS

BEGIN
	SELECT DISTINCT employee_id
	FROM [air-demo].appr.employee_monthly_detail
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/07/2014>
-- Description:	<This stored procedure is meant to ARCHIVE the employees plan year status.>
-- =============================================
CREATE PROCEDURE [dbo].[ARCHIVE_employee_plan_year]
	@employerID int,
	@employeeID int,
	@employeeTypeID int,
	@planYearID int,
	@planYearIDMeas int,
	@planYearIDStab int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN
	INSERT INTO archive_emp_planyear
	(
		employer_id,
		employee_id,
		employee_type_id,
		plan_year_id,
		plan_year_id_meas,
		plan_year_id_stab,
		modOn,
		modBy)
	VALUES(
		@employerID,
		@employeeID,
		@employeeTypeID,
		@planYearID,
		@planYearIDMeas,
		@planYearIDStab,
		@modOn,
		@modBy)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <04/23/2014>
-- Description:	<This stored procedure is meant to updaet a specific user.>
-- =============================================
CREATE PROCEDURE [dbo].[DEACTIVATE_user]
	@userID int
AS
BEGIN
	UPDATE [user]
	SET
		active=0
	WHERE
		[user_id] = @userID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure will DELETE an Employee Classification from the database.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_classification]
	@classID int
AS
BEGIN
DELETE employee_classification
WHERE classification_id=@classID;


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/29/2016>
-- Description:	<This stored procedure will DELETE an Employees Dependent.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_dependent]
	@employeeID int,
	@dependentID int
AS
BEGIN
DELETE employee_dependents
WHERE 
	employee_id=@employeeID AND
	dependent_id=@dependentID;


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/8/2015>
-- Description:	<This stored procedure will DELETE an Employee from the database. Only if the Employee ID is not used in other tables.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_employee]
	@employeeID int
AS
BEGIN
DELETE employee
WHERE employee_id=@employeeID;


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Jacob Turnbull>
-- Create date: <5/3/2016>
-- Description:	<This stored procedure will DELETE an Employee from the database. Only if the Employee ID is not used in other tables.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_employee_1095c_approval]
	@employeeID int,
	@employerID int
AS
BEGIN
DELETE tax_year_1095c_approval
WHERE employee_id=@employeeID AND employer_id = @employerID;


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <08/26/2014>
-- Description:	<This stored procedure is meant to delete all employee_import records matching the batch id.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_employee_import]
	@batchID int
AS
BEGIN
	DELETE FROM import_employee
	WHERE batchid=@batchID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <09/02/2014>
-- Description:	<This stored procedure is meant to delete a single employee_import record matching the row id.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_employee_import_row]
	@rowID int
AS
BEGIN
	DELETE FROM import_employee
	WHERE rowid=@rowID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/29/2015>
-- Description:	<This stored procedure is meant to delete all employer demographic alerts.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_employer_demographic_alerts]
	@empID int, 
	@modBy varchar(50), 
	@modOn datetime
AS
BEGIN
	DECLARE @rowCount int;
	SET @rowCount=0;
	DECLARE @alertID int;
	SET @alertID = 2;

	DELETE FROM import_employee
	WHERE employerid=@empID;

	SET @rowCount = @@ROWCOUNT;

	INSERT INTO alert_archive
	VALUES(@empID, @rowCount, @modBy, @modOn, @alertID)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/22/2015>
-- Description:	<This stored procedure is meant to delete a specific Gross Pay filter.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_employer_gross_pay_filter]
	@gpID int
AS
BEGIN
	DELETE FROM gross_pay_filter
	WHERE gross_pay_id=@gpID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/29/2015>
-- Description:	<This stored procedure is meant to delete all employer payroll alerts.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_employer_payroll_alerts]
	@empID int, 
	@modBy varchar(50), 
	@modOn datetime
AS
BEGIN
	DECLARE @rowCount int;
	SET @rowCount=0;
	DECLARE @alertID int;
	SET @alertID = 1;

	DELETE FROM import_payroll
	WHERE employerid=@empID;

	SET @rowCount = @@ROWCOUNT;

	INSERT INTO alert_archive
	VALUES(@empID, @rowCount, @modBy, @modOn, @alertID)



END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <11/16/2015>
-- Description:	<This stored procedure will DELETE a specific Equivalency.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_equivalency]
	@equivID int
AS
BEGIN
DELETE equivalency
WHERE equivalency_id=@equivID;


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/17/2015>
-- Description:	<This stored procedure will DELETE an Employers Insurance Plan from the database.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_insurance]
	@insuranceID int
AS
BEGIN
DELETE insurance
WHERE insurance_id=@insuranceID;


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <02/18/2016>
-- Description:	<This stored procedure is meant to delete all insurance_carrier_import records matching the batch id.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_insurance_carrier_batch_import]
	@batchID int
AS
BEGIN
	DELETE FROM import_insurance_coverage
	WHERE batch_id=@batchID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <04/07/2016>
-- Description:	<This stored procedure is meant to delete a single insurance_carrier_import record matching the row id.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_insurance_carrier_import_row]
	@rowID int,
	@modBy varchar(50),
	@modOn datetime
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY
		INSERT INTO import_insurance_coverage_archive
		SELECT *, @modBy, @modOn FROM import_insurance_coverage WHERE row_id=@rowID;

		DELETE FROM import_insurance_coverage
		WHERE row_id=@rowID;

		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH

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
CREATE PROCEDURE [dbo].[DELETE_insurance_contribution]
	@contID int
AS
BEGIN
DELETE insurance_contribution
WHERE ins_cont_id=@contID;


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <04/17/2016>
-- Description:	<This stored procedure is meant to delete a single insurance_carrier_editable_row record matching the row id.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_insurance_coverage_editable_row]
	@rowID int
AS
BEGIN

DELETE insurance_coverage_editable
WHERE row_id=@rowID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/27/2014>
-- Description:	<This stored procedure was built to help with the duplicate record issue, it will completely remove a Payroll record.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_payroll]
	@rowID int, 
	@modBy varchar(50), 
	@modOn datetime
AS
BEGIN
	
	BEGIN TRANSACTION
		BEGIN TRY
			--Archive the record to the Payroll_Archive table. 
			INSERT INTO payroll_archive (row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, modBy, modOn, history)
			SELECT row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, @modBy, @modOn, history
			FROM payroll
			WHERE row_id=@rowID;

			--Delete the record from the Payroll table. 
			DELETE FROM payroll
			WHERE row_id=@rowID;

			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
		END CATCH


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <09/11/2014>
-- Description:	<This stored procedure is meant to delete all payroll_import records matching the batch id.>
-- Modifications:
--    <2-4-2015>: Added the payroll archive piece. If a payroll record got into the act system. We wanted to record 
--that transaction if a user were to DELETE it from the system. 
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_payroll_import]
	@batchID int, 
	@modBy varchar(50), 
	@modOn datetime
AS
BEGIN
	BEGIN TRANSACTION
		BEGIN TRY
			--Archive any payroll records that were in the ACT system related to the batch being removed.  
			INSERT INTO payroll_archive (row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, modBy, modOn, history)
			SELECT row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, @modBy, @modOn, history
			FROM payroll
			WHERE batch_id=@batchID;

			--Remove the actual payroll records related to the batch id. 
			DELETE FROM payroll
			WHERE batch_id=@batchID;

			-- Remove the payroll records with batch id that are stuck in the payroll import table. 
			-- Note we are not archiving these records as they were never used to average any employees hours. 
			DELETE FROM import_payroll
			WHERE batchid=@batchID;

			UPDATE batch 
			SET
				delBy=@modBy, 
				delOn=@modOn
			WHERE
				batch_id=@batchID;
		
			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
		END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[DELETE_payroll_import_row]    Script Date: 5/12/2016 3:55:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <09/11/2014>
-- Description:	<This stored procedure is meant to delete a single payroll_import record matching the row id.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_payroll_import_row]
	@rowID int
AS
BEGIN
	DELETE FROM import_payroll
	WHERE rowid=@rowID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/27/2014>
-- Description:	<This stored procedure was built to help with the duplicate record issue, it will completely remove a Payroll record.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_payroll_summer_average]
	@rowID int, 
	@modBy varchar(50), 
	@modOn datetime
AS
BEGIN
	
	BEGIN TRANSACTION
		BEGIN TRY
			--Archive the record to the Payroll_Archive table. 
			INSERT INTO payroll_archive (row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, modBy, modOn, history)
			SELECT row_id, employer_id, batch_id, employee_id, gp_id, act_hours, sdate, edate, cdate, @modBy, @modOn, history
			FROM payroll
			WHERE row_id=@rowID;

			--Delete the record from the Payroll table. 
			DELETE FROM payroll_summer_averages
			WHERE row_id=@rowID;

			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
		END CATCH


END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/17/2014>
-- Description:	<This stored procedure is meant to create a new HR Status for a specific district.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_employer_alert]
	@employerID int,
	@alertTypeID int
AS

BEGIN
	INSERT INTO [alert](
		alert_type_id,
		employer_id)
	VALUES(
		@alertTypeID,
		@employerID)
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/11/2014>
-- Description:	<This stored procedure is meant to pull all data from Employee Demographic Import.>	
-- Modifications:

-- =============================================
CREATE PROCEDURE [dbo].[INSERT_import_employee]
	@batchID int,
	@hrStatusID varchar(50),
	@hrDescription varchar(50),
	@employerID int,				--This must be known at the time of import. 
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@address varchar(50),
	@city varchar(50),
	@stateID varchar(2), 
	@zip varchar(5),
	@hireDate varchar(20),
	@currDate varchar(20),
	@ssn varchar(50),				--This must be encrypted by the web application.
	@extID varchar(50),				--This is the external ID from the Payroll/HR software. 
	@termDate varchar(20),
	@dob varchar(20)
AS

BEGIN
	INSERT INTO [import_employee](
	hr_status_ext_id,
	hr_description,
	employerID,
	fName, 
	mName,
	lName,
	[address],
	city,
	stateAbb,
	zip,
	i_hDate,
	i_cDate,
	ssn,
	ext_employee_id,
	i_tDate,
	i_dob,
	batchid) 
	VALUES(
		@hrStatusID,
		@hrDescription,
		@employerID,
		@fname, 
		@mName,
		@lname,
		@address,
		@city,
		@stateID,
		@zip,
		@hireDate,
		@currDate,
		@ssn,
		@extID,
		@termDate,
		@dob,
		@batchID)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/16/2016>
-- Description:	<This stored procedure is meant to import data from the Insurance Carrier File.>	
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_import_insurance_carrier_report]
	@batchID int,
	@taxYear int,
	@employerID int,
	@dependentLink varchar(50),				--This must be known at the time of import. 
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@ssn varchar(50),
	@dob datetime,
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
	@dec bit,
	@all12 bit,
	@subscriber bit,
	@jani varchar(10),
	@febi varchar(10), 
	@mari varchar(10),
	@apri varchar(10), 
	@mayi varchar(10),
	@juni varchar(10),
	@juli varchar(10),
	@augi varchar(10),
	@sepi varchar(10),
	@octi varchar(10),
	@novi varchar(10),
	@deci varchar(10),
	@all12i varchar(10),
	@subscriberi varchar(50), 
	@address varchar(50),
	@city varchar(50),
	@state varchar(50),
	@zip varchar(10), 
	@carrierID int
AS

BEGIN
	INSERT INTO [import_insurance_coverage](
	batch_id,
	employer_id,
	tax_year,
	dependent_link,
	fName,
	mName,
	lName,
	ssn,
	dob,
	jan,
	feb,
	march,
	april,
	may,
	june,
	july,
	august,
	september,
	october,
	november,
	december,
	all12,
	subscriber,
	jan_i,
	feb_i,
	march_i,
	april_i,
	may_i,
	june_i,
	july_i,
	aug_i,
	sep_i,
	oct_i,
	nov_i,
	dec_i,
	all12_i,
	subscriber_i,
	address_i,
	city_i,
	state_i,
	zip_i,
	carrier_id
	) 
	VALUES(
	@batchID,
	@employerID,
	@taxYear,
	@dependentLink,			
	@fname,
	@mname,
	@lname,
	@ssn,
	@dob,
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
	@dec,
	@all12,
	@subscriber,
	@jani,
	@febi,
	@mari,
	@apri,
	@mayi,
	@juni,
	@juli,
	@augi,
	@sepi,
	@octi,
	@novi,
	@deci,
	@all12i,
	@subscriberi,
	@address,
	@city,
	@state,
	@zip,
	@carrierID)

END

GO
/****** Object:  StoredProcedure [dbo].[INSERT_import_payroll]    Script Date: 5/12/2016 3:55:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/4/2014>
-- Description:	<This stored procedure is meant to import data from the Payroll Import File.>	
-- Modifications:
--			-9-24-2014: Added the external employee id from the HR software.

-- =============================================
CREATE PROCEDURE [dbo].[INSERT_import_payroll]
	@batchID int,
	@employerID int,				--This must be known at the time of import. 
	@fname varchar(25),
	@mname varchar(25),
	@lname varchar(25),
	@hours varchar(6),
	@sdate varchar(8),
	@edate varchar(8),
	@ssn varchar(50),				--This must be encrypted by the web application.
	@gpDesc varchar(30),			--This is the external ID from the Payroll/HR software. 
	@gpID varchar(20),
	@cdate varchar(8), 
	@modBy varchar(50),
	@modOn datetime, 
	@employeeExtID varchar(30)
AS

BEGIN
	INSERT INTO [import_payroll](
	employerid,
	batchid,
	fname, 
	mname,
	lname,
	i_hours,
	i_sdate,
	i_edate,
	ssn,
	gp_description,
	gp_ext_id,
	i_cdate,
	modBy,
	modOn, 
	ext_employee_id) 
	VALUES(
	@employerID,
	@batchID,			
	@fname,
	@mname,
	@lname,
	@hours,
	@sdate,
	@edate,
	@ssn,	
	@gpDesc, 
	@gpID,
	@cdate, 
	@modBy,
	@modOn,
	@employeeExtID)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/25/2016>
-- Description:	<This stored procedure is meant to store a new Tax Year Approval.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_1095_tax_year_approval]
	@taxYear int,
	@employeeID int,
	@employerID int,
	@modBy varchar(50),
	@modOn datetime,
	@1095 bit
AS

BEGIN
	INSERT INTO [tax_year_1095c_approval](
		tax_year,
		employee_id,
		employer_id,
		approvedBy,
		approvedOn,
		get1095C)
	VALUES(
		@taxYear,
		@employeeID,
		@employerID,
		@modBy,
		@modOn,
		@1095)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/18/2014>
-- Description:	<This stored procedure is meant to create a new batch when data is imported.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_batch]
	@employerID int,
	@modOn datetime,
	@modBy varchar(50),
	@batchID int OUTPUT
AS

BEGIN
	INSERT INTO [batch](
		employer_id,
		modOn,
		modBy)
	VALUES(
		@employerID,
		@modOn,
		@modBy)

SELECT @batchID = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure is meant to create a Classification.>
-- Changes: 
-- 	
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_classification]
	@employerID int,
	@desc varchar(50),
	@ashCode varchar(2),
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max)
AS

BEGIN
	INSERT INTO [employee_classification](
		employer_id,
		[description],
		ash_code,
		modBy,
		modOn,
		history)
	VALUES(
		@employerID,
		@desc,
		@ashCode,
		@modBy,
		@modOn,
		@history)

END

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
CREATE PROCEDURE [dbo].[INSERT_new_editable_insurance_coverage]
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
	INSERT INTO [insurance_coverage_editable](
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

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/24/2014>
-- Description:	<This stored procedure is meant to create a new employee, specific to a district.>	
-- Modifications:
--			-  8/5/2014 TW
--					Added the following fields, 
--					- initialMeasurementEnd
--					- initial
--					- transition
--					- ongoing
--					- plan_year_id
--			-	5/4/2015 TW
--					- When inserting a new employee, the plan_year_id was only being set. Both the plan_year_id and meas_plan_year_id
-- need to be set so we don't have to manually set this when the measurement period ends. 
--			- 5-20-2015 TLW
--			Changed the process to utilize the meas_plan_year_id for employees. The only time the plan_year_id and limbo_plan_year_id can change
--		is when the measurement or plan year are rolled over. 
--			-	9/11/2015 TW
--					- Added the Classification and ACT Status ID.
--			-	11/25/2015 TW
--					- Removed the initial parameter.
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_employee]
	@employeeTypeID int,
	@hrStatusID int,
	@employerID int,
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@address varchar(50),
	@city varchar(50),
	@stateID int, 
	@zip varchar(50),
	@hireDate datetime,
	@currDate datetime,
	@ssn varchar(200),
	@extID varchar(50),
	@termDate datetime,
	@dob datetime,
	@imEnd datetime, 
	@planYearID int,
	@modOn datetime,
	@modBy varchar(50),
	@classID int,
	@actStatusID int,
	@employeeID int OUTPUT
AS

BEGIN
	INSERT INTO [employee](
		employee_type_id,
		HR_status_id,
		employer_id,
		fName,
		mName,
		lName,
		[address],
		city,
		state_id,
		zip,
		hireDate,
		currDate,
		ssn,
		ext_emp_id,
		terminationDate,
		dob,
		initialMeasurmentEnd,
		meas_plan_year_id,
		ModOn,
		ModBy,
		classification_id,
		aca_status_id)
	VALUES(
		@employeeTypeID,
		@hrStatusID,
		@employerID,
		@fname,
		@mname,
		@lname,
		@address,
		@city,
		@stateID, 
		@zip,
		@hireDate,
		@currDate,
		@ssn,
		@extID,
		@termDate,
		@dob,
		@imEnd,
		@planYearID,
		@modOn,
		@modBy,
		@classID,
		@actStatusID)

SELECT @employeeID = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/12/2014>
-- Description:	<This stored procedure is meant to create a new employee_type.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_employee_type]
	@employerid int, 
	@name varchar(50)
AS

BEGIN
	INSERT INTO [employee_type](
		employer_id,
		name)
	VALUES(
		@employerid,
		@name)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/29/2014>
-- Description:	<This stored procedure is meant to create a new district.>
-- Changes:
-- <5-6-2014>
--		Removed all contact information as this was merged into the user table. 
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_employer]
	@name varchar(50),
	@add varchar(50),
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
	@empid int OUTPUT
AS

BEGIN
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
		ein)
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
		@ein)

SELECT @empid = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/10/2014>
-- Description:	<This stored procedure is meant to create a new equivelancy.>
-- Changes: 
-- <6-22-2014>
--		- Added position ID, Activity ID, Detail ID		
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_equivalency]
	@employerID int,
	@name varchar(50),
	@gpID int,
	@every decimal(18,4),
	@unitID int,
	@credit decimal(18,4),
	@sdate datetime,
	@edate datetime,
	@notes varchar(1000),
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max),
	@active bit,
	@equivTypeID int,
	@posID int,
	@actID int,
	@detID int,
	@equivalencyID int OUTPUT
AS

BEGIN
	INSERT INTO [equivalency](
		employer_id,
		name,
		gpid,
		every,
		unit_id,
		credit,
		[start_date],
		end_date,
		notes,
		modBy,
		modOn,
		history,
		active,
		equivalency_type_id,
		position_id,
		activity_id,
		detail_id)
	VALUES(
		@employerID,
		@name,
		@gpID,
		@every,
		@unitID,
		@credit,
		@sdate,
		@edate,
		@notes,
		@modBy,
		@modOn,
		@history,
		@active,
		@equivTypeID,
		@posID,
		@actID,
		@detID)

SELECT @equivalencyID = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/16/2014>
-- Description:	<This stored procedure is meant to create a new gross pay type for a specific district.>
-- Changes:
--			9-5-2014: Added the gpID return parameter.
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_gross_pay]
	@employerID int,
	@extID varchar(50),
	@name varchar(50),
	@active bit, 
	@gpID int OUTPUT
AS

BEGIN
	INSERT INTO [gross_pay_type](
		employer_id,
		external_id,
		[description],
		active)
	VALUES(
		@employerID,
		@extID,
		@name, 
		@active)

SELECT @gpID = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/22/2015>
-- Description:	<This stored procedure is meant to create a new gross pay filter for a specific district.>
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_gross_pay_filter]
	@employerID int,
	@gpID int
AS

BEGIN
	INSERT INTO [gross_pay_filter](
		employer_id,
		gross_pay_id)
	VALUES(
		@employerID,
		@gpID)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/22/2014>
-- Description:	<This stored procedure is meant to create a new HR Status for a specific district.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_hr_status]
	@employerID int,
	@extID varchar(50),
	@name varchar(50),
	@active bit,
	@hrID int OUTPUT
AS

BEGIN
	INSERT INTO [hr_status](
		employer_id,
		ext_id,
		name,
		active)
	VALUES(
		@employerID,
		@extID,
		@name, 
		@active)

SELECT @hrID = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/10/2015>
-- Description:	<This stored procedure is meant to create a new insurance contribution.>
-- Changes: 
-- 	
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_insurance_contribution]
	@insuranceID int,
	@contributionID char(1),
	@classID int,
	@amount money,
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max),
	@newID int OUTPUT
AS

BEGIN
	INSERT INTO [insurance_contribution](
		insurance_id,
		contribution_id,
		classification_id,
		amount,
		modBy,
		modOn,
		history)
	VALUES(
		@insuranceID,
		@contributionID,
		@classID,
		@amount,
		@modBy,
		@modOn,
		@history)

SELECT @newID = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/22/2015>
-- Description:	<This stored procedure is meant to create a NEW insurance offer.>
-- Changes: 
-- 	
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_insurance_offer]
	@employerID int,
	@employeeID int,
	@planYearID int, 
	@monthlyAvg decimal(10,2),
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max)
AS

BEGIN
	INSERT INTO [employee_insurance_offer](
		employee_id,
		plan_year_id,
		employer_id,
		avg_hours_month,
		modOn,
		modBy,
		history)
	VALUES(
		@employeeID,
		@planYearID,
		@employerID,
		@monthlyAvg,
		@modOn,
		@modBy,
		@history)

END

GO
/****** Object:  StoredProcedure [dbo].[INSERT_new_insurance_plan]    Script Date: 5/12/2016 3:55:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to create a new insurance plan.>
-- Changes:
--	     5-19-2015 TLW
--				- Added the following three parameters: minValue, offSpouse, offDependent.		
-- ============================================
CREATE PROCEDURE [dbo].[INSERT_new_insurance_plan]
	@planyearID int,
	@name varchar(50),
	@monthlycost money,
	@minValue bit,
	@offSpouse bit,
	@offDependent bit,
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(max),
	@insuranceTypeID int,
	@insuranceid int OUTPUT
AS

BEGIN
	INSERT INTO [insurance](
		plan_year_id,
		[description],
		monthlycost,
		minValue,
		offSpouse,
		offDependent, 
		modOn,
		modBy, 
		history,
		insurance_type_id)
	VALUES(
		@planyearID,
		@name,
		@monthlycost, 
		@minValue,
		@offSpouse,
		@offDependent,
		@modOn,
		@modBy,
		@history,
		@insuranceTypeID)

SELECT @insuranceid = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/26/2016>
-- Description:	<This is meant to INSERT a new insurance coverage row.>	
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_insurnace_coverage]
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
	INSERT INTO [insurance_coverage_editable](
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

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/26/2015>
-- Description:	<This stored procedure is meant to create a new Invoice Record.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_invoice]
	@employerID int,
	@count int,
	@base_fee money,
	@employee_fee money,
	@su_fee money,
	@total money,
	@modBy varchar(50),
	@modOn datetime,
	@message varchar(3000),
	@month int, 
	@year int,
	@invoiceID int OUTPUT
AS

BEGIN
	INSERT INTO [invoice](
		employer_id,
		invoice_month, 
		invoice_year,
		[count],
		base_fee,
		employee_fee,
		su_fee,
		total,
		createdOn,
		createdBy,
		[message]
		)
	VALUES(
		@employerID,
		@month,
		@year,
		@count,
		@base_fee,
		@employee_fee,
		@su_fee,
		@total,
		@modOn,
		@modBy,
		@message
		)

SELECT @invoiceID = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/24/2014>
-- Description:	<This stored procedure is meant to create a new measurement period.>
-- Changes: 
-- <6-22-2014>
--		- Added position ID, Activity ID, Detail ID	
-- <7-28-2015>
--		- Added the Second Summer Window Start and End.	
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_measurement]
	@employerID int,
	@planYearID int,
	@employeeTypeID int,
	@measurementTypeID int,
	@meas_start datetime,
	@meas_end datetime,
	@admin_start datetime,
	@admin_end datetime,
	@open_start datetime,
	@open_end datetime,
	@stab_start datetime,
	@stab_end datetime,
	@notes varchar(max),
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(max),
	@swStart datetime,
	@swEnd datetime,
	@swStart2 datetime,
	@swEnd2 datetime,
	@measurementID int OUTPUT
AS

BEGIN
	INSERT INTO [measurement](
		employer_id,
		plan_year_id,
		employee_type_id,
		measurement_type_id,
		meas_start,
		meas_end,
		admin_start,
		admin_end,
		open_start,
		open_end,
		stability_start,
		stability_end,
		notes,
		history,
		modOn,
		modBy, 
		sw_start,
		sw_end,
		sw2_start,
		sw2_end)
	VALUES(
		@employerID,
		@planYearID,
		@employeeTypeID,
		@measurementTypeID,
		@meas_start,
		@meas_end,
		@admin_start,
		@admin_end,
		@open_start,
		@open_end,
		@stab_start,
		@stab_end,
		@notes,
		@history,
		@modOn,
		@modBy, 
		@swStart,
		@swEnd,
		@swStart2,
		@swEnd2)

SELECT @measurementID = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/17/2014>
-- Description:	<This stored procedure is meant to Insert clean data into the the Payroll Table.>	
-- Modifications:
--		5-27-2015 TLW
--			- Added the history to include the origional values that were imported. 
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_payroll]
	@employerID int,
	@batchID int, 
	@employeeID int,
	@gpID int, 
	@hours numeric(10,4),
	@sdate datetime, 
	@edate datetime, 
	@cdate datetime, 
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(2000)
AS

BEGIN
	INSERT INTO [payroll](
	employer_id,
	batch_id,
	employee_id,
	gp_id,
	act_hours,
	sdate,
	edate,
	cdate,
	modBy,
	modOn,
	history) 
	VALUES(
	@employerID,
	@batchID,			
	@employeeID,
	@gpID,
	@hours,
	@sdate,
	@edate,
	@cdate, 
	@modBy,
	@modOn,
	@history)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/27/2015>
-- Description:	<This stored procedure is meant to Insert a NEW Summer Average Payroll.>	
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_payroll_summer_avg]
	@employerID int,
	@planYearID int,
	@batchID int, 
	@employeeID int,
	@gpID int, 
	@hours numeric(10,4),
	@sdate datetime, 
	@edate datetime, 
	@cdate datetime, 
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(2000)
AS

BEGIN
	INSERT INTO [payroll_summer_averages](
	employer_id,
	plan_year_id,
	batch_id,
	employee_id,
	gp_id,
	act_hours,
	sdate,
	edate,
	cdate,
	modBy,
	modOn,
	history) 
	VALUES(
	@employerID,
	@planYearID,
	@batchID,			
	@employeeID,
	@gpID,
	@hours,
	@sdate,
	@edate,
	@cdate, 
	@modBy,
	@modOn,
	@history)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/19/2014>
-- Description:	<This stored procedure is meant to create a new Pay Roll record.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_plan_year]
	@employerID int,
	@description varchar(50),
	@startDate datetime,
	@endDate datetime,
	@notes varchar(max),
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@planyearid int OUTPUT
AS

BEGIN
	INSERT INTO [plan_year](
		employer_id,
		[description],
		startDate,
		endDate,
		notes,
		history,
		modOn,
		modBy)
	VALUES(
		@employerID,
		@description,
		@startDate,
		@endDate,
		@notes,
		@history,
		@modOn,
		@modBy)

SELECT @planyearid = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/7/2014>
-- Description:	<This stored procedure is meant to capture all data needed
-- when a  new business/district registers to use the system.
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_registration]
	@empTypeID int,
	@name varchar(50),
	@ein varchar(50),
	@add varchar(50),
	@city varchar(50),
	@stateID int,
	@zip varchar(15),
	@userfname varchar(50),
	@userlname varchar(50),
	@useremail varchar(50),
	@userphone varchar(15),
	@username varchar(50),
	@password varchar(50),
	@active bit,
	@power bit,
	@billing bit,
	@b_add varchar(50),
	@b_city varchar(50),
	@b_stateID int,
	@b_zip varchar(15),
	@p_desc1 varchar(50),
	@p_start1 datetime,
	@p_desc2 varchar(50),
	@p_start2 datetime,
	@b_fname varchar(50),
	@b_lname varchar(50),
	@b_email varchar(50),
	@b_phone varchar(15), 
	@b_username varchar(50),
	@b_password varchar(50),
	@b_active bit, 
	@b_power bit,
	@b_billing bit,
	@employerID int OUTPUT
AS

BEGIN
	DECLARE @userid int;
	DECLARE @planyearid int;
	DECLARE @lastModBy varchar(50);
	DECLARE @lastMod datetime;
	DECLARE @irsContact bit;
	DECLARE @irsContact2 bit;

	SET @irsContact = 1;
	SET @irsContact2 = 0;
	SET @lastModBy = 'Registration';
	SET @lastMod = GETDATE();

	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	BEGIN TRANSACTION
		BEGIN TRY
			DECLARE @default varchar(50);
			SET @default = 'All Employees';

			-- Step 1: Create a new EMPLOYER based on the registration information.
			--			- Return the new Employer_ID
			EXEC INSERT_new_employer 
				@name, 
				@add, 
				@city,
				@stateID, 
				@zip, 
				'../images/logos/EBC_logo.gif',  
				@b_add, 
				@b_city, 
				@b_stateID, 
				@b_zip, 
				@empTypeID,
				@ein,
				@employerID OUTPUT;

			-- Step 2: Create a new EMPLOYEE_TYPE for this specific employer.
			EXEC INSERT_new_employee_type 
				@employerID, 
				@default;

			-- Step 3: Create a new USER for this specific district.
			EXEC INSERT_new_user 
				@userfname, 
				@userlname, 
				@useremail, 
				@userphone, 
				@username, 
				@password,
				@employerID, 
				@active, 
				@power,
				@lastModBy,
				@lastMod,
				@billing,
				@irsContact,
				@userid OUTPUT

			IF (@b_username IS NOT NULL)
				BEGIN
					EXEC INSERT_new_user 
						@b_fname, 
						@b_lname, 
						@b_email, 
						@b_phone, 
						@b_username, 
						@b_password,
						@employerID, 
						@b_active, 
						@b_power,
						@lastModBy,
						@lastMod,
						@b_billing,
						@irsContact2,
						@userid OUTPUT
				END
	

			-- Step 4: Create a new PLAN YEAR for this specific district.
			DECLARE @p_end1 datetime;
			DECLARE @history varchar(100);
			DECLARE @p_notes varchar(100);
			SET @p_end1 = DATEADD(dd, 364, @p_start1);
			SET @p_notes = '';
			SET @history = 'Plan created on: ' + CONVERT(varchar(19), GETDATE());
			EXEC INSERT_new_plan_year 
				@employerID, 
				@p_desc1, 
				@p_start1, 
				@p_end1, 
				@p_notes,
				@history,
				@lastMod,
				@lastModBy, 
				@planyearid OUTPUT; 

			-- Step 5: Create a second PLAN YEAR for this specific district if needed.
			IF (@p_start2 IS NOT NULL OR @p_start2 = '')
				BEGIN
					DECLARE @p_end2 datetime;
					SET @p_end2 = DATEADD(dd, 364, @p_start2);
					SET @p_notes = '';
					SET @history = 'Plan created on: ' + CONVERT(varchar(19), GETDATE());
					EXEC INSERT_new_plan_year 
						@employerID, 
						@p_desc2, 
						@p_start2, 
						@p_end2, 
						@p_notes,
						@history,
						@lastMod,
						@lastModBy, 
						@planyearid OUTPUT; 
				END

			COMMIT
		END TRY
		BEGIN CATCH
			--If something fails return 0.
			SET @employerID = 0;
			ROLLBACK TRANSACTION
		END CATCH

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/21/2014>
-- Description:	<This stored procedure is meant to create a new user record.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_new_user]
	@fname varchar(100),
	@lname varchar(100),
	@email varchar(100),
	@phone varchar(25),
	@username varchar(50),
	@password varchar(max),
	@employerid int, 
	@active bit,
	@power bit,
	@user varchar(50),
	@datestamp datetime,
	@billing bit,
	@irsContact bit,
	@userid int OUTPUT
AS

BEGIN
	INSERT INTO [user](
		fname, 
		lname,
		email,
		phone, 
		username,
		[password],
		employer_id,
		poweruser,
		active, 
		last_mod_by,
		last_mod,
		billing,
		irsContact)
	VALUES(
		@fname, 
		@lname,
		@email,
		@phone,
		@username,
		@password,
		@employerid,
		@power,
		@active,
		@user,
		@datestamp,
		@billing,
		@irsContact)

SELECT @userid = SCOPE_IDENTITY();
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/17/2016>
-- Description:	<This stored procedure is meant to generate Insurance Alerts for any missing Employees.>	
-- Modifications:

-- =============================================
CREATE PROCEDURE [dbo].[INSERT_PlanYear_Missing_insurance_offers]
	@employerID int,
	@planYearEndDate datetime, --Stability Period Start Date
	@missingPlanYearID int,
	@currPlanYearID int,
	@hours int,
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max)
AS

BEGIN
	INSERT INTO dbo.employee_insurance_offer (employee_id, plan_year_id, employer_id, avg_hours_month, modOn, modBy, history)
	SELECT employee_id, @missingPlanYearID, @employerID, @hours, @modOn, @modBy, @history
	FROM employee
	WHERE 
		hireDate < @planYearEndDate AND
		employer_id = @employerID AND
		plan_year_id = @currPlanYearID AND
		employee_id NOT IN (Select employee_id FROM employee_insurance_offer WHERE plan_year_id=@missingPlanYearID and employer_id=@employerID);

END

GO
/****** Object:  StoredProcedure [dbo].[INSERT_UPDATE_employee_dependent]    Script Date: 5/12/2016 3:55:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/24/2014>
-- Description:	<This stored procedure is meant to update or insert Employee Dependents.>	
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_UPDATE_employee_dependent]
	@currDepID int,
	@employeeID int,
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@ssn varchar(50),
	@dob varchar(50),
	@dependentID int OUTPUT
AS

BEGIN
	SET @dependentID = @currDepID;

	/************************************************************************************************************************
	Compare EmployeeID and SSN, if SSN isn't available compare EMPLOYEEID, FNAME, LNAME and DOB
	************************************************************************************************************************/
IF @dependentID <= 0
	BEGIN
		SELECT @dependentID=dependent_id FROM employee_dependents 
		WHERE (employee_id=@employeeID AND ssn=@ssn) OR (employee_id=@employeeID AND fName=@fname AND dob=@dob);
	END

IF @dependentID <= 0
	BEGIN
		INSERT INTO [employee_dependents](
			employee_id,
			fName,
			mName,
			lName,
			ssn,
			dob)
		VALUES(
			@employeeID,
			@fname,
			@mname,
			@lname,
			@ssn,
			@dob)
	END
ELSE
	BEGIN
		UPDATE [employee_dependents]
		SET
			fName=@fname, 
			mName=@mname,
			lName=@lname,
			ssn=@ssn,
			dob=@dob
		WHERE
			dependent_id=@dependentID;
	END

IF @dependentID <= 0
	BEGIN
		SET @dependentID = SCOPE_IDENTITY();
	END

SELECT @dependentID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/9/2016>
-- Description:	<This stored procedure is meant to update or insert a tax year approval.>	
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_UPDATE_employer_irs_submission_approval]
	@approvalID int,
	@employerID int,
	@taxYear int,
	@dge bit,
	@dgeName varchar(50),
	@dgeEIN varchar(50),
	@dgeAddress varchar(50),
	@dgeCity varchar(50),
	@dgeStateID int,
	@dgeZip varchar(50),
	@dgeFname varchar(50),
	@dgeLname varchar(50),
	@dgePhone varchar(50),
	@ale bit,
	@tr1 bit,
	@tr2 bit, 
	@tr3 bit,
	@tr4 bit,
	@tr5 bit,
	@tr bit,
	@tobacco bit,
	@unpaidLeave bit,
	@safeHarbor bit,
	@completedBy varchar(50),
	@completedOn datetime,
	@ebcApproved bit,
	@ebcApprovedBy varchar(50),
	@ebcApprovedOn datetime,
	@allowEditing bit,
	@approvalID_Final int OUTPUT
AS

BEGIN
	SET @approvalID_Final = @approvalID;

	/************************************************************************************************************************
	Compare EmployerID and TAX YEAR to see if a record exists. 
	************************************************************************************************************************/
IF @approvalID_Final<= 0
	BEGIN
		SELECT @approvalID_Final=approval_id FROM tax_year_approval
		WHERE employer_id=@employerID AND tax_year=@taxYear;
	END

IF @approvalID_Final <= 0
	BEGIN
		INSERT INTO [tax_year_approval](
			employer_id,
			tax_year,
			dge,
			dge_name,
			dge_ein,
			dge_address,
			dge_city,
			state_id,
			dge_zip,
			dge_contact_fname,
			dge_contact_lname,
			dge_phone,
			ale,
			tr_q1,
			tr_q2,
			tr_q3,
			tr_q4,
			tr_q5,
			tr_qualified,
			tobacco,
			unpaidLeave,
			safeHarbor,
			completed_by,
			completed_on,
			ebc_approval,
			ebc_approved_by,
			ebc_approved_on,
			allow_editing)
		VALUES(
			@employerID,
			@taxYear,
			@dge,
			@dgeName,
			@dgeEIN,
			@dgeAddress,
			@dgeCity,
			@dgeStateID,
			@dgeZip,
			@dgeFname,
			@dgeLname,
			@dgePhone,
			@ale,
			@tr1,
			@tr2, 
			@tr3,
			@tr4,
			@tr5,
			@tr,
			@tobacco,
			@unpaidLeave,
			@safeHarbor,
			@completedBy,
			@completedOn,
			@ebcApproved,
			@ebcApprovedBy,
			@ebcApprovedOn,
			@allowEditing)
	END
ELSE
	BEGIN
		UPDATE [tax_year_approval]
		SET
			employer_id=@employerID,
			tax_year=@taxYear,
			dge=@dge,
			dge_name=@dgeName,
			dge_ein=@dgeEIN,
			dge_address=@dgeAddress,
			dge_city=@dgeCity,
			state_id=@dgeStateID,
			dge_zip=@dgeZip,
			dge_contact_fname=@dgeFname,
			dge_contact_lname=@dgeLname,
			dge_phone=@dgePhone,
			ale=@ale,
			tr_q1=@tr1,
			tr_q2=@tr2,
			tr_q3=@tr3,
			tr_q4=@tr4,
			tr_q5=@tr5,
			tr_qualified=@tr,
			tobacco=@tobacco,
			unpaidLeave=@unpaidLeave,
			safeHarbor=@safeHarbor,
			completed_by=@completedBy,
			completed_on=@completedOn,
			ebc_approval=@ebcApproved,
			ebc_approved_by=@ebcApprovedBy,
			ebc_approved_on=@ebcApprovedOn,
			allow_editing=@allowEditing
		WHERE
			approval_id=@approvalID_Final;
	END

IF @approvalID_Final <= 0
	BEGIN
		SET @approvalID_Final = SCOPE_IDENTITY();
	END

SELECT @approvalID_Final;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <11/17/2015>
-- Description:	<This stored procedure is meant to merge a duplicate Gross Pay Description
-- in the system.
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[MERGE_gross_pay_description]
	@gpID int,	--This ID stays in the ACT system. 
	@gpID2 int	--This ID get's replaced and removed.
AS

BEGIN
	/*************************************************************
	******* Create a transaction that must fully complete ********
	This will update each of the following tables to allow for the 
	Gross Pay Description Merger to Remove the duplicate.
	**************************************************************/
	BEGIN TRANSACTION
		BEGIN TRY
			--Step 1: Update Import_Payroll
			UPDATE import_payroll
			SET gp_id=@gpID
			WHERE gp_id=@gpID2;

			--Step 2: Update Payroll 
			UPDATE payroll
			SET gp_id=@gpID
			WHERE gp_id=@gpID2;

			--Step 3: Update Payroll_Archive
			UPDATE payroll_archive
			SET gp_id=@gpID
			WHERE gp_id=@gpID2;

			--Step 4: Update Gross_Pay_Filter
			UPDATE gross_pay_filter
			SET gross_pay_id=@gpID
			WHERE gross_pay_id=@gpID2;

			--Step 5: Update Equivalency
			UPDATE equivalency
			SET gpID=@gpID
			WHERE gpID=@gpID2;

			--Step 6: Delete gpID2 from the Gross_Pay_Type table.
			DELETE gross_pay_type
			WHERE gross_pay_id=@gpID2;

			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
		END CATCH

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <03/13/2015>
-- Description:	<This stored procedure is meant to delete all employee_import records matching the batch id.>
-- =============================================
CREATE PROCEDURE [dbo].[REMOVE_EMPLOYER_FROM_ACT]
	@employerID int
AS
BEGIN
DELETE 
  [aca-demo].dbo.equivalency
  where employer_id=@employerID

DELETE
  [aca-demo].dbo.employee_py_archives
  where employer_id=@employerID

--Insurance Contributions
DELETE
 [aca-demo].dbo.insurance_contribution
 WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--Summer Payroll Supplemental hours
DELETE [aca-demo].dbo.payroll_summer_averages
  WHERE employer_id=@employerID;

DELETE
  [aca-demo].dbo.import_employee
  WHERE employerID=@employerID

DELETE [aca-demo].dbo.alert_archive
	WHERE employer_id=@employerID

DELETE [aca-demo].dbo.payroll_archive
	WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.import_payroll
  WHERE employerid=@employerID

DELETE 
  [aca-demo].[dbo].[payroll]
  WHERE employer_id=@employerID

DELETE  
  FROM [aca-demo].dbo.employee
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.hr_status
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.gross_pay_filter
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.gross_pay_type
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.measurement
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.plan_year
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.alert
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.[user]
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.employee_type
  WHERE employer_id=@employerID

DELETE 
  [aca-demo].dbo.invoice
  WHERE employer_id=@employerID

DELETE
  [aca-demo].dbo.batch
  WHERE employer_id=@employerID

--All Employee Classifications. 
DELETE
  [aca-demo].dbo.employee_classification
  WHERE employer_id=@employerID

DELETE 
  [aca-demo].dbo.employer
  WHERE employer_id=@employerID
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <03/13/2015>
-- Description:	<This stored procedure is meant to delete all employee_import records matching the batch id.>
-- Altered:
--				<11/30/2015> TLW
--					- Changed to handle new Foreign Key constraints. 
-- =============================================
CREATE PROCEDURE [dbo].[RESET_EMPLOYER]
	@employerID int
AS
BEGIN

--Equivalencies
DELETE 
  [aca-demo].dbo.equivalency
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  [aca-demo].dbo.employee_insurance_offer
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  [aca-demo].dbo.employee_insurance_offer_archive
  where employer_id=@employerID;

DELETE 
	[aca-demo].dbo.employee_dependents
	WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--Insurance Contributions
DELETE
 [aca-demo].dbo.insurance_contribution
 WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--Payroll Summer Averages. 
DELETE
  [aca-demo].dbo.payroll_summer_averages
  where employer_id=@employerID;

--Employee Import Alerts. 
DELETE
  [aca-demo].dbo.import_employee
  WHERE employerID=@employerID

--Employee Alert Archives. Alerts that have been deleted by users. 
DELETE [aca-demo].dbo.alert_archive
	WHERE employer_id=@employerID

--Payroll Import Alerts. Alerts that have been deleted by users. 
DELETE [aca-demo].dbo.payroll_archive
	WHERE employer_id=@employerID

--Payroll Import Alerts. 
DELETE
  [aca-demo].dbo.import_payroll
  WHERE employerid=@employerID

--Insurance Carrier Import Alerts
DELETE
  [aca-demo].dbo.import_insurance_coverage
  WHERE employer_id=@employerID;

--All Carrier Import Coverage
DELETE
  [aca-demo].dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--All Payroll. 
DELETE 
  [aca-demo].[dbo].[payroll]
  WHERE employer_id=@employerID

--All Employees.
DELETE  
  FROM [aca-demo].dbo.employee
  WHERE employer_id=@employerID

--All HR Status Codes
DELETE
  [aca-demo].dbo.hr_status
  WHERE employer_id=@employerID

--All Gross Pay Filters
DELETE
  [aca-demo].dbo.gross_pay_filter
  WHERE employer_id=@employerID

--All Gross Pay Codes
DELETE
  [aca-demo].dbo.gross_pay_type
  WHERE employer_id=@employerID

--All Measurement Periods. 
DELETE
  [aca-demo].dbo.measurement
  WHERE employer_id=@employerID

--All Plan Years. 
DELETE
  [aca-demo].dbo.plan_year
  WHERE employer_id=@employerID

--All assigned Alerts. 
DELETE
  [aca-demo].dbo.alert
  WHERE employer_id=@employerID

--All Batch rows. 
DELETE
  [aca-demo].dbo.batch
  WHERE employer_id=@employerID

--All Employee Classifications. 
DELETE
  [aca-demo].dbo.employee_classification
  WHERE employer_id=@employerID

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/1/2016>
-- Description:	<This stored procedure is meant to return all payroll data for a specific Batch ID.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT__payroll_batch](
	@employerID int,
	@batchID int
	)
AS
BEGIN
	SELECT * FROM View_payroll
	WHERE employer_id=@employerID AND
	batch_id=@batchID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/17/2014>
-- Description:	<This stored procedure is meant to return all activities.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_activities]
AS

BEGIN
	SELECT *
	FROM [equiv_activity]
END

GO
/****** Object:  StoredProcedure [dbo].[SELECT_all_aca_status]    Script Date: 5/12/2016 3:55:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/11/2015>
-- Description:	<This stored procedure is meant to return all ACA Status.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_aca_status]
AS
BEGIN
	SELECT * FROM aca_status;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/17/2015>
-- Description:	<This stored procedure is meant to return all Employee types in the table.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_alert_types]
AS
BEGIN
	SELECT * FROM alert_type;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/9/2014>
-- Description:	<This stored procedure is meant to return all Employee types in the table.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_employee_types]
@employerID int
AS
BEGIN
	SELECT * FROM employee_type where employer_id = @employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/8/2014>
-- Description:	<This stored procedure is meant to return all Employer types in the table.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_employer_types]
AS
BEGIN
	SELECT * FROM employer_type;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/16/2014>
-- Description:	<This stored procedure is meant to return all employers.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_employers]
AS

BEGIN
	SELECT *
	FROM [employer]
	ORDER BY name;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/6/2016>
-- Description:	<This stored procedure is meant to return all fees.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_fees]

AS

BEGIN
	SELECT *
	FROM [fee];
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/30/2014>
-- Description:	<This stored procedure is meant to return all Initial Measurment Time Periods.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_initial_measurements]
AS
BEGIN
	SELECT * FROM initial_measurement ORDER BY initial_measurement_id;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/12/2016>
-- Description:	<This stored procedure is meant to return all insurance carriers.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_insurance_carriers]
AS

BEGIN
	SELECT *
	FROM [insurance_carrier]
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/28/2015>
-- Description:	<This stored procedure is meant to return all Insurance types in the table.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_insurance_types]
AS
BEGIN
	SELECT * FROM insurance_type;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/14/2014>
-- Description:	<This stored procedure is meant to return all calendar Months.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_months]
AS
BEGIN
	SELECT * FROM [month];
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/18/2013>
-- Description:	<This stored procedure is meant to return all States in the table.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_states]
AS
BEGIN
	SELECT * FROM state;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/24/2014>
-- Description:	<This stored procedure is meant to return all ACA Terms.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_terms]
AS
BEGIN
	SELECT * FROM term;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/2/2014>
-- Description:	<This stored procedure is meant to return all ACA users that are active.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_users]
AS
BEGIN
	SELECT *
	FROM [user]
	WHERE active = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/10/2015>
-- Description:	<This stored procedure is meant to return all vendors.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_all_vendors]
AS
BEGIN
	SELECT *
	FROM payroll_vendor;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/10/2015>
-- Description:	<This stored procedure is meant to return all contribution types.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_contribution_types]
AS

BEGIN
	SELECT *
	FROM [contribution];
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/17/2014>
-- Description:	<This stored procedure is meant to return all details.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_details]
AS

BEGIN
	SELECT *
	FROM [equiv_detail]
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/12/2016>
-- Description:	<This stored procedure is meant to return all covered individuals.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_all_individual_coverage]
	@employeeID int, 
	@taxYear int
AS

BEGIN
	SELECT *
	FROM [View_all_insurance_coverage]
	WHERE employee_id = @employeeID and tax_year=@taxYear;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/23/2016>
-- Description:	<This stored procedure is meant to return all covered dependents.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_coverage]
	@employeeID int, 
	@taxYear int
AS

BEGIN
	SELECT *
	FROM [View_employee_insurance_coverage]
	WHERE employee_id = @employeeID and tax_year=@taxYear;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/23/2016>
-- Description:	<This stored procedure is meant to return all covered dependents.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_dependent_coverage]
	@employeeID int, 
	@taxYear int
AS

BEGIN
	SELECT *
	FROM [View_dependent_insurance_coverage]
	WHERE employee_id = @employeeID and tax_year=@taxYear;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2-24-2016>
-- Description:	<This stored procedure is meant to return all dependents and existing employee has.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_dependents](
	@employeeID int
	)
AS
BEGIN
	SELECT * FROM employee_dependents
	WHERE employee_id=@employeeID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/16/2016>
-- Description:	<This stored procedure is meant to return all editable rows for a specific individuals.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_editable_individual_coverage]
	@employeeID int, 
	@taxYear int
AS

BEGIN
	SELECT *
	FROM [aca-demo].dbo.insurance_coverage_editable
	WHERE employee_id = @employeeID and tax_year=@taxYear;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <11/20/2015>
-- Description:	<This stored procedure is meant to return all employee ID's that have been paid for a specific
--		Gross Pay Description ID.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_gross_pay_count](
	@grossPayID int
	)
AS
BEGIN
	Select DISTINCT employee_id 
	from payroll
	WHERE gp_id=@grossPayID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/1/2015>
-- Description:	<This stored procedure is meant to return a single Insurance Offer.>
-- 9-30-2015
--	Added @newHire to the SP to allow for two different types of insurance offers. 
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_insurance_offer](
	@employeeID int,
	@planYearID int
	)
AS
BEGIN
	SELECT *
	FROM [aca-demo].[dbo].[View_insurance_alert_details]
	WHERE 
		employee_id=@employeeID AND
		plan_year_id=@planYearID;
    
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/22/2014>
-- Description:	<This stored procedure is meant to return all payroll data within a time frame.>
-- Modifications:
-- <12-1-2014>: TLW
--	We elected to use only the Payroll End Date as one of the HR vendors only had this available. 
--		New Logic: 12-10-2014
--			- edate is >= to the mStart date & edate <= mEnd date
--		New Logic: 1-7-2015
--			- The edate was insufficient in capturing the data due to how some employers are using 
-- very large date ranges upto 12 months for one payroll. We found we need to average this out on a 
-- per day basis to get the most accurate average possible. If the sDate or eDate falls within the 
-- date range, the data will be used. 

--  Also changed the source to a VIEW to bring along the Gross Pay Description. 
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_payroll](
	@employeeID int,
	@mStart datetime,
	@mEnd datetime
	)
AS
BEGIN
	SELECT * FROM View_payroll
	WHERE employee_id=@employeeID AND
	((edate >= @mStart AND
	edate <= @mEnd) OR
	(sdate >= @mStart AND 
	sdate <= @mEnd))
	ORDER BY edate;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/04/2014>
-- Description:	<This stored procedure is meant to sum hours for a specific time period.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_payroll_sum](
	@employeeID int,
	@mStart datetime,
	@mEnd datetime
	)
AS
BEGIN

	CREATE TABLE #tempPayroll(
	batchid int, 
	ext_emp_id varchar(50),
	fName varchar(50),
	lName varchar(50),
	gp_ext_id varchar(50),
	gp_desc varchar(50), 
	sdate datetime,
	edate datetime,  
	acthours numeric(18,2)
	)

	SELECT * FROM View_payroll
	WHERE employee_id=@employeeID AND
	edate >= @mStart AND
	edate <= @mEnd
	ORDER BY edate;

	DROP TABLE #tempPayroll;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/27/2015>
-- Description:	<This stored procedure is meant to return all payroll data that was supplemented for summer hour averaging.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_payroll_summer_avg](
	@employeeID int,
	@planYearID int
	)
AS
BEGIN
	SELECT * FROM View_payroll_summer_avg
	WHERE employee_id=@employeeID and plan_year_id=@planYearID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/22/2014>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer]
	@employerID varchar(100)
AS

BEGIN
	SELECT *
	FROM [employer]
	WHERE employer_id = @employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/19/2014>
-- Description:	<This stored procedure is meant to return all ALERTS for a specific district.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_alerts](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [View_alerts]
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/11/2015>
-- Description:	<This stored procedure is meant to return all employers who are ready for billing.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_autoupload]
AS

BEGIN
	SELECT *
	FROM [employer]
	WHERE autoUpload = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/2/2015>
-- Description:	<This stored procedure is meant to return all batch ID's for a specific employer.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_batch_top25](
	@employerID int
	)
AS
BEGIN
	SELECT TOP 100 * 
	FROM batch b
	WHERE b.employer_id=@employerID AND b.batch_id in(Select batch_id from payroll where payroll.employer_id = @employerID)
	ORDER BY modOn DESC;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/5/2014>
-- Description:	<This stored procedure is meant to return all employers who are ready for billing.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_billing]
AS

BEGIN
	SELECT *
	FROM [employer]
	WHERE autoBill=1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/6/2015>
-- Description:	<This stored procedure is meant to return a count of employees for a 12 month period. This 
-- is used for billing purposes. The total count of EMPLOYEES that have been paid in the last 12 months.>
-- =============================================
Create PROCEDURE [dbo].[SELECT_employer_billing_count](
	@employerID int,
	@sdate datetime,
	@edate datetime
	)
AS
BEGIN
	SELECT count(DISTINCT(employee_id)) as Employee_Count
	FROM [aca-demo].[dbo].[payroll]
	WHERE 
		employer_id=@employerID AND 
		sdate >= @sdate AND 
		sdate <= @edate
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/16/2015>
-- Description:	<This stored procedure is meant to return all unique check dates for a specified period.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_check_dates](
	@employerID int,
	@mStart datetime
	)
AS
BEGIN
	SELECT cdate FROM payroll
	WHERE employer_id=@employerID 
	AND sdate > @mStart
	GROUP BY cdate
	ORDER BY cdate;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure is meant to return all EMPLOYEE CLASSIFICATIONS for a specific district.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_classifications](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM employee_classification
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/07/2014>
-- Description:	<This stored procedure is meant to return a count of employees by month using the payroll table.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_employee_count](
	@employerID int,
	@sdate datetime,
	@edate datetime
	)
AS
BEGIN
	SELECT count(DISTINCT(employee_id)) as Employee_Count, MAX(DATENAME(month, sdate)) as [Month]
	FROM [aca-demo].[dbo].[payroll]
	WHERE 
		employer_id=@employerID AND 
		sdate >= @sdate AND 
		sdate <= @edate
	GROUP BY DATEPART(month, sdate)
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/30/2014>
-- Description:	<This stored procedure is meant to return all HR status for a specific employer.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_employee_export](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [View_Employee_Export]
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/30/2014>
-- Description:	<This stored procedure is meant to return all HR status for a specific employer.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_employees](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [employee]
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_employees_in_insurance_carrier_table]
	@employerID int,
	@taxYear int
AS

BEGIN
	SELECT DISTINCT employee_id
	FROM insurance_coverage
	WHERE tax_year=@taxYear AND employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID)
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return all employees who haven't had their 1095c approved for a specific tax year.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_employees_Tax_Year_Approved](
	@employerID int,
	@taxYear int
	)
AS
BEGIN
	SELECT *
	FROM [employee]
	WHERE employer_id=@employerID AND employee_id NOT IN (Select employee_id FROM tax_year_1095c_approval WHERE tax_year=@taxYear);
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/12/2014>
-- Description:	<This stored procedure is meant to return all equivalencies for a specific District.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_equivalencies](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [View_employer_equivalency]
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/22/2015>
-- Description:	<This stored procedure is meant to return all Gross Pay Filters for a specific district.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_gross_pay_filters](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [gross_pay_filter]
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/21/2014>
-- Description:	<This stored procedure is meant to return all ACA users for a specific District.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_gross_pay_types](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [gross_pay_type]
	WHERE employer_id=@employerID
	AND active = 1
	ORDER BY description;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/22/2014>
-- Description:	<This stored procedure is meant to return all HR status for a specific employer.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_hr_status](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [hr_status]
	WHERE employer_id=@employerID
	AND active = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/31/2014>
-- Description:	<This stored procedure is meant to return all insurance alerts.>
-- Modifications:
--		3-8-2016 TLW
--			Added HRA/Flex Contributions to Alerts. 
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_insurance_alerts](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [View_insurance_alert_details]
	WHERE employer_id=@employerID and 
	((accepted is null and
	offered is null and
	acceptedOn is null) OR 
	(offered = 1 AND hra_flex_contribution IS NULL));
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/23/2016>
-- Description:	<This stored procedure is meant to return all insurance coverage rows that have not been processed.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_insurance_coverage_import_alerts](
	@employerID int
	)
AS
BEGIN
	SELECT * FROM import_insurance_coverage
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/9/2015>
-- Description:	<This stored procedure is meant to return all invoices for a specific employer.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_invoices](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [invoice]
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/6/2016>
-- Description:	<This stored procedure is meant to return a single Tax Year Approval Form.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_irs_submission](
	@employerID int,
	@taxYear int
	)
AS
BEGIN
	SELECT *
	FROM [tax_year_approval]
	WHERE 
		tax_year=@taxYear AND
		employer_id = @employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/18/2014>
-- Description:	<This stored procedure is meant to return all measurement periods for a specific employer.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_measurements](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [measurement]
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/24/2014>
-- Description:	<This stored procedure is meant to return all duplicate payroll records.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_payroll_duplicates](
	@employerID int
	)
AS
BEGIN
	SELECT
	  MAX(row_id) as ROW_ID,
      [employer_id]
      ,[employee_id]
      ,[gp_id]
	  ,[act_hours]
      ,[sdate]
      ,[edate]
      ,[cdate]
	  ,count(employer_id) as total
  FROM [aca-demo].[dbo].[payroll]
  WHERE employer_id=@employerID
  GROUP BY employer_id, employee_id, gp_id, act_hours, sdate, edate, cdate
  HAVING COUNT(employer_id) > 1
  ORDER BY employee_id, sdate
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/17/2015>
-- Description:	<This stored procedure is meant to return all payroll data that was supplemented for summer hour averaging for a specific Plan Year ID.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_payroll_summer_avg](
	@planYearID int,
	@rowCount int OUTPUT
	)
AS
BEGIN
	SELECT @rowCount=COUNT(row_id) FROM payroll_summer_averages
	WHERE plan_year_id=@planYearID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/13/2014>
-- Description:	<This stored procedure is meant to return all ACA users for a specific District.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_plan_years](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [plan_year]
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/21/2014>
-- Description:	<This stored procedure is meant to return all ACA users for a specific District.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_users](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [user]
	WHERE employer_id=@employerID
	AND active = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/10/2014>
-- Description:	<This stored procedure is meant to return all Equivalency Units.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_equivalency_units]
AS
BEGIN
	SELECT *
	FROM unit
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/14/2014>
-- Description:	<This stored procedure is meant to return all employee info that is still in the import table.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_Import_employer_employees](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [import_employee]
	WHERE employerid=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/5/2014>
-- Description:	<This stored procedure is meant to return all imported payroll data info that is still in the import table.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_import_employer_payroll](
	@employerID int
	)
AS
BEGIN
	SELECT *
	FROM [import_payroll]
	WHERE employerid=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/12/2015>
-- Description:	<This stored procedure is meant to return all contributions for each insurance plan.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_insurance_contributions](
	@insuranceID int
	)
AS
BEGIN
	SELECT *
	FROM [View_Insurance_contributions]
	WHERE insurance_id=@insuranceID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/15/2016>
-- Description:	<This stored procedure is meant to return a single template for insurance carrier reports.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_insurance_coverage_template](
	@carrierID int
)
AS
BEGIN
	SELECT *
	FROM [insurance_carrier_import_template]
	WHERE carrier_id = @carrierID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/24/2014>
-- Description:	<This stored procedure is meant to return all measurement types.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_measurement_types]
AS

BEGIN
	SELECT *
	FROM [measurement_type]
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <11/10/2015>
-- Description:	<This stored procedure is meant to return all invoices that are currently open.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_open_invoices]
AS
BEGIN
	SELECT *
	FROM [invoice]
	WHERE paymentConfirmed IS NULL or paymentConfirmed = 0;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/15/2014>
-- Description:	<This stored procedure is meant to return all measurement periods for the specific values.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_past_due_measurement_periods]
AS
BEGIN
	SELECT *
	FROM measurement
	WHERE meas_end < SYSDATETIME() and (meas_complete is null or meas_complete = 0);
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/1/2016>
-- Description:	<This stored procedure is meant to return all payroll data for a specific batch ID.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_payroll_batch](
	@batchID int,
	@employerID int
	)
AS
BEGIN
	SELECT * FROM View_payroll
	WHERE employer_id=@employerID AND
	batch_id=@batchID
	ORDER BY batch_id ASC;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to return all insurance plans related to a plan year.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_plan_year_insurance_plan](
	@planyearID int
	)
AS
BEGIN
	SELECT *
	FROM [insurance]
	WHERE plan_year_ID=@planyearID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/15/2014>
-- Description:	<This stored procedure is meant to return all measurement periods for the specific values.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_planyear_measurement](
	@employerID int, 
	@planyearID int,
	@employeeTypeID int
	)
AS
BEGIN
	SELECT *
	FROM [measurement]
	WHERE employer_id=@employerID
	AND plan_year_id = @planyearID
	AND employee_type_id = @employeeTypeID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/17/2014>
-- Description:	<This stored procedure is meant to return all positions.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_positions]
AS

BEGIN
	SELECT *
	FROM [equiv_position]
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/12/2016>
-- Description:	<This stored procedure is meant to return a single employee.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_single_employee](
	@employeeID int
	)
AS
BEGIN
	SELECT *
	FROM [employee]
	WHERE employee_id=@employeeID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/24/2014>
-- Description:	<This stored procedure is meant to return all measurement periods for the specific values.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_specific_measurements](
	@employerID int, 
	@planyearID int,
	@employeeTypeID int,
	@measTypeID int
	)
AS
BEGIN
	SELECT *
	FROM [measurement]
	WHERE employer_id=@employerID
	AND plan_year_id = @planyearID
	AND employee_type_id = @employeeTypeID 
	AND measurement_type_id = @measTypeID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/30/2014>
-- Description:	<This stored procedure is meant to return a specific Vendor.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_vendor](
	@vendorID int
	)
AS
BEGIN
	SELECT *
	FROM [payroll_vendor]
	WHERE vendor_id=@vendorID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/12/2016>
-- Description:	<This stored procedure is meant to move a finalized 1095C form to the edited table.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_ETL_ShortBuild]
	@employerID int,
	@taxYear int,
	@employeeID int
AS

BEGIN
	-- Extract Employee Info through AIR Process.
	EXEC [air-demo].etl.spETL_ShortBuild
		@employerid, 
		@taxYear, 
		@employeeID
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/22/2016>
-- Description:	<This stored procedure is meant to move a finalized 1095C form to the edited table.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_INSERT_approved_monthly_detail]
	@employeeID int,
	@timeFrameID int,
	@employerID int,
	@monthlyHours decimal,
	@oocCode nchar(2),
	@mecOffered bit,
	@lcmp decimal,
	@ash nchar(2),
	@enrolled bit,
	@monthlyStatusID int, 
	@insuranceTypeID int,
	@modBy varchar(50),
	@modOn datetime
AS

BEGIN
	INSERT INTO [air-demo].emp.monthly_detail_edited(
		employee_id,
		time_frame_id,
		employer_id,
		monthly_hours,
		offer_of_coverage_code,
		mec_offered,
		share_lowest_cost_monthly_premium,
		safe_harbor_code,
		enrolled,
		monthly_status_id,
		insurance_type_id,
		modBy,
		modOn)
	VALUES(
		@employeeID,
		@timeFrameID,
		@employerID,
		@monthlyHours,
		@oocCode,
		@mecOffered,
		@lcmp,
		@ash,
		@enrolled,
		@monthlyStatusID,
		@insuranceTypeID,
		@modOn,
		@modBy)

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/21/2016>
-- Description:	<This stored procedure is meant to return all 4980H codes.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_4980H_codes]
AS

BEGIN
	SELECT * FROM [air-demo].[il]._4980H_code
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/7/2016>
-- Description:	<This stored procedure is meant to return all LINE 3 info.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employee_LINE3_coverage]
	@employeeID int
AS

BEGIN
	WITH cim AS
(
SELECT cim.covered_individual_id, time_frame_id, CAST(covered_indicator AS INT) AS COV_IND
FROM [air-demo].emp.covered_individual_monthly_detail cim
              INNER JOIN [air-demo].emp.covered_individual ci ON (cim.covered_individual_id = ci.covered_individual_id)
WHERE ci.employee_id = @employeeID       
),
cim_pivoted AS
(
SELECT covered_individual_id, [13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24]
FROM cim
PIVOT (MAX(COV_IND)  FOR time_frame_id in([13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24])) as cip
)
SELECT ci.employee_id, ci.first_name, ci.last_name, ci.ssn, ci.birth_date, cip.* 
FROM cim_pivoted cip
              INNER JOIN [air-demo].emp.covered_individual ci ON (cip.covered_individual_id = ci.covered_individual_id)
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/25/2016>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_employee_ids]
	@employerID int
AS

BEGIN
	SELECT DISTINCT employee_id
	FROM [air-demo].appr.employee_monthly_detail
	WHERE employer_id=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:
--	4-18-2016	TLW
--		- Started using the the appr.employee_yearly_detail for the source of the Employee ID's. 
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_employees_in_yearly_detail]
	@employerID int,
	@taxYear int
AS

BEGIN
	SELECT DISTINCT employee_id
	FROM [air-demo].appr.employee_yearly_detail
	WHERE _1095C=1 and employer_id=@employerID and year_id=@taxYear;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/21/2016>
-- Description:	<This stored procedure is meant to return all activities.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_mec_codes]
AS

BEGIN
	SELECT * FROM [air].[il].mec_code
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/21/2016>
-- Description:	<This stored procedure is meant to return all monthly detail records for an employee.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_monthly_detail]
	@employeeID int
AS
BEGIN
	SELECT * FROM [air].appr.employee_monthly_detail
	WHERE employee_id=@employeeID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/16/2016>
-- Description:	<This stored procedure is meant to return all Monthly Status rows from the AIR system.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_status_codes]
AS

BEGIN
	SELECT * FROM [air].[emp].monthly_status
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_Time_Frame_Months]
	@taxYear int
AS

BEGIN
	Select time_frame_id FROM [air-demo].gen.time_frame WHERE year_id=@taxYear;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/27/2016>
-- Description:	<This stored procedure is meant to move a finalized 1095C form to the edited table.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_UPDATE_approve_monthly_detail]
	@employeeID int,
	@timeFrameID int,
	@modBy varchar(50),
	@modOn datetime
AS

BEGIN
	UPDATE [air].[appr].employee_monthly_detail
	SET
		modified_by=@modBy,
		modified_date=@modOn
	WHERE
		employee_id=@employeeID AND
		time_frame_id=@timeFrameID

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/22/2016>
-- Description:	<This stored procedure is meant to move a finalized 1095C form to the edited table.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_UPDATE_approved_monthly_detail]
	@employeeID int,
	@timeFrameID int,
	@employerID int,
	@hours decimal(18,2),
	@ooc nchar(2),
	@mec bit, 
	@lcmp decimal,
	@ash nchar(2),
	@enrolled bit,
	@monthlyStatusID int,
	@insuranceTypeID int,
	@modBy varchar(50),
	@modOn datetime
AS

BEGIN

DECLARE @employeeID2 int;
SET @employeeID2=0;

UPDATE [air-demo].[appr].employee_monthly_detail
	SET
		offer_of_coverage_code=@ooc,
		mec_offered=@mec,
		share_lowest_cost_monthly_premium=@lcmp,
		safe_harbor_code=@ash,
		enrolled=@enrolled,
		monthly_status_id=@monthlyStatusID,
		insurance_type_id=@insuranceTypeID,
		modified_by=@modBy,
		modified_date=@modOn
	WHERE
		employee_id=@employeeID AND
		time_frame_id=@timeFrameID

IF @@ROWCOUNT = 0
	BEGIN
		/*************************************************************
		****** Changed this to look at monthly detail as some employees 
		existedin the system, but had no Monthly Details to write too. 
		*************************************************************/
		--SELECT @employeeID2=employee_id FROM [air].[emp].employee 
		--WHERE employee_id=@employeeID;
		SELECT @employeeID2=employee_id FROM [air-demo].appr.employee_monthly_detail
		WHERE employee_id=@employeeID;

		PRINT 'EMPLOYEE ID: ' + CONVERT(varchar, @employeeID2);

		IF @employeeID2 = 0
			BEGIN
				-- Extract Employee Info through AIR Process.
				EXEC [air-demo].etl.spETL_Build
					@employerid, 
					2015, 
					@employeeID
			END

		UPDATE [air-demo].[appr].employee_monthly_detail
			SET
				offer_of_coverage_code=@ooc,
				mec_offered=@mec,
				share_lowest_cost_monthly_premium=@lcmp,
				safe_harbor_code=@ash,
				enrolled=@enrolled,
				monthly_status_id=@monthlyStatusID,
				insurance_type_id=@insuranceTypeID,
				modified_by=@modBy,
				modified_date=@modOn
			WHERE
				employee_id=@employeeID AND
				time_frame_id=@timeFrameID
	END

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/7/2016>
-- Description:	<This stored procedure is meant to update the AIR Individual Covered Table.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_UPDATE_covered_individual]
	@covIndID int,
	@first_name varchar(50),
	@last_name varchar(50),
	@ssn varchar(50), 
	@dob datetime 
AS

BEGIN
	UPDATE [air].[emp].covered_individual
	SET
		first_name=@first_name,
		last_name=@last_name,
		ssn=@ssn,
		birth_date=@dob
	WHERE
		covered_individual_id=@covIndID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/18/2014>
-- Description:	<This stored procedure is meant update an existing employee from the import table. It will then Delete the record from the import_employee table.
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[TRANSFER_import_existing_employee]
	@employeeID int,
	@rowID int,
	@employeeTypeID int, 
	@hrStatusID int, 
	@employerID int, 
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@address varchar(50),
	@city varchar(50),
	@stateID int,
	@zip varchar(15),
	@hdate datetime, 
	@cdate datetime, 
	@ssn varchar(50),
	@externalEmployeeID varchar(50),
	@tdate datetime, 
	@dob datetime, 
	@impEnd datetime, 
	@modOn datetime,
	@modBy varchar(50),
	@planyearid int, 
	@classID int, 
	@actStatusID int
AS

BEGIN

	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	BEGIN TRANSACTION
		BEGIN TRY
			DECLARE @default varchar(50);
			SET @default = 'All Employees';

			-- Step 1: Create a new EMPLOYEE based on the registration information.
			--			- Return the new Employee_ID
			EXEC UPDATE_employee 
				@employeeID,
				@employeeTypeID,
				@hrStatusID,
				@fname,
				@mname,
				@lname,
				@address,
				@city,
				@stateID,
				@zip,
				@hdate,
				@cdate,
				@ssn,
				@externalEmployeeID,
				@tdate,
				@dob,
				@impEnd,
				@planyearid, 
				@modOn,
				@modBy, 
				@classID, 
				@actStatusID;
	
			-- Step 2: DELETE the record from the import_employee table. 
				DELETE FROM import_employee 
				WHERE
					rowid = @rowID;
			COMMIT
		END TRY
		BEGIN CATCH
			--If something fails return 0.
			SET @employerID = 0;
			ROLLBACK TRANSACTION
		END CATCH

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/18/2014>
-- Description:	<This stored procedure is meant update an existing employee from the import table. It will then Delete the record from the import_employee table.
-- Changes:		
-- =============================================
CREATE PROCEDURE [dbo].[TRANSFER_import_existing_insurance_carrier_imports]
	@rowID int,
	@taxYear int,
	@carrierID int,
	@employeeID int, 
	@dependentID int, 
	@all12 bit, 
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
	@dec bit, 
	@history varchar(max)
AS

BEGIN

	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	BEGIN TRANSACTION
		BEGIN TRY
			DECLARE @default varchar(50);
			SET @default = 'All Employees';

			-- Step 1: Create a new EMPLOYEE based on the registration information.
			--			- Return the new Employee_ID
			INSERT INTO [insurance_coverage](
				tax_year,
				carrier_id,
				employee_id,
				dependent_id,
				all12,
				jan,
				feb,
				mar,
				apr,
				may,
				jun,
				jul,
				aug,
				sep,
				oct,
				nov,
				[dec],
				history
				)
			VALUES(
				@taxYear,
				@carrierID,
				@employeeID,
				@dependentID,
				@all12,
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
				@dec,
				@history
				)
	
			-- Step 2: DELETE the record from the import_employee table. 
				DELETE FROM import_insurance_coverage
				WHERE
					row_id = @rowID;
			COMMIT
		END TRY
		BEGIN CATCH

			ROLLBACK TRANSACTION
		END CATCH

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/14/2014>
-- Description:	<This stored procedure is meant transfer a new employee from the import table over to the current employee table.
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[TRANSFER_import_new_employee]
	@offer bit,
	@offerPlanYearID int,
	@rowID int,
	@employeeTypeID int, 
	@hrStatusID int, 
	@employerID int, 
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@address varchar(50),
	@city varchar(50),
	@stateID int,
	@zip varchar(15),
	@hdate datetime, 
	@cdate datetime, 
	@ssn varchar(50),
	@externalEmployeeID varchar(50),
	@tdate datetime, 
	@dob datetime, 
	@impEnd datetime, 
	@planyearid int,
	@modOn datetime,
	@modBy varchar(50),
	@classID int, 
	@actStatusID int, 
	@employeeID int OUTPUT
AS

BEGIN
	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	BEGIN TRANSACTION
		BEGIN TRY
			DECLARE @default varchar(50);
			DECLARE @monthlyAvg decimal(10,2);

			SET @default = 'All Employees';
			SET @monthlyAvg = 0;
			DECLARE @newID int;

			-- Step 1: Create a new EMPLOYEE based on the registration information.
			--			- Return the new Employee_ID
				EXEC INSERT_new_employee 
				@employeeTypeID,
				@hrStatusID,
				@employerID,
				@fname,
				@mname,
				@lname,
				@address,
				@city,
				@stateID,
				@zip,
				@hdate,
				@cdate,
				@ssn,
				@externalEmployeeID,
				@tdate,
				@dob,
				@impEnd,
				@planyearid,
				@modOn,
				@modBy,
				@classID,
				@actStatusID,
				@newID OUTPUT;

				SELECT @newID;
				SET @employeeID = @newID;

			-- Step 2: Create an Insurance Alert if the Employee is going to recieve an Offer.
				IF (@offer = 1)
					BEGIN
						DECLARE @history varchar(100);
						SET @history = 'Record Created On:' + Convert(varchar, @modOn, 0) + ' by ' + @modBy + '.';

						EXEC INSERT_new_insurance_offer
						@employerID,
						@employeeID,
						@offerPlanYearID, 
						@monthlyAvg,
						@modBy,
						@modOn, 
						@history
					END


			-- Step 3: DELETE the record from the import_employee table. 
				DELETE FROM import_employee 
				WHERE
					rowid = @rowID;
			COMMIT
		END TRY
		BEGIN CATCH
			--If something fails return 0.
			SET @employerID = 0;
			ROLLBACK TRANSACTION
		END CATCH

		SELECT @employeeID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/17/2014>
-- Description:	<This stored procedure is meant transfer a new payroll record from the import table over to the current payroll table.
-- Changes:
--			5-27-2015 TLW
--				- Added the history parameter to record the initial data. 
-- =============================================
CREATE PROCEDURE [dbo].[TRANSFER_import_new_payroll] 
	@rowID int,
	@employerID int,
	@batchID int, 
	@employeeID int,
	@gpID int, 
	@hours numeric(10,4),
	@sdate datetime, 
	@edate datetime, 
	@cdate datetime, 
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(2000)
AS

BEGIN
	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	BEGIN TRANSACTION
		BEGIN TRY
			-- Step 1: Create a new EMPLOYEE based on the registration information.
			--			- Return the new Employee_ID
				EXEC INSERT_new_payroll 
					@employerID,
					@batchID, 
					@employeeID,
					@gpID, 
					@hours,
					@sdate, 
					@edate, 
					@cdate, 
					@modBy,
					@modOn,
					@history
	
			-- Step 2: DELETE the record from the import_employee table. 
				EXEC DELETE_payroll_import_row
					@rowID
			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
		END CATCH

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/1/2016>
-- Description:	<This stored procedure is meant update an existing employee from the import table. It will then Delete the record from the import_employee table.
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[TRANSFER_insurance_change_event]
	@rowID int,
	@insuranceID int,
	@contributionID int,
	@avgHours decimal,
	@offered bit, 
	@offeredOn datetime,
	@accepted bit,
	@acceptedOn datetime,
	@modOn datetime,
	@modBy varchar(50),
	@notes varchar(max),
	@history varchar(max),
	@effDate datetime,
	@hraFlex decimal
AS

BEGIN

	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	BEGIN TRANSACTION
		BEGIN TRY
			-- Step 1: Archive the current insurance offer.
			--			- Return the new Employee_ID
			INSERT INTO dbo.employee_insurance_offer_archive 
			SELECT * FROM dbo.employee_insurance_offer
			WHERE rowid=@rowID;

			-- Step 2: Update the new insurance offer.
			EXEC UPDATE_insurance_offer
				@rowID,
				@insuranceID,
				@contributionID,
				@avgHours,
				@offered, 
				@offeredOn,
				@accepted,
				@acceptedOn,
				@modOn,
				@modBy,
				@notes,
				@history,
				@effDate,
				@hraFlex;
	
			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
		END CATCH

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/14/2014>
-- Description:	<This stored procedure is meant to update a specifice Employer Employee.>
--		- 5-20-2015 TLW
--			Changed the process to utilize the meas_plan_year_id for employees. The only time the plan_year_id and limbo_plan_year_id can change
--		is when the measurement or plan year are rolled over. 
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee]
	@employee_id int,
	@employeeTypeID int,
	@hrstatusID int, 
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@address varchar(50),
	@city varchar(50),
	@stateID int,
	@zip varchar(5),
	@hdate datetime,
	@cdate datetime,
	@ssn varchar(50),
	@extemployeeid varchar(50),
	@tdate datetime,
	@dob datetime,
	@impEnd datetime,
	@planyearid int,
	@modOn datetime,
	@modBy varchar(50), 
	@classID int, 
	@actStatusID int
AS
BEGIN
	UPDATE employee
	SET
		employee_type_id=@employeeTypeID,
		HR_status_id = @hrstatusID,
		fName = @fname,
		mName = @mname,
		lName = @lname, 
		[address] = @address,
		city = @city,
		state_id = @stateID, 
		zip = @zip,  
		hireDate=@hdate,
		currDate=@cdate,
		ssn=@ssn,
		ext_emp_id=@extemployeeid,
		terminationDate=@tdate,
		dob=@dob,
		initialMeasurmentEnd=@impEnd,
		meas_plan_year_id=@planyearid,
		modOn=@modOn,
		modBy=@modBy, 
		classification_id=@classID,
		aca_status_id=@actStatusID
	WHERE
		employee_id=@employee_id;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/7/2015>
-- Description:	<This stored procedure is meant to update a specifice Employer Employee.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_AVG_MONTHLY_HOURS]
	@employee_id int,
	@pyAvg numeric(18,4),
	@lpyAvg numeric(18,4),
	@mpyAvg numeric(18,4), 
	@impAvg numeric(18,4)
AS
BEGIN
	UPDATE employee
	SET
		plan_year_avg_hours=@pyAvg,
		limbo_plan_year_avg_hours=@lpyAvg,
		meas_plan_year_avg_hours=@mpyAvg,
		imp_plan_year_avg_hours=@impAvg
	WHERE
		employee_id=@employee_id;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/29/2015>
-- Description:	<This stored procedure is meant to update a specifice Employer Employee.>
--		I used employer and employee ID in the WHERE clause as the Employer is importing this data and 
--change the Employee ID. This will make sure they don't change that ID to another Employer's Employee ID. 
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_class]
	@employerID int,
	@employeeID int,
	@classID int,
	@acaID int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN
	UPDATE employee
	SET
		classification_id=@classID,
		aca_status_id=@acaID,
		modOn=@modOn,
		modBy=@modBy

	WHERE
		employee_id=@employeeID AND employer_id=@employerID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure is meant to update the EMPLOYER Classification.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_classification]
	@classID int,
	@description varchar(50), 
	@ashCode varchar(2),
	@modBy varchar(50), 
	@modOn datetime, 
	@history varchar(max)
AS
BEGIN
	UPDATE employee_classification
	SET
		[description] = @description,
		ash_code = @ashCode,
		modOn = @modOn,
		modBy = @modBy,
		history=@history
	WHERE
		classification_id=@classID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/12/2016>
-- Description:	<This stored procedure is meant to update data from LINE III.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_LINEIII_DOB]
	@employee_id int,
	@modOn DateTime, 
	@modBy varchar(50),	
	@dob datetime,
	@fname varchar(50), 
	@lname varchar(50)
AS
BEGIN
	UPDATE employee
	SET
		dob=@dob,
		fName=@fname,
		lName=@lname,
		modOn=@modOn,
		modBy=@modBy
	WHERE
		employee_id=@employee_id;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/12/2016>
-- Description:	<This stored procedure is meant to update data from LINE III.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_LINEIII_Months](
	@row_id int,
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
	@dec bit,
	@history varchar(max))
AS
BEGIN
	UPDATE insurance_coverage_editable
	SET
		Jan=@jan,
		Feb=@feb,
		Mar=@mar,
		Apr=@apr,
		May=@may,
		Jun=@jun,
		Jul=@jul,
		Aug=@aug,
		Sept=@sep,
		Oct=@oct,
		Nov=@nov,
		[Dec]=@dec
	WHERE
		row_id=@row_id;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/12/2016>
-- Description:	<This stored procedure is meant to update data with SSN from LINE III.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_LINEIII_SSN]
	@employee_id int,
	@modOn DateTime, 
	@modBy varchar(50),	
	@ssn varchar(50),
	@fname varchar(50), 
	@lname varchar(50)
AS
BEGIN
	UPDATE employee
	SET
		ssn=@ssn,
		fName=@fname,
		lName=@lname,
		modOn=@modOn,
		modBy=@modBy

	WHERE
		employee_id=@employee_id;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/30/2015>
-- Description:	<This stored procedure is meant to update the plan_year_id and admin_plan_year_id. 
-- The admin_year_id moves to the plan_year_id and admin_plan_year_id is set to NULL.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_plan_year]
	@employerID int,
	@employeeTypeID int,
	@adminPlanYearID int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY

		UPDATE employee
		SET
			plan_year_id=@adminPlanYearID,
			limbo_plan_year_id=NULL,
			modOn=@modOn,
			modBy=@modBy
		WHERE
			employer_id=@employerID AND
			employee_type_id=@employeeTypeID AND
			limbo_plan_year_id=@adminPlanYearID;
		
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/07/2014>
-- Description:	<This stored procedure is meant to update the measurement_plan_year_id and limbo_plan_year_id. Since this is the first rollover period
-- it must also archive the data for the employee. The meas_plan_year_id moves to the limbo_plan_year_id and the new plan_year_id is placed in the meas_plan_year_id.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_plan_year_meas]
	@employerID int,
	@employeeTypeID int,
	@planYearID int,
	@newPlanYearID int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY
		DECLARE @employeeID int;
		DECLARE @history varchar(100);
		DECLARE @currAvg decimal(10,2);
		DECLARE @nhAvg decimal(10,2);
		DECLARE @measAvg decimal(10,2);
		DECLARE @impAvg decimal(10,2);
		DECLARE @acaStatusID int;
		DECLARE @measStart datetime;
		DECLARE @hireDate datetime;

		SET @history = 'Record Created On:' + Convert(varchar, @modOn, 0) + ' by ' + @modBy + '.';

		/***************************************************************************
		Step 1: Get the Current Measurement Period Start Date.
		***************************************************************************/
		SELECT @measStart=meas_start FROM measurement WHERE plan_year_id=@planYearID;

		/*******************************************************************************************************************
		Step 1: Create Offer of Insurance. 
		The INSERT INTO below creates the actual insurance offers. 
			- EmployerID, EmployeeTypeID, PlanYearID match up with the selections the user has passed in. 
			- Hire Date needs to be evaluated as any Employee who has been hired after the Measurement Period has started
				is viewed as a New Hire or a person that is in their IMP or Initial Measurement Period so they would not be offered
				Insurance with all of the ONGOING employees, unless: The ACA STATUS of this employee is set to FULL TIME. 
			- ACA hours were not taken into consideration as many employers offer insurance to employees who work less than
				the fulltime hours of 130 per month. We just make an offer for every person and let them decide what they want to do with it. 
			- An OFFER OF INSURANCE will NOT be created for any employee that has an ACA STATUS of TERMED.
			- ACA Status Codes are universal for all employers in the system.
			     - Seasonal = 1
				 - Part Time/Variable = 2
				 - Termed = 3
				 - Cobra Elected = 4
				 - Full time = 5
				 - Special Unpaid Leave = 6
				 - Initial Import = 7
		*******************************************************************************************************************/
		DECLARE employee_cursor CURSOR FOR 
			SELECT employee_id, hireDate, meas_plan_year_avg_hours, imp_plan_year_avg_hours, aca-demo_status_id  
			FROM employee 
			WHERE employer_id=@employerID and meas_plan_year_id=@planYearID;
			-- AND aca_status_id NOT IN (3, 4);
		OPEN employee_cursor;
		FETCH NEXT FROM employee_cursor
		INTO @employeeID, @hireDate, @measAvg, @nhAvg, @acaStatusID; 

		WHILE @@FETCH_STATUS = 0
			BEGIN
				IF @hireDate >= @measStart
					BEGIN
						PRINT 'NEW HIRE';
						SET @currAvg = @nhAvg;
					END
				ELSE
					BEGIN
						PRINT 'ONGOING';
						SET @currAvg = @measAvg;
					END

				PRINT @currAvg;

				BEGIN TRY
					INSERT INTO [employee_insurance_offer](
						employee_id,
						plan_year_id,
						employer_id,
						avg_hours_month,
						modOn,
						modBy,
						history)
					VALUES(
						@employeeID,
						@planYearID,
						@employerID,
						@currAvg,
						@modOn,
						@modBy,
						@history)
				END TRY
				BEGIN CATCH
					PRINT 'Failed';
				END CATCH

				FETCH NEXT FROM employee_cursor
				INTO @employeeID, @hireDate, @measAvg, @nhAvg, @acaStatusID; 
			END
		
		CLOSE employee_cursor;
		DEALLOCATE employee_cursor;

		/******************************************************************************************************************
		Step 2: UPDATE the Plan Year Columns 
			- The UPDATE statement below will move the meas_plan_year_id to the limbo_plan_year_id aka(admin_plan_year_id) 
				used the wrong name on that one. The new Measurement Plan Year ID is updated in the meas_plan_year_id. 
			- This affects each EMPLOYEE regardless of when they were hired. 
		******************************************************************************************************************/
		UPDATE employee
		SET
			meas_plan_year_id=@newPlanYearID,
			limbo_plan_year_id=@planYearID,
			modOn=@modOn,
			modBy=@modBy
		WHERE
			employer_id=@employerID AND
			employee_type_id=@employeeTypeID AND
			meas_plan_year_id=@planYearID;
		
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/17/2015>
-- Description:	<This stored procedure is meant to update the measurement_plan_year_id >
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_plan_year_meas_id]
	@employeeID int,
	@planYearID int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY
		

		/******************************************************************************************************************
		Step 2: UPDATE the Plan Year Columns 
			- The UPDATE statement below will move the meas_plan_year_id to the limbo_plan_year_id aka(admin_plan_year_id) 
				used the wrong name on that one. The new Measurement Plan Year ID is updated in the meas_plan_year_id. 
			- This affects each EMPLOYEE regardless of when they were hired. 
		******************************************************************************************************************/
		UPDATE employee
		SET
			meas_plan_year_id=@planYearID,
			modOn=@modOn,
			modBy=@modBy
		WHERE
			employee_id=@employeeID;
		
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/22/2014>
-- Description:	<This stored procedure is meant to update a specifice Employee SSN.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employee_ssn]
	@employee_id int,
	@modOn DateTime, 
	@modBy varchar(50),	
	@ssn varchar(50),
	@hrStatusID int,
	@classID int, 
	@acaStatusID int
AS
BEGIN
	UPDATE employee
	SET
		ssn=@ssn,
		HR_status_id=@hrStatusID,
		classification_id=@classID,
		aca_status_id=@acaStatusID,
		modOn=@modOn,
		modBy=@modBy

	WHERE
		employee_id=@employee_id;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <04/22/2014>
-- Description:	<This stored procedure is meant to update the district.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employer]
	@employerID int,
	@name varchar(50),
	@address varchar(50),
	@city varchar(50),
	@stateID int,
	@zip varchar(15),
	@logo varchar(50),
	@ein varchar(50)
AS
BEGIN
	UPDATE employer
	SET
		name = @name, 
		[address] = @address,
		city = @city,
		state_id = @stateID, 
		zip = @zip,  
		img_logo = @logo,
		ein = @ein
	WHERE
		employer_id = @employerID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/1/2014>
-- Description:	<This stored procedure is meant to update the employer.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employer_measurement]
	@measurementID int,
	@employerID int
AS
BEGIN
	UPDATE employer
	SET
		initial_measurement_id=@measurementID
	WHERE
		employer_id = @employerID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/27/2014>
-- Description:	<This stored procedure is meant to update the district's Setup Process.>
-- Updates:
--	2-2-2015
--		- Added the AutoBill and AutoUpload fields.
--  3-10-2015
--      - Added the File SU columns, so EBC can manage the file names. 
--  12-28-2015
--		- Added the IO SU column.
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employer_setup]
	@employerID int,
	@iei varchar(50),
	@iec varchar(50),
	@ftpei varchar(50),
	@ftpec varchar(50),
	@ipi varchar(50),
	@ipc varchar(50),
	@ftppi varchar(50), 
	@ftppc varchar(50),
	@ip varchar(50), 
	@billing bit,
	@fileUpload bit,
	@paySU varchar(20), 
	@demoSU varchar(20),
	@gpSU varchar(20),
	@hrSU varchar(20),
	@vendorID int,
	@ecSU varchar(20),
	@ioSU varchar(10), 
	@icSU varchar(10),
	@payMod varchar(10)
AS
BEGIN
	UPDATE employer
	SET
		iei = @iei, 
		iec = @iec,
		ftpei = @ftpei,
		ftpec = @ftpec, 
		ipi = @ipi,  
		ipc = @ipc,
		ftppi = @ftppi,
		ftppc = @ftppc,
		importProcess = @ip, 
		autoBill = @billing,
		autoUpload = @fileUpload,
		import_demo = @demoSU,
		import_payroll = @paySU,
		import_gp = @gpSU,
		import_hr = @hrSU, 
		vendor_id = @vendorID,
		import_ec = @ecSU,
		import_io = @ioSU,
		import_ic = @icSU,
		import_pay_mod = @payMod
	WHERE
		employer_id = @employerID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <01/26/2015>
-- Description:	<This stored procedure is meant to update the EMPLOYER SU FEE.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_employer_su_fee]
	@employerID int,
	@sufee bit
AS
BEGIN
	UPDATE employer
	SET
		suBilled = @sufee 
	WHERE
		employer_id = @employerID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/23/2014>
-- Description:	<This stored procedure is meant to update a new equivelancy.>
-- Changes: 	
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_equivalency]
	@equivalencyID int,
	@employerID int,
	@name varchar(50),
	@gpID int,
	@every decimal(18,4),
	@unitID int,
	@credit decimal(18,4),
	@sdate datetime,
	@edate datetime,
	@notes varchar(1000),
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max),
	@active bit,
	@equivTypeID int,
	@posID int,
	@actID int,
	@detID int
AS

BEGIN
	UPDATE [equivalency]
	SET
		employer_id = @employerID,
		name = @name,
		gpID = @gpID,
		every = @every,
		unit_id = @unitID,
		credit = @credit,
		[start_date] = @sdate,
		end_date = @edate,
		notes = @notes,
		modBy = @modBy,
		modOn = @modOn,
		history = @history,
		active = @active,
		equivalency_type_id = @equivTypeID,
		position_id = @posID,
		activity_id = @actID,
		detail_id = @detID
	WHERE
		equivalency_id = @equivalencyID

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/7/2015>
-- Description:	<This stored procedure is meant to update the Gross Pay Description.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_gp_description]
	@gpID int,
	@name varchar(50)
AS
BEGIN
	UPDATE gross_pay_type
	SET
		[description] = @name
	WHERE
		gross_pay_id=@gpID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/7/2015>
-- Description:	<This stored procedure is meant to update the EMPLOYER SU FEE.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_hr_status]
	@hrStatusID int,
	@name varchar(50)
AS
BEGIN
	UPDATE hr_status
	SET
		name = @name
	WHERE
		HR_status_id=@hrStatusID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/14/2014>
-- Description:	<This stored procedure is meant to update the import_employee table.>
-- Updates:
--		- TW 9/25/2015
--		- Added class_id and aca_status_id per our new standards for Employee records. 
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_import_employee]
	@rowID int, 
	@employeeTypeID int,
	@hrstatusID int, 
	@hrStatusExt varchar(50),
	@hrStatusDesc varchar(50),
	@planYearID int,
	@stateID int,
	@hdate datetime,
	@hdateI varchar(8),
	@cdate datetime,
	@tdate datetime,
	@tdateI varchar(8),
	@dob datetime,
	@dobI varchar(8),
	@impEnd datetime,
	@classID int,
	@acaStatusID int
AS
BEGIN
	UPDATE import_employee
	SET
		employeeTypeID=@employeeTypeID,
		HR_status_id = @hrstatusID,
		hr_status_ext_id = @hrStatusExt,
		hr_description = @hrStatusDesc,
		planYearID = @planYearID,
		stateID = @stateID,  
		hDate=@hdate,
		i_hDate=@hdateI,
		cDate=@cdate,
		tDate=@tdate,
		i_tDate=@tdateI,
		dob=@dob,
		i_dob=@dobI,
		impEnd=@impEnd, 
		class_id=@classID,
		aca_status_id=@acaStatusID
	WHERE
		rowid = @rowID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4-6-2016>
-- Description:	<This stored procedure is meant to update the import_insurance_coverage table.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_import_insurance_carrier]
	@rowID int,
	@employerID int,
	@employeeID int, 
	@dependentID int, 
	@taxYear int, 
	@fName varchar(50),
	@lName varchar(50),
	@ssn varchar(50),
	@dob datetime
AS
BEGIN
	UPDATE import_insurance_coverage
	SET
		fName=@fName,
		lName=@lName,
		ssn=@ssn,
		dob=@dob
	WHERE
		row_id = @rowID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/15/2014>
-- Description:	<This stored procedure is meant to update the import_payroll table.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_import_payroll]
	@rowID int,
	@employeeID int, 
	@grossPayDescID int,
	@actHours decimal(10,4), 
	@sdate datetime,
	@edate datetime,
	@cdate datetime,
	@modBy varchar(50),
	@modOn datetime
AS
BEGIN
	UPDATE import_payroll
	SET
		employee_id=@employeeID,
		gp_id = @grossPayDescID,
		[hours] = @actHours,
		sdate = @sdate,
		edate = @edate,
		cdate = @cdate, 
		modBy = @modBy,
		modOn = @modOn
	WHERE
		rowid = @rowID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/22/2015>
-- Description:	<This stored procedure is meant to update an insurance plan.>
-- Changes:
--		
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_insurance_contribution]
	@contID int,
	@insuranceID int,
	@contributionID char(1),
	@classificationID int,
	@cost money,
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(max)
AS
BEGIN
	UPDATE insurance_contribution
	SET
		insurance_id=@insuranceID,
		contribution_id=@contributionID,
		classification_id=@classificationID,
		amount=@cost,
		modBy = @modBy,
		modOn = @modOn,
		history = @history
	WHERE
		ins_cont_id=@contID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/23/2016>
-- Description:	<This stored procedure is meant to update the Employee ID for the import insurance coverage table.>
-- Changes:
--		
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_insurance_coverage_import]
	@rowID int,
	@employeeID int,
	@dependentID int,
	@fname varchar(50),
	@lname varchar(50), 
	@ssn varchar(50),
	@dob datetime, 
	@subscriber bit
AS
BEGIN
	UPDATE import_insurance_coverage
	SET 
		employee_id=@employeeID,
		dependent_id=@dependentID,
		fName=@fname,
		lName=@lname,
		ssn=@ssn,
		dob=@dob,
		subscriber=@subscriber
	WHERE
		row_id=@rowID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/25/2015>
-- Description:	<This stored procedure is meant to update an insurance offer.>
-- Changes:
--		
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_insurance_offer]
	@rowID int,
	@insuranceID int,
	@contributionID int,
	@avgHours decimal,
	@offered bit, 
	@offeredOn datetime,
	@accepted bit,
	@acceptedOn datetime,
	@modOn datetime,
	@modBy varchar(50),
	@notes varchar(max),
	@history varchar(max),
	@effDate datetime,
	@hraFlex decimal
AS
BEGIN
	UPDATE employee_insurance_offer
	SET
		insurance_id=@insuranceID,
		ins_cont_id=@contributionID,
		avg_hours_month=@avgHours,
		offered=@offered,
		offeredOn=@offeredOn,
		accepted=@accepted,
		acceptedOn=@acceptedOn,
		modOn=@modOn,
		modBy=@modBy,
		notes=@notes,
		history=@history,
		effectiveDate=@effDate,
		hra_flex_contribution=@hraFlex
	WHERE
		rowid=@rowID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to update an insurance plan.>
-- Changes:
--		5-19-2015 TLW
--			- Added the following three parameters: minValue, offSpouse, offDependent.
--		10-28-2015 TLW
--			- Added InsuranceTypeID
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_insurance_plan]
	@insuranceID int,
	@planyearID int,
	@name varchar(50),
	@cost money,
	@minValue bit,
	@offSpouse bit,
	@offDependent bit,
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50),
	@insuranceTypeID int

AS
BEGIN
	UPDATE insurance
	SET
		plan_year_id=@planyearID,
		[description] = @name,
		monthlycost = @cost,
		minValue=@minValue,
		offSpouse=@offSpouse,
		offDependent=@offDependent,
		history = @history,
		modOn = @modOn,
		modBy = @modBy,
		insurance_type_id=@insuranceTypeID
	WHERE
		insurance_id=@insuranceID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <11/10/2015>
-- Description:	<This stored procedure is meant to update the invoice once payment is recieved. 
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_invoice]
	@invoiceID int,
	@paid bit, 
	@history varchar(max)
AS
BEGIN

UPDATE invoice
SET
	paymentConfirmed=@paid,
	history=@history
WHERE
	invoice_id=@invoiceID

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/26/2014>
-- Description:	<This stored procedure is meant to update the district.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_measurement]
	@measurementID int,
	@measStart datetime,
	@measEnd datetime,
	@adminStart datetime,
	@adminEnd datetime,
	@openStart datetime,
	@openEnd datetime,
	@stabStart datetime, 
	@stabEnd datetime,
	@notes varchar(max),
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(max), 
	@swStart datetime,
	@swEnd datetime,
	@swStart2 datetime,
	@swEnd2 datetime
AS
BEGIN
	UPDATE measurement
	SET
		meas_start = @measStart,
		meas_end = @measEnd,
		admin_start = @adminStart,
		admin_end = @adminEnd,
		open_start = @openStart,
		open_end = @openEnd,
		stability_start = @stabStart,
		stability_end = @stabEnd,
		notes = @notes,
		history = @history,
		modOn = @modOn,
		modBy = @modBy, 
		sw_start = @swStart,
		sw_end = @swEnd,
		sw2_start = @swStart2,
		sw2_end = @swEnd2
	WHERE
		measurement_id = @measurementID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/15/2015>
-- Description:	<This stored procedure is meant to update the payroll table.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_payroll]
	@rowID int,
	@employerID int, 
	@employeeID int,
	@actHours decimal(10,4), 
	@sdate datetime,
	@edate datetime,
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max)
AS
BEGIN
	UPDATE payroll
	SET
		[act_hours] = @actHours,
		sdate = @sdate,
		edate = @edate, 
		modBy = @modBy,
		modOn = @modOn,
		history=@history
	WHERE
		row_id = @rowID and
		employer_id = @employerID and 
		employee_id = @employeeID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/8/2014>
-- Description:	<This stored procedure is meant to update the plan year.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_plan_year]
	@planyearID int,
	@description varchar(50),
	@sDate datetime,
	@eDate datetime,
	@notes varchar(max),
	@history varchar(max),
	@modOn datetime,
	@modBy varchar(50)

AS
BEGIN
	UPDATE plan_year
	SET
		description=@description,
		startDate = @sDate,
		endDate = @eDate,
		notes = @notes,
		history = @history,
		modOn = @modOn,
		modBy = @modBy
	WHERE
		plan_year_id=@planyearID;

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <06/02/2014>
-- Description:	<This stored procedure is meant to reset a user password 
-- if they are currently an active user.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_reset_pwd] 
	@pwd varchar(50),
	@username varchar(50),
	@email varchar(50), 
	@modBy varchar(50),
	@modOn datetime,
	@pwdReset bit
AS
BEGIN

UPDATE [user]
	SET
		[password]=@pwd,
		last_mod_by=@modBy,
		last_mod = @modOn,
		reset_pwd=@pwdReset
	WHERE
		username=@username AND 
		email=@email AND 
		active=1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <04/23/2014>
-- Description:	<This stored procedure is meant to update a specific user.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_user]
	@userID int, 
	@fname varchar(50),
	@lname varchar(50),
	@email varchar(50),
	@phone varchar(15),
	@power bit, 
	@modBy varchar(50),
	@billing bit,
	@irsContact bit,
	@modOn datetime
AS
BEGIN

UPDATE [user]
	SET
		fname=@fname, 
		lname=@lname,
		email=@email,
		phone=@phone,
		poweruser=@power, 
		last_mod_by=@modBy,
		last_mod = @modOn,
		billing=@billing,
		irsContact = @irsContact
	WHERE
		[user_id] = @userID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <11/02/2015>
-- Description:	<This stored procedure is meant to update a specific user billing contact.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_user_billing_contact]
	@userID int, 
	@billing bit,
	@modBy varchar(50),
	@modOn datetime
AS
BEGIN

UPDATE [user]
	SET 
		last_mod_by=@modBy,
		last_mod = @modOn,
		billing=@billing
	WHERE
		[user_id] = @userID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <11/10/2015>
-- Description:	<This stored procedure is meant to update the employer ID for EBC's floating User.>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_user_floating]
(
	@employerID int,
	@userID int
)
AS
BEGIN

UPDATE [user]
	SET 
		employer_id=@employerID
	WHERE
		[user_id] = @userID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/21/2014>
-- Description:	<This stored procedure is meant to validate user and return district ID.>
-- Changes:
--		<5/23/2014>
--			- Added active = 1 to the WHERE clause, so that only active users are allowed to 
-- login. 
-- =============================================
CREATE PROCEDURE [dbo].[VALIDATE_user]
	@username varchar(100),
	@password varchar(100)
AS

BEGIN
	SELECT *
	FROM [user]
	WHERE username = @username AND [password] = @password AND active = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aca_status](
	[aca_status_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_aca_status] PRIMARY KEY CLUSTERED 
(
	[aca_status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[affordability_safe_harbor](
	[ash_code] [char](2) NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_affordability_safe_harbor] PRIMARY KEY CLUSTERED 
(
	[ash_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[alert](
	[alert_id] [int] IDENTITY(1,1) NOT NULL,
	[alert_type_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
 CONSTRAINT [PK_alert] PRIMARY KEY CLUSTERED 
(
	[alert_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uc_alertype_employerID] UNIQUE NONCLUSTERED 
(
	[alert_type_id] ASC,
	[employer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[alert_archive](
	[alert_archive_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[record_count] [int] NOT NULL,
	[modBy] [varchar](50) NOT NULL,
	[modOn] [datetime] NOT NULL,
	[alert_id] [int] NOT NULL,
 CONSTRAINT [PK_alert_archive] PRIMARY KEY CLUSTERED 
(
	[alert_archive_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[alert_type](
	[alert_type_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[image_url] [varchar](50) NULL,
	[table_name] [varchar](50) NULL,
 CONSTRAINT [PK_alert_type] PRIMARY KEY CLUSTERED 
(
	[alert_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[batch](
	[batch_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[modOn] [datetime] NOT NULL,
	[modBy] [varchar](50) NOT NULL,
	[delOn] [datetime] NULL,
	[delBy] [varchar](50) NULL,
 CONSTRAINT [PK_batch] PRIMARY KEY CLUSTERED 
(
	[batch_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[contribution](
	[contribution_id] [char](1) NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_contribution] PRIMARY KEY CLUSTERED 
(
	[contribution_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employee](
	[employee_id] [int] IDENTITY(1,1) NOT NULL,
	[employee_type_id] [int] NOT NULL,
	[HR_status_id] [int] NULL,
	[employer_id] [int] NOT NULL,
	[fName] [varchar](50) NOT NULL,
	[mName] [varchar](50) NULL,
	[lName] [varchar](50) NOT NULL,
	[address] [varchar](50) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[state_id] [int] NOT NULL,
	[zip] [varchar](50) NOT NULL,
	[hireDate] [datetime] NOT NULL,
	[currDate] [datetime] NULL,
	[ssn] [varchar](50) NOT NULL,
	[ext_emp_id] [varchar](50) NULL,
	[terminationDate] [datetime] NULL,
	[dob] [datetime] NOT NULL,
	[initialMeasurmentEnd] [datetime] NOT NULL,
	[plan_year_id] [int] NULL,
	[limbo_plan_year_id] [int] NULL,
	[meas_plan_year_id] [int] NOT NULL,
	[modOn] [datetime] NULL,
	[modBy] [varchar](50) NULL,
	[plan_year_avg_hours] [numeric](18, 4) NULL,
	[limbo_plan_year_avg_hours] [numeric](18, 4) NULL,
	[meas_plan_year_avg_hours] [numeric](18, 4) NULL,
	[imp_plan_year_avg_hours] [numeric](18, 4) NULL,
	[classification_id] [int] NULL,
	[aca_status_id] [int] NULL,
 CONSTRAINT [PK_employee] PRIMARY KEY CLUSTERED 
(
	[employee_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uc_employer_ssn] UNIQUE NONCLUSTERED 
(
	[employer_id] ASC,
	[ssn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employee_classification](
	[classification_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[description] [varchar](50) NOT NULL,
	[modOn] [datetime] NOT NULL,
	[modBy] [varchar](50) NOT NULL,
	[history] [varchar](max) NOT NULL,
	[ash_code] [char](2) NULL,
 CONSTRAINT [PK_employee_classification] PRIMARY KEY CLUSTERED 
(
	[classification_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employee_dependents](
	[dependent_id] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[fName] [varchar](50) NOT NULL,
	[mName] [varchar](50) NULL,
	[lName] [varchar](50) NOT NULL,
	[ssn] [varchar](50) NULL,
	[dob] [datetime] NULL,
 CONSTRAINT [PK_employee_dependents] PRIMARY KEY CLUSTERED 
(
	[dependent_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employee_insurance_offer](
	[rowid] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[plan_year_id] [int] NOT NULL,
	[employer_id] [int] NULL,
	[insurance_id] [int] NULL,
	[ins_cont_id] [int] NULL,
	[avg_hours_month] [decimal](10, 2) NULL,
	[offered] [bit] NULL,
	[offeredOn] [datetime] NULL,
	[accepted] [bit] NULL,
	[acceptedOn] [datetime] NULL,
	[modOn] [datetime] NULL,
	[modBy] [varchar](50) NULL,
	[notes] [varchar](1000) NULL,
	[history] [varchar](max) NULL,
	[effectiveDate] [datetime] NULL,
	[hra_flex_contribution] [decimal](10, 2) NULL,
 CONSTRAINT [PK_employee_py_archives_1] PRIMARY KEY CLUSTERED 
(
	[rowid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employee_insurance_offer_archive](
	[rowid] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[plan_year_id] [int] NOT NULL,
	[employer_id] [int] NULL,
	[insurance_id] [int] NULL,
	[ins_cont_id] [int] NULL,
	[avg_hours_month] [decimal](10, 2) NULL,
	[offered] [int] NULL,
	[offeredOn] [datetime] NULL,
	[accepted] [bit] NULL,
	[acceptedOn] [datetime] NULL,
	[modOn] [datetime] NULL,
	[modBy] [varchar](50) NULL,
	[notes] [varchar](1000) NULL,
	[history] [varchar](max) NULL,
	[effectiveDate] [datetime] NULL,
	[hra_flex_contribution] [decimal](10, 2) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employee_type](
	[employee_type_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[employer_id] [int] NOT NULL,
 CONSTRAINT [PK_employee_type] PRIMARY KEY CLUSTERED 
(
	[employee_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employer](
	[employer_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[address] [varchar](100) NULL,
	[city] [varchar](50) NULL,
	[state_id] [int] NULL,
	[zip] [varchar](15) NULL,
	[img_logo] [varchar](50) NOT NULL,
	[bill_address] [varchar](50) NULL,
	[bill_city] [varchar](50) NULL,
	[bill_state] [int] NULL,
	[bill_zip] [varchar](15) NULL,
	[employer_type_id] [int] NULL,
	[ein] [varchar](50) NULL,
	[initial_measurement_id] [int] NULL,
	[import_demo] [varchar](50) NULL,
	[import_payroll] [varchar](50) NULL,
	[iei] [varchar](50) NULL,
	[iec] [varchar](50) NULL,
	[ftpei] [varchar](50) NULL,
	[ftpec] [varchar](50) NULL,
	[ipi] [varchar](50) NULL,
	[ipc] [varchar](50) NULL,
	[ftppi] [varchar](50) NULL,
	[ftppc] [varchar](50) NULL,
	[importProcess] [varchar](50) NULL,
	[vendor_id] [int] NULL,
	[autoUpload] [bit] NOT NULL,
	[autoBill] [bit] NOT NULL,
	[suBilled] [bit] NULL,
	[import_gp] [varchar](10) NULL,
	[import_hr] [varchar](10) NULL,
	[import_ec] [varchar](10) NULL,
	[import_io] [varchar](10) NULL,
	[import_ic] [varchar](10) NULL,
	[import_pay_mod] [varchar](10) NULL,
 CONSTRAINT [PK_company] PRIMARY KEY CLUSTERED 
(
	[employer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[employer_type](
	[employer_type_id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_employer_type] PRIMARY KEY CLUSTERED 
(
	[employer_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[equiv_activity](
	[activity_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[position_id] [int] NOT NULL,
 CONSTRAINT [PK_equiv_activity] PRIMARY KEY CLUSTERED 
(
	[activity_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[equiv_detail](
	[detail_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[activity_id] [int] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[every] [decimal](14, 4) NOT NULL,
	[unit_id] [int] NOT NULL,
	[hours] [decimal](14, 4) NOT NULL,
 CONSTRAINT [PK_equiv_detail] PRIMARY KEY CLUSTERED 
(
	[detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[equiv_position](
	[position_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_equiv_position] PRIMARY KEY CLUSTERED 
(
	[position_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[equivalency](
	[equivalency_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NULL,
	[name] [varchar](50) NULL,
	[gpID] [int] NULL,
	[every] [decimal](18, 4) NULL,
	[unit_id] [int] NULL,
	[credit] [decimal](18, 4) NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[notes] [varchar](max) NULL,
	[modBy] [varchar](50) NULL,
	[modOn] [datetime] NULL,
	[history] [varchar](max) NULL,
	[active] [bit] NULL,
	[equivalency_type_id] [int] NULL,
	[position_id] [int] NULL,
	[activity_id] [int] NULL,
	[detail_id] [int] NULL,
 CONSTRAINT [PK_equivalency] PRIMARY KEY CLUSTERED 
(
	[equivalency_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[equivalency_type](
	[equivalency_type_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_equivalency_type] PRIMARY KEY CLUSTERED 
(
	[equivalency_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[fee](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[su_fee] [money] NOT NULL,
	[base_fee] [money] NOT NULL,
	[employee_fee] [money] NOT NULL,
	[history] [varchar](max) NULL,
	[modOn] [datetime] NULL,
	[modBy] [varchar](50) NULL,
 CONSTRAINT [PK_fee] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gross_pay_filter](
	[gp_filter_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[gross_pay_id] [int] NOT NULL,
 CONSTRAINT [PK_gross_pay_filter] PRIMARY KEY CLUSTERED 
(
	[gp_filter_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uc_employerID_grossPayID] UNIQUE NONCLUSTERED 
(
	[employer_id] ASC,
	[gross_pay_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[gross_pay_type](
	[gross_pay_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[external_id] [varchar](50) NOT NULL,
	[description] [varchar](50) NOT NULL,
	[active] [bit] NULL,
 CONSTRAINT [PK_gross_pay_type] PRIMARY KEY CLUSTERED 
(
	[gross_pay_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[hr_status](
	[HR_status_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[ext_id] [varchar](50) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[active] [bit] NOT NULL,
 CONSTRAINT [PK_hr_status] PRIMARY KEY CLUSTERED 
(
	[HR_status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uc_employerID_ExtHrStatusID] UNIQUE NONCLUSTERED 
(
	[employer_id] ASC,
	[ext_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[import_employee](
	[rowid] [int] IDENTITY(1,1) NOT NULL,
	[employeeTypeID] [int] NULL,
	[hr_status_id] [int] NULL,
	[hr_status_ext_id] [varchar](50) NULL,
	[hr_description] [varchar](50) NULL,
	[employerID] [int] NULL,
	[planYearID] [int] NULL,
	[fName] [varchar](50) NULL,
	[mName] [varchar](50) NULL,
	[lName] [varchar](50) NULL,
	[address] [varchar](50) NULL,
	[city] [varchar](50) NULL,
	[stateID] [int] NULL,
	[stateAbb] [varchar](2) NULL,
	[zip] [varchar](5) NULL,
	[hDate] [datetime] NULL,
	[i_hDate] [varchar](8) NULL,
	[cDate] [datetime] NULL,
	[i_cDate] [varchar](8) NULL,
	[ssn] [varchar](50) NULL,
	[ext_employee_id] [varchar](50) NULL,
	[tDate] [datetime] NULL,
	[i_tDate] [varchar](8) NULL,
	[dob] [datetime] NULL,
	[i_dob] [varchar](8) NULL,
	[impEnd] [datetime] NULL,
	[batchid] [int] NULL,
	[aca_status_id] [int] NULL,
	[class_id] [int] NULL,
 CONSTRAINT [PK_import_employee] PRIMARY KEY CLUSTERED 
(
	[rowid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[import_insurance_coverage](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[batch_id] [int] NULL,
	[employer_id] [int] NOT NULL,
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NULL,
	[dependent_link] [varchar](50) NOT NULL,
	[dependent_id] [int] NULL,
	[fName] [varchar](50) NULL,
	[lName] [varchar](50) NULL,
	[mName] [varchar](50) NULL,
	[ssn] [varchar](50) NULL,
	[dob] [varchar](50) NULL,
	[jan] [bit] NULL,
	[feb] [bit] NULL,
	[march] [bit] NULL,
	[april] [bit] NULL,
	[may] [bit] NULL,
	[june] [bit] NULL,
	[july] [bit] NULL,
	[august] [bit] NULL,
	[september] [bit] NULL,
	[october] [bit] NULL,
	[november] [bit] NULL,
	[december] [bit] NULL,
	[subscriber] [bit] NULL,
	[all12] [bit] NULL,
	[state_id] [int] NULL,
	[zip] [int] NULL,
	[jan_i] [varchar](10) NULL,
	[feb_i] [varchar](10) NULL,
	[march_i] [varchar](10) NULL,
	[april_i] [varchar](10) NULL,
	[may_i] [varchar](10) NULL,
	[june_i] [varchar](10) NULL,
	[july_i] [varchar](10) NULL,
	[aug_i] [varchar](10) NULL,
	[sep_i] [varchar](10) NULL,
	[oct_i] [varchar](10) NULL,
	[nov_i] [varchar](10) NULL,
	[dec_i] [varchar](10) NULL,
	[all12_i] [varchar](10) NULL,
	[subscriber_i] [varchar](50) NULL,
	[address_i] [varchar](50) NULL,
	[city_i] [varchar](50) NULL,
	[state_i] [varchar](50) NULL,
	[zip_i] [varchar](50) NULL,
	[carrier_id] [int] NULL,
 CONSTRAINT [PK_import_tax_year_coverage] PRIMARY KEY CLUSTERED 
(
	[row_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[import_insurance_coverage_archive](
	[row_id] [int] NOT NULL,
	[batch_id] [int] NULL,
	[employer_id] [int] NOT NULL,
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NULL,
	[dependent_link] [varchar](50) NOT NULL,
	[dependent_id] [int] NULL,
	[fName] [varchar](50) NULL,
	[lName] [varchar](50) NULL,
	[mName] [varchar](50) NULL,
	[ssn] [varchar](50) NULL,
	[dob] [varchar](50) NULL,
	[jan] [bit] NULL,
	[feb] [bit] NULL,
	[march] [bit] NULL,
	[april] [bit] NULL,
	[may] [bit] NULL,
	[june] [bit] NULL,
	[july] [bit] NULL,
	[august] [bit] NULL,
	[september] [bit] NULL,
	[october] [bit] NULL,
	[november] [bit] NULL,
	[december] [bit] NULL,
	[subscriber] [bit] NULL,
	[all12] [bit] NULL,
	[state_id] [int] NULL,
	[zip] [int] NULL,
	[jan_i] [varchar](10) NULL,
	[feb_i] [varchar](10) NULL,
	[march_i] [varchar](10) NULL,
	[april_i] [varchar](10) NULL,
	[may_i] [varchar](10) NULL,
	[june_i] [varchar](10) NULL,
	[july_i] [varchar](10) NULL,
	[aug_i] [varchar](10) NULL,
	[sep_i] [varchar](10) NULL,
	[oct_i] [varchar](10) NULL,
	[nov_i] [varchar](10) NULL,
	[dec_i] [varchar](10) NULL,
	[all12_i] [varchar](10) NULL,
	[subscriber_i] [varchar](50) NULL,
	[address_i] [varchar](50) NULL,
	[city_i] [varchar](50) NULL,
	[state_i] [varchar](50) NULL,
	[zip_i] [varchar](50) NULL,
	[carrier_id] [int] NULL,
	[modBy] [varchar](50) NULL,
	[modOn] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[import_payroll](
	[rowid] [int] IDENTITY(1,1) NOT NULL,
	[employerid] [int] NOT NULL,
	[batchid] [int] NOT NULL,
	[employee_id] [int] NULL,
	[fname] [varchar](50) NULL,
	[mname] [varchar](50) NULL,
	[lname] [varchar](50) NULL,
	[i_hours] [varchar](50) NULL,
	[hours] [decimal](18, 4) NULL,
	[i_sdate] [varchar](8) NULL,
	[sdate] [datetime] NULL,
	[i_edate] [varchar](50) NULL,
	[edate] [datetime] NULL,
	[ssn] [varchar](50) NULL,
	[gp_description] [varchar](50) NULL,
	[gp_ext_id] [varchar](50) NULL,
	[gp_id] [int] NULL,
	[i_cdate] [varchar](8) NULL,
	[cdate] [datetime] NULL,
	[modBy] [varchar](50) NULL,
	[modOn] [datetime] NULL,
	[ext_employee_id] [varchar](30) NULL,
 CONSTRAINT [PK_import_payroll] PRIMARY KEY CLUSTERED 
(
	[rowid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[initial_measurement](
	[initial_measurement_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[months] [int] NULL,
 CONSTRAINT [PK_transition_measurment] PRIMARY KEY CLUSTERED 
(
	[initial_measurement_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[insurance](
	[insurance_id] [int] IDENTITY(1,1) NOT NULL,
	[plan_year_id] [int] NOT NULL,
	[description] [varchar](50) NOT NULL,
	[monthlycost] [money] NOT NULL,
	[minValue] [bit] NULL,
	[offSpouse] [bit] NULL,
	[offDependent] [bit] NULL,
	[modOn] [datetime] NULL,
	[modBy] [varchar](50) NULL,
	[history] [varchar](max) NULL,
	[insurance_type_id] [int] NULL,
 CONSTRAINT [PK_insurance] PRIMARY KEY CLUSTERED 
(
	[insurance_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[insurance_carrier](
	[carrier_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[import_approved] [bit] NULL,
	[hra_flex] [bit] NULL,
 CONSTRAINT [PK_insurance_carrier] PRIMARY KEY CLUSTERED 
(
	[carrier_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[insurance_carrier_import_template](
	[template_id] [int] IDENTITY(1,1) NOT NULL,
	[carrier_id] [int] NOT NULL,
	[columns] [int] NOT NULL,
	[employee_dependent_link] [int] NOT NULL,
	[fname] [int] NOT NULL,
	[mname] [int] NOT NULL,
	[lname] [int] NOT NULL,
	[ssn] [int] NOT NULL,
	[dob] [int] NOT NULL,
	[all12] [int] NOT NULL,
	[jan] [int] NOT NULL,
	[feb] [int] NOT NULL,
	[march] [int] NOT NULL,
	[april] [int] NOT NULL,
	[may] [int] NOT NULL,
	[june] [int] NOT NULL,
	[july] [int] NOT NULL,
	[august] [int] NOT NULL,
	[september] [int] NOT NULL,
	[october] [int] NOT NULL,
	[november] [int] NOT NULL,
	[december] [int] NOT NULL,
	[subscriber] [int] NOT NULL,
	[trueFormat] [varchar](50) NULL,
	[nameFormat] [varchar](10) NULL,
	[all12trueFormat] [varchar](10) NULL,
	[subscriberFormat] [varchar](20) NULL,
	[address] [int] NULL,
	[city] [int] NULL,
	[state] [int] NULL,
	[zip] [int] NULL,
 CONSTRAINT [PK_insurance_carrier_report_import_template] PRIMARY KEY CLUSTERED 
(
	[template_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[insurance_contribution](
	[ins_cont_id] [int] IDENTITY(1,1) NOT NULL,
	[insurance_id] [int] NOT NULL,
	[contribution_id] [char](1) NOT NULL,
	[classification_id] [int] NOT NULL,
	[amount] [numeric](18, 2) NOT NULL,
	[modBy] [varchar](50) NULL,
	[modOn] [datetime] NULL,
	[history] [varchar](max) NULL,
 CONSTRAINT [PK_insurance_contribution] PRIMARY KEY CLUSTERED 
(
	[ins_cont_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [ucInsuranceIDandClassificationID] UNIQUE NONCLUSTERED 
(
	[classification_id] ASC,
	[insurance_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[insurance_coverage](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[tax_year] [int] NOT NULL,
	[carrier_id] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[dependent_id] [int] NULL,
	[all12] [bit] NOT NULL,
	[jan] [bit] NOT NULL,
	[feb] [bit] NOT NULL,
	[mar] [bit] NOT NULL,
	[apr] [bit] NOT NULL,
	[may] [bit] NOT NULL,
	[jun] [bit] NOT NULL,
	[jul] [bit] NOT NULL,
	[aug] [bit] NOT NULL,
	[sep] [bit] NOT NULL,
	[oct] [bit] NOT NULL,
	[nov] [bit] NOT NULL,
	[dec] [bit] NOT NULL,
	[history] [varchar](max) NULL,
 CONSTRAINT [PK_insurance_coverage] PRIMARY KEY CLUSTERED 
(
	[row_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[insurance_coverage_editable](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[dependent_id] [int] NULL,
	[tax_year] [int] NOT NULL,
	[Jan] [bit] NOT NULL,
	[Feb] [bit] NOT NULL,
	[Mar] [bit] NOT NULL,
	[Apr] [bit] NOT NULL,
	[May] [bit] NOT NULL,
	[Jun] [bit] NOT NULL,
	[Jul] [bit] NOT NULL,
	[Aug] [bit] NOT NULL,
	[Sept] [bit] NOT NULL,
	[Oct] [bit] NOT NULL,
	[Nov] [bit] NOT NULL,
	[Dec] [bit] NOT NULL,
 CONSTRAINT [PK_insurance_coverage_editable] PRIMARY KEY CLUSTERED 
(
	[row_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UC_unique_individual] UNIQUE NONCLUSTERED 
(
	[employee_id] ASC,
	[employer_id] ASC,
	[dependent_id] ASC,
	[tax_year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[insurance_type](
	[insurance_type_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
 CONSTRAINT [PK_insurance_type] PRIMARY KEY CLUSTERED 
(
	[insurance_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[invoice](
	[invoice_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[invoice_month] [int] NULL,
	[invoice_year] [int] NULL,
	[count] [int] NOT NULL,
	[base_fee] [money] NOT NULL,
	[employee_fee] [money] NOT NULL,
	[su_fee] [money] NOT NULL,
	[total] [money] NOT NULL,
	[createdOn] [datetime] NOT NULL,
	[createdBy] [varchar](50) NOT NULL,
	[message] [varchar](3000) NOT NULL,
	[paymentConfirmed] [bit] NULL,
	[history] [varchar](max) NULL,
 CONSTRAINT [PK_invoice] PRIMARY KEY CLUSTERED 
(
	[invoice_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uc_employerID_month_year] UNIQUE NONCLUSTERED 
(
	[employer_id] ASC,
	[invoice_month] ASC,
	[invoice_year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[measurement](
	[measurement_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[plan_year_id] [int] NOT NULL,
	[employee_type_id] [int] NOT NULL,
	[measurement_type_id] [int] NOT NULL,
	[meas_start] [datetime] NOT NULL,
	[meas_end] [datetime] NOT NULL,
	[admin_start] [datetime] NOT NULL,
	[admin_end] [datetime] NOT NULL,
	[open_start] [datetime] NOT NULL,
	[open_end] [datetime] NOT NULL,
	[stability_start] [datetime] NOT NULL,
	[stability_end] [datetime] NOT NULL,
	[notes] [varchar](max) NULL,
	[history] [varchar](max) NULL,
	[modOn] [datetime] NULL,
	[modBy] [varchar](50) NULL,
	[sw_start] [datetime] NULL,
	[sw_end] [datetime] NULL,
	[sw2_start] [datetime] NULL,
	[sw2_end] [datetime] NULL,
	[meas_complete] [bit] NULL,
	[admin_complete] [bit] NULL,
	[stability_complete] [bit] NULL,
	[meas_completed_by] [varchar](50) NULL,
	[admin_completed_by] [varchar](50) NULL,
	[stability_completed_by] [varchar](50) NULL,
	[meas_completed_on] [datetime] NULL,
	[admin_completed_on] [datetime] NULL,
	[stability_completed_on] [datetime] NULL,
 CONSTRAINT [PK_measurement] PRIMARY KEY CLUSTERED 
(
	[measurement_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [con_measurement] UNIQUE NONCLUSTERED 
(
	[employer_id] ASC,
	[plan_year_id] ASC,
	[employee_type_id] ASC,
	[measurement_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[measurement_type](
	[measurment_type_id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](50) NULL,
 CONSTRAINT [PK_measurement_type] PRIMARY KEY CLUSTERED 
(
	[measurment_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[month](
	[month_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_month] PRIMARY KEY CLUSTERED 
(
	[month_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[payroll](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[batch_id] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[gp_id] [int] NOT NULL,
	[act_hours] [numeric](18, 4) NOT NULL,
	[sdate] [datetime] NOT NULL,
	[edate] [datetime] NOT NULL,
	[cdate] [datetime] NOT NULL,
	[modBy] [varchar](50) NOT NULL,
	[modOn] [datetime] NOT NULL,
	[history] [varchar](max) NULL,
 CONSTRAINT [PK_payroll] PRIMARY KEY CLUSTERED 
(
	[row_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[payroll_archive](
	[row_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[batch_id] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[gp_id] [int] NOT NULL,
	[act_hours] [numeric](18, 2) NOT NULL,
	[sdate] [datetime] NOT NULL,
	[edate] [datetime] NOT NULL,
	[cdate] [datetime] NOT NULL,
	[modBy] [varchar](50) NOT NULL,
	[modOn] [datetime] NOT NULL,
	[history] [varchar](max) NULL,
 CONSTRAINT [PK_payroll2] PRIMARY KEY CLUSTERED 
(
	[row_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[payroll_summer_averages](
	[row_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[plan_year_id] [int] NOT NULL,
	[batch_id] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[gp_id] [int] NOT NULL,
	[act_hours] [numeric](18, 4) NOT NULL,
	[sdate] [datetime] NOT NULL,
	[edate] [datetime] NOT NULL,
	[cdate] [datetime] NOT NULL,
	[modBy] [varchar](50) NOT NULL,
	[modOn] [datetime] NOT NULL,
	[history] [varchar](max) NULL,
 CONSTRAINT [PK_payroll_summer_averages] PRIMARY KEY CLUSTERED 
(
	[row_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[payroll_vendor](
	[vendor_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[autoUpload] [bit] NOT NULL,
 CONSTRAINT [PK_payroll_vendor] PRIMARY KEY CLUSTERED 
(
	[vendor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[plan_year](
	[plan_year_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[description] [varchar](50) NOT NULL,
	[startDate] [datetime] NOT NULL,
	[endDate] [datetime] NOT NULL,
	[notes] [varchar](max) NULL,
	[history] [varchar](max) NULL,
	[modOn] [datetime] NULL,
	[modBy] [varchar](50) NULL,
 CONSTRAINT [PK_plan_year] PRIMARY KEY CLUSTERED 
(
	[plan_year_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[state](
	[state_id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](100) NOT NULL,
	[abbreviation] [varchar](2) NOT NULL,
 CONSTRAINT [PK_state] PRIMARY KEY CLUSTERED 
(
	[state_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tax_year](
	[tax_year] [int] NOT NULL,
 CONSTRAINT [PK_tax_year] PRIMARY KEY CLUSTERED 
(
	[tax_year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tax_year_1095c_approval](
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[approvedBy] [varchar](50) NOT NULL,
	[approvedOn] [datetime] NOT NULL,
	[get1095C] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tax_year_approval](
	[approval_id] [int] IDENTITY(1,1) NOT NULL,
	[employer_id] [int] NOT NULL,
	[tax_year] [int] NOT NULL,
	[dge] [bit] NULL,
	[dge_name] [varchar](50) NULL,
	[dge_ein] [varchar](50) NULL,
	[dge_address] [varchar](50) NULL,
	[dge_city] [varchar](50) NULL,
	[state_id] [int] NULL,
	[dge_zip] [varchar](50) NULL,
	[dge_contact_fname] [varchar](50) NULL,
	[dge_contact_lname] [varchar](50) NULL,
	[dge_phone] [varchar](50) NULL,
	[ale] [bit] NULL,
	[tr_q1] [bit] NULL,
	[tr_q2] [bit] NULL,
	[tr_q3] [bit] NULL,
	[tr_q4] [bit] NULL,
	[tr_q5] [bit] NULL,
	[tr_qualified] [bit] NULL,
	[tobacco] [bit] NULL,
	[unpaidLeave] [bit] NULL,
	[safeHarbor] [bit] NULL,
	[completed_by] [varchar](50) NULL,
	[completed_on] [datetime] NULL,
	[ebc_approval] [bit] NULL,
	[ebc_approved_by] [varchar](50) NULL,
	[ebc_approved_on] [datetime] NULL,
	[allow_editing] [bit] NOT NULL,
 CONSTRAINT [PK_tax_year_approval] PRIMARY KEY CLUSTERED 
(
	[approval_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[term](
	[term_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[description] [varchar](max) NULL,
 CONSTRAINT [PK_term] PRIMARY KEY CLUSTERED 
(
	[term_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[unit](
	[unit_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
 CONSTRAINT [PK_unit] PRIMARY KEY CLUSTERED 
(
	[unit_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[user](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[fname] [varchar](50) NULL,
	[lname] [varchar](50) NULL,
	[email] [varchar](100) NULL,
	[phone] [varchar](15) NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[employer_id] [int] NULL,
	[active] [bit] NULL,
	[poweruser] [bit] NULL,
	[last_mod_by] [varchar](50) NULL,
	[last_mod] [datetime] NULL,
	[reset_pwd] [bit] NULL,
	[billing] [bit] NULL,
	[irsContact] [bit] NOT NULL,
	[floater] [bit] NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uc_user_username_password] UNIQUE NONCLUSTERED 
(
	[username] ASC,
	[password] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uc_useremail] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uc_username] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_employer_alerts]
AS
SELECT        dbo.alert.*, dbo.alert_type.name, dbo.alert_type.image_url, dbo.alert_type.table_name
FROM            dbo.alert LEFT OUTER JOIN
                         dbo.alert_type ON dbo.alert.alert_type_id = dbo.alert_type.alert_type_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_alerts_import_employee]
AS
SELECT        dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, COUNT(dbo.View_employer_alerts.employer_id) AS alertCount, dbo.View_employer_alerts.name
FROM            dbo.import_employee LEFT OUTER JOIN
                         dbo.View_employer_alerts ON dbo.import_employee.employerID = dbo.View_employer_alerts.employer_id
WHERE        (dbo.View_employer_alerts.alert_type_id = 2)
GROUP BY dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.name

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_alerts_import_payroll]
AS
SELECT     dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, COUNT(dbo.View_employer_alerts.employer_id) AS alertCount, 
                      dbo.View_employer_alerts.name
FROM         dbo.import_payroll LEFT OUTER JOIN
                      dbo.View_employer_alerts ON dbo.import_payroll.employerid = dbo.View_employer_alerts.employer_id
WHERE     (dbo.View_employer_alerts.alert_type_id = 1)
GROUP BY dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.name

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_alerts_insurance_offer]
AS
SELECT        dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, COUNT(dbo.View_employer_alerts.employer_id) AS alertCount, dbo.View_employer_alerts.name
FROM            dbo.employee_insurance_offer LEFT OUTER JOIN
                         dbo.View_employer_alerts ON dbo.employee_insurance_offer.employer_id = dbo.View_employer_alerts.employer_id
WHERE        (dbo.View_employer_alerts.alert_type_id = 3) AND (dbo.employee_insurance_offer.offered IS NULL) AND (dbo.employee_insurance_offer.accepted IS NULL) OR
                         (dbo.View_employer_alerts.alert_type_id = 3) AND (dbo.employee_insurance_offer.offered = 1) AND (dbo.employee_insurance_offer.hra_flex_contribution IS NULL)
GROUP BY dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.name

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_alerts_summer_window]
AS
SELECT        dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, COUNT(dbo.View_employer_alerts.employer_id) AS alertCount, dbo.View_employer_alerts.name
FROM            dbo.measurement LEFT OUTER JOIN
                         dbo.View_employer_alerts ON dbo.measurement.employer_id = dbo.View_employer_alerts.employer_id
WHERE        (dbo.View_employer_alerts.alert_type_id = 4) AND (dbo.measurement.sw_start IS NULL) OR
                         (dbo.View_employer_alerts.alert_type_id = 4) AND (dbo.measurement.sw_end IS NULL)
GROUP BY dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.name

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_billContact_alerts]
AS
SELECT        employer_id
FROM            dbo.[user]
WHERE        (employer_id NOT IN
                             (SELECT        employer_id
                               FROM            dbo.[user] AS user_1
                               WHERE        (billing = 1)))
GROUP BY employer_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_alerts_billing_contact]
AS
SELECT        dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, COUNT(dbo.View_employer_alerts.employer_id) AS alertCount, dbo.View_employer_alerts.name
FROM            dbo.View_employer_alerts RIGHT OUTER JOIN
                         dbo.View_billContact_alerts ON dbo.View_employer_alerts.employer_id = dbo.View_billContact_alerts.employer_id
WHERE        (dbo.View_employer_alerts.alert_type_id = 5)
GROUP BY dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.name

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_irs_alerts]
AS
SELECT        employer_id
FROM            dbo.[user]
WHERE        (employer_id NOT IN
                             (SELECT        employer_id
                               FROM            dbo.[user] AS user_1
                               WHERE        (irsContact = 1)))
GROUP BY employer_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_alerts_irs_contact]
AS
SELECT        dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, COUNT(dbo.View_employer_alerts.employer_id) AS alertCount, dbo.View_employer_alerts.name
FROM            dbo.View_employer_alerts RIGHT OUTER JOIN
                         dbo.View_irs_alerts ON dbo.View_employer_alerts.employer_id = dbo.View_irs_alerts.employer_id
WHERE        (dbo.View_employer_alerts.alert_type_id = 6)
GROUP BY dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.name

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_alerts_import_carrier]
AS
SELECT     dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, COUNT(dbo.View_employer_alerts.employer_id) AS alertCount, 
                      dbo.View_employer_alerts.name
FROM         dbo.import_insurance_coverage LEFT OUTER JOIN
                      dbo.View_employer_alerts ON dbo.import_insurance_coverage.employer_id = dbo.View_employer_alerts.employer_id
WHERE     (dbo.View_employer_alerts.alert_type_id = 8)
GROUP BY dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.name

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_PlanYear_Insurance]
AS
SELECT        dbo.plan_year.*, dbo.insurance.insurance_id, dbo.insurance.description AS Expr1, dbo.insurance.monthlycost, dbo.insurance.minValue, dbo.insurance.offSpouse, dbo.insurance.offDependent, 
                         dbo.insurance.modOn AS Expr2, dbo.insurance.modBy AS Expr3, dbo.insurance.history AS Expr4, dbo.insurance.insurance_type_id
FROM            dbo.plan_year RIGHT OUTER JOIN
                         dbo.insurance ON dbo.plan_year.plan_year_id = dbo.insurance.plan_year_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_missingInsuranceType_alerts]
AS
SELECT        employer_id, insurance_id
FROM            dbo.View_PlanYear_Insurance
WHERE        (insurance_type_id IS NULL)

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_alerts_insurance_type]
AS
SELECT        dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, COUNT(dbo.View_employer_alerts.employer_id) AS alertCount, dbo.View_employer_alerts.name
FROM            dbo.View_employer_alerts RIGHT OUTER JOIN
                         dbo.View_missingInsuranceType_alerts ON dbo.View_employer_alerts.employer_id = dbo.View_missingInsuranceType_alerts.employer_id
WHERE        (dbo.View_employer_alerts.alert_type_id = 7)
GROUP BY dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.name

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_alerts_union]
AS
SELECT        alert_id, employer_id, alertCount, name
FROM            View_alerts_import_employee
UNION
SELECT        alert_id, employer_id, alertCount, name
FROM            View_alerts_import_payroll
UNION
SELECT        alert_id, employer_id, alertCount, name
FROM            View_alerts_insurance_offer
UNION
SELECT        alert_id, employer_id, alertCount, name
FROM            View_alerts_summer_window
UNION
SELECT        alert_id, employer_id, alertCount, name
FROM            View_alerts_irs_contact
UNION
SELECT        alert_id, employer_id, alertCount, name
FROM            View_alerts_billing_contact
UNION
SELECT        alert_id, employer_id, alertCount, name
FROM            View_alerts_insurance_type
UNION
SELECT        alert_id, employer_id, alertCount, name
FROM            View_alerts_import_carrier

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_alerts]
AS
SELECT        dbo.View_employer_alerts.alert_id, dbo.View_employer_alerts.name, dbo.View_employer_alerts.alert_type_id, dbo.View_employer_alerts.employer_id, dbo.View_employer_alerts.table_name, dbo.View_alerts_union.alertCount, dbo.alert_type.name AS alerttypename, dbo.alert_type.image_url
FROM            dbo.View_employer_alerts LEFT OUTER JOIN
                         dbo.alert_type ON dbo.View_employer_alerts.alert_type_id = dbo.alert_type.alert_type_id LEFT OUTER JOIN
                         dbo.View_alerts_union ON dbo.View_employer_alerts.alert_id = dbo.View_alerts_union.alert_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_dependent_insurance_coverage]
AS
SELECT        dbo.employee_dependents.dependent_id, dbo.employee_dependents.employee_id, dbo.employee_dependents.fName, dbo.employee_dependents.mName, dbo.employee_dependents.lName, 
                         dbo.employee_dependents.ssn, dbo.employee_dependents.dob, dbo.insurance_coverage.row_id, dbo.insurance_coverage.tax_year, dbo.insurance_coverage.carrier_id, dbo.insurance_coverage.all12, 
                         dbo.insurance_coverage.jan, dbo.insurance_coverage.feb, dbo.insurance_coverage.mar, dbo.insurance_coverage.apr, dbo.insurance_coverage.may, dbo.insurance_coverage.jun, dbo.insurance_coverage.jul, 
                         dbo.insurance_coverage.aug, dbo.insurance_coverage.sep, dbo.insurance_coverage.oct, dbo.insurance_coverage.nov, dbo.insurance_coverage.dec
FROM            dbo.employee_dependents LEFT OUTER JOIN
                         dbo.insurance_coverage ON dbo.employee_dependents.dependent_id = dbo.insurance_coverage.dependent_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_employee_insurance_coverage]
AS
SELECT        dbo.insurance_coverage.dependent_id, dbo.insurance_coverage.employee_id, dbo.employee.fName, dbo.employee.mName, dbo.employee.lName, dbo.employee.ssn, dbo.employee.dob, 
                         dbo.insurance_coverage.row_id, dbo.insurance_coverage.tax_year, dbo.insurance_coverage.carrier_id, dbo.insurance_coverage.all12, dbo.insurance_coverage.jan, dbo.insurance_coverage.feb, 
                         dbo.insurance_coverage.mar, dbo.insurance_coverage.apr, dbo.insurance_coverage.may, dbo.insurance_coverage.jun, dbo.insurance_coverage.jul, dbo.insurance_coverage.aug, dbo.insurance_coverage.sep, 
                         dbo.insurance_coverage.oct, dbo.insurance_coverage.nov, dbo.insurance_coverage.dec
FROM            dbo.insurance_coverage LEFT OUTER JOIN
                         dbo.employee ON dbo.insurance_coverage.employee_id = dbo.employee.employee_id
WHERE        (dbo.insurance_coverage.dependent_id IS NULL)

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_all_insurance_coverage]
AS
SELECT *
  FROM [aca-demo].[dbo].[View_employee_insurance_coverage]
UNION 
Select * 
	FROM [aca-demo].[dbo].[View_dependent_insurance_coverage]         

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
--		Jan	Feb	Mar	Apr	May	Jun	Jul	Aug	Sep	Oct	Nov	Dec
--Bob	1	1	1	0	0	0	1	1	1	1	1	1
--Bob	0	0	0	0	0	1	1	1	1	1	0	0
--____________________________________________________
--Bob	1	1	1	0	0	1	1	1	1	1	1	1	Final Results for AIR system.
-- =============================================
CREATE FUNCTION [dbo].[ufnGetEmployeeInsurance]()
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
FROM   [aca-demo].dbo.insurance_coverage_editable ic
	INNER JOIN [aca-demo].dbo.employee ee ON(ic.employee_id = ee.employee_id)
	LEFT OUTER JOIN [aca-demo].dbo.employee_dependents ed ON (ic.dependent_id = ed.dependent_id)

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
--		Jan	Feb	Mar	Apr	May	Jun	Jul	Aug	Sep	Oct	Nov	Dec
--Bob	1	1	1	0	0	0	1	1	1	1	1	1
--Bob	0	0	0	0	0	1	1	1	1	1	0	0
--____________________________________________________
--Bob	1	1	1	0	0	1	1	1	1	1	1	1	Final Results for AIR system.
-- =============================================
CREATE FUNCTION [dbo].[ufnGetEmployeeInsurance_old]()
RETURNS TABLE 
AS
RETURN 
SELECT DISTINCT
              MIN(row_id) AS first_row_id,
              ic.employee_id,
              ee.employer_id, 
              ic.dependent_id, 
              tax_year,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.fName ELSE ed.fName end) AS fName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.mName ELSE ed.mName end) AS mName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.lName ELSE ed.lName end) AS lName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.ssn ELSE ed.ssn end) AS ssn,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.dob ELSE ed.dob end) AS dob,
              MAX(CONVERT(INTEGER,jan)) as Jan,
              MAX(CONVERT(INTEGER,feb))as Feb,
              MAX(CONVERT(INTEGER,mar)) as Mar,
              MAX(CONVERT(INTEGER,apr)) as Apr,
              MAX(CONVERT(INTEGER,may)) as May,
              MAX(CONVERT(INTEGER,jun)) as Jun,
              MAX(CONVERT(INTEGER,jul)) as Jul,
              MAX(CONVERT(INTEGER,aug)) as Aug,
              MAX(CONVERT(INTEGER,sep)) as Sept,
              MAX(CONVERT(INTEGER,oct)) as Oct,
              MAX(CONVERT(INTEGER,nov)) as Nov,
              MAX(CONVERT(INTEGER,[dec])) as [Dec]
FROM   [aca-demo].dbo.insurance_coverage ic
                     INNER JOIN [aca-demo].dbo.employee ee ON(ic.employee_id = ee.employee_id)
                     LEFT OUTER JOIN [aca-demo].dbo.employee_dependents ed ON (ic.dependent_id = ed.dependent_id)
GROUP BY ic.employee_id, ee.employer_id, ic.dependent_id, tax_year

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
--		Jan	Feb	Mar	Apr	May	Jun	Jul	Aug	Sep	Oct	Nov	Dec
--Bob	1	1	1	0	0	0	1	1	1	1	1	1
--Bob	0	0	0	0	0	1	1	1	1	1	0	0
--____________________________________________________
--Bob	1	1	1	0	0	1	1	1	1	1	1	1	Final Results for AIR system.
-- =============================================
CREATE FUNCTION [dbo].[ufnGetEmployeeInsurance-original]()
RETURNS TABLE 
AS
RETURN 
SELECT DISTINCT
              MIN(row_id) AS first_row_id,
              ic.employee_id,
              ee.employer_id, 
              ic.dependent_id, 
              tax_year,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.fName ELSE ed.fName end) AS fName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.mName ELSE ed.mName end) AS mName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.lName ELSE ed.lName end) AS lName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.ssn ELSE ed.ssn end) AS ssn,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.dob ELSE ed.dob end) AS dob,
              MAX(CONVERT(INTEGER,jan)) as Jan,
              MAX(CONVERT(INTEGER,feb))as Feb,
              MAX(CONVERT(INTEGER,mar)) as Mar,
              MAX(CONVERT(INTEGER,apr)) as Apr,
              MAX(CONVERT(INTEGER,may)) as May,
              MAX(CONVERT(INTEGER,jun)) as Jun,
              MAX(CONVERT(INTEGER,jul)) as Jul,
              MAX(CONVERT(INTEGER,aug)) as Aug,
              MAX(CONVERT(INTEGER,sep)) as Sept,
              MAX(CONVERT(INTEGER,oct)) as Oct,
              MAX(CONVERT(INTEGER,nov)) as Nov,
              MAX(CONVERT(INTEGER,[dec])) as [Dec]
FROM   [aca-demo].dbo.insurance_coverage ic
                     INNER JOIN [aca-demo].dbo.employee ee ON(ic.employee_id = ee.employee_id)
                     LEFT OUTER JOIN [aca-demo].dbo.employee_dependents ed ON (ic.dependent_id = ed.dependent_id)
GROUP BY ic.employee_id, ee.employer_id, ic.dependent_id, tax_year

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Avg_Hours_Ongoing]
AS
SELECT        dbo.employee.employer_id, dbo.employee.employee_id, dbo.employee.plan_year_id, dbo.employee.hireDate, dbo.measurement.meas_start, dbo.measurement.meas_end,
                             (SELECT        SUM(act_hours) AS Expr1
                               FROM            dbo.payroll
                               WHERE        (dbo.employee.employee_id = employee_id) AND (edate >= dbo.measurement.meas_start) AND (edate <= dbo.measurement.meas_end)) AS TotalHours
FROM            dbo.employee LEFT OUTER JOIN
                         dbo.measurement ON dbo.employee.plan_year_id = dbo.measurement.plan_year_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Avg_Hours_Ongoing_limbo]
AS
SELECT        dbo.employee.employer_id, dbo.employee.employee_id, dbo.employee.plan_year_id, dbo.employee.hireDate, dbo.measurement.meas_start, dbo.measurement.meas_end,
                             (SELECT        SUM(act_hours) AS Expr1
                               FROM            dbo.payroll
                               WHERE        (dbo.employee.employee_id = employee_id) AND (edate >= dbo.measurement.meas_start) AND (edate <= dbo.measurement.meas_end)) AS TotalHours
FROM            dbo.employee LEFT OUTER JOIN
                         dbo.measurement ON dbo.employee.limbo_plan_year_id = dbo.measurement.plan_year_id
WHERE        (dbo.employee.limbo_plan_year_id IS NOT NULL)

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Avg_Hours_Ongoing_meas]
AS
SELECT        dbo.employee.employer_id, dbo.employee.employee_id, dbo.employee.plan_year_id, dbo.employee.hireDate, dbo.measurement.meas_start, dbo.measurement.meas_end,
                             (SELECT        SUM(act_hours) AS Expr1
                               FROM            dbo.payroll
                               WHERE        (dbo.employee.employee_id = employee_id) AND (edate >= dbo.measurement.meas_start) AND (edate <= dbo.measurement.meas_end)) AS TotalHours
FROM            dbo.employee LEFT OUTER JOIN
                         dbo.measurement ON dbo.employee.meas_plan_year_id = dbo.measurement.plan_year_id
WHERE        (dbo.employee.meas_plan_year_id IS NOT NULL)

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Employee_Export]
AS
SELECT        dbo.employee.employee_id, dbo.employee.employee_type_id, dbo.employee.HR_status_id, dbo.employee.employer_id, dbo.employee.fName, dbo.employee.mName, dbo.employee.lName, 
                         dbo.employee.address, dbo.employee.city, dbo.employee.state_id, dbo.employee.zip, dbo.employee.hireDate, dbo.employee.currDate, dbo.employee.ssn, dbo.employee.ext_emp_id, 
                         dbo.employee.terminationDate, dbo.employee.dob, dbo.employee.initialMeasurmentEnd, dbo.employee.plan_year_id, dbo.employee.limbo_plan_year_id, dbo.employee.meas_plan_year_id, 
                         dbo.employee.modOn, dbo.employee.modBy, dbo.employee.plan_year_avg_hours, dbo.employee.limbo_plan_year_avg_hours, dbo.employee.meas_plan_year_avg_hours, 
                         dbo.employee.imp_plan_year_avg_hours, dbo.employee.classification_id, dbo.employee.aca_status_id, dbo.hr_status.name AS hrStatus, dbo.aca_status.name AS acaStatus, 
                         dbo.employee_classification.description AS className
FROM            dbo.employee LEFT OUTER JOIN
                         dbo.employee_classification ON dbo.employee.classification_id = dbo.employee_classification.classification_id LEFT OUTER JOIN
                         dbo.aca_status ON dbo.employee.aca_status_id = dbo.aca_status.aca_status_id LEFT OUTER JOIN
                         dbo.hr_status ON dbo.employee.HR_status_id = dbo.hr_status.HR_status_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_employer_equivalency]
AS
SELECT        dbo.equivalency.equivalency_id, dbo.equivalency.employer_id, dbo.equivalency.name, dbo.equivalency.gpID, dbo.equivalency.every, dbo.equivalency.unit_id, dbo.equivalency.credit, 
                         dbo.equivalency.start_date, dbo.equivalency.end_date, dbo.equivalency.notes, dbo.equivalency.modBy, dbo.equivalency.modOn, dbo.equivalency.history, dbo.equivalency.active, 
                         dbo.equivalency.equivalency_type_id, dbo.equivalency_type.name AS equivalency_type_name, dbo.unit.name AS unit_name, dbo.equivalency.position_id, dbo.equivalency.activity_id, 
                         dbo.equivalency.detail_id
FROM            dbo.equivalency LEFT OUTER JOIN
                         dbo.unit ON dbo.equivalency.unit_id = dbo.unit.unit_id LEFT OUTER JOIN
                         dbo.equivalency_type ON dbo.equivalency.equivalency_type_id = dbo.equivalency_type.equivalency_type_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_insurance_alert_details]
AS
SELECT        dbo.employee_insurance_offer.rowid, dbo.employee_insurance_offer.employee_id, dbo.employee_insurance_offer.plan_year_id, dbo.employee_insurance_offer.employer_id, 
                         dbo.employee_insurance_offer.avg_hours_month, dbo.employee_insurance_offer.offered, dbo.employee_insurance_offer.accepted, dbo.employee_insurance_offer.acceptedOn, 
                         dbo.employee_insurance_offer.modOn, dbo.employee_insurance_offer.modBy, dbo.employee_insurance_offer.notes, dbo.employee_insurance_offer.history, dbo.employee.ext_emp_id, dbo.employee.fName, 
                         dbo.employee.lName, dbo.employee_insurance_offer.effectiveDate, dbo.employee_insurance_offer.offeredOn, dbo.employee_insurance_offer.ins_cont_id, dbo.employee_insurance_offer.insurance_id, 
                         dbo.employee.HR_status_id, dbo.employee.limbo_plan_year_id, dbo.employee.limbo_plan_year_avg_hours, dbo.employee.imp_plan_year_avg_hours, dbo.employee.classification_id, 
                         dbo.employee_insurance_offer.hra_flex_contribution
FROM            dbo.employee_insurance_offer LEFT OUTER JOIN
                         dbo.employee ON dbo.employee_insurance_offer.employee_id = dbo.employee.employee_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Insurance_Contributions]
AS
SELECT        dbo.insurance_contribution.*, dbo.employee_classification.description
FROM            dbo.employee_classification LEFT OUTER JOIN
                         dbo.insurance_contribution ON dbo.employee_classification.classification_id = dbo.insurance_contribution.classification_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_payroll]
AS
SELECT        dbo.payroll.row_id, dbo.payroll.employer_id, dbo.payroll.batch_id, dbo.payroll.employee_id, dbo.payroll.gp_id, dbo.payroll.act_hours, dbo.payroll.sdate, dbo.payroll.edate, dbo.payroll.cdate, dbo.payroll.modBy, 
                         dbo.payroll.modOn, dbo.gross_pay_type.description, dbo.gross_pay_type.external_id, dbo.employee.ext_emp_id, dbo.employee.fName, dbo.employee.lName, dbo.payroll.history
FROM            dbo.payroll LEFT OUTER JOIN
                         dbo.employee ON dbo.payroll.employee_id = dbo.employee.employee_id LEFT OUTER JOIN
                         dbo.gross_pay_type ON dbo.payroll.gp_id = dbo.gross_pay_type.gross_pay_id

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_payroll_summer_avg]
AS
SELECT        dbo.payroll_summer_averages.*, dbo.gross_pay_type.description
FROM            dbo.payroll_summer_averages LEFT OUTER JOIN
                         dbo.gross_pay_type ON dbo.payroll_summer_averages.gp_id = dbo.gross_pay_type.gross_pay_id

GO
SET ANSI_PADDING ON

GO

CREATE UNIQUE NONCLUSTERED INDEX [idx_import_demo_notnull] ON [dbo].[employer]
(
	[import_demo] ASC
)
WHERE ([import_demo] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO

CREATE UNIQUE NONCLUSTERED INDEX [idx_import_ec_notnull] ON [dbo].[employer]
(
	[import_ec] ASC
)
WHERE ([import_ec] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO

CREATE UNIQUE NONCLUSTERED INDEX [idx_import_gp_notnull] ON [dbo].[employer]
(
	[import_gp] ASC
)
WHERE ([import_gp] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO

CREATE UNIQUE NONCLUSTERED INDEX [idx_import_hr_notnull] ON [dbo].[employer]
(
	[import_hr] ASC
)
WHERE ([import_hr] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO

CREATE UNIQUE NONCLUSTERED INDEX [idx_import_payroll_notnull] ON [dbo].[employer]
(
	[import_payroll] ASC
)
WHERE ([import_payroll] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[employer] ADD  CONSTRAINT [DF_employer_img_logo]  DEFAULT ('../images/logos/EBC_logo.gif') FOR [img_logo]
GO
ALTER TABLE [dbo].[employer] ADD  CONSTRAINT [DF_employer_initial_measurement_id]  DEFAULT ((9)) FOR [initial_measurement_id]
GO
ALTER TABLE [dbo].[employer] ADD  CONSTRAINT [DF_employer_autoUpload]  DEFAULT ((0)) FOR [autoUpload]
GO
ALTER TABLE [dbo].[employer] ADD  CONSTRAINT [DF_employer_autoBill]  DEFAULT ((0)) FOR [autoBill]
GO
ALTER TABLE [dbo].[employer] ADD  CONSTRAINT [DF_employer_suBilled]  DEFAULT ((0)) FOR [suBilled]
GO
ALTER TABLE [dbo].[equivalency] ADD  CONSTRAINT [DF_equivalency_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[hr_status] ADD  CONSTRAINT [DF_hr_status_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[payroll_vendor] ADD  CONSTRAINT [DF_payroll_vendor_autoUpload]  DEFAULT ((0)) FOR [autoUpload]
GO
ALTER TABLE [dbo].[tax_year_approval] ADD  CONSTRAINT [DF_tax_year_approval_allow_editing]  DEFAULT ((0)) FOR [allow_editing]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_user_active]  DEFAULT ((0)) FOR [active]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_user_poweruser]  DEFAULT ((0)) FOR [poweruser]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_user_reset_pwd]  DEFAULT ((0)) FOR [reset_pwd]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_user_billing]  DEFAULT ((0)) FOR [billing]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_user_irsContact]  DEFAULT ((0)) FOR [irsContact]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_user_floater]  DEFAULT ((0)) FOR [floater]
GO
ALTER TABLE [dbo].[alert]  WITH CHECK ADD  CONSTRAINT [FK_alert_alert_type] FOREIGN KEY([alert_type_id])
REFERENCES [dbo].[alert_type] ([alert_type_id])
GO
ALTER TABLE [dbo].[alert] CHECK CONSTRAINT [FK_alert_alert_type]
GO
ALTER TABLE [dbo].[alert]  WITH CHECK ADD  CONSTRAINT [FK_alert_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[alert] CHECK CONSTRAINT [FK_alert_employer]
GO
ALTER TABLE [dbo].[alert_archive]  WITH CHECK ADD  CONSTRAINT [FK_alert_archive_alert] FOREIGN KEY([alert_id])
REFERENCES [dbo].[alert] ([alert_id])
GO
ALTER TABLE [dbo].[alert_archive] CHECK CONSTRAINT [FK_alert_archive_alert]
GO
ALTER TABLE [dbo].[alert_archive]  WITH CHECK ADD  CONSTRAINT [FK_alert_archive_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[alert_archive] CHECK CONSTRAINT [FK_alert_archive_employer]
GO
ALTER TABLE [dbo].[batch]  WITH CHECK ADD  CONSTRAINT [FK_batch_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[batch] CHECK CONSTRAINT [FK_batch_employer]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [FK_employee_aca_status] FOREIGN KEY([aca_status_id])
REFERENCES [dbo].[aca_status] ([aca_status_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [FK_employee_aca_status]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [FK_employee_employee_type] FOREIGN KEY([classification_id])
REFERENCES [dbo].[employee_classification] ([classification_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [FK_employee_employee_type]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [FK_employee_employee_type1] FOREIGN KEY([employee_type_id])
REFERENCES [dbo].[employee_type] ([employee_type_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [FK_employee_employee_type1]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [fk_employee_employerID] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [fk_employee_employerID]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [fk_employee_hrstatusID] FOREIGN KEY([HR_status_id])
REFERENCES [dbo].[hr_status] ([HR_status_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [fk_employee_hrstatusID]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [FK_employee_plan_year] FOREIGN KEY([plan_year_id])
REFERENCES [dbo].[plan_year] ([plan_year_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [FK_employee_plan_year]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [FK_employee_plan_year1] FOREIGN KEY([limbo_plan_year_id])
REFERENCES [dbo].[plan_year] ([plan_year_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [FK_employee_plan_year1]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [FK_employee_plan_year2] FOREIGN KEY([meas_plan_year_id])
REFERENCES [dbo].[plan_year] ([plan_year_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [FK_employee_plan_year2]
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [fk_employee_stateID] FOREIGN KEY([state_id])
REFERENCES [dbo].[state] ([state_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [fk_employee_stateID]
GO
ALTER TABLE [dbo].[employee_classification]  WITH CHECK ADD  CONSTRAINT [FK_employee_classification_affordability_safe_harbor] FOREIGN KEY([ash_code])
REFERENCES [dbo].[affordability_safe_harbor] ([ash_code])
GO
ALTER TABLE [dbo].[employee_classification] CHECK CONSTRAINT [FK_employee_classification_affordability_safe_harbor]
GO
ALTER TABLE [dbo].[employee_classification]  WITH CHECK ADD  CONSTRAINT [FK_employee_classification_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[employee_classification] CHECK CONSTRAINT [FK_employee_classification_employer]
GO
ALTER TABLE [dbo].[employee_dependents]  WITH CHECK ADD  CONSTRAINT [FK_employee_dependents_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[employee_dependents] CHECK CONSTRAINT [FK_employee_dependents_employee]
GO
ALTER TABLE [dbo].[employee_insurance_offer]  WITH CHECK ADD  CONSTRAINT [FK_employee_py_archives_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer] CHECK CONSTRAINT [FK_employee_py_archives_employee]
GO
ALTER TABLE [dbo].[employee_insurance_offer]  WITH CHECK ADD  CONSTRAINT [FK_employee_py_archives_employee_py_archives] FOREIGN KEY([rowid])
REFERENCES [dbo].[employee_insurance_offer] ([rowid])
GO
ALTER TABLE [dbo].[employee_insurance_offer] CHECK CONSTRAINT [FK_employee_py_archives_employee_py_archives]
GO
ALTER TABLE [dbo].[employee_insurance_offer]  WITH CHECK ADD  CONSTRAINT [FK_employee_py_archives_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer] CHECK CONSTRAINT [FK_employee_py_archives_employer]
GO
ALTER TABLE [dbo].[employee_insurance_offer]  WITH CHECK ADD  CONSTRAINT [FK_employee_py_archives_insurance] FOREIGN KEY([insurance_id])
REFERENCES [dbo].[insurance] ([insurance_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer] CHECK CONSTRAINT [FK_employee_py_archives_insurance]
GO
ALTER TABLE [dbo].[employee_insurance_offer]  WITH CHECK ADD  CONSTRAINT [FK_employee_py_archives_insurance_contribution] FOREIGN KEY([ins_cont_id])
REFERENCES [dbo].[insurance_contribution] ([ins_cont_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer] CHECK CONSTRAINT [FK_employee_py_archives_insurance_contribution]
GO
ALTER TABLE [dbo].[employee_insurance_offer]  WITH CHECK ADD  CONSTRAINT [FK_employee_py_archives_plan_year] FOREIGN KEY([plan_year_id])
REFERENCES [dbo].[plan_year] ([plan_year_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer] CHECK CONSTRAINT [FK_employee_py_archives_plan_year]
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive]  WITH CHECK ADD  CONSTRAINT [FK_employee_insurance_change_event_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive] CHECK CONSTRAINT [FK_employee_insurance_change_event_employee]
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive]  WITH CHECK ADD  CONSTRAINT [FK_employee_insurance_change_event_employee_insurance_offer] FOREIGN KEY([rowid])
REFERENCES [dbo].[employee_insurance_offer] ([rowid])
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive] CHECK CONSTRAINT [FK_employee_insurance_change_event_employee_insurance_offer]
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive]  WITH CHECK ADD  CONSTRAINT [FK_employee_insurance_change_event_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive] CHECK CONSTRAINT [FK_employee_insurance_change_event_employer]
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive]  WITH CHECK ADD  CONSTRAINT [FK_employee_insurance_change_event_insurance] FOREIGN KEY([insurance_id])
REFERENCES [dbo].[insurance] ([insurance_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive] CHECK CONSTRAINT [FK_employee_insurance_change_event_insurance]
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive]  WITH CHECK ADD  CONSTRAINT [FK_employee_insurance_change_event_insurance_contribution] FOREIGN KEY([ins_cont_id])
REFERENCES [dbo].[insurance_contribution] ([ins_cont_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive] CHECK CONSTRAINT [FK_employee_insurance_change_event_insurance_contribution]
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive]  WITH CHECK ADD  CONSTRAINT [FK_employee_insurance_change_event_plan_year] FOREIGN KEY([plan_year_id])
REFERENCES [dbo].[plan_year] ([plan_year_id])
GO
ALTER TABLE [dbo].[employee_insurance_offer_archive] CHECK CONSTRAINT [FK_employee_insurance_change_event_plan_year]
GO
ALTER TABLE [dbo].[employee_type]  WITH CHECK ADD  CONSTRAINT [fk_employertype_employertypeID] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[employee_type] CHECK CONSTRAINT [fk_employertype_employertypeID]
GO
ALTER TABLE [dbo].[employer]  WITH CHECK ADD  CONSTRAINT [FK_employer_payroll_vendor] FOREIGN KEY([vendor_id])
REFERENCES [dbo].[payroll_vendor] ([vendor_id])
GO
ALTER TABLE [dbo].[employer] CHECK CONSTRAINT [FK_employer_payroll_vendor]
GO
ALTER TABLE [dbo].[employer]  WITH CHECK ADD  CONSTRAINT [fk_employer_stateID] FOREIGN KEY([state_id])
REFERENCES [dbo].[state] ([state_id])
GO
ALTER TABLE [dbo].[employer] CHECK CONSTRAINT [fk_employer_stateID]
GO
ALTER TABLE [dbo].[employer]  WITH CHECK ADD  CONSTRAINT [fk_employer_stateID2] FOREIGN KEY([bill_state])
REFERENCES [dbo].[state] ([state_id])
GO
ALTER TABLE [dbo].[employer] CHECK CONSTRAINT [fk_employer_stateID2]
GO
ALTER TABLE [dbo].[gross_pay_filter]  WITH CHECK ADD  CONSTRAINT [FK_gross_pay_filter_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[gross_pay_filter] CHECK CONSTRAINT [FK_gross_pay_filter_employer]
GO
ALTER TABLE [dbo].[gross_pay_filter]  WITH CHECK ADD  CONSTRAINT [FK_gross_pay_filter_gross_pay_type] FOREIGN KEY([gross_pay_id])
REFERENCES [dbo].[gross_pay_type] ([gross_pay_id])
GO
ALTER TABLE [dbo].[gross_pay_filter] CHECK CONSTRAINT [FK_gross_pay_filter_gross_pay_type]
GO
ALTER TABLE [dbo].[gross_pay_type]  WITH CHECK ADD  CONSTRAINT [fk_grosspaytype_employerID] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[gross_pay_type] CHECK CONSTRAINT [fk_grosspaytype_employerID]
GO
ALTER TABLE [dbo].[import_insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_import_insurance_coverage_batch] FOREIGN KEY([batch_id])
REFERENCES [dbo].[batch] ([batch_id])
GO
ALTER TABLE [dbo].[import_insurance_coverage] CHECK CONSTRAINT [FK_import_insurance_coverage_batch]
GO
ALTER TABLE [dbo].[import_insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_import_insurance_coverage_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[import_insurance_coverage] CHECK CONSTRAINT [FK_import_insurance_coverage_employee]
GO
ALTER TABLE [dbo].[import_insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_import_insurance_coverage_employee_dependents] FOREIGN KEY([dependent_id])
REFERENCES [dbo].[employee_dependents] ([dependent_id])
GO
ALTER TABLE [dbo].[import_insurance_coverage] CHECK CONSTRAINT [FK_import_insurance_coverage_employee_dependents]
GO
ALTER TABLE [dbo].[import_insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_import_insurance_coverage_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[import_insurance_coverage] CHECK CONSTRAINT [FK_import_insurance_coverage_employer]
GO
ALTER TABLE [dbo].[import_insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_import_insurance_coverage_insurance_carrier] FOREIGN KEY([carrier_id])
REFERENCES [dbo].[insurance_carrier] ([carrier_id])
GO
ALTER TABLE [dbo].[import_insurance_coverage] CHECK CONSTRAINT [FK_import_insurance_coverage_insurance_carrier]
GO
ALTER TABLE [dbo].[import_insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_import_insurance_coverage_state] FOREIGN KEY([state_id])
REFERENCES [dbo].[state] ([state_id])
GO
ALTER TABLE [dbo].[import_insurance_coverage] CHECK CONSTRAINT [FK_import_insurance_coverage_state]
GO
ALTER TABLE [dbo].[insurance]  WITH CHECK ADD  CONSTRAINT [FK_insurance_insurance_type] FOREIGN KEY([insurance_type_id])
REFERENCES [dbo].[insurance_type] ([insurance_type_id])
GO
ALTER TABLE [dbo].[insurance] CHECK CONSTRAINT [FK_insurance_insurance_type]
GO
ALTER TABLE [dbo].[insurance]  WITH CHECK ADD  CONSTRAINT [FK_insurance_plan_year] FOREIGN KEY([plan_year_id])
REFERENCES [dbo].[plan_year] ([plan_year_id])
GO
ALTER TABLE [dbo].[insurance] CHECK CONSTRAINT [FK_insurance_plan_year]
GO
ALTER TABLE [dbo].[insurance_carrier_import_template]  WITH CHECK ADD  CONSTRAINT [FK_insurance_carrier_report_import_template_insurance_carrier] FOREIGN KEY([carrier_id])
REFERENCES [dbo].[insurance_carrier] ([carrier_id])
GO
ALTER TABLE [dbo].[insurance_carrier_import_template] CHECK CONSTRAINT [FK_insurance_carrier_report_import_template_insurance_carrier]
GO
ALTER TABLE [dbo].[insurance_contribution]  WITH CHECK ADD  CONSTRAINT [FK_insurance_contribution_contribution] FOREIGN KEY([contribution_id])
REFERENCES [dbo].[contribution] ([contribution_id])
GO
ALTER TABLE [dbo].[insurance_contribution] CHECK CONSTRAINT [FK_insurance_contribution_contribution]
GO
ALTER TABLE [dbo].[insurance_contribution]  WITH CHECK ADD  CONSTRAINT [FK_insurance_contribution_employee_classification] FOREIGN KEY([classification_id])
REFERENCES [dbo].[employee_classification] ([classification_id])
GO
ALTER TABLE [dbo].[insurance_contribution] CHECK CONSTRAINT [FK_insurance_contribution_employee_classification]
GO
ALTER TABLE [dbo].[insurance_contribution]  WITH CHECK ADD  CONSTRAINT [FK_insurance_contribution_insurance] FOREIGN KEY([insurance_id])
REFERENCES [dbo].[insurance] ([insurance_id])
GO
ALTER TABLE [dbo].[insurance_contribution] CHECK CONSTRAINT [FK_insurance_contribution_insurance]
GO
ALTER TABLE [dbo].[insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_insurance_coverage_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[insurance_coverage] CHECK CONSTRAINT [FK_insurance_coverage_employee]
GO
ALTER TABLE [dbo].[insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_insurance_coverage_employee_dependents] FOREIGN KEY([dependent_id])
REFERENCES [dbo].[employee_dependents] ([dependent_id])
GO
ALTER TABLE [dbo].[insurance_coverage] CHECK CONSTRAINT [FK_insurance_coverage_employee_dependents]
GO
ALTER TABLE [dbo].[insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_insurance_coverage_insurance_carrier] FOREIGN KEY([carrier_id])
REFERENCES [dbo].[insurance_carrier] ([carrier_id])
GO
ALTER TABLE [dbo].[insurance_coverage] CHECK CONSTRAINT [FK_insurance_coverage_insurance_carrier]
GO
ALTER TABLE [dbo].[insurance_coverage]  WITH CHECK ADD  CONSTRAINT [FK_insurance_coverage_tax_year] FOREIGN KEY([tax_year])
REFERENCES [dbo].[tax_year] ([tax_year])
GO
ALTER TABLE [dbo].[insurance_coverage] CHECK CONSTRAINT [FK_insurance_coverage_tax_year]
GO
ALTER TABLE [dbo].[insurance_coverage_editable]  WITH CHECK ADD  CONSTRAINT [FK_insurance_coverage_editable_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[insurance_coverage_editable] CHECK CONSTRAINT [FK_insurance_coverage_editable_employee]
GO
ALTER TABLE [dbo].[insurance_coverage_editable]  WITH CHECK ADD  CONSTRAINT [FK_insurance_coverage_editable_employee_dependents] FOREIGN KEY([dependent_id])
REFERENCES [dbo].[employee_dependents] ([dependent_id])
GO
ALTER TABLE [dbo].[insurance_coverage_editable] CHECK CONSTRAINT [FK_insurance_coverage_editable_employee_dependents]
GO
ALTER TABLE [dbo].[insurance_coverage_editable]  WITH CHECK ADD  CONSTRAINT [FK_insurance_coverage_editable_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[insurance_coverage_editable] CHECK CONSTRAINT [FK_insurance_coverage_editable_employer]
GO
ALTER TABLE [dbo].[insurance_coverage_editable]  WITH CHECK ADD  CONSTRAINT [FK_insurance_coverage_editable_tax_year] FOREIGN KEY([tax_year])
REFERENCES [dbo].[tax_year] ([tax_year])
GO
ALTER TABLE [dbo].[insurance_coverage_editable] CHECK CONSTRAINT [FK_insurance_coverage_editable_tax_year]
GO
ALTER TABLE [dbo].[invoice]  WITH CHECK ADD  CONSTRAINT [FK_invoice_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[invoice] CHECK CONSTRAINT [FK_invoice_employer]
GO
ALTER TABLE [dbo].[measurement]  WITH CHECK ADD  CONSTRAINT [fk_measurement_employeetypeID] FOREIGN KEY([employee_type_id])
REFERENCES [dbo].[employee_type] ([employee_type_id])
GO
ALTER TABLE [dbo].[measurement] CHECK CONSTRAINT [fk_measurement_employeetypeID]
GO
ALTER TABLE [dbo].[measurement]  WITH CHECK ADD  CONSTRAINT [fk_measurement_employerID] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[measurement] CHECK CONSTRAINT [fk_measurement_employerID]
GO
ALTER TABLE [dbo].[measurement]  WITH CHECK ADD  CONSTRAINT [fk_measurement_measurementtypeID] FOREIGN KEY([measurement_type_id])
REFERENCES [dbo].[measurement_type] ([measurment_type_id])
GO
ALTER TABLE [dbo].[measurement] CHECK CONSTRAINT [fk_measurement_measurementtypeID]
GO
ALTER TABLE [dbo].[measurement]  WITH CHECK ADD  CONSTRAINT [fk_measurement_planyearID] FOREIGN KEY([plan_year_id])
REFERENCES [dbo].[plan_year] ([plan_year_id])
GO
ALTER TABLE [dbo].[measurement] CHECK CONSTRAINT [fk_measurement_planyearID]
GO
ALTER TABLE [dbo].[payroll]  WITH CHECK ADD  CONSTRAINT [FK_payroll_batch] FOREIGN KEY([batch_id])
REFERENCES [dbo].[batch] ([batch_id])
GO
ALTER TABLE [dbo].[payroll] CHECK CONSTRAINT [FK_payroll_batch]
GO
ALTER TABLE [dbo].[payroll]  WITH CHECK ADD  CONSTRAINT [FK_payroll_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[payroll] CHECK CONSTRAINT [FK_payroll_employee]
GO
ALTER TABLE [dbo].[payroll]  WITH CHECK ADD  CONSTRAINT [FK_payroll_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[payroll] CHECK CONSTRAINT [FK_payroll_employer]
GO
ALTER TABLE [dbo].[payroll]  WITH CHECK ADD  CONSTRAINT [FK_payroll_gross_pay_type] FOREIGN KEY([gp_id])
REFERENCES [dbo].[gross_pay_type] ([gross_pay_id])
GO
ALTER TABLE [dbo].[payroll] CHECK CONSTRAINT [FK_payroll_gross_pay_type]
GO
ALTER TABLE [dbo].[payroll_archive]  WITH CHECK ADD  CONSTRAINT [FK_payroll_archive_batch] FOREIGN KEY([batch_id])
REFERENCES [dbo].[batch] ([batch_id])
GO
ALTER TABLE [dbo].[payroll_archive] CHECK CONSTRAINT [FK_payroll_archive_batch]
GO
ALTER TABLE [dbo].[payroll_archive]  WITH CHECK ADD  CONSTRAINT [FK_payroll_archive_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[payroll_archive] CHECK CONSTRAINT [FK_payroll_archive_employee]
GO
ALTER TABLE [dbo].[payroll_archive]  WITH CHECK ADD  CONSTRAINT [FK_payroll_archive_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[payroll_archive] CHECK CONSTRAINT [FK_payroll_archive_employer]
GO
ALTER TABLE [dbo].[payroll_archive]  WITH CHECK ADD  CONSTRAINT [FK_payroll_archive_gross_pay_type] FOREIGN KEY([gp_id])
REFERENCES [dbo].[gross_pay_type] ([gross_pay_id])
GO
ALTER TABLE [dbo].[payroll_archive] CHECK CONSTRAINT [FK_payroll_archive_gross_pay_type]
GO
ALTER TABLE [dbo].[payroll_summer_averages]  WITH CHECK ADD  CONSTRAINT [FK_payroll_summer_averages_batch] FOREIGN KEY([batch_id])
REFERENCES [dbo].[batch] ([batch_id])
GO
ALTER TABLE [dbo].[payroll_summer_averages] CHECK CONSTRAINT [FK_payroll_summer_averages_batch]
GO
ALTER TABLE [dbo].[payroll_summer_averages]  WITH CHECK ADD  CONSTRAINT [FK_payroll_summer_averages_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[payroll_summer_averages] CHECK CONSTRAINT [FK_payroll_summer_averages_employee]
GO
ALTER TABLE [dbo].[payroll_summer_averages]  WITH CHECK ADD  CONSTRAINT [FK_payroll_summer_averages_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[payroll_summer_averages] CHECK CONSTRAINT [FK_payroll_summer_averages_employer]
GO
ALTER TABLE [dbo].[payroll_summer_averages]  WITH CHECK ADD  CONSTRAINT [FK_payroll_summer_averages_gross_pay_type] FOREIGN KEY([gp_id])
REFERENCES [dbo].[gross_pay_type] ([gross_pay_id])
GO
ALTER TABLE [dbo].[payroll_summer_averages] CHECK CONSTRAINT [FK_payroll_summer_averages_gross_pay_type]
GO
ALTER TABLE [dbo].[payroll_summer_averages]  WITH CHECK ADD  CONSTRAINT [FK_payroll_summer_averages_plan_year] FOREIGN KEY([plan_year_id])
REFERENCES [dbo].[plan_year] ([plan_year_id])
GO
ALTER TABLE [dbo].[payroll_summer_averages] CHECK CONSTRAINT [FK_payroll_summer_averages_plan_year]
GO
ALTER TABLE [dbo].[plan_year]  WITH CHECK ADD  CONSTRAINT [fk_planyear_employerID] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[plan_year] CHECK CONSTRAINT [fk_planyear_employerID]
GO
ALTER TABLE [dbo].[tax_year_1095c_approval]  WITH CHECK ADD  CONSTRAINT [FK_tax_year_1095c_approval_employee] FOREIGN KEY([employee_id])
REFERENCES [dbo].[employee] ([employee_id])
GO
ALTER TABLE [dbo].[tax_year_1095c_approval] CHECK CONSTRAINT [FK_tax_year_1095c_approval_employee]
GO
ALTER TABLE [dbo].[tax_year_1095c_approval]  WITH CHECK ADD  CONSTRAINT [FK_tax_year_1095c_approval_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[tax_year_1095c_approval] CHECK CONSTRAINT [FK_tax_year_1095c_approval_employer]
GO
ALTER TABLE [dbo].[tax_year_1095c_approval]  WITH CHECK ADD  CONSTRAINT [FK_tax_year_1095c_approval_tax_year] FOREIGN KEY([tax_year])
REFERENCES [dbo].[tax_year] ([tax_year])
GO
ALTER TABLE [dbo].[tax_year_1095c_approval] CHECK CONSTRAINT [FK_tax_year_1095c_approval_tax_year]
GO
ALTER TABLE [dbo].[tax_year_approval]  WITH CHECK ADD  CONSTRAINT [FK_tax_year_approval_employer] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[tax_year_approval] CHECK CONSTRAINT [FK_tax_year_approval_employer]
GO
ALTER TABLE [dbo].[tax_year_approval]  WITH CHECK ADD  CONSTRAINT [FK_tax_year_approval_tax_year] FOREIGN KEY([tax_year])
REFERENCES [dbo].[tax_year] ([tax_year])
GO
ALTER TABLE [dbo].[tax_year_approval] CHECK CONSTRAINT [FK_tax_year_approval_tax_year]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [fk_user_employerID] FOREIGN KEY([employer_id])
REFERENCES [dbo].[employer] ([employer_id])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [fk_user_employerID]
GO
ALTER TABLE [dbo].[payroll]  WITH CHECK ADD  CONSTRAINT [CK__payroll__act_hou__30AE302A] CHECK  (([act_hours]>(0)))
GO
ALTER TABLE [dbo].[payroll] CHECK CONSTRAINT [CK__payroll__act_hou__30AE302A]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[15] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "alert"
            Begin Extent = 
               Top = 74
               Left = 67
               Bottom = 204
               Right = 237
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "alert_type"
            Begin Extent = 
               Top = 123
               Left = 713
               Bottom = 236
               Right = 883
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "View_alerts_union"
            Begin Extent = 
               Top = 0
               Left = 445
               Bottom = 113
               Right = 615
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "View_employer_alerts"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 273
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "View_billContact_alerts"
            Begin Extent = 
               Top = 22
               Left = 465
               Bottom = 140
               Right = 775
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_billing_contact'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_billing_contact'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[15] 2[14] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "import_employee"
            Begin Extent = 
               Top = 7
               Left = 80
               Bottom = 249
               Right = 411
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "alert"
            Begin Extent = 
               Top = 6
               Left = 449
               Bottom = 136
               Right = 635
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_import_employee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_import_employee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[20] 2[17] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "import_payroll"
            Begin Extent = 
               Top = 10
               Left = 437
               Bottom = 260
               Right = 720
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "View_employer_alerts"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_import_payroll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_import_payroll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[15] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "employee_insurance_offer"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 254
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "View_employer_alerts"
            Begin Extent = 
               Top = 86
               Left = 66
               Bottom = 216
               Right = 324
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 4095
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1770
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_insurance_offer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_insurance_offer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "View_employer_alerts"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "View_missingInsuranceType_alerts"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 102
               Right = 626
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_insurance_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_insurance_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "View_employer_alerts"
            Begin Extent = 
               Top = 8
               Left = 59
               Bottom = 187
               Right = 262
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "View_irs_alerts"
            Begin Extent = 
               Top = 32
               Left = 374
               Bottom = 187
               Right = 544
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_irs_contact'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_irs_contact'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "measurement"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 335
               Right = 319
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "alert"
            Begin Extent = 
               Top = 11
               Left = 375
               Bottom = 247
               Right = 695
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_summer_window'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_summer_window'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[7] 4[38] 2[41] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_union'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_alerts_union'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_all_insurance_coverage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_all_insurance_coverage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[31] 4[30] 2[19] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "employee"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 157
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "measurement"
            Begin Extent = 
               Top = 19
               Left = 456
               Bottom = 149
               Right = 661
            End
            DisplayFlags = 280
            TopColumn = 4
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 42
         Width = 284
         Width = 1290
         Width = 1335
         Width = 1485
         Width = 2280
         Width = 1500
         Width = 1725
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
       ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Avg_Hours_Ongoing'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'  Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Avg_Hours_Ongoing'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Avg_Hours_Ongoing'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "employee"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "measurement"
            Begin Extent = 
               Top = 75
               Left = 492
               Bottom = 205
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Avg_Hours_Ongoing_limbo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Avg_Hours_Ongoing_limbo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "employee"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "measurement"
            Begin Extent = 
               Top = 88
               Left = 432
               Bottom = 218
               Right = 637
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Avg_Hours_Ongoing_meas'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Avg_Hours_Ongoing_meas'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[19] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "user"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_billContact_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_billContact_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[8] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "employee_dependents"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 298
               Right = 291
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "insurance_coverage"
            Begin Extent = 
               Top = 22
               Left = 411
               Bottom = 304
               Right = 705
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 27
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_dependent_insurance_coverage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_dependent_insurance_coverage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "employee"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "employee_classification"
            Begin Extent = 
               Top = 182
               Left = 645
               Bottom = 312
               Right = 817
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "aca_status"
            Begin Extent = 
               Top = 243
               Left = 277
               Bottom = 339
               Right = 447
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "hr_status"
            Begin Extent = 
               Top = 6
               Left = 310
               Bottom = 136
               Right = 480
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 34
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Employee_Export'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'        Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2625
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Employee_Export'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Employee_Export'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[15] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "insurance_coverage"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 263
               Right = 292
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "employee"
            Begin Extent = 
               Top = 3
               Left = 671
               Bottom = 214
               Right = 905
            End
            DisplayFlags = 280
            TopColumn = 13
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 24
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_employee_insurance_coverage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_employee_insurance_coverage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "alert"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 218
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "alert_type"
            Begin Extent = 
               Top = 51
               Left = 470
               Bottom = 291
               Right = 640
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_employer_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_employer_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[50] 4[11] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "equivalency"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 318
               Right = 233
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "unit"
            Begin Extent = 
               Top = 139
               Left = 543
               Bottom = 268
               Right = 713
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "equivalency_type"
            Begin Extent = 
               Top = 6
               Left = 271
               Bottom = 102
               Right = 466
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_employer_equivalency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_employer_equivalency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[45] 4[26] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "employee_insurance_offer"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 366
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "employee"
            Begin Extent = 
               Top = 15
               Left = 501
               Bottom = 268
               Right = 746
            End
            DisplayFlags = 280
            TopColumn = 18
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3480
         Alias = 1830
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_insurance_alert_details'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_insurance_alert_details'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "employee_classification"
            Begin Extent = 
               Top = 22
               Left = 652
               Bottom = 198
               Right = 824
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "insurance_contribution"
            Begin Extent = 
               Top = 6
               Left = 248
               Bottom = 204
               Right = 420
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Insurance_Contributions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Insurance_Contributions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "user"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_irs_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_irs_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "View_PlanYear_Insurance"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 221
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_missingInsuranceType_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_missingInsuranceType_alerts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "payroll"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "employee"
            Begin Extent = 
               Top = 141
               Left = 508
               Bottom = 271
               Right = 713
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "gross_pay_type"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 15
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_payroll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_payroll'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "payroll_summer_averages"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "gross_pay_type"
            Begin Extent = 
               Top = 38
               Left = 437
               Bottom = 168
               Right = 607
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_payroll_summer_avg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_payroll_summer_avg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "plan_year"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "insurance"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 429
            End
            DisplayFlags = 280
            TopColumn = 7
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PlanYear_Insurance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PlanYear_Insurance'
GO
USE [master]
GO
ALTER DATABASE [aca-demo] SET  READ_WRITE 
GO
USE [aca-demo]
GO
SET IDENTITY_INSERT [dbo].[state] ON 
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (1, N'Alabama', N'AL')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (2, N'Alaska', N'AK')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (3, N'Arizona', N'AZ')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (4, N'Arkansas', N'AR')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (5, N'California', N'CA')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (6, N'Colorado', N'CO')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (7, N'Connecticut', N'CT')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (8, N'Delaware', N'DE')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (9, N'Florida', N'FL')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (10, N'Georgia', N'GA')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (11, N'Hawaii', N'HI')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (12, N'Idaho', N'ID')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (13, N'Illinois', N'IL')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (14, N'Indiana', N'IN')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (15, N'Iowa', N'IA')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (16, N'Kansas', N'KS')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (17, N'Kentucky', N'KY')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (18, N'Louisiana', N'LA')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (19, N'Maine', N'ME')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (20, N'Maryland', N'MD')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (21, N'Massachusetts', N'MA')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (22, N'Michigan', N'MI')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (23, N'Minnesota', N'MN')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (24, N'Mississippi', N'MS')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (25, N'Missouri', N'MO')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (26, N'Montana', N'MT')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (27, N'Nebraska', N'NE')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (28, N'Nevada', N'NV')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (29, N'New Hampshire', N'NH')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (30, N'New Jersey', N'NH')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (31, N'New Mexico', N'NM')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (32, N'New York', N'NY')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (33, N'North Carolina', N'NC')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (34, N'North Dakota', N'ND')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (35, N'Ohio', N'OH')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (36, N'Oklahoma', N'OK')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (37, N'Oregon', N'OR')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (38, N'Pennsylvania', N'PA')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (39, N'Rhode Island', N'RI')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (40, N'South Carolina', N'SC')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (41, N'South Dakota', N'SD')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (42, N'Tennessee', N'TN')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (43, N'Texas', N'TX')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (44, N'Utah', N'UT')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (45, N'Vermont', N'VT')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (46, N'Virginia', N'VA')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (47, N'Washington', N'WA')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (48, N'West Virginia', N'WV')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (49, N'Wisconsin', N'WI')
GO
INSERT [dbo].[state] ([state_id], [description], [abbreviation]) VALUES (50, N'Wyoming', N'WY')
GO
SET IDENTITY_INSERT [dbo].[state] OFF
GO
SET IDENTITY_INSERT [dbo].[aca_status] ON 
GO
INSERT [dbo].[aca_status] ([aca_status_id], [name]) VALUES (1, N'Seasonal')
GO
INSERT [dbo].[aca_status] ([aca_status_id], [name]) VALUES (2, N'Part time/Variable')
GO
INSERT [dbo].[aca_status] ([aca_status_id], [name]) VALUES (3, N'Termed')
GO
INSERT [dbo].[aca_status] ([aca_status_id], [name]) VALUES (4, N'Cobra Elected')
GO
INSERT [dbo].[aca_status] ([aca_status_id], [name]) VALUES (5, N'Full time')
GO
INSERT [dbo].[aca_status] ([aca_status_id], [name]) VALUES (6, N'Special Unpaid Leave')
GO
INSERT [dbo].[aca_status] ([aca_status_id], [name]) VALUES (7, N'Initial Import')
GO
SET IDENTITY_INSERT [dbo].[aca_status] OFF
GO
SET IDENTITY_INSERT [dbo].[alert_type] ON 
GO
INSERT [dbo].[alert_type] ([alert_type_id], [name], [image_url], [table_name]) VALUES (1, N'Payroll Import', N'~/images/circle_red.png', N'import_payroll')
GO
INSERT [dbo].[alert_type] ([alert_type_id], [name], [image_url], [table_name]) VALUES (2, N'Employee Import', N'~/images/circle_red.png', N'import_employee')
GO
INSERT [dbo].[alert_type] ([alert_type_id], [name], [image_url], [table_name]) VALUES (3, N'Insurance Offer', N'~/images/circle_red.png', N'view_alert_insurance_offer')
GO
INSERT [dbo].[alert_type] ([alert_type_id], [name], [image_url], [table_name]) VALUES (4, N'Summer Window', N'~/images/circle_red.png', N'view_alerts_summer_window')
GO
INSERT [dbo].[alert_type] ([alert_type_id], [name], [image_url], [table_name]) VALUES (5, N'Billing Contact', N'~/images/circle_orange.png', N'view_billContact_alerts')
GO
INSERT [dbo].[alert_type] ([alert_type_id], [name], [image_url], [table_name]) VALUES (6, N'IRS Contact', N'~/images/circle_orange.png', N'view_alerts_irs_contact')
GO
INSERT [dbo].[alert_type] ([alert_type_id], [name], [image_url], [table_name]) VALUES (7, N'Insurance Type', N'~/images/circle_orange.png', N'view_alerts_insurance_type')
GO
INSERT [dbo].[alert_type] ([alert_type_id], [name], [image_url], [table_name]) VALUES (8, N'Carrier Import', N'~/images/circle_red.png', N'import_insurance_coverage')
GO
SET IDENTITY_INSERT [dbo].[alert_type] OFF
GO
-- missing identity insert is intentional. gc5
INSERT [dbo].[affordability_safe_harbor] ([ash_code], [Description]) VALUES (N'2F', N'W2')
GO
INSERT [dbo].[affordability_safe_harbor] ([ash_code], [Description]) VALUES (N'2G', N'Federal Poverty Line')
GO
INSERT [dbo].[affordability_safe_harbor] ([ash_code], [Description]) VALUES (N'2H', N'Rate of Pay')
GO
-- missing identity insert is intentional. gc5
INSERT [dbo].[contribution] ([contribution_id], [name]) VALUES (N'$', N'Dollar')
GO
INSERT [dbo].[contribution] ([contribution_id], [name]) VALUES (N'%', N'Percentage')
GO
SET IDENTITY_INSERT [dbo].[employer_type] ON 
GO
INSERT [dbo].[employer_type] ([employer_type_id], [description]) VALUES (1, N'K-12 Public School District')
GO
INSERT [dbo].[employer_type] ([employer_type_id], [description]) VALUES (2, N'Charter School')
GO
INSERT [dbo].[employer_type] ([employer_type_id], [description]) VALUES (3, N'Other')
GO
INSERT [dbo].[employer_type] ([employer_type_id], [description]) VALUES (4, N'Designated Government Entity')
GO
SET IDENTITY_INSERT [dbo].[employer_type] OFF
GO
SET IDENTITY_INSERT [dbo].[equivalency_type] ON 
GO
INSERT [dbo].[equivalency_type] ([equivalency_type_id], [name]) VALUES (1, N'Payroll')
GO
INSERT [dbo].[equivalency_type] ([equivalency_type_id], [name]) VALUES (2, N'Date Range') -- not really used, the Payroll option was the best. Most folks went the payroll route.
GO
SET IDENTITY_INSERT [dbo].[equivalency_type] OFF
GO
SET IDENTITY_INSERT [dbo].[insurance_type] ON 
GO
INSERT [dbo].[insurance_type] ([insurance_type_id], [name]) VALUES (1, N'Fully-Insured')
GO
INSERT [dbo].[insurance_type] ([insurance_type_id], [name]) VALUES (2, N'Self-Insured')
GO
SET IDENTITY_INSERT [dbo].[insurance_type] OFF
GO
SET IDENTITY_INSERT [dbo].[measurement_type] ON 
GO
INSERT [dbo].[measurement_type] ([measurment_type_id], [description]) VALUES (1, N'Transition') -- magic year version. gc5
GO
INSERT [dbo].[measurement_type] ([measurment_type_id], [description]) VALUES (2, N'Ongoing')
GO
SET IDENTITY_INSERT [dbo].[measurement_type] OFF
GO
SET IDENTITY_INSERT [dbo].[month] ON 
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (1, N'January')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (2, N'February')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (3, N'March')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (4, N'April')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (5, N'May')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (6, N'June')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (7, N'July')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (8, N'August')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (9, N'September')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (10, N'October')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (11, N'November')
GO
INSERT [dbo].[month] ([month_id], [name]) VALUES (12, N'December')
GO
SET IDENTITY_INSERT [dbo].[month] OFF
GO
-- missing identity insert is intentional. gc5
INSERT [dbo].[tax_year] ([tax_year]) VALUES (2015)
GO
SET IDENTITY_INSERT [dbo].[unit] ON 
GO
-- Per EFS, 1 and 2 where deleted. gc5
INSERT [dbo].[unit] ([unit_id], [name]) VALUES (3, N'Pay Period')
GO
INSERT [dbo].[unit] ([unit_id], [name]) VALUES (4, N'Unit')
GO
SET IDENTITY_INSERT [dbo].[unit] OFF
GO
