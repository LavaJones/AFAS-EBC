USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 9/11/2015
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spInsert_ale_employer]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @strip_characters NVARCHAR(100) = '&|*|#|-|;|:|,|.|''|=|(|)'
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '1: Insert Employer';
INSERT INTO air.ale.employer (
	employer_id,
	ein,
	name,
	[address],
	city,
	state_code,
	zipcode, 
	contact_first_name,
	contact_last_name,
	contact_telephone
)
SELECT DISTINCT
	er.employer_id, 
	air.etl.ufnStripCharactersFromString(er.ein,@strip_characters,1,0), 
	UPPER(air.etl.ufnStripCharactersFromString(er.name,@strip_characters, 1,0)),
	UPPER(air.etl.ufnStripCharactersFromString(er.[address],@strip_characters, 1,0)),
	UPPER(air.etl.ufnStripCharactersFromString(er.city,@strip_characters, 1,0)),
	UPPER(s.abbreviation),
	SUBSTRING(er.zip,1,5), 
	UPPER(air.etl.ufnStripCharactersFromString(u.fname,@strip_characters, 1,0)) AS contact_first_name, 
	UPPER(air.etl.ufnStripCharactersFromString(u.lname,@strip_characters, 1,1)) AS contact_last_name,
	UPPER(air.etl.ufnStripCharactersFromString(u.phone,@strip_characters,1,1)) AS  contact_telephone
FROM
	aca.dbo.employer er
	INNER JOIN aca.dbo.tax_year_approval tya ON (er.employer_id = tya.employer_id)
	INNER JOIN aca.dbo.[state] s ON (er.state_id = s.state_id)
	INNER JOIN aca.dbo.[user] u ON (er.employer_id = u.employer_id)
	LEFT OUTER JOIN ale.employer em ON (er.employer_id = em.employer_id)
WHERE
	(em.employer_id IS NULL)
		AND
	(er.employer_id = @employer_id)
		AND
	(u.irsContact = 1)
		AND
	(u.active = 1);

-- ______________________________________________________________________________________________________________________________________________________

GO


