using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.SwitchSimulator
{
    public interface IAuthor
    {
        string Name { get; set; }
        string Email { get; set; }
    }
}
