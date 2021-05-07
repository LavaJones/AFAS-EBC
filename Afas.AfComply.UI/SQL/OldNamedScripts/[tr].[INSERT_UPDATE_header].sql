USE air
GO
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
-- Author:		Obiye Kolokolo
-- Create date: 03/17/2017
-- Description:	inserts and updates header table
-- =============================================
CREATE PROCEDURE [tr].[INSERT_UPDATE_header] 
	-- Add the parameters for the stored procedure here
	@header_id int OUTPUT,
	@transmitter_control_code nchar(5),
	@unique_transmission_id nchar(51),
	@transmission_timestamp datetime2(4),
	@1094c_id int = null,
	@transmitted_base_64 varchar(max) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 DECLARE @new_header_id INT;

	 SELECT @new_header_id = ISNULL(MAX(header_id),0) + 1 FROM tr.header;
	 
    -- Insert statements for procedure here
	 IF @header_id <= 0
        BEGIN
			INSERT INTO tr.header(header_id,universally_unique_id, transmitter_control_code,unique_transmission_id,message_type_id,transmission_timestamp,_1094c_id,transmitted_base_64) 
			VALUES (@new_header_id, NEWID(),@transmitter_control_code,@unique_transmission_id,1,@transmission_timestamp,@1094c_id,@transmitted_base_64);
		END
	ELSE
		BEGIN
			UPDATE tr.header
			SET transmission_timestamp = @transmission_timestamp,
				_1094c_id = @1094c_id,
				transmitted_base_64 = @transmitted_base_64
			WHERE header_id = @header_id
		END

	 IF @header_id <= 0
     BEGIN
        SET @header_id = @new_header_id;
     END

	SELECT @header_id;

END
GO