USE [aca]
GO

CREATE TABLE [dbo].[Approved1095FinalPart2] (
    [Approved1095FinalPart2Id] [bigint] NOT NULL IDENTITY,
    [employeeID] [int] NOT NULL,
    [TaxYear] [int] NOT NULL,
    [MonthId] [int] NOT NULL,
    [Line14] [nvarchar](max),
    [Line15] [nvarchar](max),
    [Line16] [nvarchar](max),
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [Approved1095Final_ID] [bigint],
    CONSTRAINT [PK_dbo.Approved1095FinalPart2] PRIMARY KEY ([Approved1095FinalPart2Id])
)
CREATE INDEX [IX_Approved1095Final_ID] ON [dbo].[Approved1095FinalPart2]([Approved1095Final_ID])
CREATE TABLE [dbo].[Approved1095FinalPart3] (
    [Approved1095FinalPart3Id] [bigint] NOT NULL IDENTITY,
    [InsuranceCoverageRowID] [int] NOT NULL,
    [EmployeeID] [int] NOT NULL,
    [DependantID] [int] NOT NULL,
    [TaxYear] [int] NOT NULL,
    [FirstName] [nvarchar](max) NOT NULL,
    [MiddleName] [nvarchar](max),
    [LastName] [nvarchar](max) NOT NULL,
    [SSN] [nvarchar](max),
    [Dob] [datetime],
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [Approved1095Final_ID] [bigint],
    CONSTRAINT [PK_dbo.Approved1095FinalPart3] PRIMARY KEY ([Approved1095FinalPart3Id])
)
CREATE INDEX [IX_Approved1095Final_ID] ON [dbo].[Approved1095FinalPart3]([Approved1095Final_ID])
ALTER TABLE [dbo].[Approved1095Final] ADD [FirstName] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[Approved1095Final] ADD [MiddleName] [nvarchar](max)
ALTER TABLE [dbo].[Approved1095Final] ADD [LastName] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[Approved1095Final] ADD [SSN] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[Approved1095Final] ADD [StreetAddress] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[Approved1095Final] ADD [City] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[Approved1095Final] ADD [State] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[Approved1095Final] ADD [Zip] [nvarchar](max) NOT NULL DEFAULT ''
ALTER TABLE [dbo].[UserEditPart2] ALTER COLUMN [OldValue] [nvarchar](max)
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'terminationDate';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [terminationDate]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'acaStatusID';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [acaStatusID]
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'classificationID';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var2 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [classificationID]
DECLARE @var3 nvarchar(128)
SELECT @var3 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'monthID';
IF @var3 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var3 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [monthID]
DECLARE @var4 nvarchar(128)
SELECT @var4 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'monthlyAverageHours';
IF @var4 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var4 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [monthlyAverageHours]
DECLARE @var5 nvarchar(128)
SELECT @var5 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'defaultOfferOfCoverage';
IF @var5 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var5 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [defaultOfferOfCoverage]
DECLARE @var6 nvarchar(128)
SELECT @var6 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'mec';
IF @var6 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var6 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [mec]
DECLARE @var7 nvarchar(128)
SELECT @var7 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'defaultSafeHarborCode';
IF @var7 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var7 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [defaultSafeHarborCode]
DECLARE @var8 nvarchar(128)
SELECT @var8 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'employeesMonthlyCost';
IF @var8 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var8 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [employeesMonthlyCost]
DECLARE @var9 nvarchar(128)
SELECT @var9 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'employeeOfferedCoverage';
IF @var9 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var9 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [employeeOfferedCoverage]
DECLARE @var10 nvarchar(128)
SELECT @var10 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'employeeEnrolledInCoverage';
IF @var10 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var10 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [employeeEnrolledInCoverage]
DECLARE @var11 nvarchar(128)
SELECT @var11 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'insuranceType';
IF @var11 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var11 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [insuranceType]
DECLARE @var12 nvarchar(128)
SELECT @var12 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'offeredToSpouse';
IF @var12 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var12 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [offeredToSpouse]
DECLARE @var13 nvarchar(128)
SELECT @var13 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'OfferedToSpouseConditional';
IF @var13 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var13 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [OfferedToSpouseConditional]
DECLARE @var14 nvarchar(128)
SELECT @var14 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'mainLand';
IF @var14 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var14 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [mainLand]
DECLARE @var15 nvarchar(128)
SELECT @var15 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'coverageEffectiveDate';
IF @var15 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var15 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [coverageEffectiveDate]
DECLARE @var16 nvarchar(128)
SELECT @var16 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'fullyPlusSelfInsured';
IF @var16 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var16 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [fullyPlusSelfInsured]
DECLARE @var17 nvarchar(128)
SELECT @var17 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'minValue';
IF @var17 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var17 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [minValue]
DECLARE @var18 nvarchar(128)
SELECT @var18 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'waitingPeriodID';
IF @var18 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var18 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [waitingPeriodID]
DECLARE @var19 nvarchar(128)
SELECT @var19 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'taxYear';
IF @var19 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var19 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [taxYear]
DECLARE @var20 nvarchar(128)
SELECT @var20 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'planYearID';
IF @var20 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var20 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [planYearID]
DECLARE @var21 nvarchar(128)
SELECT @var21 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'hireDate';
IF @var21 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var21 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [hireDate]
DECLARE @var22 nvarchar(128)
SELECT @var22 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'line14';
IF @var22 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var22 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [line14]
DECLARE @var23 nvarchar(128)
SELECT @var23 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'line15';
IF @var23 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var23 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [line15]
DECLARE @var24 nvarchar(128)
SELECT @var24 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Approved1095Final')
AND col_name(parent_object_id, parent_column_id) = 'line16';
IF @var24 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Approved1095Final] DROP CONSTRAINT [' + @var24 + ']')
ALTER TABLE [dbo].[Approved1095Final] DROP COLUMN [line16]
ALTER TABLE [dbo].[Approved1095FinalPart2] ADD CONSTRAINT [FK_dbo.Approved1095FinalPart2_dbo.Approved1095Final_Approved1095Final_ID] FOREIGN KEY ([Approved1095Final_ID]) REFERENCES [dbo].[Approved1095Final] ([Approved1095FinalId])
ALTER TABLE [dbo].[Approved1095FinalPart3] ADD CONSTRAINT [FK_dbo.Approved1095FinalPart3_dbo.Approved1095Final_Approved1095Final_ID] FOREIGN KEY ([Approved1095Final_ID]) REFERENCES [dbo].[Approved1095Final] ([Approved1095FinalId])
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201712271634532_00004-Afas-AFcomply', N'Afas.AfComply.Reporting.Migrations.Configuration',  0x1F8B0800000000000400ED5DD96E2339967D1F60FE41D0D34C036DD9292BBBAB6077C3E5A5CAE8F4022BAB06D32F095A41D981094528234299F6B7F5C37CD2FCC23076EE5B306459C9375B641C9297F71E2E97E4FDBF7FFDEFC9DF5F56D1E81B4CB330894FC7470787E3118C174910C64FA7E34DBEFCF35FC77FFFDBBFFFDBC965B07A19FDD1E49B16F9D09771763A7ECEF3F5CF9349B678862B901DACC2459A64C9323F5824AB090892C987C3C39F264747138820C6086B343A79D8C479B882E53FE8DFF3245EC075BE01D14D12C028AB7F4729F31275740B56305B83053C1D9F2D512167CBF364B58E5E0F1EE03A497354D9838B6405C2783C3A8B42806A3587D1723C02719CE4204775FEF9F70CCEF334899FE66BF403883EBFAE21CAB7045106EBB6FCDC65D76DD6E187A25993EE432BB18CDB06A3265F22D1E4AF45F5CA66A316AFD769F20D4447873F1DE33951DE7FC057E207F4D37D9AAC619ABF3EC065FDFDF5C5783421BF9BD01FB69F61DF1455407FC5F9C7E3F1E8761345E03182ADC430D1CEF32485BFC218A62087C13DC8739822095E07B06C0A533A5556D53E18FCF2DA94893A0A75E97874035E3EC1F8297F3E1DA33FC7A3ABF00506CD2F753D7E8F43A4AEE8A33CDD404E3DF5CABE8B9BB22F501B3E23DD34867A8059B24917F03A68A07EDD8481314CD5FD7324DA4DD6CA0329F3019E70196F56C6C8E7292CFA472AE6D9E11052AE4B2E44DB5BCC8821C265F826CD688AB668C7C9A4336B5D639F7963F7C6EE8D7DEF8D1D0685B15F853188EE419A7FD847B38768B696BC42489439FD60DC2B9FC1CB7F4390F603B949E2FCB9B35C3B904F610C8F8E1D90984631B3ED14F371F0623C6DAA4AF6B469499BD37DA4CDEB38DBA4002D8ECF517B53F0041F92EF7D29F4D211155FC0358C0310E73BC1E957619AE5C59FDB9F56DE8441104147652B681ABC5523E7F3DBC15B77913CB294E107143FA06C7D40D9C7B1A49E82A77DE9DAD554DE33F67B606CD342F314C2FC2C08529865DB2FFEBCB483ED371AE3A6ED15FBCF70BDFD42FD80AB2AF9071E7011D82DF8163E95E31505BB2EB6B6505F3FC0A84CCE9EC375DDE7CCF0FBA5C97C9526AB8724E20DD1759E2FF3521D51451345C6CF207D82B95985A726159E6A5478AA5BE129BFC256539AEB382C3C9E7E52A3C4E93DA941B55FA13E2C9AE68407C002545CD8B7628B086419B2EB4559B7BE68AB7207D50548F47A56ED6CFC86CC22EB0718C025D844F9DD7209D3BB65B36532F8146F0517839751376D0E96F037903E26E939FA74F0521BABC86EAAAE3A4FB2BC55E964837AC6DAD0CA4E8201DD49BF248813416C0D7B19A74914C1E03A76851C365B70054C3FFD4CAA367F4EE6EB6493F5AED91D09779EC44158183762FA9EC8C5499A4F200EFAE22CEA3EB844355DE4E137E884139728FBEB7DB4C98AB1B8DC2185BD6B8A48FB0F106D7A76F07710166791EE611A2641EFA1C4C5B6E83A027181D2B732CF61EAA6F3A2EDB8CDA2EDB8CD22EF36F38BAEF7B2CB799EA46941C349BCAF2709BD9D783BB1DA9CC056CBC7DC153F693B5FC8FCDDA25F928D59F7CBF29AEE55FC9184817EEDF1DCC2BA77995435C772F6DAB22870F695992A193938785801BDC5E9C90708B224F6731DCFE1BBCCE1C5E297C3800DB77CE97275CCC724328CC7E63065E87B24DC5C5EB53A0BA75E658AB85255722FEE2D21F6957CEF36F97A935F8511449F3E7B9F99A7B477456992895D6BB6C219293F07C325826C562CF70BC817CF8AEAE2197995EDD22555C532F527BFAA327BC87E0F705D0D2CFD76484B21151C7A962E9EC36F6F46A55F37305371D5D0853B98C7CF51E7F97B487E6879B3A1258A4A8BAE889E7FCCC18AAE1B2696D275C3E956B59DC96B3B53D476A653DB99AAB6C633EBBDBCCDE967D69EFEDE27FD61C7A3146C4266E5F1099E43385DA5B20D34B336223F71555DCDAC899DE2BD6440CF409E81866120D276843424C92677DCF424A4D6EBA3557B3CB7B0EE5D2655CDB19C2E5C4E7BC94CDEE5E45D4E9EC3DFDCE53493B99C664A97D36C4097D34CE8729AC95D4E33172EA742F65729FA731FC9D7D12325FD203CAF795EEB7F6CF0730AE26C1566D91E1F1CC4DB58C9AEA74E1080D8C581D26EE8C4CC181E69CDD70D7C808B240D3AEBDEDEC40C6F01871878C986255CA7196A5EB8CE9D34CFF3A4E7C9619C28C56CA850F75ADBF9AE098A42D1A295E6546C0AA6CCCC382DD45F18FB5B54070F981244E70FA41999F9A53C77BFE92625243F8CF9616CF861CC0F2DAA92FDD062BEB5C0103E6F8B419849C5BA3DB61CD8716DF8D150D91AE1F869358EFC9EC1F43208F3BD7DAAF52E0A88ABB0834DBB6FE1775705D9ECADF47D00D6C9B385C5BBAB7D2BD23EE8E80627ED8BE3C73C55C93FF09827E6D9CD0A6359B68FAFB3AB083C65580B029846AFA8C1B8AE92D5BE81AB479836236AF902C1785472CEE9F888692491FB3A0664FE0FF2FC17308279F10E419D7DCA36BB6AA0A4D19C0975EF56D78E53AC661FFF226FC95D1A3E558F48D41FFCE527F9070F701D81055CC1C295507FF3D70F3DDBDFD8BD836E2F42CB60ED57747C93FFBFC2FCF9324D93E2511A3D1578804FA4A4A7F2FCC8A8161035B630D9FA8B63F917B7497E956CE2AE84998E94CFB22C5984E5DC8073F68078708B2CFB320E46BAAF6F75F4CC3EA639BAD94479B88EC205B2E9D3F1E1C101DB071A65B5DB1E92B2AA591955E09F68296112B111D4D45450D32D0A6AAA2FA8A93B41C9EE478BEAAF7559BA6B02F57C81BCEADA05096555C75BA28A39225B838AB98B2BEA1F9D2DAAC051E7205B80801D43916D0676C2652F2F8A5A2AB9C9D8B5AFBD676D2042F105484DC1B1BA6C2000C13527516555779EBA1A77D71E0D64A1B82BB5CB9A2438D5AA6AA9E858192BC8998D200527D1F882644A7863410A2F8FF1DAC93B7CEB4C88BC6B0D14787D2E788704786C2240FEBD4067E6FCAE04485D1B57123775A0C7D170409E03D2E98A5E0381F82113CDB19FF7AAC900530CCE93281A127F133D921D33D66AAD7A70A06E01D88AF51D0E13EC19408545F176E919C53112A1F81CA1A6E07A192C75D2505949154BD9355DC9526E1B2D3EFBAEA9E7BC83F0039813E714BD86C4DFC490C41E2F515B35DC5F7C4FAFA950D5DEB35D6628B5334EBBDD62CFDC4082161F8C1116B81B63AEFCF48C66FBD5AB5B55DB0D84BDF36BDDCA61709EC43908637C6BBA0A0F7E01725024C21776102A3D0D30A71CEF47D5B5F8CE5BC16D27233F21D84C0136D304A3373BC5B0ECB6A84D0153ED02D8ED6E65015AD846B075AC031570131241054DCEEB79A8F49A4101D8CCFD7950DDBA4001D22EAE7828D8CA4B07A65CD1422150BDE0D5AC1057C9B1499689B4B958F4DC474FDA5CA86E7EA300692FAAF050BA5B2C4A188A51B9680C639B81721BCA8EB90A50E2880B0F913C0343C3610C2D31FD36C60B965DC4005CF7949983AA6D239F852616D0CD2028836E44488F6AA48CACE53735901FEDB532F35BB996DF545F7E5317F2933EE0CB8A50DB9F65ECD1C25ACB0C2E1229EAF8B01841B6B398DEE2E3BC9FC90A4DE1A7D2F45461ADC0064B8968C4BEA90105227A798F958A8EF3CAC47D85350A9F074804A470586D414AF4F3056229C976204D7C531C29CDF4A424D866E44B89C5EC25257CCA259391C867A2EF76EA231F8E9F84866B67950E0D4D4F38628792BE4BA98F896D5538F4DBC1124EE66CC0EA388AECD998DC74D51269FF711D7FDA5E39AA8BDC47860E2437233AC765A423FBBE225333B3B683C8D84524129C8288749C4203B235E75D07A1D989F6A9355D3FAC0228442376F66C43202A12E27A8174FC40F6725092901309481E3B52DA93C83B64E81F72634B1C8F908EEC2D4426B9C2C48A4CD3FB63E8FFC15AC6D94691084DEDF119D0DA346E4F690850EEE5B1F4F3B813A8D0B3232EC2C9D0A8B8D6AC14ACF61A4FCF9763D25A0BEF8D83855F739DA375BCB4692793399AE4AE40FDC3C90465294ED66F4074930430CA9A841BB05E87F153D67D59FF329AAFC1A2E0A83FCFC7A397551467A7E3E73C5FFF3C9964257476B00A17699225CBFC6091AC262048261F0E0F7F9A1C1D4D5615C66441EC7AD16EA2B6A43C29E35492A9053D06F02A4CB3BCF01F3D82E202C279B062B299B8999A2279DE26B6439B8DD7E6ABE2EFFACB256AFFD9F23C59ADA3D783B60607174911BD93745351B89D88AF50AB8B7B1BA500A0401FD8CF11C07C01229072AE299E27D16615B3AD2B6E83D11A2B466AF4B5B8DCC42256BF9BA3158FE1F1D08ADFF5D1F0FB6D381AFEBB3E1A79CD0DC7C353CC30B10B6E3820F6B3315675D58B835625E8E3E1D7D67038FC777334B67A640A8B7832A16C8036BE09637D8C679A346953839F0D65F0CC4CD2CCE0399F5B18FCCC1BBC37786FF04297A073D3678E7BD892801AC88C0E683C33D56E22BBD3D8F8EFFA68EDA57D1CAAFDD1440DEB1704480DAC7FD4C7F95487C3C6619ADF0C51661C14C6D7AF44F9C841F9E8C9D293E5DB9125F7E0825BB29CBA224B0E502FB29C9AA9F6759C6DD0D27F01CF1152B1A27D48BED3E588F21818A580922FAD28F902AE611C8038A7E18884ED537CB9EEBF2D4F76E148D8CF0686180641045930FC7703A206BC8A75BFEA23CDE7B72448F98341DF258F549F153FF8C1C20F1664FA76068B61C709174384C3D1C16A169D0A66D1A919C5BA9D937BA2D5F83E4F21CCCF82208519C56D5492011D8539CD6A21FB3C9CBC560C57D43FE963FC335C9308E50F7E08F14308993EF810D2DE1C19681069AE9CF41B4684283603490DB63F43490ED3151A1C0B3F25AB834CA23E2E5880DAF2A96A1209FA788B086419328845591B1A944DD5475E957B501460FBA3214EF47A562D117F43549A7130E90CFAF8015C824D94DF2D9730BD5B364B51B208511E8356C00555EBE207E35ACEC112FE06D2C7242D1CC2DC4AD259CC2D20BBA9247A9E6439DF16C81CE625948284015FD8C24CE6E55CC669124530B88EE545F1F2E9971636FB18D5C3E4780154923E665235FE73325F279B8C426512F571EFC84FCF9338080BDB2E8E3BE145C8F219683D1A7A3E81989A1075BF1AF054DD2F97A85EE573982CAB0AB2E897B1DC44D1EB7DB4C98A0335E5E614A46ACECF61208F30AE1F1A26E4D1FEAA8FF41D84C5F07E0FD33009689665120DC62DDE9E556EBE67B58E405C7C43570DFF5D1FED394C391DDEFDAA8F1471DC1C91B19B23E2B83922633747C4717344DECDA183E5971D6E961DF43501872B0EEA3ABAF9624305A0BBCE2071CC54D71B97372E6BE3EAAECA3834ABF669067383127FAA6B4A0D82998A76F17F692CD3F34F5D006016C94C999A80BEA45157BF797AF0F440A60F420FD8ED4387FCD03DBA624E10926F7519A2853053D3BB4DBEDEE4576104EF41116C96588A5269DE3CBD7992E9C399677341DAB581722F85EB5AA8E06323132D314CE7C2EBEA5A25694AF58FFA3865F985399FA58BE7F01BC7E2F9394C6AFA7503338E5D11091678F41C864830F056A2AEA5A19ADF3CBB797623D3079D7CB8BDFAD13DB0663FF9E871E9A385F0930F1D4C6F9E3B6B9EF4CDF9A1F6E56C0C550560B52F6768B2DEB8BC71F5DD97736B56ED239ED6FB723D4CA941F0FB729E1E3C3DD0E986F480BDB2EB901FBAE779CD0942F2AD2E43B410666ACA9E05B0BA9F48F77BECA7D24A2C6FCE8ECC9979AFC6A5552B5E2FD2316E2584B68D5348662A4CC4FC0DE9AB066CAA253273448E4DD547FE3D0EBF6EE0035C24694093039D66575F1E4DF0D2F5D1AFD30C552A5CE7748589044F8E9E1CC9F46D90A3DB2511F3DC7F6F72ECB144A2913C39EE22397A32F364664D6654B410874C46861931A731C5F7BA1C46C018FA38A28073F2BCFB551FE9167EE72075BF1A2E0D1D3C5DE3EA9D85E28919BA3ACD6F06A4D3DC17A390F0DF8DD152015AEAE78A4A2C4FAF86F44ABEB1AA78EAA08D9E64FF9E410B61F70A58F1DA2C255B5111CD7BB6AC48B5F89785E55D6329230536483DEA5A3F8EEBAAAEC241435ADFF6E2D7E83ABBDD44D1E9780922FA3E9A8E5CE8A77B1D69DEB4BFE64D75358F795249BB37A7EF48F3A683681EF7F9A877A279D2885D96F7612814B3BB2F9CBED488CAD5BF3B8F5D689D466832CB9AAA2FFA88ABDA5B493871C98CEF7460DFEADEDDE008581885AC9702B8A11C61B035CBBAC92EA4B8629761484514B6CDFCA0BF824A8487FA39DD238FCFB62304228F4A675949E9BD850169431497CEFCC82505A07DBC5222617E482307D302976AC00FE9D4530DB8DBC35B51033CB69C8512E09FBB5101368C5C1FD956404EBB9F0D9BF74E3BFFB85FE71F6B75BED930B0E39DCF8B99F8CE06003A70A2F9A4B1FED0C58C910852D647904EE78A4408B69D9C288ADAEC7AE9890795B45D78E218AE979D6CE8C81EDD35D892938D9CF91E179CD2E09996B701147348F9C97F95E0777A36A91125D485926C7B6AC109116A7CA25DBD2BA1A30AC280A0BBB32BC14666EC37D81876F62EEC4AD01154CD95453E1B31D2945D9C8DCC5CCE46065290EDCC46C8E8B2B6030E8EE17AB86163C8F69D8D0C31D4B02174DFE3402389A26B7D5E5032F0A8CE0572C4AE8C95BB0303913254B0651D750E3F6E5339D848C1F64AC2620DA12CA238C06E3AC4C92A473B3CF25E2891B5DF4481637A3D43DD113BED4CD10AFFEC48C95D2B0C13F299CED21E94AA7F69FF6F433ED7E1968938D065F38AA8CE65B3B23AF4331D7FB9CA321EA1BA7F4303787A3A9EBF66395C1D14190EE65FA3F32884C54CB8C97003E27009B3FC73F23F303E1D7F383CFA301E9D4521C8AA08DEE691A561B09A6459403CC18F9D721D91E313ABBC27FF808C86345DF100979CEFD91E3C99D020279456D0DF17553B1D3F864FDD3AE157883AAF3891770FF21CA6483AD7012CDB311E15B35CF0588412AF67BA138DD2AAB3785549F137902E9E41FA1F2BF0F29FD678C57DE80A2F4015CDCB43FE8650F821CB0A6A539EC20FCBB62E43981A43D2E72C2BD852B08648D8014B526CE3D10D78F904E3A7FCF9743C3BB405AECE32F694207ED0D26D2DC9239746D5C44F5BEA1AE2ACA72172C65E23439C7943F486F8831BA2FC58B18949AA0FD9EA1A270F695833C5A3DBD82B6D7BD9C21EA2BDF7610FD1842B56D04D9EB2D754B8503377501F9D4079E2DA4BE2D260832F9D7D364C20539D7E54387546859C53DF9654381D9E0A45E188ED4DE2D209B912D187DF94A3B140982EA67478284C17440B5C56AE0C8BE9A056651C62311D78DA57007BDADF06ED3B617C2764BFB5296FDA8F4EDD4C9C3DA3DA009171865D4056F186DD540E63827E5065FC6117407E00D9CB01C48AEEB9617BCD095F18B0D78CF231981F87F499F0BF3D959308FD6B5F2D36D8AF3DD6AA89F3DB13820EEB6B0F270AE1EB60982C43F93AC01104F07580CC8FDB5B012FA304D85B0213A4B731637B485E305E7B542A02AFBD02314177EDEB240BAE6B8FDA85D5B5C710C4D0ED4950FCB0B93D5ADA06CCB5EF4E26466E0F3EEFBFB38287C5B547E902E2F6ECB0C8DD4E7AE46E273DF23BE97E46EC66462CBBE8A4371956DDFD51CF83598461A7C05EE7F752E7A50BAC63E90EA2161A7515CF004BDB1AF95755F5EC507CB1536D81F8B7C3DA5E1754A367DF763135DC6C0C559135FC88EAD9C58C5D8C7D132C50774D69000F87E0B9033D4E91BC05A02615E2E36159850E4EE7F78ABD1DBFD12C817ADF6388598228F6AD814D0BA2DE6A1A75FBF5D0F3F47577F9D66E83841F40D70D3D6011749D023A981C3531747BC2780ADC4B0A349E3B589DD2973C22A53F77D8C2F97C3F77F086E3E4A0D2BB983BC89E12B0D8EFB3326E16C1EFF7796BDC396BA41EBB1870BFCFCA0EC54FA768EFF76DC1F6FC7E9F6797BD62975DDFEF1384C8D5E31449705C35A9101F0FCB2A4EEED8F501F046BF9746AF6F65D2C750348D4DF92A8886CD713086353D36C4634F4560233BDA6B3E1DCDD1C5548117C7D1BE86446C5B3FFBF04464B1B619EEFC81FC81302B5AB35A9FF0303CADED30AD791AFAA16848B1083256C041494D1273568FD114D166D574C6000CECD16883D93A986074F16C5DB08E83C74D1C5CDF6F62D9DA23E0316C7BA3A4FD503CF7EE25F78AF80D7B0C92B3ED4DC49125AB761907A38724E27DD054ADB896C206074542D84479B88EC205AACFE9F8F0E0E08869B804BD225E69117516B29C3F3185204E8669A1D5203A4FE22C4F41C806FD2DB6D7D00A0B44C2D6525F70495FE3527FD1476D61744AF5584911035D2A15ABAA08DE3B6D8BA4862795D48807466D546DBA53AA3655ABDAF4C753352666EFAEAB9A2C4CAC5C1FEA275E592D6812C8BE3F625E32BE8B2F600473383A2BCB477501D902042CDB176FF68AEA42DDACC16B43270DA88BBCE76EA57D2F7ADFD64803E5E193549A277ACF790B4AC706A075AA6A0A3A6BAF7FE078DD8F7BA726A2C86CD272DF4E39048166DF031975B700F08A60BFEE9D6E09C33CEE30FB0842D86A28D84CA460B32D2BD88CAB604C2DDC2B1813CC42A56082B00DC60A6652B03476CE56154C788780EA522C062BDEA7F5CF7BAC5A220109FA5776AB6240B5524693DDEAB0F80E956ABB03E25B2B95DE60B80B4A4505F575D48D3B38FFB6E89A6DCFBC65A103B7BF118085ED75D28F7BBAFC37EA617950E82197FDCAF8C6DB56B07738F7A6EE7408556D1F67E1F2D0983B3C1567830A3B5535BD916EC663C87D541351D4DD1DDD69A282083B5A19EDA04E58AC76B6AD0DBB34FB21C2043BE9C73D1D928C7A581E847AC8A148194F790B0A260E36FC1EE63ECC8955BC3E6CE2DE0D6CAA90BE3B3AC0A983176B76F8B1ACC3B7B5D6DB05255485E7E52A825E60DA6195513302F4F695F21DBAF97A19C71E38FDAC8CE02D7C7F9764BCE676AE47C5566694AE8EC6CD91DC78D41D17E42A6015A7F9741C3C26482DAA6387640E6665202C6D262F6DA62C6DA6591A7B4450502E9B515C0336AF4D5DA6BA75991AD4656A5E179D6A68D6C0A8F0267E86BCF83697AA026D464515E8CD53A6743A03AF603A8FA2CC6EEB9829AD4BE295D3A52A4AC03C164C11581AAF0C2C59A790DAC9C62FA54E141653A6C34CBB393C9EC0D264CD513304BD90946B82A0383A8F9E26F04AEB92249AA02EA1BBBFCF1681A5F1CA6893D51DC4CE05D8C2982CDC32995C6625F324C966D128592D59F21E105B2C95CE2B93CA4215888DF512BE6E0EE78FB0EC22DEE61EE4E7CC3E39C301AA8106C3CBB10849F101F9929890A2B016D3D4404CF421F4ED8A69AA16133BB69B8B4976289A2329ED33D492150DD32CFE7451BCFD476228865E7391B047763982509CEB75DC7C7ABE507E2D9C0B983759701095D36E9D23AB8E1BCF4C65CACFC5B314FBE653AE5A49F3654E5DC9D626BFF9CC4823F2C9709ACF8E527D9A8FCFD9648D171F3B129CCCA26B5EFFBC13CD3ED66CB6F838DA00CD1E4ED9A98350326EE3380BFB5476AB94263EE4A31ED6442782EC1BF0668399EC288A4A105B2439FED28B15890BBB670F4A886D40E4BC72DC7C7A1588EB92D326AB6C9E7B40A00F2F6FB59962D7B65AD5457E70FB06BC99828B1DB01C31687A6B1D2BBC68D15EA22817E20E44C26E33688846E50D903A4EC40D948F166F2C2AF592C0C0CBE57865D04BC812D1342F25B4EE9436ED6452EDE2D43FA07FF3A488EA79930430CACA5F4F260F9BB8788EA1FAEF0266E15307718230E3CADA3BD026CF75BC4C1A771255A3264B93DC3C07017310801C9CA579B8048B1C252F206A72FC341E95AFAC146F823C16115FAB57E95193E1EA317AC5855178A364E59F4C983A9FDCAD8BFF32174D40D50C8B172CEEE25F366114B4F5BEE2BC60218028DC5CF57337455FE6C5B3374FAF2DD26D126B02D5E26BBD739F8BC8B9082CBB8BE7E01B14D74D2D435262271721784AC12AAB31BAEFD1BF48FD82D5CBDFFE1F2E133A3ABBA50100 , N'6.0.2-21211')

