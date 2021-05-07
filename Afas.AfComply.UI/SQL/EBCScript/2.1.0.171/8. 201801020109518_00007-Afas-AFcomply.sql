USE [aca]
GO

ALTER TABLE [dbo].[UserEditPart2] ALTER COLUMN [NewValue] [nvarchar](max)
CREATE TABLE [dbo].[UserRevieweds] (
    [UserReviewedId] [bigint] NOT NULL IDENTITY,
    [TaxYear] [int] NOT NULL,
    [EmployeeId] [int] NOT NULL,
    [EmployerId] [int] NOT NULL,
    [ReviewedBy] [nvarchar](max) NOT NULL,
    [ReviewedOn] [datetime] NOT NULL,
    [ResourceId] [uniqueidentifier] NOT NULL,
    [EntityStatusId] [int] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.UserRevieweds] PRIMARY KEY ([UserReviewedId])
)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201801020109518_00007-Afas-AFcomply', N'Afas.AfComply.Reporting.Migrations.Configuration',  0x1F8B0800000000000400ED5DDB6EE3389ABE5F60DFC1F0D5EE001327E5B867BA91CC20954357309503E2542F766E1A8C4427C2CA925B9253C9B3CDC53ED2BEC252678A675294EDB8789558A43EFEFCF59F78F8C9FFFBD7FF9EFCFD6D198E5E61920671743A3E3A381C8F60E4C57E103D9F8ED7D9E2CF7F1DFFFD6FFFFE6F2797FEF26DF45B5D6F9AD7436F46E9E9F825CB56BF4C26A9F70297203D58065E12A7F1223BF0E2E504F8F1E4D3E1E1CF93A3A30944106384351A9D3CACA32C58C2E207FA791E471E5C656B10DEC43E0CD3EA392A9917A8A35BB084E90A78F0747CB6408D9C2DCEE3E52A7C3F7880AB38C910B10717F11204D17874160600513587E1623C02511467204334FFF22D85F32C89A3E7F90A3D00E1E3FB0AA27A0B10A6B0EACB2F6D75D56E1D7ECABB35695F3462CBB8E930EAF225624DF69E9357741BF578B54AE257101E1DFE7C8CD74475FF01DF3B0FD0A3FB245EC1247B7F808BEAFDEB8BF168D27D6F42BED8BC86BD939380FE8BB29F8EC7A3DB751882A710361CC3583BCFE204FE0A2398800CFAF720CB60823878EDC3A22B54EB445B65FFA0FFF9BD6E137D28F449C7A31BF0F61546CFD9CBE918FD3B1E5D056FD0AF9F54747C8B0224AEE8A52C5943069D6A6DDF4575DB17A80F8F4836B5A11E601AAF130F5EFB35D4AFEBC0D786293FFF1CB1769D36FC40C27C80175C46EBA536F27902F3EF2364F3EC70082E572DE7ACEDCD6664218245B0956ED44D9BF5E316BC06CF85C210B008029CC72BD49F07181615D29760557D755CF97F6F6B5E25F1F2210E09EBD054F87D5E8822223216D57A04C933CCBA949E4C5A03A46096A09F435E051108F7D1365D222F13BFC3A4D3E6F493BE565738B68CC4F56D3F7A3EAFD32082699AFFFA8AFE3BDABCE9AD4938F3FD04FD29A8787CCBB647C8399287DBE5F6DAFF36CF8D3B3CF7B749C23F83D5360840116006BCECFE258EE0ED7A0B1FC1796F59CB3FB0F7D6F489859B9DEDA33B74A1BA53F67D57761BA1FA4C16AACF9442F599FD507DE64275351C680BC792B1B90A9234CBFFDDBCE5BD097C3F8496DA1637F5156CAB93F3F9ED161ACD1208B36A04B485A8BB50ABCD771A33CA9B6B168D6CB6D057E418AEA3749DC046FB3FC7C8CC83C8052D2E68D94CD0B20249F62915842C5860F07B5D998C5AE83A9CC085519115BBC8089EEA103C552078AA4AF0944DB079B0759F73611F232E6829527A046FFF0D41D20FE4268EB297D6369A81149390C7C347397933B3CD34F3D3E0CD38C7246BF907764CFDCCE6741FCD66110F82C883E7A8BF0978860FF1F75D19B45EC0158C7C10653B61D3DD98F7238C792512153FD126C33914E75036E950AEA320DFF3B58FDE045A9AF6B415CC23EA97C87DE75DB322A5C003A5A6F625CC0B419A22A9F30ADAFAA22D8BE1860D90F0FDAC0C03BE20A397F603F4E102ACC3EC6EB180C9DDA28E2F06B7F14BE80DDE46D5B53958C02F20798A9373F4EAE0ADD65A91DE949FEA3C4E9B1D2A17F11A7D1963452B3E12F4C98F643A3D58C35E46491C86D0BF8E6C210775BC9AC3F493CFB8ECF3633C5FC5EBB43765775DB8F338F2835CB991A5EF899CEF25FE0AA2DE93B65EF50D2E11A55E16BC422B367181AABFDF87EBD4E2F43232DABF8170DDF3037F0741BE1BFB1E2641ECF7762536C610AB1044394A5F625E82C4CEC70B3733C7146E668E2974734C6E48F0518604E77192E466388EF63597C2E989D313A345427C373F73E5ADAB3BBF77EBB78B6F826AD4FA9BA8AEEE9AE16F71E0AB538FD7E6D2DE5692518ED5ECB57498E3ECAB652A796461F36609B48D1DA80F10A471E4621D67C377D986E7835F8605AC6DCBEF6DADD6F2518594C5A36BE85AE87BC4DC4C4C5A5585415751C227AA2CEE657B0B887D35BE77EB6CB5CEAE8210A2575F5C768D33691FCAA40902BB466DB91129BB06654B38D58CACDC6790792F1272F18A2C62DB7201A958A5FEC6AF24660FADDF035C958EA5DF0C69C1A4DC869E25DE4BF0BA3553FAC71AA6325B3574E316E2F839FA782E97CBB996ADB996302C34BA34F4ECEDC646E6BAB6C442735DDB74236A67626A67126A672AD4CE64D46A47D67B9911EB226B67FE3EA6F9C3B64749AC49B72ACB9EE035B8E12A516DA0C85ACBF8F149B5155977668AF7D2023A0BE42CD03016A8AB3B5C3324A8265EB8E969909A551F25EAF1DA5CDADB4A32CAB19A36969CF6D232B92527B7E4E46CF8D6979C66A225A79974C96936E092D38CBBE434132F39CD6C2C39E5BCBF4AD0BFFB687C2D65F4F6837076CDD9B5FEDB061F1310A5CB204DF778E320DEC792773D65A20388250E147A4316A6DAF0486AFE58C307E8C589DF6AF7E60233BC070CC3C02AD66CE13A4951F7825566A57BCE4E3A3B39CC224A1E0DE5E25E493B7B698230A168D04ADA542C049356A6162DE46F68AFB7C8361E502DF0F61F082B52F1A5B876BF709360927363CE8D0DEFC69C6B91B5EC5C8BFED40265F059530CDC4A32ABDB63CA81F66BC37B43696FB8FED3C88F7C4B6172E907D9DE9E6B7617FA9D54D8C1C2EE5BF87D330D59391CCDCA913EF999647D09690E3BB28393F4C5712E4ED6F20FECE2B4CCEA037C0DE077E8EFA355B5623E764FF5CB0FB69DB5C1B26DB789D659B0ED58B0F512B35FF437BE4EAF42F09C623DF06112BEA30EE32AD725FB062E9F60520F018A2353C6A322483A1D1F519DECD4BE8E40B7FE2771FD0B18C22C3F38A5AA3EA5BB5D7650D069C60C40EF5E573B3D30CA7EFA8BB8277749F05C9E7A53BDF0979FC52F3CC055083CB884F9DA67F5CE5F3FF5EC7FADF7163E7B7E1B28D67FC987AFEBFF5790BD5C26499C9FA2A526020FF0B9CBE9A9B83E522A0FA2CEE62A5BBD712C7EE336CEAEE275D4B63053E1F2599AC65E50B85D6234DCBD07B0DBF065E48F942E056C0D73E76ED1D1CD3ACC8255187848914FC77FA27A266BA09996251BC06F09245A39EA7602B57217959A393AF3CAAB58CF41EA019F367188757E879518DB54B8C938F75CDC5FD121E8CC2ECF985D3E3C38A0255AA12D317BF1C3C6C55FB237A3A6BA8C9A6E90515375464D6D338ABCFC47417D58370131F57366A89F8C4B8474BEC156F45374D808AFCB4A278FB41D27CE02D2E0ADCAB1258A26762BCCA54F02E0F554702C40DBBFE6D0120D16F24F1350641C6D193418C0C919E6112B4B206E296ECF10D0E08524F178972589932222EB296F8F36CD482D9B27C93351B4AA5B662437139BD54F56268B3526B2720409F02AC966871878ACC3407692BD3575FE500C24CE60911A6E6277AC2577D0DD54ABF2297A3902FEA9608ABE9F7544D8002106E37C31058E6F3F7253F40B4A093C1CB66A193795EC9F5D7613F4867A8946B196BC29C1D162217F53BE22E37A292CB16D5F4AA4CC4A99755D6AA5EC769A9F48A628E7ACACB201D4899192A6C0F1AD28127FFB08AFAF0A7B49D8DBA674992ADF8AB2CB164ABEB345B9DFFC6D2E03319ABFCB94DBE06EF85CF15654C5FECB47B7B2BE6B307BE7C7BAE562D6791C652088F0659338C9CFB72F67D4A20CBED14EA85805831963DE3E1D8FDA9534663F29FEB1C03A93E91CC04E1D25D4D28848489CE99058CE27F2113B938EBAB0F7D53DA572EC6A46DCA481A97203F4BA91A881EA3A22197A7D6B910CBA3B5A60A192231109603DA26041B5A30D0948336463A160E3391598629C0CB940D5305A9120A69063A19B0EB799586444A5C66D26541B3549409A5C52164A9B682A8521EC34138DF2037AA0CC8ED29E5C02DAD985CA42EC6E5355812B37D4F0B09ABD592414E642D876BF5D0F1A6175190E80B370A4B1B4DB748BF23C135DC8DA29539022DF32E932439951AC2BA879BC922DDBEA2DDCB2BBD7F82E29DB04EBB422E85A6C07E2DF54837FE46AAEDE7AAE6DFE4DD5F937B5C73F72E156ACA8BC155E8D355EB6A2CE941595B1AAABF5250C3825BC0182E697F21AAEF62A2ED6512AF411704F65DD56D57E1AB08F71003BCD34C9DAACE2EA2CD60B2C9413B086BF1E3B2043784737D35C5159B0D559B2C53A8547A9020649166937C025F2FC2B3E9744B3EE3AEBB10C2E49EC93640556D5E6F5E2123E2010F188B74EA8BED4DA873F8CB54112AE19F358543435E6F01751D59751FBA8D84699435E3E21B0C98C450795C551736BDC5D685062697FBF8EDF8D24F5EABC2553CD45533B1E9DB14CAAC2FBBE2C935B66E54551ED65511EE3248648652174406BCD38188CAB76BCB519C5E54E5A0024ACE12F706E82213223C45CF95459FB34E783D40859E180E0B44CA93EF1564435D744EDE81263155485F7062C13E4C0D32C535CF1D45CF3C47AC698E413304DBECA39A0B629A4DF2B3050BCB269B8B6698FA1DCD54C7E13565CA3E45C1C296395C7786AEB973ABD3558B1B430F0ABD3EB9AC5C6A6EC64324741EE12540F4E26A84A9EE9B406E14DECC330AD0B6EC06A1544CF69FB66F564345F012FB7517F9E8F476FCB304A4FC72F59B6FA6532490BE8F4601978499CC68BECC08B9713E0C7934F87873F4F8E8E26CB1263E275E607C9A5D1A6A52C2E2E3AEF96E6E6D18757419266F97CD513C813C2CEFD25554D6769B56E92B5C24A7FD07A2ABF7E2BFFBF7A7381FA7FB6388F97ABF0FDA0A1E0E022CEAF7FEF2ECD12B82D8BAF50AFF33CBA820190230FF4EB0860EE811024F77446F6791CAE9711DDBB6B6AED418454CB6B9E6C4A2396CFF5D1F26463165AFE5C1D0DCF37C6D1F0E7EA68DDB4631C0F2FD1C3C4128E7140ECB13656997ACB402B0BD4F1F034621C0E7FAE8F4693D72DA1114F26840E90CA37A1B48FDA8DD155690D85C757A1AC2BBD005C59F38518AAEA4F1FEA207ABB39D58040C19FEBA3F1949555AE817E7D4BC0E50FD4DFFFBC4E91E748D3FC577EA8CC51178D51AC8F7DE6FB09FA53BCFF88FC11B305AA927E3BE748726E976CF8BA4C1FF5DB3C377CF0DC670363C526D8FF0C567CE4AA50C344E6918097DDBFC411BC5D13ACA00A9DA3718EA65B3EA0A3A9C79DC34496D494855E64C978DD20B29CB9C8D229BC5378D6B6896142CB9985D09287A1A7FF1894A66A0C116B420E1A34439344AED0CC2814B319B7C56E4A1C147BACA12281EF879006C39FABA37D052CC2DAA7EA48F339119D170F34DECF1208B32A342690BA451AC62EC8489B19D0C7CE89A9A22C51F5481D0385B65D84E281060D305C5C47E93A818450760A9CC3730EAF5BBE198777CFDB4F6BCFEB318E0D2EDAD0767D1C2063FF57E0E98936E4B82D68E4B69A133271A8E6A18E1856A7FD7625B07AA8E14EF2298D63C29954CF3451660C146A8FBF14E52706CA4FCE583A63B93D63C9DC3C6FD7584E6D194B06502F6339D513ED22B8019107CF1152BE56F8107FA7E6BB3975B63592B8802B18F920CA48B84EC1E64DBC1B83287CBBF889F866F903E72C9CB3E8960FEE2C9A74DF813C459D27DCCF4D70514C7C440566144D53534AD0684AC96E6C9EC16489FC5EBE7D879641AA501D1778A0D27C82CC4E813A9E178234450AE115D490A074A93AF2B218401080CD434D9CF0FDACF4EF5F90294D19986405757C1F2EC03ACCEE160B98DC2DEA38A2DB04AF8E462FA047509D3FD0A6720E16F00B489EE224DF27C52492ACA2AF01E94DC9D1F338CDD8BAD0ADA1DF42C148E8B399CDADA4DFCE6594C46108FDEB48DC14AB9E7A6B411D84E6D6B8DB0051A48E19979D7F8CE7AB789D12A854A13AEE5DF7D5F338F2835CB7F35DC07813A27A1A528F5CCF5710110151FB54C34E55DFE512D1559CDA4F5B554E15F53616EB307CBF0FD72977CE955D43831F41545DE0D4E147F3541DE93B0872F77E0F9320F6492B4B156AF82DD68023D31F70AC4210E5EF90A4E1CFD5D15E8284F1C1DBA7EA4821638E2AD49EA30A197354A1F61C55C898A30ADD1C950A961B76D8197690D97316471CC41942FA830D1980EA38A38BA327BA4EB99C72192B579B416A51AD9AF3B4F4158AFFAAAA2AD5087A225AA6B891FBB5DAA7BA48A438B54F75141BA471442A75F9CC9907671EBAE58398072C29DFA27D684FCAD337108277552D4403A127A677EB6CB5CEAE8210DE83EC85188A12654E3D9D7A76CB8753CFFADC10DB0ACA3C2B455543392F6BA96881A11B0BAFCAD306BAAA543D54C729DACFD5F92CF15E825786C6B36BE850FAC71AA60CBDEA1418E091314CA740674F5F949150F53367DD9C75EB960F1A7CD84D54694FC5350F3E7AA4A834102EF850C174EAB9B3EA491E2833D4BC9C89A2CA008CE6E53455D6299753AEBEF37276D5AA3979DD785EAE872AD5086E5ECE9907671EC8724DF3805D8D60D13EB4772AE81B08C1BBAA16A281D013537A2F8051720923B5C4A9B353E7CDA833758C9B4DAD961CEAA7A2DC5208651D2790F444187F3BB7167CECB2D41099DA224797AA237F8B823FD6F0017A71E293C6812C33A397652658E51A692D498A880A56194970A7C01947671CBBE59B308E768744D41D4DBD8D638F211289E48CE32E1A4767CC9C31333666C4156F162D59F76E387D3326795FD586756034D738429FB1F3BC7DAA8E740BBF3390DAA79A43430BE70ED84A92CDCF0720C9A99F69189D3A5F8C738890E1B1496CB4C4C58A522C675EED99D7FACA4BCBB6B5B92BD3CCB4F25FD7B1AC35CA768CCF6E9B8D9233F4769AF6B93E1ABD99A67DEE4C9A3369DDF27E26AD7B9B02FF4200EC464FA373FFB1F775CEF7CFEF912078C944ADEF91A0F9A764E51A1856F65D71D77D0D604A5F758D86217D38264BC7F82492F765180A01EB5A61F3C3781A08B3A3C9B84CE7DF4EDC8BEF38AC3501E15F776C8B56EE6048486F93D03CBA4E6FD761783A5E8090CCB356E1CB409237ED2F795355C9A3CE7952FE9AD30F2479D341248F79A6D587923CEA2A6BA363C9D51C1FF3C235BE63A12FADDE2DC747DFD3DDDFF13127A607747CC25BBA0D937D0914BDC45E06CB156EE2EEAFD3C7368443E13A72434AE559CC030A09E32E72ED8455EC5DD5C4540683B9378FF7D43C1B1F9F7BC1BA216DA26C5B5B2E6618CFC2BBAA5D3F8B51624AB8198B8CCF23BE937D470C88F8267A4322854999039A0DDE5DF4FAF924048072EE8880C3EC6B8C2DC48636C5807D8D734F31D87488C1BAC6DE4808F0D7ED88007D757C1FDE9640563F3F86FBB13FFE71BF8F7FACF4F1F5DCC08E7FFC63FB1F7F6B7163754FB97ED058BD682362EC5C4CDE87915663C5CEB5EB3B1928F2FA6C7BE8D9DC84DE63E08963D81E76B6D83D850803B23FE4C4C03FF080B39B1B681A3E0A51F4D21A658CDFE9685244A94521D974685167BDF5989598C9672554448142DBBD598999E55909CD8FBD0BB3120D270CA39199241AD192945D8C466636A39181046433D1C8ACF5A33D1C0E8E61DBDDB4D8B6A291215C0D06FE811D0D993B60E2700418BA490F0CB673D177C811F169EC271A2A991D9B140E2AA1AF8790D05843080BD94A4FA1A1E086101EAA917D1222E37513098E6EEEA9FC43ECF4628A9856AB02637D805CEF6844415186BE2C4CC82ACD96C9EA49F33BAD1FE45F1B3CC39BD88761DABE37F75EE01214DD4A57C02B9CAB0F8BABC0F2ED0C4F20856595F108D1FE8A1C78723A9EBFA7195C1EE4150EE67F84E76100F348B8AE7003A26001D3EC31FE1F189D8E3F1D1E7D1A8FCEC200A4E545B5E3D1DB328CD08F972C5BFD3299A44503E9C132F092388D17D981172F27C08F279F0E0F7F9E1C1D4DA0BF9CA4A9DFB95F08DB66CED885D8FD9227FF809484D49FE2012E18EFD35FF06442829C105241BE9F93763A7E0A9EDB71C2AF107DBC7C6FEE3DC8329820EE5CFBB0E8C7789447B9E029844DA43B5168ADDC955BB614BD82C47B01C97F2CC1DB7F1AE3E53BAE4B3C1F119A15198C9A50F876EB126A5DA41806455F17014CB421C91DD7256CC1584D246CAB75976DE3D10D78FB0AA3E7ECE5743C3B34052E7735F7E420BEE5DA2E95DDCDD73DC9EC6CA5EA8ABB1009DFC1ADA0D2A58D665C59AFA6D7ACF1A14C97E90ED9D65FFCAE7B7369AE518650B9EBDB3E947D5EA7C853A569FEABB86CD78E99AA61AB3BE00BE4C7B7CC2E787E37FCEDD22EE6B779715DFCB96F1BF69FC1CA1668115F78D9FD4B1CC1DBB52506386FB097DE40D386B3B6FC6A87658C919856583673619953C47D53C4ED866533E3B08C0231576E02E403056ED00ECA00AA8D5D076EC38EE117824BF0B2843E3A82846BEF04B7415C7139B815A02C8130AB62534B315921A37688C38C4B3F2814735AA209BF3BB3D65B7D3D70AE6D2F5D9BB9436224D11A7A254E4AA9816B6A9086F54FD08A67694EAD3087684EEF3187F85ADDC66AC3635457B2DA82FAC90A94335C7B69B814AC813038A745A79F299C5A33858C1C674353381DDE145ED737AED737B93FC4DF77216CBF802B18F920CAB66EA35D94AF4FD545FC243207CEEC4B809DD91FD8EC5F474116F49F94A960FA1A7C0C6623816FCF89193BE133EACA127DEE7C47871539021EA854BB17595E08D21449B75750D60F6B5984F7FD21C2F7B3D2397F412631ED03E7C3055887D9DD620193BB45EDF3ADD8FC25F4ACE05424CEC1027E01C9539CE4DB6DAC20D7729BDE944C3D8FD366557411C6C05C130A7E429FE4A7C93C4D0D791925711842FF3AB2811AD4615E0ED14780E2B2A78FF17C15AFD35E34DD75A19AEDECF53E6133D47C5FDF5710F59A2BF32A8E5F220ABD2C7885560CD402557FBF0FD7A9A5193D643DAB3384CD3FE77710E4FB21EF6112C47E4F7BDE3FD45E8520CA31FA11F21224763E58686F6A25B437B512BAA9151763DB991316A5F8AA05C3B2AC57791C4C230C1B023B99DF4B99170EB08E7597F9693422097D882D03EC431AD4F4907FA4815C03F17787D5BDF6AECC9EDFB6BD2AD3CEF6BBF2C24CE7519D75D1B32EDA935534509BA03BC09417E7A01F359B223805476E543A2F0F6B55C83BE7ED9804A7C73F901EDB8A128893AD868812B073BD4C75BA78DB58A99BB7878ED357EDB11366132405B9B95D38437217BC5A350FC816A44C1DEC096821389A238EBBBDD0CE04DA3034A6190982E313D563870DE422B8D8C1298E9595EB0F113B880ED13198EF33526E1AC1CDF7396DDC396D248E791A70BECF480FF9878629CFF76D40F7DC7C9FB32E7B655D767DBE2FBF4DFD2A41FF9AD894E66513A3D2797958AB6225E9A20F8053FABD547A752D131E03A6A86CD2F3B014748E8131ACEAE12D3E06CBFE82D001ECB935ED5BA1840FD08B13BF6545BF5001A7AFBEE6D69CC2EB2445E405AB4C813C177D4880F7D710D95DA530356B46E312F9B990DA666D03E31467D67A993567867E2833241904690BE0A046ED5B0A934B3FC88CB3DD3B0026E68C0218784523F43BBBC27B0518B7F0BB3D300BC9EE16D239F3CCF47E443409AE3650927E28CEF4EEA5E9D5326F0FF03580DFA16F6ADDEAF74D8D1BFEFEC0715A7FF5DF2DE52D19676F5EBBC4739B3F9C5DD1B32BD8E9EA44A8570467CD65F55DBA2E237FF410879075087145579EEA76C028BD598759B00A030F11733A3EA20ED2BF8B2E600833383A2B968AD1A701A9077C9A37F991F162AAAAD3CE6982EA822E2D7FA29A40D61226B9D883F03C8ED22C01486869D31A445EB002219723C41B4C73CC3EC4BFC1264BCA43321069AC0EABB4D73906916EB8C127782EE348E7B87E1539C3B3FF57799C9CAA88DA4C286A33A6A81D1E1C1C515F58805E8E1B844D5455861623D631929CCFAA706EA4815C89CF0E532565D7446DBA53A236958BDAF4C713B5E9C714B5F240652DEF692267C37BCF19CF7BCE7E0059641DCBCD2160FBEE94B8DDB6739D91D5306920A1233292716AC8A201054F238212DF88A3256EE20B976512CFBB016A0342D75CCEDDDCD16755D4247EB4499BC5F1DA877B2726BCBBDC85ED6E4F389A44C08F678CDAEC499C10ECE9DEC916275F54D8F0B6AD4F932DD2BDB0DC6A5833B080CD9802366C70A511D688AF173112309D8685B7ED6E54C0B8B997C4272DAB50DFB47ABCC7A2C56310E7FB8AB25107142B22D366CB6EF1030AD5661DE2B6854ACD19EE8250358178418C589CD43FE30EC6DF069F66D39177BBCF7F172602DAE38BB852A1F51DF774F8AFF585F9672E0D3DEC270EA3DABE807DC0D89BC885E58ADA3E46E1A23C6061EBDB0EC5EBD445AD9926755153F374339685DC47316127A84ADADD01E1508B7E1447463B281306A39D4D4BC32E453FB336FAE1BB26ADEFB8A72E49EB0BF333D0877645446AFE56048CCC5119C8210D246854A60F4E0F5DB8778E4D9CE924697F87844E9288CAFDE0C7A20FBEA9B1DE2E08A198811C4190A7EE0E2F8CCCFC995D10CA0FB8CCD74B39F660D1CF4809B6B1F657EE5D46EF64E80D9834B19E0F8B3BDAF2DD374F2025CFBEA8F69DC38CC1B9F1A8DD0FCD14C0B9F70297E074EC3FC5482CCA7DD5DD1AD4C880D55A67EF33A7C94E1D7EBB9D6A4A8DD7A185A8AB336957673A5D9D297475A6D6D5994E57E95DB9720AAA8A4A6454754D6899AAD232D5A065AA454B7DE9999890A6968C8AA6A2840472E6966A9DACC06A98AC2369B39DB7A65A6B8B58EDB4A59216B0E512AA09AC8CD50656ACD248B5C2C76EA52AE4365394C354B93B2C3B819589BA23B710E428562C099CE6C83A6A92C06AAD2D124882BC85F6D025BA09AC8CD546532CFF4074204237465561B649D5D26B99C549BA8A42CB72CE7693B7E9668972569B441585069B744A767B6D31AFB9BA06FD4DB1B8861D8FB47BC547585D4664C2D954CE88B119A1076A5B219A10C4EB140C27189A743BACCC0C463A10971FB2D4211E4B664296B0A30E3156474AD9806C29B4C5A6A9069BC8B497CDB2692A67131DDA98B2894CC310AB162F5F63600675837452B56853A9CF0C516A008325CA9904FDED047F12BC8B218901F559426F5C673042B2BBDD72F7C9C0B5789B1B94EA7799B31D9BD16F958DDB963B4FC5D4C5EBFC70D9BCFBC4860541F7455B1BFAEB316F6592D17D1B3680B99555D879FEE63BCEFE4492F2EAF14E74FB58B1DBFC4D9903747B386127B6038A6C1B63C9BC0FB11B3569FCAD6E72B7C6DB1767DE81AD3933D1862C19233668E4D87300344B6CE83DBD5D88AF03BC255CCBDD27A7237059B2DA6599CE33B7C9F4B1CB1BED267F83875CD479BB41CC3BB03501E76F4360B04171CF826581E7CD1E1528D219210B2CA1E7BB1458235B13132E1FF23B28F6165B66957C48A0B1D66B7964D08BC902D6D40722358B8A4DD9C9A49CDFAB1EA09F599C80677813FB304C8BA727938775949FBA54FEBA8069F0DC429C20CCA8D4F616B4AE731D2DE27A5195A0A8AE5217D7A73EC10CF820036749162C8097A1620FA22E47CFE3517146637E2AD913F4AFA3F24E2BD465B87C0ADF7166E46BB2A2F64F2614CD2777ABFC576AA30B88CC203FA8EA2EFABC0E42BFA1FB8A71501507225FECAD0E94CBBF65961F2CF7FCDE20DDC6912250C5BE668DFA112E5721024BEFA23978857CDAE43CEC72ECE42200CF0958A61546FB3EFA89C4CF5FBEFDEDFF01FC3340862ADC0100 , N'6.0.2-21211')

