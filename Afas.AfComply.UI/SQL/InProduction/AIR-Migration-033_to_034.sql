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
ALTER PROCEDURE [etl].[spUpdate_employer]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @strip_characters NVARCHAR(100) = '&|*|#|-|;|:|,|''|.|=|(|)'
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '2: Update Employer';
UPDATE air.ale.employer 
		SET	ein = air.etl.ufnStripCharactersFromString(er.ein,@strip_characters,1,0),
			name = UPPER(air.etl.ufnStripCharactersFromString(er.name,@strip_characters, 1,0)),
			[address] = UPPER(air.etl.ufnStripCharactersFromString(er.[address],@strip_characters, 1,0)),
			city = UPPER(air.etl.ufnStripCharactersFromString(er.city,@strip_characters, 1,0)),
			state_code = UPPER(s.abbreviation),
			zipcode = SUBSTRING(er.zip,1,5),
			contact_first_name = UPPER(air.etl.ufnStripCharactersFromString(u.fname,@strip_characters, 1,0)), 
			contact_last_name= UPPER(air.etl.ufnStripCharactersFromString(u.lname,@strip_characters, 1,0)), 
			contact_telephone= UPPER(air.etl.ufnStripCharactersFromString(u.phone,@strip_characters, 1,1)),
			dge_ein = air.etl.ufnStripCharactersFromString(tya.dge_ein,@strip_characters, 1,0)
FROM
	aca.dbo.employer er
	INNER JOIN aca.dbo.tax_year_approval tya ON (er.employer_id = tya.employer_id)
	INNER JOIN aca.dbo.[state] s ON (er.state_id = s.state_id)
	INNER JOIN ale.employer er1  ON (er.employer_id = er1.employer_id)
	INNER JOIN  aca.dbo.[user] u ON (er.employer_id = u.employer_id) 
WHERE
	(er.employer_id = @employer_id)
		AND
	(u.irsContact = 1)
		AND
	(u.active = 1)

-- ______________________________________________________________________________________________________________________________________________________


GO