USE aca
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[UpdateAIR]
	-- Add the parameters for the stored procedure here
	@yearId int,
	@employerId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @status int
    -- Insert statements for procedure here
	UPDATE 
		air.appr.employee_monthly_detail 
	SET monthly_hours = ad.MonthlyAverageHours, monthly_status_id = ms.monthly_status_id
	FROM 
		air.appr.employee_monthly_detail emd 
			INNER JOIN (
				SELECT 
					MonthlyAverageHours, 
					employee_id, 
					mea.employer_id
				FROM dbo.EmployeeMeasurementAverageHours eah 
					INNER JOIN measurement mea 
						ON eah.MeasurementId = mea.measurement_id 
							INNER JOIN dbo.plan_year py
								ON mea.plan_year_id = py.plan_year_id 
									INNER JOIN dbo.employee em
										 ON py.plan_year_id = em.plan_year_id
				WHERE  ((stability_start >= '2016-01-01 00:00:00.000' AND stability_start <= '2016-12-31 00:00:00.000') OR (stability_end <='2016-12-31 00:00:00.000' AND stability_end >= '2016-01-01 00:00:00.000' ))) ad 
					ON ad.employee_id = emd.employee_id 
						INNER JOIN air.gen.time_frame tf 
							ON emd.time_frame_id = tf.time_frame_id
								INNER JOIN dbo.employee em 
									ON em.employee_id = emd.employee_id
										INNER JOIN dbo.aca_status ast 
											ON em.aca_status_id = ast.aca_status_id
												INNER JOIN air.emp.monthly_status ms
													ON ms.status_description = ast.name
	WHERE tf.year_id = @yearId AND emd.employer_id =  @employerId



END
