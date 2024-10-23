//-----------------------------------------------------------------------
// <summary>Defines the Switch class.</summary>
//-----------------------------------------------------------------------
// 17-Sep-13  	cdr     6.0.0   Initial definition
using System;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;

namespace ASCOM.DriverAccess
{
    /// <summary>
    /// Defines the ISwitchV2 Interface
    /// </summary>
    /// <remarks>
    /// <para>The Switch interface is used to define a number of 'switch devices'. A switch device can be used to control something, such as a power switch
    /// or may be used to sense the state of something, such as a limit switch.</para>
    /// <para>This SwitchV2 interface is an extension of the original Switch interface.  The changes allow devices to have more than two states and
    /// to distinguish between devices that are writeable and those that are read only.</para>
    /// <para><b>Compatibility between Switch and SwitchV2 interfaces:</b></para>
    /// <list type="bullet"><item>Switch devices that implemented the original Switch interface and
    /// client applications that use the original interface will still work together.</item>
    /// <item>Client applications that implement the original
    /// Switch interface should still work with drivers that implement the new interface.</item>
    /// <item>Client applications that use the new features in this interface
    /// will not work with drivers that do not implement the new interface.</item>
    /// </list>
    /// <para>Each device has a CanWrite method, this is true if it can be written to or false if the device can only be read.</para>
    /// <para>The new MinSwitchValue, MaxSwitchValue and SwitchStep methods are used to define the range and values that a device can handle.
    /// This also defines the number of different values - states - that a device can have, from two for a traditional on-off switch, through
    /// those with a small number of states to those which have many states.</para>
    /// <para>The SetSwitchValue and GetSwitchValue methods are used to set and get the value of a device as a double.</para>
    /// <para>There is no fundamental difference between devices with different numbers of states.</para>
    /// <para><b>Naming Conventions</b></para>
    /// <para>Each device handled by a Switch is known as a device or switch device for general cases,
    /// a controller device if it can alter the state of the device and a sensor device if it can only be read.</para>
    /// <para>For convenience devices are referred to as Boolean if the device can only have two states, and multi-state if it can have more than two values.
    /// <b>These are treated the same in the interface definition</b>.</para>
    /// </remarks>
    public class Switch : AscomDriver, ISwitchV2, ISwitchV3
    {
        private MemberFactory memberFactory;

        #region Switch constructors

        /// <summary>
        /// Creates a Switch object with the given Prog ID
        /// </summary>
        /// <param name="switchId">ProgID of the Switch device to be accessed.</param>
        public Switch(string switchId)
            : base(switchId)
        {
            memberFactory = base.MemberFactory; //Get the member factory created by the base class
        }

        #region IDisposable Members
        // No member here, we are relying on Dispose in the base class
        #endregion

        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialogue to choose a Switch
        /// </summary>
        /// <param name="switchId">FilterWheel Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen Switch or null for none</returns>
        public static string Choose(string switchId)
        {
            using (Chooser chooser = new Chooser())
            {
                chooser.DeviceType = "Switch";
                return chooser.Choose(switchId);
            }
        }

        // There is no SwitchState support because the interface would have to be variable to incorporate an arbitrary number of switches

        #endregion

        #region ISwitchV2 members

        /// <inheritdoc/>
        public short MaxSwitch
        {
            get { return Convert.ToInt16(memberFactory.CallMember(1, "MaxSwitch", new Type[] { }, new object[] { })); }
        }

