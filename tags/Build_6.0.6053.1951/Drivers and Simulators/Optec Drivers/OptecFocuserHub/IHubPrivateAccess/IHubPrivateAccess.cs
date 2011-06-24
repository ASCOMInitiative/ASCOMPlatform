using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.OptecFocuserHub
{

    // 
    // COM-Visible Interface
    //
    [Guid("4DB31DDA-93A6-4EB4-9FFA-AF0F00D989D6")]
    public interface IHubPrivateAccess
    {
        string F1Nickname { get; set; }
        string F2Nickname { get; set; }

    }
}
