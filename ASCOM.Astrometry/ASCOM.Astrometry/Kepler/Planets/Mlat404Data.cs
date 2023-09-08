﻿
namespace ASCOM.Astrometry
{
    static class Mlat404Data
    {
        // /* geocentric moon
        // polar coordinates re mean ecliptic and equinox of date
        // S. L. Moshier
        // December, 1996
        // See mlr404.c for test summary. */

        internal static double[] tabr = new double[] { -1 };
        internal static double[] tabb = new double[] { -1 };
        internal static double[] tabl = new double[] { -3, -4, 4d, -1856, 0d, 8043d, -9, -1082, -1, -310, -1, -522, -330, -1449, -853, 4656d, -66, 7d, -1, 9996928d, -66, 6d, 23d, 183d, 0d, 173d, 0d, -56, 0d, 50d, 0d, -785, 1d, 51d, 0d, -60, 1d, 11843d, 0d, -50754, 0d, 1834d, 1d, -7910, 0d, -48060, 1d, 56d, 0d, 13141d, -1, -56318, 0d, 2541d, -1, -649, -133, 778d, -46, 8d, 1d, 1665737d, -47, 7d, 0d, 65d, 0d, 45d, 0d, -138, 0d, -1005, 0d, -2911, 0d, -47, 0d, 96d, 0d, -394, 2d, 76d, 2d, -17302, 0d, 74337d, 0d, -101, 0d, 58d, 0d, -171, 0d, -77, 0d, -1283, 0d, 2686d, 0d, -55, 0d, 99d, 0d, 55d, 0d, 397d, 0d, 540d, 0d, 626d, -1, -5188, 0d, 10857d, 0d, -216, -2, -123, 0d, 6337d, 2d, 224d, -152, -23472, -29, -74336, 0d, 295775d, -20, 149d, -2, 84d, 9d, 304d, 0d, -3051, -70, -6, -57, 34d, 0d, -638, 0d, -201, -73, 9d, 0d, -100, -101, -8, 0d, -57, 0d, -207, -3, 80d, -45, 45d, -5, 102d, -59, -23, 52d, 201d, -48, 233d, -220, 71d, 4d, 2810d, 0d, 6236541d, -61, 218d, -216, 67d, 51d, 201d, -59, -23, -144, -837, -457, 3029d, -45, 42d, -15, 73d, -6, -169, 0d, 135d, -64, -7, 0d, -16245, 0d, -81, -74, -10, 0d, 702d, 0d, -3013, 0d, -5889, 1d, 141d, 58d, 9598d, 12d, 30443d, 1d, -120946, -1, -84, -2, 11246d, -1, -48391, 0d, 1393d, 0d, 200d, -136, -17, 0d, 558d, -64, -8, 0d, -71, 0d, 317577d, -28, 183d, 1d, 219d, 0d, 421d, 0d, -133, 501d, -139, 3d, 354d, -101, -13, 74d, 7d, 144d, -84, 59d, -2, 1d, 64d, -2931, 12559d, -4641, 2638d, -303, -2058, -13, -100, -123, -79, -19214, 6084d, 1494d, 26993d, 15213d, -82219, 42d, 52d, 48d, -101, -53, -4, 4d, 47d, 58d, -131, 46d, 14d, -21, -6, -1311, -8791, 10198d, -4185, 2815d, 5640d, 167d, 422d, -229, 83d, 3140d, 39d, 1221d, 120d, 96d, -30, -1, 184612405d, 187d, 416d, -226, 81d, -1985, -10083, 9983d, -4464, 2807d, 5643d, -21, -9, 113d, -367, 120d, 580d, -667, 27d, 8d, 66d, -56, -6, 337d, 95d, -87, 3303d, -1, 65d, 68d, -374, 0d, -574, 15d, -94, 0d, -53, 0d, -1303, 0d, -236, 283d, 36d, -1, -54, 269d, -35, 0d, -83, 0d, -52, 0d, 730d, 0d, -3129, 0d, 813d, 0d, -4299, 1d, 59d, -6, 5130d, 1d, 16239d, -1, -64603, 0d, -80, 91d, 12d, 0d, -561, 133d, -17, 0d, 250d, -12, 71d, 0d, 155664d, 82d, -11, 0d, 106d, 0d, -604, 0d, 21862d, 55d, -7, 0d, -1514, 0d, 6501d, 0d, 906d, 0d, -68, 0d, 241d, 0d, 366d, 0d, 70d, 0d, -1382, 0d, 5957d, 0d, 113d, 0d, -51, 0d, -55, 0d, 731d, 0d, -264, 0d, 65788d, 1d, -1504, 0d, 3147d, 0d, 217d, 0d, -4105, 0d, 17658d, 1d, 69d, 0d, -3518, 0d, -1767, -43, -7044, -10, -22304, 0d, 88685d, 3d, 91d, 0d, -485, 0d, -57, -1, 333548d, -24, 172d, 11d, 544d, 1d, -1132, 0d, 353d, 0d, -188, 0d, 53d, 0d, 77d, 158d, -887, 35d, 131d, -54, 13d, 0d, 1994821d, -53, 14d, 36d, 125d, 2d, 56d, 0d, -243, 0d, -364, -2, 1916d, 0d, -8227, 0d, 15700d, -1, -67308, 1d, 66d, 0d, -53686, 1d, 3058d, 1d, -13177, 0d, -72, 0d, -72, 0d, 61d, 0d, 15812d, 0d, 165d, 8d, -96, 318d, 1341d, 803d, -4252, 24d, 193d, 1137d, -226, 310d, 622d, -56, 30d, -3, 10101666d, -56, 30d, 1096d, -225, 300d, 600d, -31, 409d, -1, -507, 0d, -287, 0d, -1869, 0d, 8026d, 1d, 544d, -1, -1133, 0d, 27984d, 0d, -62, 0d, -249, 0d, 187d, 0d, -1096, 1d, 53d, 2d, 12388d, 0d, -53107, 0d, -322, 0d, -94, 0d, 15157d, 0d, -582, 0d, 3291d, 0d, 565d, 0d, 106d, 0d, 112d, 0d, 306d, 0d, 809d, 0d, 130d, 0d, -961, 0d, 4149d, 0d, 174d, 0d, -105, 0d, 2196d, 0d, 59d, 0d, 36737d, -1, -1832, 0d, 3835d, 0d, -139, 0d, 24138d, 0d, 1325d, 1d, 64d, 0d, -361, 0d, -1162, -44, -6320, -10, -20003, 0d, 79588d, 2d, 80d, 0d, -2059, 0d, -304, 0d, 21460d, 0d, -166, 0d, -87, 89d, -493, 32d, 114d, 34d, 510d, 1d, 1172616d, 31d, 113d, -1, 57d, 0d, 214d, 0d, -656, 0d, -646, 0d, 1850d, 0d, -7931, 0d, -6674, 0d, 2944d, 0d, -12641, 0d, 916d, 45d, -255, 16d, 60d, -1, 619116d, 16d, 57d, 0d, -58, 0d, 1045d, 0d, -156, -15, 88d, 0d, -62964, 0d, -126, 0d, 1490d, 0d, -6387, 0d, 119d, 0d, 1338d, 0d, -56, 0d, 204d, 0d, 153d, 0d, 940d, 0d, 251d, 0d, 312d, 0d, 584d, 0d, -786, 0d, 3388d, 0d, -52, 0d, 4733d, 0d, 618d, 0d, 29982d, 0d, 101d, 0d, -174, 0d, -2637, 0d, 11345d, 0d, -284, 0d, -524, 0d, -121, 0d, 1464d, 11d, -60, -1, 151205d, 0d, 139d, 0d, -2448, 0d, -51, 0d, -768, 0d, -638, 0d, 552d, 0d, -2370, 0d, 70d, 0d, 64d, 0d, 57d, 0d, 39840d, 0d, 104d, 0d, -10194, 0d, -635, 0d, 69d, 0d, 113d, 0d, 67d, 0d, 96d, 0d, 367d, 0d, 134d, 0d, 596d, 0d, 63d, 0d, 1622d, 0d, 483d, 0d, 72d, 0d, 11917d, 0d, -63, 0d, 1273d, 0d, -66, 0d, -262, 0d, -97, 0d, 103d, 0d, 15196d, 0d, -1445, 0d, -66, 0d, -55, 0d, -323, 0d, 2632d, 0d, -1179, 0d, 59d, 0d, -56, 0d, 78d, 0d, 65d, 0d, 422d, 0d, 309d, 0d, 2125d, 0d, -66, 0d, 124d, 0d, -57, 0d, 1379d, 0d, -304, 0d, 177d, 0d, -118, 0d, 146d, 0d, 283d, 0d, 119d };










































































































































































































































































































































































