// All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
// Required code must lie within the device implementation region
// The //ENDOFINSERTEDFILE tag must be the last but one line in this file

using ASCOM;
using ASCOM.Utilities;

class DeviceFocuser
{
    Util util = new Util();
    TraceLogger tl = new TraceLogger();

    private bool Connected // Dummy Connected method because this is referenced in the Link method
    {
        get { return false; }
        set { }
    }

    #region IFocuser Implementation

    private int focuserPosition = 0; // Class level variable to hold the current focuser position
    private const int focuserSteps = 10000;

	/// <summary>
	/// True if the focuser is capable of absolute position; that is, being commanded to a specific step location.
	/// </summary>
	public bool Absolute
    {
        get
        {
            tl.LogMessage("Absolute Get", true.ToString());
            return true; // This is an absolute focuser
        }
    }

	/// <summary>
	/// Immediately stop any focuser motion due to a previous <see cref="Move" /> method call.
	/// </summary>
	public void Halt()
    {
        tl.LogMessage("Halt", "Not implemented");
        throw new MethodNotImplementedException("Halt");
    }

	/// <summary>
	/// True if the focuser is currently moving to a new position. False if the focuser is stationary.
	/// </summary>
	public bool IsMoving
    {
        get
        {
            tl.LogMessage("IsMoving Get", false.ToString());
            return false; // This focuser always moves instantaneously so no need for IsMoving ever to be True
        }
    }

	/// <summary>
	/// State of the connection to the focuser.
	/// </summary>
	public bool Link
    {
        get
        {
            tl.LogMessage("Link Get", this.Connected.ToString());
            return this.Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
        }
        set
        {
            tl.LogMessage("Link Set", value.ToString());
            this.Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
        }
    }

	/// <summary>
	/// Maximum increment size allowed by the focuser;
	/// i.e. the maximum number of steps allowed in one move operation.
	/// </summary>
	public int MaxIncrement
    {
        get
        {
            tl.LogMessage("MaxIncrement Get", focuserSteps.ToString());
            return focuserSteps; // Maximum change in one move
        }
    }

	/// <summary>
	/// Maximum step position permitted.
	/// </summary>
	public int MaxStep
    {
        get
        {
            tl.LogMessage("MaxStep Get", focuserSteps.ToString());
            return focuserSteps; // Maximum extent of the focuser, so position range is 0 to 10,000
        }
    }

	/// <summary>
	/// Moves the focuser by the specified amount or to the specified position depending on the value of the <see cref="Absolute" /> property.
	/// </summary>
	/// <param name="Position">Step distance or absolute position, depending on the value of the <see cref="Absolute" /> property.</param>
	public void Move(int Position)
    {
        tl.LogMessage("Move", Position.ToString());
        focuserPosition = Position; // Set the focuser position
    }

	/// <summary>
	/// Current focuser position, in steps.
	/// </summary>
	public int Position
    {
        get
        {
            return focuserPosition; // Return the focuser position
        }
    }


	/// <summary>
	/// Step size (microns) for the focuser.
	/// </summary>
	public double StepSize
    {
        get
        {
            tl.LogMessage("StepSize Get", "Not implemented");
            throw new PropertyNotImplementedException("StepSize", false);
        }
    }

	/// <summary>
	/// The state of temperature compensation mode (if available), else always False.
	/// </summary>
	public bool TempComp
    {
        get
        {
            tl.LogMessage("TempComp Get", false.ToString());
            return false;
        }
        set
        {
            tl.LogMessage("TempComp Set", "Not implemented");
            throw new PropertyNotImplementedException("TempComp", false);
        }
    }

	/// <summary>
	/// True if focuser has temperature compensation available.
	/// </summary>
	public bool TempCompAvailable
    {
        get
        {
            tl.LogMessage("TempCompAvailable Get", false.ToString());
            return false; // Temperature compensation is not available in this driver
        }
    }

	/// <summary>
	/// Current ambient temperature in degrees Celsius as measured by the focuser.
	/// </summary>
	public double Temperature
    {
        get
        {
            tl.LogMessage("Temperature Get", "Not implemented");
            throw new PropertyNotImplementedException("Temperature", false);
        }
    }

    #endregion

    //ENDOFINSERTEDFILE
}