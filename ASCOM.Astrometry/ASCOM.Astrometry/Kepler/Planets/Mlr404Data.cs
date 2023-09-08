﻿
namespace ASCOM.Astrometry
{
    static class Mlr404Data
    {
        // /* geocentric moon
        // polar coordinates re mean equinox and ecliptic of date
        // For latitude coefficients, see mlat404.c.
        // S. L. Moshier
        // December, 1996
        // Residuals against JPL ephemeris DE404 (arc seconds)
        // First date in file = 1221000.5
        // Number of samples = 1053099
        // Sampling interval = 1.515625 days
        // Peak excursions from these mostly different test points
        // were consolidated with the above.  They added .01" to a few
        // of the peak readings.
        // First date in file = 1221000.50
        // Number of samples = 524290.0
        // Sampling interval = 3.0 days
        // Julian Years             Longitude          Latitude           Distance
        // 1 = 1.9 km
        // Peak  RMS   Ave    Peak  RMS   Ave    Peak  RMS   Ave
        // -1369.0 to -1000.0:   0.43  0.07  0.00   0.33  0.05 -0.00   0.18  0.03  0.00
        // -1000.0 to  -500.0:   0.49  0.06 -0.00   0.33  0.04 -0.00   0.18  0.03  0.00
        // -500.0 to     0.0:   0.48  0.06  0.00   0.32  0.04  0.00   0.15  0.03  0.00
        // 0.0 to   500.0:   0.45  0.05  0.00   0.30  0.04 -0.00   0.17  0.03 -0.00
        // 500.0 to  1000.0:   0.48  0.05 -0.00   0.29  0.04  0.00   0.17  0.03 -0.00
        // 1000.0 to  1500.0:   0.42  0.05 -0.00   0.28  0.04 -0.00   0.16  0.03  0.00
        // 1500.0 to  2000.0:   0.35  0.05 -0.00   0.26  0.04  0.00   0.15  0.03  0.00
        // 2000.0 to  2500.0:   0.39  0.06  0.00   0.25  0.04 -0.00   0.15  0.03 -0.00
        // 2500.0 to  3000.0:   0.44  0.07 -0.00   0.30  0.05 -0.00   0.19  0.03 -0.00
        // 3000.0 to  3000.8:   0.23  0.08 -0.04   0.11  0.04 -0.00   0.08  0.03 -0.00
        // */

