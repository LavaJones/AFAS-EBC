USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[BULK_UPSERT_AverageHours]    Script Date: 1/4/2017 2:40:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:          Ryan McCully 
-- Create date: 10/7/2016
-- Description:     Bulk Insert or Update Calculation Averages
-- =============================================
ALTER PROCEDURE [dbo].[BULK_UPSERT_AverageHours]     
       @averages Bulk_AverageHours readonly,
       @CreatedBy nvarchar(50)
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

       UPDATE [dbo].[EmployeeMeasurementAverageHours] SET [EntityStatusId] = 2 WHERE MeasurementId in 
       (SELECT DISTINCT MeasurementId MeasurementId FROM @averages);

       MERGE [dbo].[EmployeeMeasurementAverageHours] AS T  
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
             T.[EntityStatusId] = 1,
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
ALTER TABLE dbo.employee_insurance_offer_archive ADD OfferResourceId uniqueidentifier not null default '00000000-0000-0000-0000-000000000000';
GO
ALTER PROCEDURE [dbo].[TRANSFER_insurance_change_event]
      @rowID int,
      @insuranceID int,
      @contributionID int,
      @avgHours decimal,
      @offered bit, 
      @offeredOn datetime,
      @accepted bit,
      @acceptedOn datetime,
      @modOn datetime,
      @modBy varchar(50),
      @notes varchar(max),
      @history varchar(max),
      @effDate datetime,
      @hraFlex decimal
AS

BEGIN

      /*************************************************************
      ******* Create a transaction that must fully complete ********
      **************************************************************/
      BEGIN TRANSACTION
            BEGIN TRY
                  -- Step 1: Archive the current insurance offer.
                  --                - Return the new Employee_ID
      INSERT INTO dbo.employee_insurance_offer_archive ([employee_id]
      ,[plan_year_id]
      ,[employer_id]
      ,[insurance_id]
      ,[ins_cont_id]
      ,[avg_hours_month]
      ,[offered]
      ,[offeredOn]
      ,[accepted]
      ,[acceptedOn]
      ,[modOn]
      ,[modBy]
      ,[notes]
      ,[history]
      ,[effectiveDate]
      ,[hra_flex_contribution]
        ,OfferResourceId)
      SELECT [employee_id]
      ,[plan_year_id]
      ,[employer_id]
      ,[insurance_id]
      ,[ins_cont_id]
      ,[avg_hours_month]
      ,[offered]
      ,[offeredOn]
      ,[accepted]
      ,[acceptedOn]
      ,[modOn]
      ,[modBy]
      ,[notes]
      ,[history]
      ,[effectiveDate]
      ,[hra_flex_contribution]
        ,[ResourceId]
        FROM dbo.employee_insurance_offer
        WHERE rowid=@rowID;

                  -- Step 2: Update the new insurance offer.
                  EXEC UPDATE_insurance_offer
                        @rowID,
                        @insuranceID,
                        @contributionID,
                        @avgHours,
                        @offered, 
                        @offeredOn,
                        @accepted,
                        @acceptedOn,
                        @modOn,
                        @modBy,
                        @notes,
                        @history,
                        @effDate,
                        @hraFlex;
      
                  COMMIT
            END TRY
            BEGIN CATCH
                  exec dbo.INSERT_ErrorLogging
                  ROLLBACK TRANSACTION
            END CATCH

END

