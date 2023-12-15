using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
#if NETSTANDARD2_0
using ASCOM.Tools;
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

#if NETSTANDARD2_0
        TraceLogger logger = new TraceLogger("StateValueCollection", true);
#else
        TraceLogger logger = new TraceLogger("StateValueCollection");
#endif

        #region Initialisers

        /// <summary>
        /// 
        /// </summary>
        public StateValueCollection()
        {
            logger.LogMessage("Init", $"No parameters");

            stateValues = new IStateValue[0];
            logger.LogMessage("Init", $"Count: {stateValues.Length}");
        }

        /// <summary>
        /// 
        /// </summary>
        public StateValueCollection(List<IStateValue> list)
        {
            logger.LogMessage("Init", $"List<IStateValue>");
            stateValues = list.ToArray<IStateValue>();
            logger.LogMessage("Init", $"Count: {stateValues.Length}");
        }

        /// <summary>
        /// 
        /// </summary>
        public StateValueCollection(List<StateValue> list)
        {
            logger.LogMessage("Init", $"List<StateValue>");
            IStateValue[] stateValues = new StateValue[list.Count];

            int index = -1;
            foreach (StateValue stateValue in list)
            {
                stateValues[index++] = stateValue;
            }

            logger.LogMessage("Init", $"Count: {stateValues.Length}");
        }

        #endregion

        #region Add members to the collection

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [ComVisible(false)]
        public void Add(IStateValue value)
        {
            Array.Resize(ref stateValues, stateValues.Length + 1);
            stateValues[stateValues.Length - 1] = value;
            logger.LogMessage("Add IStateValue", $"Count: {stateValues.Length}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, object value)
        {
            StateValue stateValue = new StateValue(name, value);
            Array.Resize(ref stateValues, stateValues.Length + 1);
            stateValues[stateValues.Length - 1] = stateValue;
            logger.LogMessage("Add Name, Value", $"Count: {stateValues.Length}");
        }

        #endregion

        #region IStateValueCollection Members

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                logger.LogMessage("Count", $"Returning count: {stateValues.Length}");
                return stateValues.Length;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            logger.LogMessage("GetEnumerator", $"Count: {stateValues.Length}");
            return new StateValueCollectionEnumerator(stateValues, logger);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidValueException"></exception>
        public IStateValue this[int index]
        {
            get
            {
                logger.LogMessage("This", $"Index: {index}, Count: {stateValues.Length}");

                if ((index < 0) || (index >= stateValues.Length))
                    throw new InvalidValueException("StateValueCollection.Index", index.ToString(CultureInfo.CurrentCulture), $"1 to {stateValues.Length.ToString(CultureInfo.CurrentCulture)}");
                try
                {
                    logger.LogMessage("This", $"Returning index: {index}");

                    logger.LogMessage("This", $"StateValues is null: {stateValues is null}");

                    logger.LogMessage("This", $"Returning: {stateValues[index].Name}");
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
        /// 
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
                logger.LogMessage("Collection.Dispose", $"Dispose called - Disposing: {disposing}");
            }
        }

        #endregion

    }
}

