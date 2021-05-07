USE [aca]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_AIR_UPDATE_approved_monthly_detail]
@employeeID int,
@timeFrameID int,
@employerID int,
@hours decimal(18,2),
@ooc nchar(2),
@mec bit, 
@lcmp decimal(18,2),
@ash nchar(2),
@enrolled bit,
@monthlyStatusID int,
@insuranceTypeID int,
@modBy varchar(50),
@modOn datetime
AS

BEGIN TRANSACTION

BEGIN TRY

-- todo add in the while loop for the 1205 errors. gc5

UPDATE [air].[appr].employee_monthly_detail
SET
offer_of_coverage_code = @ooc,
mec_offered = @mec,
share_lowest_cost_monthly_premium = @lcmp,
safe_harbor_code = @ash,
enrolled = @enrolled,
monthly_status_id = @monthlyStatusID,
insurance_type_id = @insuranceTypeID,
modified_by = @modBy,
modified_date = @modOn
WHERE
employee_id = @employeeID
AND
time_frame_id = @timeFrameID

-- removed the ETL_Build call that used to be here. Use the new Missing Employees flow. gc5

COMMIT

END TRY
BEGIN CATCH

ROLLBACK

EXEC dbo.INSERT_ErrorLogging

END CATCH

GO
