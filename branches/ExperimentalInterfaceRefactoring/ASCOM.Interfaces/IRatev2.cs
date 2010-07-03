//-----------------------------------------------------------------------
// <summary>Defines the IRate Interface</summary>
//-----------------------------------------------------------------------
using System;
using ASCOM.Interface;

namespace ASCOM.Interface
{
    /// <summary>
    /// Describes a range of rates supported by the MoveAxis() method (degrees/per second)
    /// These are contained within the AxisRates collection. They serve to describe one or more supported ranges of rates of motion about a mechanical axis. 
    /// It is possible that the Rate.Maximum and Rate.Minimum properties will be equal. In this case, the Rate object expresses a single discrete rate. 
    /// Both the Rate.Maximum and Rate.Minimum properties are always expressed in units of degrees per second. 
    /// </summary>
    public interface IRatev2 : ASCOM.Interface.IRate
    {

    }
}
