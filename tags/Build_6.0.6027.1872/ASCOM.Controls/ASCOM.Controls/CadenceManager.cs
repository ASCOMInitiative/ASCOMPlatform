using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using ASCOM.Internal;
using Timer = System.Timers.Timer;

/*
 * Note: the reader may wonder why the lock() statement was not used to protect the UpdateList collection.
 * This is because the threading model of a WinForms app makes it possible to get into a deadlock situation.
 * Our timer tick event marshalls updates onto the GUI thread, and for reasons I don't fully understand,
 * this causes deadlock, particularly in the Visual Studio designer, which hangs Visual Studio.
 * 
 * So, an alternative strategy was used. The Add and Remove methods execute within a finally block, making them
 * contrained regions, i.e. the code is atomic and cannot be interrupted. There is a danger here in that an exception within a finally()
 * block can cause the process to abort, so this code is kept deliberately simple and should not pose a problem here.
 * 
 * In the timer tick event, the UpdateList collection is cloned before iterating over the clone. This could mean
 * that some of the referenced objects could be disposed during an update cycle, and it is possible that an object may receive
 * one update even after it has unregistered from the CadenceManager. For this reason, updates are protected in a
 * try/catch block. Any exception causes the object to be permanently removed from the UpdateList.
 * CadenceManager expects that the objects it manages will ocaationally throw exceptions, for example if they've been
 * disposed. If this happens, then that control will stop updating, but that's probably what was intended anyway.
 */

namespace ASCOM.Controls
{
    /// <summary>
    ///   Manages objects that must be toggled on and off in a regular pattern over time. This is known as a cadence.
    ///   CadenceManager is intended primarily for Windows Forms controls, but can be used for any item that implements
    ///   the <see cref = "ICadencedControl" /> interface.
    /// </summary>
    /// <remarks>
    ///   CadenceManager behaves slightly differently if the managed item is a Windows Forms control.
    ///   <list type = "bulleted">
    ///     <item>Invisible controls do not receive updates until they become visible again.</item>
    ///     <item>The <see cref = "ICadencedControl.CadenceUpdate" /> method is marshalled to the GUI thread.</item>
    ///   </list>
    /// </remarks>
    public sealed class CadenceManager
    {
        /// <summary>
        ///   The one and only instance of this class.
        /// </summary>
        private static volatile CadenceManager instance;

        /// <summary>
        ///   An object used for thread synchronization during object initialization.
        ///   This ensures that the singleton is thread-safe.
        /// </summary>
        private static readonly object SyncRoot = new Object();

        /// <summary>
        ///   A list of all the anunciator controls that have been created which need updating
        ///   when the timer ticks.
        /// </summary>
        internal static List<ICadencedControl> UpdateList = new List<ICadencedControl>();

        /// <summary>
        ///   Indicates the current bit position within the cadence register.
        /// </summary>
        internal static int CadenceBitPosition;

        /// <summary>
        ///   A timer that triggers updates to anunciators to simulate flashing.
        /// </summary>
        private static readonly Timer CadenceTimer = new Timer();


        private CadenceManager()
        {
            UpdateList.Clear();
        }