        internal static double[] tabl = new double[] { 175667d, 66453d, 5249d, -42, 20057d, 403d, -2360, 6148d, -7644, 24646d, -1273, 9127d, -1395, 1958d, 232d, -289, -97, 553d, 69d, 130d, -80, 6d, 129d, -868, 26d, -89, 1042d, 1172d, 194d, -112, -47433, -241666, 224626d, -103752, 63419d, 127606d, 2294d, -691, -1827, -1254, -1, -119, 1057d, 324d, 505d, -195, 254d, -641, -36, 1008d, -1082, -3, -87, 122d, 161d, 11d, 2d, -106, 29d, -123, -32, 41d, -524, -35, 133d, -595, 225d, 837d, -108, -191, -2294, 841d, -340, -394, -351, -1039, 238d, -108, -66, 21d, 1405d, 869d, 520d, 2776d, -174, 71d, 425d, 652d, -1260, -80, 249d, 77d, -192, -17, -97, 134d, -7, -54, -802, -7436, -2824, 70869d, -35, 2481d, 1865d, 1749d, -2166, 2415d, 33d, -183, -835, 283d, 27d, -45, 56d, 235d, 2d, 718d, -1206, 275d, -87, -158, -7, -2534, 0d, 10774d, 1d, -324, -208, 821d, 281d, 1340d, -797, 440d, 224d, 72d, -65, -5, -7, -44, -48, 66d, -151, -40, -41, -45, 76d, -108, -18, 1202d, 0d, -2501, 1438d, -595, 900d, 3040d, -3435, -5, -100, -26, 0d, -13714, -183, 68d, 453d, -83, -228, 325d, 97d, 13d, 2d, 105d, -61, 257d, 0d, 57d, 88d, -11, -1, -8220, 0d, 275d, -43, -10, -199, 105d, 1d, -5849, 2d, 24887d, -128, 48d, 712d, 970d, -1407, 845d, -266, 378d, 311d, 1526d, -1751, 27d, 0d, -185858, 133d, 6383d, -108, 25d, -7, 1944d, 5d, 390d, -11, 31d, 277d, -384, 158d, 72d, -81, -41, -13, -111, -2332, -65804, -698, 505812d, 34d, 1676716d, -72, -6664384, 154d, -57, 52d, 95d, -4, -5, -7, 37d, -63, -32, 4d, 3349d, 1d, -14370, 16d, -83, 0d, -401, 13d, 3013d, 48d, -20, 0d, 250d, 51d, -79, -7, -146, 148d, 9d, 0d, -64, -17, -59, -67, -492, -2, 2116601d, -12, -1848, 8d, -436, -6, 324d, 0d, -1363, -163, 9d, 0d, -74, 63d, 8167d, -29, 37587d, -22, -74501, -71, 497d, -1, 551747d, -87, -22, 0d, -51, -1, -463, 0d, -444, 3d, 89d, 15d, -84, -36, -6829, -5, -21663, 0d, 86058d, 0d, -298, -2, 751d, -2, -1015, 0d, 69d, 1d, -4989, 0d, 21458d, 0d, -330, 0d, -7, 0d, -226, 0d, -1407, 0d, 2942d, 0d, 66d, 0d, 667d, 0d, -155, 0d, 105d, 0d, -107, 0d, -74, 0d, -52, 0d, 91d, 0d, 59d, 0d, 235d, -1, -1819, 0d, 2470d, 71d, 13d, 0d, 1026d, 14d, -54, 0d, -174, -121, -19, 0d, -200, 0d, 3008d, -16, -8043, -10, -37136, -3, 73724d, -157, -5, 0d, -854, 8d, 147d, -13, -893, 0d, 11869d, -23, -172, 89d, 14d, -1, 872d, 0d, -3744, 11d, 1606d, 0d, -559, -1, -2530, 0d, 454d, 0d, -193, -60, -10, -82, -13, -75, 6d, 36d, 81d, 354d, -162836, 148d, -516569, 4d, 2054441d, 4d, -94, 39d, 38d, 61d, -30, 2d, 121d, -11, 590d, 62d, 2108d, 0d, -12242, -476, -42, -84, 113d, -394, 236d, 0d, 276d, -49, 31d, 0d, 86d, 1d, -1313, 1d, 69d, -60, 88d, -46, 18d, 0d, -63818, 14d, -93, 113d, 547d, -618, 17d, -7, 12290d, -1, -25679, 0d, 92d, -115, 50d, -48, 233d, 4d, 1311d, 1d, -5567, 3d, 1251d, 29d, 548d, -244, 257d, -2, 1825d, 42d, 637d, -46, 68d, -62, 8d, 3d, 110d, 445d, -100, -316, -202, 2925d, -621, 763d, 1495d, -169, -184, 20d, -76, -475, -138, 8d, -141, -197, 1351d, -1284, 422d, -129, 1879d, -102, 8382d, -9, 45864958d, -215, 1350d, -1285, 422d, -481, -136, 8d, -140, 40d, -53, 2622d, -543, 700d, 1406d, 402d, -95, -318, -194, 122d, 13d, -30, 147d, -121, -902, 61d, -23, -63, 7d, 69d, 479d, -224, 228d, -7, 500d, 0d, -429, -42, 193d, -92, 37d, 67d, 5d, -350, -31, 0d, 67d, -55, -5, 0d, 47d, -36, 53d, 5d, 561d, 0d, -126, 0d, 871d, -52, 4d, -201, 116922d, -22, 371352d, -12, -1473285, 0d, 87d, -164, 84d, -3, 422d, 30d, 1434d, -26, 38d, 2d, -1249943, -404, -34, -57, 79d, 5d, 509d, 1d, 131d, -344, 168d, 112d, 22540d, 30d, 71218d, 18d, -283983, 0d, -851, 0d, -1538, 0d, 1360d, -12, 51d, -48, 68d, 88d, -20, 1d, 63d, 0d, -568, 303d, 25d, 0d, -122, 87d, 586d, -606, -14, 0d, -100, -85, 8d, -165, 54d, -45, 140d, 0d, -54, 4d, -831, 1d, 3495d, 31d, 116d, -46, -11, -371, 190d, -507, 399d, -2, 57d, -60, 36d, -198, -1174, -613, 4988d, -87, -4, 141d, 560d, -276, 187d, 1876d, 1379d, 778d, 4386d, 24d, -15, 167d, -774, -71, -9, -62, 90d, 98d, 580d, -663, -7, 34d, -112, 57d, 15d, -355, -214, -3240, -13605, 12229d, -5723, 3496d, 7063d, 33d, -51, 1908d, 1160d, -226, 715d, 964d, 1170d, -1264, 623d, 14071d, 5280d, 5614d, 3026d, 488d, 1576d, -2, 226395859d, 824d, 1106d, -1287, 617d, 1917d, 1156d, -214, 718d, 90d, -97, 12078d, -2366, 3282d, 6668d, -219, 9179d, 593d, 2015d, -282, -186, 57d, 25d, 31d, -102, -77, -4, -268, -341, -7, -45, -3, 74d, 15d, -615, -88, -7, 234d, -353, 1d, -119, -163, -1159, -601, 4969d, 22d, -58, -17, -11434, 17d, 54d, 348d, 348d, -460, 434d, -371, 175d, -11, -204, 4d, -6440, -5, -53, -4, -14388, -37, -45231, -7, 179562d, -44, 136d, -160, 49d, -101, 81d, -1, -188, 0d, 2d, -4, 12124d, -11, -25217, 71d, 543d, -557, -14, -75, 526d, 0d, 395274d, -233, -16, 93d, -20, -43, 61d, 0d, -1275, 0d, -824, 1d, -415, 0d, 1762d, -261, 131d, -45, 64d, -297, -25, 0d, -17533, -6, -56, 21d, 1100d, 1d, 327d, 1d, 66d, 23d, -217, -83, -7, 83d, 86847d, 49d, 275754d, -4, -1093857, -46, 2d, 0d, -24, 0d, -419, 0d, -5833, 1d, 506d, 0d, -827, -1, -377, -11, -78, 0d, 131945d, -2, -334, 1d, -75, 0d, -72, 0d, -213, -6, 5564d, -2, -11618, 0d, 1790d, 0d, -131, 0d, 6d, 0d, -76, 0d, -130, 0d, -1115, 0d, 4783d, 0d, -195, 0d, -627, 0d, -55, 0d, -83, 0d, 163d, 0d, -54, 0d, 82d, 0d, 149d, 0d, -754, 0d, 1578d, 0d, 138d, 0d, 68d, 2d, -2506, 0d, 3399d, 0d, -125, 86d, 16d, 0d, -6350, 0d, 27316d, 18d, -63, 0d, -169, -1, 46d, -136, -21, 0d, -239, -30, -8788, -15, -40549, -4, 80514d, -46, -8, -168, -6, -1, 536d, 0d, -2314, 9d, 148d, -13, -842, -1, 307713d, -23, -175, 95d, 15d, 0d, -297, 11d, 1341d, 0d, -106, 0d, 5d, -4, 68d, -114, 10d, 32d, 75d, 159d, -130487, 98d, -413967, 2d, 1647339d, -4, -85, 100d, -46, 2d, 95d, -11, 461d, 51d, 1647d, 0d, -32090, -375, -33, -65, 86d, -300, 180d, 0d, 836d, 0d, -3576, 0d, -222, 0d, -993, -41, 60d, 0d, -4537, -431, -34, 2d, 927d, 0d, -1931, -79, 33d, -31, 144d, -1, 284d, 0d, -1207, 0d, 88d, -11, 315d, -178, 177d, -1, 144d, -58, 986d, 11d, 86d, -228, -110, 2636d, -494, 718d, 1474d, 28d, -35, -24, 782d, -797, 277d, 2142d, -1231, 856d, 1853d, 74d, 10797d, 0d, 23699298d, -21, 786d, -796, 277d, 27d, -34, 2615d, -494, 712d, 1461d, -226, -109, -11, 663d, 0d, -123, -169, 157d, -54, 266d, 0d, -76, 1d, -634, 0d, 2738d, -25, 106d, -63, 24d, 0d, -372, -221, -24, 0d, -5356, 0d, -219, 0d, 91d, -28, 7684d, -6, 24391d, -1, -96795, -77, 43d, 2d, 95d, -47, -3, 0d, -84530, 2d, 310d, 1d, 88d, 111d, 19331d, 32d, 61306d, 4d, -243595, 0d, 770d, 0d, -103, 0d, 160d, 0d, 356d, 0d, 236d, -41, 354d, 39d, 303d, 12d, -56, 873d, -143, 238d, 482d, -28, 35d, -93, 31d, -3, 7690211d, -91, 33d, -34, 43d, 824d, -130, 226d, 450d, -39, 341d, -1, -687, 0d, -303, 11d, -2935, 1d, 12618d, 121d, 924d, 9d, -1836, -268, -1144, -678, 3685d, -69, -261, 0d, -4115951, -69, -261, 5d, -151, 0d, -88, 0d, 91d, 0d, 187d, 0d, -1281, 1d, 77d, 1d, 6059d, 3d, 19238d, 0d, -76305, 0d, -90, 0d, -238, 0d, -962, 0d, 4133d, 0d, 96d, 0d, 9483d, 0d, 85d, 0d, -688, 0d, -5607, 0d, 55d, 0d, -752, 0d, 71d, 0d, 303d, 0d, -288, 0d, 57d, 0d, 45d, 0d, 189d, 0d, 401d, 0d, -1474, 0d, 3087d, 0d, -71, 0d, 2925d, 0d, -75, 0d, 359d, 0d, 55d, 1d, -10155, 0d, 43735d, 0d, -572, 0d, -49, 0d, -660, 0d, -3591, 0d, 7516d, 0d, 668d, -1, -53, -2, 384259d, 0d, -163, 0d, -93, 1d, 112d, -95, -11528, -22, -36505, -1, 145308d, 5d, 145d, 0d, 4047d, 1d, 1483d, 0d, -6352, 0d, 991d, 0d, -4262, 0d, -93, 0d, -334, 0d, -160, 0d, -153, -10, 127d, 51d, 185d, -77, 18d, 56d, 1217d, 6d, 1919574d, -74, 17d, 50d, 180d, -5, 93d, 0d, -104, 0d, -58, -3, -353, -1, 1499d, 0d, -229, -15, 86d, 0d, -93657, 0d, 1561d, 0d, -6693, 0d, -5839, 1d, 6791d, 0d, -29143, 1d, -701, 0d, 3015d, 0d, 2543d, 0d, 693d, -1, 361233d, 0d, -50, 0d, 946d, -1, -140, -70, 407d, 0d, -450995, 0d, -368, 0d, 54d, 0d, -802, 0d, -96, 0d, 1274d, 0d, -5459, 0d, -614, 0d, 2633d, 0d, 685d, 0d, -915, 0d, -85, 0d, 88d, 0d, 106d, 0d, 928d, 0d, -726, 0d, 1523d, 0d, 5715d, 0d, -4338, 0d, 18706d, 0d, -135, 0d, -132, 0d, -158, 0d, -98, 0d, 680d, -1, 138968d, 0d, -192, 0d, -1698, 0d, -2734, 0d, 11769d, 0d, 4d, 0d, 673d, 0d, -2891, 0d, 889d, 0d, -3821, 0d, 121d, -1, 143783d, 0d, 231d, -9, 51d, 0d, -57413, 0d, -483, 0d, -407, 0d, 676d, 0d, -2902, 0d, 531d, 0d, 445d, 0d, 672d, 0d, 19336d, 0d, 70d, 0d, -39976, 0d, -68, 0d, 4203d, 0d, -406, 0d, 446d, 0d, -108, 0d, 79d, 0d, 84d, 0d, 734d, 0d, 255d, 0d, 3944d, 0d, -655, 0d, 2825d, 0d, -109, 0d, -234, 0d, 57d, 0d, 19773d, 0d, -2013, 0d, 958d, 0d, -521, 0d, -757, 0d, 10594d, 0d, -9901, 0d, 199d, 0d, -275, 0d, 64d, 0d, 54d, 0d, 165d, 0d, 1110d, 0d, -3286, 0d, 909d, 0d, 54d, 0d, 87d, 0d, 258d, 0d, 1261d, 0d, -51, 0d, 336d, 0d, -114, 0d, 2185d, 0d, -850, 0d, 75d, 0d, -69, 0d, -103, 0d, 776d, 0d, -1238, 0d, 137d, 0d, 67d, 0d, -260, 0d, 130d, 0d, 49d, 0d, 228d, 0d, 215d, 0d, -178, 0d, 57d, 0d, -133 };















































































































































































































































































































































































































































































































































































































































































        internal static double[] tabb = new double[] { -1 };

