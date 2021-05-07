USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_AIR_ETL_ShortBuild]
	@employerID int,
	@taxYear int,
	@employeeID int
AS

BEGIN TRY
	
	EXEC [air].etl.spETL_Build
		@employerid,
		@taxYear,
		@employeeID

END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH

GO

SET IDENTITY_INSERT [dbo].[state] ON

INSERT INTO [dbo].[state] (
	state_id,
	[description],
	[abbreviation]
) VALUES (
	51,
	'District of Columbia',
	'DC'
)

SET IDENTITY_INSERT [dbo].[state] OFF
