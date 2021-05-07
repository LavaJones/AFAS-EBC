USE [aca]
GO

-------------------------------------------------------------------------------------------------------------------------
-- Drop for testing
-------------------------------------------------------------------------------------------------------------------------
GO
--DROP PROCEDURE [dbo].[UPDATE_ROLLBACK_employee_plan_year_meas];
GO

GO
--alter table [dbo].[EmployeeMeasurementAverageHours] alter column [MeasurementId] int null;
alter table [dbo].[EmployeeMeasurementAverageHours] add [IsNewHire] bit not null DEFAULT 0;
GO
-------------------------------------------------------------------------------------------------------------------------
-- New stored Procs
-------------------------------------------------------------------------------------------------------------------------
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan, McCully
-- Create date: 10/18/2016
-- Description:	This stored procedure is meant to update the measurement_plan_year_id and limbo_plan_year_id in reverse of the normal rollover period.
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_ROLLBACK_employee_plan_year_meas]
	@employerID int,
	@employeeTypeID int,
	@CurrPlanYearID int,
	@RollbackToPlanYearID int,
	@modOn datetime,
	@modBy varchar(50)
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY
	
		/***************************************************************************
		Step 1: Get the Current Measurement Period Start Date.
		***************************************************************************/
		DECLARE @measStart datetime;
		SELECT @measStart=meas_start FROM measurement WHERE plan_year_id=@CurrPlanYearID;
		

		/******************************************************************************************************************
		Step 2: UPDATE the Plan Year Columns 
		******************************************************************************************************************/
		UPDATE employee
		SET
			meas_plan_year_id=@RollbackToPlanYearID,
			limbo_plan_year_id=null,
			plan_year_id=null,
			modOn=@modOn,
			modBy=@modBy
		WHERE
			employer_id=@employerID AND
			employee_type_id=@employeeTypeID AND
			meas_plan_year_id=@CurrPlanYearID AND 
			hireDate < @measStart;
		
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		EXEC dbo.INSERT_ErrorLogging
	END CATCH
END
GO




GO
GRANT EXECUTE ON [dbo].[UPDATE_ROLLBACK_employee_plan_year_meas] TO [aca-user] AS [dbo]
GO




------------------------------------------------------------------------
-- Modify Previous Stored Procs and tables
------------------------------------------------------------------------
GO 
ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] ADD [TrendingWeeklyAverageHours] [numeric](18, 4) NULL
GO
ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] ADD [TrendingMonthlyAverageHours] [numeric](18, 4) NULL
GO
ALTER TABLE [dbo].[EmployeeMeasurementAverageHours] ADD [TotalHours] [numeric](18, 4) NULL
GO

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
ALTER PROCEDURE [dbo].[UPSERT_AverageHours]
	@employeeId int,
	@measurementId int,
	@weeklyAverageHours numeric(18, 4),
	@monthlyAverageHours numeric(18, 4),
	@trendingWeeklyAverageHours numeric(18, 4),
	@trendingMonthlyAverageHours numeric(18, 4),
	@totalHours numeric(18, 4),
	@isNewHire bit,
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
			@trendingWeeklyAverageHours trendingWeeklyAverageHours,
			@trendingMonthlyAverageHours trendingMonthlyAverageHours,
			@totalHours totalHours,
			@isNewHire isNewHire,
			@CreatedBy CreatedBy
		) AS S 
	ON T.EmployeeId = S.employeeId AND T.MeasurementId = S.measurementId AND T.IsNewHire = S.isNewHire
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[WeeklyAverageHours] = S.weeklyAverageHours,
		T.[MonthlyAverageHours] = S.monthlyAverageHours,
		T.[TrendingWeeklyAverageHours] = S.trendingWeeklyAverageHours,
		T.[TrendingMonthlyAverageHours] = S.trendingMonthlyAverageHours,
		T.[TotalHours] = S.totalHours,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE(),
		@CreatedBy = T.[EmployeeMeasurementAverageHoursId]
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[EmployeeId], [MeasurementId], [WeeklyAverageHours], [MonthlyAverageHours],
		[TrendingWeeklyAverageHours], [TrendingMonthlyAverageHours], [TotalHours], [IsNewHire],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employeeId, S.measurementId, S.weeklyAverageHours, S.monthlyAverageHours, 
		  S.trendingWeeklyAverageHours, S.trendingMonthlyAverageHours, S.totalHours, S.isNewHire,
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );

	SELECT @insertedID = SCOPE_IDENTITY();

END
GO

GO
drop PROCEDURE [dbo].[BULK_UPSERT_AverageHours];
GO
Drop type Bulk_AverageHours;
GO
CREATE type Bulk_AverageHours as table
(
	EmployeeMeasurementAverageHoursId int,
	EmployeeId int,
	MeasurementId int,
	WeeklyAverageHours numeric(18, 4),
	MonthlyAverageHours numeric(18, 4),
	TrendingWeeklyAverageHours numeric(18, 4),
	TrendingMonthlyAverageHours numeric(18, 4),
	TotalHours numeric(18, 4),
	IsNewHire bit
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
			TrendingWeeklyAverageHours trendingWeeklyAverageHours, 
			TrendingMonthlyAverageHours trendingMonthlyAverageHours, 
			TotalHours totalHours,
			IsNewHire isNewHire,
			@CreatedBy CreatedBy From @averages
		) AS S 
	ON T.EmployeeId = S.employeeId AND T.MeasurementId = S.measurementId AND T.IsNewHire = S.isNewHire
	WHEN MATCHED THEN  
	  UPDATE SET 
		T.[WeeklyAverageHours] = S.weeklyAverageHours,
		T.[MonthlyAverageHours] = S.monthlyAverageHours,		
		T.[TrendingWeeklyAverageHours] = S.trendingWeeklyAverageHours,
		T.[TrendingMonthlyAverageHours] = S.trendingMonthlyAverageHours,
		T.[TotalHours] = S.totalHours,
		T.[IsNewHire] = S.isNewHire,
		T.[ModifiedBy] = S.CreatedBy,
		T.[ModifiedDate] = GETDATE(),
		@CreatedBy = T.[EmployeeMeasurementAverageHoursId]
	WHEN NOT MATCHED THEN  
	  INSERT 
	  (	
		[EmployeeId], [MeasurementId], [WeeklyAverageHours], [MonthlyAverageHours],
		[TrendingWeeklyAverageHours], [TrendingMonthlyAverageHours], [TotalHours], [IsNewHire],
		[EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]
	  ) 
	  VALUES 
	  (
		  S.employeeId, S.measurementId, S.weeklyAverageHours, S.monthlyAverageHours, 
		  S.trendingWeeklyAverageHours, S.trendingMonthlyAverageHours, S.totalHours, S.isNewHire,
		  1, @CreatedBy, GETDATE(), @CreatedBy, GETDATE()
	  );
END
GO

GO
GRANT EXECUTE ON [dbo].[BULK_UPSERT_AverageHours] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON TYPE::[dbo].[Bulk_AverageHours] TO [aca-user] AS [dbo]
GO