        internal static int[] args = new int[] { 0, 1, 3, 1, 10, 1, 12, -1, 11, 1, 4, 2, 10, 2, 12, -1, 13, -1, 11, 0, 5, 2, 10, -1, 13, -1, 11, 3, 2, -3, 3, 0, 5, 2, 10, -1, 13, -1, 11, 2, 3, -2, 5, 0, 2, -1, 13, 1, 14, 1, 5, -1, 13, 1, 11, 4, 3, -8, 4, 3, 5, 0, 2, 1, 13, -1, 11, 0, 5, 1, 13, -1, 11, 4, 3, -8, 4, 3, 5, 0, 5, 2, 10, -1, 13, -1, 11, 2, 3, -3, 5, 0, 4, 1, 10, 1, 12, -2, 13, 1, 11, 0, 4, 1, 13, -1, 11, 1, 2, -1, 3, 0, 5, 2, 10, -1, 13, -1, 11, 2, 2, -2, 3, 0, 3, 1, 10, -2, 13, 1, 11, 0, 4, 1, 13, -1, 11, 1, 3, -1, 5, 0, 4, -1, 13, 1, 11, 1, 2, -1, 3, 0, 3, 1, 12, 1, 13, -1, 11, 1, 4, 2, 10, 1, 12, -1, 13, -1, 11, 1, 2, 1, 10, -1, 11, 0, 4, -1, 13, 1, 11, 1, 3, -1, 5, 0, 3, 1, 12, -1, 13, 1, 11, 1, 3, 2, 10, -3, 13, 1, 11, 0, 3, 2, 12, 1, 13, -1, 11, 0, 3, -2, 10, 1, 13, 1, 14, 0, 6, -2, 10, 1, 13, 1, 11, 4, 3, -8, 4, 3, 5, 0, 3, 2, 10, -1, 13, -1, 11, 0, 6, 2, 10, -1, 13, -1, 11, 4, 3, -8, 4, 3, 5, 0, 4, -1, 13, 1, 11, 2, 3, -2, 5, 0, 4, -1, 13, 1, 11, 3, 2, -3, 3, 0, 3, 1, 10, -1, 12, -1, 11, 0, 3, 2, 12, -1, 13, 1, 11, 0, 3, 2, 10, 1, 13, -3, 11, 0, 5, -2, 10, 1, 13, 1, 11, 1, 2, -1, 3, 0, 4, 2, 10, -1, 12, -3, 13, 1, 11, 0, 3, 3, 10, -2, 13, -1, 11, 0, 5, -2, 10, 1, 13, 1, 11, 1, 3, -1, 5, 0, 4, 2, 10, -1, 12, -1, 13, -1, 11, 1, 2, 3, 10, -3, 11, 0, 5, -2, 10, 1, 13, 1, 11, 2, 2, -2, 3, 0, 4, 2, 10, -1, 12, 1, 13, -3, 11, 0, 3, 4, 10, -3, 13, -1, 11, 0, 4, 2, 10, -2, 12, -1, 13, -1, 11, 1, 3, 4, 10, -1, 13, -3, 11, 0, 4, 2, 10, -3, 12, -1, 13, -1, 11, 0, 3, 4, 10, -1, 12, -3, 11, 0, 3, 2, 10, -3, 12, -1, 11, 0, 4, 4, 10, -1, 12, -2, 13, -1, 11, 0, 2, 4, 10, -3, 11, 0, 3, 2, 10, -2, 12, -1, 11, 1, 4, 3, 10, -1, 12, -1, 13, -1, 11, 0, 4, -2, 10, 1, 11, 2, 3, -2, 5, 0, 3, 4, 10, -2, 13, -1, 11, 0, 4, -2, 10, 1, 11, 2, 2, -2, 3, 0, 3, 2, 10, -1, 12, -1, 11, 2, 3, -2, 10, 1, 12, 1, 14, 0, 4, -2, 10, 1, 11, 2, 3, -2, 4, 0, 4, -2, 10, 1, 11, 1, 3, -1, 5, 0, 3, 3, 10, -1, 13, -1, 11, 0, 4, -2, 10, 1, 11, 3, 2, -4, 3, 0, 4, -2, 10, 1, 11, 1, 3, -2, 5, 0, 4, 2, 10, -1, 12, -2, 13, 1, 11, 0, 4, -2, 10, 1, 11, 1, 2, -1, 3, 0, 2, -1, 10, 1, 2, 0, 3, 2, 10, 2, 13, -3, 11, 0, 4, -2, 10, 1, 11, 2, 2, -3, 3, 0, 3, 2, 12, -2, 13, 1, 11, 0, 4, 1, 10, -1, 12, 1, 13, -1, 11, 0, 3, -2, 10, 1, 11, 1, 5, 0, 4, 2, 10, -1, 11, 1, 3, -2, 4, 0, 3, 2, 10, -2, 11, 1, 14, 0, 4, -2, 10, 1, 11, 8, 2, -13, 3, 0, 5, -2, 10, -1, 13, 1, 11, 18, 2, -16, 3, 0, 5, 2, 10, -1, 11, 4, 3, -8, 4, 3, 5, 1, 2, 2, 10, -1, 11, 1, 5, -2, 10, 1, 11, 4, 3, -8, 4, 3, 5, 1, 5, 2, 10, -1, 13, -1, 11, 18, 2, -16, 3, 0, 4, 2, 10, -1, 11, 8, 2, -13, 3, 0, 2, -2, 10, 1, 14, 1, 4, -2, 10, 1, 11, 1, 3, -2, 4, 0, 3, 2, 10, -1, 11, 1, 5, 0, 2, 2, 12, -1, 11, 0, 4, 3, 10, 1, 12, -1, 13, -1, 11, 0, 4, 2, 10, -1, 11, 2, 2, -3, 3, 0, 3, 2, 10, -2, 13, 1, 11, 0, 4, 2, 10, -1, 11, 1, 2, -1, 3, 0, 3, 1, 10, 1, 2, -2, 3, 0, 3, 1, 12, -2, 13, 1, 11, 1, 3, 1, 10, 1, 13, -1, 11, 0, 4, 2, 10, -1, 11, 1, 3, -1, 5, 0, 3, 2, 10, 1, 12, -1, 11, 2, 3, -2, 10, -1, 12, 1, 14, 0, 2, 1, 12, -1, 11, 1, 3, 1, 10, -1, 13, 1, 11, 0, 4, 2, 10, -1, 11, 2, 2, -2, 3, 0, 3, 1, 10, 2, 2, -3, 3, 0, 4, 2, 10, 1, 12, -2, 13, 1, 11, 0, 3, -1, 10, 1, 2, -2, 3, 0, 3, -1, 11, 1, 2, -1, 3, 0, 2, 2, 13, -1, 11, 0, 2, -2, 13, 1, 14, 0, 4, 2, 10, -1, 11, 2, 3, -2, 5, 0, 4, 2, 10, -1, 11, 3, 2, -3, 3, 0, 4, 2, 10, 2, 12, -2, 13, -1, 11, 0, 3, 1, 10, 1, 3, -2, 5, 0, 4, 1, 10, 1, 12, 1, 13, -1, 11, 0, 3, 1, 10, 3, 2, -4, 3, 0, 3, 1, 10, 1, 3, -1, 5, 0, 3, 1, 10, 1, 3, -2, 6, 0, 3, 1, 10, 2, 3, -2, 4, 0, 4, 1, 10, 1, 12, -1, 13, -1, 11, 0, 3, 2, 10, 2, 12, -1, 11, 2, 4, 1, 10, 1, 3, 2, 5, -5, 6, 1, 1, 1, 14, 2, 3, 1, 10, 8, 2, -12, 3, 1, 5, -2, 10, 1, 13, -1, 11, 20, 2, -21, 3, 0, 5, 2, 10, -2, 13, 1, 11, 2, 3, -3, 5, 0, 3, 1, 10, 1, 3, 1, 6, 0, 4, -1, 13, -1, 11, 26, 2, -29, 3, 0, 3, -1, 11, 8, 2, -13, 3, 0, 4, -1, 13, -1, 11, 18, 2, -16, 3, 2, 4, -1, 13, 1, 11, 10, 2, -3, 3, 1, 1, 1, 11, 3, 4, -1, 13, -1, 11, 10, 2, -3, 3, 1, 4, -1, 13, 1, 11, 18, 2, -16, 3, 2, 3, 1, 11, 8, 2, -13, 3, 0, 2, 1, 10, 2, 4, 0, 4, 2, 10, -1, 11, 5, 2, -6, 3, 1, 5, 2, 10, -2, 13, -1, 11, 2, 3, -3, 5, 0, 5, -2, 10, 1, 13, 1, 11, 20, 2, -21, 3, 0, 3, 1, 10, 1, 3, 1, 5, 0, 2, -2, 11, 1, 14, 0, 5, 2, 10, -2, 13, 1, 11, 2, 3, -2, 5, 0, 3, 1, 10, 5, 2, -7, 3, 0, 4, 1, 10, 1, 12, -1, 13, 1, 11, 0, 3, 1, 10, 2, 2, -2, 3, 0, 4, 2, 10, 2, 12, -2, 13, 1, 11, 0, 2, 2, 13, -3, 11, 0, 4, 2, 10, -1, 11, 4, 2, -4, 3, 0, 3, 1, 10, 4, 2, -5, 3, 0, 3, 1, 10, -3, 13, 1, 11, 0, 2, 1, 10, 1, 2, 0, 3, 1, 11, 1, 2, -1, 3, 0, 4, 2, 10, -1, 11, 3, 3, -3, 5, 0, 3, 1, 12, 2, 13, -1, 11, 1, 4, 2, 10, 1, 12, -2, 13, -1, 11, 0, 3, 1, 10, -1, 13, -1, 11, 0, 3, 1, 11, 1, 3, -1, 5, 0, 2, 1, 12, 1, 11, 2, 4, 2, 10, -1, 11, 5, 2, -5, 3, 0, 3, 1, 10, 5, 2, -6, 3, 0, 3, 2, 10, 1, 12, -3, 11, 0, 3, 1, 10, 2, 2, -1, 3, 0, 3, 2, 10, -4, 13, 1, 11, 0, 3, -2, 10, 2, 13, 1, 14, 0, 3, 2, 10, -2, 13, -1, 11, 0, 3, 1, 10, 3, 2, -2, 3, 0, 4, 1, 10, -1, 12, -1, 13, -1, 11, 0, 2, 2, 12, 1, 11, 0, 2, 2, 10, -3, 11, 0, 3, 1, 10, 4, 2, -3, 3, 0, 4, 2, 10, -1, 12, -2, 13, -1, 11, 1, 3, 2, 10, -1, 12, -3, 11, 0, 3, 4, 10, -4, 13, -1, 11, 0, 4, 2, 10, -2, 12, -2, 13, -1, 11, 0, 4, 4, 10, -2, 12, -1, 13, -1, 11, 0, 3, 6, 10, -3, 13, -1, 11, 0, 4, 4, 10, -1, 12, -1, 13, -1, 11, 1, 4, 2, 10, -3, 12, -1, 13, 1, 11, 0, 3, 5, 10, -2, 13, -1, 11, 0, 3, 4, 10, 1, 13, -3, 11, 0, 4, 2, 10, -2, 12, 1, 13, -1, 11, 0, 3, 3, 10, -1, 12, -1, 11, 0, 3, 4, 10, -1, 13, -1, 11, 0, 4, 2, 10, -2, 12, -1, 13, 1, 11, 1, 3, 4, 10, -3, 13, 1, 11, 0, 4, 2, 10, -1, 12, 1, 13, -1, 11, 1, 5, -2, 10, 1, 13, -1, 11, 2, 2, -2, 3, 0, 2, 3, 10, -1, 11, 0, 4, 4, 10, 1, 12, -1, 13, -1, 11, 0, 4, 2, 10, -1, 12, -1, 13, 1, 11, 2, 5, -2, 10, 1, 13, -1, 11, 1, 3, -1, 5, 0, 3, 3, 10, -2, 13, 1, 11, 0, 5, -2, 10, 1, 13, -1, 11, 1, 2, -1, 3, 0, 3, 2, 10, 1, 13, -1, 11, 0, 3, -2, 10, -1, 13, 1, 14, 0, 3, 2, 12, -1, 13, -1, 11, 1, 3, 3, 10, 1, 12, -1, 11, 0, 3, 1, 10, -1, 12, 1, 11, 0, 4, -1, 13, -1, 11, 3, 2, -3, 3, 0, 4, -1, 13, -1, 11, 2, 3, -2, 5, 0, 3, 2, 10, -1, 13, 1, 14, 0, 4, -2, 10, -1, 11, 18, 2, -16, 3, 0, 6, 2, 10, -1, 13, 1, 11, 4, 3, -8, 4, 3, 5, 0, 3, 2, 10, -1, 13, 1, 11, 0, 6, -2, 10, 1, 13, -1, 11, 4, 3, -8, 4, 3, 5, 0, 5, 2, 10, -2, 13, 1, 11, 18, 2, -16, 3, 0, 4, -2, 10, 1, 13, -2, 11, 1, 14, 0, 3, 1, 12, -3, 13, 1, 11, 0, 3, 1, 10, 2, 13, -1, 11, 0, 4, 2, 10, 1, 12, 1, 13, -1, 11, 1, 3, 1, 12, -1, 13, -1, 11, 1, 4, -1, 13, -1, 11, 1, 3, -1, 5, 0, 2, 1, 10, 1, 11, 0, 4, 2, 10, 1, 12, -1, 13, 1, 11, 1, 3, 1, 12, 1, 13, -3, 11, 0, 4, -1, 13, -1, 11, 1, 2, -1, 3, 0, 5, 2, 10, -1, 13, 1, 11, 2, 2, -2, 3, 0, 2, 3, 13, -1, 11, 0, 4, 1, 10, 1, 12, -2, 13, -1, 11, 0, 4, 2, 10, 2, 12, 1, 13, -1, 11, 0, 2, 1, 13, 1, 14, 1, 5, 2, 10, -1, 13, 1, 11, 2, 3, -3, 5, 0, 4, -2, 13, -1, 11, 18, 2, -16, 3, 1, 5, 1, 13, 1, 11, 4, 3, -8, 4, 3, 5, 0, 2, 1, 13, 1, 11, 0, 5, -1, 13, -1, 11, 4, 3, -8, 4, 3, 5, 0, 3, 1, 11, 18, 2, -16, 3, 1, 3, -1, 13, -2, 11, 1, 14, 0, 5, 2, 10, -1, 13, 1, 11, 2, 3, -2, 5, 0, 5, 2, 10, -1, 13, 1, 11, 3, 2, -3, 3, 0, 3, 1, 10, 1, 12, 1, 11, 1, 4, 2, 10, 2, 12, -1, 13, 1, 11, 1, 2, 1, 13, -3, 11, 0, 4, 1, 13, 1, 11, 1, 2, -1, 3, 0, 3, 1, 12, 3, 13, -1, 11, 0, 4, 2, 10, 1, 12, -3, 13, -1, 11, 0, 3, 1, 10, -2, 13, -1, 11, 0, 4, 1, 13, 1, 11, 1, 3, -1, 5, 0, 3, 1, 12, 1, 13, 1, 11, 1, 2, 1, 10, -3, 11, 0, 3, 1, 12, -1, 13, 3, 11, 0, 3, 2, 10, -3, 13, -1, 11, 0, 3, 2, 12, 1, 13, 1, 11, 0, 3, 2, 10, -1, 13, -3, 11, 0, 4, 2, 10, -1, 12, -3, 13, -1, 11, 0, 4, 2, 10, -1, 12, -1, 13, -3, 11, 0, 4, 6, 10, -1, 12, -2, 13, -1, 11, 0, 3, 4, 10, -2, 12, -1, 11, 0, 3, 6, 10, -2, 13, -1, 11, 0, 4, 4, 10, -2, 12, -2, 13, 1, 11, 0, 3, 4, 10, -1, 12, -1, 11, 1, 3, 2, 10, -3, 12, 1, 11, 0, 3, 5, 10, -1, 13, -1, 11, 0, 4, 4, 10, -1, 12, -2, 13, 1, 11, 0, 4, 2, 10, -2, 12, 2, 13, -1, 11, 0, 2, 4, 10, -1, 11, 0, 3, 2, 10, -2, 12, 1, 11, 1, 4, 3, 10, -1, 12, -1, 13, 1, 11, 0, 3, 4, 10, -2, 13, 1, 11, 0, 4, 2, 10, -1, 12, 2, 13, -1, 11, 0, 4, -2, 10, -1, 11, 2, 2, -2, 3, 0, 3, 3, 10, 1, 13, -1, 11, 0, 3, 4, 10, 1, 12, -1, 11, 0, 3, 2, 10, -1, 12, 1, 11, 2, 4, -2, 10, -1, 11, 1, 3, -1, 5, 0, 3, 3, 10, -1, 13, 1, 11, 0, 4, 4, 10, 1, 12, -2, 13, 1, 11, 0, 3, 2, 10, 2, 13, -1, 11, 0, 3, 2, 12, -2, 13, -1, 11, 0, 4, 1, 10, -1, 12, 1, 13, 1, 11, 0, 2, 2, 10, 1, 14, 0, 5, -2, 10, -1, 13, -1, 11, 18, 2, -16, 3, 0, 2, 2, 10, 1, 11, 1, 5, 2, 10, -1, 13, 1, 11, 18, 2, -16, 3, 0, 3, -2, 10, -2, 11, 1, 14, 0, 4, 3, 10, 1, 12, -1, 13, 1, 11, 0, 3, 2, 10, -2, 13, 3, 11, 0, 4, 2, 10, 1, 12, 2, 13, -1, 11, 0, 3, 1, 12, -2, 13, -1, 11, 1, 3, 1, 10, 1, 13, 1, 11, 0, 3, 2, 10, 1, 12, 1, 11, 1, 2, 4, 13, -1, 11, 0, 2, 2, 13, 1, 14, 0, 4, -3, 13, -1, 11, 18, 2, -16, 3, 0, 2, 2, 13, 1, 11, 0, 4, 1, 13, 1, 11, 18, 2, -16, 3, 0, 4, 2, 10, 1, 11, 2, 3, -2, 5, 0, 4, 1, 10, 1, 12, 1, 13, 1, 11, 0, 3, 2, 10, 2, 12, 1, 11, 0, 2, 2, 11, 1, 14, 0, 1, 3, 11, 0, 3, 1, 10, -3, 13, -1, 11, 0, 3, 1, 12, 2, 13, 1, 11, 1, 2, 1, 12, 3, 11, 0, 3, 2, 10, -4, 13, -1, 11, 0, 3, 2, 12, 2, 13, 1, 11, 0, 3, 2, 10, -2, 13, -3, 11, 0, 4, 6, 10, -1, 12, -1, 13, -1, 11, 0, 3, 6, 10, -1, 13, -1, 11, 0, 4, 4, 10, -2, 12, -1, 13, 1, 11, 0, 3, 6, 10, -3, 13, 1, 11, 0, 4, 4, 10, -1, 12, 1, 13, -1, 11, 0, 4, 4, 10, -1, 12, -1, 13, 1, 11, 1, 3, 5, 10, -2, 13, 1, 11, 0, 3, 4, 10, 1, 13, -1, 11, 0, 4, 2, 10, -2, 12, 1, 13, 1, 11, 0, 3, 4, 10, -1, 13, 1, 11, 0, 4, 2, 10, -1, 12, 3, 13, -1, 11, 0, 4, 4, 10, 1, 12, 1, 13, -1, 11, 0, 4, 2, 10, -1, 12, 1, 13, 1, 11, 1, 2, 3, 10, 1, 11, 0, 4, 4, 10, 1, 12, -1, 13, 1, 11, 0, 4, 2, 10, -1, 12, -1, 13, 3, 11, 0, 3, 2, 10, 3, 13, -1, 11, 0, 3, 2, 10, 1, 13, 1, 14, 0, 3, 2, 10, 1, 13, 1, 11, 0, 3, 3, 10, 1, 12, 1, 11, 0, 3, 2, 10, -1, 13, 3, 11, 0, 4, 2, 10, 1, 12, 3, 13, -1, 11, 0, 3, 1, 12, -3, 13, -1, 11, 0, 3, 1, 10, 2, 13, 1, 11, 0, 4, 2, 10, 1, 12, 1, 13, 1, 11, 1, 3, 1, 12, -1, 13, -3, 11, 0, 2, 1, 10, 3, 11, 0, 2, 5, 13, -1, 11, 0, 2, 3, 13, 1, 11, 0, 4, 1, 10, 1, 12, 2, 13, 1, 11, 0, 2, 1, 13, 3, 11, 0, 3, 1, 12, 3, 13, 1, 11, 0, 3, 1, 12, 1, 13, 3, 11, 0, 3, 2, 10, -5, 13, -1, 11, 0, 3, 6, 10, -1, 12, -1, 11, 0, 4, 6, 10, -1, 12, -2, 13, 1, 11, 0, 2, 6, 10, -1, 11, 0, 3, 4, 10, -2, 12, 1, 11, 0, 3, 6, 10, -2, 13, 1, 11, 0, 4, 4, 10, -1, 12, 2, 13, -1, 11, 0, 3, 4, 10, -1, 12, 1, 11, 0, 3, 4, 10, 2, 13, -1, 11, 0, 4, 2, 10, -2, 12, 2, 13, 1, 11, 0, 2, 4, 10, 1, 11, 0, 3, 4, 10, -2, 13, 3, 11, 0, 4, 2, 10, -1, 12, 2, 13, 1, 11, 0, 3, 3, 10, 1, 13, 1, 11, 0, 3, 4, 10, 1, 12, 1, 11, 0, 3, 2, 10, -1, 12, 3, 11, 0, 3, 2, 10, 4, 13, -1, 11, 0, 3, 2, 10, 2, 13, 1, 11, 0, 2, 2, 10, 3, 11, 0, 3, 1, 12, -4, 13, -1, 11, 0, 3, 1, 10, 3, 13, 1, 11, 0, 4, 2, 10, 1, 12, 2, 13, 1, 11, 0, 2, 4, 13, 1, 11, 0, 2, 2, 13, 3, 11, 0, 1, 5, 11, 0, 3, 1, 12, 4, 13, 1, 11, 0, 4, 6, 10, -1, 12, -1, 13, 1, 11, 0, 3, 6, 10, 1, 13, -1, 11, 0, 3, 6, 10, -1, 13, 1, 11, 0, 4, 4, 10, -1, 12, 1, 13, 1, 11, 0, 3, 4, 10, 1, 13, 1, 11, 0, 3, 4, 10, -1, 13, 3, 11, 0, 4, 2, 10, -1, 12, 3, 13, 1, 11, 0, 4, 4, 10, 1, 12, 1, 13, 1, 11, 0, 3, 2, 10, 3, 13, 1, 11, 0, 3, 2, 10, 1, 13, 3, 11, 0, 2, 5, 13, 1, 11, 0, 2, 3, 13, 3, 11, 0, 2, 6, 10, 1, 11, 0, 3, 4, 10, 2, 13, 1, 11, 0, 3, 2, 10, 4, 13, 1, 11, 0, -1 };










































































































































































































































































































































































        internal static KeplerGlobalCode.plantbl moonlat = new KeplerGlobalCode.plantbl(14, new int[19] { 0, 26, 29, 8, 3, 5, 0, 0, 0, 6, 5, 3, 5, 1, 0, 0, 0, 0, 0 }, 3, args, tabl, tabb, tabr, 0.0d, 3652500.0d, 0.0001d);










    }
}