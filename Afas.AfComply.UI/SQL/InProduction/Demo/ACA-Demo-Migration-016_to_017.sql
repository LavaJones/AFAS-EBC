USE [aca-demo]
GO

--clean out the Tables/Procs if they exist
--Drop TABLE [dbo].[EmployeeMeasurementAverageHours];
--GO

-----------------------------------------------------

--Drop PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours];
--GO
--Drop PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ForEmployee];
--GO
--Drop PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ForMeasurement];
--GO
--Drop PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ByEmployeeMeasurement];
--GO
--Drop PROCEDURE [dbo].[UPSERT_AverageHours];
--GO
--Drop PROCEDURE [dbo].[BULK_UPSERT_AverageHours];
--GO
--Drop PROCEDURE [dbo].[Update_EmployeeMeasurementAverageHours_EntityStatus];
--GO
--Drop PROCEDURE [dbo].[SELECT_All_BreakInService_ForEmployer];
--GO
--Drop PROCEDURE [BULK_UPDATE_employee_AVG_MONTHLY_HOURS];
--GO
--Drop PROCEDURE [SELECT_employer_payroll];
--GO
--Drop type Bulk_AverageHours;
--GO
--Drop type Bulk_Employee_AverageHours;
--GO

-----------------------------------------------------
-- Create Tables
-----------------------------------------------------

GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EmployeeMeasurementAverageHours]
(
	[EmployeeMeasurementAverageHoursId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[MeasurementId] [int] NOT NULL,
	[WeeklyAverageHours] [numeric](18, 4) NULL,	
	[MonthlyAverageHours] [numeric](18, 4) NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL 
	CONSTRAINT [DF_EmployeeMeasurementAverageHours_resourceId] DEFAULT (newid()),
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,

	CONSTRAINT [PK_EmployeeMeasurementAverageHours] PRIMARY KEY NONCLUSTERED 
	(
	[EmployeeMeasurementAverageHoursId] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
	ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMeasurementAverageHours_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMeasurementAverageHours_Employee] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[employee] ([employee_id])
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMeasurementAverageHours_Measurement] FOREIGN KEY([MeasurementId])
REFERENCES [dbo].[measurement] ([measurement_id])
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] CHECK CONSTRAINT [FK_EmployeeMeasurementAverageHours_EntityStatus]
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] CHECK CONSTRAINT [FK_EmployeeMeasurementAverageHours_Employee]
GO

ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] CHECK CONSTRAINT [FK_EmployeeMeasurementAverageHours_Measurement]
GO


SET ANSI_PADDING OFF
GO


-------------------------------------------------------------
--Create Stored Procs
-------------------------------------------------------------

-- SELECT

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Select a Single Row of 
-- =============================================
Create PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours]
      @Id int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeMeasurementAverageHoursId] = @Id;
END
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Select a Single Row of 
-- =============================================
Create PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ByEmployeeMeasurement]
      @employeeId int,
	  @measurementId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeId] = @employeeId 
		AND [MeasurementId] = @measurementId
	    AND [EntityStatusId] = 1;
END
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Select all Rows of AverageHours for an Employee
-- =============================================
Create PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ForEmployee]
      @employeeId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeId] = @employeeId AND [EntityStatusId] = 1;
END
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Select all Rows of AverageHours for a Measurement Period
-- =============================================
Create PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ForMeasurement]
      @measurementId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT * FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [MeasurementId] = @measurementId AND [EntityStatusId] = 1;
END
GO


--INSERT / UPDATE 


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan, Mccully
-- Create date: 10/6/2016
-- Description:	Upsert: update or insert AverageHours into the table
-- =============================================
Create PROCEDURE [dbo].[UPSERT_AverageHours]
	@employeeId int,
	@measurementId int,
	@weeklyAverageHours numeric(18, 4),
	@monthlyAverageHours numeric(18, 4),
	@CreatedBy nvarchar(50),
	@insertedID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	MERGE [dbo].[EmployeeMeasurementAverageHours]  AS T  
	USING (
			SELECT @employeeId employeeId,
			@measurementId measurementId,
			@weeklyAverageHours weeklyAverageHours, 
			@monthlyAverageHours monthlyAverageHours, 
			@CreatedBy CreatedBy
		) AS S 
	ON T.EmployeeId = S.employeeId AND T.MeasurementId = S.measurementId
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[WeeklyAverageHours] = S.weeklyAverageHours,
		T.[MonthlyAverageHours] = S.monthlyAverageHours,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE(),
		@CreatedBy = T.[EmployeeMeasurementAverageHoursId]
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[EmployeeId], [MeasurementId], [WeeklyAverageHours], [MonthlyAverageHours],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employeeId, S.measurementId, S.weeklyAverageHours, S.monthlyAverageHours, 
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );

	SELECT @insertedID = SCOPE_IDENTITY();

