using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
#if NETSTANDARD2_0
//using ASCOM.Tools;
#else
using ASCOM.Utilities;
#endif
namespace ASCOM.DeviceInterface
{
    /// <summary>
    /// StateValueCollection is a strongly-typed collection that is enumerable by both COM and .NET. The IStateValueCollection, IEnumerator and IEnumerable interfaces provide this polymorphism. 
    /// </summary>
    [Guid("5F55B067-21B3-4D44-A46C-11DC5EBA2931")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class StateValueCollection : IStateValueCollection, IEnumerable, IDisposable
    {
        private IStateValue[] stateValues;

//#if NETSTANDARD2_0
//        TraceLogger logger = new TraceLogger("StateValueCollection", true);
//#else
//        TraceLogger logger = new TraceLogger("StateValueCollection");
//#endif

        #region Initialisers

        /// <summary>
        /// Create an empty state value collection
        /// </summary>
        public StateValueCollection()
        {
            //logger.LogMessage("Init", $"No parameters");

            stateValues = new IStateValue[0];
            //logger.LogMessage("Init", $"Count: {stateValues.Length}");
        }

        /// <summary>
        /// Create a state value collection populated with values a list of objects that implement IStateValue
        /// </summary>
        /// <param name="list">List of objects that implement IStateValue.</param>
        public StateValueCollection(List<IStateValue> list)
        {
            //logger.LogMessage("Init", $"List<IStateValue>");
            stateValues = list.ToArray<IStateValue>();
            //logger.LogMessage("Init", $"Count: {stateValues.Length}");
        }

        /// <summary>
        /// Create a state value collection populated with values a list of StateValue objects
        /// </summary>
        /// <param name="list">List of StateValue objects.</param>
        public StateValueCollection(List<StateValue> list)
        {
            //logger.LogMessage("Init", $"List<StateValue>");
            stateValues = new StateValue[list.Count];

            int index = -1;
            foreach (StateValue stateValue in list)
            {
                index++;
                stateValues[index] = stateValue;
            }

            //logger.LogMessage("Init", $"Count: {stateValues.Length}");
        }

        #endregion

        #region Add members to the collection

        /// <summary>
        /// Add an object that implements IStateValue to the state value collection.
        /// </summary>
        /// <param name="value">An object that implements the IStateValue interface</param>
        [ComVisible(false)]
        public void Add(IStateValue value)
        {
            Array.Resize(ref stateValues, stateValues.Length + 1);
            stateValues[stateValues.Length - 1] = value;
            //logger.LogMessage("Add IStateValue", $"Count: {stateValues.Length}");
        }

        /// <summary>
        /// Add a new state value with the given name and value
        /// </summary>
        /// <param name="name">The name of the state value</param>
        /// <param name="value">The state's value</param>
        public void Add(string name, object value)
        {
            StateValue stateValue = new StateValue(name, value);
            Array.Resize(ref stateValues, stateValues.Length + 1);
            stateValues[stateValues.Length - 1] = stateValue;
            //logger.LogMessage("Add Name, Value", $"Count: {stateValues.Length}");
        }

        #endregion

        #region IStateValueCollection Members

        /// <summary>
        /// The number of state values in the collection
        /// </summary>
        public int Count
        {
            get
            {
                //logger.LogMessage("Count", $"Returning count: {stateValues.Length}");
                return stateValues.Length;
            }
        }

        /// <summary>
        /// An enumerator that enables items in the collection to be retrieved
        /// </summary>
        /// <returns>An enumerator object.</returns>
        public IEnumerator GetEnumerator()
        {
            //logger.LogMessage("GetEnumerator", $"Count: {stateValues.Length}");
            //return new StateValueCollectionEnumerator(stateValues, logger);
            return new StateValueCollectionEnumerator(stateValues);
        }

        /// <summary>
        /// Returns the state value as an IStateValue object at the given index into the collection.
        /// </summary>
        /// <param name="index">The zero based item number in the collection to return.</param>
        /// <returns></returns>
        /// <exception cref="InvalidValueException"></exception>
        public IStateValue this[int index]
        {
            get
            {
                //logger.LogMessage("This", $"Index: {index}, Count: {stateValues.Length}");

                if ((index < 0) || (index >= stateValues.Length))
                    throw new InvalidValueException("StateValueCollection.Index", index.ToString(CultureInfo.CurrentCulture), $"1 to {stateValues.Length.ToString(CultureInfo.CurrentCulture)}");
                try
                {
                    //logger.LogMessage("This", $"Returning index: {index}");

                    //logger.LogMessage("This", $"StateValues is null: {stateValues is null}");

                    //logger.LogMessage("This", $"Returning: {stateValues[index].Name}");
                    return stateValues[index];

                }
                catch (Exception ex)
                {
                    return new StateValue("Exception:", ex.ToString());
                }
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disopse of the state value collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //logger.LogMessage("Collection.Dispose", $"Dispose called - Disposing: {disposing}");
            }
        }

        #endregion

    }
}