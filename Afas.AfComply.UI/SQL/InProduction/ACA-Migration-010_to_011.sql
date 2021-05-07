USE [aca]
GO

/****** Object:  Table [dbo].[ErrorLog]    Script Date: 8/18/2016 1:06:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ErrorLog](
       [ErrorLogID] [int] IDENTITY(1,1) NOT NULL,
       [ErrorTime] [datetime] NOT NULL CONSTRAINT [DF_ErrorLog_ErrorTime]  DEFAULT (getdate()),
       [UserName] [sysname] NOT NULL,
       [ErrorNumber] [int] NOT NULL,
       [ErrorSeverity] [int] NULL,
       [ErrorState] [int] NULL,
       [ErrorProcedure] [nvarchar](126) NULL,
       [ErrorLine] [int] NULL,
       [ErrorMessage] [nvarchar](4000) NOT NULL,
CONSTRAINT [PK_ErrorLog_ErrorLogID] PRIMARY KEY CLUSTERED 
(
       [ErrorLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for ErrorLog records.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorLogID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The date and time at which the error occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The user who executed the batch in which the error occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'UserName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The error number of the error that occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The severity of the error that occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorSeverity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The state number of the error that occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorState'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the stored procedure or trigger where the error occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorProcedure'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The line number at which the error occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The message text of the error that occurred.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorMessage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Audit table tracking errors in the the CloneOfAW database that are caught by the CATCH block of a TRY...CATCH construct. Data is inserted by stored procedure dbo.uspLogError when it is executed from inside the CATCH block of a TRY...CATCH construct.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key (clustered) constraint' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'CONSTRAINT',@level2name=N'PK_ErrorLog_ErrorLogID'
GO

CREATE PROCEDURE [dbo].[INSERT_ErrorLogging] 

AS
BEGIN 
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       INSERT INTO dbo.ErrorLog ([UserName]
      ,[ErrorNumber]
      ,[ErrorSeverity]
      ,[ErrorState]
      ,[ErrorProcedure]
      ,[ErrorLine]
      ,[ErrorMessage])
       VALUES (SYSTEM_USER 
         ,ERROR_NUMBER() 
         ,ERROR_SEVERITY() 
         ,ERROR_STATE() 
         ,ERROR_PROCEDURE() 
         ,ERROR_LINE() 
         ,ERROR_MESSAGE());
END
GO



CREATE PROCEDURE [dbo].[SELECT_errorLog]
       @time datetime,
       @errorNumber int,
       @errorLine int,
       @errorProcedure varchar(50),
       @rowsPage int,
       @pageNumber int

AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    -- Insert statements for procedure here

       SELECT [ErrorLogID]
      ,[ErrorTime]
      ,[UserName]
      ,[ErrorNumber]
      ,[ErrorSeverity]
      ,[ErrorState]
      ,[ErrorProcedure]
      ,[ErrorLine]
      ,[ErrorMessage]
  FROM [aca].[dbo].[ErrorLog]
  WHERE ((@time is null) OR (ErrorTime = @time))
  AND  ((@errorNumber is null) OR (ErrorNumber = @errorNumber))
  AND   ((@errorLine is null) OR (ErrorLine = @errorLine))
  AND   ((@errorProcedure is null) OR (ErrorLine = @errorLine))
  ORDER BY Errortime DESC OFFSET ((@pageNumber  - 1) * @rowsPage) ROWS
  FETCH NEXT @rowsPage ROWS ONLY;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO




CREATE PROCEDURE [dbo].[UPDATE_floater]

       @setFloater bit,
       @userId int

AS
BEGIN TRY

SET NOCOUNT ON;

    UPDATE [aca].[dbo].[user] SET floater = @setFloater
       WHERE [user_id] = @userId AND Employer_id = 1
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO




CREATE PROCEDURE [dbo].[DELETE_Alert]
       @alertId int,
       @employerId int
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    DELETE FROM dbo.alert 
       WHERE alert_type_id = @alertId AND employer_id = @employerId
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
       @employerTypeId int
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
             employer_type_id = @employerTypeId
       WHERE
             employer_id = @employerID;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO




ALTER PROCEDURE [dbo].[REMOVE_EMPLOYER_FROM_ACT]
       @employerID int
AS
BEGIN TRY
DELETE 
  aca.dbo.equivalency
  where employer_id=@employerID

DELETE  aca.dbo.employee_insurance_offer
  where employer_id=@employerID

--Insurance Contributions
DELETE
aca.dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--Summer Payroll Supplemental hours
DELETE aca.dbo.payroll_summer_averages
  WHERE employer_id=@employerID;

DELETE
  aca.dbo.import_employee
  WHERE employerID=@employerID

DELETE aca.dbo.alert_archive
       WHERE employer_id=@employerID

DELETE aca.dbo.payroll_archive
       WHERE employer_id=@employerID

DELETE
  aca.dbo.import_payroll
  WHERE employerid=@employerID

DELETE 
  [aca].[dbo].[payroll]
  WHERE employer_id=@employerID

DELETE  
  FROM aca.dbo.employee
  WHERE employer_id=@employerID

DELETE
  aca.dbo.hr_status
  WHERE employer_id=@employerID

DELETE
  aca.dbo.gross_pay_filter
  WHERE employer_id=@employerID

DELETE
  aca.dbo.gross_pay_type
  WHERE employer_id=@employerID

DELETE
  aca.dbo.measurement
  WHERE employer_id=@employerID

DELETE
  aca.dbo.plan_year
  WHERE employer_id=@employerID

DELETE
  aca.dbo.alert
  WHERE employer_id=@employerID

DELETE
  aca.dbo.[user]
  WHERE employer_id=@employerID

DELETE
  aca.dbo.employee_type
  WHERE employer_id=@employerID

DELETE 
  aca.dbo.invoice
  WHERE employer_id=@employerID

DELETE
  aca.dbo.batch
  WHERE employer_id=@employerID

--All Employee Classifications. 
DELETE
  aca.dbo.employee_classification
  WHERE employer_id=@employerID

DELETE 
  aca.dbo.employer
  WHERE employer_id=@employerID
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO
