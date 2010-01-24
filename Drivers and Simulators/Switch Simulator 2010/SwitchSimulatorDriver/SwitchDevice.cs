using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.SwitchSimulator
{
    public class SwitchDevice : ISwitchDevice
    {
        private string name;
        private string description;
        private bool on;
        private int id;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public bool On
        {
            get { return on; }
            set { on = value; }
        }

    }
}