END
GO

GO
create type Bulk_AverageHours as table
(
	EmployeeMeasurementAverageHoursId int,
	EmployeeId int,
	MeasurementId int,
	WeeklyAverageHours numeric(18, 4),
	MonthlyAverageHours numeric(18, 4)
);	

GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/7/2016
-- Description:	Bulk Insert or Update Calculation Averages
-- =============================================
CREATE PROCEDURE [dbo].[BULK_UPSERT_AverageHours]	
	@averages Bulk_AverageHours readonly,
	@CreatedBy nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	MERGE [dbo].[EmployeeMeasurementAverageHours]  AS T  
	USING (
			SELECT EmployeeId employeeId,
			MeasurementId measurementId,
			WeeklyAverageHours weeklyAverageHours, 
			MonthlyAverageHours monthlyAverageHours, 
			@CreatedBy CreatedBy From @averages
		) AS S 
	ON T.EmployeeId = S.employeeId AND T.MeasurementId = S.measurementId
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[WeeklyAverageHours] = S.weeklyAverageHours,
		T.[MonthlyAverageHours] = S.monthlyAverageHours,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE(),
		@CreatedBy = T.[EmployeeMeasurementAverageHoursId]
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[EmployeeId], [MeasurementId], [WeeklyAverageHours], [MonthlyAverageHours],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employeeId, S.measurementId, S.weeklyAverageHours, S.monthlyAverageHours, 
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );
END
GO


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 10/6/2016
-- Description: Set the entity Status
-- =============================================
Create PROCEDURE [dbo].[Update_EmployeeMeasurementAverageHours_EntityStatus]
	@employeeId int,
	@measurementId int,
	@modifiedBy nvarchar(50),
	@EntityStatus int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    UPDATE [dbo].[EmployeeMeasurementAverageHours] 
	SET EntityStatusId = @EntityStatus
	WHERE [MeasurementId] = @measurementId AND [EmployeeId]= @employeeId;
END
GO





-----------------------------------------------------------------------
-- Exising Tables
-----------------------------------------------------------------------

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        <Author,,Name>
-- Create date: <Create Date,,>
-- Description:   <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_All_BreakInService_ForEmployer]
      @employerId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT bis.BreakInServiceId, bis.CreatedBy, bis.CreatedDate, bis.StartDate, bis.EndDate, 
		   bis.EntityStatusId, bis.ResourceId, bis.ModifiedBy, bis.ModifiedDate, mbis.measurement_id  
	  FROM 
	  [dbo].[BreakInService] bis 
	  INNER JOIN [dbo].[measurementBreakInService] mbis ON bis.BreakInServiceId = mbis.BreakInServiceId 
	  INNER JOIN [dbo].[Measurement] meas ON mbis.measurement_id = meas.measurement_id
      WHERE meas.employer_id = @employerId AND bis.EntityStatusId = 1;
END
GO

GO
create type Bulk_Employee_AverageHours as table
(
	employee_id int,
	pyAvg numeric(18,4),
	lpyAvg numeric(18,4),
	mpyAvg numeric(18,4), 
	impAvg numeric(18,4)
);	

GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/7/2016
-- Description:	Bulk Update Data to speed up uploading based on <Travis Wells> UPDATE_employee_AVG_MONTHLY_HOURS
-- =============================================
CREATE PROCEDURE [dbo].[BULK_UPDATE_employee_AVG_MONTHLY_HOURS]
	@employeeHours Bulk_Employee_AverageHours readonly
AS
BEGIN
	UPDATE employee
	SET
		plan_year_avg_hours=a.pyAvg,
		limbo_plan_year_avg_hours=a.lpyAvg,
		meas_plan_year_avg_hours=a.mpyAvg,
		imp_plan_year_avg_hours=a.impAvg
	FROM @employeeHours a
	JOIN employee b ON b.employee_id=a.employee_id;
END
GO



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/10/2016
-- Description:	Select all payroll for an employer based on <Travis Wells> [SELECT_employee_payroll]
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_payroll]
	@employerId int
AS
BEGIN
	SELECT * FROM View_payroll
	WHERE employer_id=@employerId 
	ORDER BY employee_id, edate;
