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
-- Author:		Obiye Kolokolo
-- Create date: 03/17/2017
-- Description:	Inserts and updates manifest table
-- =============================================
CREATE PROCEDURE [br].[INSERT_UPDATE_manifest] 
	-- Add the parameters for the stored procedure here
	@header_id int OUTPUT,
	@unique_transmission_id nchar(51),
	@payment_year nchar(4),
	@prior_year_indicator bit,
	@ein nchar(9),
	@br_type_code nchar(1),
	@test_file_indicator char(1),
	@original_unique_transmission_id nchar(51) = null,
	@transmitter_foreign_entity_indicator bit,
	@original_receipt_id nchar(17) = null,
	@vendor_indicator nchar(1),
	@vendor_id int = null,
	@payee_count tinyint,
	@payer_record_count tinyint,
	@software_id nvarchar(15),
	@form_type nchar(10),
	@binary_format nchar(15),
	@checksum nvarchar(36),
	@attachment_byte_size int,
	@document_system_file_name nvarchar(300),
	@mtom varchar(max) = null,
	@manifest_file_name nvarchar(300)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @rowCount INT;
	SELECT @rowCount = COUNT(*) FROM air.br.manifest WHERE header_id = @header_id

    -- Insert statements for procedure here
	 IF @rowCount <= 0
        BEGIN
			INSERT br.manifest(
				 header_id
				,unique_transmission_id
				,payment_year
				,prior_year_indicator
				,ein
				,br_type_code
				,test_file_indicator
				,transmitter_foreign_entity_indicator
				,vendor_indicator
				,vendor_id
				,payee_count
				,payer_record_count
				,software_id
				,form_type
				,binary_format
				,[checksum]
				,attachment_byte_size
				,document_system_file_name
				,manifest_file_name)
			VALUES
				(
				 @header_id
				,@unique_transmission_id
				,@payment_year
				,@prior_year_indicator
				,@ein
				,@br_type_code
				,@test_file_indicator
				,@transmitter_foreign_entity_indicator
				,@vendor_indicator
				,@vendor_id
				,@payee_count
				,@payer_record_count
				,@software_id
				,@form_type
				,@binary_format
				,@checksum
				,@attachment_byte_size
				,@document_system_file_name
				,@manifest_file_name)
		END
	 ELSE
		BEGIN
			UPDATE br.manifest
			SET original_unique_transmission_id = @original_unique_transmission_id
				,original_receipt_id = @original_receipt_id
				,mtom = @mtom
			WHERE header_id = @header_id
		END

	 SELECT @header_id;

END
GO
