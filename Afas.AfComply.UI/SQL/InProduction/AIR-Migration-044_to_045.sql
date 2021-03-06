USE [air]
GO
/****** Object:  StoredProcedure [br].[INSERT_UPDATE_manifest]    Script Date: 3/24/2017 3:58:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [br].[INSERT_UPDATE_manifest] 
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
	@vendor_id tinyint = null,
	@payee_count smallint,
	@payer_record_count int,
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
