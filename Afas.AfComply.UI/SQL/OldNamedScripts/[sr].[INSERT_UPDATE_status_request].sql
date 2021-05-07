USE air
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
-- Author:		Obiye, Kolokolo
-- Create date: 03/21/2017
-- Description:	inserts and updates sr.status_request
-- =============================================
CREATE PROCEDURE [sr].[INSERT_UPDATE_status_request]
	-- Add the parameters for the stored procedure here
	@status_request_id int OUTPUT,
	@header_id int,
	@receipt_id int,
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
GO
