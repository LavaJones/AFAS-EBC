USE [aca]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[INSERT_new_registration]
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
       @dbaName varchar(100),
       @employerID int OUTPUT
AS

BEGIN
       DECLARE @userid int;
	   DECLARE @planyearGroupId int;
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
                    --                  - Return the new Employer_ID
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
                           @dbaName,
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
					-- Step 4.1: Creat a Plan Year Group For this employer
					EXEC [INSERT_PlanYearGroup]
						@lastModBy,
						@name,
						@employerID,
						@planyearGroupId OUTPUT;

					-- Now insert the plan year
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
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
                           null,
						   @planyearGroupId,
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
										null,
									   null,
									   null,
									   null,
									   null,
									   null,
									   null,
									   null,
									   @planyearGroupId,
                                        @planyearid OUTPUT; 
                           END

                    COMMIT
             END TRY
             BEGIN CATCH
                    --If something fails return 0.
                    SET @employerID = 0;
                    ROLLBACK TRANSACTION
                    exec dbo.INSERT_ErrorLogging
             END CATCH

END
