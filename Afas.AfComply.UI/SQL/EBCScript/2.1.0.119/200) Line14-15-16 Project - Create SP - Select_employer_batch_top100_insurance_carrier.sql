USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[SELECT_employer_batch_top25]    Script Date: 12/18/2017 11:25:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/18/2017>
-- Description:	<This stored procedure is meant to return all batch ID's for a specific employers Insurance Carrier imports.>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employer_batch_top100_insurance_carrier](
	@employerID int
	)
AS
BEGIN
	SELECT TOP 100 [batch_id]
      ,[employer_id]
      ,[modOn]
      ,[modBy]
      ,[delOn]
      ,[delBy]
      ,[ResourceId] 
	FROM [batch] b
	WHERE
		b.[employer_id]=@employerID AND b.[batch_id] 
			in(Select [batch_id] from [import_insurance_coverage_archive] where [import_insurance_coverage_archive].[employer_id] = @employerID)
		OR  
		b.[employer_id]=@employerID AND b.[batch_id] 
			in(Select [batch_id] from [import_insurance_coverage] where [import_insurance_coverage].[employer_id] = @employerID)
		OR 
		b.[employer_id]=@employerID AND b.[batch_id] 
			in(Select [batch_id] from [insurance_coverage] where [insurance_coverage].[employee_id] 
			in(Select [employee_id] from [employee] where employee.employer_id=@employerID) )
	ORDER BY [modOn] DESC;
END


GO