        internal static double[] tabr = new double[] { -5422, -2120, 1077d, 772d, 39d, 75d, 3d, 10d, -468, -326, -113, -78, -4, -2, 1d, 3d, 29d, 24d, 4d, 2d, 1d, 0d, -9, 7d, -2, 0d, -32, -13, -3, -3, 233d, 126d, 89d, 77d, -33, 16d, 3d, -3, 0d, -1, 2d, 0d, 0d, 1d, 4d, 9d, 1d, 1d, 16d, -1, 0d, 18d, 3d, 2d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0d, -1, -22, -5, 10d, 3d, 1d, 1d, -15, 7d, -2, 1d, -8, -11, -1, -2, -1, 1d, 46d, -58, 126d, -23, 4d, 8d, 35d, 8d, 10d, -17, 0d, 0d, 0d, 0d, -10, -7, 0d, 0d, -23, 3d, 151d, 10d, -327, 0d, 4d, -5, 6d, 5d, 1d, 0d, -1, -3, 0d, 0d, 0d, 1d, -185, 0d, -3, -24, -5, -2, -1062, 3d, 4560d, 0d, -3, 0d, 4d, 1d, 8d, -1, 2d, 4d, 0d, 1d, 0d, -1, 0d, 0d, -1, 0d, 0d, 1d, 0d, 0d, -1, -1, 277d, 3d, -583, 1d, -1, 4d, -32, 7d, 0d, -34, 1d, -1, -23685, 0d, -1, -2, -1, -7, -5, -4, 0d, 2d, -2, 0d, -5, -1, 35d, 0d, 0d, 2d, 202d, 0d, 180d, 0d, 0d, -1, -3, -6, -193, 0d, 770d, -1, -2, -4, -32, 23d, -28, -46, -13, -9, -54, 10d, -1, -61, -44895, 0d, -230, 5d, -1, -4, -71, 0d, -15, 0d, 1d, 0d, 15d, 11d, -3, 6d, 2d, -3, 4d, -1, 2576d, -138, -19881, -47, -65906, -1, 261925d, -4, -2, -7, 4d, -2, 0d, 0d, -1, 0d, 1d, -3, 172d, -2, -727, 0d, 4d, 1d, 324d, 0d, -139, 1d, 1d, 3d, -276, 0d, 5d, 3d, 9d, 0d, -1, 10d, -37, 0d, 5d, -1, 76d, -10, 1318810d, 1d, 12d, -1, -38, 1d, -141, 0d, 611d, 0d, 0d, -11, 4d, 0d, -627, 2d, -2882, -3, 5711d, -2, -48, -7, 55294d, 0d, 2d, -7, 31d, 0d, 34d, 0d, -259, 0d, -55, 2d, 6d, 3d, -4273, 20d, -13554, 3d, 53878d, 0d, -46, 0d, -85, 0d, 114d, 0d, -45, 0d, -818, 0d, 3520d, 0d, 34d, 0d, -157, 0d, 29d, 0d, -878, 0d, 1838d, 0d, -428, 0d, 161d, 0d, 24d, 0d, 65d, 0d, 19d, 0d, 15d, 0d, 12d, 0d, -26, 0d, -14, 0d, -149, 0d, 584d, 0d, -793, 0d, 4d, -23, -238, 0d, -18, -5, 45d, 0d, -7, 42d, 79d, 0d, -1723, 0d, 2895d, -6, 13362d, -4, -26525, -1, -2, 57d, 291d, 0d, 52d, -3, -327, 5d, -2755, 0d, -63, 9d, 5d, -33, -261, -1, 1122d, 0d, 621d, -4, -227, 0d, 1077d, 0d, -167, 0d, 85d, 0d, -4, 23d, -5, 32d, 3d, 30d, -32, 14d, 64607d, 141d, 204958d, 59d, -815115, 2d, -37, -1, 15d, -15, 12d, 24d, 48d, -1, 235d, 4d, 843d, -25, 4621d, 0d, -17, 191d, 45d, 34d, 95d, 159d, -132, 0d, 13d, 20d, 32d, 0d, -540, 0d, 29d, 0d, 37d, 25d, 8d, 19d, 22127d, 0d, -35, -5, 232d, -48, 7d, 262d, 5428d, 3d, -11342, 1d, -45, 0d, -21, -49, -100, -21, -626, 1d, 2665d, 0d, 532d, -2, 235d, -12, -111, -105, 774d, 1d, -283, 17d, 29d, 20d, 3d, 27d, 47d, -2, -43, -192, -87, 136d, -269, -1264, 646d, -330, -79, 73d, -33, -9, 60d, -205, 61d, 4d, -584, -85, -182, -555, -780, -57, -3488, -45, -19818328, -4, 583d, 93d, 182d, 555d, -59, 208d, -60, -4, 23d, 17d, 235d, 1133d, -608, 302d, 41d, 174d, 84d, -137, 6d, -53, 63d, 13d, -392, 52d, -10, -27, -3, -27, 199d, -31, 99d, 97d, -218, -3, 209d, 0d, 84d, 18d, 16d, 40d, 2d, -30, 14d, -154, 30d, 0d, -2, 24d, -108, 0d, -24, -16, 262d, -2, 55d, 0d, -304, 0d, 2d, 25d, 55112d, 95d, 175036d, 11d, -694477, 5d, 41d, 0d, -38, -76, 199d, 1d, 679d, -14, -17, -12, 582619d, 1d, -16, 191d, 38d, 27d, -234, 2d, -60, 0d, 80d, 163d, -10296, 48d, -32526, 13d, 129703d, 8d, -1366, 0d, -741, 0d, -646, 0d, 25d, 6d, 33d, 23d, 10d, 43d, -31, 0d, -6, 0d, -12, 147d, 59d, 0d, 287d, -42, -7, 297d, -59, 0d, -4, -42, -27, -81, -69, -22, 27d, 0d, -423, -2, 1779d, -1, -57, 15d, 5d, -23, 94d, 182d, -197, -250, 24d, 1d, -18, -30, 581d, -98, -2473, -303, -2, 43d, -277, 70d, -92, -136, -681, 925d, -2165, 384d, -8, -12, 382d, 82d, -4, 35d, -45, -31, -286, 48d, 3d, -328, -55, -17, 8d, -28, -106, 175d, -6735, 1601d, -2832, -6052, 3495d, -1730, -25, -17, -574, 944d, -354, -112, -579, 476d, -308, -625, -2411, 7074d, -1529, 2828d, -1335, 247d, -112000844, -1, 545d, -409, 305d, 637d, 572d, -950, 356d, 106d, 48d, 44d, 1170d, 5974d, -3298, 1624d, -4538, -106, -996, 294d, 92d, -139, -12, 28d, 50d, 16d, 2d, -38, 169d, -133, 22d, -3, 38d, 1d, 305d, 7d, 4d, -44, 175d, 116d, 59d, 1d, -573, 81d, 2453d, 297d, 29d, 11d, 5674d, -8, -27, 9d, 173d, -173, 215d, 228d, -87, -184, 102d, -5, 3206d, 2d, -53, 2d, 7159d, -7, 22505d, -19, -89344, -3, 67d, 22d, 24d, 79d, -40, -50, 94d, 0d, 186d, 0d, -6063, 0d, 12612d, -5, -271, 35d, 7d, -278, -479, -74, 426754d, 0d, 8d, -116, -10, -47, -31, -22, 645d, 0d, 426d, 0d, -213, 0d, 903d, 0d, -67, -133, -33, -23, 13d, -152, -9316, 0d, 29d, -3, -564, 11d, -167, 0d, -34, 0d, 114d, 12d, 4d, -44, -44561, 42d, -141493, 25d, 561256d, -2, -1, -24, -261, 0d, 211d, 0d, -4263, 0d, -262, 1d, 1842d, 0d, 202d, 0d, 41d, -6, 77165d, 0d, 176d, -1, 39d, 1d, -24, 0d, 118d, 0d, -2991, -4, 6245d, -1, 46886d, 0d, -75, 0d, -100, 0d, 40d, 0d, 75d, 0d, -618, 0d, 2652d, 0d, 112d, 0d, 1780d, 0d, 30d, 0d, 49d, 0d, 86d, 0d, 33d, 0d, -30, 0d, -95, 0d, 277d, 0d, -580, 0d, -35, 0d, -319, 0d, 1622d, 1d, -2201, 0d, 79d, 0d, 10d, -57, 2363d, 0d, -10162, 0d, -41, -12, 62d, 0d, 30d, 1d, -14, 89d, -2721, 0d, 5780d, -19, 26674d, -10, -52964, -2, -5, 30d, -4, 111d, -317, -1, 1369d, 0d, 93d, -6, -564, 9d, -115913, 0d, -113, 15d, 10d, -62, 99d, 0d, 891d, -7, 36d, 0d, 108d, 0d, -42, -2, 7d, 75d, -50, 21d, 86822d, 104d, 275441d, 65d, -1096109, 1d, -56, 3d, 31d, 66d, 63d, -1, 307d, 7d, 1097d, -34, 17453d, 0d, -22, 250d, 57d, 43d, 120d, 200d, -297, 0d, 1269d, 0d, 166d, 0d, -662, 0d, 40d, 28d, 1521d, 0d, -23, 288d, 351d, -2, -729, 0d, -22, -52, -96, -21, -139, -1, 589d, 0d, 35d, 0d, 210d, 7d, -118, -119, 62d, 0d, -583, -26, -42, 5d, -73, 152d, -330, -1759, 983d, -479, -23, -19, -522, -15, -185, -533, 739d, 1559d, -1300, 614d, -7332, 52d, -15836758, 0d, 524d, 16d, 185d, 532d, 23d, 18d, 330d, 1751d, -978, 476d, 73d, -151, 519d, 18d, 38d, 0d, 105d, 113d, -178, -37, 26d, 0d, 262d, 1d, -1139, 0d, 71d, 17d, 16d, 42d, 151d, 0d, 16d, -148, 4147d, 0d, 149d, 0d, -30, 0d, 2980d, 9d, 9454d, 2d, -37519, 0d, -28, -49, 37d, -1, 2d, -31, 33870d, 0d, -208, 1d, -59, 1d, -13105, 68d, -41564, 21d, 165148d, 3d, -1022, 0d, -40, 0d, -132, 0d, -228, 0d, 95d, 0d, -138, -16, -126, 16d, 24d, 5d, -57, -346, 191d, -94, -14, -11, -12, -37, -3053364, -1, 13d, 36d, 17d, 13d, 51d, 327d, -179, 90d, 138d, 16d, 233d, 0d, 62d, 0d, 1164d, 0d, -5000, 0d, -407, 117d, 770d, 9d, -4, 1d, 21d, 2d, 1d, 0d, -16869, 0d, -1, 0d, 1d, 0d, 35d, 0d, -78, 0d, 78d, 0d, -533, 0d, -31, 1d, -2448, -6, -7768, -1, 30812d, 0d, 37d, 0d, -227, 0d, 197d, 0d, -846, 0d, -77, 0d, 4171d, 0d, -67, 0d, 287d, 0d, 2532d, 0d, -19, 0d, -40, 0d, -56, 0d, 128d, 0d, 83d, 0d, -45, 0d, -36, 0d, -92, 0d, -134, 0d, 714d, 0d, -1495, 0d, 32d, 0d, -981, 0d, 15d, 0d, -166, 0d, -59, 0d, 4923d, 0d, -21203, 0d, 246d, 0d, 15d, 0d, 104d, 0d, 1683d, 0d, -3523, 0d, -865, 0d, -25, 1d, -186329, -1, 10d, 0d, 50d, 0d, 53d, 0d, 5455d, -45, 17271d, -10, -68747, 0d, 69d, -2, -7604, 0d, -724, 1d, 3101d, 0d, -46, 0d, 200d, 0d, -44, 0d, 97d, 0d, -53, 0d, 62d, 0d, -54, -4, 88d, -24, -9, -36, -581, 27d, -914711, 3d, 8d, 35d, -86, 24d, 51d, 3d, 48d, 0d, 26d, 0d, 133d, 1d, -577, 0d, 105d, 0d, -3, -1, 3194d, 0d, 528d, 0d, -2263, 0d, 2028d, 0d, -3266, 1d, 14016d, 0d, 10d, 0d, -41, 0d, -100, 0d, -32, 0d, -124348, 0d, 16d, 0d, -325, 0d, 50d, -1, 1d, 0d, -553, 0d, 0d, 0d, 0d, 0d, 2d, 0d, -34, 0d, -444, 0d, 1902d, 0d, 9d, 0d, -37, 0d, 254d, 0d, 156d, 0d, -2, 0d, -35, 0d, -48, 0d, -368, 0d, 327d, 0d, -686, 0d, -2263, 0d, 1952d, 0d, -8418, 0d, -13, 0d, 52d, 0d, 9d, 0d, 21d, 0d, -261, 0d, -62404, 0d, 0d, 0d, 79d, 0d, 1056d, 0d, -4547, 0d, -351, 0d, -305, 0d, 1310d, 0d, -1, 0d, 6d, 0d, 0d, 0d, -55953, 0d, -80, 0d, 0d, 0d, 168d, 0d, -147, 0d, 127d, 0d, -265, 0d, 1138d, 0d, -1, 0d, -9, 0d, -8, 0d, -5984, 0d, -22, 0d, -5, 0d, 0d, 0d, 0d, 0d, 127d, 0d, -2, 0d, 10d, 0d, -31, 0d, -29, 0d, -286, 0d, -98, 0d, -1535, 0d, 252d, 0d, -1087, 0d, 43d, 0d, 4d, 0d, -19, 0d, -7620, 0d, 29d, 0d, -322, 0d, 203d, 0d, 0d, 0d, -3587, 0d, 10d, 0d, 0d, 0d, 94d, 0d, 0d, 0d, -1, 0d, -1, 0d, -315, 0d, 1d, 0d, 0d, 0d, 0d, 0d, -30, 0d, -94, 0d, -460, 0d, 1d, 0d, -114, 0d, 0d, 0d, -746, 0d, 4d, 0d, -23, 0d, 24d, 0d, 0d, 0d, -237, 0d, 1d, 0d, 0d, 0d, -18, 0d, 0d, 0d, 0d, 0d, -16, 0d, -76, 0d, -67, 0d, 0d, 0d, -16, 0d, 0d, 0d };















































































































































































































































































































































































































































































































































































































































































