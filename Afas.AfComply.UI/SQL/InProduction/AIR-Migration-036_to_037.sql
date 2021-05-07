USE [air]
GO

ALTER TABLE br.manifest ALTER COLUMN vendor_id tinyint NULL
GO

ALTER TABLE br.manifest ADD manifest_file_name nvarchar(300) null
GO

ALTER TABLE br.manifest DROP CONSTRAINT FK_manifest_vendor
GO

ALTER TABLE fdf._1094C ALTER COLUMN vendor_id int NULL
GO 

EXEC sp_RENAME 'fdf._1095C.annual_share_lowest_cost_montyly_premium', 'annual_share_lowest_cost_monthly_premium', 'COLUMN'
GO

ALTER TABLE tr.header DROP CONSTRAINT FK_header_transmitter
GO

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

-- =============================================
-- Author:		Obiye Kolokolo
-- Create date:03/17/2017
-- Description:	inserts and updates _1094C table
-- =============================================
CREATE PROCEDURE [fdf].[INSERT_UPDATE_1094C]
	-- Add the parameters for the stored procedure here
	@_1094C_id int OUTPUT,
	@header_id int = null,
	@unique_transmission_id nchar(51) = null,
	@submission_id int,
	@test_scenario_id nvarchar(10) = null,
	@tax_year smallint,
	@corrected_indicator bit,
	@corrected_usid nchar(52) = null,
	@corrected_tin nchar(10) = null,
	@ein nchar(9),
	@authoritative_transmittal_indicator bit,
	@_1095C_attached_count int,
	@_1095C_total_count smallint = null,
	@fulltime_employee_count int = null,
	@total_employee_count int = null,
	@aag_code tinyint = null,
	@annual_aag_indicator bit = null,
	@qom_indicator bit = null,
	@qom_transition_relief_indicator bit = null,
	@_4980H_transition_relief_indicator bit = null,
	@annual_4980H_transition_relief_code char(1) = null,
	@_98_percent_offer_method bit = null,
	@annual_mec_code tinyint = null,
	@self_insured bit = null,
	@vendor_id int = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 DECLARE @new_1094C_id INT;

	 SELECT @new_1094C_id = ISNULL(MAX(_1094C_id),0) + 1 FROM fdf._1094C;

    -- Insert statements for procedure here
	IF @_1094C_id <= 0
		BEGIN
			INSERT INTO fdf._1094C(
			     _1094C_id
				,header_id
				,unique_transmission_id
				,submission_id
				,test_scenario_id
				,tax_year
				,corrected_indicator
				,corrected_usid
				,corrected_tin
				,ein
				,authoritative_transmittal_indicator
				,_1095C_attached_count
				,_1095C_total_count
				,fulltime_employee_count
				,total_employee_count
				,aag_code
				,annual_aag_indicator
				,qom_indicator
				,qom_transition_relief_indicator
				,_4980H_transition_relief_indicator
				,annual_4980H_transition_relief_code
				,_98_percent_offer_method
				,annual_mec_code
				,self_insured
				,vendor_id
			)
			VALUES(
				@new_1094C_id,
				@header_id,
				@unique_transmission_id,
				@submission_id,
				@test_scenario_id,
				@tax_year,
				@corrected_indicator,
				@corrected_usid,
				@corrected_tin,
				@ein,
				@authoritative_transmittal_indicator,
				@_1095C_attached_count,
				@_1095C_total_count,
				@fulltime_employee_count,
				@total_employee_count,
				@aag_code,
				@annual_aag_indicator,
				@qom_indicator,
				@qom_transition_relief_indicator,
				@_4980H_transition_relief_indicator,
				@annual_4980H_transition_relief_code,
				@_98_percent_offer_method,
				@annual_mec_code,
				@self_insured,
				@vendor_id
			)
		END
	ELSE
		BEGIN
			UPDATE fdf._1094C
			SET header_id = @header_id,
				unique_transmission_id = @unique_transmission_id,
				test_scenario_id = @test_scenario_id,
				corrected_usid = @corrected_usid,
				corrected_tin = @corrected_tin,
				_1095C_total_count = @_1095C_total_count,
				fulltime_employee_count = @fulltime_employee_count,
				total_employee_count = @total_employee_count,
				aag_code = @aag_code,
				annual_aag_indicator = @annual_aag_indicator,
				qom_indicator = @qom_indicator,
				qom_transition_relief_indicator = @qom_transition_relief_indicator,
				_4980H_transition_relief_indicator = @_4980H_transition_relief_indicator,
				annual_4980H_transition_relief_code = @annual_4980H_transition_relief_code,
				_98_percent_offer_method = @_98_percent_offer_method,
				annual_mec_code = @annual_mec_code,
				self_insured = @self_insured,
				vendor_id = @vendor_id
			WHERE 
				_1094C_id = @_1094C_id
		END

	IF @_1094C_id <= 0
    BEGIN
        SET @_1094C_id = @new_1094C_id;
    END

	SELECT @_1094C_id;

END
GO

