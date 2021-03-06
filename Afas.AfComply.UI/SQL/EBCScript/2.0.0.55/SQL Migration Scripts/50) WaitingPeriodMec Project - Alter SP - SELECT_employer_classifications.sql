USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employer_classifications]    Script Date: 9/1/2017 9:18:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure is meant to return all EMPLOYEE CLASSIFICATIONS for a specific district.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_classifications](
	@employerID int
	)
AS
BEGIN
	SELECT [classification_id]
      ,[employer_id]
      ,[description]
      ,[ModifiedDate]
      ,[ModifiedBy]
      ,[history]
      ,[ash_code]
      ,[ResourceId]
	  ,[WaitingPeriodID]
	  ,[CreatedBy]
	  ,[CreatedDate]
	  ,[EntityStatusID]
	  ,[Ooc]
	FROM [employee_classification]
	WHERE [employer_id]=@employerID;
END


