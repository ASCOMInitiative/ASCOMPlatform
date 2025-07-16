using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
    public class DomeOffsetsViewModel : DeviceHubDialogViewModelBase
    {
        public DomeOffsetsViewModel()
            : base("Dome Offsets")
        {
            _domeOffsetsVm = new DomeOffsetsViewModel();
        }
        public string DomeID { get; set; }
        private DomeOffsetsViewModel _domeOffsetsVm;

        public DomeOffsetsViewModel DomeOffsetsVm
        {
            get { return _domeOffsetsVm; }
            set
            {
                if (value != _domeOffsetsVm)
                {
                    _domeOffsetsVm = value;
                    OnPropertyChanged();
                }
            }
        }


    }
}
