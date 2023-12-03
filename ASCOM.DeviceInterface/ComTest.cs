using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// COM visible ArrayList class for use by .NET 5, 6, 7, 8 and later
    /// </summary>
    [Serializable]
    [Guid("F71A68C5-4130-48E6-90CD-0CD2D977D4D1")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class ComTest : IComTest
    {
        Dictionary<string, string> list = new Dictionary<string, string> { { "Default1", "Value1" }, { "Default2", "Value2" } };

        /// <summary>
        /// Create a ComTest instance
        /// </summary>
        public ComTest()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int NumberOfItems() => list.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value)
        {
            list.Add(name, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (KeyValuePair<string, string> value in list)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetValues()
        {
            string retVal = "";
            foreach (KeyValuePair<string, string> item in list)
            {
                retVal += $"{item.Key} = {item.Value}\r\n";
            }

            return retVal;
        }
    }
}
