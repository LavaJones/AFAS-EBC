USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_SELECT_employer_submissions]    Script Date: 3/17/2017 2:13:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

