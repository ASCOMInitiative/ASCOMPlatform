using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DynamicRemoteClients
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Append a byte array to an existing array
        /// </summary>
        /// <param name="first">First array</param>
        /// <param name="second">Second array</param>
        /// <returns>Concatenated array of byte</returns>
        public static byte[] Append(this byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static string ToConcatenatedString(this List<string> list, string separator)
        {
            string concatenatedList = "";
            foreach (string item in list)
            {
                concatenatedList += item + separator;
            }
            return concatenatedList.Trim(separator.ToCharArray());
        }

        public static void FromConcatenatedString(this List<string> list, string concatenatedString, string separator)
        {
            string[] items = concatenatedString.Split(separator.ToCharArray());

            list.Clear();

            foreach (string item in items)
            {
                list.Add(item);
            }
        }

        public static List<StringValue> ToListStringValue(this List<string> fromList)
        {
            List<StringValue> toList = new List<StringValue>();

            foreach (string item in fromList)
            {
                toList.Add(new StringValue(item));
            }

            return toList;
        }

        public static List<string> ToListString(this List<StringValue> fromList)
        {
            List<string> toList = new List<string>();

            foreach (StringValue item in fromList)
            {
                toList.Add(item.Value);
            }

            return toList;
        }
    }
}
