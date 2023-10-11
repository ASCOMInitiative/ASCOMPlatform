
namespace ASCOM.Utilities
{
    
    internal class SimulatorDescriptor
    {
        private string _ProgID;
        private bool _SixtyFourBit;
        private bool _IsPlatform5;
        private string _Description;
        private string _DeviceType;
        private int _InterfaceVersion;
        private string _DriverVersion;
        private string _Name;
        private double[,] _AxisRates;
        private bool _AxisRatesRelative;

        public string ProgID
        {
            get
            {
                return _ProgID;
            }
            set
            {
                _ProgID = value;
            }
        }

        public bool SixtyFourBit
        {
            get
            {
                return _SixtyFourBit;
            }
            set
            {
                _SixtyFourBit = value;
            }
        }

        public bool IsPlatform5
        {
            get
            {
                return _IsPlatform5;
            }
            set
            {
                _IsPlatform5 = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        public string DeviceType
        {
            get
            {
                return _DeviceType;
            }
            set
            {
                _DeviceType = value;
            }
        }

        public int InterfaceVersion
        {
            get
            {
                return _InterfaceVersion;
            }
            set
            {
                _InterfaceVersion = value;
            }
        }

        public string DriverVersion
        {
            get
            {
                return _DriverVersion;
            }
            set
            {
                _DriverVersion = value;
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public double[,] AxisRates
        {
            get
            {
                return _AxisRates;
            }
            set
            {
                _AxisRates = value;
            }
        }
        public bool AxisRatesRelative
        {
            get
            {
                return _AxisRatesRelative;
            }
            set
            {
                _AxisRatesRelative = value;
            }
        }
    }
}