-- =============================================
-- Author:		Obiye Kolokolo
-- Create date: 03/21/2017
-- Description:	insert and update 1095C
-- =============================================
CREATE PROCEDURE [fdf].[INSERT_UPDATE_1095C]
	-- Add the parameters for the stored procedure here
	@_1095C_id int OUTPUT,
	@_1094C_id int,
	@unique_transmission_id nchar(51) = null,
	@record_id int,
	@corrected_indicator bit,
	@corrected_urid nchar(52) = null,
	@tax_year smallint,
	@employee_id int,
	@annual_offer_of_coverage_code nchar(2) = null,
	@annual_share_lowest_cost_monthly_premium money = null,
	@annual_safe_harbor_code nchar(2) = null,
	@enrolled bit,
	@insurance_type_id tinyint = null,
	@must_supply_ci_info bit,
	@is_1G bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @_1095C_id = 0
		BEGIN
			INSERT INTO fdf._1095C(
				 _1094C_id
				,unique_transmission_id
				,record_id
				,corrected_indicator
				,corrected_urid
				,tax_year
				,employee_id
				,annual_offer_of_coverage_code
				,annual_share_lowest_cost_monthly_premium
				,annual_safe_harbor_code
				,enrolled
				,insurance_type_id
				,must_supply_ci_info
				,is_1G
			)
			VALUES(
				 @_1094C_id
				,@unique_transmission_id
				,@record_id
				,@corrected_indicator
				,@corrected_urid
				,@tax_year
				,@employee_id
				,@annual_offer_of_coverage_code
				,@annual_share_lowest_cost_monthly_premium
				,@annual_safe_harbor_code
				,@enrolled
				,@insurance_type_id
				,@must_supply_ci_info
				,@is_1G
			)
		END
	ELSE
		BEGIN
			UPDATE fdf._1095C
			SET unique_transmission_id = @unique_transmission_id,
				corrected_indicator = @corrected_indicator,
				corrected_urid = @corrected_urid,
				annual_offer_of_coverage_code = @annual_offer_of_coverage_code,
				annual_share_lowest_cost_monthly_premium = @annual_share_lowest_cost_monthly_premium,
				annual_safe_harbor_code = @annual_safe_harbor_code,
				insurance_type_id = @insurance_type_id
			WHERE 
				@_1095C_id = @_1095C_id
		END

	IF @_1095C_id <= 0
    BEGIN
        SET @_1095C_id = SCOPE_IDENTITY();
    END

	SELECT @_1095C_id;

END
GO

-- =============================================
-- Author:		Obiye,Kolokolo
-- Create date: 03/21/2017
-- Description:	insert and update request error
-- =============================================
CREATE PROCEDURE [sr].[INSERT_UPDATE_request_error]
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
			VALUES (@error_id,@error_type,@status_request_id,@transmitter_error_id,@error_message_code,@error_message_text,@x_path_content);
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

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/15/2017>
-- Description:	<This stored procedure is meant to return all AIR submission status possibilities.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_submission_statuses]
AS

BEGIN
	SELECT
		*
	FROM
		air.sr.status_code
END
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/17/2017>
-- Description:	<This stored procedure is meant to return all status possibilities for the br schema.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_receipt_statuses]
AS

BEGIN
	SELECT
		*
	FROM
		air.br.status_code
END
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/27/2016>
-- Description:	<This stored procedure is meant to return all submissions to the IRS by EIN and Year.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_submissions]
	@ein nchar(9),
	@year nchar(4)
AS

BEGIN
	SELECT * FROM air.br.output_detail
	WHERE air.br.output_detail.unique_transmission_id IN 
	(Select air.br.manifest.unique_transmission_id FROM air.br.manifest WHERE ein=@ein and payment_year=@year)
END
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/29/2016>
-- Description:	<This stored procedure is meant to return all status request rows for a specific receipt id.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_status_request]
	@receiptID varchar(50)
AS

BEGIN
	SELECT * FROM air.sr.status_request
	WHERE air.sr.status_request.receipt_id=@receiptID;
END
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/29/2016>
-- Description:	<This stored procedure is meant to return all errors pertaining to a specific Status Request ID.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_status_error]
	@srID int
AS

BEGIN
	SELECT * FROM air.sr.request_error
	WHERE air.sr.request_error.status_request_id=@srID;
END
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/15/2017>
-- Description:	<This stored procedure is meant to return all employer manifest rows by by EIN and Year.>
-- Changes:
--			If the unique_transmission_id already exists in the output_detail table than the receipt has already been 
--	attached to is to there is no need to show those ones. 
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_manifest]
	@ein nchar(9),
	@year nchar(4)
AS

BEGIN
	Select * FROM air.br.manifest WHERE ein=@ein and payment_year=@year
	AND air.br.manifest.unique_transmission_id NOT IN 
	(SELECT air.br.output_detail.unique_transmission_id FROM air.br.output_detail);

END
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/27/2016>
-- Description:	<This stored procedure is meant to return all submissions to the IRS by EIN and Year.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employeeID_status_error]
	@utID varchar(100), 
	@recordID int
AS

BEGIN
	SELECT air.fdf._1095C.employee_id FROM air.fdf._1095C
	WHERE unique_transmission_id=@utID AND record_id=@recordID;
END
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

