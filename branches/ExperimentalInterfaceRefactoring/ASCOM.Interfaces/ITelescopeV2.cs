//-----------------------------------------------------------------------
// <summary>Defines the ITelescope Interface</summary>
//-----------------------------------------------------------------------
using System;
using ASCOM.Interface;
namespace ASCOM.Interface
{
    /// <summary>
    /// Defines the ITelescope  Interface
    /// </summary>
    public interface ITelescopeV2 : ASCOM.Interface.ITelescope
    {
        /// <summary>
        /// Invokes the specified device-specific action.
        /// </summary>
        /// <param name="ActionName">
        /// A well known name agreed by interested parties that represents the action
        /// to be carried out. 
        /// <example>suppose filter wheels start to appear with automatic wheel changers; new actions could 
        /// be “FilterWheel:QueryWheels” and “FilterWheel:SelectWheel”. The former returning a 
        /// formatted list of wheel names and the second taking a wheel name and making the change.
        /// </example>
        /// </param>
        /// <param name="ActionParameters">
        /// List of required parameters or <see cref="String.Empty"/>  if none are required.
        /// </param>
        /// <returns>A string response and sets the <c>IDeviceControl.LastResult</c> property.</returns>
        string Action(string ActionName, string ActionParameters);

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>The supported actions.</value>
        string[] SupportedActions { get; }

        /// <summary>
        /// Gets the last result.
        /// </summary>
        /// <value>
        /// The result of the last executed action, or <see cref="String.Empty"	/>
        /// if no action has yet been executed.
        /// </value>
        string LastResult { get; }

    }
}
