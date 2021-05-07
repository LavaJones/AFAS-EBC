USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[SELECT_tax_years]    Script Date: 8/30/2017 9:05:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_Waiting_Periods]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT *
	FROM WaitingPeriod
END


GO


