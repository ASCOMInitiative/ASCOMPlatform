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
    /// <remarks>
    /// <para>See <conceptualLink target="320982e4-105d-46d8-b5f9-efce3f4dafd4"/> for further information on how to implement DeviceState, which properties to include, and the implementation support provided by the Platform.</para>
    /// </remarks>
    [Guid("5F55B067-21B3-4D44-A46C-11DC5EBA2931")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class StateValueCollection : IStateValueCollection, IEnumerable, IDisposable
    {
        // Array to hold the state values
        private IStateValue[] stateValues;

        #region Initialisers

        /// <summary>
        /// Create an empty state value collection
        /// </summary>
        public StateValueCollection()
        {
            stateValues = new IStateValue[0];
        }

        /// <summary>
        /// Create a state value collection populated with values a list of objects that implement IStateValue
        /// </summary>
        /// <param name="stateValueList">List of objects that implement IStateValue.</param>
        public StateValueCollection(List<IStateValue> stateValueList)
        {
            stateValues = stateValueList.ToArray<IStateValue>();
        }

        /// <summary>
        /// Create a state value collection populated with values a list of StateValue objects
        /// </summary>
        /// <param name="stateValueList">List of StateValue objects.</param>
        public StateValueCollection(List<StateValue> stateValueList)
        {
            stateValues = new StateValue[stateValueList.Count];

            int index = -1;
            foreach (StateValue stateValue in stateValueList)
            {
                index++;
                stateValues[index] = stateValue;
            }
        }

        /// <summary>
        /// Create a state value collection populated with values from an array of StateValue objects
        /// </summary>
        /// <param name="stateValueArray">Array of StateValue objects.</param>
        public StateValueCollection(StateValue[] stateValueArray)
        {
            stateValues = stateValueArray;
        }

        #endregion

        #region Add members to the collection

        /// <summary>
        /// Add an object that implements IStateValue to the state value collection.
        /// </summary>
        /// <param name="stateValue">An object that implements the IStateValue interface</param>
        [ComVisible(false)]
        public void Add(IStateValue stateValue)
        {
            Array.Resize(ref stateValues, stateValues.Length + 1);
            stateValues[stateValues.Length - 1] = stateValue;
        }

        /// <summary>
        /// Add a new state value with the given name and value
        /// </summary>
        /// <param name="stateName">The name of the state value</param>
        /// <param name="stateValue">The state's value</param>
        public void Add(string stateName, object stateValue)
        {
            StateValue value = new StateValue(stateName, stateValue);
            Array.Resize(ref stateValues, stateValues.Length + 1);
            stateValues[stateValues.Length - 1] = value;
        }

        /// <summary>
        /// Add the local date and time to the state value collection.
        /// </summary>
        public void AddLocalDateTime()
        {
            StateValue currentDateTime = new StateValue(DateTime.Now);
            Array.Resize(ref stateValues, stateValues.Length + 1);
            stateValues[stateValues.Length - 1] = currentDateTime;
        }

        /// <summary>
        /// Add the UTC date and time to the state value collection.
        /// </summary>
        public void AddUtcDateTime()
        {
            StateValue currentDateTime = new StateValue(DateTime.UtcNow);
            Array.Resize(ref stateValues, stateValues.Length + 1);
            stateValues[stateValues.Length - 1] = currentDateTime;
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
                return stateValues.Length;
            }
        }

        /// <summary>
        /// An enumerator that enables items in the collection to be retrieved
        /// </summary>
        /// <returns>An enumerator object.</returns>
        public IEnumerator GetEnumerator()
        {
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
                if ((index < 0) || (index >= stateValues.Length))
                    throw new InvalidValueException("StateValueCollection.Index", index.ToString(CultureInfo.CurrentCulture), $"1 to {stateValues.Length.ToString(CultureInfo.CurrentCulture)}");

                try
                {
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
        /// Dispose of the state value collection.
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
            stateValues= null;
        }

        #endregion

    }
}