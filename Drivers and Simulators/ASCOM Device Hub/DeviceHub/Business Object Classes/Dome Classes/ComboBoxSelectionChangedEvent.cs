using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ASCOM.DeviceHub
{
    internal class ComboBoxSelectionChangedEvent
    {
        public ComboBox ComboBox { get; set; }
        
        public SelectionChangedEventArgs SelectionChangedEventArgs { get; set; }

        public ComboBoxSelectionChangedEvent(ComboBox comboBox, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            ComboBox = comboBox;
            SelectionChangedEventArgs = selectionChangedEventArgs;
        }
    }
}
