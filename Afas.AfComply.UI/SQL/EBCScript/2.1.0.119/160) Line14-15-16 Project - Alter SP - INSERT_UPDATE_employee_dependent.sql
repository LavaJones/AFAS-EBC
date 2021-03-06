USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[INSERT_UPDATE_employee_dependent]    Script Date: 12/14/2017 10:47:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/24/2014>
-- Description:	<This stored procedure is meant to update or insert Employee Dependents.>	
-- Modifications:
--			12/14/2017
--				- Added the modBy and EntityStatusID
-- =============================================
ALTER PROCEDURE [dbo].[INSERT_UPDATE_employee_dependent]
	@currDepID int,
	@employeeID int,
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@ssn varchar(50),
	@dob varchar(50),
	@modBy varchar(50),
	@entityStatusID int,
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
			dob, 
			CreatedBy,
			CreatedDate,
			ModifiedBy,
			ModifiedDate,
			EntityStatusID)
		VALUES(
			@employeeID,
			@fname,
			@mname,
			@lname,
			@ssn,
			@dob,
			@modBy,
			GETDATE(),
			@modBy,
			GETDATE(),
			@entityStatusID)
	END
ELSE
	BEGIN
		UPDATE [employee_dependents]
		SET
			fName=@fname, 
			mName=@mname,
			lName=@lname,
			ssn=@ssn,
			dob=@dob,
			ModifiedBy=@modBy,
			ModifiedDate=GETDATE(),
			EntityStatusID=@entityStatusID
		WHERE
			dependent_id=@dependentID;
	END

IF @dependentID <= 0
	BEGIN
		SET @dependentID = SCOPE_IDENTITY();
	END

SELECT @dependentID;
END

