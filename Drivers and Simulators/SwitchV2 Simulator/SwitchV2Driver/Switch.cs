﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.Utilities;
using System.Globalization;

namespace ASCOM.Simulator
{
    /// <summary>
    /// Local switch class
    /// </summary>
    public class LocalSwitch
    {
        private double max;
        private double min;
        private double step;

        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double StepSize { get; set; }
        public string Name { get; set; }
        public bool ReadOnly { get; set; }
        public double Value { get; set; }
        
        public LocalSwitch()
        {
            this.Maximum = 1.0;
            this.StepSize = 1.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalSwitch"/> class.
        /// default values are for a binary switch.
        /// </summary>
        /// <param name="name">The name.</param>
        internal LocalSwitch(string name)
            : this(name, 1, 0, 1, 0)
	    { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalSwitch"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="stepSize">Size of the step.</param>
        /// <param name="value">The value.</param>
        internal LocalSwitch(string name, double maximum, double minimum, double stepSize, double value) : this(name, maximum, minimum, stepSize, value, false)
        {  }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalSwitch"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="max">The max.</param>
        /// <param name="min">The min.</param>
        /// <param name="step">The step.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <param name="value">The value.</param>
        public LocalSwitch(string name, double max, double min, double step, double value, bool readOnly)
        {
            this.Name = name;
            this.max = max;
            this.min = min;
            this.step = step;
            this.ReadOnly = readOnly;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalSwitch"/> class using
        /// data from the ASCOM profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="driverId">The driver id.</param>
        /// <param name="id">The id.</param>
        internal LocalSwitch(Profile profile, string driverId, int id)
        {
            var subKey = "Switch" + id.ToString();
            this.Name = profile.GetValue(driverId, "Name", subKey, subKey);
            this.Minimum = Convert.ToDouble(profile.GetValue(driverId, "Minimum", subKey, "0"), CultureInfo.InvariantCulture);
            this.Maximum = Convert.ToDouble(profile.GetValue(driverId, "Maximum", subKey, "1"), CultureInfo.InvariantCulture);
            this.StepSize = Convert.ToDouble(profile.GetValue(driverId, "StepSize", subKey, "1"), CultureInfo.InvariantCulture);
            this.ReadOnly = Convert.ToBoolean(profile.GetValue(driverId, "ReadOnly", subKey, bool.FalseString), CultureInfo.InvariantCulture);
            this.Value = Convert.ToDouble(profile.GetValue(driverId, "Value", subKey, "0"), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Sets the value with a check that the value is correct.
        /// Throws a MethodNotImplementedException if the switch is read only
        /// Throws an InvalidValueException if the value is out of range
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="message">The message.</param>
        internal void SetValue(double value, string message)
        {
            if (this.ReadOnly)
            {
                throw new MethodNotImplementedException(string.Format("{0} is read only", this.Name));
            }
            if (value < Minimum || value > Maximum)
            {
                throw new ASCOM.InvalidValueException("Switch " + this.Name, value.ToString(), string.Format("{0} to {1}", Minimum, Maximum));
            }
            this.Value = value; 
        }

        /// <summary>
        /// Saves the switch to the ASCOM profile
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="driverId">The driver id.</param>
        /// <param name="id">The id.</param>
        internal void Save(Profile profile, string driverId, int id)
        {
            var subKey = "Switch" + id.ToString();
            profile.WriteValue(driverId, "Name", this.Name, subKey);
            profile.WriteValue(driverId, "Minimum", this.Minimum.ToString(CultureInfo.InvariantCulture), subKey);
            profile.WriteValue(driverId, "Maximum", this.Maximum.ToString(CultureInfo.InvariantCulture), subKey);
            profile.WriteValue(driverId, "StepSize", this.StepSize.ToString(CultureInfo.InvariantCulture), subKey);
            profile.WriteValue(driverId, "ReadOnly", this.ReadOnly.ToString(CultureInfo.InvariantCulture), subKey);
            profile.WriteValue(driverId, "Value", this.Value.ToString(CultureInfo.InvariantCulture), subKey);
        }

        /// <summary>
        /// Determines if the switch parameters are valid, returns false if not with the reason.
        /// </summary>
        /// <param name="reason">The reason this switch is invalid</param>
        /// <returns>
        ///   <c>true</c> if the switch parameters are valid; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsValid(out string reason)
        {
            if (Minimum >= Maximum)
            {
                reason = "Maximum not greater than Minimum";
                return false;
            }
            if ((Maximum - Minimum) / StepSize < 1)
            {
                reason = "Step size gives less than two positions";
                return false;
            }
            if (Value < Minimum || Value > Maximum)
            {
                reason = "Value not between Minimum and Maximum";
                return false;
            }
            reason = string.Empty;
            return true;
        }
    }
}
