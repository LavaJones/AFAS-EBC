USE [aca]
GO

/****** Object:  View [dbo].[View_1094Part1]    Script Date: 3/5/2018 11:00:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[View_1094Part1]
AS

SELECT 
	tya.[approval_id] AS [ApprovalId],
	tya.[employer_id] AS [EmployerId],
	er.name AS [EmployerName],
	er.DBAName AS [EmployerDBAName],
	er.ein AS [Ein],
	er.[address] AS [Address],
	er.[city] as [City],
	er.[state_id] AS [StateId],
	er.[zip] AS [ZipCode],
	u.fname + ' ' + u.lname AS [IrsContactName],
	u.phone AS [IrsContactPhoneNumber],
	tya.[tax_year] AS [TaxYearId],
	tya.[dge] AS [IsDge],
	tya.[dge_name] AS [DgeName],
	tya.[dge_ein] AS [DgeEin],
	tya.[dge_address] AS [DgeAddress],
	tya.[dge_city] AS [DgeCity],
	tya.[state_id] AS [DgeStateId],
	tya.[dge_zip] AS [DgeZipCode],
	tya.[dge_contact_fname] + ' ' + tya.[dge_contact_lname] AS [DgeContactName],
	tya.[dge_phone] AS [DgePhoneNumber],
	tya.[tr_q1],
	tya.[tr_q2],
	tya.[tr_q3],
	tya.[tr_q4],
	tya.[tr_q5],
	tya.[tr_qualified],
	tya.[tobacco],
	tya.[unpaidLeave],
	tya.[safeHarbor],
	0 AS [TransmissionTotal1095Forms],
	0 AS [IsAuthoritiveTransmission],
	tya.[completed_by] AS [CreatedBy],
	tya.[completed_on] AS [CreatedDate],
	tya.[completed_by] AS [ModifiedBy],
	tya.[completed_on] AS [ModifiedDate],
	tya.[ebc_approval],
	tya.[ebc_approved_by],
	tya.[ebc_approved_on],
	tya.[allow_editing],
	tya.[ResourceId],
	1 AS [EntityStatusId]
FROM
	[dbo].[tax_year_approval] tya
	INNER JOIN [dbo].[employer] er ON (tya.employer_id = er.employer_id)
	INNER JOIN [dbo].[user] u ON
		(
			er.employer_id = u.employer_id
				AND
			u.active = 1
				AND
			u.irsContact = 1
		) 




GO


