USE [aca]

ALTER TABLE dbo.employee DROP CONSTRAINT fk_employee_stateID
ALTER TABLE dbo.employee ALTER COLUMN state_id integer NULL
ALTER TABLE dbo.employer ADD DBAName varchar(100) NULL
GO
ALTER PROCEDURE [dbo].[INSERT_new_employer]
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
		DBAName)
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
		@dbaName)

SELECT @empid = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_employer]
       @employerID int,
       @name varchar(50),
       @address varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @logo varchar(50),
       @ein varchar(50),
       @employerTypeId int,
	   @dbaName varchar(100)
AS
BEGIN TRY
       UPDATE employer
       SET
             name = @name, 
             [address] = @address,
             city = @city,
             state_id = @stateID, 
             zip = @zip,  
             img_logo = @logo,
             ein = @ein,
             employer_type_id = @employerTypeId,
			 DBAName = @dbaName
       WHERE
             employer_id = @employerID;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH