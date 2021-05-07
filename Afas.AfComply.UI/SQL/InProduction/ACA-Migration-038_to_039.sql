USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/21/2016>
-- Description:	<This stored procedure is meant to return all monthly detail records for an employee.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_SELECT_monthly_detail]
	@employeeID int
AS
BEGIN
	SELECT
		[employee_id],
		[time_frame_id],
		[employer_id],
		[monthly_hours],
		[offer_of_coverage_code],
		[mec_offered],
		[share_lowest_cost_monthly_premium],
		[safe_harbor_code],
		[enrolled],
		[monthly_status_id],
		[insurance_type_id],
		[hra_flex_contribution],
		[create_date],
		[modified_date],
		[modified_by]
	FROM
		[air].appr.employee_monthly_detail
	WHERE
		employee_id = @employeeID;
END

GO


