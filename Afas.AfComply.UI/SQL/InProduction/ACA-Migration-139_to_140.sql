USE [aca]
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
-- Modifications:06/20/2017 (GN)reverted changes by adding @hdate, @cdate and @impEnd to the stroedproc.
-- =============================================
ALTER PROCEDURE [dbo].[UPDATE_employee]
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
		dob=@dob,
		initialMeasurmentEnd=@impEnd,
		terminationDate = @tdate,
		meas_plan_year_id=@planyearid,
		modOn=@modOn,
		modBy=@modBy, 
		classification_id=@classID,
		aca_status_id=@actStatusID
	WHERE
		employee_id=@employee_id;

END
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/18/2014>
-- Description:	<This stored procedure is meant update an existing employee from the import table. It will then Delete the record from the import_employee table.
-- Modifications:06/20/2017 (GN)reverted changes by adding @hdate, @cdate and @impEnd to the stroedproc.
--			
-- =============================================
ALTER PROCEDURE [dbo].[TRANSFER_import_existing_employee]
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
			EXEC INSERT_ErrorLogging
		END CATCH

END
GO
