USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SearchEmployee]    Script Date: 6/4/2018 3:13:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SearchEmployee]
@FirstName varchar(50),
@LastName varchar(50),
@MiddleName varchar(50),
@EmployerID INT


AS 
begin 


SELECT 

[employee_id]
      ,[employee_type_id]
      ,[HR_status_id]
      ,[employer_id]
      ,[fName]  
      ,[mName] 
      ,[lName] 
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[hireDate]
      ,[currDate]
      ,[ssn]
      ,[ext_emp_id]
      ,[terminationDate]
      ,[dob]
      ,[initialMeasurmentEnd]
      ,[plan_year_id]
      ,[limbo_plan_year_id]
      ,[meas_plan_year_id]
      ,[modOn]
      ,[modBy]
      ,[plan_year_avg_hours]
      ,[limbo_plan_year_avg_hours]
      ,[meas_plan_year_avg_hours]
      ,[imp_plan_year_avg_hours]
      ,[classification_id]
      ,[aca_status_id]
      ,[ResourceId]

     FROM dbo.employee
	 	  


	WHERE ([fName] like @FirstName  or [lName] LIKE @LastName or [mName] LIKE @MiddleName) AND [employer_id] = @employerID;
	

	
	END
	GO
	GRANT EXEC ON [dbo].[SearchEmployee] TO [aca-user] 

