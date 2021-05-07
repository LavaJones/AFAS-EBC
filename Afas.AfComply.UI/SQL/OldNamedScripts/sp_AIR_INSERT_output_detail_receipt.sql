USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_INSERT_output_detail_receipt]    Script Date: 3/17/2017 2:15:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Travis Wells>
-- Create date: <03/16/2017>
-- Description:	<This stored procedure is meant to link a receipt id with the unique transmission id.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_INSERT_output_detail_receipt]
	@headerid int,
	@utid nchar(51),
	@utransmitterid nvarchar(100),
	@tcc nchar(5),
	@shipmentNumber nchar(7),
	@receiptid nchar(17),
	@formtype nvarchar(10),
	@response datetime,
	@statusCodeID tinyint,
	@filename nvarchar(70), 
	@checksum nvarchar(36),
	@attachmentSize int,
	@errorCode nvarchar(9),
	@errorMessage nvarchar(500),
	@xpath nvarchar(100), 
	@responseBase64 varchar(max)
AS

BEGIN
	INSERT INTO air.br.output_detail(
		header_id,
		unique_transmission_id,
		unique_transmitter_id,
		transmitter_control_code,
		shipment_record_number,
		receipt_id,
		form_type,
		response_timestamp,
		status_code_id,
		document_system_file_name,
		[checksum],
		attachment_byte_size,
		error_message_code,
		error_message_text,
		xpath_content,
		response_base_64)
	VALUES(
		@headerid,
		@utid,
		@utransmitterid,
		@tcc,
		@shipmentNumber,
		@receiptid,
		@formtype,
		@response,
		@statusCodeID,
		@filename, 
		@checksum,
		@attachmentSize,
		@errorCode,
		@errorMessage,
		@xpath, 
		@responseBase64 )

END



GO

