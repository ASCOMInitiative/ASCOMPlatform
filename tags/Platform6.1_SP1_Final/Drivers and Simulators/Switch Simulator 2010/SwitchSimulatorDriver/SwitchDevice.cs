using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ASCOM.Simulator
{
    /// <summary>
    /// This class is a conceptual implementation of an individual hardware switch.
    /// In reality, this code would need to tweak some hardware in response to the State
    /// property being written. Our fake implementation simply writes to the debug
    /// window.
    /// </summary>
    [Guid("B3E5FC9D-4D76-4f4b-8FF5-854A01DF413F")]
    [ClassInterface(ClassInterfaceType.None)]
    class SwitchDevice 
    {
        #region ISwitchDevice Members

        private string name;
        private bool state;

        /// <summary>
        /// The name of the switch (human readable).
        /// </summary>
        public string Name
        {
            get { return name; }
            protected internal set { name = value; }
        }

        /// <summary>
        /// Sets the state of the switch. True = on, False = off.
        /// </summary>
        public bool State
        {
            get { return state; }
            set
            {
                state = value;
                Switch.SaveProfileSetting(this.Name, this.State);
                // In reality we would go off and tweak some hardware at this point.
                Trace.WriteLine(String.Format("Switch {0} new state is {1}", this.ID, value));
            }
        }

        #endregion

        #region private implementation details (hidden from the client application)
        private static int _sequence = 0;
        /// <summary>
        /// Gets the sequence number for a new switch.
        /// Each time this property is read, its value increments by one.
        /// Note that this feature is entirely internal to the driver implementation and is not
        /// exposed to the client application.
        /// </summary>
        /// <value>The sequence number.</value>
        private static int SequenceNumber
        {
            get
            {
                return _sequence++;
            }
        }

        /// <summary>
        /// Gets or sets the switch ID.
        /// Note that this feature is entirely internal to the driver implementation and is not
        /// exposed to the client application.
        /// </summary>
        /// <value>The ID.</value>
        private int ID { get; set; }
        #endregion

        #region Public Switch Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchDevice"/> class.
        /// Assigns each new instance a unique sequential ID.
        /// </summary>
        public SwitchDevice()
        {
            this.ID = SequenceNumber;	// Allocate the switch ID based on sequential order of creation.
            this.Name = String.Format("Switch {0}", ID);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchDevice"/> class
        /// and sets its name to the supplied label.
        /// </summary>
        /// <param name="label">The name of the switch.</param>
        public SwitchDevice(string label)
        {
            this.ID = SequenceNumber;
            this.Name = label;
        }

        #endregion
    }
}
