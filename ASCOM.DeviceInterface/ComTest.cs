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
        ArrayList list = new ArrayList { new StateValue("Default 1","Value 1"), new StateValue("Default 2","Value 2")};

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
        public void Add(StateValue stateValue)
        {
            list.Add(stateValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (StateValue value in list)
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
            foreach (StateValue item in list)
            {
                retVal += $"{item.Name} = {item.Value}\r\n";
            }

            return retVal;
        }
    }
}