        internal static int[] args = new int[] { 0, 3, 3, 4, 3, -8, 4, 3, 5, 1, 2, 2, 5, -5, 6, 2, 5, -1, 10, 2, 13, -1, 11, 3, 3, -7, 4, 0, 3, 1, 13, -1, 11, 2, 5, 1, 2, 4, 5, -10, 6, 0, 4, 2, 10, -2, 13, 14, 3, -23, 4, 1, 3, 3, 2, -7, 3, 4, 4, 1, 3, -1, 13, 18, 2, -16, 3, 2, 2, 8, 2, -13, 3, 1, 5, 2, 10, -2, 13, 2, 3, -3, 5, 1, 6, 0, 3, -1, 13, 26, 2, -29, 3, 0, 3, 1, 10, -1, 11, 2, 4, 1, 4, 1, 10, -1, 13, 3, 2, -4, 3, 1, 4, 1, 10, -1, 13, 3, 3, -4, 4, 0, 3, -1, 10, 15, 2, -12, 3, 0, 4, 2, 10, -3, 13, 24, 2, -24, 3, 0, 3, -1, 10, 23, 2, -25, 3, 0, 4, 1, 10, -1, 11, 1, 3, 1, 6, 0, 4, 2, 10, -2, 11, 5, 2, -6, 3, 0, 4, 2, 10, -2, 13, 6, 2, -8, 3, 0, 4, -2, 10, 1, 13, 12, 2, -8, 3, 1, 5, -1, 10, 1, 13, -1, 11, 20, 2, -20, 3, 1, 4, -2, 10, 1, 13, 3, 1, -1, 3, 1, 5, 2, 10, -2, 13, 2, 3, -5, 5, 5, 6, 0, 4, 2, 10, -2, 13, 2, 3, -3, 5, 1, 4, 2, 10, -2, 13, 6, 3, -8, 4, 0, 4, -2, 10, 1, 13, 20, 2, -21, 3, 1, 4, 1, 10, -1, 11, 1, 3, 1, 5, 0, 1, 1, 6, 0, 4, 2, 10, -2, 13, 5, 3, -6, 4, 0, 3, 3, 2, -5, 3, 2, 5, 0, 2, -1, 11, 1, 14, 1, 4, 2, 10, -2, 13, 2, 3, -2, 5, 0, 2, 1, 3, -2, 4, 1, 4, 1, 10, -1, 11, 5, 2, -7, 3, 0, 1, 1, 5, 0, 2, 7, 3, -13, 4, 0, 4, -2, 10, 1, 13, 15, 2, -13, 3, 0, 4, 2, 10, -2, 13, 3, 2, -3, 3, 0, 2, -2, 11, 2, 14, 1, 3, 1, 10, 1, 12, -1, 13, 1, 3, -1, 13, 21, 2, -21, 3, 0, 2, 3, 2, -5, 3, 0, 2, 2, 3, -4, 4, 1, 2, 5, 2, -8, 3, 0, 3, -1, 13, 23, 2, -24, 3, 0, 2, 6, 3, -11, 4, 0, 1, 2, 5, 0, 2, 3, 3, -6, 4, 0, 2, 5, 3, -9, 4, 0, 4, 1, 10, -1, 11, 1, 3, -2, 5, 0, 3, 2, 10, 2, 12, -2, 13, 1, 2, 2, 2, -3, 3, 2, 2, 4, 3, -7, 4, 0, 2, 2, 13, -2, 11, 0, 2, 3, 3, -5, 4, 0, 2, 1, 2, -2, 3, 0, 2, 2, 3, -3, 4, 0, 4, 1, 10, -1, 11, 4, 2, -5, 3, 0, 2, 1, 3, -1, 4, 0, 2, 4, 2, -6, 3, 0, 4, 2, 10, -2, 13, 2, 2, -2, 3, 0, 3, 1, 10, -1, 11, 1, 2, 0, 2, 1, 2, -1, 3, 0, 3, 1, 12, 2, 13, -2, 11, 0, 2, 5, 3, -8, 4, 0, 2, 1, 3, -3, 5, 0, 3, 2, 10, 1, 12, -2, 13, 1, 2, 4, 3, -6, 4, 0, 2, 1, 3, -2, 5, 1, 2, 3, 3, -4, 4, 0, 2, 3, 2, -4, 3, 1, 2, 1, 10, -1, 13, 0, 2, 1, 3, -1, 5, 0, 2, 1, 3, -2, 6, 0, 2, 2, 3, -2, 4, 0, 2, 1, 3, -1, 6, 0, 2, 8, 2, -14, 3, 0, 3, 1, 3, 2, 5, -5, 6, 1, 3, 5, 3, -8, 4, 3, 5, 1, 1, 1, 12, 3, 3, 3, 3, -8, 4, 3, 5, 1, 3, 1, 3, -2, 5, 5, 6, 0, 2, 8, 2, -12, 3, 0, 2, 1, 3, 1, 5, 0, 3, 2, 10, 1, 12, -2, 11, 1, 2, 5, 2, -7, 3, 0, 3, 1, 10, 1, 13, -2, 11, 0, 2, 2, 2, -2, 3, 0, 2, 5, 3, -7, 4, 0, 3, 1, 12, -2, 13, 2, 11, 0, 2, 4, 3, -5, 4, 0, 2, 3, 3, -3, 4, 0, 1, 1, 2, 0, 3, 3, 10, 1, 12, -3, 13, 0, 2, 2, 3, -4, 5, 0, 2, 2, 3, -3, 5, 0, 2, 2, 10, -2, 13, 0, 2, 2, 3, -2, 5, 0, 2, 3, 2, -3, 3, 0, 3, 1, 10, -1, 12, -1, 13, 1, 2, 2, 3, -1, 5, 0, 2, 2, 3, -2, 6, 0, 1, 2, 12, 2, 3, -2, 10, 1, 11, 1, 14, 0, 2, 2, 10, -2, 11, 0, 2, 2, 2, -1, 3, 0, 4, -2, 10, 2, 13, 1, 2, -1, 3, 0, 2, 4, 2, -4, 3, 0, 2, 3, 10, -3, 13, 0, 4, -2, 10, 2, 13, 1, 3, -1, 5, 0, 2, 3, 3, -3, 5, 0, 3, 2, 10, -1, 12, -2, 13, 2, 3, 3, 10, -1, 13, -2, 11, 0, 1, 3, 12, 1, 4, -2, 10, 2, 13, 2, 2, -2, 3, 0, 3, 2, 10, -1, 12, -2, 11, 1, 2, 5, 2, -5, 3, 0, 2, 4, 10, -4, 13, 0, 2, 6, 2, -6, 3, 0, 3, 2, 10, -2, 12, -2, 13, 1, 3, 4, 10, -2, 13, -2, 11, 0, 3, 2, 10, -2, 12, -2, 11, 0, 2, 7, 2, -7, 3, 0, 3, 2, 10, -3, 12, -2, 13, 0, 2, 8, 2, -8, 3, 0, 2, 9, 2, -9, 3, 0, 2, 10, 2, -10, 3, 0, 3, 2, 10, -4, 12, -1, 13, 0, 3, 4, 10, -2, 12, -3, 13, 0, 4, 4, 10, -1, 12, -1, 13, -2, 11, 0, 3, 2, 10, -3, 12, -1, 13, 1, 4, -2, 10, 1, 13, 3, 3, -2, 5, 0, 3, 4, 10, -1, 12, -3, 13, 0, 4, -2, 10, 1, 13, 3, 3, -3, 5, 0, 4, 2, 10, -2, 12, 1, 13, -2, 11, 0, 4, -2, 10, 1, 13, 2, 2, -1, 3, 0, 3, 3, 10, -1, 12, -2, 11, 0, 3, 4, 10, -1, 13, -2, 11, 0, 3, 2, 10, -2, 12, -1, 13, 2, 4, -2, 10, 1, 13, 2, 3, -1, 5, 0, 3, 3, 10, -1, 12, -2, 13, 0, 4, -2, 10, 1, 13, 3, 2, -3, 3, 0, 4, -2, 10, 1, 13, 2, 3, -2, 5, 0, 2, 4, 10, -3, 13, 0, 4, -2, 10, 1, 13, 2, 3, -3, 5, 0, 3, -2, 10, 1, 13, 1, 2, 0, 4, 2, 10, -1, 12, 1, 13, -2, 11, 1, 4, -2, 10, 1, 13, 2, 2, -2, 3, 0, 2, 3, 12, -1, 13, 0, 2, 3, 10, -2, 11, 0, 2, 1, 10, -2, 12, 0, 4, 4, 10, 1, 12, -1, 13, -2, 11, 0, 3, -1, 13, 3, 2, -2, 3, 0, 3, -1, 13, 3, 3, -2, 5, 0, 3, -2, 10, 18, 2, -15, 3, 0, 5, 2, 10, -1, 13, 3, 3, -8, 4, 3, 5, 0, 3, 2, 10, -1, 12, -1, 13, 2, 5, -2, 10, 1, 13, 5, 3, -8, 4, 3, 5, 0, 5, -2, 10, 1, 13, 1, 3, 2, 5, -5, 6, 0, 4, 2, 10, -2, 13, 18, 2, -17, 3, 0, 4, -2, 10, 1, 13, 1, 3, -1, 6, 0, 4, -2, 10, 1, 13, 2, 3, -2, 4, 0, 4, -2, 10, 1, 13, 1, 3, -1, 5, 0, 2, 3, 10, -2, 13, 0, 4, -2, 10, 1, 13, 3, 2, -4, 3, 0, 4, -2, 10, 1, 13, 3, 3, -4, 4, 0, 4, -2, 10, 1, 13, 1, 3, -2, 5, 0, 3, 4, 10, 1, 12, -3, 13, 0, 4, -2, 10, 1, 13, 1, 3, -3, 5, 0, 3, -1, 13, 4, 2, -4, 3, 0, 4, -2, 10, 1, 13, 1, 2, -1, 3, 0, 4, -2, 10, 1, 13, 1, 3, -1, 4, 0, 4, -2, 10, 1, 13, 2, 3, -3, 4, 0, 4, -2, 10, 1, 13, 3, 3, -5, 4, 0, 3, 2, 10, 1, 13, -2, 11, 0, 4, -2, 10, -1, 13, 1, 11, 1, 14, 0, 4, -2, 10, 1, 13, 2, 2, -3, 3, 1, 2, 2, 12, -1, 13, 1, 3, 3, 10, 1, 12, -2, 11, 0, 4, 2, 10, -1, 13, 2, 3, -4, 4, 0, 4, 2, 10, -1, 13, 3, 2, -5, 3, 0, 2, 1, 10, -1, 12, 1, 3, -1, 13, 3, 2, -3, 3, 0, 3, -2, 10, 1, 13, 1, 5, 0, 4, 2, 10, -1, 13, 1, 3, -2, 4, 0, 3, -1, 13, 2, 3, -2, 5, 0, 4, 2, 10, -1, 13, -1, 11, 1, 14, 0, 3, -1, 13, 5, 3, -6, 4, 0, 3, -2, 10, 1, 13, 1, 6, 0, 3, -1, 10, 1, 3, -1, 5, 0, 4, -2, 10, 1, 13, 8, 2, -13, 3, 1, 3, -2, 10, 18, 2, -16, 3, 1, 5, -2, 10, 1, 13, 3, 2, -7, 3, 4, 4, 1, 4, 2, 10, -1, 13, 2, 5, -5, 6, 1, 5, 2, 10, -1, 13, 4, 3, -8, 4, 3, 5, 1, 2, 2, 10, -1, 13, 2, 5, -2, 10, 1, 13, 4, 3, -8, 4, 3, 5, 1, 4, -2, 10, 1, 13, 2, 5, -5, 6, 1, 5, 2, 10, -1, 13, 3, 2, -7, 3, 4, 4, 0, 4, 2, 10, -2, 13, 18, 2, -16, 3, 1, 4, 2, 10, -1, 13, 8, 2, -13, 3, 1, 3, -1, 10, 3, 2, -4, 3, 0, 3, -1, 13, 6, 2, -8, 3, 0, 3, -1, 13, 2, 3, -3, 5, 0, 3, -1, 13, 6, 3, -8, 4, 0, 3, 2, 10, -1, 13, 1, 6, 0, 4, -2, 10, 1, 13, -1, 11, 1, 14, 0, 4, -2, 10, 1, 13, 1, 3, -2, 4, 0, 3, 2, 10, -1, 13, 1, 5, 0, 3, 3, 10, 1, 12, -2, 13, 0, 4, -2, 10, 1, 13, 3, 2, -5, 3, 0, 4, -2, 10, 1, 13, 2, 3, -4, 4, 0, 2, -1, 13, 1, 2, 0, 4, 2, 10, -1, 13, 2, 2, -3, 3, 0, 3, -1, 10, 1, 2, -1, 3, 0, 3, -1, 13, 4, 2, -5, 3, 0, 3, 2, 10, -3, 13, 2, 11, 0, 4, 2, 10, -1, 13, 2, 3, -3, 4, 0, 3, -1, 13, 2, 2, -2, 3, 0, 4, 2, 10, -1, 13, 1, 2, -1, 3, 0, 4, 2, 10, 1, 12, 1, 13, -2, 11, 0, 3, -2, 13, 18, 2, -15, 3, 0, 2, 1, 12, -1, 13, 2, 3, -1, 13, 1, 3, -1, 6, 0, 4, 2, 10, -1, 13, 1, 3, -2, 5, 0, 3, -1, 13, 2, 3, -2, 4, 0, 3, -1, 13, 1, 3, -1, 5, 0, 4, 2, 10, -1, 13, 3, 3, -4, 4, 0, 1, 1, 10, 0, 3, -1, 13, 3, 2, -4, 3, 0, 3, -1, 13, 3, 3, -4, 4, 0, 4, 2, 10, -1, 13, 1, 3, -1, 5, 0, 4, 2, 10, -1, 13, 2, 3, -2, 4, 0, 3, -1, 13, 1, 3, -2, 5, 0, 3, 2, 10, 1, 12, -1, 13, 2, 3, 1, 12, 1, 13, -2, 11, 0, 3, -1, 13, 1, 2, -1, 3, 0, 4, 2, 10, -1, 13, 2, 2, -2, 3, 0, 3, -1, 13, 4, 2, -6, 3, 0, 3, -1, 13, 2, 3, -3, 4, 0, 3, 1, 13, 1, 2, -2, 3, 0, 4, 2, 10, -1, 13, 3, 3, -3, 4, 0, 2, 3, 13, -2, 11, 0, 4, 2, 10, -1, 13, 4, 2, -5, 3, 0, 3, 1, 10, 1, 2, -1, 3, 0, 3, -1, 13, 2, 2, -3, 3, 1, 3, 2, 10, 2, 12, -3, 13, 0, 3, 2, 10, -1, 13, 1, 2, 0, 3, 1, 13, 2, 3, -4, 4, 0, 3, 1, 13, 3, 2, -5, 3, 0, 2, 21, 2, -21, 3, 0, 3, 1, 10, 1, 12, -2, 13, 1, 4, 2, 10, -1, 13, 2, 3, -4, 5, 0, 4, 2, 10, -1, 13, 7, 3, -10, 4, 0, 2, -1, 13, 1, 5, 0, 3, 1, 13, 1, 3, -2, 4, 0, 4, 2, 10, -3, 13, 2, 3, -2, 5, 0, 3, 1, 10, 1, 3, -2, 5, 0, 3, 1, 13, -1, 11, 1, 14, 1, 2, -1, 13, 1, 6, 0, 4, 2, 10, -1, 13, 6, 3, -8, 4, 1, 4, 2, 10, -1, 13, 2, 3, -3, 5, 1, 3, -1, 13, 8, 3, -15, 4, 0, 4, 2, 10, -1, 13, 6, 2, -8, 3, 0, 5, 2, 10, -1, 13, -2, 11, 5, 2, -6, 3, 0, 3, 1, 10, 3, 3, -4, 4, 0, 3, 1, 10, 3, 2, -4, 3, 1, 4, 1, 10, -1, 13, -1, 11, 2, 4, 0, 3, -2, 13, 26, 2, -29, 3, 0, 3, -1, 13, 8, 2, -13, 3, 0, 3, -2, 13, 18, 2, -16, 3, 2, 4, -1, 13, 3, 2, -7, 3, 4, 4, 0, 3, 1, 13, 2, 5, -5, 6, 1, 4, 1, 13, 4, 3, -8, 4, 3, 5, 1, 1, 1, 13, 3, 4, -1, 13, 4, 3, -8, 4, 3, 5, 1, 3, -1, 13, 2, 5, -5, 6, 1, 4, 1, 13, 3, 2, -7, 3, 4, 4, 0, 2, 18, 2, -16, 3, 1, 3, 1, 13, 8, 2, -13, 3, 2, 2, 26, 2, -29, 3, 0, 4, 1, 10, 1, 13, -1, 11, 2, 4, 0, 5, 2, 10, 1, 13, -2, 11, 5, 2, -6, 3, 0, 3, 1, 13, 8, 3, -15, 4, 1, 4, 2, 10, -3, 13, 2, 3, -3, 5, 0, 3, 1, 10, 1, 3, -1, 5, 0, 2, 1, 13, 1, 6, 0, 4, 2, 10, -1, 13, 5, 3, -6, 4, 0, 3, 1, 10, 2, 3, -2, 4, 0, 3, -1, 13, -1, 11, 1, 14, 1, 4, 2, 10, -1, 13, 2, 3, -5, 6, 0, 4, 2, 10, -1, 13, 2, 3, -2, 5, 0, 5, 2, 10, -1, 13, 2, 3, -4, 5, 5, 6, 0, 3, -1, 13, 1, 3, -2, 4, 1, 2, 1, 13, 1, 5, 0, 4, 2, 10, -1, 13, 4, 3, -4, 4, 0, 4, 2, 10, -1, 13, 3, 2, -3, 3, 0, 4, 2, 10, 2, 12, -1, 13, -2, 11, 0, 2, 1, 10, 1, 12, 2, 3, -1, 13, 3, 2, -5, 3, 0, 3, -1, 13, 2, 3, -4, 4, 0, 4, 2, 10, -1, 13, 2, 3, -1, 5, 0, 4, 2, 10, -1, 13, 2, 3, -2, 6, 0, 3, 1, 10, 1, 12, -2, 11, 0, 3, 2, 10, 2, 12, -1, 13, 1, 3, 1, 13, 2, 2, -3, 3, 1, 3, -1, 13, 1, 11, 1, 14, 0, 2, 1, 13, -2, 11, 0, 4, 2, 10, -1, 13, 5, 2, -6, 3, 0, 3, -1, 13, 1, 2, -2, 3, 0, 3, 1, 13, 2, 3, -3, 4, 0, 3, 1, 13, 1, 2, -1, 3, 0, 4, 2, 10, -1, 13, 4, 2, -4, 3, 0, 3, 2, 10, 1, 12, -3, 13, 1, 3, 1, 13, 1, 3, -2, 5, 0, 3, 1, 13, 3, 3, -4, 4, 0, 3, 1, 13, 3, 2, -4, 3, 0, 2, 1, 10, -2, 13, 0, 4, 2, 10, -1, 13, 3, 3, -4, 5, 0, 3, 1, 13, 1, 3, -1, 5, 0, 3, 1, 13, 2, 3, -2, 4, 0, 3, 1, 13, 1, 3, -1, 6, 0, 4, 2, 10, -1, 13, 3, 3, -3, 5, 0, 4, 2, 10, -1, 13, 6, 2, -7, 3, 0, 2, 1, 12, 1, 13, 2, 4, 2, 10, -1, 13, 3, 3, -2, 5, 0, 4, 2, 10, 1, 12, -1, 13, -2, 11, 0, 2, 1, 10, 2, 12, 0, 2, 1, 10, -2, 11, 0, 3, 1, 13, 2, 2, -2, 3, 0, 3, 1, 12, -1, 13, 2, 11, 0, 4, 2, 10, -1, 13, 5, 2, -5, 3, 0, 3, 1, 13, 2, 3, -3, 5, 0, 2, 2, 10, -3, 13, 0, 3, 1, 13, 2, 3, -2, 5, 0, 3, 1, 13, 3, 2, -3, 3, 0, 3, 1, 10, -1, 12, -2, 13, 0, 4, 2, 10, -1, 13, 6, 2, -6, 3, 0, 2, 2, 12, 1, 13, 1, 3, 2, 10, -1, 13, -2, 11, 0, 3, 1, 10, -1, 12, -2, 11, 0, 3, 2, 10, 1, 13, -4, 11, 0, 3, 1, 13, 4, 2, -4, 3, 0, 4, 2, 10, -1, 13, 7, 2, -7, 3, 0, 3, 2, 10, -1, 12, -3, 13, 1, 2, 3, 12, 1, 13, 0, 4, 2, 10, -1, 12, -1, 13, -2, 11, 0, 3, 1, 13, 5, 2, -5, 3, 0, 4, 2, 10, -1, 13, 8, 2, -8, 3, 0, 3, 2, 10, -2, 12, -3, 13, 0, 4, 2, 10, -1, 13, 9, 2, -9, 3, 0, 3, 4, 10, -3, 12, -2, 13, 0, 2, 2, 10, -4, 12, 0, 3, 4, 10, -2, 12, -2, 13, 1, 2, 6, 10, -4, 13, 0, 3, 4, 10, -1, 12, -2, 11, 0, 2, 2, 10, -3, 12, 1, 3, 3, 10, -2, 12, -1, 13, 0, 3, -2, 10, 3, 3, -2, 5, 0, 3, 4, 10, -1, 12, -2, 13, 1, 3, -2, 10, 3, 3, -3, 5, 0, 2, 5, 10, -3, 13, 0, 3, -2, 10, 4, 2, -4, 3, 0, 3, -2, 10, 2, 2, -1, 3, 0, 2, 4, 10, -2, 11, 0, 2, 2, 10, -2, 12, 2, 3, -2, 10, 3, 3, -2, 4, 0, 3, -2, 10, 2, 3, -1, 5, 0, 3, 3, 10, -1, 12, -1, 13, 1, 3, -2, 10, 3, 2, -3, 3, 0, 3, -2, 10, 2, 3, -2, 5, 0, 2, 4, 10, -2, 13, 0, 3, -2, 10, 2, 3, -3, 5, 0, 2, -2, 10, 1, 2, 0, 4, 2, 10, -1, 12, 2, 13, -2, 11, 0, 3, -2, 10, 2, 2, -2, 3, 0, 3, 3, 10, 1, 13, -2, 11, 0, 3, 4, 10, 1, 12, -2, 11, 0, 4, 2, 10, -1, 12, -1, 11, 1, 14, 0, 4, -2, 10, -1, 13, 18, 2, -15, 3, 0, 4, 2, 10, 3, 3, -8, 4, 3, 5, 0, 2, 2, 10, -1, 12, 2, 4, -2, 10, 5, 3, -8, 4, 3, 5, 0, 4, 2, 10, -1, 13, 18, 2, -17, 3, 0, 3, -2, 10, 1, 3, -1, 6, 0, 3, -2, 10, 2, 3, -2, 4, 0, 3, -2, 10, 1, 3, -1, 5, 0, 2, 3, 10, -1, 13, 0, 3, -2, 10, 3, 2, -4, 3, 0, 3, -2, 10, 3, 3, -4, 4, 0, 3, -2, 10, 1, 3, -2, 5, 0, 3, 4, 10, 1, 12, -2, 13, 1, 4, 2, 10, -1, 12, -2, 13, 2, 11, 0, 3, -2, 10, 1, 2, -1, 3, 0, 3, -2, 10, 2, 3, -3, 4, 0, 3, 2, 10, 2, 13, -2, 11, 0, 3, -2, 10, 2, 2, -3, 3, 0, 2, 2, 12, -2, 13, 1, 3, 2, 10, 2, 3, -4, 4, 0, 3, 2, 10, 3, 2, -5, 3, 0, 3, 1, 10, -1, 12, 1, 13, 1, 3, -2, 13, 3, 2, -3, 3, 0, 2, -2, 10, 1, 5, 0, 3, 2, 10, 1, 3, -2, 4, 0, 3, -2, 13, 2, 3, -2, 5, 0, 3, 2, 10, -1, 11, 1, 14, 0, 4, 4, 10, -2, 13, 2, 3, -3, 5, 0, 3, -2, 10, 8, 2, -13, 3, 0, 4, -2, 10, -1, 13, 18, 2, -16, 3, 1, 4, -2, 10, 3, 2, -7, 3, 4, 4, 0, 4, 2, 10, 4, 3, -8, 4, 3, 5, 1, 1, 2, 10, 3, 4, -2, 10, 4, 3, -8, 4, 3, 5, 1, 4, 2, 10, 3, 2, -7, 3, 4, 4, 0, 4, 2, 10, -1, 13, 18, 2, -16, 3, 1, 3, 2, 10, 8, 2, -13, 3, 0, 3, -2, 10, -1, 11, 1, 14, 0, 4, 4, 10, -2, 13, 2, 3, -2, 5, 0, 3, -2, 10, 1, 3, -2, 4, 0, 2, 2, 10, 1, 5, 0, 4, 4, 10, -2, 13, 3, 2, -3, 3, 0, 3, 3, 10, 1, 12, -1, 13, 1, 3, -2, 10, 3, 2, -5, 3, 0, 3, -2, 10, 2, 3, -4, 4, 0, 3, 4, 10, 2, 12, -2, 13, 0, 3, 2, 10, 2, 2, -3, 3, 0, 3, 2, 10, -2, 13, 2, 11, 0, 3, 2, 10, 1, 2, -1, 3, 0, 4, 2, 10, 1, 12, 2, 13, -2, 11, 0, 2, 1, 12, -2, 13, 2, 3, 2, 10, 1, 3, -2, 5, 0, 3, -2, 13, 1, 3, -1, 5, 0, 3, 2, 10, 3, 2, -4, 3, 0, 2, 1, 10, 1, 13, 0, 3, 2, 10, 1, 3, -1, 5, 0, 3, 2, 10, 2, 3, -2, 4, 0, 2, 2, 10, 1, 12, 2, 2, 1, 12, -2, 11, 0, 3, -2, 13, 1, 2, -1, 3, 0, 3, 1, 10, -1, 13, 2, 11, 0, 3, 2, 10, 2, 2, -2, 3, 0, 3, 1, 10, 1, 12, -3, 13, 0, 3, 2, 13, -1, 11, 1, 14, 0, 3, 2, 10, 2, 3, -3, 5, 0, 3, 2, 10, 6, 2, -8, 3, 0, 3, -3, 13, 18, 2, -16, 3, 1, 3, 2, 13, 2, 5, -5, 6, 0, 4, 2, 13, 4, 3, -8, 4, 3, 5, 0, 1, 2, 13, 0, 4, -2, 13, 4, 3, -8, 4, 3, 5, 0, 3, -2, 13, 2, 5, -5, 6, 0, 3, 1, 13, 18, 2, -16, 3, 1, 3, -2, 13, -1, 11, 1, 14, 0, 3, 2, 10, 2, 3, -2, 5, 0, 3, 2, 10, 3, 2, -3, 3, 0, 3, 1, 10, 1, 12, 1, 13, 1, 2, 2, 10, 2, 12, 1, 2, 1, 11, 1, 14, 1, 4, -1, 13, -2, 11, 18, 2, -16, 3, 0, 1, 2, 11, 0, 4, -1, 13, 2, 11, 18, 2, -16, 3, 0, 2, -3, 11, 1, 14, 0, 3, 2, 13, 1, 2, -1, 3, 0, 3, 2, 10, 4, 2, -4, 3, 0, 3, 2, 10, 1, 12, -4, 13, 0, 2, 1, 10, -3, 13, 0, 3, 2, 13, 1, 3, -1, 5, 0, 2, 1, 12, 2, 13, 2, 3, 1, 10, 2, 12, 1, 13, 0, 3, 1, 10, -1, 13, -2, 11, 0, 2, 1, 12, 2, 11, 1, 3, 2, 10, 5, 2, -5, 3, 0, 2, 2, 10, -4, 13, 0, 3, 2, 10, 6, 2, -6, 3, 0, 2, 2, 12, 2, 13, 0, 3, 2, 10, -2, 13, -2, 11, 0, 2, 2, 12, 2, 11, 0, 2, 2, 10, -4, 11, 0, 3, 2, 10, 7, 2, -7, 3, 0, 3, 2, 10, -1, 12, -4, 13, 0, 4, 2, 10, -1, 12, -2, 13, -2, 11, 0, 3, 2, 10, 8, 2, -8, 3, 0, 3, 2, 10, 9, 2, -9, 3, 0, 3, 4, 10, -3, 12, -1, 13, 0, 3, 6, 10, -1, 12, -3, 13, 0, 3, 4, 10, -2, 12, -1, 13, 1, 3, 5, 10, -1, 12, -2, 13, 0, 2, 6, 10, -3, 13, 0, 4, 4, 10, -1, 12, 1, 13, -2, 11, 0, 3, 2, 10, -3, 12, 1, 13, 0, 2, 3, 10, -2, 12, 0, 3, 4, 10, -1, 12, -1, 13, 1, 2, 5, 10, -2, 13, 0, 3, 6, 10, 1, 12, -3, 13, 0, 3, 4, 10, 1, 13, -2, 11, 0, 3, 2, 10, -2, 12, 1, 13, 1, 2, 3, 10, -1, 12, 0, 4, -2, 10, -1, 13, 2, 3, -2, 5, 0, 2, 4, 10, -1, 13, 0, 4, 2, 10, -2, 12, -1, 13, 2, 11, 0, 3, 4, 10, -3, 13, 2, 11, 0, 4, -2, 10, -1, 13, 2, 2, -2, 3, 0, 3, 2, 10, -1, 12, 1, 13, 2, 4, -2, 10, -1, 13, 1, 3, -1, 5, 0, 1, 3, 10, 0, 3, 4, 10, 1, 12, -1, 13, 1, 4, 2, 10, -1, 12, -1, 13, 2, 11, 1, 4, -2, 10, -1, 13, 1, 2, -1, 3, 0, 3, 2, 10, 3, 13, -2, 11, 0, 2, 2, 12, -3, 13, 0, 3, 1, 10, -1, 12, 2, 13, 0, 4, 2, 10, 1, 13, -1, 11, 1, 14, 0, 4, -2, 10, -2, 13, 18, 2, -16, 3, 0, 5, 2, 10, 1, 13, 4, 3, -8, 4, 3, 5, 0, 2, 2, 10, 1, 13, 1, 5, -2, 10, -1, 13, 4, 3, -8, 4, 3, 5, 0, 3, 2, 10, 18, 2, -16, 3, 0, 4, -2, 10, -1, 13, -1, 11, 1, 14, 0, 4, 4, 10, -1, 13, 2, 3, -2, 5, 0, 4, 4, 10, -1, 13, 3, 2, -3, 3, 0, 2, 3, 10, 1, 12, 1, 3, 4, 10, 2, 12, -1, 13, 0, 4, 2, 10, -1, 13, 1, 11, 1, 14, 0, 3, 2, 10, -1, 13, 2, 11, 0, 2, 1, 12, -3, 13, 1, 2, 1, 10, 2, 13, 0, 3, 2, 10, 1, 12, 1, 13, 1, 3, 1, 12, -1, 13, -2, 11, 1, 2, 1, 10, 2, 11, 0, 4, 2, 10, 1, 12, -1, 13, 2, 11, 0, 1, 3, 13, 0, 4, 2, 10, 1, 13, 2, 3, -2, 5, 0, 3, 1, 10, 1, 12, 2, 13, 0, 3, 2, 10, 2, 12, 1, 13, 0, 3, 1, 13, 1, 11, 1, 14, 0, 2, 1, 13, 2, 11, 0, 3, 1, 10, 1, 12, 2, 11, 0, 4, 2, 10, 2, 12, -1, 13, 2, 11, 0, 2, 1, 13, -4, 11, 0, 2, 1, 10, -4, 13, 0, 2, 1, 12, 3, 13, 1, 3, 1, 12, 1, 13, 2, 11, 1, 2, 2, 10, -5, 13, 0, 3, 2, 10, -3, 13, -2, 11, 0, 3, 2, 10, -1, 13, -4, 11, 0, 3, 6, 10, -2, 12, -2, 13, 0, 2, 4, 10, -3, 12, 0, 3, 6, 10, -1, 12, -2, 13, 0, 2, 4, 10, -2, 12, 1, 2, 6, 10, -2, 13, 0, 2, 4, 10, -1, 12, 1, 2, 5, 10, -1, 13, 0, 3, 6, 10, 1, 12, -2, 13, 0, 4, 4, 10, -1, 12, -2, 13, 2, 11, 0, 3, 4, 10, 2, 13, -2, 11, 0, 3, 2, 10, -2, 12, 2, 13, 0, 1, 4, 10, 0, 3, 2, 10, -2, 12, 2, 11, 0, 3, 4, 10, -2, 13, 2, 11, 0, 3, 2, 10, -1, 12, 2, 13, 1, 2, 3, 10, 1, 13, 0, 2, 4, 10, 1, 12, 1, 3, 2, 10, -1, 12, 2, 11, 1, 3, 3, 10, -1, 13, 2, 11, 0, 2, 2, 10, 2, 13, 0, 3, 3, 10, 1, 12, 1, 13, 0, 3, 2, 10, 1, 11, 1, 14, 0, 2, 2, 10, 2, 11, 0, 2, 1, 12, -4, 13, 0, 2, 1, 10, 3, 13, 0, 3, 2, 10, 1, 12, 2, 13, 1, 3, 1, 12, -2, 13, -2, 11, 0, 3, 1, 10, 1, 13, 2, 11, 0, 3, 2, 10, 1, 12, 2, 11, 0, 1, 4, 13, 0, 3, 1, 10, 1, 12, 3, 13, 0, 2, 2, 13, 2, 11, 0, 4, 1, 10, 1, 12, 1, 13, 2, 11, 0, 1, 4, 11, 0, 2, 1, 12, 4, 13, 0, 3, 1, 12, 2, 13, 2, 11, 0, 3, 2, 10, -4, 13, -2, 11, 0, 3, 6, 10, -2, 12, -1, 13, 0, 2, 8, 10, -3, 13, 0, 3, 6, 10, -1, 12, -1, 13, 0, 3, 4, 10, -2, 12, 1, 13, 0, 2, 6, 10, -1, 13, 0, 3, 4, 10, -1, 12, 1, 13, 1, 3, 6, 10, 1, 12, -1, 13, 0, 4, 4, 10, -1, 12, -1, 13, 2, 11, 0, 3, 2, 10, -2, 12, 3, 13, 0, 2, 4, 10, 1, 13, 0, 3, 4, 10, -1, 13, 2, 11, 0, 3, 2, 10, -1, 12, 3, 13, 0, 3, 4, 10, 1, 12, 1, 13, 0, 4, 2, 10, -1, 12, 1, 13, 2, 11, 0, 2, 2, 10, 3, 13, 0, 3, 2, 10, 1, 13, 2, 11, 0, 3, 2, 10, -1, 13, 4, 11, 0, 3, 2, 10, 1, 12, 3, 13, 0, 3, 1, 12, -3, 13, -2, 11, 0, 3, 1, 10, 2, 13, 2, 11, 0, 4, 2, 10, 1, 12, 1, 13, 2, 11, 0, 1, 5, 13, 0, 2, 3, 13, 2, 11, 0, 2, 1, 13, 4, 11, 0, 3, 1, 12, 3, 13, 2, 11, 0, 2, 8, 10, -2, 13, 0, 2, 6, 10, -1, 12, 0, 1, 6, 10, 0, 3, 6, 10, -2, 13, 2, 11, 0, 3, 4, 10, -1, 12, 2, 13, 0, 3, 4, 10, -1, 12, 2, 11, 0, 2, 4, 10, 2, 13, 0, 2, 4, 10, 2, 11, 0, 3, 2, 10, -1, 12, 4, 13, 0, 3, 4, 10, 1, 12, 2, 13, 0, 4, 2, 10, -1, 12, 2, 13, 2, 11, 0, 2, 2, 10, 4, 13, 0, 3, 2, 10, 2, 13, 2, 11, 0, 2, 2, 10, 4, 11, 0, 1, 6, 13, 0, 2, 4, 13, 2, 11, 0, 2, 2, 13, 4, 11, 0, 3, 6, 10, -1, 12, 1, 13, 0, 2, 6, 10, 1, 13, 0, 2, 4, 10, 3, 13, 0, 3, 4, 10, 1, 13, 2, 11, 0, 2, 2, 10, 5, 13, 0, 3, 2, 10, 3, 13, 2, 11, 0, -1 };

























































































































































































































































































































































































































































































































































































































































        internal static KeplerGlobalCode.plantbl moonlr = new KeplerGlobalCode.plantbl(14, new int[19] { 3, 26, 29, 23, 5, 10, 0, 0, 0, 8, 4, 4, 6, 2, 0, 0, 0, 0, 0 }, 3, args, tabl, tabb, tabr, 0.00257356868953d, 3652500.0d, 0.0001d);










    }
}