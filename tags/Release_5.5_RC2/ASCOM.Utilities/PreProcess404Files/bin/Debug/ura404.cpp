/*
First date in file = 625296.50
Number of records = 16731.0
Days per record = 131.0
      Julian Years      Lon    Lat    Rad
 -3000.0 to  -2499.7:   0.35   0.06   0.42 
 -2499.7 to  -1999.7:   0.50   0.06   0.38 
 -1999.7 to  -1499.7:   0.39   0.07   0.34 
 -1499.7 to   -999.8:   0.34   0.06   0.30 
  -999.8 to   -499.8:   0.35   0.05   0.32 
  -499.8 to      0.2:   0.32   0.05   0.27 
     0.2 to    500.2:   0.26   0.04   0.25 
   500.2 to   1000.1:   0.28   0.04   0.25 
  1000.1 to   1500.1:   0.26   0.06   0.31 
  1500.1 to   2000.1:   0.33   0.05   0.24 
  2000.1 to   2500.0:   0.32   0.06   0.26 
  2500.0 to   3000.0:   0.34   0.06   0.32 
  3000.0 to   3000.4:  0.406  0.035  0.172 
*/
#include "structs.h"
static double tabl[] = {
          21.56000,       -4652.06828,   154246324.90417,     1130486.05080,

         330.11531,       -3020.20235,

          -8.03769,        -122.02019,

         212.45130,         254.23866,          25.39758,          60.08296,

        6949.85053,       51951.42606,       -1834.66531,       44481.91144,
       -3267.45825,       10776.65972,        -628.05388,         532.83011,
         -16.80583,         -30.05544,

        1420.33767,        2007.21040,         592.32842,        1541.61732,
        -163.55984,         121.14134,         114.74969,         -16.04944,

           0.06069,           0.00725,

          -0.16861,           0.28785,

           0.07399,          -0.09680,

           0.19936,          -0.41620,

           0.02922,           0.07398,

           0.17272,           0.05602,

           1.65461,          -0.68278,          -2.18745,          -0.85327,
           0.52467,          -0.30863,

           0.01598,           0.30017,

          -0.04190,          -0.03288,

          -0.02013,           0.02257,

          -0.54883,          -0.22701,          -0.09257,          -0.03921,

           0.02644,           0.04667,

           0.24773,          -0.16562,

       44242.85814,     -223163.54065,      123776.84417,     -206375.74884,
       70472.73820,      -27456.55173,        4065.74401,       13202.39154,
       -3260.72648,         802.50579,        -153.13236,        -503.81026,
          30.17812,         -31.91893,

         -65.14719,          77.78417,         -37.38185,          19.13337,
          -3.14043,          -0.21147,

           0.27143,           0.17424,

           0.04458,           0.10976,

          -0.41841,          -0.21887,          -0.09194,          -0.02303,

           0.02896,           0.10044,

           0.01385,           0.01723,

          -0.01126,          -0.09318,

         -57.95890,          29.69059,         -46.41390,           3.07177,
           0.42494,           2.33678,          -3.09621,           0.05256,

          -0.02134,          -0.35202,

          -0.44475,          -0.83135,

        1318.18265,       25605.86848,       -9168.38371,       18917.31507,
       -5145.74480,        2130.77612,        -485.25920,        -438.44867,
          19.97802,         -33.14800,

      -23383.91826,      -45133.19122,      -18520.80729,      -26549.95198,
       -2276.70124,       -2974.01604,         603.23665,         306.87616,
         -87.73070,         -32.49134,

      549975.14525,      261920.31896,      526261.09735,      362619.26839,
      150616.68873,      164643.90808,        9550.02662,       27381.83042,
       -1065.89047,        1024.20231,         -66.63822,         -44.75169,

         -92.10532,         -20.26930,

     -313205.95341,     1462242.64616,      112982.53079,     1865690.41965,
      308844.30901,      639864.93227,       89716.32843,       10378.80773,
        4395.08428,      -14565.35913,       -3016.07754,      -19348.64612,

        3838.36899,       -9813.42713,        6883.58821,       -6064.92588,
        2740.47455,        -176.29547,         241.91895,         268.44181,
          -6.13397,          17.92503,

          -0.01377,          -0.08742,

         387.51915,         257.03872,         152.81792,         221.56197,
         -22.94836,          29.56640,          -2.27801,           4.72805,
          -6.03420,          -0.36763,

           0.00667,           0.00443,

          -0.01405,           0.04658,

          -0.06533,          -0.01966,

           0.10738,           0.00443,

           0.02889,           0.01056,

           0.00900,          -0.02206,

           0.00013,           0.05281,

           0.03035,           0.34793,

           0.19460,           2.47360,

           0.18189,          -0.83895,           0.24983,          15.32050,

           0.46010,           2.79643,

          -0.45793,           0.96707,          -0.31226,           0.51911,
           0.04071,           0.39399,

           0.00038,           0.03854,

           0.22446,           0.13630,          -0.04357,           0.03635,

           0.00202,          -0.04502,

          -0.00458,          -0.03884,

           1.32597,           3.40849,          -1.67839,          -0.95411,

          -1.00116,          -0.72744,          -0.22484,          -0.27682,

          -0.18069,           0.00405,

          -0.01000,           0.27523,

          -0.07038,          -0.01051,

          -0.09064,           0.08518,

           0.02083,          -0.25406,

           0.17745,          -0.00944,

           0.21326,           0.20454,

          18.84894,          -7.64400,           0.62670,         -11.02728,
           8.91329,          20.67190,

           0.17757,          -0.15471,

          -0.11385,          -0.46057,

           6.23014,         -14.46025,           2.30012,          -2.22677,

           5.16823,          -1.64235,

        -274.58413,         833.33247,        -191.26241,         269.90157,
         -17.25965,           9.11368,

        -261.65136,      -18274.45858,       -2553.83872,      -10039.10490,
        -508.52567,         336.18172,          14.88587,         421.35954,
         162.43462,         544.92580,

          -0.44246,           0.23216,

          -0.29024,          -0.13057,

          -1.58438,           0.34032,          -0.31604,          -0.01166,

          -0.07112,           0.05721,

          -0.10813,           0.01064,

          -0.05413,           0.06705,

          -0.41582,          -0.47725,           0.31031,           0.08605,

           0.00409,           0.02373,

           0.08092,           0.06247,          -0.01026,           0.05863,

          -0.00238,           0.02948,

           0.00117,           0.02714,

           0.01720,           0.18261,

          -0.04067,           0.88639,

          -0.15502,          -0.96383,

          -0.05307,          -0.17319,

          -0.00486,          -0.02373,

          -0.14748,          -0.11884,           0.07798,          -0.00358,

           0.01104,           0.00805,

           0.15099,          -0.03453,           0.01846,           0.03459,

           0.02197,           0.07012,

          -0.43677,          -1.87445,           1.35202,           2.28294,

          -0.03592,           0.07679,

           0.16427,           0.03014,           0.02472,           0.05549,

          -0.04985,           0.05874,

           0.35361,           0.01144,          -0.57400,           1.34898,

           0.00265,           0.01540,

           0.00951,           0.08159,

          -0.00435,           0.34759,

          -0.12413,          -0.49848,

          -0.77075,          -2.73810,

         -31.77702,          12.16042,         -14.87605,          11.98287,
          12.69358,           1.31307,          -8.22911,         -21.47437,

          -0.24051,          -0.38332,

          -0.01162,          -0.03175,

           0.00556,           0.02454,

          -0.02297,          -0.01654,

           0.00707,           0.04828,

          -0.00309,           0.17381,

          -0.00500,          -0.07579,

           0.02008,           0.05356,

           0.00702,           0.01133,

          -0.00237,          -0.00612,

           0.18551,           0.22799,          -0.14194,          -0.08593,

           0.00002,          -0.01049,

          -0.17363,          -0.13986,           0.00078,          -0.06993,

          -0.00430,          -0.07795,

          -0.03232,          -4.13170,

           0.00311,           0.05356,

          -0.17324,          -0.15505,          -0.00590,          -0.06608,

           0.04257,          -0.04571,

           0.00501,           0.02141,

          -0.00037,           0.07845,

          -0.00381,          -0.03417,

           0.01834,           0.03349,

           0.07994,           0.15297,

          -0.82299,           0.24672,           0.51764,           0.96379,

           0.01729,           0.02489,

          -0.08581,           0.13252,

           0.00538,           0.01995,

          -0.00148,          -0.02261,

           0.00534,           0.01565,

          -0.07518,          -0.28114,           0.22386,           0.39023,

          -0.00864,           0.00964,

          -0.01923,          -0.02426,

          -0.00112,           0.00923,

          -0.00685,           0.02450,

           0.26733,          -0.99972,          -0.82005,           0.13725,

           0.01520,          -0.00790,

           0.00358,           0.00751,

          -0.00648,          -0.00605,

          -0.04966,          -0.04633,

           0.06394,          -0.01965,

           0.50185,           0.40553,          -0.25809,           0.28853,
           0.52545,          -3.41675,

          -0.00347,          -0.11848,

           0.02945,          -0.01061,

          -0.04160,          -0.03519,

          -0.03234,          -0.81852,

          -0.02156,          -0.00841,

           0.00029,           0.00020,

          -0.02281,          -0.00364,

           0.04738,          -0.04504,

          -0.19161,           0.37225,           0.05765,           0.11987,

           0.00050,           0.02012,

          -0.03806,           0.39498,

           0.29982,           0.00886,           0.01671,          53.04042,

          -0.04160,          -0.38856,

          -0.00174,          -0.01773,

          -0.47661,          -0.32010,          -0.01088,          -0.16231,

          -0.01584,          -0.00144,

           0.06659,           0.12734,

           0.04884,           0.02236,

           0.00146,           0.06030,

          -0.20660,          -0.03982,           0.15091,           1.24562,

          -0.01303,          -0.22426,

          -0.01518,          -0.03922,

          -0.00043,          -0.00047,

           0.02451,           0.04437,

           0.02380,          -0.00189,

          -0.00640,          -0.07114,

          -0.00320,          -0.02491,

          -0.00829,           0.07284,

           0.02846,          -0.28034,

          -0.00268,           0.00256,

          -0.43420,           0.39645,          -0.31053,           1.25916,

          -0.00371,          -0.00651,

          -0.00096,           0.02762,

          -0.00067,          -0.02503,

          -0.01517,           0.03748,

};
static double tabb[] = {
           0.00000,         107.91527,          83.39404,        -124.29804,

          -7.73277,          -3.99442,

          -0.08328,          -1.74251,

          -9.05659,         -22.88559,          -2.30655,          -4.40259,

        -470.94604,       -3648.43408,         326.28960,       -2972.91303,
         337.37285,        -650.33570,          57.18479,         -18.29130,
           1.13897,           2.70158,

         -13.64388,         -71.88619,           7.36408,         -43.79994,
           6.57463,          -5.81111,          -0.06451,           0.73379,

           0.00574,          -0.01635,

           0.00074,          -0.01496,

          -0.00418,           0.00647,

          -0.00407,           0.00548,

           0.00002,           0.00187,

          -0.00591,           0.00557,

           0.32568,          -0.01574,           0.19347,          -0.01705,
           0.00173,           0.02384,

          -0.00248,          -0.00103,

           0.00227,           0.00146,

           0.00307,          -0.00040,

           0.03886,           0.01987,           0.00546,           0.00345,

           0.00134,          -0.00609,

          -0.01502,          -0.01569,

      -10080.59325,       10806.67752,      -14013.76861,        9928.38683,
       -6540.83480,        2084.91597,       -1093.05006,        -305.34266,
          -9.04558,        -110.32310,           9.26094,          -3.93195,
           0.25552,           0.50327,

         -13.12170,          -4.19317,          -4.50857,          -3.37626,
          -0.26850,          -0.36028,

          -0.00357,           0.05862,

          -0.00828,           0.00926,

          -0.01515,          -0.03687,          -0.00224,          -0.00802,

          -0.00225,          -0.00158,

          -0.00022,          -0.00044,

          -0.00281,           0.00371,

           2.28259,          -4.29888,           1.74622,          -2.13604,
           0.37023,          -0.37022,           0.00886,           0.07081,

           0.01669,           0.00056,

          -0.02020,           0.01586,

       -4255.31929,        5978.03267,       -7264.48027,        1884.12585,
       -2353.93882,       -1593.23001,          17.57205,        -498.54139,
          33.28704,         -13.79498,

      -38416.64883,      -13774.09664,      -32822.03952,       -3983.42726,
       -7538.09822,        1906.66915,        -221.24439,         512.77046,
          32.26101,          12.46483,

      142710.47871,      -96584.83892,      145395.05981,      -86630.96423,
       48202.96749,      -23596.77676,        5286.16967,       -1626.44031,
         -16.53568,          95.15428,         -15.19472,           5.69207,

          -6.72181,           7.28683,

        9515.16142,     -166495.49381,        5588.84271,     -146260.29445,
        2023.55881,      -30687.22422,         243.64741,         971.58076,
         390.73247,        -236.13754,       -2684.56349,         739.81087,

        -597.39429,         474.89313,        -631.69166,         213.04947,
        -204.89515,         -33.09139,         -17.78004,         -22.21866,
           0.61083,          -1.41177,

          -0.00070,          -0.00501,

         -58.24552,          25.27978,         -36.39386,           0.36376,
          -2.21030,          -6.46685,          -0.58473,          -0.09357,
           0.12829,          -0.94855,

           0.00042,           0.00048,

           0.00411,           0.00101,

           0.00249,          -0.00865,

           0.00223,           0.00293,

           0.00041,          -0.00042,

           0.00104,          -0.00086,

           0.00126,          -0.00380,

           0.00906,          -0.02253,

           0.05998,          -0.10318,

           0.00004,          -0.03225,           0.14303,          -0.05273,

           0.32683,           0.09386,

          -0.17053,           0.60847,          -0.06190,           0.28166,
           0.06411,           0.05289,

           0.01138,           0.00128,

          -0.00930,           0.00272,           0.00037,           0.00215,

           0.00004,           0.00050,

           0.00114,          -0.00217,

           0.05358,          -0.06413,          -0.00124,           0.03842,

           0.01006,           0.22479,           0.00412,           0.04040,

           0.01708,           0.02164,

           0.02484,          -0.02463,

          -0.00103,           0.02633,

          -0.01303,          -0.03214,

           0.03613,           0.02205,

          -0.02677,          -0.02522,

          -0.00293,           0.03130,

          -1.87255,          -2.50308,          -1.53715,           0.36859,
          -0.17829,          -1.12095,

          -0.05652,          -0.00786,

          -0.06992,           0.07279,

          -2.95896,           0.55138,          -0.61498,          -0.11008,

          -0.87790,          -0.50965,

         119.73553,         -35.18217,          44.78683,          -4.22438,
           1.95723,           0.58033,

       -4077.02379,        -353.39110,       -2781.63273,         -75.23318,
        -312.50478,         -23.86495,          24.59887,          32.56837,
         120.09593,         -51.00495,

           0.09737,           0.09111,

           0.04799,          -0.05029,

           0.08351,          -0.33726,           0.03158,          -0.06435,

          -0.00523,          -0.01736,

           0.00751,          -0.01757,

          -0.00406,          -0.01198,

           0.16402,          -0.10986,          -0.02024,           0.07205,

          -0.00440,          -0.00072,

          -0.00465,           0.00310,          -0.00121,          -0.00121,

           0.00083,           0.00020,

           0.00140,          -0.00176,

           0.00381,          -0.00731,

          -0.01618,           0.01570,

          -0.10201,           0.05809,

          -0.03359,           0.01024,

          -0.00535,           0.00018,

           0.00024,           0.00509,          -0.00158,          -0.00466,

           0.00009,          -0.00083,

          -0.00700,          -0.00090,          -0.00011,          -0.00079,

           0.00133,          -0.00126,

           0.01416,           0.05553,           0.04283,          -0.06719,

           0.00119,           0.00291,

          -0.00263,           0.01282,          -0.00040,           0.00188,

          -0.00237,           0.00973,

          -0.39533,           0.18773,          -0.79821,          -0.40168,

           0.00151,          -0.00161,

           0.00123,          -0.00516,

          -0.01432,          -0.00293,

          -0.05477,           0.04130,

          -0.48837,           0.18944,

          -0.12552,           9.37098,           1.02045,           5.11382,
           0.72098,          -3.70049,          -5.80982,           3.30105,

          -0.09682,           0.09696,

          -0.00876,           0.00504,

           0.00318,           0.00245,

           0.00563,          -0.00665,

           0.00108,          -0.00233,

          -0.00117,           0.00177,

          -0.00343,           0.00503,

           0.01044,          -0.00651,

           0.00296,          -0.00162,

           0.00037,           0.00028,

          -0.00020,          -0.00786,           0.00029,           0.00836,

           0.00004,           0.00033,

          -0.00309,          -0.00086,          -0.00157,          -0.00086,

          -0.00058,           0.00105,

          -0.04557,           0.01794,

          -0.00122,          -0.00086,

           0.00420,          -0.00285,           0.00118,          -0.00020,

           0.00743,          -0.01217,

           0.00053,          -0.00084,

          -0.00075,           0.00097,

          -0.00107,           0.00314,

           0.00576,          -0.00505,

           0.03624,          -0.02546,

           0.05379,           0.30081,           0.29870,          -0.22106,

           0.00696,          -0.00801,

          -0.03995,          -0.01808,

          -0.00139,           0.00102,

          -0.00059,           0.00138,

           0.00019,          -0.00037,

           0.00274,           0.00658,           0.00672,          -0.01132,

           0.00023,           0.00051,

           0.00031,           0.00090,

          -0.00017,          -0.00001,

           0.00085,           0.00004,

           0.02221,          -0.01977,           0.07498,           0.03025,

          -0.00082,          -0.00022,

          -0.00073,          -0.00028,

          -0.00253,           0.00259,

          -0.01329,           0.01805,

           0.00096,           0.00833,

          -0.11836,           0.04277,          -0.10820,          -0.03018,
           0.34504,           0.09834,

          -0.00538,          -0.00231,

           0.00036,           0.00042,

          -0.00023,           0.00260,

          -0.01137,           0.00036,

           0.01081,          -0.03271,

          -0.00029,          -0.00028,

           0.00018,          -0.00003,

           0.00009,           0.00012,

           0.00127,           0.00343,           0.00100,          -0.00064,

           0.00014,           0.00004,

           0.00150,           0.00069,

          -0.01484,           0.00135,           0.03930,           0.01405,

           0.00064,           0.00029,

           0.00009,           0.00009,

           0.00054,          -0.00048,           0.00019,           0.00005,

          -0.00009,           0.00018,

           0.00192,          -0.00333,

           0.01824,           0.01071,

           0.00107,          -0.00341,

           0.25530,          -0.18414,          -0.84151,          -0.31475,

          -0.00400,          -0.00010,

          -0.00174,           0.00019,

           0.00006,          -0.00079,

           0.00066,          -0.00070,

           0.00599,           0.00330,

          -0.00160,          -0.00013,

          -0.00067,          -0.00006,

          -0.00176,          -0.00111,

           0.00652,           0.00368,

           0.00004,           0.00001,

          -0.00081,           0.00089,           0.00366,           0.00139,

           0.00002,           0.00001,

          -0.01870,          -0.00998,

          -0.00020,          -0.00007,

           0.00005,           0.00003,

};
static double  tabr[] = {
           0.00000,         -53.23277,         -44.70609,         -62.54432,

         -19.15218,           0.10867,

          -1.91911,           1.47517,

          16.51994,           5.00458,           3.88980,           1.55740,

        3598.17109,        1831.07574,        2633.34851,        1775.69482,
         497.10486,         488.77343,           6.03892,          31.08365,
          -2.06585,          -1.12599,

         230.37762,        -113.95449,         162.40244,         -46.57185,
           6.70207,          17.27241,          -0.66092,         -14.42065,

          -0.01044,          -0.00287,

          -0.03894,          -0.01663,

           0.01629,           0.00496,

           0.08411,           0.02855,

           0.01795,          -0.00695,

           0.02426,          -0.03921,

          -0.24495,          -0.77369,          -0.31404,           0.38668,
          -0.05682,          -0.17197,

           0.06145,          -0.00510,

           0.00606,          -0.00886,

          -0.00370,          -0.00588,

           0.02173,          -0.11909,           0.00302,          -0.01796,

          -0.01067,           0.00990,

           0.05283,           0.06517,

       59710.89716,        -491.12783,       58672.38609,       19564.41947,
       10597.99050,       14313.02561,       -2585.52040,         766.78396,
        -138.39893,        -802.43403,         131.35006,         -31.97561,
           7.95978,           8.16075,

          28.72669,          31.72473,           6.45792,          16.50701,
           0.01066,           1.29718,

           0.11565,          -0.13240,

           0.05110,          -0.01543,

          -0.09994,           0.18864,          -0.01330,           0.04148,

           0.03510,          -0.00366,

           0.00604,          -0.00604,

           0.03752,          -0.00256,

          -7.00488,         -21.63748,           1.43064,         -17.10914,
          -0.62987,           0.48719,           0.00697,          -1.22665,

          -0.14435,          -0.00550,

           0.32008,          -0.19855,

      -13976.73731,       -3559.49432,       -7709.90803,       -9310.80334,
         749.31835,       -3491.50696,         540.94979,         -84.57550,
          16.96663,          35.53930,

       37214.64771,      -36361.15845,       21093.74492,      -31855.33076,
        1500.84653,       -7031.97901,        -453.40865,         -18.36692,
          -2.07726,         -17.92336,

      -56348.30507,      378512.71483,     -111444.43340,      370543.95160,
      -61893.70301,      112131.05507,      -11977.44617,        9156.15245,
        -567.61838,        -495.25760,          16.96202,         -44.06279,

           4.24760,         -48.83674,

     -643705.49516,     -131013.09649,     -838580.02217,       67627.11556,
     -288441.70339,      150227.25291,       -2500.57537,       42676.19888,
        7084.60505,        2043.65642,        9639.56835,       -1502.03390,

       -4126.00409,        -828.73564,       -2801.35204,       -2293.77751,
        -209.23365,       -1045.31476,          95.57334,        -102.74623,
           7.19216,           1.89593,

          -0.05661,           0.02166,

         120.38332,        -141.16507,          98.31386,         -40.23448,
          10.84269,          17.57713,           1.69239,           1.45065,
          -0.19626,           2.76108,

          -0.00270,           0.00360,

          -0.02333,          -0.00710,

          -0.01035,           0.02950,

           0.00737,          -0.06311,

          -0.00613,           0.01407,

           0.01377,           0.00879,

          -0.03287,           0.00012,

          -0.21667,           0.01793,

          -1.54865,           0.10953,

           0.54543,           0.12102,          -9.48047,           0.11477,

          -1.34966,           0.23199,

          -1.50834,           0.26567,          -0.64503,           0.10742,
          -0.21452,           0.04428,

          -0.01920,          -0.00906,

          -0.09378,           0.12773,          -0.02787,          -0.03090,

           0.03111,           0.00140,

           0.03771,          -0.01269,

          -1.94794,           1.22823,           0.64183,          -1.11467,

          -0.19301,          -0.27357,           0.05710,          -0.08115,

          -0.07318,           0.00806,

           0.14286,           0.20297,

           0.14920,          -0.07897,

           0.09682,           0.02379,

          -0.13928,           0.01679,

          -0.00774,           0.10060,

           0.24433,           0.16760,

          -2.88905,          -1.61439,           2.83052,          -3.41031,
          36.37048,           3.37867,

           0.29321,           0.09687,

           0.29324,          -0.14651,

           8.11116,           1.79211,           1.36421,           0.88111,

           1.21683,           2.37950,

        -357.76211,         -87.84636,        -117.55745,         -67.18338,
          -5.26029,          -6.27559,

        7509.94562,           3.68942,        4223.62097,       -1041.13557,
         -74.64464,        -251.41613,        -166.22180,          -1.68190,
        -214.55340,          62.79593,

          -0.08250,          -0.15936,

          -0.03830,           0.10857,

           0.21368,           0.50812,           0.00869,           0.09832,

           0.02158,           0.02045,

           0.01407,           0.03591,

           0.03460,           0.01171,

          -0.16400,           0.09751,           0.03521,          -0.12858,

           0.00700,          -0.00524,

           0.01698,          -0.04796,           0.04006,           0.00565,

          -0.02783,          -0.00205,

          -0.02296,           0.00153,

          -0.16139,           0.01514,

          -0.78136,          -0.01546,

           0.40374,          -0.06014,

           0.06212,          -0.01828,

           0.00831,          -0.00173,

           0.06857,          -0.11677,           0.00028,           0.05765,

          -0.00796,           0.00691,

           0.03764,           0.14902,          -0.02653,           0.02122,

          -0.05503,           0.01549,

           1.56630,          -0.35551,          -1.87960,           1.14303,

          -0.06063,          -0.03425,

           0.03367,          -0.11969,           0.04485,          -0.01651,

           0.04647,          -0.02097,

           0.22841,           0.47362,           0.99226,          -0.60660,

          -0.01249,           0.00134,

          -0.07435,           0.00722,

          -0.31796,          -0.00015,

           0.20533,          -0.04398,

           0.93944,          -0.26710,

          -5.60051,          -9.32918,          -5.13538,          -4.05130,
          -0.56529,           4.34112,           7.18308,          -2.66103,

           0.13241,          -0.07999,

           0.01046,          -0.00535,

          -0.04037,          -0.00455,

          -0.00510,           0.00731,

          -0.04576,           0.00513,

          -0.15846,          -0.00236,

           0.04628,          -0.00463,

          -0.01585,           0.00585,

          -0.00213,           0.00283,

           0.00778,          -0.00198,

          -0.17803,           0.18321,           0.07702,          -0.12325,

           0.01091,           0.00349,

           0.14211,          -0.21830,           0.07289,          -0.00994,

           0.07090,          -0.00079,

           4.18441,          -0.07413,

          -0.06247,          -0.00011,

          -0.15453,           0.14499,          -0.06557,          -0.00098,

           0.00290,           0.02921,

          -0.01923,           0.00457,

          -0.07538,          -0.00120,

           0.02263,          -0.00037,

          -0.01061,           0.00591,

          -0.04725,           0.02364,

          -0.07460,          -0.24108,          -0.28310,           0.14643,

          -0.00700,           0.00427,

           0.22963,           0.03713,

          -0.02062,           0.00478,

           0.01434,           0.00095,

          -0.01425,           0.00376,

           0.29611,          -0.08038,          -0.37811,           0.21703,

          -0.00723,          -0.00924,

          -0.02736,           0.01814,

           0.00934,           0.00731,

           0.00613,           0.00686,

          -0.91503,          -0.32009,          -0.15505,           0.79589,

          -0.00555,          -0.01536,

          -0.00698,           0.00480,

           0.00373,          -0.00046,

           0.00715,          -0.00470,

          -0.01970,          -0.05238,

           0.60649,          -0.32669,           0.17790,           0.33383,
          -2.74922,          -0.25827,

          -0.07862,           0.00406,

          -0.00948,          -0.02117,

           0.03127,          -0.04199,

           0.89670,          -0.02413,

           0.01954,           0.03990,

           0.00063,          -0.00071,

          -0.00226,           0.02009,

          -0.04407,          -0.05069,

           0.38230,           0.16101,           0.11893,          -0.06125,

           0.02051,          -0.00046,

           0.39211,           0.03679,

           0.01666,          -0.31336,          53.28735,          -0.01791,

          -0.39414,           0.04181,

          -0.01885,           0.00165,

           0.31349,          -0.47359,           0.16133,          -0.01023,

           0.00007,           0.01758,

          -0.13351,           0.07249,

           0.00977,           0.05445,

           0.11650,          -0.00191,

          -0.09824,           0.40106,           2.41155,          -0.30655,

           0.24975,          -0.01248,

          -0.03688,           0.01097,

           0.00038,          -0.00051,

          -0.04736,           0.02610,

           0.00968,           0.02634,

           0.07918,          -0.00606,

           0.02735,          -0.00320,

          -0.07544,          -0.00468,

           0.19996,          -0.01964,

           0.00201,           0.00267,

           0.39562,           0.43289,           1.24743,           0.31084,

          -0.00666,           0.00377,

           0.05668,           0.00148,

           0.03220,          -0.00026,

           0.03717,           0.01509,

};

