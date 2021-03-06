USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[TRANSFER_import_existing_employee]    Script Date: 5/26/2017 12:31:17 PM ******/
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
				--@hdate,
				--@cdate,
				@ssn,
				@externalEmployeeID,
				@tdate,
				@dob,
				--@impEnd,
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

