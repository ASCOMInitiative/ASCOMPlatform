﻿Namespace KEPLER
    Module Plu404Data
        '/*
        'First date in file = 625296.50
        'Number of records = 16731.0
        'Days per record = 131.0
        'Julian Years      Lon    Lat    Rad
        '-3000.0 to  -2499.7:   1.17   0.90   0.83
        '-2499.7 to  -1999.7:   0.57   0.51   0.58
        '-1999.7 to  -1499.7:   0.63   0.39   0.40
        '-1499.7 to   -999.8:   0.40   0.45   0.41
        '-999.8 to   -499.8:   0.42   0.22   0.30
        '-499.8 to      0.2:   0.41   0.24   0.35
        '0.2 to    500.2:   0.58   0.24   0.26
        '500.2 to   1000.1:   0.47   0.35   0.33
        '1000.1 to   1500.1:   0.43   0.31   0.28
        '1500.1 to   2000.1:   0.37   0.40   0.35
        '2000.1 to   2500.0:   0.46   0.35   0.39
        '2500.0 to   3000.0:   1.09   0.70   0.46
        '3000.0 to   3000.4:  0.871  0.395  0.051
        '*/

        Friend tabl() As Double = { _
        74986469.33577, 100898343.7369, 48199471.54076, 9520076.03177, _
        690431.6734, -427355.12716, 52266623.77862, 860190.70714, _
        -21.08511, -143.39295, _
        -126.71124, 48.18528, _
        -88.74508, 40.50942, _
        51.29367, -10.24075, _
        0.63094, 32.03258, _
        -410.00781, 399.90234, -116.54319, 51.50329, _
        116.84565, 687.76781, -13.38605, 182.70107, _
        -1668.12226, -5162.22648, -585.68297, -2247.56041, _
        -20.95875, -193.13703, _
        -57.12097, -10.35058, _
        -1778.01442, -6463.73779, -657.86093, -2713.44436, _
        -41.3256, -211.82042, _
        -107.16513, -36.58628, _
        97929588.08231, -33582903.63417, 143382679.3177, -47411568.92345, _
        79428827.73581, -24658834.87499, 19861233.17488, -5755585.62084, _
        1995060.95931, -693507.08147, 135176.31467, 109360.38672, _
        -8188.00598, -1680.95072, 71020.19608, -70785.39049, _
        -24.56034, -20.34919, _
        1618.45976, -2079.48538, 635.62954, -850.87068, _
        44.95257, -64.04459, _
        -18.61475, -1.77734, _
        9.38079, 5.19958, _
        17422.08783, -4872.53852, 10985.76629, -1137.68569, _
        1771.28893, 288.93745, 40.22664, 37.90027, _
        6.81361, -32.65868, _
        16.97268, 11.76152, _
        29.33024, -12.92289, _
        -55.13187, -14.73791, _
        7.52474, -102.0506, _
        182.53144, -20.1896, _
        -490237997.494, 486646248.6336, -781277018.2643, 602300460.5729, _
        -463787999.4642, 249529525.961, -123964746.8642, 31353019.97807, _
        -13353800.92159, -3463382.63269, -35469.17654, -1035343.45385, _
        65076.64025, -38181.61312, -16473.33813, 3928.44674, _
        188.60263, 1000.4253, _
        -208376.39376, -700566.62363, 114839.84613, -342407.71113, _
        39467.04812, -14553.84849, _
        581895.261, 1012499.16715, 406317.22416, 310804.78515, _
        43625.07033, -4157.26545, _
        -5930.13795, -2070.62413, _
        3348.17582, -10871.23729, _
        144609.1855, 60383.6365, 27912.02226, 15254.61228, _
        -98561.37758, -67442.28158, -15573.63338, -19931.99773, _
        24323.06905, -37473.32558, 2840.64042, -8911.23694, _
        -19636.31898, 71725.21946, -12280.54554, 12251.00101, _
        88626.5226, 5513.6845, 18506.41546, -6513.87434, _
        -83350.14621, 44300.00743, -22075.37353, 3731.57531, _
        -29167.7602, -21642.67384, _
        56602666.72177, -22225578.01823, 50576897.80669, -50319847.79086, _
        5689259.25622, -29585299.79697, -4249711.27661, -4490830.29568, _
        -727678.08724, 366050.85631, 19183.62792, 55647.98226, _
        1897.78091, -1091.03988, 432.38158, -138.62556, _
        101.38743, 25.67379, _
        320.20735, 362.16615, 85.06067, 54.02616, _
        2.3946, 18.70004, _
        -8.43353, 2.721, _
        -3.11205, -3.06201, _
        136.31503, -28.3362, 48.68781, -18.45285, _
        1.15302, -1.5236, _
        -0.13706, -0.37489, _
        0.0875, -0.14579, _
        -0.07051, -0.06518, _
        0.30237, -0.00448, _
        4.83172, 6.83684, _
        1752447.78043, -945086.75857, 2340978.12819, -1963675.42559, _
        1254147.25257, -1274861.91191, 279459.60237, -263954.01378, _
        11835.6229, -16344.44434, 9119.9896, -2390.44218, _
        -23.67909, 86.73916, -642.78635, -1290.12208, _
        -0.43345, -1.85348, _
        0.03094, -0.01655, _
        0.1238, 0.31834, _
        5.54756, -1.63109, 1.10598, -0.17578, _
        2.66994, -2.17573, 0.9736, -0.92226, _
        -0.18533, -0.39747, _
        0.45809, -0.65286, _
        0.26129, 0.91922, _
        0.81068, 0.11183, _
        6.32182, 14.16786, 0.20872, 3.28489, _
        -1.47458, -2.11724, _
        1.7002, -1.99889, _
        3.13003, 1.90638, _
        -4483669.52795, -446454.90158, -6586256.67478, -671890.16779, _
        -3620444.55554, -499672.41074, -855998.32655, -191073.94266, _
        -73186.6911, -22649.38582, -2414.81729, -1294.40542, _
        436.80907, 125.48109, -81.16877, 458.86508, _
        -11.57414, -26.39114, -4.00801, -5.01054, _
        -18.17569, 20.86879, -4.80965, 3.10535, _
        -4.71122, 1.18169, _
        74.75544, 649.21464, -26.5506, 272.35592, _
        -8.06982, 16.8611, _
        -26.54868, 26.75711, _
        -35.8291, 38.51063, _
        22.22814, 19.38336, _
        -6.30462, 0.90602, _
        0.62856, -0.34981, _
        -0.10232, -0.00939, _
        0.04439, -0.18875, _
        0.16025, 0.11306, _
        -0.06803, 0.06004, _
        -91305.66728, 262370.61704, -194633.44577, 304838.17733, _
        -124505.90904, 94111.75602, -22317.18255, 1575.23438, _
        748.66316, -349.78711, 166.6445, -89.05045, _
        120.76207, -100.26715, _
        3.13806, 3.71747, _
        -1.44731, -0.35235, _
        -0.5166, -1.50621, _
        2.8131, -3.93573, 1.20292, -0.36412, _
        -0.0334, -0.00561, _
        -5.29764, 26.02941, 1.91382, 3.30686, _
        -3.35265, -3.20868, _
        0.05807, -0.11885, _
        -0.78588, 0.34807, -0.19038, 0.11295, _
        -0.03642, -0.03794, _
        0.00251, 0.03449, _
        -0.08426, -0.0031, _
        0.05297, -0.09278, _
        0.10941, 0.00099, _
        -228688.56632, 312567.73069, -331458.31119, 328200.1946, _
        -143760.57524, 104182.01134, -17313.30132, 12591.15513, _
        -440.32735, -105.67674, 104.35854, -852.8459, _
        0.95527, 0.30212, -54.63983, 4.06948, _
        0.07545, -0.13429, _
        16.21005, 29.24658, 9.2341, 50.48867, _
        30.55641, 12.76809, 0.11781, 0.70929, _
        -0.041, 13.60859, _
        0.04976, -0.02083, _
        0.36279, 0.3013, -0.02129, 0.09363, _
        -0.07812, 0.0157, _
        -0.06217, -0.37181, _
        -29348.55031, 43889.87672, -35765.41577, 33855.9007, _
        -10128.69894, 4535.32148, 281.75353, -218.49194, _
        -7.55224, 134.2864, 2.11319, -2.13109, _
        15.71244, 11.07183, _
        -0.05406, -0.23337, _
        -1.28949, 1.34281, _
        0.04212, -0.0208, _
        0.08109, 0.1482, _
        -6010.46564, 3639.4178, -5973.16, 1381.66999, _
        -1177.36865, -501.06937, 166.14792, -103.36431, _
        14.92766, 4.12877, -2.20893, -6.32033, _
        -0.29038, -0.43172, _
        -0.59341, 0.20477, -0.13143, -0.0315, _
        0.10992, 0.01976, _
        -0.00254, 0.02028, _
        -0.30044, -0.44658, -0.03409, -0.10758, _
        0.08349, 0.06153, _
        -0.06055, 0.18249, _
        -1.15341, -8.68699, -0.11348, -3.30688, _
        1.08604, 1.04018, _
        -0.46892, -0.69765, 0.21504, 0.01968, _
        -0.00455, -0.01678, _
        3.95643, -3.17191, 3.9522, -2.1267, _
        0.99305, -0.16651, _
        0.34839, -0.49162, _
        0.85744, 0.20173, -0.00975, 0.20225, _
        -0.02627, -0.02281, _
        -0.18002, -0.01803, _
        -0.06144, -0.2151, _
        0.15935, -0.01251, _
        -0.21378, 0.44806, -0.01174, 0.05779, _
        0.07646, -0.19656, -0.04044, -0.02521, _
        0.02996, 0.06169, _
        0.16698, -0.0471, -0.06506, -0.02114, _
        0.055, 0.00276, _
        0.08433, 0.0316, _
        0.08193, 0.35773, 0.05454, 0.10718, _
        -0.02823, -0.00839, _
        0.54078, 0.49347, 0.09609, 0.11825, _
        -0.16092, -0.11897, _
        0.09059, 0.08254, _
        0.16712, 0.0586, _
        -0.09547, -0.03206, _
        0.03876, 0.04719, _
        -0.02345, 0.0224, _
        -0.00609, -0.00649, _
        0.03859, 0.00077, _
        0.47819, 0.26196, 0.0978, 0.08104, _
        -0.16919, 0.05042, _
        -0.42652, 0.3081, _
        -0.03409, -0.51452, _
        -0.2312, -0.0138, _
        -0.01157, -0.00143, _
        -0.00512, -0.01628, _
        -0.00189, 0.00183, _
        -0.01427, -0.02861, _
        0.00618, -0.00015, _
        0.13087, 0.1387, _
        0.15158, -0.21056, _
        -3.94829, -1.06028, -1.36602, 0.77954, _
        0.08709, -0.03118, _
        -44.74949, 91.17393, 8.78173, 45.8401, _
        1.9756, -15.02849, -0.10755, -0.02884, _
        3.3867, 0.30615, _
        130.92778, -24.33209, 43.01636, -40.81327, _
        -19.439, 22.18162, -0.12691, 0.33795, _
        -6.4479, -6.23145, _
        0.00319, 0.01141, _
        -0.03252, 0.03872, _
        0.04467, 0.01614, _
        -0.00382, -0.00019, _
        0.05955, 0.01533, _
        16.11371, 41.37565, 61.44963, 6.90615, _
        1.41326, -0.7392, -0.03871, 24.81978, _
        -0.10229, -0.32775, -0.05188, -0.05628, _
        -2.33618, 2.39053, _
        -0.00584, 0.00436, _
        0.20903, 0.0222, _
        -0.01738, -0.02765, _
        -0.00217, 0.00613, _
        -0.01772, 0.01903, _
        0.07075, -0.0053, _
        0.15234, -0.3776, -0.11641, -0.20102, _
        -0.63675, 0.20525, -0.15783, 0.58945, _
        -0.06243, 0.04306}

        Friend tabb() As Double = { _
        -35042727.30412, -49049197.81293, -25374963.60995, -5761406.03035, _
        -467370.5754, 14040.11453, 2329.15763, -13978.6939, _
        45.43441, 29.70305, _
        32.33772, -38.34012, _
        26.43575, -28.76136, _
        -18.5904, 12.64837, _
        5.56569, -12.51581, _
        248.3735, -64.44466, 54.02618, 4.39466, _
        -269.35114, -290.63134, -48.03841, -52.83576, _
        1508.94995, 1682.78967, 554.02336, 715.65819, _
        34.37602, 58.44397, _
        16.63685, 16.10176, _
        -1069.51609, 2300.89166, -437.16796, 927.89245, _
        -33.17679, 68.74495, _
        18.72022, 32.9764, _
        -34004958.12619, -17758805.77098, -48416073.75788, -24973405.03542, _
        -25374996.23732, -13351084.9734, -5738294.54942, -3082092.6335, _
        -519989.39256, -206440.89101, 44186.23548, -87639.2263, _
        2506.47602, 2327.01164, -53878.47903, -19670.13471, _
        2.66934, -3.86086, _
        106.32427, 576.47944, 46.56388, 218.28339, _
        4.35402, 15.04642, _
        2.68717, -2.86835, _
        0.81728, -2.34417, _
        -1604.85823, -1999.24986, -631.47343, -1382.19156, _
        -15.74075, -256.97077, 6.99648, -4.54257, _
        2.63478, 1.88838, _
        0.17628, -2.11518, _
        -2.46735, -1.48743, _
        1.83456, 4.68487, _
        -7.10919, 3.57046, _
        -5.36342, -7.70367, _
        28395956.20816, -37176795.74372, 48969952.83034, -48145798.96248, _
        31155823.23557, -21163596.14822, 9057634.3826, -3167688.51696, _
        1167488.70078, 219103.97591, -19017.97335, 107849.61195, _
        -3814.43474, 4405.9212, 5800.13959, 12619.88708, _
        22.18168, -89.47801, _
        52202.81929, 55119.44083, 5082.58907, 37955.06062, _
        -3165.24355, 3316.67588, _
        -113906.4397, -69279.41495, -57358.07767, -10176.17329, _
        -4179.79867, 2495.99374, _
        787.8718, -154.35591, _
        -1148.62509, 1034.58199, _
        -22194.95235, 3341.97949, -4578.53994, 108.30832, _
        7444.39789, 16646.40725, 509.7543, 3808.92686, _
        -179.85869, 7408.76716, 340.65366, 1504.64227, _
        -3783.09873, -13505.60867, 875.74489, -3181.27898, _
        -16220.93983, 8041.37347, -2631.07448, 2899.50781, _
        18894.92095, -20072.81471, 5925.05701, -1947.91902, _
        -6731.56601, 8014.52403, _
        -987793.49463, 6491762.34471, -279205.73643, 6117135.96868, _
        -140925.91402, 2259422.06929, 114028.61646, 605600.90358, _
        91858.00186, 56506.65187, 8949.15777, -9782.67413, _
        -394.66541, -105.19208, -76.54752, -32.59411, _
        -19.28741, 10.40013, _
        -107.64003, -7.36229, -22.25126, 4.05952, _
        -3.74402, -2.79308, _
        1.03337, -2.13968, _
        1.53794, -0.02617, _
        35.70756, 12.97733, 14.46213, 6.20518, _
        1.79381, 1.65422, _
        -0.31216, 0.29053, _
        -0.03538, -0.01584, _
        -0.08934, 0.00079, _
        0.05539, -0.21591, _
        2.86929, -2.24724, _
        320797.07455, 93342.16556, -20903.39115, 79523.22083, _
        -226588.37473, -121017.23944, -48472.25935, -74195.36778, _
        -7962.48081, -4607.76339, -4597.33274, -7983.12541, _
        -20.345, 56.82999, -1038.19507, 619.69624, _
        1.08907, -0.91278, _
        -0.13391, 0.34956, _
        -0.19982, -0.18296, _
        -0.97688, 2.36806, -0.30127, 0.5098, _
        0.96103, 1.96432, 0.43338, 0.87317, _
        0.36997, -0.01583, _
        -0.44692, -0.25159, _
        -0.53525, 0.01154, _
        -0.13231, 0.35562, _
        3.88928, -4.02882, 1.06967, -0.56305, _
        -0.45204, 0.77213, _
        -0.82873, -0.25854, _
        0.21136, -1.06696, _
        458529.05491, 616790.47568, 698431.01349, 1124501.41713, _
        300226.10339, 766533.33698, 26896.22954, 207880.7572, _
        1116.29607, 21793.26153, -850.64044, 3528.95568, _
        29.61278, -120.13367, 376.95131, 66.45758, _
        -3.64868, 2.76062, -0.85352, 0.95115, _
        5.35056, 2.52803, 0.90026, 0.76403, _
        0.43191, 0.83605, _
        125.81792, -39.65364, 50.14425, -5.75891, _
        2.78555, 2.05055, _
        -4.27266, -4.92428, _
        6.78868, 5.73537, _
        3.35229, -3.70143, _
        0.08488, 1.07465, _
        0.10227, 0.06074, _
        0.00291, 0.01522, _
        -0.02274, 0.00297, _
        0.01095, -0.01856, _
        -0.02862, 0.00178, _
        143640.07486, 707.21331, 177163.08586, 53386.52697, _
        56856.89297, 48268.74645, 1764.52814, 7711.76224, _
        352.34159, -968.03169, -45.16568, -81.60481, _
        -76.35993, -98.06932, _
        -1.42185, 1.81425, _
        -0.23427, 0.59023, _
        0.57127, -0.36335, _
        1.89975, 0.6689, 0.28797, 0.43592, _
        -0.03769, 0.03273, _
        -6.06571, -2.68515, -0.55315, 0.86977, _
        1.5384, -0.59422, _
        -0.05453, 0.02447, _
        -0.12658, 0.22814, -0.01715, 0.08497, _
        -0.01288, -0.00606, _
        0.01547, -0.00692, _
        0.01157, 0.02407, _
        -0.03883, 0.00835, _
        -0.01542, -0.04761, _
        174386.39024, 158048.26273, 159192.81681, 220154.55148, _
        33716.11953, 87537.86597, -116.90381, 7535.83928, _
        -962.06994, -132.28837, -644.90482, -110.52332, _
        3.42499, 3.7466, -0.94008, 41.55548, _
        -0.03824, -0.05607, _
        28.74787, -37.31399, 30.87853, -26.1194, _
        10.79742, -5.97905, 1.01237, -0.04429, _
        0.54402, 0.41905, _
        -0.0244, -0.03991, _
        -0.00347, -0.04362, -0.00347, -0.00469, _
        -0.02707, 0.02761, _
        -0.17773, -0.11789, _
        26475.0258, 35363.04345, 19877.11475, 41430.3594, _
        2948.09998, 12983.41406, 281.93744, 570.70054, _
        147.83157, 16.0009, -1.62814, -8.30846, _
        9.29131, -10.16496, _
        -0.15799, 0.03843, _
        1.44716, 0.46953, _
        -0.0215, -0.02502, _
        0.08861, -0.0669, _
        2237.41551, 3739.08722, 753.74867, 3460.41553, _
        -298.69226, 520.47031, -33.62615, -138.12767, _
        3.61843, -8.2986, -4.56656, 0.79553, _
        0.20041, -0.25771, _
        -0.35233, -0.27913, -0.02799, -0.08328, _
        -0.06889, -0.16853, _
        0.01701, -0.00964, _
        -0.37737, 0.1803, -0.08525, 0.01906, _
        0.05236, -0.05155, _
        0.1132, 0.05991, _
        -5.66926, -0.54402, -2.08508, -0.39407, _
        0.82155, -0.55975, _
        0.39168, -0.25551, 0.00623, 0.16162, _
        -0.02519, 0.0242, _
        -1.23293, -3.19649, -0.60519, -2.79729, _
        0.05362, -0.61569, _
        -0.25638, -0.27033, _
        -0.03987, 0.46623, -0.1207, 0.00643, _
        0.00849, -0.00768, _
        -0.03687, 0.10445, _
        -0.13544, -0.00592, _
        0.02078, 0.09172, _
        0.15824, 0.15815, 0.0202, 0.00747, _
        0.10919, 0.09553, 0.01953, -0.00135, _
        0.04266, -0.00218, _
        0.02182, -0.13742, -0.01249, 0.01724, _
        -0.022, 0.02975, _
        -0.01401, 0.03416, _
        -0.28873, 0.04235, -0.08137, 0.04223, _
        -0.00326, 0.02144, _
        -0.40423, 0.14281, -0.08256, 0.02142, _
        0.08116, -0.0368, _
        -0.02324, 0.0726, _
        -0.06746, 0.11645, _
        0.03233, -0.05997, _
        -0.03101, 0.02197, _
        -0.00896, -0.00491, _
        0.00574, 0.00855, _
        0.00052, 0.01209, _
        -0.31828, 0.29955, -0.08133, 0.04318, _
        0.06787, -0.08865, _
        -0.13228, -0.06507, _
        0.34008, 0.06417, _
        -0.00177, -0.15116, _
        -0.00553, -0.0195, _
        0.01144, -0.00309, _
        -0.00115, -0.00153, _
        0.02063, -0.00791, _
        -0.00314, 0.00493, _
        -0.10614, 0.08338, _
        0.08845, 0.20168, _
        1.38955, -2.52285, -0.30475, -1.05787, _
        0.0058, 0.06623, _
        -44.33263, -47.70073, -29.80583, -8.77838, _
        7.02948, 2.77221, 0.05248, -0.13702, _
        -0.78176, 1.77489, _
        -16.32831, 46.68457, 2.54516, 21.78145, _
        -5.0908, -8.42611, -0.24419, -0.03315, _
        2.80629, -1.12755, _
        -0.00402, 0.00053, _
        0.00024, -0.00043, _
        0.00403, -0.0021, _
        0.00603, 0.00411, _
        -0.0026, 0.00416, _
        2.29235, 3.05992, 2.36465, -0.5875, _
        0.1403, 0.13523, 0.89998, 0.70156, _
        -0.02188, 0.02003, -0.00533, 0.00447, _
        2.96411, 1.30183, _
        0.01422, 0.00624, _
        -0.10737, -0.38316, _
        -0.05968, 0.04379, _
        0.01171, 0.0118, _
        -0.00989, -0.01375, _
        -0.00845, 0.03782, _
        0.09484, 0.09909, 0.0764, -0.00898, _
        -0.01076, 0.0276, 0.0163, 0.02198, _
        0.05985, 0.0413}

        Friend tabr() As Double = { _
        17990649.12487, 24806479.30874, 12690953.00645, 2892671.69562, _
        249947.71316, -5138.71425, 1142.68629, 6075.25751, _
        -34.76785, -19.72399, _
        -15.81516, 30.47718, _
        -11.73638, 21.87955, _
        9.42107, -10.40957, _
        -5.5967, 6.85778, _
        -167.06735, -2.31999, -32.42575, -13.72714, _
        130.16635, 117.97555, 31.33915, 39.64331, _
        -1378.54934, -395.83244, -562.79856, -167.74359, _
        -45.12476, -17.08986, _
        -4.20576, -16.56724, _
        1762.12089, -1148.86987, 736.5532, -423.09108, _
        56.13621, -26.26674, _
        9.7781, -38.05151, _
        4702224.98754, 27254904.94363, 5306232.25993, 39518429.29982, _
        1725110.05669, 21833263.27069, 46010.62605, 5425411.66252, _
        17238.09865, 536771.62156, -61263.36051, 66270.70142, _
        2084.66296, -1936.71208, 35898.49503, 34885.28549, _
        1.93276, 10.66292, _
        -665.11445, 3.70467, -265.68478, 16.16272, _
        -19.45954, 2.32738, _
        3.04237, 3.97339, _
        -2.64312, 0.66308, _
        -3207.68754, 3418.0372, -2342.6231, 1729.1503, _
        -450.84643, 179.00943, -13.20367, -1.86087, _
        -4.95659, 7.22347, _
        -5.0889, -1.28891, _
        -6.21713, 5.10551, _
        13.97276, 0.44529, _
        3.25177, 25.02775, _
        -45.56672, 11.5847, _
        124443355.5545, -100018293.41775, 190506421.77863, -118262753.40162, _
        108199328.45091, -45247957.63323, 27272084.41143, -4125106.01144, _
        2583469.66051, 1024678.12935, -22702.55109, 199269.51481, _
        -15783.14789, 5564.52481, -427.22231, -6330.86079, _
        -97.50757, -204.32241, _
        -9060.54822, 156661.77631, -47791.83678, 59725.58975, _
        -8807.74881, -92.38886, _
        -28886.11572, -244419.59744, -53336.36915, -92232.16479, _
        -8724.89354, -2446.76739, _
        889.71335, 936.51108, _
        494.80305, 2252.83602, _
        -18326.60823, -25443.13554, -3130.86382, -5426.29135, _
        23494.08846, 91.28882, 4664.14726, 1552.06143, _
        -8090.43357, 2843.48366, -1445.73506, 1023.11482, _
        11664.20863, -7020.08612, 3100.21504, -64.16577, _
        -9724.97938, -12261.47155, -3008.08276, -1523.06301, _
        6788.74046, 10708.27853, 343.09434, 1701.5276, _
        14743.99857, -4781.96586, _
        -15922236.41469, 1825172.51825, -14006084.36972, 10363332.64447, _
        -979550.9136, 6542446.18797, 1160614.26915, 570804.88172, _
        89912.68112, -171247.08757, -13899.52899, -6182.25841, _
        -240.64725, 412.42581, -66.2451, 71.30726, _
        -15.81125, -15.76899, _
        -21.85515, -102.12717, -10.18287, -19.38527, _
        1.43749, -3.87533, _
        1.97109, 0.20138, _
        0.32012, 1.02928, _
        -40.22077, 20.80684, -15.69766, 9.63663, _
        -1.2601, 0.56197, _
        0.08592, 0.1854, _
        -0.07303, 0.03897, _
        0.01438, -0.08809, _
        0.15479, 0.10354, _
        0.19052, 2.0879, _
        405480.24475, -607986.83623, 582811.58843, -915111.10396, _
        258696.21023, -493391.09443, 23403.62628, -119503.67282, _
        -4036.86957, -9766.17805, -663.93268, 2544.07799, _
        40.36638, 76.2639, 246.67716, -13.9344, _
        0.12403, 0.25378, _
        0.14004, -0.08501, _
        0.07904, 0.12731, _
        1.02117, -1.34663, 0.25142, -0.26903, _
        0.18135, -0.57683, -0.30092, -0.36121, _
        -0.09623, 0.05873, _
        -0.05803, 0.02869, _
        -0.01194, 0.04983, _
        0.0425, 0.04894, _
        1.34245, 0.70137, 0.24217, 0.25912, _
        -0.32759, -0.03575, _
        0.0678, -0.41277, _
        0.43865, 0.17857, _
        -763933.02226, 465658.17048, -1082753.91241, 593319.68634, _
        -553911.8934, 274748.95145, -122250.71547, 56608.95768, _
        -9914.173, 2988.43709, 707.94605, -765.0147, _
        52.7326, -34.22263, -43.583, -38.43647, _
        -4.95939, -1.97173, -1.04406, -0.13072, _
        -0.34281, 4.75202, -0.35513, 0.93597, _
        -0.5438, 0.70536, _
        84.83116, 102.93003, 26.34884, 48.57746, _
        0.02853, 2.91676, _
        -8.07116, 1.66613, _
        -2.07908, 11.62592, _
        6.64704, 0.98291, _
        -1.19192, 0.93791, _
        0.18822, 0.009, _
        -0.03181, -0.02, _
        0.02755, -0.01398, _
        -0.03971, -0.03756, _
        0.13454, -0.04193, _
        -18672.98484, 28230.75834, -28371.58823, 26448.45214, _
        -13352.09393, 7461.71279, -2609.33578, 726.50321, _
        -309.72942, -86.71982, 12.48589, -9.69726, _
        1.82185, 14.9222, _
        -0.04748, 0.4251, _
        -0.20047, 0.00154, _
        0.00176, -0.26262, _
        0.78218, -0.73243, 0.23694, -0.03132, _
        -0.0029, -0.03678, _
        14.03094, 4.25948, 0.79368, -0.78489, _
        -2.30962, 2.31946, _
        0.00158, -0.04125, _
        -0.01387, 0.28503, 0.00892, 0.05154, _
        0.00184, -0.01727, _
        -0.00889, 0.03526, _
        -0.00521, -0.02093, _
        0.002, 0.04872, _
        -0.02163, 0.00578, _
        20699.27413, -2175.57827, 31177.33085, 4572.02063, _
        15486.2819, 8747.74091, 2455.51737, 3839.83609, _
        51.31433, 507.91086, 15.90082, 44.75942, _
        -0.98374, -2.64477, 2.52336, -3.09203, _
        -0.08897, -0.00083, _
        -15.91892, 0.72597, 14.04523, -3.16525, _
        4.33379, -30.8298, 0.40462, -0.75845, _
        13.14831, -0.02721, _
        -0.01779, 0.00481, _
        0.42365, -0.09048, 0.08653, 0.04391, _
        0.00846, 0.01082, _
        -0.04736, 0.02308, _
        6282.21778, -4952.70286, 7886.57505, -5328.36122, _
        3113.76826, -1696.8459, 330.70011, -155.51989, _
        -18.31559, -3.90798, -3.11242, 1.87818, _
        -1.05578, 0.11198, _
        0.05077, -0.01571, _
        2.41291, 2.40568, _
        -0.01136, -0.00076, _
        -0.00392, -0.02774, _
        634.85065, -352.21937, 674.31665, -260.73473, _
        199.16422, -28.44198, 6.54187, 6.4496, _
        -1.55155, 0.29755, 0.16977, 0.1754, _
        -0.02652, 0.03726, _
        -0.00623, 0.11777, -0.00933, 0.02602, _
        -0.13943, -0.24818, _
        0.02876, -0.01463, _
        -0.07166, 0.06747, -0.01578, 0.01628, _
        0.00233, -0.00686, _
        0.00431, -0.00276, _
        0.21774, 0.09735, 0.07894, 0.07279, _
        -0.013, -0.00268, _
        0.10824, 0.09435, 0.0072, 0.02111, _
        -0.0196, 0.06154, _
        0.56867, -0.07544, 0.1821, 0.06343, _
        -0.00906, 0.01942, _
        -0.0085, -0.00351, _
        -0.06988, 0.01713, -0.0111, -0.00663, _
        0.00196, -0.02064, _
        -0.00008, 0.00043, _
        0.00375, 0.00084, _
        -0.00279, 0.001, _
        0.00271, -0.02017, -0.00074, -0.00357, _
        0.03793, -0.10108, -0.01083, -0.03952, _
        0.0003, 0.00012, _
        0.01576, 0.01142, 0.00351, 0.00277, _
        0.01409, -0.00774, _
        -0.00065, 0.01895, _
        0.0735, -0.02519, 0.01528, -0.01057, _
        -0.00099, -0.00295, _
        0.21347, -0.17458, 0.0494, -0.02757, _
        -0.06243, 0.05203, _
        0.01055, -0.00109, _
        0.00003, -0.04201, _
        -0.00263, 0.02387, _
        0.00886, -0.01168, _
        0.00479, 0.00204, _
        -0.00239, 0.00022, _
        -0.00223, -0.02029, _
        -0.1413, -0.15237, -0.01827, -0.04877, _
        0.12104, 0.06796, _
        0.16379, 0.31892, _
        -0.15605, 0.07048, _
        -0.007, 0.07481, _
        -0.0037, -0.00142, _
        -0.00446, 0.00329, _
        -0.00018, 0.00117, _
        -0.0091, 0.0051, _
        -0.00055, -0.00114, _
        0.04131, -0.04013, _
        -0.13238, 0.0268, _
        -0.10369, 1.38709, 0.35515, 0.41437, _
        -0.01327, -0.02692, _
        38.02603, 13.38166, 15.33389, -7.40145, _
        -8.55293, -0.13185, -0.03316, 0.13016, _
        0.04428, -1.60953, _
        -12.87829, -76.97922, -23.96039, -22.45636, _
        14.83309, 14.09854, 0.24252, 0.1385, _
        -4.16582, 4.08846, _
        0.00751, -0.00051, _
        0.03456, 0.029, _
        0.01625, -0.0466, _
        0.0139, -0.0053, _
        0.01665, -0.04571, _
        40.90768, -14.11641, 7.46071, -58.07356, _
        -0.27859, -1.33816, 23.76074, -0.03124, _
        -0.2786, 0.13654, -0.048, 0.05375, _
        4.38091, 4.39337, _
        0.02233, 0.00514, _
        -0.25616, -0.54439, _
        -0.05155, 0.11553, _
        0.02944, -0.00818, _
        0.0057, 0.00119, _
        -0.00733, -0.027, _
        -0.23759, -0.08712, -0.12433, 0.07397, _
        0.20629, 0.60251, 0.56512, 0.1479, _
        0.07778, 0.11614}

        Friend args() As Integer = { _
        0, 7, _
        2, 3, 7, -9, 9, 0, _
        2, 4, 7, -12, 9, 0, _
        2, 4, 7, -8, 8, 0, _
        3, -4, 7, 5, 8, 4, 9, 0, _
        3, 3, 7, -5, 8, -1, 9, 0, _
        2, 1, 6, -8, 9, 1, _
        2, 3, 8, -5, 9, 1, _
        2, 1, 6, -9, 9, 2, _
        3, 6, 7, -6, 8, -8, 9, 0, _
        3, 4, 7, -10, 8, 4, 9, 2, _
        2, 3, 7, -8, 9, 0, _
        1, 1, 9, 7, _
        2, 3, 7, -10, 9, 0, _
        3, 4, 7, -10, 8, 2, 9, 2, _
        3, 5, 7, -12, 8, 2, 9, 0, _
        2, 1, 6, -7, 9, 0, _
        1, 1, 8, 3, _
        2, 1, 6, -10, 9, 0, _
        3, 6, 7, -12, 8, 2, 9, 0, _
        3, 5, 7, -10, 8, 2, 9, 0, _
        2, 5, 7, -13, 9, 0, _
        2, 4, 7, -10, 9, 0, _
        2, 3, 7, -7, 9, 0, _
        1, 2, 9, 7, _
        2, 3, 7, -11, 9, 0, _
        3, 4, 7, -9, 8, 4, 9, 2, _
        3, 3, 7, -5, 8, 1, 9, 2, _
        2, 1, 6, -6, 9, 0, _
        2, 7, 8, -13, 9, 0, _
        2, 3, 8, -2, 9, 1, _
        3, 1, 7, -5, 8, 2, 9, 1, _
        3, 6, 7, -12, 8, 3, 9, 1, _
        2, 5, 7, -12, 9, 1, _
        2, 4, 7, -9, 9, 1, _
        2, 2, 7, -3, 9, 1, _
        1, 1, 7, 0, _
        1, 3, 9, 5, _
        2, 3, 7, -12, 9, 1, _
        3, 5, 7, -9, 8, 2, 9, 0, _
        3, 4, 7, -7, 8, 2, 9, 1, _
        3, 3, 7, -5, 8, 2, 9, 0, _
        3, 2, 7, -5, 8, 5, 9, 0, _
        2, 1, 6, -5, 9, 0, _
        2, 3, 8, -1, 9, 2, _
        2, 1, 6, -12, 9, 0, _
        3, 2, 7, -7, 8, 1, 9, 0, _
        2, 5, 7, -11, 9, 0, _
        2, 4, 7, -8, 9, 0, _
        2, 2, 7, -2, 9, 0, _
        1, 4, 9, 7, _
        3, 2, 7, -8, 8, 2, 9, 0, _
        3, 5, 7, -9, 8, 3, 9, 0, _
        3, 4, 7, -9, 8, 6, 9, 0, _
        3, 3, 7, -5, 8, 3, 9, 1, _
        2, 2, 7, -1, 8, 1, _
        2, 3, 8, -9, 9, 0, _
        2, 9, 8, -9, 9, 0, _
        2, 1, 6, -13, 9, 0, _
        3, 2, 7, -5, 8, -3, 9, 0, _
        2, 6, 7, -13, 9, 1, _
        2, 5, 7, -10, 9, 0, _
        2, 4, 7, -7, 9, 0, _
        2, 3, 7, -4, 9, 0, _
        1, 5, 9, 7, _
        3, 6, 7, -9, 8, 1, 9, 1, _
        3, 4, 7, -5, 8, 1, 9, 1, _
        3, 3, 7, -3, 8, 1, 9, 0, _
        2, 1, 6, -3, 9, 2, _
        2, 3, 8, -10, 9, 0, _
        2, 1, 8, 4, 9, 0, _
        2, 5, 8, -2, 9, 0, _
        2, 11, 8, -11, 9, 0, _
        3, 1, 7, -9, 8, 5, 9, 0, _
        2, 6, 7, -12, 9, 0, _
        2, 5, 7, -9, 9, 0, _
        2, 4, 7, -6, 9, 0, _
        2, 3, 7, -3, 9, 0, _
        1, 6, 9, 6, _
        2, 2, 7, -12, 9, 0, _
        3, 6, 7, -9, 8, 2, 9, 0, _
        3, 3, 7, -12, 8, 3, 9, 0, _
        3, 4, 7, -10, 8, -3, 9, 1, _
        3, 3, 7, -3, 8, 2, 9, 0, _
        2, 1, 6, -2, 9, 2, _
        2, 1, 8, 5, 9, 0, _
        2, 13, 8, -13, 9, 1, _
        3, 2, 7, -9, 8, 1, 9, 0, _
        2, 6, 7, -11, 9, 0, _
        2, 5, 7, -8, 9, 0, _
        2, 4, 7, -5, 9, 0, _
        2, 3, 7, -2, 9, 0, _
        1, 7, 9, 7, _
        3, 6, 7, -9, 8, 3, 9, 0, _
        2, 1, 6, -1, 9, 4, _
        2, 3, 8, 3, 9, 0, _
        2, 7, 7, -13, 9, 1, _
        2, 3, 7, -1, 9, 0, _
        2, 2, 7, 2, 9, 0, _
        1, 8, 9, 6, _
        3, 7, 7, -9, 8, 1, 9, 0, _
        1, 1, 6, 0, _
        1, 3, 7, 0, _
        2, 2, 7, 3, 9, 0, _
        1, 9, 9, 5, _
        3, 1, 7, -10, 8, 3, 9, 0, _
        3, 2, 7, -12, 8, 3, 9, 1, _
        2, 1, 6, 1, 9, 0, _
        3, 1, 7, -1, 8, 8, 9, 0, _
        2, 3, 7, 1, 9, 1, _
        2, 2, 7, 4, 9, 0, _
        2, 1, 7, 7, 9, 0, _
        2, 4, 8, 4, 9, 1, _
        2, 12, 8, -8, 9, 0, _
        3, 1, 7, -10, 8, 2, 9, 1, _
        2, 1, 6, 2, 9, 0, _
        1, 11, 9, 2, _
        2, 12, 8, -7, 9, 0, _
        3, 1, 7, -10, 8, 1, 9, 1, _
        1, 4, 7, 0, _
        1, 12, 9, 0, _
        2, 6, 8, 3, 9, 0, _
        3, 1, 7, -2, 8, -12, 9, 0, _
        3, 7, 7, -7, 8, 2, 9, 1, _
        2, 2, 6, -4, 9, 1, _
        1, 13, 9, 0, _
        2, 10, 8, -2, 9, 1, _
        2, 4, 7, 2, 9, 0, _
        2, 2, 6, -3, 9, 0, _
        2, 2, 7, 8, 9, 1, _
        2, 8, 8, 2, 9, 0, _
        1, 5, 7, 1, _
        2, 4, 7, 3, 9, 0, _
        2, 3, 7, 6, 9, 0, _
        2, 1, 5, -6, 9, 0, _
        3, 2, 7, 8, 8, -3, 9, 0, _
        3, 1, 7, 6, 8, 3, 9, 0, _
        2, 6, 8, 6, 9, 0, _
        3, 8, 7, -7, 8, 2, 9, 0, _
        2, 9, 7, -11, 9, 0, _
        2, 5, 7, 1, 9, 1, _
        2, 4, 7, 4, 9, 0, _
        2, 2, 6, -1, 9, 0, _
        3, 2, 6, -1, 7, 2, 9, 0, _
        2, 2, 7, 10, 9, 0, _
        2, 1, 7, 13, 9, 0, _
        2, 8, 7, -7, 9, 0, _
        2, 7, 7, -4, 9, 0, _
        2, 6, 7, -1, 9, 0, _
        2, 5, 7, 3, 9, 0, _
        2, 4, 7, 5, 9, 0, _
        1, 2, 6, 0, _
        2, 1, 5, -4, 9, 1, _
        3, 1, 6, 9, 8, -5, 9, 0, _
        2, 1, 5, -3, 9, 4, _
        2, 1, 5, -2, 9, 4, _
        3, 9, 7, -9, 8, 6, 9, 0, _
        2, 8, 7, -4, 9, 0, _
        2, 7, 7, -1, 9, 0, _
        2, 1, 6, 3, 9, 0, _
        2, 2, 6, 3, 9, 0, _
        2, 1, 5, -1, 9, 3, _
        3, 6, 7, -3, 8, 7, 9, 1, _
        1, 1, 5, 0, _
        2, 2, 6, 5, 9, 0, _
        2, 1, 5, 1, 9, 0, _
        2, 1, 5, 2, 9, 0, _
        2, 1, 5, 3, 9, 0, _
        2, 2, 5, -4, 9, 0, _
        2, 2, 5, -3, 9, 0, _
        2, 2, 5, -2, 9, 1, _
        2, 2, 5, -1, 9, 1, _
        1, 2, 5, 0, _
        -1}

        '/* Total terms = 173, small = 156 */
        Friend plu404 As New plantbl( _
            9, _
            New Integer(NARGS) {0, 0, 0, 0, 2, 2, 9, 13, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, _
            7, _
            args, _
            tabl, _
            tabb, _
            tabr, _
            39.54, _
            3652500.0, _
            1.0)
    End Module
End Namespace
