using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASCOM.DeviceInterface;

namespace ASCOM.Simulator
{
    public interface IFocuserExt 

{
    bool CanHalt { get; set; }
    bool TempProbe { get; set; }
    bool Synchronus { get; set; }
    bool CanStepSize { get; set; }
    int TempMax { get; set; }
    int TempMin { get; set; }
    int TempPeriod { get; set; }
    int TempSteps { get; set; }

    bool Absolute { set; }
    int MaxStep { set; }
    double StepSize { set; }
    int Position { set; }

    int MaxIncrement { set; }
    double Temperature { set; }

    void SaveProfileSettings();
    void SetDefaultProfileSettings();

}
}
