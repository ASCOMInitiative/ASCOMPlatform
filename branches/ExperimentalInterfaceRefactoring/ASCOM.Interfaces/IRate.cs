using System;
namespace ASCOM.Interfaces
{
    public interface IRate
    {
        void Dispose();
        double Maximum { get; set; }
        double Minimum { get; set; }
    }
}
