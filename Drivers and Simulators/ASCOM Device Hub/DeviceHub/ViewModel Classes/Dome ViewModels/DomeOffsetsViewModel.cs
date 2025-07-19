using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace ASCOM.DeviceHub
{
    public class DomeOffsetsViewModel : DeviceHubViewModelBase
    {
        private bool _supportMultipleTelescopes;
        private int _profileIndex;

        private int _gemAxisOffset;
        private int _gemAxisOffset1;
        private int _gemAxisOffset2;
        private int _gemAxisOffset3;
        private int _gemAxisOffset4;
        private int _gemAxisOffset5;

        private int _opticalOffset;
        private int _opticalOffset1;
        private int _opticalOffset2;
        private int _opticalOffset3;
        private int _opticalOffset4;
        private int _opticalOffset5;

        private string _telescopeName1;
        private string _telescopeName2;
        private string _telescopeName3;
        private string _telescopeName4;
        private string _telescopeName5;

        public bool SupportMultipleTelescopes
        {
            get { return _supportMultipleTelescopes; }
            set
            {
                if (value != _supportMultipleTelescopes)
                {
                    _supportMultipleTelescopes = value;
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

        public int GemAxisOffset1
        {
            get { return _gemAxisOffset1; }
            set
            {
                if (value != _gemAxisOffset1)
                {
                    _gemAxisOffset1 = value;
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

        public int OpticalOffset1
        {
            get { return _opticalOffset1; }
            set
            {
                if (value != _opticalOffset1)
                {
                    _opticalOffset1 = value;
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

        public int ProfileIndex
        {
            get { return _profileIndex; }
            set
            {
                if (value != _profileIndex)
                {
                    _profileIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TelescopeName1
        {
            get { return _telescopeName1; }
            set
            {
                if (value != _telescopeName1)
                {
                    _telescopeName1 = value;
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

        public DomeOffsetsViewModel()
        {
        }

        public void InitializeLayout(DomeLayoutSettings settings)
        {
            SupportMultipleTelescopes = settings.SupportMultipleTelescopes;

            GemAxisOffset = settings.GemAxisOffset;
            GemAxisOffset1 = settings.GemAxisOffset1;
            GemAxisOffset2 = settings.GemAxisOffset2;
            GemAxisOffset3 = settings.GemAxisOffset3;
            GemAxisOffset4 = settings.GemAxisOffset4;
            GemAxisOffset5 = settings.GemAxisOffset5;

            OpticalOffset = settings.OpticalOffset;
            OpticalOffset1 = settings.OpticalOffset1;
            OpticalOffset2 = settings.OpticalOffset2;
            OpticalOffset3 = settings.OpticalOffset3;
            OpticalOffset4 = settings.OpticalOffset4;
            OpticalOffset5 = settings.OpticalOffset5;

            ProfileIndex = settings.ProfileIndex;
            TelescopeName1 = settings.TelescopeName1;
            TelescopeName2 = settings.TelescopeName2;
            TelescopeName3 = settings.TelescopeName3;
            TelescopeName4 = settings.TelescopeName4;
            TelescopeName5 = settings.TelescopeName5;
        }

        public DomeLayoutSettings GetDomeOffsets()
        {
            DomeLayoutSettings settings = new DomeLayoutSettings
            {
                SupportMultipleTelescopes = _supportMultipleTelescopes,

                GemAxisOffset = _gemAxisOffset,
                GemAxisOffset1 = _gemAxisOffset1,
                GemAxisOffset2 = _gemAxisOffset2,
                GemAxisOffset3 = _gemAxisOffset3,
                GemAxisOffset4 = _gemAxisOffset4,
                GemAxisOffset5 = _gemAxisOffset5,

                OpticalOffset = _opticalOffset,
                OpticalOffset1 = _opticalOffset1,
                OpticalOffset2 = _opticalOffset2,
                OpticalOffset3 = _opticalOffset3,
                OpticalOffset4 = _opticalOffset4,
                OpticalOffset5 = _opticalOffset5,

                ProfileIndex = _profileIndex,
                TelescopeName1 = _telescopeName1,
                TelescopeName2 = _telescopeName2,
                TelescopeName3 = _telescopeName3,
                TelescopeName4 = _telescopeName4,
                TelescopeName5 = _telescopeName5,
            };

            return settings;
        }

        #region EditGemOffsetCommand

        private ICommand _editGemOffsetCommand;

        public ICommand EditGemOffsetCommand
        {
            get
            {
                if (_editGemOffsetCommand == null)
                {
                    _editGemOffsetCommand = new RelayCommand((parameter) => this.EditGemOffsetProcess(parameter));
                }

                return _editGemOffsetCommand;
            }
        }

        private void EditGemOffsetProcess(object parameter)
        {
            string negativeText = "Negative";
            string positiveText = "Positive";
            int offset = 1;

            switch (parameter.ToString())
            {
                case "1":
                    offset = GemAxisOffset1;
                    break;

                case "2":
                    offset = GemAxisOffset2;
                    break;

                case "3":
                    offset = GemAxisOffset3;
                    break;

                case "4":
                    offset = GemAxisOffset4;
                    break;

                case "5":
                    offset = GemAxisOffset5;
                    break;
            }

            offset = EditTheOffset(offset, negativeText, positiveText);

            switch (parameter.ToString())
            {
                case "1":
                    GemAxisOffset1 = offset;
                    break;

                case "2":
                    GemAxisOffset2 = offset;
                    break;

                case "3":
                    GemAxisOffset3 = offset;
                    break;

                case "4":
                    GemAxisOffset4 = offset;
                    break;

                case "5":
                    GemAxisOffset5 = offset;
                    break;
            }
        }

        #endregion


        #region EditOpticalOffsetCommand

        private ICommand _editOpticalOffsetCommand;

        public ICommand EditOpticalOffsetCommand
        {
            get
            {
                if (_editOpticalOffsetCommand == null)
                {
                    _editOpticalOffsetCommand = new RelayCommand((parameter) => this.EditOpticalOffsetProcess(parameter));
                }

                return _editOpticalOffsetCommand;
            }
        }

        private void EditOpticalOffsetProcess(object parameter)
        {
            string negativeText = "Negative";
            string positiveText = "Positive";
            int offset = 1;

            switch (parameter.ToString())
            {
                case "1":
                    offset = OpticalOffset1;
                    break;

                case "2":
                    offset = OpticalOffset2;
                    break;

                case "3":
                    offset = OpticalOffset3;
                    break;

                case "4":
                    offset = OpticalOffset4;
                    break;

                case "5":
                    offset = OpticalOffset5;
                    break;
            }

            offset = EditTheOffset(offset, negativeText, positiveText);

            switch (parameter.ToString())
            {
                case "1":
                    OpticalOffset1 = offset;
                    break;

                case "2":
                    OpticalOffset2 = offset;
                    break;

                case "3":
                    OpticalOffset3 = offset;
                    break;

                case "4":
                    OpticalOffset4 = offset;
                    break;

                case "5":
                    OpticalOffset5 = offset;
                    break;
            }
        }

        #endregion


        private int EditTheOffset(int offset, string negativeText, string positiveText)
        {
            string directions = $"{positiveText}/{negativeText}";

            ScopeDomeOffsetViewModel vm = new ScopeDomeOffsetViewModel(directions)
            {
                NegativeText = negativeText,
                PositiveText = positiveText
            };

            vm.InitializeValues(new int[1] { offset });

            IDialogService svc = ServiceContainer.Instance.GetService<IDialogService>();
            bool? result = svc.ShowDialog(vm);

            if (result.HasValue && result.Value)
            {
                int[] values = vm.GetValues();

                offset = values[0];
            }

            vm.Dispose();
            return offset;
        }






    }
}
