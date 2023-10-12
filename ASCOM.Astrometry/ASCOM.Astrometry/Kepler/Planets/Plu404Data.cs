﻿
namespace ASCOM.Astrometry
{
    static class Plu404Data
    {
        // /*
        // First date in file = 625296.50
        // Number of records = 16731.0
        // Days per record = 131.0
        // Julian Years      Lon    Lat    Rad
        // -3000.0 to  -2499.7:   1.17   0.90   0.83
        // -2499.7 to  -1999.7:   0.57   0.51   0.58
        // -1999.7 to  -1499.7:   0.63   0.39   0.40
        // -1499.7 to   -999.8:   0.40   0.45   0.41
        // -999.8 to   -499.8:   0.42   0.22   0.30
        // -499.8 to      0.2:   0.41   0.24   0.35
        // 0.2 to    500.2:   0.58   0.24   0.26
        // 500.2 to   1000.1:   0.47   0.35   0.33
        // 1000.1 to   1500.1:   0.43   0.31   0.28
        // 1500.1 to   2000.1:   0.37   0.40   0.35
        // 2000.1 to   2500.0:   0.46   0.35   0.39
        // 2500.0 to   3000.0:   1.09   0.70   0.46
        // 3000.0 to   3000.4:  0.871  0.395  0.051
        // */

