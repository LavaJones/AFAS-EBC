USE aca
GO
UPDATE dbo.ArchiveFileInfo SET [SourceFilePath] = REPLACE([SourceFilePath], 'PROD-Secure', 'PROD-Secure-Docs') WHERE [SourceFilePath] not like '%PROD-Secure-Docs%'