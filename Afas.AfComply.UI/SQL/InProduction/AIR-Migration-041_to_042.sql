USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Ryan McCully>
-- Create date: <3/23/2016>
-- Description:	<This stored procedure updates an Employee's name, ssn in ACA and AIR.>
-- =============================================
CREATE PROCEDURE [dbo].[spUPDATE_employee__ACA_AIR]
	@employee_id int,
	@fname varchar(50),
	@mname varchar(50),
	@lname varchar(50),
	@address varchar(50),
	@city varchar(50),
	@stateID int,
	@zip varchar(5),
	@ssn varchar(50) = NULL,
	@modBy varchar(50)
AS
BEGIN

	DECLARE @stateCode Varchar(2);
	SELECT 
		@stateCode = [abbreviation] 
	FROM 
		[aca].[dbo].[state]
	WHERE 
		state_id = @stateID


	IF @ssn is null
		Select 
			@ssn = ssn 
		from 
			[aca].[dbo].[employee]	
		WHERE
			employee_id=@employee_id;
			 

	UPDATE 
		[aca].[dbo].[employee]
	SET
		fName = @fname,
		mName = @mname,
		lName = @lname, 
		[address] = @address,
		city = @city,
		state_id = @stateID, 
		zip = @zip,  
		ssn = @ssn,
		modOn=SYSDATETIME(),
		modBy=@modBy
	WHERE
		employee_id=@employee_id;


	UPDATE  
		[air].[emp].[employee]
	SET
      [first_name] = @fname
      ,[middle_name] = @mname
      ,[last_name] = @lname
      ,[address] = @address
      ,[city] = @city
      ,[state_code] = @stateCode
      ,[zipcode] = @zip
      ,[ssn] = @ssn
  	WHERE
		[employee_id]=@employee_id;

END

GO
GRANT EXECUTE ON [dbo].[spUPDATE_employee__ACA_AIR] TO [air-user] AS [DBO]
GO

--GO
--GRANT EXECUTE ON [dbo].[spUPDATE_employee__ACA_AIR] TO [aca-user] AS [DBO]
--GO