--this is an script you can run to populate the 
--employee_dependents and insurance_coverage_editable
--with data so you can generate the 1094 and 1095 files

DECLARE @employerId INT;
SET @employerId = 2;

DECLARE @taxYear INT;
SET @taxYear = 2015;

DECLARE @temp_employee_dependents TABLE (employee_id INT, dependent_id INT, UNIQUE (employee_id, dependent_id)) 

USE aca
DELETE FROM insurance_coverage_editable
DELETE FROM employee_dependents

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Donald' as fName,
	'Driver' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'James' as fName,
	'White' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Mike' as fName,
	'Wallace' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Randy' as fName,
	'Couture' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'James' as fName,
	'Jones' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Russell' as fName,
	'Westbrook' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Lee' as fName,
	'Daniels' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Dewayne' as fName,
	'Carter' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Shawn' as fName,
	'Carter' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Julio' as fName,
	'Jones' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Calvin' as fName,
	'Broudas' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Steve' as fName,
	'Smith' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Marcus' as fName,
	'Garvey' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Boyce' as fName,
	'Watkins' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Benard' as fName,
	'King' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Joseph' as fName,
	'Benjamin' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Anderson' as fName,
	'Silva' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.employee_dependents(employee_id,fName,lName,dob,ResourceId) 
OUTPUT inserted.employee_id, inserted.dependent_id INTO @temp_employee_dependents(employee_id,dependent_id)
SELECT
	employee_id,
	'Victor' as fName,
	'Moses' as lName,
	DATEADD(day, (ABS(CHECKSUM(NEWID())) % 65530), 0) as dbo,
	NEWID() as ResourceId
FROM employee
WHERE employer_id = @employerId

INSERT INTO dbo.insurance_coverage_editable(employer_id,employee_id,dependent_id,tax_year,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sept,Oct,Nov,[Dec])
SELECT @employerId, employee_id, dependent_id, @taxYear,1,0,1,1,1,0,1,1,1,1,0,1
FROM @temp_employee_dependents