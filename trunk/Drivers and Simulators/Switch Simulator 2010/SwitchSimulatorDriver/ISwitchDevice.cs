using System;
namespace ASCOM.SwitchSimulator
{
    public interface ISwitchDevice
    {
        string Description { get; set; }
        string Name { get; set; }
        int Id { get; set; }
        bool On { get; set; }
    }
}
