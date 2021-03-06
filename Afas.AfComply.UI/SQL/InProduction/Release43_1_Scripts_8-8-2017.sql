USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[BULK_UPSERT_AverageHours]    Script Date: 8/1/2017 3:22:26 PM ******/
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

--       UPDATE [dbo].[EmployeeMeasurementAverageHours] SET [EntityStatusId] = 2 WHERE MeasurementId in 
--       (SELECT DISTINCT MeasurementId MeasurementId FROM @averages);

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
