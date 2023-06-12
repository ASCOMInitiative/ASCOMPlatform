using System.Runtime.InteropServices;

namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// 
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("109DB703-2B74-424A-89AC-EAA94CFDD86D")]
    [ComVisible(true)]
    public class StateValue : IStateValue
    {
        /// <summary>
        /// 
        /// </summary>
        public StateValue() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        public StateValue(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// State name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// State value
        /// </summary>
        public object Value { get; set; }

    }

}
