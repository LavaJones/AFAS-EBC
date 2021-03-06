USE [aca]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UPDATE_employeeType]
       -- Add the parameters for the stored procedure here
       @employeeId int,
       @name varchar(50)
       
AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    UPDATE [dbo].[employee_type] SET name = @name
       WHERE employee_type_id = @employeeId
       
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO

GO
ALTER PROCEDURE [dbo].[SELECT_errorLog]
       @time datetime,
       @errorNumber int,
       @errorLine int,
       @errorProcedure varchar(50),
       @rowsPage int,
       @pageNumber int

AS
BEGIN TRY
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    -- Insert statements for procedure here

       SELECT [ErrorLogID]
      ,[ErrorTime]
      ,[UserName]
      ,[ErrorNumber]
      ,[ErrorSeverity]
      ,[ErrorState]
      ,[ErrorProcedure]
      ,[ErrorLine]
      ,[ErrorMessage]
  FROM [dbo].[ErrorLog]
  WHERE ((@time is null) OR (ErrorTime = @time))
  AND  ((@errorNumber is null) OR (ErrorNumber = @errorNumber))
  AND   ((@errorLine is null) OR (ErrorLine = @errorLine))
  AND   ((@errorProcedure is null) OR (ErrorLine = @errorLine))
  ORDER BY Errortime DESC OFFSET ((@pageNumber  - 1) * @rowsPage) ROWS
  FETCH NEXT @rowsPage ROWS ONLY;

END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