        /// <inheritdoc/>
        public string GetSwitchName(short id)
        {
            return (string)memberFactory.CallMember(3, "GetSwitchName", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <inheritdoc/>
        public void SetSwitchName(short id, string name)
        {
            memberFactory.CallMember(3, "SetSwitchName", new Type[] { typeof(short), typeof(string) }, new object[] { id, name });
        }

        /// <inheritdoc/>
        public string GetSwitchDescription(short id)
        {
            return (string)memberFactory.CallMember(3, "GetSwitchDescription", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <inheritdoc/>
        public bool CanWrite(short id)
        {
            return (bool)memberFactory.CallMember(3, "CanWrite", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <inheritdoc/>
        public bool GetSwitch(short id)
        {
            return (bool)memberFactory.CallMember(3, "GetSwitch", new Type[] { typeof(short) }, new object[] { id });
        }

        /// <inheritdoc/>
        public void SetSwitch(short id, bool state)
        {
            memberFactory.CallMember(3, "SetSwitch", new Type[] { typeof(short), typeof(bool) }, new object[] { id, state });
        }

        /// <inheritdoc/>
        public double MaxSwitchValue(short id)
        {
            try { return (double)memberFactory.CallMember(3, "MaxSwitchValue", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <inheritdoc/>
        public double MinSwitchValue(short id)
        {
            try { return (double)memberFactory.CallMember(3, "MinSwitchValue", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 0.0; }
        }

        /// <inheritdoc/>
        public double SwitchStep(short id)
        {
            try { return (double)memberFactory.CallMember(3, "SwitchStep", new Type[] { typeof(short) }, new object[] { id }); }
            catch (System.NotImplementedException) { return 1.0; }
        }

        /// <inheritdoc/>
        public double GetSwitchValue(short id)
        {
            try
            {
                return (double)memberFactory.CallMember(3, "GetSwitchValue", new Type[] { typeof(short) }, new object[] { id });
            }
            catch (System.NotImplementedException) { return this.GetSwitch(id) ? 1.0 : 0.0; }
        }

        /// <inheritdoc/>
        public void SetSwitchValue(short id, double value)
        {
            try
            {
                memberFactory.CallMember(3, "SetSwitchValue", new Type[] { typeof(short), typeof(double) }, new object[] { id, value });
            }
            catch (System.NotImplementedException)
            {
                bool bv = value >= 0.5 ? true : false;
                this.SetSwitch(id, bv);
            }
        }

        #endregion

        #region ISwitchV3 members

        /// <inheritdoc/>
        public void SetAsync(short id, bool state)
        {
            // Call the device's SetAsync method if this is a Platform 7 or later device, otherwise throw a MethodNotImplementedException.
            if (HasConnectAndDeviceState) // We are presenting a Platform 7 or later device so call the device's method
            {
                TL.LogMessage("SetAsync", "Issuing SetAsync command");
                memberFactory.CallMember(3, "SetAsync", new Type[] { typeof(short), typeof(bool) }, new object[] { id, state });
                return;
            }

            // Platform 6 or earlier device
            throw new MethodNotImplementedException($"DriverAccess.Switch - SetAsync is not supported by this device because it exposes interface ISwitchV{DriverInterfaceVersion}.");
        }

        /// <inheritdoc/>
        public void SetAsyncValue(short id, double value)
        {
            // Call the device's SetAsyncValue method if this is a Platform 7 or later device, otherwise throw a MethodNotImplementedException.
            if (HasConnectAndDeviceState) // We are presenting a Platform 7 or later device so call the device's method
            {
                TL.LogMessage("SetAsyncValue", "Issuing SetAsyncValue command");
                memberFactory.CallMember(3, "SetAsyncValue", new Type[] { typeof(short), typeof(bool) }, new object[] { id, value });
                return;
            }

            // Platform 6 or earlier device - method not implemented
            throw new MethodNotImplementedException($"DriverAccess - SetAsyncValue is not supported by this device because it exposes interface ISwitchV{DriverInterfaceVersion}.");
        }

        /// <inheritdoc/>
        public bool CanAsync(short id)
        {
            // Call the device's SetAsyncValue method if this is a Platform 7 or later device, otherwise return false to indicate no async capability.
            if (HasConnectAndDeviceState) // We are presenting a Platform 7 or later device so call the device's method
            {
                TL.LogMessage("CanAsync", "Issuing CanAsync command");
                return (bool)memberFactory.CallMember(3, "CanAsync", new Type[] { typeof(short) }, new object[] { id });
            }

            // Platform 6 or earlier device - async is not supported so return false to show no async support.
            return false;
        }

        /// <inheritdoc/>
        public bool StateChangeComplete(short id)
        {
            // Call the device's StateChangeComplete method if this is a Platform 7 or later device, otherwise throw a MethodNotImplementedException.
            if (HasConnectAndDeviceState) // We are presenting a Platform 7 or later device so call the device's method
            {
                TL.LogMessage("StateChangeComplete", "Issuing StateChangeComplete command");
                return (bool)memberFactory.CallMember(3, "StateChangeComplete", new Type[] { typeof(short) }, new object[] { id });
            }

            // Platform 6 or earlier device
            throw new MethodNotImplementedException($"DriverAccess - StateChangeComplete is not supported by this device because it exposes interface ISwitchV{DriverInterfaceVersion}.");
        }

        /// <inheritdoc/>
        public void CancelAsync(short id)
        {
            // Call the device's CancelAsync method if this is a Platform 7 or later device, otherwise throw a MethodNotImplementedException.
            if (HasConnectAndDeviceState) // We are presenting a Platform 7 or later device so call the device's method
            {
                TL.LogMessage("CancelAsync", "Issuing CancelAsync command");
                memberFactory.CallMember(3, "CancelAsync", new Type[] { typeof(short) }, new object[] { id });
                return;
            }

            // Platform 6 or earlier device
            throw new MethodNotImplementedException($"DriverAccess - CancelAsync is not supported by this device because it exposes interface ISwitchV{DriverInterfaceVersion}.");
        }

        #endregion
    }
}
