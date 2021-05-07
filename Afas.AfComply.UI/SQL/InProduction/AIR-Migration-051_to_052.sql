Use air
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
-- Create date: 03/28/2017
-- Description:	updates the output details with the new status codes
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_UPDATE_output_detail_receipt] 
	-- Add the parameters for the stored procedure here
	@headerid int,
	@statusCodeID tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE air.br.output_detail
	SET status_code_id = @statusCodeID
	WHERE header_id = @headerid

END
GO

GRANT EXECUTE ON [dbo].[sp_AIR_UPDATE_output_detail_receipt] TO [air-user] AS [DBO]
GO

