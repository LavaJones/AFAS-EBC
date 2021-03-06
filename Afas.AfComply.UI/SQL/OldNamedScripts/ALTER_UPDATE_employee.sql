USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[UPDATE_employee]    Script Date: 6/5/2017 10:28:30 AM ******/
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
	@ssn varchar(50),
	@extemployeeid varchar(50),
	@tdate datetime,
	@dob datetime,
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
		ssn=@ssn,
		ext_emp_id=@extemployeeid,
		dob=@dob,
		terminationDate = @tdate,
		meas_plan_year_id=@planyearid,
		modOn=@modOn,
		modBy=@modBy, 
		classification_id=@classID,
		aca_status_id=@actStatusID
	WHERE
		employee_id=@employee_id;

END

