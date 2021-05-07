GO
ALTER TABLE aca.dbo.insurance_coverage
ADD batch_id int NULL;


ALTER TABLE aca.dbo.insurance_coverage
ADD CONSTRAINT FK_icbid FOREIGN KEY (batch_id)
    REFERENCES aca.dbo.batch(batch_id);