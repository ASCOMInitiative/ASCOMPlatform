using System;
using System.Windows.Media.Media3D;

namespace ASCOM.DeviceHub
{
    public class DomeLayoutSettings : DevicePropertiesBase, ICloneable
    {
        #region Constructors

        public DomeLayoutSettings()
        { }

        public DomeLayoutSettings(DomeLayoutSettings other)
        {
            this._domeScopeOffset = other._domeScopeOffset;
            this._domeRadius = other._domeRadius;
            this._azimuthAccuracy = other._azimuthAccuracy;
            this._slaveInterval = other._slaveInterval;

            // Copy the offsets for the GEM axis
            this._gemAxisOffset = other._gemAxisOffset;
            this._gemAxisOffset2 = other._gemAxisOffset2;
            this._gemAxisOffset3 = other._gemAxisOffset3;
            this._gemAxisOffset4 = other._gemAxisOffset4;
            this._gemAxisOffset5 = other._gemAxisOffset5;

            // Copy the optical offsets
            this._opticalOffset = other._opticalOffset;
            this._opticalOffset2 = other._opticalOffset2;
            this._opticalOffset3 = other._opticalOffset3;
            this._opticalOffset4 = other._opticalOffset4;
            this._opticalOffset5 = other._opticalOffset5;

            // Copy the telescope names
            this._telescopeName = other._telescopeName;
            this._telescopeName2 = other._telescopeName2;
            this._telescopeName3 = other._telescopeName3;
            this._telescopeName4 = other._telescopeName4;
            this._telescopeName5 = other._telescopeName5;
        }

        #endregion Constructors

        #region Change Notification Properties

        private Point3D _domeScopeOffset;
        private int _domeRadius;
        private int _slaveInterval;
        private int _azimuthAccuracy;

        private int _gemAxisOffset;
        private int _gemAxisOffset2;
        private int _gemAxisOffset3;
        private int _gemAxisOffset4;
        private int _gemAxisOffset5;

        private int _opticalOffset;
        private int _opticalOffset2;
        private int _opticalOffset3;
        private int _opticalOffset4;
        private int _opticalOffset5;

        private string _telescopeName;
        private string _telescopeName2;
        private string _telescopeName3;
        private string _telescopeName4;
        private string _telescopeName5;

        public Point3D DomeScopeOffset
        {
            get { return _domeScopeOffset; }
            set
            {
                if (value != _domeScopeOffset)
                {
                    _domeScopeOffset = value;
                    OnPropertyChanged();
                }
            }
        }

        public int DomeRadius
        {
            get { return _domeRadius; }
            set
            {
                if (value != _domeRadius)
                {
                    _domeRadius = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GemAxisOffset
        {
            get { return _gemAxisOffset; }
            set
            {
                if (value != _gemAxisOffset)
                {
                    _gemAxisOffset = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GemAxisOffset2
        {
            get { return _gemAxisOffset2; }
            set
            {
                if (value != _gemAxisOffset2)
                {
                    _gemAxisOffset2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GemAxisOffset3
        {
            get { return _gemAxisOffset3; }
            set
            {
                if (value != _gemAxisOffset3)
                {
                    _gemAxisOffset3 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GemAxisOffset4
        {
            get { return _gemAxisOffset4; }
            set
            {
                if (value != _gemAxisOffset4)
                {
                    _gemAxisOffset4 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GemAxisOffset5
        {
            get { return _gemAxisOffset5; }
            set
            {
                if (value != _gemAxisOffset5)
                {
                    _gemAxisOffset5 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AzimuthAccuracy
        {
            get { return _azimuthAccuracy; }
            set
            {
                if (value != _azimuthAccuracy)
                {
                    _azimuthAccuracy = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SlaveInterval
        {
            get { return _slaveInterval; }
            set
            {
                if (value != _slaveInterval)
                {
                    _slaveInterval = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OpticalOffset
        {
            get { return _opticalOffset; }
            set
            {
                if (value != _opticalOffset)
                {
                    _opticalOffset = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OpticalOffset2
        {
            get { return _opticalOffset2; }
            set
            {
                if (value != _opticalOffset2)
                {
                    _opticalOffset2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OpticalOffset3
        {
            get { return _opticalOffset3; }
            set
            {
                if (value != _opticalOffset3)
                {
                    _opticalOffset3 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OpticalOffset4
        {
            get { return _opticalOffset4; }
            set
            {
                if (value != _opticalOffset4)
                {
                    _opticalOffset4 = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OpticalOffset5
        {
            get { return _opticalOffset5; }
            set
            {
                if (value != _opticalOffset5)
                {
                    _opticalOffset5 = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TelescopeName
        {
            get { return _telescopeName; }
            set
            {
                if (value != _telescopeName)
                {
                    _telescopeName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TelescopeName2
        {
            get { return _telescopeName2; }
            set
            {
                if (value != _telescopeName2)
                {
                    _telescopeName2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TelescopeName3
        {
            get { return _telescopeName3; }
            set
            {
                if (value != _telescopeName3)
                {
                    _telescopeName3 = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TelescopeName4
        {
            get { return _telescopeName4; }
            set
            {
                if (value != _telescopeName4)
                {
                    _telescopeName4 = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TelescopeName5
        {
            get { return _telescopeName5; }
            set
            {
                if (value != _telescopeName5)
                {
                    _telescopeName5 = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Change Notification Properties

        #region ICloneable Implementation

        object ICloneable.Clone()
        {
            return new DomeLayoutSettings(this);
        }

        public DomeLayoutSettings Clone()
        {
            return new DomeLayoutSettings(this);
        }

        #endregion ICloneable Implementation
    }
}