        internal static double[] tabl = new double[] { 74986469.33577d, 100898343.7369d, 48199471.54076d, 9520076.03177d, 690431.6734d, -427355.12716d, 52266623.77862d, 860190.70714d, -21.08511d, -143.39295d, -126.71124d, 48.18528d, -88.74508d, 40.50942d, 51.29367d, -10.24075d, 0.63094d, 32.03258d, -410.00781d, 399.90234d, -116.54319d, 51.50329d, 116.84565d, 687.76781d, -13.38605d, 182.70107d, -1668.12226d, -5162.22648d, -585.68297d, -2247.56041d, -20.95875d, -193.13703d, -57.12097d, -10.35058d, -1778.01442d, -6463.73779d, -657.86093d, -2713.44436d, -41.3256d, -211.82042d, -107.16513d, -36.58628d, 97929588.08231d, -33582903.63417d, 143382679.3177d, -47411568.92345d, 79428827.73581d, -24658834.87499d, 19861233.17488d, -5755585.62084d, 1995060.95931d, -693507.08147d, 135176.31467d, 109360.38672d, -8188.00598d, -1680.95072d, 71020.19608d, -70785.39049d, -24.56034d, -20.34919d, 1618.45976d, -2079.48538d, 635.62954d, -850.87068d, 44.95257d, -64.04459d, -18.61475d, -1.77734d, 9.38079d, 5.19958d, 17422.08783d, -4872.53852d, 10985.76629d, -1137.68569d, 1771.28893d, 288.93745d, 40.22664d, 37.90027d, 6.81361d, -32.65868d, 16.97268d, 11.76152d, 29.33024d, -12.92289d, -55.13187d, -14.73791d, 7.52474d, -102.0506d, 182.53144d, -20.1896d, -490237997.494d, 486646248.6336d, -781277018.2643d, 602300460.5729d, -463787999.4642d, 249529525.961d, -123964746.8642d, 31353019.97807d, -13353800.92159d, -3463382.63269d, -35469.17654d, -1035343.45385d, 65076.64025d, -38181.61312d, -16473.33813d, 3928.44674d, 188.60263d, 1000.4253d, -208376.39376d, -700566.62363d, 114839.84613d, -342407.71113d, 39467.04812d, -14553.84849d, 581895.261d, 1012499.16715d, 406317.22416d, 310804.78515d, 43625.07033d, -4157.26545d, -5930.13795d, -2070.62413d, 3348.17582d, -10871.23729d, 144609.1855d, 60383.6365d, 27912.02226d, 15254.61228d, -98561.37758d, -67442.28158d, -15573.63338d, -19931.99773d, 24323.06905d, -37473.32558d, 2840.64042d, -8911.23694d, -19636.31898d, 71725.21946d, -12280.54554d, 12251.00101d, 88626.5226d, 5513.6845d, 18506.41546d, -6513.87434d, -83350.14621d, 44300.00743d, -22075.37353d, 3731.57531d, -29167.7602d, -21642.67384d, 56602666.72177d, -22225578.01823d, 50576897.80669d, -50319847.79086d, 5689259.25622d, -29585299.79697d, -4249711.27661d, -4490830.29568d, -727678.08724d, 366050.85631d, 19183.62792d, 55647.98226d, 1897.78091d, -1091.03988d, 432.38158d, -138.62556d, 101.38743d, 25.67379d, 320.20735d, 362.16615d, 85.06067d, 54.02616d, 2.3946d, 18.70004d, -8.43353d, 2.721d, -3.11205d, -3.06201d, 136.31503d, -28.3362d, 48.68781d, -18.45285d, 1.15302d, -1.5236d, -0.13706d, -0.37489d, 0.0875d, -0.14579d, -0.07051d, -0.06518d, 0.30237d, -0.00448d, 4.83172d, 6.83684d, 1752447.78043d, -945086.75857d, 2340978.12819d, -1963675.42559d, 1254147.25257d, -1274861.91191d, 279459.60237d, -263954.01378d, 11835.6229d, -16344.44434d, 9119.9896d, -2390.44218d, -23.67909d, 86.73916d, -642.78635d, -1290.12208d, -0.43345d, -1.85348d, 0.03094d, -0.01655d, 0.1238d, 0.31834d, 5.54756d, -1.63109d, 1.10598d, -0.17578d, 2.66994d, -2.17573d, 0.9736d, -0.92226d, -0.18533d, -0.39747d, 0.45809d, -0.65286d, 0.26129d, 0.91922d, 0.81068d, 0.11183d, 6.32182d, 14.16786d, 0.20872d, 3.28489d, -1.47458d, -2.11724d, 1.7002d, -1.99889d, 3.13003d, 1.90638d, -4483669.52795d, -446454.90158d, -6586256.67478d, -671890.16779d, -3620444.55554d, -499672.41074d, -855998.32655d, -191073.94266d, -73186.6911d, -22649.38582d, -2414.81729d, -1294.40542d, 436.80907d, 125.48109d, -81.16877d, 458.86508d, -11.57414d, -26.39114d, -4.00801d, -5.01054d, -18.17569d, 20.86879d, -4.80965d, 3.10535d, -4.71122d, 1.18169d, 74.75544d, 649.21464d, -26.5506d, 272.35592d, -8.06982d, 16.8611d, -26.54868d, 26.75711d, -35.8291d, 38.51063d, 22.22814d, 19.38336d, -6.30462d, 0.90602d, 0.62856d, -0.34981d, -0.10232d, -0.00939d, 0.04439d, -0.18875d, 0.16025d, 0.11306d, -0.06803d, 0.06004d, -91305.66728d, 262370.61704d, -194633.44577d, 304838.17733d, -124505.90904d, 94111.75602d, -22317.18255d, 1575.23438d, 748.66316d, -349.78711d, 166.6445d, -89.05045d, 120.76207d, -100.26715d, 3.13806d, 3.71747d, -1.44731d, -0.35235d, -0.5166d, -1.50621d, 2.8131d, -3.93573d, 1.20292d, -0.36412d, -0.0334d, -0.00561d, -5.29764d, 26.02941d, 1.91382d, 3.30686d, -3.35265d, -3.20868d, 0.05807d, -0.11885d, -0.78588d, 0.34807d, -0.19038d, 0.11295d, -0.03642d, -0.03794d, 0.00251d, 0.03449d, -0.08426d, -0.0031d, 0.05297d, -0.09278d, 0.10941d, 0.00099d, -228688.56632d, 312567.73069d, -331458.31119d, 328200.1946d, -143760.57524d, 104182.01134d, -17313.30132d, 12591.15513d, -440.32735d, -105.67674d, 104.35854d, -852.8459d, 0.95527d, 0.30212d, -54.63983d, 4.06948d, 0.07545d, -0.13429d, 16.21005d, 29.24658d, 9.2341d, 50.48867d, 30.55641d, 12.76809d, 0.11781d, 0.70929d, -0.041d, 13.60859d, 0.04976d, -0.02083d, 0.36279d, 0.3013d, -0.02129d, 0.09363d, -0.07812d, 0.0157d, -0.06217d, -0.37181d, -29348.55031d, 43889.87672d, -35765.41577d, 33855.9007d, -10128.69894d, 4535.32148d, 281.75353d, -218.49194d, -7.55224d, 134.2864d, 2.11319d, -2.13109d, 15.71244d, 11.07183d, -0.05406d, -0.23337d, -1.28949d, 1.34281d, 0.04212d, -0.0208d, 0.08109d, 0.1482d, -6010.46564d, 3639.4178d, -5973.16d, 1381.66999d, -1177.36865d, -501.06937d, 166.14792d, -103.36431d, 14.92766d, 4.12877d, -2.20893d, -6.32033d, -0.29038d, -0.43172d, -0.59341d, 0.20477d, -0.13143d, -0.0315d, 0.10992d, 0.01976d, -0.00254d, 0.02028d, -0.30044d, -0.44658d, -0.03409d, -0.10758d, 0.08349d, 0.06153d, -0.06055d, 0.18249d, -1.15341d, -8.68699d, -0.11348d, -3.30688d, 1.08604d, 1.04018d, -0.46892d, -0.69765d, 0.21504d, 0.01968d, -0.00455d, -0.01678d, 3.95643d, -3.17191d, 3.9522d, -2.1267d, 0.99305d, -0.16651d, 0.34839d, -0.49162d, 0.85744d, 0.20173d, -0.00975d, 0.20225d, -0.02627d, -0.02281d, -0.18002d, -0.01803d, -0.06144d, -0.2151d, 0.15935d, -0.01251d, -0.21378d, 0.44806d, -0.01174d, 0.05779d, 0.07646d, -0.19656d, -0.04044d, -0.02521d, 0.02996d, 0.06169d, 0.16698d, -0.0471d, -0.06506d, -0.02114d, 0.055d, 0.00276d, 0.08433d, 0.0316d, 0.08193d, 0.35773d, 0.05454d, 0.10718d, -0.02823d, -0.00839d, 0.54078d, 0.49347d, 0.09609d, 0.11825d, -0.16092d, -0.11897d, 0.09059d, 0.08254d, 0.16712d, 0.0586d, -0.09547d, -0.03206d, 0.03876d, 0.04719d, -0.02345d, 0.0224d, -0.00609d, -0.00649d, 0.03859d, 0.00077d, 0.47819d, 0.26196d, 0.0978d, 0.08104d, -0.16919d, 0.05042d, -0.42652d, 0.3081d, -0.03409d, -0.51452d, -0.2312d, -0.0138d, -0.01157d, -0.00143d, -0.00512d, -0.01628d, -0.00189d, 0.00183d, -0.01427d, -0.02861d, 0.00618d, -0.00015d, 0.13087d, 0.1387d, 0.15158d, -0.21056d, -3.94829d, -1.06028d, -1.36602d, 0.77954d, 0.08709d, -0.03118d, -44.74949d, 91.17393d, 8.78173d, 45.8401d, 1.9756d, -15.02849d, -0.10755d, -0.02884d, 3.3867d, 0.30615d, 130.92778d, -24.33209d, 43.01636d, -40.81327d, -19.439d, 22.18162d, -0.12691d, 0.33795d, -6.4479d, -6.23145d, 0.00319d, 0.01141d, -0.03252d, 0.03872d, 0.04467d, 0.01614d, -0.00382d, -0.00019d, 0.05955d, 0.01533d, 16.11371d, 41.37565d, 61.44963d, 6.90615d, 1.41326d, -0.7392d, -0.03871d, 24.81978d, -0.10229d, -0.32775d, -0.05188d, -0.05628d, -2.33618d, 2.39053d, -0.00584d, 0.00436d, 0.20903d, 0.0222d, -0.01738d, -0.02765d, -0.00217d, 0.00613d, -0.01772d, 0.01903d, 0.07075d, -0.0053d, 0.15234d, -0.3776d, -0.11641d, -0.20102d, -0.63675d, 0.20525d, -0.15783d, 0.58945d, -0.06243d, 0.04306d };
        internal static double[] tabb = new double[] { -35042727.30412d, -49049197.81293d, -25374963.60995d, -5761406.03035d, -467370.5754d, 14040.11453d, 2329.15763d, -13978.6939d, 45.43441d, 29.70305d, 32.33772d, -38.34012d, 26.43575d, -28.76136d, -18.5904d, 12.64837d, 5.56569d, -12.51581d, 248.3735d, -64.44466d, 54.02618d, 4.39466d, -269.35114d, -290.63134d, -48.03841d, -52.83576d, 1508.94995d, 1682.78967d, 554.02336d, 715.65819d, 34.37602d, 58.44397d, 16.63685d, 16.10176d, -1069.51609d, 2300.89166d, -437.16796d, 927.89245d, -33.17679d, 68.74495d, 18.72022d, 32.9764d, -34004958.12619d, -17758805.77098d, -48416073.75788d, -24973405.03542d, -25374996.23732d, -13351084.9734d, -5738294.54942d, -3082092.6335d, -519989.39256d, -206440.89101d, 44186.23548d, -87639.2263d, 2506.47602d, 2327.01164d, -53878.47903d, -19670.13471d, 2.66934d, -3.86086d, 106.32427d, 576.47944d, 46.56388d, 218.28339d, 4.35402d, 15.04642d, 2.68717d, -2.86835d, 0.81728d, -2.34417d, -1604.85823d, -1999.24986d, -631.47343d, -1382.19156d, -15.74075d, -256.97077d, 6.99648d, -4.54257d, 2.63478d, 1.88838d, 0.17628d, -2.11518d, -2.46735d, -1.48743d, 1.83456d, 4.68487d, -7.10919d, 3.57046d, -5.36342d, -7.70367d, 28395956.20816d, -37176795.74372d, 48969952.83034d, -48145798.96248d, 31155823.23557d, -21163596.14822d, 9057634.3826d, -3167688.51696d, 1167488.70078d, 219103.97591d, -19017.97335d, 107849.61195d, -3814.43474d, 4405.9212d, 5800.13959d, 12619.88708d, 22.18168d, -89.47801d, 52202.81929d, 55119.44083d, 5082.58907d, 37955.06062d, -3165.24355d, 3316.67588d, -113906.4397d, -69279.41495d, -57358.07767d, -10176.17329d, -4179.79867d, 2495.99374d, 787.8718d, -154.35591d, -1148.62509d, 1034.58199d, -22194.95235d, 3341.97949d, -4578.53994d, 108.30832d, 7444.39789d, 16646.40725d, 509.7543d, 3808.92686d, -179.85869d, 7408.76716d, 340.65366d, 1504.64227d, -3783.09873d, -13505.60867d, 875.74489d, -3181.27898d, -16220.93983d, 8041.37347d, -2631.07448d, 2899.50781d, 18894.92095d, -20072.81471d, 5925.05701d, -1947.91902d, -6731.56601d, 8014.52403d, -987793.49463d, 6491762.34471d, -279205.73643d, 6117135.96868d, -140925.91402d, 2259422.06929d, 114028.61646d, 605600.90358d, 91858.00186d, 56506.65187d, 8949.15777d, -9782.67413d, -394.66541d, -105.19208d, -76.54752d, -32.59411d, -19.28741d, 10.40013d, -107.64003d, -7.36229d, -22.25126d, 4.05952d, -3.74402d, -2.79308d, 1.03337d, -2.13968d, 1.53794d, -0.02617d, 35.70756d, 12.97733d, 14.46213d, 6.20518d, 1.79381d, 1.65422d, -0.31216d, 0.29053d, -0.03538d, -0.01584d, -0.08934d, 0.00079d, 0.05539d, -0.21591d, 2.86929d, -2.24724d, 320797.07455d, 93342.16556d, -20903.39115d, 79523.22083d, -226588.37473d, -121017.23944d, -48472.25935d, -74195.36778d, -7962.48081d, -4607.76339d, -4597.33274d, -7983.12541d, -20.345d, 56.82999d, -1038.19507d, 619.69624d, 1.08907d, -0.91278d, -0.13391d, 0.34956d, -0.19982d, -0.18296d, -0.97688d, 2.36806d, -0.30127d, 0.5098d, 0.96103d, 1.96432d, 0.43338d, 0.87317d, 0.36997d, -0.01583d, -0.44692d, -0.25159d, -0.53525d, 0.01154d, -0.13231d, 0.35562d, 3.88928d, -4.02882d, 1.06967d, -0.56305d, -0.45204d, 0.77213d, -0.82873d, -0.25854d, 0.21136d, -1.06696d, 458529.05491d, 616790.47568d, 698431.01349d, 1124501.41713d, 300226.10339d, 766533.33698d, 26896.22954d, 207880.7572d, 1116.29607d, 21793.26153d, -850.64044d, 3528.95568d, 29.61278d, -120.13367d, 376.95131d, 66.45758d, -3.64868d, 2.76062d, -0.85352d, 0.95115d, 5.35056d, 2.52803d, 0.90026d, 0.76403d, 0.43191d, 0.83605d, 125.81792d, -39.65364d, 50.14425d, -5.75891d, 2.78555d, 2.05055d, -4.27266d, -4.92428d, 6.78868d, 5.73537d, 3.35229d, -3.70143d, 0.08488d, 1.07465d, 0.10227d, 0.06074d, 0.00291d, 0.01522d, -0.02274d, 0.00297d, 0.01095d, -0.01856d, -0.02862d, 0.00178d, 143640.07486d, 707.21331d, 177163.08586d, 53386.52697d, 56856.89297d, 48268.74645d, 1764.52814d, 7711.76224d, 352.34159d, -968.03169d, -45.16568d, -81.60481d, -76.35993d, -98.06932d, -1.42185d, 1.81425d, -0.23427d, 0.59023d, 0.57127d, -0.36335d, 1.89975d, 0.6689d, 0.28797d, 0.43592d, -0.03769d, 0.03273d, -6.06571d, -2.68515d, -0.55315d, 0.86977d, 1.5384d, -0.59422d, -0.05453d, 0.02447d, -0.12658d, 0.22814d, -0.01715d, 0.08497d, -0.01288d, -0.00606d, 0.01547d, -0.00692d, 0.01157d, 0.02407d, -0.03883d, 0.00835d, -0.01542d, -0.04761d, 174386.39024d, 158048.26273d, 159192.81681d, 220154.55148d, 33716.11953d, 87537.86597d, -116.90381d, 7535.83928d, -962.06994d, -132.28837d, -644.90482d, -110.52332d, 3.42499d, 3.7466d, -0.94008d, 41.55548d, -0.03824d, -0.05607d, 28.74787d, -37.31399d, 30.87853d, -26.1194d, 10.79742d, -5.97905d, 1.01237d, -0.04429d, 0.54402d, 0.41905d, -0.0244d, -0.03991d, -0.00347d, -0.04362d, -0.00347d, -0.00469d, -0.02707d, 0.02761d, -0.17773d, -0.11789d, 26475.0258d, 35363.04345d, 19877.11475d, 41430.3594d, 2948.09998d, 12983.41406d, 281.93744d, 570.70054d, 147.83157d, 16.0009d, -1.62814d, -8.30846d, 9.29131d, -10.16496d, -0.15799d, 0.03843d, 1.44716d, 0.46953d, -0.0215d, -0.02502d, 0.08861d, -0.0669d, 2237.41551d, 3739.08722d, 753.74867d, 3460.41553d, -298.69226d, 520.47031d, -33.62615d, -138.12767d, 3.61843d, -8.2986d, -4.56656d, 0.79553d, 0.20041d, -0.25771d, -0.35233d, -0.27913d, -0.02799d, -0.08328d, -0.06889d, -0.16853d, 0.01701d, -0.00964d, -0.37737d, 0.1803d, -0.08525d, 0.01906d, 0.05236d, -0.05155d, 0.1132d, 0.05991d, -5.66926d, -0.54402d, -2.08508d, -0.39407d, 0.82155d, -0.55975d, 0.39168d, -0.25551d, 0.00623d, 0.16162d, -0.02519d, 0.0242d, -1.23293d, -3.19649d, -0.60519d, -2.79729d, 0.05362d, -0.61569d, -0.25638d, -0.27033d, -0.03987d, 0.46623d, -0.1207d, 0.00643d, 0.00849d, -0.00768d, -0.03687d, 0.10445d, -0.13544d, -0.00592d, 0.02078d, 0.09172d, 0.15824d, 0.15815d, 0.0202d, 0.00747d, 0.10919d, 0.09553d, 0.01953d, -0.00135d, 0.04266d, -0.00218d, 0.02182d, -0.13742d, -0.01249d, 0.01724d, -0.022d, 0.02975d, -0.01401d, 0.03416d, -0.28873d, 0.04235d, -0.08137d, 0.04223d, -0.00326d, 0.02144d, -0.40423d, 0.14281d, -0.08256d, 0.02142d, 0.08116d, -0.0368d, -0.02324d, 0.0726d, -0.06746d, 0.11645d, 0.03233d, -0.05997d, -0.03101d, 0.02197d, -0.00896d, -0.00491d, 0.00574d, 0.00855d, 0.00052d, 0.01209d, -0.31828d, 0.29955d, -0.08133d, 0.04318d, 0.06787d, -0.08865d, -0.13228d, -0.06507d, 0.34008d, 0.06417d, -0.00177d, -0.15116d, -0.00553d, -0.0195d, 0.01144d, -0.00309d, -0.00115d, -0.00153d, 0.02063d, -0.00791d, -0.00314d, 0.00493d, -0.10614d, 0.08338d, 0.08845d, 0.20168d, 1.38955d, -2.52285d, -0.30475d, -1.05787d, 0.0058d, 0.06623d, -44.33263d, -47.70073d, -29.80583d, -8.77838d, 7.02948d, 2.77221d, 0.05248d, -0.13702d, -0.78176d, 1.77489d, -16.32831d, 46.68457d, 2.54516d, 21.78145d, -5.0908d, -8.42611d, -0.24419d, -0.03315d, 2.80629d, -1.12755d, -0.00402d, 0.00053d, 0.00024d, -0.00043d, 0.00403d, -0.0021d, 0.00603d, 0.00411d, -0.0026d, 0.00416d, 2.29235d, 3.05992d, 2.36465d, -0.5875d, 0.1403d, 0.13523d, 0.89998d, 0.70156d, -0.02188d, 0.02003d, -0.00533d, 0.00447d, 2.96411d, 1.30183d, 0.01422d, 0.00624d, -0.10737d, -0.38316d, -0.05968d, 0.04379d, 0.01171d, 0.0118d, -0.00989d, -0.01375d, -0.00845d, 0.03782d, 0.09484d, 0.09909d, 0.0764d, -0.00898d, -0.01076d, 0.0276d, 0.0163d, 0.02198d, 0.05985d, 0.0413d };
        internal static double[] tabr = new double[] { 17990649.12487d, 24806479.30874d, 12690953.00645d, 2892671.69562d, 249947.71316d, -5138.71425d, 1142.68629d, 6075.25751d, -34.76785d, -19.72399d, -15.81516d, 30.47718d, -11.73638d, 21.87955d, 9.42107d, -10.40957d, -5.5967d, 6.85778d, -167.06735d, -2.31999d, -32.42575d, -13.72714d, 130.16635d, 117.97555d, 31.33915d, 39.64331d, -1378.54934d, -395.83244d, -562.79856d, -167.74359d, -45.12476d, -17.08986d, -4.20576d, -16.56724d, 1762.12089d, -1148.86987d, 736.5532d, -423.09108d, 56.13621d, -26.26674d, 9.7781d, -38.05151d, 4702224.98754d, 27254904.94363d, 5306232.25993d, 39518429.29982d, 1725110.05669d, 21833263.27069d, 46010.62605d, 5425411.66252d, 17238.09865d, 536771.62156d, -61263.36051d, 66270.70142d, 2084.66296d, -1936.71208d, 35898.49503d, 34885.28549d, 1.93276d, 10.66292d, -665.11445d, 3.70467d, -265.68478d, 16.16272d, -19.45954d, 2.32738d, 3.04237d, 3.97339d, -2.64312d, 0.66308d, -3207.68754d, 3418.0372d, -2342.6231d, 1729.1503d, -450.84643d, 179.00943d, -13.20367d, -1.86087d, -4.95659d, 7.22347d, -5.0889d, -1.28891d, -6.21713d, 5.10551d, 13.97276d, 0.44529d, 3.25177d, 25.02775d, -45.56672d, 11.5847d, 124443355.5545d, -100018293.41775d, 190506421.77863d, -118262753.40162d, 108199328.45091d, -45247957.63323d, 27272084.41143d, -4125106.01144d, 2583469.66051d, 1024678.12935d, -22702.55109d, 199269.51481d, -15783.14789d, 5564.52481d, -427.22231d, -6330.86079d, -97.50757d, -204.32241d, -9060.54822d, 156661.77631d, -47791.83678d, 59725.58975d, -8807.74881d, -92.38886d, -28886.11572d, -244419.59744d, -53336.36915d, -92232.16479d, -8724.89354d, -2446.76739d, 889.71335d, 936.51108d, 494.80305d, 2252.83602d, -18326.60823d, -25443.13554d, -3130.86382d, -5426.29135d, 23494.08846d, 91.28882d, 4664.14726d, 1552.06143d, -8090.43357d, 2843.48366d, -1445.73506d, 1023.11482d, 11664.20863d, -7020.08612d, 3100.21504d, -64.16577d, -9724.97938d, -12261.47155d, -3008.08276d, -1523.06301d, 6788.74046d, 10708.27853d, 343.09434d, 1701.5276d, 14743.99857d, -4781.96586d, -15922236.41469d, 1825172.51825d, -14006084.36972d, 10363332.64447d, -979550.9136d, 6542446.18797d, 1160614.26915d, 570804.88172d, 89912.68112d, -171247.08757d, -13899.52899d, -6182.25841d, -240.64725d, 412.42581d, -66.2451d, 71.30726d, -15.81125d, -15.76899d, -21.85515d, -102.12717d, -10.18287d, -19.38527d, 1.43749d, -3.87533d, 1.97109d, 0.20138d, 0.32012d, 1.02928d, -40.22077d, 20.80684d, -15.69766d, 9.63663d, -1.2601d, 0.56197d, 0.08592d, 0.1854d, -0.07303d, 0.03897d, 0.01438d, -0.08809d, 0.15479d, 0.10354d, 0.19052d, 2.0879d, 405480.24475d, -607986.83623d, 582811.58843d, -915111.10396d, 258696.21023d, -493391.09443d, 23403.62628d, -119503.67282d, -4036.86957d, -9766.17805d, -663.93268d, 2544.07799d, 40.36638d, 76.2639d, 246.67716d, -13.9344d, 0.12403d, 0.25378d, 0.14004d, -0.08501d, 0.07904d, 0.12731d, 1.02117d, -1.34663d, 0.25142d, -0.26903d, 0.18135d, -0.57683d, -0.30092d, -0.36121d, -0.09623d, 0.05873d, -0.05803d, 0.02869d, -0.01194d, 0.04983d, 0.0425d, 0.04894d, 1.34245d, 0.70137d, 0.24217d, 0.25912d, -0.32759d, -0.03575d, 0.0678d, -0.41277d, 0.43865d, 0.17857d, -763933.02226d, 465658.17048d, -1082753.91241d, 593319.68634d, -553911.8934d, 274748.95145d, -122250.71547d, 56608.95768d, -9914.173d, 2988.43709d, 707.94605d, -765.0147d, 52.7326d, -34.22263d, -43.583d, -38.43647d, -4.95939d, -1.97173d, -1.04406d, -0.13072d, -0.34281d, 4.75202d, -0.35513d, 0.93597d, -0.5438d, 0.70536d, 84.83116d, 102.93003d, 26.34884d, 48.57746d, 0.02853d, 2.91676d, -8.07116d, 1.66613d, -2.07908d, 11.62592d, 6.64704d, 0.98291d, -1.19192d, 0.93791d, 0.18822d, 0.009d, -0.03181d, -0.02d, 0.02755d, -0.01398d, -0.03971d, -0.03756d, 0.13454d, -0.04193d, -18672.98484d, 28230.75834d, -28371.58823d, 26448.45214d, -13352.09393d, 7461.71279d, -2609.33578d, 726.50321d, -309.72942d, -86.71982d, 12.48589d, -9.69726d, 1.82185d, 14.9222d, -0.04748d, 0.4251d, -0.20047d, 0.00154d, 0.00176d, -0.26262d, 0.78218d, -0.73243d, 0.23694d, -0.03132d, -0.0029d, -0.03678d, 14.03094d, 4.25948d, 0.79368d, -0.78489d, -2.30962d, 2.31946d, 0.00158d, -0.04125d, -0.01387d, 0.28503d, 0.00892d, 0.05154d, 0.00184d, -0.01727d, -0.00889d, 0.03526d, -0.00521d, -0.02093d, 0.002d, 0.04872d, -0.02163d, 0.00578d, 20699.27413d, -2175.57827d, 31177.33085d, 4572.02063d, 15486.2819d, 8747.74091d, 2455.51737d, 3839.83609d, 51.31433d, 507.91086d, 15.90082d, 44.75942d, -0.98374d, -2.64477d, 2.52336d, -3.09203d, -0.08897d, -0.00083d, -15.91892d, 0.72597d, 14.04523d, -3.16525d, 4.33379d, -30.8298d, 0.40462d, -0.75845d, 13.14831d, -0.02721d, -0.01779d, 0.00481d, 0.42365d, -0.09048d, 0.08653d, 0.04391d, 0.00846d, 0.01082d, -0.04736d, 0.02308d, 6282.21778d, -4952.70286d, 7886.57505d, -5328.36122d, 3113.76826d, -1696.8459d, 330.70011d, -155.51989d, -18.31559d, -3.90798d, -3.11242d, 1.87818d, -1.05578d, 0.11198d, 0.05077d, -0.01571d, 2.41291d, 2.40568d, -0.01136d, -0.00076d, -0.00392d, -0.02774d, 634.85065d, -352.21937d, 674.31665d, -260.73473d, 199.16422d, -28.44198d, 6.54187d, 6.4496d, -1.55155d, 0.29755d, 0.16977d, 0.1754d, -0.02652d, 0.03726d, -0.00623d, 0.11777d, -0.00933d, 0.02602d, -0.13943d, -0.24818d, 0.02876d, -0.01463d, -0.07166d, 0.06747d, -0.01578d, 0.01628d, 0.00233d, -0.00686d, 0.00431d, -0.00276d, 0.21774d, 0.09735d, 0.07894d, 0.07279d, -0.013d, -0.00268d, 0.10824d, 0.09435d, 0.0072d, 0.02111d, -0.0196d, 0.06154d, 0.56867d, -0.07544d, 0.1821d, 0.06343d, -0.00906d, 0.01942d, -0.0085d, -0.00351d, -0.06988d, 0.01713d, -0.0111d, -0.00663d, 0.00196d, -0.02064d, -0.00008d, 0.00043d, 0.00375d, 0.00084d, -0.00279d, 0.001d, 0.00271d, -0.02017d, -0.00074d, -0.00357d, 0.03793d, -0.10108d, -0.01083d, -0.03952d, 0.0003d, 0.00012d, 0.01576d, 0.01142d, 0.00351d, 0.00277d, 0.01409d, -0.00774d, -0.00065d, 0.01895d, 0.0735d, -0.02519d, 0.01528d, -0.01057d, -0.00099d, -0.00295d, 0.21347d, -0.17458d, 0.0494d, -0.02757d, -0.06243d, 0.05203d, 0.01055d, -0.00109d, 0.00003d, -0.04201d, -0.00263d, 0.02387d, 0.00886d, -0.01168d, 0.00479d, 0.00204d, -0.00239d, 0.00022d, -0.00223d, -0.02029d, -0.1413d, -0.15237d, -0.01827d, -0.04877d, 0.12104d, 0.06796d, 0.16379d, 0.31892d, -0.15605d, 0.07048d, -0.007d, 0.07481d, -0.0037d, -0.00142d, -0.00446d, 0.00329d, -0.00018d, 0.00117d, -0.0091d, 0.0051d, -0.00055d, -0.00114d, 0.04131d, -0.04013d, -0.13238d, 0.0268d, -0.10369d, 1.38709d, 0.35515d, 0.41437d, -0.01327d, -0.02692d, 38.02603d, 13.38166d, 15.33389d, -7.40145d, -8.55293d, -0.13185d, -0.03316d, 0.13016d, 0.04428d, -1.60953d, -12.87829d, -76.97922d, -23.96039d, -22.45636d, 14.83309d, 14.09854d, 0.24252d, 0.1385d, -4.16582d, 4.08846d, 0.00751d, -0.00051d, 0.03456d, 0.029d, 0.01625d, -0.0466d, 0.0139d, -0.0053d, 0.01665d, -0.04571d, 40.90768d, -14.11641d, 7.46071d, -58.07356d, -0.27859d, -1.33816d, 23.76074d, -0.03124d, -0.2786d, 0.13654d, -0.048d, 0.05375d, 4.38091d, 4.39337d, 0.02233d, 0.00514d, -0.25616d, -0.54439d, -0.05155d, 0.11553d, 0.02944d, -0.00818d, 0.0057d, 0.00119d, -0.00733d, -0.027d, -0.23759d, -0.08712d, -0.12433d, 0.07397d, 0.20629d, 0.60251d, 0.56512d, 0.1479d, 0.07778d, 0.11614d };
        internal static int[] args = new int[] { 0, 7, 2, 3, 7, -9, 9, 0, 2, 4, 7, -12, 9, 0, 2, 4, 7, -8, 8, 0, 3, -4, 7, 5, 8, 4, 9, 0, 3, 3, 7, -5, 8, -1, 9, 0, 2, 1, 6, -8, 9, 1, 2, 3, 8, -5, 9, 1, 2, 1, 6, -9, 9, 2, 3, 6, 7, -6, 8, -8, 9, 0, 3, 4, 7, -10, 8, 4, 9, 2, 2, 3, 7, -8, 9, 0, 1, 1, 9, 7, 2, 3, 7, -10, 9, 0, 3, 4, 7, -10, 8, 2, 9, 2, 3, 5, 7, -12, 8, 2, 9, 0, 2, 1, 6, -7, 9, 0, 1, 1, 8, 3, 2, 1, 6, -10, 9, 0, 3, 6, 7, -12, 8, 2, 9, 0, 3, 5, 7, -10, 8, 2, 9, 0, 2, 5, 7, -13, 9, 0, 2, 4, 7, -10, 9, 0, 2, 3, 7, -7, 9, 0, 1, 2, 9, 7, 2, 3, 7, -11, 9, 0, 3, 4, 7, -9, 8, 4, 9, 2, 3, 3, 7, -5, 8, 1, 9, 2, 2, 1, 6, -6, 9, 0, 2, 7, 8, -13, 9, 0, 2, 3, 8, -2, 9, 1, 3, 1, 7, -5, 8, 2, 9, 1, 3, 6, 7, -12, 8, 3, 9, 1, 2, 5, 7, -12, 9, 1, 2, 4, 7, -9, 9, 1, 2, 2, 7, -3, 9, 1, 1, 1, 7, 0, 1, 3, 9, 5, 2, 3, 7, -12, 9, 1, 3, 5, 7, -9, 8, 2, 9, 0, 3, 4, 7, -7, 8, 2, 9, 1, 3, 3, 7, -5, 8, 2, 9, 0, 3, 2, 7, -5, 8, 5, 9, 0, 2, 1, 6, -5, 9, 0, 2, 3, 8, -1, 9, 2, 2, 1, 6, -12, 9, 0, 3, 2, 7, -7, 8, 1, 9, 0, 2, 5, 7, -11, 9, 0, 2, 4, 7, -8, 9, 0, 2, 2, 7, -2, 9, 0, 1, 4, 9, 7, 3, 2, 7, -8, 8, 2, 9, 0, 3, 5, 7, -9, 8, 3, 9, 0, 3, 4, 7, -9, 8, 6, 9, 0, 3, 3, 7, -5, 8, 3, 9, 1, 2, 2, 7, -1, 8, 1, 2, 3, 8, -9, 9, 0, 2, 9, 8, -9, 9, 0, 2, 1, 6, -13, 9, 0, 3, 2, 7, -5, 8, -3, 9, 0, 2, 6, 7, -13, 9, 1, 2, 5, 7, -10, 9, 0, 2, 4, 7, -7, 9, 0, 2, 3, 7, -4, 9, 0, 1, 5, 9, 7, 3, 6, 7, -9, 8, 1, 9, 1, 3, 4, 7, -5, 8, 1, 9, 1, 3, 3, 7, -3, 8, 1, 9, 0, 2, 1, 6, -3, 9, 2, 2, 3, 8, -10, 9, 0, 2, 1, 8, 4, 9, 0, 2, 5, 8, -2, 9, 0, 2, 11, 8, -11, 9, 0, 3, 1, 7, -9, 8, 5, 9, 0, 2, 6, 7, -12, 9, 0, 2, 5, 7, -9, 9, 0, 2, 4, 7, -6, 9, 0, 2, 3, 7, -3, 9, 0, 1, 6, 9, 6, 2, 2, 7, -12, 9, 0, 3, 6, 7, -9, 8, 2, 9, 0, 3, 3, 7, -12, 8, 3, 9, 0, 3, 4, 7, -10, 8, -3, 9, 1, 3, 3, 7, -3, 8, 2, 9, 0, 2, 1, 6, -2, 9, 2, 2, 1, 8, 5, 9, 0, 2, 13, 8, -13, 9, 1, 3, 2, 7, -9, 8, 1, 9, 0, 2, 6, 7, -11, 9, 0, 2, 5, 7, -8, 9, 0, 2, 4, 7, -5, 9, 0, 2, 3, 7, -2, 9, 0, 1, 7, 9, 7, 3, 6, 7, -9, 8, 3, 9, 0, 2, 1, 6, -1, 9, 4, 2, 3, 8, 3, 9, 0, 2, 7, 7, -13, 9, 1, 2, 3, 7, -1, 9, 0, 2, 2, 7, 2, 9, 0, 1, 8, 9, 6, 3, 7, 7, -9, 8, 1, 9, 0, 1, 1, 6, 0, 1, 3, 7, 0, 2, 2, 7, 3, 9, 0, 1, 9, 9, 5, 3, 1, 7, -10, 8, 3, 9, 0, 3, 2, 7, -12, 8, 3, 9, 1, 2, 1, 6, 1, 9, 0, 3, 1, 7, -1, 8, 8, 9, 0, 2, 3, 7, 1, 9, 1, 2, 2, 7, 4, 9, 0, 2, 1, 7, 7, 9, 0, 2, 4, 8, 4, 9, 1, 2, 12, 8, -8, 9, 0, 3, 1, 7, -10, 8, 2, 9, 1, 2, 1, 6, 2, 9, 0, 1, 11, 9, 2, 2, 12, 8, -7, 9, 0, 3, 1, 7, -10, 8, 1, 9, 1, 1, 4, 7, 0, 1, 12, 9, 0, 2, 6, 8, 3, 9, 0, 3, 1, 7, -2, 8, -12, 9, 0, 3, 7, 7, -7, 8, 2, 9, 1, 2, 2, 6, -4, 9, 1, 1, 13, 9, 0, 2, 10, 8, -2, 9, 1, 2, 4, 7, 2, 9, 0, 2, 2, 6, -3, 9, 0, 2, 2, 7, 8, 9, 1, 2, 8, 8, 2, 9, 0, 1, 5, 7, 1, 2, 4, 7, 3, 9, 0, 2, 3, 7, 6, 9, 0, 2, 1, 5, -6, 9, 0, 3, 2, 7, 8, 8, -3, 9, 0, 3, 1, 7, 6, 8, 3, 9, 0, 2, 6, 8, 6, 9, 0, 3, 8, 7, -7, 8, 2, 9, 0, 2, 9, 7, -11, 9, 0, 2, 5, 7, 1, 9, 1, 2, 4, 7, 4, 9, 0, 2, 2, 6, -1, 9, 0, 3, 2, 6, -1, 7, 2, 9, 0, 2, 2, 7, 10, 9, 0, 2, 1, 7, 13, 9, 0, 2, 8, 7, -7, 9, 0, 2, 7, 7, -4, 9, 0, 2, 6, 7, -1, 9, 0, 2, 5, 7, 3, 9, 0, 2, 4, 7, 5, 9, 0, 1, 2, 6, 0, 2, 1, 5, -4, 9, 1, 3, 1, 6, 9, 8, -5, 9, 0, 2, 1, 5, -3, 9, 4, 2, 1, 5, -2, 9, 4, 3, 9, 7, -9, 8, 6, 9, 0, 2, 8, 7, -4, 9, 0, 2, 7, 7, -1, 9, 0, 2, 1, 6, 3, 9, 0, 2, 2, 6, 3, 9, 0, 2, 1, 5, -1, 9, 3, 3, 6, 7, -3, 8, 7, 9, 1, 1, 1, 5, 0, 2, 2, 6, 5, 9, 0, 2, 1, 5, 1, 9, 0, 2, 1, 5, 2, 9, 0, 2, 1, 5, 3, 9, 0, 2, 2, 5, -4, 9, 0, 2, 2, 5, -3, 9, 0, 2, 2, 5, -2, 9, 1, 2, 2, 5, -1, 9, 1, 1, 2, 5, 0, -1 };

        // /* Total terms = 173, small = 156 */
        internal static KeplerGlobalCode.plantbl plu404 = new(9, new int[19] { 0, 0, 0, 0, 2, 2, 9, 13, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 7, args, tabl, tabb, tabr, 39.54d, 3652500.0d, 1.0d);

    }
}