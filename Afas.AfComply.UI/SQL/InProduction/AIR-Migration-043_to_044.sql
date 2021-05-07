USE [air]
GO

/****** Object:  StoredProcedure [dbo].[sp_AIR_SELECT_employer_submissions_NOT_Acknowledged]    Script Date: 3/21/2017 5:05:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Travis Wells>
-- Create date: <03/20/2017>
-- Description:	<This stored procedure is meant to return all submissions that have not been acknowledged by a client/irs.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[sp_AIR_SELECT_employer_submissions_NOT_Acknowledged]
	@ein varchar(9)
AS

BEGIN
	SELECT * FROM air.br.output_detail
	WHERE air.br.output_detail.receipt_id NOT IN (Select air.sr.status_request.receipt_id FROM air.sr.status_request)
	AND air.br.output_detail.header_id in (Select air.br.manifest.header_id FROM air.br.manifest WHERE air.br.manifest.ein=@ein);

END
GO

GRANT EXECUTE ON [dbo].[sp_AIR_SELECT_employer_submissions_NOT_Acknowledged] TO [air-user] AS [DBO]
GO











































GO