static char args[] = {
  0,  3,
  2,  1,  7, -2,  8,  0,
  2,  2,  7, -4,  8,  0,
  2,  3,  7, -6,  8,  1,
  2,  2,  5, -5,  6,  4,
  2,  1,  6, -3,  7,  3,
  3,  1,  6, -1,  7, -4,  8,  0,
  3,  2,  5, -7,  6,  6,  7,  0,
  3,  2,  6, -6,  7,  1,  8,  0,
  3,  2,  6, -7,  7,  3,  8,  0,
  3,  2,  6, -8,  7,  4,  8,  0,
  3,  2,  6, -7,  7,  2,  8,  0,
  2,  2,  6, -6,  7,  2,
  3,  1,  5, -4,  6,  4,  7,  0,
  3,  1,  6, -2,  7, -1,  8,  0,
  3,  1,  6, -3,  7,  1,  8,  0,
  3,  1,  6, -4,  7,  3,  8,  1,
  2,  5,  7, -9,  8,  0,
  2,  4,  7, -7,  8,  0,
  2,  2,  7, -3,  8,  6,
  2,  1,  7, -3,  8,  2,
  2,  2,  7, -5,  8,  0,
  2,  3,  7, -7,  8,  0,
  3,  1,  6, -6,  7,  5,  8,  1,
  3,  1,  6, -5,  7,  3,  8,  0,
  3,  2,  5, -8,  6,  8,  7,  0,
  3,  1,  5, -4,  6,  5,  7,  0,
  2,  2,  6, -5,  7,  3,
  3,  1,  6,  1,  7, -9,  8,  0,
  3,  2,  5, -4,  6, -2,  7,  0,
  2,  1,  6, -4,  8,  4,
  2,  1,  6, -2,  7,  4,
  2,  5,  7, -8,  8,  5,
  2,  3,  7, -4,  8,  0,
  1,  1,  7,  5,
  2,  2,  7, -6,  8,  4,
  3,  1,  6, -6,  7,  4,  8,  0,
  2,  1,  6, -4,  7,  4,
  3,  2,  6, -5,  7,  1,  8,  0,
  3,  2,  6, -6,  7,  3,  8,  0,
  2,  2,  6, -7,  7,  0,
  3,  1,  5, -4,  6,  3,  7,  0,
  3,  1,  6, -1,  7, -1,  8,  0,
  2,  1,  5, -2,  6,  0,
  2,  6,  7, -9,  8,  0,
  2,  5,  7, -7,  8,  0,
  2,  4,  7, -5,  8,  0,
  2,  3,  7, -3,  8,  1,
  2,  2,  7, -1,  8,  0,
  2,  1,  7,  1,  8,  2,
  1,  3,  8,  0,
  2,  3,  6, -7,  7,  1,
  3,  2,  5, -3,  6, -4,  7,  0,
  3,  2,  6, -3,  7, -2,  8,  0,
  2,  2,  6, -4,  7,  1,
  3,  2,  6, -5,  7,  2,  8,  1,
  3,  5,  5, -9,  6, -8,  7,  0,
  3,  2,  5, -4,  6, -1,  7,  0,
  3,  1,  6,  3,  7, -8,  8,  0,
  3,  2,  6, -8,  7,  1,  8,  0,
  3,  2,  5, -7,  6,  4,  7,  0,
  3,  4,  5,-10,  6,  2,  7,  0,
  2,  1,  6, -2,  8,  0,
  2,  1,  6, -1,  7,  2,
  2,  8,  7,-12,  8,  0,
  2,  7,  7,-10,  8,  0,
  2,  6,  7, -8,  8,  1,
  2,  5,  7, -6,  8,  0,
  2,  4,  7, -4,  8,  2,
  1,  2,  7,  4,
  1,  4,  8,  0,
  2,  1,  7, -6,  8,  0,
  2,  2,  7, -8,  8,  1,
  2,  3,  7,-10,  8,  0,
  2,  4,  7,-12,  8,  0,
  3,  1,  6, -6,  7,  2,  8,  0,
  2,  1,  6, -5,  7,  1,
  3,  1,  6, -4,  7, -2,  8,  0,
  3,  1,  5, -4,  6,  2,  7,  1,
  3,  1,  5, -2,  6,  1,  7,  0,
  2,  7,  7, -9,  8,  0,
  2,  6,  7, -7,  8,  0,
  2,  5,  7, -5,  8,  0,
  2,  4,  7, -3,  8,  0,
  2,  3,  7, -1,  8,  0,
  2,  2,  7,  1,  8,  0,
  2,  3,  6, -6,  7,  1,
  3,  3,  6, -7,  7,  2,  8,  0,
  3,  2,  5, -3,  6, -3,  7,  1,
  3,  2,  6, -2,  7, -2,  8,  0,
  2,  2,  6, -3,  7,  1,
  3,  2,  6, -4,  7,  2,  8,  0,
  3,  2,  5, -7,  6,  3,  7,  1,
  3,  1,  6,  1,  7, -2,  8,  0,
  1,  1,  6,  1,
  2,  8,  7,-10,  8,  0,
  2,  7,  7, -8,  8,  0,
  2,  6,  7, -6,  8,  0,
  2,  5,  7, -4,  8,  0,
  2,  4,  7, -2,  8,  0,
  1,  3,  7,  3,
  2,  2,  7,  2,  8,  0,
  2,  1,  7,  4,  8,  0,
  2,  1,  5, -4,  7,  0,
  2,  1,  6, -6,  7,  0,
  2,  8,  7, -9,  8,  0,
  2,  7,  7, -7,  8,  0,
  2,  6,  7, -5,  8,  0,
  2,  5,  7, -3,  8,  0,
  2,  4,  7, -1,  8,  0,
  3,  3,  6, -4,  7, -2,  8,  0,
  2,  3,  6, -5,  7,  1,
  3,  3,  6, -6,  7,  2,  8,  0,
  3,  2,  5, -3,  6, -2,  7,  1,
  3,  2,  6, -1,  7, -2,  8,  0,
  2,  2,  6, -2,  7,  0,
  3,  2,  6, -3,  7,  2,  8,  0,
  3,  2,  5, -7,  6,  2,  7,  1,
  2,  1,  6,  1,  7,  0,
  2,  9,  7,-10,  8,  0,
  2,  8,  7, -8,  8,  0,
  2,  7,  7, -6,  8,  0,
  2,  6,  7, -4,  8,  0,
  2,  5,  7, -2,  8,  0,
  1,  4,  7,  1,
  2,  3,  7,  2,  8,  0,
  2,  1,  5, -3,  7,  0,
  2,  9,  7, -9,  8,  0,
  2,  8,  7, -7,  8,  0,
  3,  3,  6, -3,  7, -2,  8,  0,
  2,  3,  6, -4,  7,  1,
  3,  3,  6, -5,  7,  2,  8,  0,
  3,  2,  5, -3,  6, -1,  7,  0,
  3,  2,  5, -8,  6,  4,  7,  0,
  2,  2,  6, -2,  8,  0,
  2,  2,  6, -1,  7,  1,
  3,  2,  6, -2,  7,  2,  8,  0,
  3,  2,  5, -7,  6,  1,  7,  0,
  2,  6,  7, -2,  8,  0,
  1,  5,  7,  0,
  3,  3,  6, -4,  7,  1,  8,  0,
  2,  1,  5, -2,  7,  2,
  3,  1,  5, -3,  7,  2,  8,  0,
  3,  1,  5, -1,  6,  1,  7,  0,
  2,  4,  6, -6,  7,  0,
  2,  3,  6, -3,  7,  0,
  1,  2,  6,  0,
  3,  2,  5, -4,  6,  3,  7,  0,
  3,  1,  5,  1,  6, -4,  7,  0,
  3,  3,  5, -5,  6, -1,  7,  0,
  1,  6,  7,  1,
  3,  1,  5,  1,  7, -4,  8,  0,
  2,  1,  5, -2,  8,  0,
  2,  1,  5, -1,  7,  1,
  3,  1,  5, -2,  7,  2,  8,  0,
  3,  1,  5, -3,  7,  4,  8,  0,
  3,  1,  5, -5,  6,  1,  7,  1,
  3,  1,  5, -1,  6,  2,  7,  0,
  2,  4,  6, -5,  7,  0,
  2,  3,  6, -2,  7,  0,
  3,  1,  5,  1,  7, -2,  8,  0,
  1,  1,  5,  1,
  2,  4,  6, -4,  7,  0,
  2,  3,  6, -1,  7,  0,
  3,  3,  5, -5,  6,  1,  7,  0,
  2,  5,  6, -6,  7,  0,
  2,  4,  6, -3,  7,  0,
  2,  5,  6, -5,  7,  0,
  2,  6,  6, -6,  7,  0,
  2,  2,  5, -3,  7,  0,
  2,  2,  5, -2,  7,  0,
  2,  2,  5, -2,  8,  0,
  2,  2,  5, -1,  7,  1,
  3,  2,  5, -2,  7,  2,  8,  0,
  1,  2,  5,  0,
  2,  3,  5, -3,  7,  0,
  2,  3,  5, -1,  7,  0,
 -1
};
/* Total terms = 177, small = 171 */
struct plantbl ura404 = {
 9,
  {  0,  0,  0,  0,  5, 10,  9, 12,  0,},
 6,
 args,
 tabl,
 tabb,
 tabr,
 1.9218446061800002e+01,
 3652500.0,
 1.0
};
