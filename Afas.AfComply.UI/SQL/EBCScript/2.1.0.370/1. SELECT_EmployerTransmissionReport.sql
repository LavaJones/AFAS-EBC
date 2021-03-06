USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_EmployerTransmissionReport]    Script Date: 6/4/2018 12:28:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_EmployerTransmissionReport]
	@startDate Datetime,
	@endDate Datetime	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT er.name,
	   er.ein
	  ,[employeeID]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[FirstName]
      ,[MiddleName]
      ,[LastName]
      ,[SSN]
      ,[StreetAddress]
      ,AF.[City]
      ,[State]
      ,AF.[Zip]
      ,[EmployeeResourceId]
      ,[SelfInsured]
      ,[TaxYear]
  FROM [aca].[dbo].[Approved1095Final] AF INNER JOIN aca.dbo.employer ER ON AF.employerID = ER.employer_id 
  where EntityStatusId = 1 and CreatedDate BETWEEN CONVERT(datetime, @startDate) AND CONVERT(datetime,@endDate)
END
GO
GRANT EXEC ON [dbo].[SELECT_EmployerTransmissionReport] TO [aca-user]
