USE [air]
GO

ALTER TABLE [gen].[state_code] ALTER COLUMN [name] NVARCHAR(100) NOT NULL;
GO

INSERT INTO [gen].[state_code] (
	[code],
	[name]
) VALUES (
	'DC',
	'District of Columbia'
)
