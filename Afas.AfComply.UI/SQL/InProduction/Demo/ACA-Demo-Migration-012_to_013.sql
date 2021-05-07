USE [aca-demo]
GO
 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityStatus](
      [EntityStatusId] [int] IDENTITY(1,1) NOT NULL,
      [EntityStatusName] [nvarchar](50) NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,
CONSTRAINT [XPKEntityStatus] PRIMARY KEY CLUSTERED
(
      [EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 

SET IDENTITY_INSERT EntityStatus ON
GO
INSERT into EntityStatus (EntityStatusId, EntityStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) VALUES (1, 'Active', 'SYSTEM', '2014-08-15', 'SYSTEM', '2014-08-15')
GO
INSERT into EntityStatus (EntityStatusId, EntityStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) VALUES (2, 'Inactive', 'SYSTEM', '2014-08-15', 'SYSTEM', '2014-08-15')
GO
INSERT into EntityStatus (EntityStatusId, EntityStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) VALUES (3, 'Deleted', 'SYSTEM', '2014-08-15', 'SYSTEM', '2014-08-15') 
GO
SET IDENTITY_INSERT EntityStatus OFF
GO

GRANT SELECT ON [dbo].[EntityStatus] TO [aca-user] AS [dbo]
GO

CREATE TABLE [dbo].[BreakInService](
      [BreakInServiceId] [bigint] IDENTITY(1,1) NOT NULL,
      [ResourceId] [uniqueidentifier] NOT NULL,
      [EntityStatusId] [int] NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,
      [StartDate] [datetime2](7) NULL,
      [EndDate] [datetime2](7) NULL,
      [Weeks] [int] NULL,
CONSTRAINT [PK_BreakInService_BreakInServiceId] PRIMARY KEY NONCLUSTERED
(
      [BreakInServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
  
ALTER TABLE [dbo].[BreakInService] ADD  DEFAULT (newid()) FOR [resourceId]
GO

ALTER TABLE [dbo].[BreakInService]  WITH CHECK ADD  CONSTRAINT [FK_BreakInService_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
 
ALTER TABLE [dbo].[BreakInService] CHECK CONSTRAINT [FK_BreakInService_EntityStatus]
GO

GRANT SELECT ON [dbo].[BreakInService] TO [aca-user] AS [dbo]
GO
 
CREATE TABLE [dbo].[measurementBreakInService](
      [measurementBreakInserviceId] [bigint] IDENTITY(1,1) NOT NULL,
      [EntityStatusId] [int] NOT NULL,
      [resourceId] [uniqueidentifier] NOT NULL,
      [measurement_id] [int] NOT NULL,
      [BreakInServiceId] [bigint] NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,
CONSTRAINT [PK_measurementBreakInService_measurementBreakInServiceId] PRIMARY KEY NONCLUSTERED
(
      [measurementBreakInserviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
 GO
 
ALTER TABLE [dbo].[measurementBreakInService] ADD  DEFAULT (newid()) FOR [resourceId]
GO
 
ALTER TABLE [dbo].[measurementBreakInService]  WITH CHECK ADD  CONSTRAINT [FK_measurementBreakInService_BreakInService] FOREIGN KEY([BreakInServiceId])
REFERENCES [dbo].[BreakInService] ([BreakInServiceId])
ON DELETE CASCADE
GO
 
ALTER TABLE [dbo].[measurementBreakInService] CHECK CONSTRAINT [FK_measurementBreakInService_BreakInService]
GO
 
ALTER TABLE [dbo].[measurementBreakInService]  WITH CHECK ADD  CONSTRAINT [FK_measurementBreakInService_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
 
ALTER TABLE [dbo].[measurementBreakInService] CHECK CONSTRAINT [FK_measurementBreakInService_EntityStatus]
GO
 
ALTER TABLE [dbo].[measurementBreakInService]  WITH CHECK ADD  CONSTRAINT [FK_measurementBreakInService_measurement] FOREIGN KEY([measurement_id])
REFERENCES [dbo].[measurement] ([measurement_id])
GO
 
ALTER TABLE [dbo].[measurementBreakInService] CHECK CONSTRAINT [FK_measurementBreakInService_measurement]
GO

GRANT SELECT ON [dbo].[measurementBreakInService] TO [aca-user] AS [dbo]
GO

-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_BreakInService]
      @CreatedBy nvarchar(50),
      @CreatedDate datetime2(7),
      @startDate datetime2(7),
      @endDate datetime2(7),
      @breakInServiceId int,
      @measurementId int,
      @week int
     
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
      INSERT INTO [dbo].[BreakInService] (EntityStatusId, CreatedBy, CreatedDate, startDate, endDate, Weeks)
      VALUES (1,@CreatedBy,@CreatedDate,@startDate,@endDate,@week)
   
      SELECT @breakInServiceId = BreakInServiceId FROM [dbo].[BreakInService]
      WHERE (startDate = @startDate AND endDate = @endDate) AND (CreatedBy = @CreatedBy AND CreatedDate = @CreatedDate)
 
      INSERT INTO [dbo].[measurementBreakInService] (EntityStatusId, measurement_id, BreakInServiceId, CreatedBy, CreatedDate)
      VALUES (1, @measurementId, @breakInServiceId, @CreatedBy, @CreatedDate)
END
GO 
GRANT EXECUTE ON [dbo].[INSERT_BreakInService] TO [aca-user] AS [dbo]
GO

-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_BreakInService]
      @employerId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    SELECT employer_id, bis.StartDate, bis.EndDate, bis.Weeks FROM [dbo].[measurement] mea INNER JOIN [dbo].[measurementBreakInService] mbis
      ON mea.measurement_id = mbis.measurement_id INNER JOIN [dbo].[BreakInService] bis ON mbis.BreakInServiceId = bis.BreakInServiceId
      WHERE mea.employer_id = @employerId
END
GO
GRANT EXECUTE ON [dbo].[SELECT_BreakInService] TO [aca-user] AS [dbo]
GO

-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_BreakInServiceStatus]
      @EntityStatus int,
      @BreakInServiceId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [dbo].[BreakInService] SET EntityStatusId = @EntityStatus
      WHERE BreakInServiceId = @BreakInServiceId
END
GO
GRANT EXECUTE ON [dbo].[UPDATE_BreakInServiceStatus] TO [aca-user] AS [dbo]
GO

ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
      @employerID int
AS
BEGIN TRY
 
--Equivalencies
DELETE
  aca.dbo.equivalency
  where employer_id=@employerID;
 
--Plan Year Archives/Insurance Offers
DELETE
  aca.dbo.employee_insurance_offer
  where employer_id=@employerID;
 
--Plan Year Archives/Insurance Offers
DELETE
  aca.dbo.employee_insurance_offer_archive
  where employer_id=@employerID;
 
DELETE
      aca.dbo.employee_dependents
      WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);
 
--Insurance Contributions
DELETE
aca.dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));
 
--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);
 
--Payroll Summer Averages.
DELETE
  aca.dbo.payroll_summer_averages
  where employer_id=@employerID;
 
--Employee Import Alerts.
DELETE
  aca.dbo.import_employee
  WHERE employerID=@employerID
 
--Payroll Import Alerts. Alerts that have been deleted by users.
DELETE aca.dbo.payroll_archive
      WHERE employer_id=@employerID
 
--Payroll Import Alerts.
DELETE
  aca.dbo.import_payroll
  WHERE employerid=@employerID
 
--Insurance Carrier Import Alerts
DELETE
  aca.dbo.import_insurance_coverage
  WHERE employer_id=@employerID;
 
--All Carrier Import Coverage
DELETE
  aca.dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);
 
--All Payroll.
DELETE
  [aca].[dbo].[payroll]
  WHERE employer_id=@employerID
 
--All Employees.
DELETE 
  FROM aca.dbo.employee
  WHERE employer_id=@employerID
 
--All HR Status Codes
DELETE
  aca.dbo.hr_status
  WHERE employer_id=@employerID
 
--All Gross Pay Filters
DELETE
  aca.dbo.gross_pay_filter
  WHERE employer_id=@employerID
 
--All Gross Pay Codes
DELETE
  aca.dbo.gross_pay_type
  WHERE employer_id=@employerID
 
--All Batch rows.
DELETE
  aca.dbo.batch
  WHERE employer_id=@employerID
 
--All Employee Classifications.
DELETE
  aca.dbo.employee_classification
  WHERE employer_id=@employerID
 
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO

CREATE PROCEDURE [dbo].[UPDATE_employeeType]
      -- Add the parameters for the stored procedure here
      @employerId int,
      @name varchar(50)
     
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [aca].[dbo].[employee_type] SET name = @name
      WHERE employer_id = @employerId
     
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[UPDATE_employeeType] TO [aca-user] AS [dbo]
GO
 
CREATE PROCEDURE [dbo].[INSERT_employeeType]
      -- Add the parameters for the stored procedure here
      @employerId int,
      @name varchar(50)
     
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    INSERT INTO [aca].[dbo].[employee_type] (name,employer_id)
      VALUES (@name, @employerId)
     
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[INSERT_employeeType] TO [aca-user] AS [dbo]
GO
 
CREATE PROCEDURE [dbo].[DELETE_employeeType]
      -- Add the parameters for the stored procedure here
      @employeeId int
     
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    DELETE [aca].[dbo].[employee_type]
      WHERE employee_type_id = @employeeId
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[DELETE_employeeType] TO [aca-user] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[UPDATE_BreakInService]    Script Date: 9/6/2016 4:31:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_BreakInService]
      @modifiedBy nvarchar(50),
      @BreakInServiceId int,
      @startDate datetime2(7),
      @endDate datetime2(7),
      @weeks int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [dbo].[BreakInService] SET StartDate = @startDate, EndDate = @endDate, Weeks = @weeks,
      ModifiedBy = @modifiedBy, ModifiedDate = GETDATE()
      WHERE BreakInServiceId = @BreakInServiceId
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[UPDATE_BreakInService] TO [aca-user] AS [dbo]
GO
 
ALTER PROCEDURE [dbo].[UPDATE_BreakInServiceStatus]
      @EntityStatus int,
      @modifiedBy nvarchar(50),
      @BreakInServiceId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
    UPDATE [dbo].[BreakInService] SET EntityStatusId = @EntityStatus, ModifiedBy = @modifiedBy, ModifiedDate = GETDATE()
      WHERE BreakInServiceId = @BreakInServiceId
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[UPDATE_BreakInServiceStatus] TO [aca-user] AS [dbo]
GO
 
ALTER PROCEDURE [dbo].[INSERT_BreakInService]
      @CreatedBy nvarchar(50),
      @CreatedDate datetime2(7),
      @startDate datetime2(7),
      @endDate datetime2(7),
      @measurementId int,
      @week int
     
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
 
      DECLARE @ModifiedBy nvarchar(50) = @CreatedBy;
      DECLARE @ModifiedDate datetime2(7) = @CreatedDate;
 
      INSERT INTO [dbo].[BreakInService] (EntityStatusId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, startDate, endDate, Weeks)
      VALUES (1,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@startDate,@endDate,@week)
      DECLARE @breakInServiceId int;
      SELECT @breakInServiceId = BreakInServiceId FROM [dbo].[BreakInService]
      WHERE (startDate = @startDate AND endDate = @endDate) AND (CreatedBy = @CreatedBy AND CreatedDate = @CreatedDate)
 
      INSERT INTO [dbo].[measurementBreakInService] (EntityStatusId, measurement_id, BreakInServiceId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
      VALUES (1, @measurementId, @breakInServiceId, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate)
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO
 
GRANT EXECUTE ON [dbo].[INSERT_BreakInService] TO [aca-user] AS [dbo]
GO
 
ALTER PROCEDURE [dbo].[SELECT_BreakInService]
  @measurementId int
AS
BEGIN TRY
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;

    SELECT bis.BreakInServiceId, bis.CreatedBy, bis.CreatedDate, bis.StartDate, bis.EndDate, bis.EntityStatusId, bis.ResourceId,
   bis.ModifiedBy, bis.ModifiedDate FROM [dbo].[BreakInService] bis INNER JOIN [dbo].[measurementBreakInService] mbis
    ON bis.BreakInServiceId = mbis.BreakInServiceId
  WHERE mbis.measurement_id = @measurementId AND bis.EntityStatusId = 1;
END TRY
BEGIN CATCH
  EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [dbo].[SELECT_BreakInService] TO [aca-user] AS [dbo]
GO