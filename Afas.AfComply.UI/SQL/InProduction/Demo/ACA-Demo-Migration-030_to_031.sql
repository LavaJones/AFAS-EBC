USE [aca-demo]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spUpdateAIR]
	@employerId int,
	@yearId int
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE 
		[air-demo].appr.employee_monthly_detail 
	SET monthly_hours = ad.MonthlyAverageHours, 
		monthly_status_id = CASE
								WHEN ISNULL(ad.MonthlyAverageHours, 0) = 0 THEN 7
								WHEN ad.MonthlyAverageHours > 129.99 THEN 1
								WHEN ad.MonthlyAverageHours < 130 THEN 2
							END
	FROM 
		[air-demo].appr.employee_monthly_detail emd 
			INNER JOIN (
				SELECT
					MonthlyAverageHours, 
					eah.EmployeeId, 
					mea.employer_id
				FROM dbo.EmployeeMeasurementAverageHours eah 
					INNER JOIN measurement mea 
						ON eah.MeasurementId = mea.measurement_id
				WHERE  ((stability_start >= '2016-01-01 00:00:00.000' AND stability_start <= '2016-12-31 00:00:00.000'))) ad 
					ON ad.EmployeeId = emd.employee_id 
						INNER JOIN [air-demo].gen.time_frame tf 
							ON emd.time_frame_id = tf.time_frame_id
	WHERE tf.year_id = @yearId AND emd.employer_id =  @employerId

END