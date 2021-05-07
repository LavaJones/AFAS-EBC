USE aca
GO
DECLARE @tempTable as TABLE (
	CountId int IDENTITY(1,1),
	employerId int
)
INSERT INTO @tempTable (employerId)
SELECT employer_id FROM dbo.[tax_year_employer_transmission] GROUP BY employer_id

DECLARE @inc as int 
DECLARE @id as int
DECLARE @emp as int

SELECT @id = min(CountId) FROM @tempTable

WHILE @id is not null
BEGIN
	SET @inc = 1
	SELECT @emp = employerId FROM @tempTable WHERE CountId = @id
	UPDATE dbo.tax_year_employer_transmission SET UniqueSubmissionId = @inc, @inc = @inc + 1
	WHERE TransmissionType = 'C' and employer_id = @emp and tax_year = 2017
	SELECT @id = min(CountId) FROM @tempTable WHERE CountId > @id
END