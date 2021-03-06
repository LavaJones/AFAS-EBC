USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employee_offer_and_coverage]    Script Date: 2/23/2017 9:53:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employee_offer_and_coverage]
	-- Add the parameters for the stored procedure here
	@employeeID int,
	@taxYear int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		a.employee_id,
		a.time_frame_id,
		a.employer_id, 
		a.offer_of_coverage_code, 
		a.monthly_status_id, 
		a.monthly_hours,
		m.name,
		f.year_id,
		a.safe_harbor_code, 
		a.mec_offered, 
		a.insurance_type_id
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								INNER JOIN air.gen.time_frame f ON f.time_frame_id = b.time_frame_id
								INNER JOIN air.appr.employee_yearly_detail e ON a.employee_id = e.employee_id
	WHERE a.employee_id = @employeeID AND b.year_id = @taxYear AND e._1095C = 1
	ORDER By m.month_id

END
