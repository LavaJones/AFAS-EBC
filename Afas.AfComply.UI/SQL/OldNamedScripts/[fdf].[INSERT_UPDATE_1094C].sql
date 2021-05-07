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
