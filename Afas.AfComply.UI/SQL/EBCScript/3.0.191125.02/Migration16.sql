CREATE TABLE [dbo].[FileCabinetAccess] (
    [FileCabinetAccessID] [bigint] NOT NULL IDENTITY,
    [OwnerResourceId] [uniqueidentifier] NOT NULL,
    [HasFiles] [bit] NOT NULL,
    [HasSubFolders] [bit] NOT NULL,
    [ApplicationId] [int] NOT NULL,
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [FileCabinetFolderInfo_ID] [bigint] NOT NULL,
    CONSTRAINT [PK_dbo.FileCabinetAccess] PRIMARY KEY ([FileCabinetAccessID])
)
CREATE INDEX [IX_FileCabinetFolderInfo_ID] ON [dbo].[FileCabinetAccess]([FileCabinetFolderInfo_ID])
CREATE TABLE [dbo].[FileCabinetFolderInfo] (
    [FileCabinetFolderInfoID] [bigint] NOT NULL IDENTITY,
    [FolderName] [nvarchar](max) NOT NULL,
    [FolderDepth] [int] NOT NULL,
    [ApplicationId] [int] NOT NULL,
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [ParentFolderId] [bigint],
    CONSTRAINT [PK_dbo.FileCabinetFolderInfo] PRIMARY KEY ([FileCabinetFolderInfoID])
)
CREATE INDEX [IX_ParentFolderId] ON [dbo].[FileCabinetFolderInfo]([ParentFolderId])
CREATE TABLE [dbo].[FileCabinetInfo] (
    [FileCabinetInfoID] [bigint] NOT NULL IDENTITY,
    [Filename] [nvarchar](256) NOT NULL,
    [FileDescription] [nvarchar](1000),
    [FileType] [nvarchar](max),
    [OwnerResourceId] [uniqueidentifier] NOT NULL,
    [ApplicationId] [int] NOT NULL,
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [ArchiveFileInfo_ArchiveFileInfoId] [bigint] NOT NULL,
    [FileCabinetFolderInfo_ID] [bigint],
    CONSTRAINT [PK_dbo.FileCabinetInfo] PRIMARY KEY ([FileCabinetInfoID])
)
CREATE INDEX [IX_ArchiveFileInfo_ArchiveFileInfoId] ON [dbo].[FileCabinetInfo]([ArchiveFileInfo_ArchiveFileInfoId])
CREATE INDEX [IX_FileCabinetFolderInfo_ID] ON [dbo].[FileCabinetInfo]([FileCabinetFolderInfo_ID])
CREATE TABLE [dbo].[SsisFileTransfers] (
    [ SSISFileTransferId] [bigint] NOT NULL IDENTITY,
    [FEIN] [nvarchar](50),
    [FileName] [nvarchar](max),
    [RunTime] [datetime] NOT NULL,
    [EmployerId] [int] NOT NULL,
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.SsisFileTransfers] PRIMARY KEY ([ SSISFileTransferId])
)
ALTER TABLE [dbo].[FileCabinetAccess] ADD CONSTRAINT [FK_dbo.FileCabinetAccess_dbo.FileCabinetFolderInfo_FileCabinetFolderInfo_ID] FOREIGN KEY ([FileCabinetFolderInfo_ID]) REFERENCES [dbo].[FileCabinetFolderInfo] ([FileCabinetFolderInfoID]) ON DELETE CASCADE
ALTER TABLE [dbo].[FileCabinetFolderInfo] ADD CONSTRAINT [FK_dbo.FileCabinetFolderInfo_dbo.FileCabinetFolderInfo_ParentFolderId] FOREIGN KEY ([ParentFolderId]) REFERENCES [dbo].[FileCabinetFolderInfo] ([FileCabinetFolderInfoID])
ALTER TABLE [dbo].[FileCabinetInfo] ADD CONSTRAINT [FK_dbo.FileCabinetInfo_dbo.ArchiveFileInfo_ArchiveFileInfo_ArchiveFileInfoId] FOREIGN KEY ([ArchiveFileInfo_ArchiveFileInfoId]) REFERENCES [dbo].[ArchiveFileInfo] ([ArchiveFileInfoId]) ON DELETE CASCADE
ALTER TABLE [dbo].[FileCabinetInfo] ADD CONSTRAINT [FK_dbo.FileCabinetInfo_dbo.FileCabinetFolderInfo_FileCabinetFolderInfo_ID] FOREIGN KEY ([FileCabinetFolderInfo_ID]) REFERENCES [dbo].[FileCabinetFolderInfo] ([FileCabinetFolderInfoID])
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201905222232571_00016-Afas-AFcomply', N'Afas.AfComply.Reporting.Migrations.Configuration',  0x1F8B0800000000000400ED5DDB72E3B8767D4F55FEC1A5A72455C7B25B569FCC94FB9CF258F6B427ED4B593D27957971C12424B342911A5EDCED6FCB433E29BF1080146FB8830025598D379B8016808D8D850D6063E3FFFEE77FCFFFFE7D151EBDC2240DE2E8D3E8F4F8647404232FF68368F96994678BBFFCFBE8EF7FFBE77F3ABFF257DF8FFE51E59BE07CE89751FA69F49265EB9FC7E3D47B812B901EAF022F89D378911D7BF16A0CFC78FCE1E4E4A7F1E9E918228811C23A3A3A7FCCA32C58C1E21FF4EF651C79709DE520BC8D7D18A69BEF28655EA01EDD81154CD7C0839F46170B54C8C5E2325EADC3B7E347B88E930C55F67816AF40108D8E2EC200A05ACD61B8181D81288A3390A13AFFFC7B0AE7591247CBF91A7D00E1D7B73544F916204CE1A62D3F37D9559B75F201376BDCFCB097584675835193AF9068B2375CBDA2D9A8C5EB7512BF42FFF4E4A7B3EB2002E10348B2D3F66FD0AFFE03BE753EA04F0F49BC8649F6F608171BA49BD9E868DCFDDD98FC61FDB3D66F7065D05F51F6F16C7474978721780E612DBB9690E7599CC05F6104139041FF0164194C902C6F7C58348A2A9D28EBC2F71398A65581A8BF50CF8E8E6EC1F72F305A662F9F46E8CFD1D175F01DFAD5974D257E8F02A4B5E8475992434625C5055F1695DB76A9B325B4D762695196DA282F278E32E065F8FF2D16F7F01247F02E5F3DC3641BA55EDDDC6DA3986D09718EC62F6C8DF3C9873ECAFC47B0BE44A50E5E613BC2D76CDF159A6FE23798DCF86672AA7066BF5C6CA577ABF22C15D6B3B58F308DF3C4838DF47ECD035F1BEE2649EDD24BEFF20BBED94105D28B3C7B8993200B5EE1D70444E92A48D3C2EE28ABF24B1C8710443D80D1003605F90ABEFF1704C643A4DDAEAFC8B40891E533BD8E93556A865BB09C69E5EC919CAE542C4A02A9D17299C025B6D22E42F86B12E76BD3CEB734C44BF3177755DE9845C8983F6E275C45F94ADFC84B206EEF2F222B687A32887959963C6B4DB2F8EFAFC14A1F0BAD908245B093665445F76BC71D780D96C53281B4FA59EB9B09EAFC471816F9D39760BD510276DE27CE12E9E83A89578F71C82D84F3C3A7AF2059C20CB531EEF3EB79310E0C0570A62180B3BE02E0FD504D00DC5FB304703E6E16B6FACBDDC9212E776F9129F1623A1FDD0651B0CA5757698A0B05E125925D0296F07EB140C66AE4071E40B53425F76B941D0FF38D3D87565D799419CEF2783EB388D7CC68C57466ADE96E5E9395ECE635755A3FDDE5B456F172BF69AD9A13ECB1FAD921B2FA4E772776B32C7614E92872271469DFF055A248A9D96C4A91D31AF3C3215224DC587D9D32FB9890E57E93A1016DC30AFF1244F0F46CF0CDDCA298E9768AF9387831C51205FAE606BA07835754453C702E9DB9EFE6B2C1E632332E3FC84D8C9B28CD131079B0DA78788CBF99F2FA95A5F96106D730F24194EDC544731D24E98ECEAD6E03DF0FB7739AFC05ECAA91F3F916CEE4E3679A32643341128721F47F03C6877415D4357CB605750B8C37C82AA88BB535A85BF0660BEAB7DC9AD87FCB436BB2CA97B6A0E6D0F8E8AE82BAF7325B5077F1AB2DA819F49C51E78CBA3D34EA0ED19EB362EAD4CE527B620856389678C05972EFC192D3F60E4A20CC7E3017E48EE3E7F68AFD2358EFA0AD68A62E56AB36B69C9C3D212EF907B62744071E6BBCA52FF5EC29AD8BA72A33F32CA39347746CD1CDC83AA1905558EE8BD52A63A250E1896A8527EC0AF7B2D86EA200FBC71CA2CD062D995BB6CE6550ED57A80F71D3ACF000F040C985A615F34290A6685C7B45DD4CD156C5C9910D90F0EDA2DC3CFD8C8685A16FAF0F17200FB3E270E57E51EDCA0E6E2DAE9A55FA60656C9A36070BF81924CF71B295EB26D5A8486FCBAEBA8CD37A9B6416E7A8677A0FB4CD0918D9497DCD920AB6DA39B9896C2107D52E3F8631D3CFB86CF3D778BE8EF3D4B866F75DB8CB38F2033CB881F15E1DBE4FFA0544C6C6A2B7E9832B54530FDFDBB0C2890B94FDED21CC538B662D22ED7F803037ECE06F20C037721F6012C4BEF15462633B621D82A8B8986258999720B1D379E176DC05C2EDB80B84DB7117708B2E59C93FF0A24B7D4990782F8883AF8310DE448BB8CF728080C0FAA8BB3A60406CCD3FD5F245D672101A0CC88D2CFCB2970DB5174B54B269F961FAD1021B950B585C1CEA8197C18B6B29CC36CB7B84206D2E970E569AA37659C98EDAE5D47E192709B6B0E308BBE51EE2468F1B276E9C183BDA333773BB63E7A99BBFD9CF1564A3B674457975B7A1FF1107BE7AEDDBB9B9756F32C96ADECA69B41B8D710E95994A19DD47C663AF04128EBCC16E31496C1DB78C751CBE730EC7FB9A0C06ACB8E5A9C9D5301F9548311E9D4397A11F90703371D5365918F52A52F8952A938DB8B7803854F2BDCFB3759E29AC0DDDFD4F4769FB476902C3AE1EB65C8B949D83E2124EB65E2CF70BC8BC174975DB1959956DD205556D653227BFB23207C87E8F705D4E2C66875F859030876EB6BE7643A5752D76E379F908FFCC612AA3CAA10BB7B08C9823DDB100636BB3DC8A57350E7D5CCBC858DDFD457971962D6E37E1BB097F88093F0C0B862BA75FB65F61AF49940C58269C697BD5762AAEED5452DBA94A6DA746B5DD9CA3E1D9835FD512BB9B97A82D9D856DA330F25959A34D0FD14A716B3447D9EF93B25B3ED41206EC666571603B0777E143641B688DA645D8FCAADA5AA375CE1C0E92011D0339061A8681BA63874B43826CE223404342AACF0F956ADFCECDAD7B934956F3564E1B879707C94CEEF0D21D5E3A0EDFF9E1E554747839951E5E4E073CBC9C720F2FA7E2C3CBA98DC34BBC6ABC04CF4104B30BCFC3D7D30F9084EFBF45D6DE89F90C522CB39A5BFA6E92229C79FE7C1D873E4C8CC1907686D5CD43C3ED6447C3B2921D0D3389AEC524A55617772058C44771CE13E7B70D232AFE84A24AD5DFD9E2D056ED0F9047CBD6EDE6F8B02C7B06D7CD0E67CFA32EC7958E2B77CC95DE4B10FA098C64F4D862A8E6274C5664E41491212BBB2D0E3C58F6434D8CC4DCA77A65A9C7CDB3194CBD2458678170297E7A72A2360EE405E232065FF4DBB4CC1DB13B62DF35B193578025FC5E1030F51B26C1B3B28A189E995F77F3A2974D5F94A463D1F37F206DE000D6FC3C0D8A157EF154E80226073995895F12521C72F239C4D2524142D67964E57EB72D873C3779C84AFE81270F651AC258D7091E3F07C83F969ED671E3D48DD39D8FD3D68BE2877A09ACF36ABA8DA9B603D85E67E2714326A6DAF0486BFECCE123F4E2C46F46F716DF536FB580410CAC64CD126E9214352F5867569AE778D2F1E4301EF9F8981AABFB46DBD97EEE04854E9F284E6DAD1BA599290F78F92F4C5E55576911FF8AA13023B5FA15E7365AF5924272D3989BC6869FC6DCD4222BD94D2DFA3E5F14E1B37CBFB89964AC6BE00B46CF6BC3CF86D2D670E7CF5EF3C8EF294CAEFC203BD80786EF43BF13C87830B3FB0E7EDB4E41565E29B6720B193F0E6C2976A82D1CB71BECA6B83DD865C2B4FA085F03F80DFA87C8AA369F86DBA3A15F76D86E2E6D94655BB879E2184C56B263300683E5AB167FD17D7C935E876099B65AE0C3247C430D6E0FB96EB56FE1EA1926D512A078F062745418499F46A754233BB96F22D0CDFF419C7F0643588442D9649FD0CD2E1B2868346307C0B8D59B2B78AD9A7DFCABB825F749B02CDF2CD9FCE0AF3F897FF008D721F0E00AE24B299BDFFCFB07C3F657E3DE42B77B70DD6EBFA4E3ABFCFF19642F574912E3CB166A2AF008975D494FC4F9D1A0C21EE6C590DDFCE24CFC8BBB38BB8EF3A82961AA22E58B348DBDA0987619FB91C50B6778F53579627E3E25847B15F947D43B697294665DD910391B00F1571E6601F6CD4314F069F46F944CFA56A1DEE89554E194AC0221035485FBA81CF04717C5F55634CE40EA019F664ED4237EA7875ABDD1A393CEAC74120F45B993CE8C3B895B8577DB49DD97121584C1793691D9EECD23CF44934F8E8F693653284B2CE3692DE30F926E3616D4445750932D0A6AA22E28196969084A14FD9C577FA550E84D1388C7093486B24A1CF5F73170E920C5BC360B2216372DADE3A96B08931FE8585B84B47E6B888213D894576D5994D3A6EE4DA0630DA948A2A3BE0FEDE2C42192B59917088416E9B48F4839B143748874C7D2E4468F6535961533C99A245911F408F04D38A77D112023DA9EB099A2D07B2A2D95CA5110B3AFAD92E46D8741984F43B1D80193ADB1DEBB522C229EBE74A623E253589A3FBB612D54BAC2486BF82FBC289A4DACE75E06B0CE186FC528487C277A248A9AA5D45AF9CC4904B5EB2BD6F73A87D2716D24C38AE5E040698F961CF9B171CC96727DA4A0C457543C1D4BED97F215056B8FAF3A41DD14359E15E16D8081C5080FA720F19D8C26C9957B5E8355EFDF37ADE65C8BD4DADD50BCC6AF5B6AFF2D0ED5782E0A0D520BEEC26CD9267C9586BEEA0687D195E8AE559979BB584118E2ABC64C212828945669FA0B87BD10766FBD57B9023D88E0ED6ABCD1BCC677DBE43546C18793EDAEAC3BB3C95D40F7DE6094BB952A379EEF633A90B4F9573CB805EEC73A487C0F44B1FDF28D5959DB3584FD8EB6694B9F92CB38CA0062A596F7429C6441B49C810CE044F89D5E1814CE2830139DCC9E2253A2716D91B49D92AE420113557CDAED4009FF4CB901B49782A080D669A2A480CEB9639F0226CA056889A8FC9912B616EC4D1464811C78934D0EDDB57798B0A4492481EC6E28B110C9CD2A0960B5E9C4826A36A42420F5AE1E0BA5B5E5A702536CA5422ED066A755B142535185A67AD26662914B6D356933A19AE5B40484B162A2C01879D451DBD6A108B99D4F1D5D8EAB844805F9A121E92C12CC3A62074BA44D380F290C312133D1A8095F0F94A942B4C92601EDDCF56121762F03A9C0956ECB3CACDA039E846A5908B25997E77075D4C210CEC64A1E5B1D13AB9F635D2D1EBEFD30B6556665E6C9CA3CA59960DC957DDF7EE13AC2A9F68B9A279D5C46525F3A998CCE28191914BAAB8EE93ABF89FB40E028C76B39DB558EDDC8DA5C53132ADB334E045DD1D740F29B68C88FF49F5368E46438F94DD4E537B1213F91AB1C4384CA9E75A24355E6D2BAD55ACA64164851C59B6E2B0399768663884FE231C73E9AA736D85AED692D060442E27BC96D4534BC57BF69F9A8B8D1717C3E644AD55EF1084425719DDBAABCC8A7AFF8F2121DF7339BC739EB67C86BAA262FCE99BE163D1A89AABDD614098AE7A5C46915C34FC944480CCF2412AE5E4EDB110EEB81588E7C64DE6D749B04FE6D8ACD52C2646913B5536391A6D47489EFF1C66114355D5226A8ADEA12E1A5269ADB187E21ECD9A8EB19D27F56EBFA822889D4DC526AB9ACC9ED249E7F9BC8946178B8D9B191183E6D2AB23715997C3653F6601379C0C8E6356A875055703B9AE1188FA971C71EEFF896ED42A560574AE4C3F746DB9A546474C474536337424647AAC290D2110D643EB6DACF8C4A4716CF7D4DA4FC0C07363BA38AE1B2A622FB1E22933DF242CB4DC7478DE7F221F2526B355469975EA70C8630D54E028C042B7E5C4C2861753F369E18943CD9D8F2609FB9F42A683782673F6B2114B8DC574DCB7F4C2C601D7516BAA70DBB08507C53432E57033556704CB324E39D2BB0206C202D624567354D77B5562B1927760261CA1DD486B68214C2162A4851EC94D6D32DCD9E54B98E68FC22AC2C5E24F184A58255DEC354733DD3696D0F6733AB1B9B5580A2DA4FAC4E3B1FCFBD17B8029B0FE7639405C78AC941781BFB304CAB845BB05E07D1326D7EB9F972345F030F5B917F998F8EBEAFC228FD347AC9B2F5CFE3715A40A7C7ABC04BE2345E64C75EBC1A033F1E7F3839F9697C7A3A5E951863AF73E6437AB5D525657102969048C506AC8FE69C24CDB0BBDB33C021752EFD15954DC72BAE2A52EC1C47777275605FFD1EFFBDC15820495C2C2EE3D53A7C3BAEEB723C8B57A87E3CAF3AA28446ECD74812383A51211428D5161A0841CD3D1082E4818E78771987F92A12B5FD86F23F10615EF8C802C6D65C07B8FAA88E7319E0287A6D90F28B3AC26C099995697FD742A3AB547FD4C3C17AEA657785770C09D74EEB83FAF01247F02EC7519778E09D2C5A6514CF6811A0C5372D1466C37BB418C7D8A281365FB590FE08D6985828ACFABB3A1A25224DF9B4833176605ADFF5D166BF5CD052A712F571F9A07D11DBD11759B8ED7475F49B24E58E3A32AD0F6A31A478B09B440DDCF422CF5EE224C011FCDA360451023F9B4E5948CF49DCE2933AC6269A29D965ADCF1A58ED208271068A2735AEE36445B0B8289F7A69055990F5AE3FAAE33009A4077B085BDCBB95485196CB042E71ECCE8B10FE9AC4F99A5225460EF51278A3B6DF68ED864EEDB0402B450FB31534B56354349FB5B166D4F4D34950C76B87426DC3B5BFEBA3D1D5EBA6D088E763C2E2248DDE3165F512CB12D2A8EE6F724F06B7B8A912FA5ADC0C20238B7BA2A7D9753CFB6E576F3E6AE00451B0CA5757698A635883F012550CAFC1EE170BBCE7E4E3B7A663C2A854FD8D7A2DAEF330C4CEE35570EBCB38C7874CED42395934595650002B5D630554336941A31CC9F17339D675ACDB4DDF1EEB9E0DBFD1416DC7F5A55D069011ED9E690E174B0B3D7B4B2847078E0E6CD041E79AC03074D0BE2B6B440742A01E74D0C2D3536D583DC64160B7BF6BAFA899EBE95DD886F8A5A2D3B32E4CF54D1365CA40A1EEDE49513E32503EEAA014162A2424537FD4215D0F06AF485BB1F25C92C4DB4D7354EEA8BC9BBE3D2A675EB5B24BE5860B6A219011956B2EA86FA2344F40E4C16A35FB187F23CBE1E5D1B6FFA809E3AAD78431836B18F920CA48B84EC2F627A0E2AC9636715B9F7536287C3F649C1EB5BF6B4C238055B1E6ABC626F29C5809141F34FA2E7E26FA0C7FD0A1F7240E43E8FF062292DD5B09FA78D7F0998D5724E8E3DD92FAD449D0C7BB5873F08A843EF57BE3D54F6BC2A8C59EF3FA23EFD51FBFE5210F8F0AAFA224BF7CC9911F4ED0C79BC3351BAF48D0C7BBF732365E91A08F7717BFB2F18A047DBC19F4D878458233F69CB1D74DDF8EB137AC9D67C3C4B368DD699E2D5B326A6A5F0CB6E996ECD210AC7E25F1A380FDC8C599730ABFCF120833A6EB1991B44D6F3886B394B6A7D41F0131C1171F34EA00C345B17022F75D3A096EE2741367377DF089B30E6538D0D459C540349B3CB9287DA6CF0D58AF4D6E6AE283BD263EBB5BE6194C56C824C01EF1B40E5289EAB8C0039B914F54B393A08EE785204DD180F08ADA90A074AA3AF2AAD8D72700EB8F9A38E1DB45B9B1F5195169CAC02433A8E3FB7001F2302B36DBEF17D5065AB7085E1E8D5690ABB395DEAA6C53833958C0CF20798E13DAC18F93457F04A4B7A5442FE334638F856E0EFD1236271B6C617333E997532D816F227151AC7CEAA505D5EE2B66E36E0144923A665C36FE6B3C5FC7794AA05289DA074DD54F2FE3C80FF0D806C4668E289F86D6A3A9E70B880883A8F9AAC1539B7EB942F52A9E12A759959345BD8C451E866F0F619E722D43760E0D790451F1D634218FFAAB3AD23710E0E9FD012641EC932C4B256ACC5BAC4569A6BF285D87202ADCAD89AAB5BFABA3BD0409A3C39BAFEA4821E3E838D43E3A0E1947C7A1F6D171C8383A0EB58F8EDDB2C32D3BFA2F3BC87BF336971CE278022ACB0D1902AF1F881F92438391DC63BBCDEA95A85FF3808357A66838FC6EE2A5619F6466B337293ADB6CAC8DB1E6ABC6964BC148F8970F207B21765D8834ED16B361A9446DDC470852F2B21391E4D8DAB175377D10B62643BD59246BE2E9097DAE9601A8EE0A7571F454D70D2E37B87A0FAE26DCA1C561553FC3A23FA0F83F551D4A15829E8A9651D8EE231AABFCAA8B44AA53F3556760D353B09B7B95B01C3DD8A1875604598BFCD03CB0A44F1082DFAA32440DA1A7A6F779B6CE33B6BD4DA6B9E1E98667377DB8E15905B9B63D409981BD554728E7C75A43B4C0D0B585D76540DCEE50DA7C54C729CAC7C379B3E4A5473C3B478F12E83D062249A7F57FE630658CD54E420F3CD22EEA24E8F89E441909557DDBD556942D7F343CA26AB110DB26DD240D1DF117E5F525BA07882437DFB8F9A69B3EA839381DC41CA4A2716B98838CDFEA9A8353670EAA60BAE1B9B7C3938C423FD44E699F812A03E8B553AA3964DDE07283CB74A7D4EEB0AA9F50EEBD536A30942A04B753EAE8C1D10399AE490FF45B1A1669420AAEC0170A18AAC44141E9F9B5DD7F8BF83166A94475DCCFA0783C9D186DCD572DA479FE5CBEF440C3B593343608D6EBB07229273708BA498EB11C6375D387662CE10B2D36588B5F801E7389707AB05703A7C760E5EFE89DDBF6775DB4195C93DB089D04C734624CC734EF816906E4187376B1C62B3D1805FD38A2F9A4FEAA873483A99704EB8C0A574F25EAE1D2577D9AAFBBB7FF1CEF39DEDB2BDE9BA741B1F628DE4958C0C426F1C9B015984F0EA14A7D47F3F9CDBC0DA519D1828AE37BAD19C8D79EB3FE631ED17708EA8FBB3AA57634E468A8370D61D54525AEECFA08D5A83D9847F05B55CAA921F4D494F6F5E815CE97ECF7C89DF24AB1DC70B6349CA9E72F6D8E6AC963A82A835B0AA13CC609248357ACA8299D4EED894C2D89E85475E4DFA3E0CF1C3E422F4E7C921CC8B47EF565D1042B5D1DFD264951A5D08292AC7027C191A323C76EFA36C8D1EE693D096E4E8E06A7F7249223C77D244747668ECC7A93D9EF294CAEFC20B3FF0C4C07B9078D497EAFCA611D184DF7DBD06704156ABEAA23DDC16F0CA4E6ABE6D2D0C24B2FB6EE24E01759C8EA54DFB477B2B8516C771948C4D1ABA357237A7D84AF01FC669D5B4B58E8F7A456FECF7598B542D96980EE3DA58D5232F43DB9E6BB3E1A7D4BAEF9EE28CD515A37DD8CD22ED234F682E2405BF12DE727E6E7D391E90BCE5CDC7EEF3623E6F5E9737DF5729FBE8264093306F12AF1261B94353E700FD535B5DD88322C98D546301FE0E237E27CCC5431432D3C1B480BF9B8FD9EB155ED405EB9EF4A0BB98D18400B997BF8DBD1C2F2B592A775B99836787FA586E8F726AAB85B3A45D8EB810D2CCB29ADBF0A75EB6A4DE525EFBF0AEB5B876C3EBA49EFF230FC345A80908C24AD229781346F62AE791355CDD39C5B3B45BC1FCD9B0CA2799AB3E55E695E37986087DC35944F88A2170791D19702748B73A70DAD13D5D46C90C8833E0E38315661F2360DC211EEB5E3FBB57EAB1AC78F21600ACD8A02D8A11CBA6E665D2E0A4E688B5D8621953A685A5F3EE10128077863740F1B73CF08845349334512C6B01B9036EA6029EDC64C7BA80109A01CD84520E12EA645B3C0A61A1095B4A306CCD3FFADA8412BDA5B1F2568FFDC8E0A3488860AD002B2DAFD2DDC77DCF96503AAE0FAD8D35EB7FF5908EAB1047922A65135D540ED15851259E149853E6AC26883054DE18653B460786809C5A22D62C43E674AECA36787EC39FB9CD9679F9D2D5C1ECAA09EFAAB96CD0F6D2C590A281B3D6D75B15222EEF34A85D766DB7B1F653021B39D8F3686ED7D8F06DB50895A40F6F73C5AE0EF78C7A31B39AEEFFA4588A217F44E26F8BD5ECE886A6A5149B66DDB5631D10CB6C5A6F26D311555A0D0F66F5B6C6A795B4CB3B3F7615BAC96444F6B642AB146B434651FAD91A94D6B642005D98E35326DE6518309A78D617BBA69B06D5923434C352DF0773CD130A3503DA19573E82730D2D00F1950AF685A8C5E109763BA980408A3C235DF4591D4D54CB995E387592026A1586CEA6019C7F189AD1C06812A7988DAD129C5BD2C2A6C88EEB6416BAAD5371B594AB13AB7C373456BC8D76D7B87FAA29134237B89BB8455C4203BC935B8FDCD645973AC69963E035AD72B8BCC25C2B3AF63EF95B30495DFA15EA9CEACAA1232D653F29A769F8D050186EEFD72469F72D1F768C3815F47335D53B9443F208951CDA262A71828098D3584B290A5182A0D053784F250851C9212F576D092E0E886F99177C45E7B6D89EB6A5561AC1F845497C7D04C98A19E850999A5BE9DB6F952FF9F561F706F8325BC8D7D18A6CDEFE6DE0B5C81A259E91A78C5268A8F0CCA24CD662003CF20856596D111AAFB6B8026D64FA3F95B9AC1D531CE703CFF33BC0C0388773CAB0CB7200A1630CDBEC6FF0DA34FA30F27A71F464717610052FC3A60B8181D7D5F8511FAE725CBD63F8FC76951407ABC0ABC244EE34576ECC5AB31F0E3F18793939FC6A7A763E8AFC669EA87EDFE69DDE815DD8539EDF6E9F97F404A57AA4E79840B21126BC14EC29D139AC247C215FF347A0E96CD6EF1AF10752DBE24F900B20C264876373E2C5A393AC2860F780E616DFC8CC5E5FA7E8217DD9B62A25780960A20F99715F8FEAFDA6097450D6C20CD9650B5665942C76A60C029D54D150B0F2F2FC3FF5B867C7889237897AF9E61620BB908A86A07CA6683F1FDE31AAC506D7D15F923586316B25225153129D5AB7D31BD7FEB2A94D92F17D6A45E612A016AD5B27D45BDC4CD8BB04C41414B8B006BB326F44D92EA0C334DCC629C59024D2FF2EC254ED0CAF315B627F886BAF5BBFF2645EA6D02B089DB60A6849D985D7106426CE45EC7C92A35412D46BE59C55407BE5A2BADB50CA9C27299C0259E9A2F42F86B12E76B934E1C605891C11FFA37B615F5A1DB0BA3A35BF0FD0B8C96D9CBA7D1F4A42FF0AC353FF8E8EFAC0847A789D58EFE60B796DD38105AD56C8780D0375427D60C55C655C79E86EA647843B58EA6D55F636F832858E5ABAB34C57500E1256A075EEBDC2F70E0FAC8C7EF44C489C980BD46D971D8C42AD4CD659C57BE213D1918739335B4869D0A6AB2D264C75107C951FD57A80352DF9935EA63ECBCF4A4BEB3E1A9CFFAD2C49E45ED46BF1BFDC38E7E6E3C15EDD12F8C2EA235FA09A461473FACC2E6CD8C0C892A8E9F81F9646E81E118A4A7675636330AA8A93DA88F56A00A5312FA6606950783D7205A6245BB74A69923671D726E0597E11133ADC766BC6CBA2015C6DEE9C9CB5B5890DE44699E80C883D532F231FE66C6D15756987E06D730F24194ED7CC2284E07EDD99AB781EF87F6CE20BE009B959BCF2D9DB2C4CF223A50DAE68F92380CA1FF1B30DA82AE60AEE1B30D985B60B4CD50C15CACADC0DC82371B30BFE55644FC5B1E5A914DBEB4013387469BD615CCBD97D980B98B5F6DC0CCA0E74C29674AEDB12965C58AB262400D6F3B59302EEA63F63D30B92A940158C2D9507D80B204C26C4F5D9B3A8E2F66507F046B4B7582E1A258CF986E5DB859F20067C95E73DA4D14E03356D3596D03633AAFB560B6B25B6B382DD9D9F3454D59A1F91C7B0B5B514EE081CD4034AA9617823445BAE8153533C35A157BD2E610E1DB45B989F3191198910F900F17200FB36223F87E51ED0D59996D57CD1AC6086753C53958C0CF20798E136B4E9495DEA6B7A5502FE3B45E042EC218F41F099B8D75529E7D66A90AB25A1ADE443650836A3B10439828505CB6F46B3C5FC7796A54A7FB2E547D3F0E186D37E03B235F40646429781B895FA11A7AD871D20A412D50F6B787304F2DD933883D374F01F6EFCE6F20C0776D1E6012C4BE219F9BAF9AD621880ABF50A38ABC04899D0E0BED9D0786F6CE03437BE781CE22FEC12D622254411F6B581E55556A09D310037B2C59BD03F16B1ED81B3E55C4DAF2756B43E5C41265ED7F7454F3C3F4A32E6D94B70C313AEA8B17DBE82D751810FE1182B4B904610BDC112AA1B31FF07D4AE805F88EC6A7D15FDF09BFEAD55A996E455159D5D85616A8544EB634C2B05CEB46C4419A18C2FDAC33E1A994121A113758034B7934B2E36AAB8D437E146AF9086CFF76D8B1570AF13E32EEDB128856C0BE5ED9ACA9D72D601CBBF0C13A417BD4CFBB69A026A6EA00A7E69CB719D43845F070819C543A3F1E9655EEF36C9D673CF3DC5DD470E3786B5602F118C9105602EFA51F8D31CD79E0467150D7BF1EDA4E5F3791C2FBED4717D5C5BCB05962DBA3871AD99E37C72344F4923287B521A0057B6B8E3AD1028C9D7D2E0BDE5038C2572D1F2315F317E5051499989DF12801FEA1261DC93360035A8253134B9011B14FDD129C3A4BD00DCA7D1E94E4C33FFB6E098A5EB1E8B17BDB6B70D3086EF7D68DC6BD1B8DC43B2B03EEDEF61A87FC577B94776FB730F6DCEEAD63978362977DDFBD95BCF5A2C62D0AEF9AC849860932B02DFF2D1A2450E66790E2D6A44DFDF58722C298E7CFE57B084640480FC3CAB1DB881C1C73FD20CCC57F8F6308A346E10D346D1612BD05A6C5445DA061D9A82CCBDE6E6F893783EB669BA2D73EA6E30FC71F7AFB12C44B79C39A2D16E8C202516C89225081118320442E9CCAC033987A49B0CE02898BE8E9C9C989AE0D8BF131A49525D450669B633AC774BD0E5944CF061AED29E95B62069C3A4F8362D95284985F201DED41AA47F3F9CDBC0DD267BB89093230B13202BA0A344D95F2948C39A55DA33CB2723BC2CEE9B8E3B783E43765A6C0AA789D60DDEE4111F58FFB5043E7C7C3528295D8AC6E94B951D67B94091FF5531C6CD2D7ED14C61C0363D8A1D779E4C6C6B4D7017C330B06F07B31081FA117277E230AC3F76E5AF52B47A7490D6F9214550FADE414AAE78EAC24C0874B44C203F1E1AE20891FBAED456BBD0EB559188ED6F698D61C0DFD5034243939D756C04149EDF71426577E90F57E14A303D087CE2880818FCE43BF1387C7C8C0B883DFEC81597813C382F73F7EC0C24AA80D3B286EC3C751AF21BD3DC2D7007E837E5F76AB7EDF97DCDABF1FD84EB3160A794F066F29387BCE90259E052F4DC72B3F14AF5CA469EC05C5012B63C5D9BC14F3C47EDA8C609EABC83F7A8C431ECA6955691C79F09897E5360FB3001FFBA2EA7E1A1145A042EEA3190C61068F2E8A1B08A8F340EA019F961E6AB2AF55BF89BC7E13B27EFF46158B381626D5DBA9519A2500A93A4DC841E4056B108AE545FC8CC9E4FC47E7C664A9E3BA5832A57C0F07D55A2C1FDBF5A98B257A4E26C3F3714B717BE8F3D90FA1CF67F2FA9DFDC8FA7CF6CEF579F3F6C61AAF2F492F71A66A6CDED0E0684595DA558893E3E3534A2704E8E57A5B58C426CBD08AC77A3144DEC7BC2742FAAA1BF7694ED5AA3CD17E703B55B5C95EA91A77CE9E6E73CEDE3755D39EA977AD6ADD78809DD9F9DDCEC9449CC576BDC8A41F620E16C59D54A8C7EE94B30A95F7546D830FAF92122AAC8306B6919B8F3F843AB103270A6BB04B86AB43A31D06B93531E6DA556A7DFD21749013694FA10ABB57C4695B11C9337F6B36DDC0EA3765AADFF4A02D3E4E501F85D2F741E9B891EC88CE2DB350BDBBF97CC04AC61310A77F45B1FD06542B22D2CDEE94AAAC433BB6169FCA8877263A4446A669D963DA1A6B8B90442F67707A4EFA5486BED6A86BAB2C06DA8E4DB377484EDB35C0764D4E6A86D63E9053BD682C2A235627F56EDCC375618FAE61FBBD0FB7FA6BE2B5ECC3E656F3A800572BB4FAF140B7AFB47A98FF12C2D05B54C41311BB57B077BDAE23A21B7295EEB05778A2188F0A55D8F9A437D5DA291DE0A8A80ECBC7E0CFC3561D76404261E1BBDC15AD1546CD4A525C89EFA176F4585D575760B6A50DFB64254D1B2B893F8569F5E3814E585A3DCC8F383AF4F4448462DD8982B14377782F41E8A312B85AC68E86D6EE684E0E2D4E62626074FD922C69964214384E672B867DD3D239BE7C94A88D887FB56BFD2BC3693EA904DAB3AE880311211DB69453B32AF5C7525A5628579D9AEC056DB2222A0DB1DB3EBC8A8A86CE809CBAFBFD7A61983EB51AECCDBE3DA9993BA153F5797D473AB7B79468568FDD112219A660F0CD8E81E8900AFBD0AE149D78C09B26E20018C24AEC72F3845244497C226ED79F89BA7E5BE70DFBA08E62017214411ED1697865648655D807A53C107747A30173A0CE8FBD06CB2E7C20CBABAFE83719FA054CEAFD461F99B0499ACD40069E414AFB0D15D796612694E1E8A8B9582B51D4B9F70257E0D3C87FC666607955979797B22715EA3251ADCB44A32E937E753953ADCB99465DCE74EAD2B96A26AA4B27A3A42E9DBC7DEA22E9A3A9461F4DFBF5516562CAABA15803ADC26FA2008F5F49F1752E5905EA8CB22A90FB0D74F1640E66D1642649B1A49F04552A9981552899475266E3254295D624B1CA69522525B49C93A8225A69AC325AC92A856CFCE9D8A56C12B9C514E9F8C51FC5E64C05CD998A9B33D5D30456496406A926C8CB6C4EC2789AC029A7499594C0D8D5A58A62E46195C9C8A65E787B77465481763E4925DA59D52B22AF8252E14AC552C1E1E972E92CAC82C95C72D137D1A6E9325B69ACC2EA64855228B39A2E8CCAC22C93CAA557326B04D159144A968FA86ED43ABA58229D55269145A1C03A8E14BBBC2699575C9583EED396452EB35E7921398E5A1842AB5629A687D2AAB31692AA812E479DC85169336EDC155F5FD172A39DA88A562D5CCAAE447B2647A587BC9168BB8137C4521404E990ED11B35BC5B6BAC5581D3A6103B2E9C29698261A6222034C6C574CDC812A5A6FE98B4914F0802129E5F8085B1884EC554D812659ABE88B89BE7ACF108EE47EFE1644422EBA0A1CEE824A5F0C9CCBE30C59A85C33DF8240A8356201C45FFEF51709E1EE2E1089C8317E308EA156971D41D006A18920DACB629118F897B838F7DCC89A6F3EEFBED98CCB9FBC96CBEE89763580BD5F54F6BF7813C84C8846DCA0D6FFFC5B8E03F4FF70E39FB85F279A1618BEE52695DD2AF3F3EF8EC9AD04DE45B3FE0DD8991D20BAE12413C44E789FBDE1470BC70615D2B76EF8A381E7AD329820C85DC8B67E596DBC8C0798774C4C26ADAD36937F3B42AEFEBCAB14FD1BB0335597F8F03364A1E3F5DF6996707FB9689DD276B11CB3F4F2D7C535139ED0015D2C4575DFF56D8AB37BFE40E2718E148C44C87486168B4EEE3F3D8815CA39A02085645FCB045EB90A82DA330DDB82E8F89EA50C7129BAA10E36B1F30E490A28E9C18705E1D0C73A0A4292392D097DBEF80D145BCA3B16957CD748C3696F0B9B4746821788AB7A04A1F604ABD3CEC7E5D1D6E603FA378B13B084B7B10FC3B4F87A3E7ECC23FCD242F9DF0CA6C1B2813847985169E934A0551ECC0E95271C51A32A4B955CBDF40033E0830C5C2459B0005E8692F1041644CBD151F12E137E89E419FA37D17D9EADF30C3519AE9EC3B7B630B0239DA8FCF33155E7F3FB35FE2FB5D10454CD003F4E711FFD92235BABAEF735E3710A0E04F6D0DB3C2283FB32C38FC92CDF6AA4BB385204DA88AF762CFC0A57EB1081A5F7D11CBC427EDDE432EC4AEC7C1680650256E906A3F93DFA17A99FBFFAFEB7FF070A7AB87389870200 , N'6.0.2-21211')