END
GO


-- fix nuking ----------------------------------

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[RESET_EMPLOYER]
      @employerID int
AS
BEGIN TRY

--Plan Year Archives/Insurance Offers
DELETE
  dbo.employee_insurance_offer
  where employer_id=@employerID;

--Plan Year Archives/Insurance Offers
DELETE
  dbo.employee_insurance_offer_archive
  where employer_id=@employerID;

--All Carrier Import Coverage
DELETE
  dbo.insurance_coverage
  WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

--
DELETE
	dbo.insurance_coverage_editable
	where employer_id=@employerID;

--Insurance Carrier Import Alerts
DELETE
  dbo.import_insurance_coverage
  WHERE employer_id=@employerID;

--All Measurement Periods. 
CREATE TABLE #entityValue (value integer)

INSERT INTO #entityValue (value)
SELECT BreakInServiceId
FROM dbo.measurementBreakInService
WHERE measurement_id IN (SELECT measurement_id FROM [dbo].[measurement] WHERE employer_id = @employerId)

--Measurement Break In Service
DELETE dbo.measurementBreakInService WHERE measurement_id IN (SELECT measurement_id FROM [dbo].[measurement] WHERE employer_id = @employerId)

--Break in Service
UPDATE dbo.BreakInService SET EntityStatusId = 3 WHERE BreakInServiceId IN (SELECT Value FROM #entityValue)

DROP TABLE #entityValue

--Insurance Contributions
DELETE
dbo.insurance_contribution
WHERE insurance_id IN (Select insurance_id FROM insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID));

-- Tax Year
DELETE
	dbo.tax_year_1095c_approval
	where employer_id=@employerID;

--Payroll Summer Averages. 
DELETE
  dbo.payroll_summer_averages
  where employer_id=@employerID;

--All Payroll. 
DELETE 
  dbo.payroll
  WHERE employer_id=@employerID
  
--Payroll Import Alerts. Alerts that have been deleted by users. 
DELETE dbo.payroll_archive
      WHERE employer_id=@employerID

-- dependents
DELETE 
      dbo.employee_dependents
      WHERE employee_id IN (Select employee_id FROM employee WHERE employer_id=@employerID);

-- must clear out average hours before clearing measurement period 
DELETE dbo.EmployeeMeasurementAverageHours where MeasurementId in (Select measurement_id from dbo.measurement WHERE employer_id = @employerId);

--Measurement 
DELETE dbo.measurement WHERE employer_id = @employerId;

--Insurance Plans
DELETE
   insurance WHERE plan_year_id IN (Select plan_year_id FROM plan_year WHERE employer_id=@employerID);

--All Employees.
DELETE  
  FROM dbo.employee
  WHERE employer_id=@employerID
  
--All Gross Pay Filters
DELETE
  dbo.gross_pay_filter
  WHERE employer_id=@employerID

--Employee Alert Archives. Alerts that have been deleted by users. 
DELETE dbo.alert_archive
      WHERE employer_id=@employerID

--All Gross Pay Codes
DELETE
  dbo.gross_pay_type
  WHERE employer_id=@employerID

--All Plan Years. 
DELETE
  dbo.plan_year
  WHERE employer_id=@employerID

  -- tax year approval
DELETE
  dbo.tax_year_approval
  WHERE employer_id=@employerID

--All Batch rows. 
DELETE
  dbo.batch
  WHERE employer_id=@employerID

  -- Theses are not referenced at all in the spreadsheet
  
--Equivalencies
DELETE 
  dbo.equivalency
  where employer_id=@employerID;

--Employee Import Alerts. 
DELETE
  dbo.import_employee
  WHERE employerID=@employerID
  
--Payroll Import Alerts. 
DELETE
  dbo.import_payroll
  WHERE employerid=@employerID

--All HR Status Codes
DELETE
  dbo.hr_status
  WHERE employer_id=@employerID

END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH



-----------------------------



GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeMeasurementAverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeMeasurementAverageHours_ForEmployee] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeMeasurementAverageHours_ForMeasurement] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeMeasurementAverageHours_ByEmployeeMeasurement] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[UPSERT_AverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_UPSERT_AverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Update_EmployeeMeasurementAverageHours_EntityStatus] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_All_BreakInService_ForEmployer] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[BULK_UPDATE_employee_AVG_MONTHLY_HOURS] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[SELECT_employer_payroll] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_AverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_Employee_AverageHours] TO [aca-user] AS [dbo]
GO
