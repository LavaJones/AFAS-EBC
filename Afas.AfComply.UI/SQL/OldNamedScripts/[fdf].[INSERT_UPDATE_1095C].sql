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
