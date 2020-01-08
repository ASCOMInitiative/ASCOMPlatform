using System;

namespace ASCOM.DynamicRemoteClients
{
    public static class Library
    {
        /// <summary>
        /// Convert 3D 32bit Integer array to double array
        /// </summary>
        /// <param name="inputArray">32bit integer array to be converted</param>
        /// <returns>3D double array containing the integer values</returns>
        public static double[,] Array2DToDouble(Array inputArray)
        {
            double[,] outputArray = new double[inputArray.GetLength(0), inputArray.GetLength(1)];

            for (int j = 0; j <= inputArray.GetUpperBound(1); j++)
            {
                for (int i = 0; i <= inputArray.GetUpperBound(0); i++)
                {
                    outputArray[i, j] = Convert.ToDouble(inputArray.GetValue(i, j));
                }
            }
            return outputArray;
        }

        /// <summary>
        /// Convert 3D array to double array
        /// </summary>
        /// <param name="inputArray">32bit integer array to be converted</param>
        /// <returns>3D double array containing the integer values</returns>
        public static double[,,] Array3DToDouble(Array inputArray)
        {
            double[,,] outputArray = new double[inputArray.GetLength(0), inputArray.GetLength(1), inputArray.GetLength(2)];

            for (int k = 0; k <= inputArray.GetUpperBound(2); k++)
            {
                for (int j = 0; j <= inputArray.GetUpperBound(1); j++)
                {
                    for (int i = 0; i <= inputArray.GetUpperBound(0); i++)
                    {
                        outputArray[i, j, k] = Convert.ToDouble(inputArray.GetValue(i, j, k));
                    }
                }
            }
            return outputArray;
        }

        /// <summary>
        /// Convert 3D 32bit Integer array to double array
        /// </summary>
        /// <param name="inputArray">32bit integer array to be converted</param>
        /// <returns>3D double array containing the integer values</returns>
        public static int[,] Array2DToInt(Array inputArray)
        {
            int[,] outputArray = new int[inputArray.GetLength(0), inputArray.GetLength(1)];

            for (int j = 0; j <= inputArray.GetUpperBound(1); j++)
            {
                for (int i = 0; i <= inputArray.GetUpperBound(0); i++)
                {
                    outputArray[i, j] = Convert.ToInt32(inputArray.GetValue(i, j));
                }
            }
            return outputArray;
        }

        /// <summary>
        /// Convert 3D array to double array
        /// </summary>
        /// <param name="inputArray">32bit integer array to be converted</param>
        /// <returns>3D double array containing the integer values</returns>
        public static int[,,] Array3DToInt(Array inputArray)
        {
            int[,,] outputArray = new int[inputArray.GetLength(0), inputArray.GetLength(1), inputArray.GetLength(2)];

            for (int k = 0; k <= inputArray.GetUpperBound(2); k++)
            {
                for (int j = 0; j <= inputArray.GetUpperBound(1); j++)
                {
                    for (int i = 0; i <= inputArray.GetUpperBound(0); i++)
                    {
                        outputArray[i, j, k] = Convert.ToInt32(inputArray.GetValue(i, j, k));
                    }
                }
            }
            return outputArray;
        }

        /// <summary>
        /// Convert 3D 32bit Integer array to double array
        /// </summary>
        /// <param name="inputArray">32bit integer array to be converted</param>
        /// <returns>3D double array containing the integer values</returns>
        public static short[,] Array2DToShort(Array inputArray)
        {
            short[,] outputArray = new short[inputArray.GetLength(0), inputArray.GetLength(1)];

            for (int j = 0; j <= inputArray.GetUpperBound(1); j++)
            {
                for (int i = 0; i <= inputArray.GetUpperBound(0); i++)
                {
                    outputArray[i, j] = Convert.ToInt16(inputArray.GetValue(i, j));
                }
            }
            return outputArray;
        }

        /// <summary>
        /// Convert 3D array to double array
        /// </summary>
        /// <param name="inputArray">32bit integer array to be converted</param>
        /// <returns>3D double array containing the integer values</returns>
        public static short[,,] Array3DToShort(Array inputArray)
        {
            short[,,] outputArray = new short[inputArray.GetLength(0), inputArray.GetLength(1), inputArray.GetLength(2)];

            for (int k = 0; k <= inputArray.GetUpperBound(2); k++)
            {
                for (int j = 0; j <= inputArray.GetUpperBound(1); j++)
                {
                    for (int i = 0; i <= inputArray.GetUpperBound(0); i++)
                    {
                        outputArray[i, j, k] = Convert.ToInt16(inputArray.GetValue(i, j, k));
                    }
                }
            }
            return outputArray;
        }
    }
}
