USE [aca-demo]
GO

/****** Object:  Table [dbo].[NightlyCalculation1]    Script Date: 9/29/2016 9:58:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NightlyCalculation](
	[CalculationId] [bigint] IDENTITY(1,1) NOT NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
	[EmployerId] [int] NOT NULL,
	[BatchId] [bigint] NOT NULL,
	[ProcessStatus] [bit] NOT NULL,
	[ProcessFail] [bit] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[NightlyCalculation] ADD  CONSTRAINT [DF_NightlyCalculation_ResourceId]  DEFAULT (newid()) FOR [ResourceId]
GO
GRANT SELECT ON [dbo].[NightlyCalculation] TO [aca-user] AS [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'INSERT_NightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[INSERT_NightlyCalculation] AS SET NOCOUNT ON;')
GO
GRANT EXECUTE ON [dbo].[INSERT_NightlyCalculation] TO [aca-user] AS [dbo]
GO
ALTER PROCEDURE [dbo].[INSERT_NightlyCalculation]
	@EmployerId int,
	@CreatedBy nvarchar(50)
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[NightlyCalculation] (CreatedBy,
		CreatedDate,
		ModifiedBy,
		ModifiedDate,
		EmployerId,
		BatchId,
		ProcessStatus,
		ProcessFail)
	VALUES (@CreatedBy,
		GETDATE(),
		@CreatedBy,
		GETDATE(),
		@EmployerId,
		0,
		0,
		0)
   
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO
--IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'SELECT_NightlyCalculation' AND type = 'P') 
--	EXEC('CREATE PROCEDURE [dbo].[SELECT_NightlyCalculation] AS SET NOCOUNT ON;')
--GO
--GRANT EXECUTE ON [dbo].[SELECT_NightlyCalculation] TO [aca-user] AS [dbo]
--GO
--ALTER PROCEDURE [dbo].[SELECT_NightlyCalculation]
	
--AS
--BEGIN TRY
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	SET NOCOUNT ON;

--	SELECT CalculationId,
--		CreatedBy,
--		CreatedDate,
--		ModifiedBy,
--		ModifiedDate,
--		EmployerId,
--		BatchId,
--		ProcessStatus,
--		ProcessFail 
--	FROM [dbo].[NightlyCalculation]
--	WHERE ProcessStatus = 0 AND ProcessFail != 1;
	   
--END TRY
--BEGIN CATCH
--	EXEC INSERT_ErrorLogging
--END CATCH
--GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_FailNightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_FailNightlyCalculation] AS SET NOCOUNT ON;')
GO
GRANT EXECUTE ON [dbo].[UPDATE_FailNightlyCalculation] TO [aca-user] AS [dbo]
GO
ALTER PROCEDURE [dbo].[UPDATE_FailNightlyCalculation]
	@employerId as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[NightlyCalculation] SET processStatus = 0, processFail = 1
	WHERE EmployerId = @employerId

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
IF NOT EXISTS (SELECT * FROM sys.procedures WITH(NOLOCK) WHERE name = N'UPDATE_NightlyCalculation' AND type = 'P') 
	EXEC('CREATE PROCEDURE [dbo].[UPDATE_NightlyCalculation] AS SET NOCOUNT ON;')
GO
GRANT EXECUTE ON [dbo].[UPDATE_NightlyCalculation] TO [aca-user] AS [dbo]
GO
ALTER PROCEDURE [dbo].[UPDATE_NightlyCalculation]
	@employerId as int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[NightlyCalculation] SET processStatus = 1, processFail = 0
	WHERE EmployerId = @employerId
	
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
