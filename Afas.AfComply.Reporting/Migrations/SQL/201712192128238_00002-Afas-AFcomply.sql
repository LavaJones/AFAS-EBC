IF object_id(N'[dbo].[FK_dbo.Correction1095_dbo.Void1095_Voided_ID]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[Correction1095] DROP CONSTRAINT [FK_dbo.Correction1095_dbo.Void1095_Voided_ID]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Voided_ID' AND object_id = object_id(N'[dbo].[Correction1095]', N'U'))
    DROP INDEX [IX_Voided_ID] ON [dbo].[Correction1095]
CREATE TABLE [dbo].[Approved1095Final] (
    [Approved1095FinalId] [bigint] NOT NULL IDENTITY,
    [employerID] [int] NOT NULL,
    [employeeID] [int] NOT NULL,
    [terminationDate] [datetime] NOT NULL,
    [acaStatusID] [int] NOT NULL,
    [classificationID] [int] NOT NULL,
    [monthID] [int] NOT NULL,
    [monthlyAverageHours] [int] NOT NULL,
    [defaultOfferOfCoverage] [nvarchar](max),
    [mec] [nvarchar](max),
    [defaultSafeHarborCode] [nvarchar](max),
    [employeesMonthlyCost] [float] NOT NULL,
    [employeeOfferedCoverage] [bit] NOT NULL,
    [employeeEnrolledInCoverage] [bit] NOT NULL,
    [insuranceType] [int] NOT NULL,
    [offeredToSpouse] [bit] NOT NULL,
    [OfferedToSpouseConditional] [bit] NOT NULL,
    [mainLand] [bit] NOT NULL,
    [coverageEffectiveDate] [datetime] NOT NULL,
    [fullyPlusSelfInsured] [bit] NOT NULL,
    [minValue] [int] NOT NULL,
    [waitingPeriodID] [int] NOT NULL,
    [taxYear] [int] NOT NULL,
    [planYearID] [int] NOT NULL,
    [hireDate] [datetime] NOT NULL,
    [line14] [nvarchar](max),
    [line15] [nvarchar](max),
    [line16] [nvarchar](max),
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Approved1095Final] PRIMARY KEY ([Approved1095FinalId])
)
CREATE TABLE [dbo].[Approved1095Initial] (
    [Approved1095InitialId] [bigint] NOT NULL IDENTITY,
    [employerID] [int] NOT NULL,
    [employeeID] [int] NOT NULL,
    [terminationDate] [datetime] NOT NULL,
    [acaStatusID] [int] NOT NULL,
    [classificationID] [int] NOT NULL,
    [monthID] [int] NOT NULL,
    [monthlyAverageHours] [int] NOT NULL,
    [defaultOfferOfCoverage] [nvarchar](max),
    [mec] [nvarchar](max),
    [defaultSafeHarborCode] [nvarchar](max),
    [employeesMonthlyCost] [float] NOT NULL,
    [employeeOfferedCoverage] [bit] NOT NULL,
    [employeeEnrolledInCoverage] [bit] NOT NULL,
    [insuranceType] [int] NOT NULL,
    [offeredToSpouse] [bit] NOT NULL,
    [OfferedToSpouseConditional] [bit] NOT NULL,
    [mainLand] [bit] NOT NULL,
    [coverageEffectiveDate] [datetime] NOT NULL,
    [fullyPlusSelfInsured] [bit] NOT NULL,
    [minValue] [int] NOT NULL,
    [waitingPeriodID] [int] NOT NULL,
    [taxYear] [int] NOT NULL,
    [planYearID] [int] NOT NULL,
    [hireDate] [datetime] NOT NULL,
    [line14] [nvarchar](max),
    [line15] [nvarchar](max),
    [line16] [nvarchar](max),
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Approved1095Initial] PRIMARY KEY ([Approved1095InitialId])
)
CREATE TABLE [dbo].[UserEditPart2] (
    [UserEditPart2Id] [bigint] NOT NULL IDENTITY,
    [OldValue] [nvarchar](max) NOT NULL,
    [NewValue] [nvarchar](max) NOT NULL,
    [MonthId] [int] NOT NULL,
    [LineId] [int] NOT NULL,
    [EmployeeId] [int] NOT NULL,
    [EmployerId] [int] NOT NULL,
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.UserEditPart2] PRIMARY KEY ([UserEditPart2Id])
)
ALTER TABLE [dbo].[Correction1095] ADD [Voided1095_ID] [bigint] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Transmission1094] ADD [TransmissionType] [int] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Transmission1095] ADD [TransmissionType] [int] NOT NULL DEFAULT 0
CREATE INDEX [IX_Voided1095_ID] ON [dbo].[Correction1095]([Voided1095_ID])
ALTER TABLE [dbo].[Correction1095] ADD CONSTRAINT [FK_dbo.Correction1095_dbo.Void1095_Voided1095_ID] FOREIGN KEY ([Voided1095_ID]) REFERENCES [dbo].[Void1095] ([Void1095Id]) ON DELETE CASCADE
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Correction1095')
AND col_name(parent_object_id, parent_column_id) = 'Voided_ID';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Correction1095] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[Correction1095] DROP COLUMN [Voided_ID]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Transmission1094')
AND col_name(parent_object_id, parent_column_id) = 'TranmissionType';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Transmission1094] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[Transmission1094] DROP COLUMN [TranmissionType]
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Transmission1095')
AND col_name(parent_object_id, parent_column_id) = 'TranmissionType';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Transmission1095] DROP CONSTRAINT [' + @var2 + ']')
ALTER TABLE [dbo].[Transmission1095] DROP COLUMN [TranmissionType]
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201712192128238_00002-Afas-AFcomply', N'Afas.AfComply.Reporting.Migrations.Configuration',  0x1F8B0800000000000400ED5DDB6E2BB9B17D0F907F10F4940488657BAC9DCCC04EE0F1656264BCBD61EF99833C0D68356537D2EAD674B71CFBDBF2703EE9FCC221FBCAFBADD9B2ACE19B2DB217C96255B1582CB2FEEFBFFF7BFAF7D755327981791167E9D9F4E8E0703A81E9228BE2F4E96CBA29977FFEEBF4EF7FFBFDEF4EAFA2D5EBE4E7B6DE37B81EFA322DCEA6CF65B9FE6E362B16CF70058A8355BCC8B3225B96078B6C350351363B3E3CFC767674348308628AB02693D3FB4D5AC62B58FD83FEBDC8D2055C971B90DC66114C8AE67754F250A14E3E83152CD66001CFA6E74BD4C8F9F2225BAD93B7837BB8CEF21275F6E0325B81389D4ECE9318A05E3DC064399D8034CD4A50A23E7FF753011FCA3C4B9F1ED6E807907C7D5B43546F0992023663F9AEAF6E3AACC3633CAC59FFA11359A6DD80D190AF1069CA37DCBD6AD868C4EB759EBD80E4E8F0DB13B226AAFB4FF846FD807EFA92676B98976FF770D97C7F73399DCCE8EF66EC87DD67C437B80BE8AFB4FC74329D7CDE2409784C60473182B40F6596C31F600A7350C2E80B284B98230ADE44B01A0AD73AD3563D3E187DFFD6B689260A4DE974720B5E7F84E953F97C36457F4E27D7F12B8CDA5F9A7EFC94C6885DD14765BE81827E9AB57D97B66D5FA2317C45BC690D750F8B6C932FE04DD442FDB089236B987AFA1F10693745470FC4CC0764C155BA5959235FE410CF8F92CCF3C331A8DCB48C493B98CC4843C4CBF85D86D136ED308ED3592FD6A6C23E0FC21E843D08FBDE0B3B8CB0B05FC72948F651E22132D4B23798536D7E736C3D210D0E1C8A837ABF42B4C643F3C2A360016A391DDAB145028A02F1DCA2EADB50B4559696CF5E4092B773B441004FF01F48E515C30023B8049BA4BC5B2E617E878CF81AD8C352A019075C8CDE4633B407B084FF00F963965FA04F476FB5958AE2B69EAA8BAC283B96CE3668669C05AD9A2418B193F47D962510A4CEB057699E25098C6E525FC8715A6C7280B6901866187F66F598BF660FEB6C530CEED91D0D87F6B9518C851B69FA81C878B3FB2348A3A1388B660EAE504F1765FC02BDE8C425AAFEF625D914D898B9C1D30307F71429ED9F41B21938C1FF013176177C81799C45839712F0FA2F08F26120EB04A4186568679EE3DCCFE425710A8F4E46D75C5533F3ED34F369F466C28640D772D810D86D086ED218FB29C396408B13B604614B10B604614B10B604614B10B604614B10B604614BB0375B828B2CCFB11ACED27D3DFF0F7212E44405F619BCC44F15A749CE732BC198DCC3A4AA543CC7EB66E669D9F985AE7F9D67ABFB2CE1448CAAF6CB43C59AA8D399BEEE57903FC1D2BCFF3F677164DE7BB2B6B4EF7D255DCF899AA27E1BEB278CB3AF9AA9A6918770811AE83D621EEE2128B234D83A4187EFB20EC79B5F81066C75CB2F7DAD5EF371859CC6E36BD86AE82F88B8A5BA6B4D1541BFAA1279A7EAE241BAB782D857E57BB729D79BF23A4E20FAF4F93D3467506941A58D619676622BB548C535385D22A9E6A4E5BE07E5E259D35DB2A2A8B37DB9A2AB44A5E1CAAFEECC1E6ABF7BB8AE1796611ED28A4858879EE78BE7F8E5DD54E9AF1B58E874D5D88D7BB0E31FD0E485E8E1B0B4BCDBD292249544D78ABEF0A7AE5B4DAC54D7AD4E77EAED5CDDDBB9A6B77393DECE75BDB5B6ACF7F20E46B0AC83FAFB98EA8F088FD26813BAAA489F9035A4E62A536D24CBDA4AF9C9BBEACBB2A63CC57BA90183060A1A681C0D44CB8E540D29AAA90F6E062AA4EED4C7A8F7646D69DFFB4ABA9E13357D1C39EDA5660A474EE1C829E8F0773F729AAB8E9CE6DA23A7F988474E73E991D35C7DE434F771E484697F9DA33FF751F90E8FDCAC42AE874104BD16F4DAF0B0C1AF39488B555C147B1C38488EB1A6DD409EA000898B0395DCB08585353CE29A5F37F01E2EB23CEAA57B7B86193902816210155BB67093176878F1BAF432BCA027839E1CE710055B4398DD1B6E171F4D302A146D5A599D4A9860DACADCA185FE0BEBF3165DE001D7822CFE405991B32FD5B587999B0C91C2321696B1F197B1B0B4E85A0E4B8BBD6B8153F8221783B4924EEB0E7039F0EBDAF8ABA17634D2F5D3691DF9A980F95514975F405E1EEFE322729744D455D8EDA9E3CFF03FEFD472E56CE975B49BBBE5C738854331AEDA7728FCE0E44371C2FAA56BF937BC7EC975E66645684C7E8E6F8AEB043C15C4082298276F68C024AFD2DDBE85AB4798B7AB63F59AC07452A98BB3E9113748AAF64D0AE8FAC7EAFA973081257E53A0A9FE0D3FEC7A808A410B8CE3C1A36E0E41899E7DFA8B7A247779FC543F08D17CF0976FD51FDCC37502167005F1B140F3CD5F8F078EBF957B0FD38E1F7727C6AF99F8B6FEFFC4E5F3559E67F881193316B8874F34A5BF51D74742B58068B058649B2F4ED45F7CCECAEB6C93F62DCC4DA87C5E14D922AED67951880DB30FA79BBF4AA389C535D55E493317C727B79BA48CD749BC40627D36FD13374EB3863A2F46DF10F53E3DD3CC113D1AD4CC5D5A0BEAE47C513FB47F018A0588788D872819519425A8A8262E7F6D4C3652C51DB27E7CDD0D570B12CAAF9E1912EEF0E0E088652D6302482E98C83AABBB6DD2F7B8BF7066410BCD2D955DE624493CA16EA4B2801E9E907317424A6280C484E45A7867424AAFED88C6290A7BF4464451403903DE4464EE10014F6C0828BE91E54D9C3F1401990BBB5AC5CD8452785A0EE8080C93A918B410C89F90305CFB45EF498C6062081EA330A0F8BBF0912AC0D368B4FAC58189BF7625EB075C26F8E82B8D4489FCA31CE35891501EC16548B84102CBC478693BA9D3526E43D76A29BF8396471D1BF2B9280479047112C42F1B50FC5D04497ED6201BABC1C183F88CCD96A8FA738B5DD650FA6310E371CBCF444622B43C2441DAE06EACB9EAB805C3F1EB77B7BAB15B107BE7F7BAB57BF7224B4B10A7A423B14EA778094A800BE12BBF08557E615832479E47F585E4DEB72C1C27473F29D85C0336370423B22BC911C9144C36B0CD1BED3AE0F629771D346D158B50598B5B03D85ACE22A8DEAAD680745B13110AB16F3181A9F683500AD46C170D3B246411C244B1A1B6108BB51CCCA82D84EAAD030D4817602F42E9A3EFB5308C3E12A271FACE0E5438507EC5D2805247F32244FAEC9E8523F49B4C90983037E20B837722594D6CE78FEF062B10EE991B74BB9610D08C0E6697049A4406E413BCBBC6134DE36537F4B313A3209495823472CFFA880491BDD8C453C5C4F56EE37C270645EA61058134EEF62D5089BDF62AA792CA7F62E3591750696E46258993444C251E731095C8254F452399C7D7DC693E843E022F2F0BD7ADEA1E05CD8C387277B8B9437C88886D9538EC9B930A9D2C701F99B8B9DDB531ED323222A90305144F226B577599F3DBD2FDED67451738BC4D683F94647ACD6CECDEB67670CB08A75144262EED11B5B5E03EB054EC645E3643C735CF001AD2C85DD5DB20884E09097DD8265E6C773A689590170A281EC9D0CA93CCB76DE9DDF6234B027FB609ED1D48A6087DE74966E8BBB6F45E1323136C631544D3FBAB47943683A87B0302AA7DD48E5E6A7F0495FAA5E54D78591A35D7E1B48435DEE39979A26D46EBE07BF6B0F16B43873BB77157763A7B4046EE0A343F9CCE50151CC5B901C96D16C1A4680B6EC17A1DA74F45FF65F3CBE4610D165847FDF9613A795D256971367D2ECBF577B35951411707AB78916745B62C0F16D96A06A26C767C78F8EDECE868B6AA31668B829C05D6C9DDB55466557E33BA14ABC7085EC7795162EFF723C0C1AE17D18AAB66E3246F9B14F9CAF9096D1D5FED57F8EFE6CB251AFFF9F2225BAD93B783AE07079719CEFA463BD919DC9EC4D768D43846B8220094F003FF390278588004E482EB2D1759B259A5FCE8F0CD039663E5482DBFE2407A1EB1FEDD1E0D3FA22442C3BF9BA39177294834F2777334FA4A05894796D8611297294840E2676BACFA5A8100AD2E30C723AF489070E4EFF6687CF7E8121EF174C6C8002B7C334EFAB873355AA46D057E3E96C07396A49DC00B3E7710F87910F820F041E04567CEDEA59E3CAC76157D25869DFC1350766C4C66012761C9DFADD1A0040DDAA17119C14948AED01C97CA0C4E625205E6787C827012942F3547EE92859380DD8F96386CBE700E93AD608E2F4B1F4E3621AB63310A9C2D9CEA35FEC1BA976C26704127D92AF61240E7FD16C9025DC3BE052E0DB8A811AE927D3BA2BCE0A2A644F5CC5B637285930D3045E6985CD27012952B34C755650F279B50D5B3E0FA2EA338C5FADDAF167A4A9C539C5256E22AE66D88338C934D886B58D0A3CB374ED1A3FBD51C89CB394E02728516EB569B7E9C5AAFDA1FCD71C80CE42414F9BB395A9F879CC4EA7F35476A13919338ED6F962873010A17ADA445F92440F914B61B61BBB1F5ED46178B3AD286A30D621DB6E590A2B86C3A1AB0B0EDD0E1866D47D876846D87693B61DB11B61D61DB11B61D61DB11B61D61DB21D876B081AF1E771CCC0537FBCD860EC0749F41E3D8B16E10AE205CCEC2D5077F7B14ABEEB2A7BD40C93F3515A516C18E45FB4C682C96ED897E9F0A8D47B263A636B5192DD4F56F413D04F540978FA21E88FB341EF5437F8DDB5E4128BE35D5101D841D9BB289ACA9AD285316C43388275D3E9E78B657FE7C0BA8F09AA3A9844A3EB612D10AC3D6165ED7178568516A7E34C7A9DAC7E27C9E2F9EE31781C48B6BD8F4F4D70D2C0472451538E0B1360C55608EF780A696856A7F0BDA2D6837BA7C54E3C36F3073FF648BBBF131208CB98308C687096610CF9D154FF62EE8587E391741D50138F9E52C4536085710AEA17E39BF62D53D0BE6EC971B204A2D42F0CB05F510D4035B6EA91E8877FB3CEA87FEC13F7B05A1F8D654437410766CCAC702D80602549142ECBCA7C194D6620571F624CEDC0B0C3EA55AF31E8789706B218C659C41B263613E79B10CBB2E7544E642E4F852736436813189CB96B9F557A42644E5E6E83779813A15AF4BB6C35441508E4139D2E5DB508E7EB744EA94EF15BAA5721CB04562918272DC45E51894595066CECA8C797FDCA32653241DAFA0756A4CF3BDA90EA3602CCF38BAACE2D4E946F7AB39529F259C44EA7FB5DC1AB292DEFD688ED326FE2661DADF2C940591FA9B5215C4EFD668B9042D0F369E162BA8454BB548BFF6A70AF466DE47748CE96650ECE2B7F1AB87ECBCEB7325F03435529C1494E8EA49956DA70519D6D3E61148C79EEA83D5E55D65DF7BB4661241B608EBB864E25BD3F8630181A5B92106310082F130F9D214188E7D5305552BBBD65DCA9BDC149F3749D22576371CF9607E9125D3B00F56D5A8126960AA607AD45933764481A87385387652197B3BA2DA90650BB10F1B62008C43841414163F343F9C0DE63ED940FCD0FE403610BA38B6C20664C60F0726203FF7C3027C728F21B4AD81BC4E3F9FCCE4834EFEC9B0C93F319A7CBB6560C7275F94C9E6832D006C3A1B7BA3B1F9D087C548A58E184248AFB622951863270D45D99807738722D58FEBC693C4F0BDEDE413FA0C98AED1B69C7C3EA38FB8E154A634728C68D5D890EAE8551DE177DA9A34C8DDE48349B66D5A081237594765EABD1226AC204DD3B43B5E093E5FCEB0C5C672B277C12BC1E6B5B26716B53562C529BB688DCC7D5A232331C876AC113AE797EB824362F85E6EF8CC5E43AD9131961A3EB1D9475C6814B9CD9C635E140B8F2EB64540766D06B31D5888B409DC1CFB6812C0B34DE6E0F3B7B933098F3506B3C8B2B3F999102FBB1CE3A4757BC144CEE7261A1CDB1063FD44ECF4618A51523E4F4CEE9B61B8447C6C952E68A0F9A5FBBF4BC4D724C1A3B2F355C3C3B9F6AA61154D423E362B5E5D653A417D7F410B787E367D782B4AB83AC0150E1E7E4D2E9218624BB8AD700BD278098BF26BF66F989E4D8F0F8F8EA793F32406459D57D13EDF1F8C56B3A288A867A48948AD09BD3EF1CC7BFA4FC871483B15F77029F89E9FC1D3190B72CA7005FB3DEEDAD9F4317EEAF7093F403479383AE50B284B9823EADC44B01AC77482AD5CF088133C3696EECCA0B53A2EA56E297D01F9E219E47F5881D73F3AE3E13B7D355E843A5A5681AA965064C0510DB5A92249E36AACCB18E6D6906CCC510D5B11D612890836A2C9369DDC82D71F61FA543E9F4DE787AEC0755CCF400A9241477E7B49871F5975938C3C3215C4F9404114ACBD5682380F821804F1372E88D2E46F36D2A84CFB662A920CC8B87249267870E75232B1833B0A97CB61205B52791CDCBBC5676E70C75AB5491B0642B0391ADCE164F918344ABACCF90075AEA7382F83071C4936060FC8E2240C35F032C980BB247019175A317687146556704765D229B833109741C1BD4FAA4C09EEA87D8E04770C494284810A4A9C0361C048BBEC07EED3C9253C18A0CFDB5C07EE10648E0377943EBBC1C0096BD31B78503D6D8E035F509FBC40055B38D8C2CAAC64F6D6B0341F999D3D4CC0048B3858C4C1220E1671B08883451C2CE26011078B3858C4A359C4AA18783363581716AEB7837984714DE0C0F37BC9F3CA0D561D3E42F395251A734BC302CB581AC5B798CCE4507EE7472F81E4B7E3CA5EFF66F0C0B9ED9F0CF671DEDA3E1C1C56D4A05DECB44B130A2BD306261CD347B09BA318AB14C94D58339DA2B826AA572AD4C7E36A1536F7861F9510E4F83724C7BEAC04E6EAF71856822CB597854C4B927A190A75F7F5D876FABABF97E5E62011E707F3A31E8804615E013D18476D8AB0103A1654E05045E31AC0A9785FC4DC76D842E866B01D82E0F8B01DE61FC27650DD3275F0F73909378F10FC7D411A774E1A997BD023FAFB9CE4507EABDED8DFB705D90BFEBEA05DF64ABBECBABF4F9201CC4CA728727FE9950AF5F1B85A656894419357CC1D2008FD5E0ABDB99429EFC91B0A9BF6C2B881CC0930C6153D3E83CD4046E013D7B8733E9BACC687A9204A53E3DE432A7557B03E822272D8DB8C177FA07E3BC649AD39ED4F441841ADEDB05A0B6AE837A586349B206B061C55A929526A9969344D322DBD3AE300463ED1E87275F950147DBE2E1F685DD22E77716E1376B9239089BA06A3E4C35082E6DC4BCD29D34EC42B5FA2D310E67D36BA77576934B9CF12C8BC72D5740C5F2939A00B6E374919AF9378813A72363DE21E73BB4B2F61024B3839AFDA477D01C502443C5DF0B365B2BE3011E4646FD822BA3F7FE29A412A17E698ED417291A5459983984F7D86BD6768030512112D98CA4275AE7EE20B8FB66B822DB9846B9862C1148FDDBC75E593765D2BCC24E8C843BD21A7663A3E079757563B3C3838E266B7C7ECC29C49BCFEC7BD631359720A65BBEFC71C925C5B1F4119F5D1AE6447885FF78EB7A4996E7658FB48B2781930D85CC660F32D33D85CC8605C2FFC33982837BA92C1242FD75A33984DC3CAE7C3B7CA60D25859664A893454E49C363FEF316BC90824995F55F4F0886CA54DA8B5D565F10332D57617C4F7662AB3C57017988AC96BE6691A77D0FE76989A6D5BDEAAEC295B600579E6322FF3B8A7DB7FAB1956E7C51B73DBAF4DF1B66D06FB80B63713BB2C65B57DB4C2D5D98176D814E7F3AA796535B3956E2ED290FBC826B2C4633BEA6962F2A879DA19ED204F38EC76B6CD0DBB64FD5099D2BCCCE39E2E495633ACCEC337E652A44D29B7050693E75BFB08B60F179945F6872FDCBB854D97D56C4717387DFE36C3093F514DF8B6F67ABBC084BA0C65424630CBCD352E331A26C1DB3E537EC063BE41C2B107877E4E42F01E677F5774CABACED663D2CB714CD7242414506E3AE9036B840C58A7AA3B9B468F19628B3A4087AEC1ED0CA4ADCDD5ADCDB5ADCD0D5B23D3F3C89AA4EAC8DBA5AA5934DEBE87AE6EBEABA5EB405751D305D649C8B5CE561035CCD6D1B4D9BB48B9D6FA22513B7DA9A605C233CF35419489DA208A4D1A690E93C4AD3485D266AA7258180F47240F44996A387A4960374C6A4E9034C7D631E304516B7D918213F42DF4F731F9268832511B5DB17E82F8358F6F8CCF1C2C6A93AB65D7B288927C158396F594A5E3BAF9669972519B4C15A6413A23AB41B8E684F842A8BD149696C2D6EA06AE5EC894C732048646593289680D48C207130A08A18938F43C7C56C3575F4BB5B7FD9025217282719B04D3791E3CB7F8549FCBD715F7E13387488AE1AB8E9B144E17F1F039DD20F3160B86CFEB9521C3275759D5E0E50111929811B6E7CDCF3B31EC13C361CB03654618F678CCCE8468A8749BE018634867B7AAD2E4E107FA654D16ABE03E80775BCC5487E43A426C51C9898D659E243EE49E3FC295CB80CCADEE79F8ACDD4EF292D721EB645E787439442F6F7598F243373DABCB4EE8DC07F06E0C2E3F1A1290C1F01CC933C3CBB659158A76EBE48124FCC6D080343A3FA5D2A52B1FA07AB5786752E9B70416FE77CF3B8341445690A6BDEDD8397ABBB2D359BDEF6E7E40FF9619CEAB759B453029AA5F4F67F79B145FA9ACFFBB8445FCD4439C22CCB496F61EB4AD73932EB3D6D1CDF4A8ADC25EE98425884009CEF3325E8245898A17100D397D9A4EAA6BCEF85EEF23CEB956BF0B8B860C578FC91B490CEC2757B57F3AE3FA7C7AB7C6FF153E8680BA19E35BA877E9F79B3889BA7E5F0B6EA14A20B003BEB9708EE7B2C417CF9FDE3AA4CF596A08D490AF3B37F88A73D721B0E22E7D002F50DE373D0D698A9D5EC6E02907ABA2C1E8BF47FF22F68B56AF7FFB7F25E4513DDC7E0100 , N'6.0.2-21211')

