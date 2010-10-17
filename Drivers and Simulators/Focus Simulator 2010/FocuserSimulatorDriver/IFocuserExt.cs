using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    public interface IFocuserExt : IFocuser

{
    bool CanHalt { get; set; }
    bool TempProbe { get; set; }
    bool Synchronus { get; set; }
    bool CanStepSize { get; set; }
    int TempMax { get; set; }
    int TempMin { get; set; }
    int TempPeriod { get; set; }
    int TempSteps { get; set; }

    new bool Absolute { set; }
    new int MaxStep { set; }
    new double StepSize { set; }
    new int Position { set; }

    new int MaxIncrement { set; }
    new double Temperature { set; }

    void SaveProfileSettings();
    void SetDefaultProfileSettings();

}
}
