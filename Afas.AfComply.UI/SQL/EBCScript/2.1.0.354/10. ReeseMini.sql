USE aca
GO
UPDATE dbo.PrintBatches SET PdfReceivedOn = GETDATE() WHERE EmployerId = 844
