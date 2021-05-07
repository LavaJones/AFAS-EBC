USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_SELECT_employer_manifest]    Script Date: 3/17/2017 2:14:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

