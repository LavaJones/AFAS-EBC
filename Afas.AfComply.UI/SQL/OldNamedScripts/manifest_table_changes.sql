USE air
GO

ALTER TABLE br.manifest ALTER COLUMN vendor_id tinyint NULL
GO

ALTER TABLE br.manifest ADD manifest_file_name nvarchar(300) null
GO

ALTER TABLE br.manifest DROP CONSTRAINT FK_manifest_vendor
GO

ALTER TABLE fdf._1094C ALTER COLUMN vendor_id int NULL
GO 

EXEC sp_RENAME 'fdf._1095C.annual_share_lowest_cost_montyly_premium', 'annual_share_lowest_cost_monthly_premium', 'COLUMN'
GO

ALTER TABLE tr.header DROP CONSTRAINT FK_header_transmitter
GO


