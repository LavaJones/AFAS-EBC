USE [air]
GO
/****** Object:  StoredProcedure [sr].[INSERT_UPDATE_status_request]    Script Date: 3/23/2017 2:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [sr].[INSERT_UPDATE_status_request]
	-- Add the parameters for the stored procedure here
	@status_request_id int OUTPUT,
	@header_id int,
	@receipt_id nvarchar(17),
	@status_code_id int,
	@sr_base_64 varchar(max) = null,
	@return_time_utc nvarchar(100) = null,
	@return_time_local datetime = null,
	@return_time_zone nchar(3) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @new_status_request_id INT;

	SELECT @new_status_request_id = ISNULL(MAX(status_request_id),0) + 1 FROM sr.status_request;
	 
    -- Insert statements for procedure here
	 IF @status_request_id <= 0
        BEGIN
			INSERT INTO sr.status_request(status_request_id,header_id, receipt_id,status_code_id,sr_base_64,return_time_utc,return_time_local,return_time_zone) 
			VALUES (@new_status_request_id,@header_id,@receipt_id,@status_code_id,@sr_base_64,@return_time_utc,@return_time_local,@return_time_zone);
		END
	ELSE
		BEGIN
			UPDATE sr.status_request
			SET receipt_id = @receipt_id,
				status_code_id = @status_code_id,
				sr_base_64 = @sr_base_64,
				return_time_utc = @return_time_utc,
				return_time_local = @return_time_local,
				return_time_zone = @return_time_zone
			WHERE status_request_id = @status_request_id
		END

	 IF @status_request_id <= 0
     BEGIN
        SET @status_request_id = @new_status_request_id;
     END

	SELECT @status_request_id;

END
