USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employer_hr_status]    Script Date: 9/8/2017 3:01:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <9/8/2017>
-- Description:	<This stored procedure is meant to return all EntityStatus Rows.>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_entity_status]
AS
BEGIN
	SELECT *
	FROM [EntityStatus];
END


