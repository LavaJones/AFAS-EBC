USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employer]    Script Date: 9/6/2017 9:01:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/6/2017>
-- Description:	<This stored procedure is meant to return all affordability safe harbors.>
-- Changes:		<removed select *>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_affordability_safe_harbor]
AS

BEGIN
	SELECT *
	FROM [affordability_safe_harbor];
END


