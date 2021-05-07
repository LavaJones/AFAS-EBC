USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_1094Part3]
AS


   
  SELECT count(*) as NoOfEmployees,
		 f.employerID as EmployerId,
		 Receiving1095C ,
		 f.TaxYear,
		 Offered as OfferedInsurance,
		 MonthId
  FROM [aca].[dbo].[Approved1095FinalPart2] p2
  join [aca].[dbo].[Approved1095Final] f on p2.Approved1095Final_ID=f.Approved1095FinalId
  group by MonthId, Offered,f.TaxYear,f.employerID, Receiving1095C 
 

GO
