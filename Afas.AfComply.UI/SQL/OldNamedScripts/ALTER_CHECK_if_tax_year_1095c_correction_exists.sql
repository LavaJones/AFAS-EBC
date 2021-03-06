USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[CHECK_if_tax_year_1095c_correction_exists]    Script Date: 4/25/2017 10:37:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 04/18/2017
-- Description:	check if a tax year 1095c correction already exists
-- =============================================
ALTER PROCEDURE [dbo].[CHECK_if_tax_year_1095c_correction_exists]
	-- Add the parameters for the stored procedure here
	@employee_id int,
	@tax_year int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 *
	FROM tax_year_1095c_correction
	WHERE employee_id = @employee_id
	AND tax_year = @tax_year
	AND Corrected = 1
	AND EntityStatusId = 1

END
