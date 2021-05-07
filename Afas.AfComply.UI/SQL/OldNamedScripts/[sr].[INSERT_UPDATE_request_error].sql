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
-- Author:		Obiye,Kolokolo
-- Create date: 03/21/2017
-- Description:	insert and update request error
-- =============================================
ALTER PROCEDURE [sr].[INSERT_UPDATE_request_error]
	-- Add the parameters for the stored procedure here
	@error_id int OUTPUT,
	@error_type char(1),
	@status_request_id int,
	@transmitter_error_id int,
	@error_message_code nvarchar(9),
	@error_message_text nvarchar(500),
	@x_path_content nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @new_error_id INT;

	SELECT @new_error_id = ISNULL(MAX(error_id),0) + 1 FROM sr.request_error;

    -- Insert statements for procedure here
	 IF @error_id <= 0
        BEGIN
			INSERT INTO sr.request_error(error_id, error_type,status_request_id,transmitter_error_id,error_message_code,error_message_text,x_path_content) 
			VALUES (@new_error_id,@error_type,@status_request_id,@transmitter_error_id,@error_message_code,@error_message_text,@x_path_content);
		END
	 ELSE
		BEGIN
			UPDATE sr.request_error
			SET status_request_id = @status_request_id,
				transmitter_error_id = @transmitter_error_id,
				error_message_code = @error_message_code,
				error_message_text = @error_message_text,
				x_path_content = @x_path_content
			WHERE error_id = @error_id
		END

	 IF @error_id <= 0
     BEGIN
        SET @error_id = @new_error_id;
     END

	SELECT @error_id;

END
GO