        /// <summary>
        ///   Gets a reference to the Singleton.
        ///   If the Singleton has not yet be instantiated, this causes the object
        ///   to be created and the constructor to execute.
        ///   This operation is thread-safe.
        /// </summary>
        public static CadenceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot) // Ensures thread safety
                    {
                        if (instance == null)
                            instance = new CadenceManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        ///   Adds the specified <see cref = "ICadencedControl" /> to the list of managed controls.
        ///   If this is the first control being added, then the update timer is configured and started.
        /// </summary>
        /// <param name = "control">The control to be managed.</param>
        /// <remarks>
        ///   Each control can only appear in the list once (duplicate adds will be silently ignored).
        /// </remarks>
        public void Add(ICadencedControl control)
        {
            try
            {
                // Putting all the code in the finally block makes it a CONSTRAINED REGION that executes atomically
                // This prevents a timer event from happening in the middle of an add/remove operation.
            } 
            finally
            {
                if (!UpdateList.Contains(control))
                {
                    UpdateList.Add(control);

                    // If this is the first instance, then create and start the timer.
                    if (UpdateList.Count == 1)
                    {
                        CadenceTimer.Interval = 125;
                        CadenceTimer.Elapsed += TmrCadenceTick;
                        CadenceTimer.Start(); // Let rip.
                    }
                }
            }
        }

        /// <summary>
        ///   Removes a control from the <see cref = "UpdateList" />.
        ///   If no managed controls remain in the list, then the update timer is stopped.
        /// </summary>
        public void Remove(ICadencedControl control)
        {
            try
            {
                // Putting all the code in the finally block makes it a CONSTRAINED REGION that executes atomically
                // This prevents a timer event from happening in the middle of an add/remove operation.
            } 
            finally
            {
                if (UpdateList.Contains(control))
                    UpdateList.Remove(control); // Make sure this object doesn't receive any more timer updates.

                // Stop the update timer if there are no items left in the update list.
                if (UpdateList.Count == 0)
                {
                    CadenceTimer.Stop();
                    CadenceTimer.Elapsed -= TmrCadenceTick; // For symmetry, unregister the handler.
                }
            }
        }

        /// <summary>
        ///   Handles the Tick event of the tmrCadence control.
        ///   Computes the new display status for each cadenced control based on its <see cref = "ICadencedControl.Cadence" />
        ///   property and requests the control update itself with the new value.
        /// </summary>
        /// <param name = "sender">The source of the event.</param>
        /// <param name = "e">The <see cref = "System.EventArgs" /> instance containing the event data.</param>
        private void TmrCadenceTick(object sender, ElapsedEventArgs e)
        {
            // First check if there are any managed controls and, if not, shut down the timer.
            if (UpdateList.Count < 1)
            {
                CadenceTimer.Stop(); // Stop any further tick events.
                CadenceTimer.Elapsed -= TmrCadenceTick; // Withdraw the event handler delegate.
                return; // Nothing more to do.
            }

            // Increment and (if necessary) wrap the cadence bit position index.
            if (++CadenceBitPosition > 31)
            {
                CadenceBitPosition = 0;
            }

            // Make a clone of the UpdateList collection to avoid threading problems,
            // and InvalidOperationException if we need to modify the list.
            // Note that this only clones teh collection, the objects referenced objects are not cloned.
            var clonedUpdateList = new List<ICadencedControl>(UpdateList);

            // Visit each managed control and ask it to update its status.
            foreach (ICadencedControl item in clonedUpdateList)
            {
                if (item == null) continue;
                var control = item as Control;                      // Attempt to cast the item to a WinForms control
                if (control != null && !control.Visible) continue;  // Shortcut invisible controls.
                try
                {
                    uint cadenceMask = (uint) item.Cadence;
                    bool state = cadenceMask.Bit(CadenceBitPosition);

                    if (control != null && control.InvokeRequired)
                        // It its a control and we're not on the GUI thread, marshall to the GUI thread.
                    {
                        control.Invoke(new CadenceUpdateDelegate(item.CadenceUpdate), new object[] {state});
                    }
                    else
                        // If it's not a control or we are already on the GUI thread, simply call the item's update method.
                    {
                        item.CadenceUpdate(state);
                    }
                }
                    catch (InvalidOperationException ex)
                    {
                        // This probably means that the collection was modified while we were iterating over it.
                        // This shouldn't happen, so emit some diagnostics and then stop processing updates for this cycle.
                        Trace.WriteLine("Collection possibly modified while iterating\n" +ex);
                        break;
                    }
                catch (Exception ex)
                {
                    // If any managed item causes an exception of any other kind, remove it from the update list.
                    Debug.WriteLine("Exception while updating ICadencedControl - removing item from update list\n" +
                                    ex);
                    UpdateList.Remove(item);    // This is why we needed to use a cloned collection
                }
            }
        }

        #region Nested type: CadenceUpdateDelegate

        /// <summary>
        ///   Delegate used to make thread-safe control updates.
        /// </summary>
        private delegate void CadenceUpdateDelegate(bool state);

        #endregion
    }
}