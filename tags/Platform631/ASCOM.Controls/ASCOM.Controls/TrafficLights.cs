using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.Controls
{
    ///<summary>
    /// The TrafficLight enumeration may be used in any situation where a Normal/Warning/Error status indication is needed.
    ///</summary>
    public enum TrafficLight
    {
        /// <summary>
        /// Green traffic light represents a good or normal status.
        /// </summary>
        Green,
        /// <summary>
        /// Yellow traffic light represents a warning condition, which does not necessarily prevent continued
        /// operation but which merits further investigation.
        /// </summary>
        Yellow,
        /// <summary>
        /// Red traffic light represents an error condition or a situation that prevents further progress.
        /// </summary>
        Red
    }
}
