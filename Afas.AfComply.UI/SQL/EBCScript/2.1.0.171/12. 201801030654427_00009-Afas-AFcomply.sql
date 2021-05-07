USE [aca]
GO

ALTER TABLE [dbo].[Approved1095FinalPart2] ADD [Offered] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Approved1095FinalPart2] ADD [Receiving1095C] [bit] NOT NULL DEFAULT 0
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'201801030654427_00009-Afas-AFcomply', N'Afas.AfComply.Reporting.Migrations.Configuration',  0x1F8B0800000000000400ED5DDB72E338927DDF88FD07859E7627622CBB24D54C77D833E1F6A5DB31ED4B5855B3B1FDD2019390CD588A549394CBFEB679D84FDA5F58F00EE20E109464155E2ACA047490482432134800F97FFFFADFD3BFBFADC2D12B4CD2208ECEC62747C7E3118CBCD80FA2E7B3F1265BFEF9AFE3BFFFEDDFFFEDF4CA5FBD8DFE59D79BE6F5D02FA3F46CFC9265EB1F2793D47B812B901EAD022F89D378991D79F16A02FC78F2E9F8F887C9C9C904228831C21A8D4E1F375116AC60F107FAF3228E3CB8CE3620BC8D7D18A6D57754B2285047776005D335F0E0D9F87C891A395F5EC4AB75F87EF408D7719221628F2EE31508A2F1E83C0C00A26A01C3E57804A228CE408668FEF16B0A17591247CF8B35FA00C22FEF6B88EA2D4198C2AA2F3FB6D555BB75FC29EFD6A4FDA1115BC64D875197AF106BB2F79CBCA2DBA8C7EB7512BF42FFE4F887F9751081F00124D927FC37E857FF80EF9D0FE8D34312AF6192BD3FC265857473391E4DBABF9B903F6C7E86FD262706FD2FCA3ECFC6A3BB4D1882A71036BCC398BCC8E204FE0C2398800CFA0F20CB60827879E3C3A25354EB445B108D69FC0E61A7CDE927469B629C2FE0EDBF2148FA81DCC651F672E3F703F93588E0C9ACC640028844753CBA056FBFC2E8397B41AD80B7F1E83A78837EFDA5C2FD1A05681AA21F65C946AD99F9769AF93C7833F7CB254C60C3F99FE2388420D2E6FD23F460F08A48CC27CE457FB434DE241E6C25E2E74DE06BC394B37B81E6CB266D188974D5115E70156D56DAC81709CC27DD4FEF82F1991F2B0D8F59CB97E89FBAEDFCFF5F90863798757EB00C76D28DBA69837E9C4E5AADADAFCBA787A8CB6FA274930064D72F507F13F00C1FE36F7DF5FA9525FB7009D730F24194ED85A1B90E9234CBFF6B41B1EA8A7CE0FB21B4D4B6C476805D7572B1B81BBC7797F113AD329C41710665EB06E5106D8915355B198F645F8C508D63490F382BF211AC886EA359026176EEFB094CD3ED377F51CCCDED771AD397DB6BF6B760BD83BE224B5D78CA3696BBCE9F10B7FC1DFB1308EC0EBC06CF85392660D7F976221AEB471816C5E94BB0AEC69CF22E7EAF2B5F27F1EA310E591E4855E7F745218E88D05852F10B489E61A647F05487E0A902C1535582A76C828D3CB69B28C8F7A20FD1678396DC2D5B7BC288FA151AC3BC6B56F400F040A90BFB12E685204DD1BCF60ADAFAA2AD8A5D6B1B20E1FB79B971F30B9A16693F401F2EC126CC8A8DDDFB65BD2334B8B7B882DEE06D545D5B8025FC05244F7172817E3A78ABF5AC486FCBA1BA88D3AC11E9788346C678A255BBEFE42099BA2535EC5594C46108FD9BC8167250EF30E630FDE4332EFBFC255EACE34DDA9BB2FB2EDC451CF9413EB991A6EF899CC7387F05516F67D1ABC6E00A51EA65C12BB4A21397A8FAFB43B8492DBAB54869FF13849B9E03FC0D047994F8012641ECF7362536B623D621887294BEC4BC04899DC10BB713AA0CB713AA0CB713AA748B2E59CBDFF1A24B794970112749AE86E3082D0A6687B81A70F3C4CD13A3CD096CB53C63AEF8BB73E7F76EFD76D12FA846ADFB457575F72AFE1907BE3AF5786D2EED6D2519E5584D5B5B16B3830D32598E0F25B654DECD5D3F7A7EDAA4C8174AD3FCAFE200D7F677B96B12AA404641C597B76C7784E4818DBBD5EEDAFFBA28821C17FE2E49F82D58EF8200B41ACE80973DBCC411BCDBEC60109C2F226BF93BF645946D626E5B0FD55B2FFD86FBA8B70C94404209186C9283348E2CB4EB7489D32583AD6BF20D61C6AAA0D62DBFB7B5DAD5005548AD02E81ABAAB9607C4DC4C4C5A5585415751C227AA2CEEB51E29200E55F9DE6FB2F526BB0E42887EFAE2DC23A7D23E944A136C7634D396BB4BC3AE41E9124E35232DF713C8BC1709B9784516B16DB98054AC527FE557127380DAEF11AE4BC3D22F6A583029D7A1E789F712BCEE4C95FEB181A94C570DDDB8053F7E8106CF028C332DB2969D69619B96302C6674A9E8D947FF8CD475AD8985EABAD6E946D4CEC5D4CE25D4CE55A89DCBA8D5F6ACE787685B9C67EDD4DFC7547FD891618936E95665E913BC06D75D25AA0DE4596B293F3EA9B63CEB4EF4F42035A0D3404E030DA381BA7387AB8604D5C487197A2AA4E6248412F5786D2EED6D2519E558CD5EFAA9DA423D48CDE4424E2EE4E474F8CE434E7351C8692E0D39CD070C39CDB921A7B938E434B71172CA797F9DA0FF1EA2F2B5F4585A3F08A7D79C5EEB7F2CE74B02A27415A4E9011FA6C7FB58F2AEA74C7400DFDBCB74C5BC210B536D7824357F6CE023F4E2C46F67F7F61C33BC070CC5C02AD66CE1264951F7827566A57B4E4F3A3D394C1025F7867271AFA49D1D9A2054285AB4923A1573C1A495A9A085FC17DAF116D9C103AA05DEF9036145CABF14D7EEE76E124C7266CC99B1E1CD98332DB2969D69D1DF5AA0143E6B8B815B49A6757B6C39D0766D786B28ED0DD77E1AD991AF294CAEFC203BD827E3EF43BFF33CC4606EF71DFCB69D86ACBC3B6FE5C5CCFC9E5E5F429AD732EDE0247D719C8993B5FC1D9B382DB5FA085F03F80DFA87A8556D3EB8BB4753BF1CB0DDC406CBB6DD215AA7C176A3C1362B4C7FD1637C935E87E039C57AE0C3247C471DC6A75C97EC5BB87A8249BD04289E111B8F0A27E96C7C4275B253FB2602DDFA9FC4F52F6108B3FC31B1AAFA94EE76D94141A7193B00BD7B5D9DF4C028FBFC17714FEE93E0B97C09AEFAC15F7E10FFE011AE43E0C115CC639FD56FFEFAA967FFEB796F61D8F3CC5D58FF25035FD7FFAF207BB94A92387F59524D041EE17397D353717D34A93C883A9B4FD9EA1733F12FEEE2EC3ADE446D0B73152E9FA769EC0585D9651C96EABC9ADB6DFB2AF247AA4FE8B6EA997EF07F74BB09B3601D061E9AD367E3E3A3237A0C14DA6AF669056D95CB48A2C13F915CC23862C2A8A92EA3A65B64D4549D51537B8C123D72C4A35FE9C5A3B60BC41B6462D2951B12F36AC61C97936E97505BF751A9FF47E75E999CEF02A41EF069438A26A86FC661FACA35AFBB82FBD76D279BD72134F8C8BFB6ADC33D5AAA35B8C0B9A1C9A358765DB325BBBDB1ADC110C935CFBD9729CEA97C597779C762696ECE4DB8C93949ABA33977CC4DEE0D585667593708AC719275378B00AF2E37EC1103673A0C645F6EB636B13F140389B72FA47A9C389568C93A740F33AA0C452F93C07FA150D11F603D573880DBC178EB5081E33B9123D15D09A5DECA2D047195C994AD1FD556D0A79925D38A156FA4A4478B8FFC13D1FDD628265C50D257D4296A4BFD97EA2B0AD69EBEEA5CE5519478D6BD9E012616E3529002C777329BF8017C5E5F15A2F9EC832BBA4C951F06D87B5D253F60A0DC79FE698381B8CD3FECC76D703F4CB0F844A062FFE5CB5E59DF3598FD3116C16560A178E23588F02DEC38C9F36F5C820CE485F08D364745440266BCADC1872AF1581BDF906C52528C556860AADC00BDF92C6D40095B0BB64A1F2603AEB38CC9A0BB1E350B95F4D6D569AD255244E84C8D03B52FCF22B0F5F32520CD62898582ADA454608A152AE402550B584582E62282E67A63C8C4223D18356E33A15A2F4502D2DC9E63A1B457EBA430844A64A2512A570F94D951DA684A403BE7EE5888DD83792A70E511021E56731A8584C214B5403735791DB1EA3C15C58C66E9C5B39AFEB1D5E4C400BA368822E87A3448E3D6E59131FFA61AFC23835C7A612EDBFC9BAAF36F6A837FC2A41D340B95C35FDA0130ACB794F513705125E4C56624C7C6E9B390F14630CD3849544B31AE85F504B3BD02F6F02359033385F7C228CD199548974EAC0BEB18EE5A089824896E6D8953E4532D7C4E89362A75E2580C4ECDD538C5D98DD45286BD5885BB732246F1E22BEA21AA3E4C62C45448B8C663B538E3D498C30F3EA9879FFACCB5AD32877C2C5DA0A0195BB42A412573D5DCDD965562697F438FE7B7929A795EA84933D864C7C433C24B2ABCEFCB32B97A560E26698793788C9328229500D2D02A9BF19A0D77EEF1B6B315C344B41448F8C30F0C6D8D2B3275C48C18A9C48CCC992155473450FFB985BFF3269D59BC4892662CC9CEAC62448F54786FC032C1ED4D9A658A9122CD5811D633C6668D8069F2E8D0D0534EE1F6A80217C51121C398903DAE72A340FC26AC584AC9B30E52C62AAFFDD4E23E3ABD3588F4585A10D637449A184D53763A5920BF7705AA0FA71354253FACBF01E16DECC330AD0B6EC17A1D44CF69FBCBEACB68B1065EAEACFEBC188FDE5661949E8D5FB26CFDE3649216D0E9D12AF092388D97D99117AF26C08F279F8E8F7F989C9C4C5625C6C4EBEC8C9111A5A6A52C2EF2D7774B733DE9C3EB2049B33CD4F404F23B0D17FE8AAAA61391AA9B1407A6E8E1AD7769EBDFE7FFAF30968813E7CB8B78B50EDF8F1A5A8E2EE315A28F17D1225A68D97E8D38915F0F299802A51BA03410825A78200409E3CAE1451C6E5691A8EF37D4A6B30813D6B702096CFCBB3A5A735111876A3EAAE33497AE719CE6A33A4E91DA74D685A9BF69A2CC192854E0418AF29981F25907E57EB984092438D37C54C779841E0C5E91B4E6C273D18523CB7450DBCB905DC4F6BB3A5AF74E248E8797E86162B7217140ECB33656792F90815616E8487D7BC7B12BF8ED777D349ABC6E098D783A213419A94C279436A58E277495B5B92A670666ECAAF2A92D55CE00EAA5CAA77AA27D13A51BE4BA78F00221E5D6F831FE46B6C3ABA331293906E3CAC8605CC2358C7C1065245CA760FB06A8F059EE8A20388E847DD6988881EF879006C3BF6B9811C022ACFDAA8EB458DC75418A0F1A63173F1163967F70C6C2198B6EF9768CC5B076C28689B0681DF4C4D896526C5EFE60ABFE649786A4FE154FA9B0CA9D39B0690E1659026176EEFB094C090D4C146928CD2023756F403F9723A68AD268D527758CDF827517A1F8A041030C9785E345AEDB3A05CE703AC3D92D1FDC7036079607329DF549E77EC6938B62623E2B30A34D32CAF04123C36777CB2D83C90AB904F9CE322D8354A13A2EF04035F309323B05EA785E08D2144D08AFA08604A54BD59157C5BE2001D87CD4C409DFCFCB85F12F4895A60C4CB2823ABE0F97601366C566DDFDB25E80779BE0D5D1E805F408AAF30FDA542EC012FE0292A738C9B7F099449255F467407A5B72F4224E33F65CE8D6D06FA1DA1965339B5B49BF9DAB2889C310FA3791B829563DF5D6827AF7A67C4E1B6F802852C78CCBCE7F8917EB789312A854A1F64675FDD38B38F2837C6EE7D131C6DE35B39E86D423D3F32B880887A8FDAAA1A7AA71B94274156FA2D15A955345BD8DE5260CDF1FC24DCAF50CD93534F81144D5F3B81D7E345FD591BE812037EF0F3009629FD4B254A186DD622D4A33FD45E93A0451FE1B9234FCBB3ADA4B903006BCFDAA8E1432424FA176E82964849E42EDD053C8083D85DAA127B7EC70CB0EE3650779D6D3E28A83B805A9BFD89001A8AE33BA387AA2EB26979B5C36D6F433E656B3A5053D1B5C6F39CFC3509D6446DBC276B7AC13C926736238696F880DD8E283FAEF7FDAA4C8A6A769FE5771B0A48BC628D6C7AEF6708BDF7F79CBD82D5095F4DBC9B779EF566CF8BA4C1FF5EBA2D8FBBDF0D9C058B109F66FC19A8F5C156AA8C8FC749C973DBCC411BCDBAC484B43143A43E30C4DB77C1043D35EACB168609A4722F4ED0AFFA7AAE6A446D013D1363D3A89557ED54522C5A9FDAA33B1CB7CE7DD495D7E73EAC1A9876EF920EA01BBAB68513FB4CFBFE82B08C16F55354403A127A6F79B6CBDC9AE83103E803C177767CF932873D3D34DCF6EF970D3B3BE4E6D7B8232AF90ABCE50CE8FB5A66881A1BBE9B22EAF5E76A752F5511DA7683F9FCEE789F712BC32663CBB860EA57F6C60CA98579D02033CD287E914E81C71893212AAFEE6B49BD36EDDF2419D8FF920CE0775CB58C3F960FC56D7F9983BE74305D34DCFBD9D9EE4EDFAA1024026135506601400D29CB26E72B9C9D5775FCEEEB46A9E1335DE97EB31956A04B72FE7D483530F64B9A67AC0DEFBB5A81FDA8782F51584E0B7AA1AA281D01353FAD099D1E304E4B847CE959662B9E96C693A53CFD9D89CD592C78D5426B71442798E13489AF71EF10CC30179478E2E3544A6CE62D3A5EAC85FA3E08F0D7C845E9CF8A47220CBCCE865A90956B93AFA4D9222A282754612DC2970CAD129C76EF93694A3DD25119578A0B772ECB14422919C72DC47E5E894995366C6CA8CC85B62519375139EE8AB31C9EF557558074633C611FA8C2B4EED5775A43BF88D81D47ED55C1A5A78B7CED68B1DF9115C929CFA9BF6B167EE9B1A466809EFF0B4F315A5584EBDDA53AF751E27CBBAB5490065A65AF93FD7D1AC35CA4E9F0BDA53B55172863E4ED37ED747A30FD3B4DF9D4A732AAD5BDE4FA5755F94963C8ED6E493337F01AD81307BD5387F5F9BE02DAF89FA056F9AA54A8A8F86655DF22A52A8D6483D68AD9E03B7452BD70F16D2DB3C9A30BA49EF366178365E82907CCB41852FE443E596246FDA5FF2A6AA92473DC2AA3C9AD30F2479D341248FF9E0EC07913C610E43C3BBE4048ADEBD71C6582AE429EC3F9C331B52A790ACD19052F925793EA9BD858491A551FB9A1AF65BD5EB680C06737332F612003B2A879B7AD29036D11D3B5BDA6518A5C24B60A97F7749A24AB8F79418C323CE54B9270A449C9FD39048E155AC01D5062F3BA7FE29720240F9C4B880C3EC9C6E16DC029B62C0CE69D7530C9811AFAD88019E5CD34008F09FDB11013A8F661FDE964056879FCE1BFA41077FD66FF0674A83AF6706F67CF05949633F98012033C7EA3B8DD50F6D788C9DDC8C7D1869D557EC649EDC4B4791D767DB4B4F3CABAEE9C213C7B0BDECA473E7F618AEC1969C74EAE08FB8E014660F36BCE024F121C59799648CDF6B6F52214DB20D21D9B66BC1488FAC7D4947BE2BA1220ADC64C8FBB32B4127A4ED676C34077B1F7625C8C4D1FAC222F646B424651FBD91B94D6F642001D98E37D24DAA6D6A70700CDBE6864E9DDDD71B19C2D4D099C33FA2A111240F373E022D303CB2A3CE0CB64B5384EF812192664837A451E53CF7368583CE8D6E2E2434D610C2C2CB7C6E6740ACAC729413C21F841019C74D2438BA37CEE403B1D7C114A584F79684DCB6C05009EEC92ACD41A9EA4BF37793E0BE4A2EDFC97A5F742FCF615F742BAD12DD93D9E6CB2AE311A2FD1519F0E46CBC784F33B83ACA2B1C2DFE082FC200E69E705DE11644C112A6D997F87F607436FE747CF2693C3A0F039096D9DAC6A3B75518A13F5EB26CFDE36492160DA447ABC04BE2345E66475EBC9A003F9E7C3A3EFE6172723281FE6A92A67EE7C56BEC7029EF9407E310D5E93F20252BF5A03CC2A510891ED5D30909774A480A1F2927FC6CFC143CB7AB889F211ADAFCBCDE03C8329820DEDDF8B0E8E57894FBC0E029848D1F3C11B68BE7C12A5B2A9AD144690EAC9A433407F7CD21EA64F32542F40A12EF0524FFB1026FFF896365097DCF800935B707F5D90A54937ABE96097D1E91E9E6FB20B527644B944D712B2C28447119C0441B923C246B2E0AD8E9D82EDFC7A35BF0F62B8C9EB397B3F1FCD814B83C885A42FBE8FF59716D4F5BE4DB53B276A9EC9E97ED4926FB4459572D89E4183F7CABAF97A7D6F432E3C09DA15E9E0EAF9779B9E3CDA7C495154DDF4915BF538381E50396A856B529836504B6A1F5814DE28AECC016A82A92C6F3D5810A8453FB4EED0FACF6AD687C2BCA7E783D6F4111E2E975766D1E5899DF2D6909A7EF4D80BAC9E06D409649E1ED1087E9A97E504592782B34E12940DDE2C859C9FE368D99525DDFAA7193A9EBD9350C662B3B4B3DCD929DFD292A357B4FE1ECA46537278B4EC46E8EB5AA73B0F7842053AE9BC3F1D2AB5BB0B6459A750B389CE4EA1690D939D54BE0651803F39940255037B752A244E9E6A84476747301A212A29BD3244A7C6E8EDAA63C37C7E0E437EFA9A0D829CD7BF4B449666E3E9C54FEF21EFABCFFAA094F596E8ED2262BEF3960A1BDD845682F7611DA8B5D388FF8FBF6884507E9D59C61D9D972B91F4C230CEB023B993F4899172EB066C26D522534E2AA870696C9FA9495235C6D42B2CED3CAA620DD21DB93CEEA766832C414BEB9EB431923B5B88D8D2F6E3E719BE07536719B98582271BBB05516712B7BA6640A711BA0CEBA1CA47551D6E1ECEBAC6AAA9B7FF953AEC2F1DF0EABCADB5C423D07BA4D256467E2950985DCAAC8CD613D0F513B884E03B557990608C5739E4450D32982F702E44AA5F3E361B50A9993D3D962378F77B4D223DE001962A5C74BF9AD31A739C9BE152775F3EBA1F75AD6ED055DB34D6E76DE703BEA014B1C6E15D0827354A70EEF09E354E041AA406DDF61DEC777605CA153F71DE6CE777013676F268ED077987F08DF41F4DC8041CCC66872D3082E66E366E3DECD46E2418C216623FBC118ADFD3EA31988FFD6EDF7B9FD3EA75D0E66BF8F93195C4DA7087282CB954AE7C7C36A152B37D3FB00B8497F90935E7D96091F4C519C6CD2974314E61C0363D8A94767B6ED290874425B73C92793D8DA701558E96BCD29ECA4F476DE875344066B9BE1CE90891F1133526B46EB131686536B7BACD69C1AFAAED4906411A42D80832A3541AA6D358D2649B22D576714C0C0118D2687B70507A34DE36D01CCC28B60165E57A833789B23E029787BA324FD509CEA3D48D5ABA5DED8E9AED5B51B3FD1B59A72C37F3FB09F66ED71953D99BC6DBE6C3BFBDA6DC66C77F8C3E91575BD82BD43CB88A67552587749BB8AFCD1631CB27E509396DF58A6F31223266CC22C58878187E8391B1F1F1D9D501D17A097FE9CB089AA4AB79D3F518D206508935CAA4178114769968080CE379EEFDA7BC11A84DCDE12BF606A5B8547ADF2316A1A234BCAC7FA10AD62AE1891C2796AB96992300C32AE75DE363611B5E95E89DA542E6AD3EF4FD4A874E1FB2E6AA20CD50AF230134ADB8C296D27D473EAF7D1250C610647E70525882A907AC0A7F57EFE70388F2AE2FA354E125934B454B22E9C7244813DF0CA42284EDE26133EDE6BF25B903B3AFDF540D226D16DCD15331CB4FD783892C24B0D2990915DEA254EA6EB8FA592DACB463835D8D7C3112F6EAAD93DD6419C34DA03395903CBD89C2963F38376C1B8C9ADA5ADEF83D071EF2D11838BE586C647B7FA7CC042C66310677C4537B906142B6996EBAD5ACB0F2854DBB58EBB162A3503B90F4245241BB7348C7BE88F1B0C0D3FB5D5300EB928A5E9F67709B074E256C6F1407704B446589CAC7EC80D0169DEF56D0BD887F6C7891B655CA13B6CCF5C9CC6778FDD733A01FA40E2A76607E72CFD79D8A2C3CB1ABEA71B554412744B2BA83D940E835511FBBCEB70D2B04F5E5227CDB995713C5083A535C2FC5B9E439B27693EF82D08183F59FAC7F291A873F5385174E1011B3B599AF23D357AF284EC8A433F130DFDB6D689FB208EB294E34C41504BB63DAC302A66B5DFBE507EE8C861AF69F291E38846F36017E1C4AB6E1AFAC6052452C65372571CF98419CF9E5447F4DA438992B37C654AFAB3B1FF142319298F39F2EA52CB09055AA6AAB44C356899EAD3A2428622055A8DD7A9CDC4CD37B564043415252490FB9354EB640556C3641DF56ECF14783E53E3F94C8DE7EDD630D5665BC46AAA2D95B4804524A826B032561B58B14A2355108DDD4A55C86DA62887A97277E682EECCC5DD99EB8921AB25B282540CE56DB6CB5F9E2470DA694B252DB46F82D04D6065AC369A62F900D1569A6E8CAAC26C93AAA5D7328B9374158596E59CEDDE2DA49B25CA596D1255141A6C6EFBB0DB6B8B79CDD535E831C56CBFC036D517014658759E8D625E1A902DD99B5E2B583331566760D8806CC64FBAAC3066D354834DE481F7EDB2692A6713EDC7E8B34974009BC129E5F3DAB2450EBB6F6C6BCDDF2F9C7580240E873E73E853C20C96488E120FC608D2652920B8EE887EE7394760191C50392C3B181B28BFAAC0E0BB4CE68C20E2C202468822C8836911CA23EB3082369E7D1881BB922236F04F3B710E849194579FF7A2DB33C56EF34FC10DD0EDE1C49E387F25D27C8CD8631F62B7AAE6F8678BE4E68F7710C9BC033B3375A213303246EC44DDB1D78634736C6800FA54067F36F0A2628331825CB0E2F265B5F3323DC03C83D047576FB59BFCE8B95CFC79A176F30EEC4CD4F9315E061B1403C283893E6FA7A18092EE1E58600EBD37A2C024598C411887E177506C4B76CC2AF92242237C36D85AA217BB054CAA5F7968C2354DD9E9A4DC15AA3EA03FB3384F567F1BFB304C8BAFA793C74D943F2551FE7509D3E0B98538459851A9015AD0BACE4DB48CEB701541515DA52EAE9FB28019F04106CE932C58E6093593D883A8CBD1F378543C3C953FB5F204FD9BA84CD481BA0C574FE13BCE8C3CDA256AFF7442D17C7ABFCEFF4A6D74019119E4AF6FDC473F6D82D06FE8BE66BCBEC181C8C368D52B39F95866F96B39CFEF0DD25D1C290255EC6BA27F5FE06A1D22B0F43E5A8057C8A74DCEC32EC74E2F03F09C80555A61B4BF477F22F1F3576F7FFB7F98E74ED792B50100 , N'6.0.2-21211')

