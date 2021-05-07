USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [appr].[spDeleteCoveredIndividual]
	@rowid INT

AS

	BEGIN TRANSACTION

	DELETE [emp].[covered_individual_monthly_detail]
	WHERE [covered_individual_id] = @rowId;

	DELETE [emp].[covered_individual]
	WHERE [covered_individual_id] = @rowId;

	COMMIT TRANSACTION

GO
