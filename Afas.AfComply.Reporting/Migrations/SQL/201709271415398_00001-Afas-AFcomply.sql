USE [ACA];

GO

DECLARE @CurrentMigration [nvarchar](max)

IF object_id('[dbo].[__MigrationHistory]') IS NOT NULL
    SELECT @CurrentMigration =
        (SELECT TOP (1) 
        [Project1].[MigrationId] AS [MigrationId]
        FROM ( SELECT 
        [Extent1].[MigrationId] AS [MigrationId]
        FROM [dbo].[__MigrationHistory] AS [Extent1]
        WHERE [Extent1].[ContextKey] = N'Afas.AfComply.Reporting.Migrations.Configuration'
        )  AS [Project1]
        ORDER BY [Project1].[MigrationId] DESC)
else
		CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)

IF @CurrentMigration IS NULL
    SET @CurrentMigration = '0'
PRINT @CurrentMigration

IF @CurrentMigration < N'201709271415398_00001-Afas-AFcomply'
BEGIN

	CREATE TABLE [dbo].[Approval1094] (
		[Approval1094Id] [bigint] NOT NULL IDENTITY,
		[ApprovedBy] [nvarchar](max) NOT NULL,
		[ApprovedOn] [datetime] NOT NULL,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		CONSTRAINT [PK_dbo.Approval1094] PRIMARY KEY ([Approval1094Id])
	)
	CREATE TABLE [dbo].[Approval1095] (
		[Approval1095Id] [bigint] NOT NULL IDENTITY,
		[ApprovedBy] [nvarchar](max) NOT NULL,
		[ApprovedOn] [datetime] NOT NULL,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		CONSTRAINT [PK_dbo.Approval1095] PRIMARY KEY ([Approval1095Id])
	)
	CREATE TABLE [dbo].[Correction1094] (
		[Correction1094Id] [bigint] NOT NULL IDENTITY,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Approved1094_ID] [bigint] NOT NULL,
		[Voided1094_ID] [bigint] NOT NULL,
		CONSTRAINT [PK_dbo.Correction1094] PRIMARY KEY ([Correction1094Id])
	)
	CREATE INDEX [IX_Approved1094_ID] ON [dbo].[Correction1094]([Approved1094_ID])
	CREATE INDEX [IX_Voided1094_ID] ON [dbo].[Correction1094]([Voided1094_ID])
	CREATE TABLE [dbo].[Void1094] (
		[Void1094Id] [bigint] NOT NULL IDENTITY,
		[VoidedOn] [datetime] NOT NULL,
		[VoidedBy] [nvarchar](max) NOT NULL,
		[Reason] [nvarchar](max),
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Approval_ID] [bigint],
		[Print_ID] [bigint],
		CONSTRAINT [PK_dbo.Void1094] PRIMARY KEY ([Void1094Id])
	)
	CREATE INDEX [IX_Approval_ID] ON [dbo].[Void1094]([Approval_ID])
	CREATE INDEX [IX_Print_ID] ON [dbo].[Void1094]([Print_ID])
	CREATE TABLE [dbo].[Print1094] (
		[Print1094Id] [bigint] NOT NULL IDENTITY,
		[OutputFilePath] [nvarchar](max) NOT NULL,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Approved1094_ID] [bigint] NOT NULL,
		[PrintBatch_ID] [bigint] NOT NULL,
		CONSTRAINT [PK_dbo.Print1094] PRIMARY KEY ([Print1094Id])
	)
	CREATE INDEX [IX_Approved1094_ID] ON [dbo].[Print1094]([Approved1094_ID])
	CREATE INDEX [IX_PrintBatch_ID] ON [dbo].[Print1094]([PrintBatch_ID])
	CREATE TABLE [dbo].[PrintBatches] (
		[PrintBatchId] [bigint] NOT NULL IDENTITY,
		[Reprint] [bit] NOT NULL,
		[PrintFileArchivePath] [nvarchar](max) NOT NULL,
		[RequestedBy] [nvarchar](max) NOT NULL,
		[RequestedOn] [datetime] NOT NULL,
		[SentOn] [datetime] NOT NULL,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		CONSTRAINT [PK_dbo.PrintBatches] PRIMARY KEY ([PrintBatchId])
	)
	CREATE TABLE [dbo].[Print1095] (
		[Print1095Id] [bigint] NOT NULL IDENTITY,
		[OutputFilePath] [nvarchar](max) NOT NULL,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Approved1095_ID] [bigint] NOT NULL,
		[PrintBatch_ID] [bigint] NOT NULL,
		CONSTRAINT [PK_dbo.Print1095] PRIMARY KEY ([Print1095Id])
	)
	CREATE INDEX [IX_Approved1095_ID] ON [dbo].[Print1095]([Approved1095_ID])
	CREATE INDEX [IX_PrintBatch_ID] ON [dbo].[Print1095]([PrintBatch_ID])
	CREATE TABLE [dbo].[Correction1095] (
		[Correction1095Id] [bigint] NOT NULL IDENTITY,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Approved1095_ID] [bigint] NOT NULL,
		[Voided_ID] [bigint] NOT NULL,
		CONSTRAINT [PK_dbo.Correction1095] PRIMARY KEY ([Correction1095Id])
	)
	CREATE INDEX [IX_Approved1095_ID] ON [dbo].[Correction1095]([Approved1095_ID])
	CREATE INDEX [IX_Voided_ID] ON [dbo].[Correction1095]([Voided_ID])
	CREATE TABLE [dbo].[Void1095] (
		[Void1095Id] [bigint] NOT NULL IDENTITY,
		[VoidedOn] [datetime] NOT NULL,
		[VoidedBy] [nvarchar](max) NOT NULL,
		[Reason] [nvarchar](max),
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Approval_ID] [bigint],
		[Print_ID] [bigint],
		CONSTRAINT [PK_dbo.Void1095] PRIMARY KEY ([Void1095Id])
	)
	CREATE INDEX [IX_Approval_ID] ON [dbo].[Void1095]([Approval_ID])
	CREATE INDEX [IX_Print_ID] ON [dbo].[Void1095]([Print_ID])
	CREATE TABLE [dbo].[TimeFrames] (
		[TimeFrameId] [bigint] NOT NULL IDENTITY,
		[Year] [int] NOT NULL,
		[Month] [int] NOT NULL,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		CONSTRAINT [PK_dbo.TimeFrames] PRIMARY KEY ([TimeFrameId])
	)
	CREATE TABLE [dbo].[Transmission1094] (
		[Transmission1094Id] [bigint] NOT NULL IDENTITY,
		[TransmissionTime] [datetime] NOT NULL,
		[TranmissionType] [int] NOT NULL,
		[UniqueRecordId] [nvarchar](max) NOT NULL,
		[TransmissionStatus] [int] NOT NULL,
		[IrsReciptId] [nvarchar](max),
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Approved1094_ID] [bigint] NOT NULL,
		CONSTRAINT [PK_dbo.Transmission1094] PRIMARY KEY ([Transmission1094Id])
	)
	CREATE INDEX [IX_Approved1094_ID] ON [dbo].[Transmission1094]([Approved1094_ID])
	CREATE TABLE [dbo].[Transmission1095] (
		[Transmission1095Id] [bigint] NOT NULL IDENTITY,
		[TransmissionTime] [datetime] NOT NULL,
		[TranmissionType] [int] NOT NULL,
		[UniqueRecordId] [nvarchar](max) NOT NULL,
		[TransmissionStatus] [int] NOT NULL,
		[ResourceId] [uniqueidentifier] NOT NULL,
		[EntityStatusId] [int] NOT NULL,
		[CreatedBy] [nvarchar](50) NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[ModifiedBy] [nvarchar](50) NOT NULL,
		[ModifiedDate] [datetime] NOT NULL,
		[Approval_ID] [bigint] NOT NULL,
		[Transmission1094_ID] [bigint] NOT NULL,
		CONSTRAINT [PK_dbo.Transmission1095] PRIMARY KEY ([Transmission1095Id])
	)
	CREATE INDEX [IX_Approval_ID] ON [dbo].[Transmission1095]([Approval_ID])
	CREATE INDEX [IX_Transmission1094_ID] ON [dbo].[Transmission1095]([Transmission1094_ID])
	ALTER TABLE [dbo].[Correction1094] ADD CONSTRAINT [FK_dbo.Correction1094_dbo.Approval1094_Approved1094_ID] FOREIGN KEY ([Approved1094_ID]) REFERENCES [dbo].[Approval1094] ([Approval1094Id]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Correction1094] ADD CONSTRAINT [FK_dbo.Correction1094_dbo.Void1094_Voided1094_ID] FOREIGN KEY ([Voided1094_ID]) REFERENCES [dbo].[Void1094] ([Void1094Id]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Void1094] ADD CONSTRAINT [FK_dbo.Void1094_dbo.Approval1094_Approval_ID] FOREIGN KEY ([Approval_ID]) REFERENCES [dbo].[Approval1094] ([Approval1094Id])
	ALTER TABLE [dbo].[Void1094] ADD CONSTRAINT [FK_dbo.Void1094_dbo.Print1094_Print_ID] FOREIGN KEY ([Print_ID]) REFERENCES [dbo].[Print1094] ([Print1094Id])
	ALTER TABLE [dbo].[Print1094] ADD CONSTRAINT [FK_dbo.Print1094_dbo.Approval1094_Approved1094_ID] FOREIGN KEY ([Approved1094_ID]) REFERENCES [dbo].[Approval1094] ([Approval1094Id]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Print1094] ADD CONSTRAINT [FK_dbo.Print1094_dbo.PrintBatches_PrintBatch_ID] FOREIGN KEY ([PrintBatch_ID]) REFERENCES [dbo].[PrintBatches] ([PrintBatchId]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Print1095] ADD CONSTRAINT [FK_dbo.Print1095_dbo.Approval1095_Approved1095_ID] FOREIGN KEY ([Approved1095_ID]) REFERENCES [dbo].[Approval1095] ([Approval1095Id]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Print1095] ADD CONSTRAINT [FK_dbo.Print1095_dbo.PrintBatches_PrintBatch_ID] FOREIGN KEY ([PrintBatch_ID]) REFERENCES [dbo].[PrintBatches] ([PrintBatchId]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Correction1095] ADD CONSTRAINT [FK_dbo.Correction1095_dbo.Approval1095_Approved1095_ID] FOREIGN KEY ([Approved1095_ID]) REFERENCES [dbo].[Approval1095] ([Approval1095Id]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Correction1095] ADD CONSTRAINT [FK_dbo.Correction1095_dbo.Void1095_Voided_ID] FOREIGN KEY ([Voided_ID]) REFERENCES [dbo].[Void1095] ([Void1095Id]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Void1095] ADD CONSTRAINT [FK_dbo.Void1095_dbo.Approval1095_Approval_ID] FOREIGN KEY ([Approval_ID]) REFERENCES [dbo].[Approval1095] ([Approval1095Id])
	ALTER TABLE [dbo].[Void1095] ADD CONSTRAINT [FK_dbo.Void1095_dbo.Print1095_Print_ID] FOREIGN KEY ([Print_ID]) REFERENCES [dbo].[Print1095] ([Print1095Id])
	ALTER TABLE [dbo].[Transmission1094] ADD CONSTRAINT [FK_dbo.Transmission1094_dbo.Approval1094_Approved1094_ID] FOREIGN KEY ([Approved1094_ID]) REFERENCES [dbo].[Approval1094] ([Approval1094Id]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Transmission1095] ADD CONSTRAINT [FK_dbo.Transmission1095_dbo.Approval1095_Approval_ID] FOREIGN KEY ([Approval_ID]) REFERENCES [dbo].[Approval1095] ([Approval1095Id]) ON DELETE CASCADE
	ALTER TABLE [dbo].[Transmission1095] ADD CONSTRAINT [FK_dbo.Transmission1095_dbo.Transmission1094_Transmission1094_ID] FOREIGN KEY ([Transmission1094_ID]) REFERENCES [dbo].[Transmission1094] ([Transmission1094Id]) ON DELETE CASCADE
	CREATE TABLE [dbo].[__MigrationHistory] (
		[MigrationId] [nvarchar](150) NOT NULL,
		[ContextKey] [nvarchar](300) NOT NULL,
		[Model] [varbinary](max) NOT NULL,
		[ProductVersion] [nvarchar](32) NOT NULL,
		CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
	)
	INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
	VALUES (N'201709271415398_00001-Afas-AFcomply', N'Afas.AfComply.Reporting.Migrations.Configuration',  0x1F8B0800000000000400ED1DCB6EE338F2BEC0FE83E1D3EE021327E9685E7066904E3A8360269D86D3D38B3D0D188B718495258F2407C9B7ED613F697F61A937DF0F89B215376FB64816AB8A5545B2582CFEEF3FFF9DFFFCB20E27CF304983383A9F9E1C1D4F27305AC67E10ADCEA7DBECF19BEFA73FFFF4D7BFCC3FF8EB97C997BADEBBBC1E6A19A5E7D3A72CDBFC389BA5CB27B806E9D13A5826711A3F6647CB783D037E3C3B3D3EFE617672328308C414C19A4CE68B6D94056B58FC417F2FE3680937D91684B7B10FC3B4FA8E4AEE0BA8938F600DD30D58C2F3E9C523EAE4E2F1325E6FC2D7A305DCC44986903DBA8AD72088A6938B300008AB7B183E4E27208AE20C6408E71F7F4FE17D96C4D1EA7E833E80F0F3EB06A27A8F204C6145CB8F6D755DB28E4F73B2666DC34E6C99360423923F20D664AF397A05D988E2CD26899F417872FCC3195E13D5FD15BE121FD0A74F49BC8149F6BA808F55FB9BABE96446B69BD10D9B66589B1C05F42BCABE3D9B4E3E6EC3103C84B0E118C6DAFB2C4EE02F308209C8A0FF0964194C10076F7C5890C2F44EF555D207FDF7AF759F68A0D0904E27B7E0E53718ADB2A7F329FA399D5C072FD0AFBF5478FC1E05485C51A32CD9420E9E7A7DDF4575DF578886CF48368D412D601A6F9325BCF16B50BF6C03DF184C39FCF788B5DBB4E10712E623BCE043B45D1B43BE4C603E3E52367BC74370B9EA39676D6F36230B113C067B21A3EEBA031DF359ABD6BACAEE396577CAEE94FD8095FD324E12B8CCD5EA50E776A7274E4F64C03E82E76055489AC056178A3159C0B0A8943E059B6AE449DDF983AC7F9DC4EB451C322A4654FBE3BE104D8474ACAEFB19242B98E9E3FF250E7C7DECF1DA42DCDB4A2ACCB19A3CBCB5ED530EE7502D53C9230B4B8112D03ED6330B08D238B2D0AFB3E1CE860F66C341C8B580B56DF9A3ADD55A3EA690B1786C0D530BFD09313793A35655E1E0559488912A8B7BD9DE02C4A11ADFBB6DB6D966D7410851D3A77D584E67D29C491B6259DAA8AD7045CAAFC1D81241B54E56EE3DC8964F0A74F18A3C64DB7209AA58A5FEC6AF44E600ADDF026ECA89A5ECF07D8CB8082263112F9894DBD08B64F9143CEFCD94FEB985A9CA560DDDB98575FC3D1A3CE7197453CBDEA696302C34BA34F4A93D735D5B62A9B9AE6D7A276C3D39B69E025B4F075B4F85ADF1CAFA20CF57DCCADA99BFB769FEDA55AEA7B02664559E3DC16B0897AB54B58156D646C64F8CAAAD9535E1293E480BE82C90B340C358205277846648524D7E70D3D3209507113A98D7358538971554D856B56C1C311DA42572474CEE88C9D9ECBD1F3179B223264F79C4E40D78C4E4098F983CF9119367E38829E7FD75827E1EA2F1FD170409D6DBBBD30E221BB53BE86E209C5D7376AD7F98E0E70444E93A48D3030E14C4692C79D753267280353C0489501BA2B3FC1E88317424347F6EE1022EE3C46F957B77EB329C028E5DE0151BF67093A488BC60935921CF99496726873933C9174398AEF34F22280B8AF6D6B449C55660CACACC1985BA85F1F18A2ACE80E941146E20ADC82C2FE5B5FBAD362926B959CCCD6283CF626E6651F5EC661673C70263EF790E06612595D1EDE17060A7B5E127432535C2E9536F1AD9AEB14984D5B79BF43A04AB1493261F26E12B123E7CE34EF2EF16AE1F60528FF4320B9E51C32F20DCA2BF270CB789DA371120EB9FCAEB5FC11066B9AFBDAAFE8E25BB24504234C7D0F7A6BA72E563987DFB9D9C92BB2458C5512EE35583EF7E903758C04D0896700D730757D5E6FBD39EF4D736D8C2B0E7290730FA15035FD7FF67903D7D48923849754560015724A7DFC9EB239D5E42446C6E3EAB1667F2161FE3EC3ADE466D0F9E0E972FD2345E068575E01D0E534B4AB2FB0F913F31B860D54E98D495C7C9ED36CC824D182C915A9F4FFFC1D0A9D751B3206F3B22B22650DD9C90D4A06EEEA252512717CB32FDC3254897C067270CC4499FE02CC6453973D90B0F224A25B71F5AFA9ABB59062C145F9AD064DCF1D1D1092D5ADA0C1084468B9055C549B718B757250C78A188AF1EB32409226154948A8EA259467A5D182938BDE63392E961CF8C14069CF3E8E405EC5863222F1492025EC5128D888167260CE4DF25B0A6CE6F8A81D45533A5E1A60E052D4D07E459A2CE50F49A08C4979F35E77EDE4DE80196189C6BD41A1CDF8B1CC94293B4A8554F0E54E46057B6BEC169828D235068146FAFCF088E110BC5B1089A8CEBA5B054B48212499595EA46BAD24AD9259A1F33A729E37400DD006A4445DF6970792FCA23F69589E8D4709CF17DC4A60C55FBDDC66C95D46E3C6DBAC53EBD81182D3E511376388E79567EECA649BF7A47ABA2DD80D9A3DFDF962EDDCB38CA4010E1CEC332B1E715C8405E085FD889A7F005C38C72D99F94D7E75A7F32974E867F42609E0298A70446AE2B79E0E835AB0260BDF6E4816AD7A50A20CDE29E07055BF9EB802976545008A8DA706922C4653836C99B709B0B8B9E83F5B8CD05D5CEB50A204DB0250F4A1B89A904436937171A633DCC80720965ED3F0514536F91E453410A580B8DA45EB421327341372473B471D60D746D4A31D09409A22D22C9220DF67192E4B04C533896355DCB1815987591B046EC4C1E9021A2F41A2C5774BCCD26FE668C28DC704A18A4F030EF804BF41D253197642E03136732874B9E1E97047E013E975898BDB884CF51321E899C9CFA7EE23EFCE138366970CD346C51D1F49823F600EBFB80FBA8D84E9943270893D8648EC744C7B3DBDD1A935E122D9676E080247FA5725617F97B0D3DBE7666748E8F5787F77D59A6B6CCDA1E5D639FAE88710A43A4E3C51DD05A732E7309D54EE464D2F4D5B202A0608DD83BBB0B86A88C10D76DABE3B8EDCE07A511B2C201C1AD66A52EF15CB906CE5C3BFA43B96F7578DD8145924845964D9AAE5A43672D4619679F296198DA3D3BA0766904496A3050EE92EDE894B5C750A11B56DC8595A950717941C958ED3D9D9EE3D584DA0EAE560B1BBD3A3AB6F1923665F359F94452F5613E13BCA534BF059B4D10ADB0B795AA2F93FBF261A5CB6FEECD1F2F5A973066CB94F38651836DD35316276005A9D2DC34FAF03A48D22C77F63E803C9EF3D25F33D54C7CC275973CD7303BA0B5AFAB6E95FFAE5A4A9F9B227DCA14DC96C5D788EA3C0CB6600014C803DB7C92BF7F054290706EB45CC6E1761DB1D4E5171D68891543C25F63612196DFCDA1E5192F78D0F2EFFAD0F0AB1B3834FCBB3E34F206070E0F2F318389DDDDC001629F8D6195B71838D0CA027D78F88D0C1C1CFEDD1C1A8B1E59C2429CCF281DA0956FC6681F738C44AAB4A9C27B43293CB30F3353784EF30E0AEF3985770AEF149E7101595479D92B4D056C95D2AB00E8AA3D09C74C749D7239E5EAAC5CAD1BD4A25AF11F172AA0AA144ADC5457956A086622DA2674A36199CE756D463716929930D519DA48A52EBF39F3E0CC03593E8879C04E962CDA07C103380558958190B4D5B5100D083331A5F36FE320E932A79E4E3DC9F2E1D4B33EFCB6ADA0DC037F5D0D15343652D10286E95AB87A7F8554A5EAA33E1CFE032C0C864C0D134CB1C755486CB1820EF0E8350C51A00FAF7E330507557F73D6CD5937B27CD0C5875D379FE08D9002ACEEE2A38783AF01E1161F3A309D7A8E563DE90889A1FC725D145505A0935FCE50659D7239E5EAEB97B3AB56FC17190AA89A7EB91EAA5443707E39671E9C79A0CB0DCD0376E5CCA27D10BC1A5080551908495B5D0BD1803013D3F239001C50F9C564F8237A4D5D7D72EAECD4992C1F469D99D8449B5AAD8854D5516E25086D1DA7209989309B845904BB2C35834C6463A6011385FA70E93CCC3858BAAC1B1F78468257AE0F9D781F00074B1438D3E84C2359BE0BD3687743244F5C5F4037348D3D36483424671AC7671A9D2973A64C6DCAC8CB1EB26846EA7A4CC7C0450A8A5990627EE9851E77756A0C96A706E1CD1528F6C98F92B50D3AFD30ADEE0075C4541D91294695BEEE632C249CE420C6C177585BDD203B0E8385A9407A0900026361F085194F3AE2268B1C94A27619477E50A412BB49F367299AD4F59A94F7961751EE14F3882C852911465F7186479E24652406449E1AA62392D200B301CD8628398CF9D9380540FB1C5CC2617E5E81FE62E0D914037E5E859E62C05DC9EF440CF0042F1D84006F6E4704D85C2E7D785B02B23AFC6CEE9A373AF867FD06FF4C6BF0CDA681910F3E2F71D11B9B00E8EC45E68BC6AAA18D15239129A40F23ADAE15893C28A35C288A68EE2D1D92CC4E5D379E380CDBDB4E367F538FE11A6CCBC9A6AF7A8B1B4E6906AB8E615B8A35A43C444BC5F851AF26355275D910925D2F2D3879BA8C438FD45E091D511066E51A8F57824D97D46FB2311CEC317825E83466E6C2225F8D1849CA1857239ECDD5C84002B29BD5489BE2ADEB6453B7B73DCD90C9DC7AAD4006985AC8FC756F715291A4B1EB7C8C2B996454C7B51C962B93D58D60D251E6EAEB88A3CE99F42E85834DD5D75D48585843088B28119F9D01B1B2A3D1CE4F781042D4F98C4401C734664E3D10A33E38D1CABF6849C86D0B0C937391AED20408545F9AFF4DCEC52ADF219188B1202F4FAB58909556B917E904886595E904E1FE8C26EFE47C7AFF9A66707D945738BAFF33BC0C0398AF7AEB0AB7200A1E619A7D8EFF0DA3F3E9E9F1C9E974721106202D53689AA77684FE7A96A63E91FC150BA39A90F3132BBCF35F212321F5502CE023A73D3B82F3190D644E4905DDBE7CB1E42158B57B825F201ABC3C12E513C8329820EEDCF8B0A0633AC957B4E021CFE559AD6A671ABD953128654FD13348964F20F9DB1ABCFCBD33BCFC924A09CF47886645EC9521283CB8A804B52DC2A38282D6C70026C620E9F8A2126CC1584348586011C9B6E9E416BCFC06A355F6743EF58EBB022E63787A72100F30B28B25196A6484261E65A4AB88F4939FA68AC8997B8D14D1738AE814F1AB5544D989869E2AAA9CFC6A6564210CAB8E4EE60F52E6C576B2D920907265088D3A733380A5AD8DFC33693D3D149FE0AA35106F3BACEEB5D79C7B8E6D7BCBD9C68C5ADF7556C0CA92ADB32E32C05F9F75A99C9D226BA02331ED79843E146D9322886BD2B32992A01FB551211A0F6B55E87441764C82D3E3AF488F6DAD12A840BE215609A26C84063A2DC843A8A9D44DEBA1D7E99BF6943DEFA6E36830290DED98072CA7A15580161647755643E71C7026B0AFA1E9EAA293448BEBAF1D76E09C736B07A73836D60EDE9B583BC862873AF8FB3A29370BC1F9FB9C368E4E1BB108B7017D7D9D74501C1FA9EDEBDB81DE395F9FB32C076559C6EEEB13242CD4B3299254856AA342341ED6AA949910BBAB459506B13B00A7F407A9F4FA5A268D82D454366538A086CE71600CAB7A6CCAAD9E82C0A4DAEA2EF8747A2D1B2B055E62ADEE1812A906DDE2C3D9A10EDB9AE1420FE417033A59B54EDB131E0C67D5C66BD59C15FAAAAC90620B642C80366D1A7647821B5948DE6E2151FC10F993451C42EA8E4085567E27E1882CB8DD8659B009832542E47C7AC25C85B98BAE60083338B928FA47B88074097C9639F9A50F112E5474268E0D5D44E2F30FA61B64A56192AB24082FE328CD1210B04922F3DD295AA18090C70BAA327706905F90C8A96DBAA04BAEE00646B9D1E0D3AEDFBBF44250D30B35082AF6103770E442C7662BB42A6AC7474727CCE8B6309B10421C5EFBF1E0C444F230B3B8DFFD0987202BE15B30466D24198E08F6F5E0644BF6A8EF58AD8F20DFA18680792201F3762C601E57C0182CEC0B182F59BA54C004F77E8D05CCA463694E9F9D0A98300E8D1A522C611F3EA6D5E703162DE99BC3669179038A9532F5E04EA7C5372854BB9D10F72D547A93E118848ACA0069691847B8FEEE3034BB5E79CBF24CED4014C4391EAD8CE3816EFF8D46589E4174C86DBF3219E6AE05EC0DAEBDA9B840A1A81DE22A5CF934F35897E26C064AABA2A637D3793C0B79886222796A788C9E262AE3A4A59DD10865A2C36E67D7D230A6D54F9357D2CA181EE8746434BAF26CA5434E43D2249C3B102C7196CAB7B0E661421E707CD8C2839BD0349E8A1CE3C4A6CE7AA939E067B201DFD51E6F0C42A8F19C729710C6E185513375E8EE85F20D1EEFF5528E0338ECEBA404FB38F3A3127D366B3C2A29272374551A570EE7A69336A0862B806582CFF3A9FF1023B1280373C81ACC8E40D89B27EFCD53F6E6297BA39D634C7F74055E8F741D459FAD6B90E9AD2DE2F5D3962A7AC03CD24C175819AF0FAC58A793EA1085DF4BFD7E96A89BA21C324F100BC9E1C903562623C75012783DD1159492A0EEB3DD2A892441D04F5BAAE8A1BDE7C3768195F1FA688AD503C4DA7CB63336DF38AF4FA69659CF3C4EB255347A66394BA655D6881A9C602DB8C64432F14BA6FE860372BB2A3D1DC060286C17954D5A83259C177859462802DF2C934F1B5CF251B7FE240B22B53874EBC47459269E990BA8370CED914FBF7925265F76EA21F101F0C9675455E4B4E490CFAA791FF2F1494F46BCF85C5E10BA40635E7D1E05D9679A648BE33506207B3861A7DF8A94D8368E37BD0FB23B356992970E95D39AE8C8BC3B017B9BCCA46FFA2918B14323C75FBBF25E80B2A6009ECEFC2EF2F25A269F5E4693CFA4592459A5F3DC13B43E7679A7640ADE14538A39EF90A83BE27B136CC93B5A2C0B348F332C0BBA68B7C37FAF690096B0FB330DD6E8BC0524F42C8A0994CF127B66957A2B60E006B6BC23E8C564096B9887859AB2F9ACDC72571FD05FE601A1F96CB18DF2DB86E5BF2B9806AB1644FE5A63546A7B0BB4AE73133DC6B5BF95C2A8AE42DF768419F041062E922C7804CB0C152F2122395A4D275F40B845553EAC1FA07F139529FF10C970FD10BEE2CCC8DDB5B2FEE73306E7F9DD26FF97DA2001A119E41734EFA2F7DB20F41BBCAF3997200520723F7075393A1FCB2CBF24BD7A6D207D8C234D4015FB1AF7F567B8DE8408587A17DD836728C64DCD439263F3AB00AC12B04E2B186D7BF417899FBF7EF9E9FF2BE0EE85FD250100 , N'6.0.2-21211')

	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Approval1094] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Approval1095] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Correction1094] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Void1094] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Print1094] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[PrintBatches] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Print1095] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Correction1095] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Void1095] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[TimeFrames] TO [aca-user] AS [dbo];	
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Transmission1094] TO [aca-user] AS [dbo];
	GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [aca].[dbo].[Transmission1095] TO [aca-user] AS [dbo];

END

GO

