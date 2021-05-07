USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 1/31/2017
-- Description: Select all ALE for an employer
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ALE_Members_ForEmployer]
      @EmployerId int
AS
BEGIN TRY

      SET NOCOUNT ON;

	SELECT
		[Ale_Member_Id],
		[employer_id],
		[ein],
		[name],
		[ResourceId],
		[EntityStatusId],
		[CreatedBy],
		[CreatedDate],
		[ModifiedBy],
		[ModifiedDate]
	FROM
		[dbo].[other_ale_group_members]
	WHERE
		[employer_id] = @EmployerId
			AND 
		[EntityStatusId] = 1

END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH

GO
