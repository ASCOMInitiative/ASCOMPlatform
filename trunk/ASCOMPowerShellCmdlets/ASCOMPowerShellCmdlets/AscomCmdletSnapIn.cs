using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace ASCOMPowerShellCmdlets
{
    [RunInstaller(true)]
    public class AscomCmdletSnapIn : CustomPSSnapIn
    {
        private Collection<CmdletConfigurationEntry> _cmdlets;

        /// <summary>
        /// Gets description of powershell snap-in.
        /// </summary>
        public override string Description
        {
            get { return "Cmdlets to interface with the ASCOM platform"; }
        }

        /// <summary>
        /// Gets name of power shell snap-in
        /// </summary>
        public override string Name
        {
            get { return "ASCOM.PowerShell.Cmdlets"; }
        }

        /// <summary>
        /// Gets name of the vendor
        /// </summary>
        public override string Vendor
        {
            get { return "ASCOM Initiative - http://ascom-standards.org/"; }
        }

        public override Collection<CmdletConfigurationEntry> Cmdlets
        {
            get
            {
                if (null == _cmdlets)
                {
                    _cmdlets = new Collection<CmdletConfigurationEntry>();
                    _cmdlets.Add(new CmdletConfigurationEntry
                      ("Show-Chooser", typeof(AscomChooser), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry
                      ("New-Telescope", typeof(AscomTelescope), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry
                      ("New-Dome", typeof(AscomDome), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry
                     ("New-Util", typeof(AscomUtil), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry
                     ("New-Camera", typeof(AscomCamera), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry
                     ("New-FilterWheel", typeof(AscomFilterWheel), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry
                     ("New-Focuser", typeof(AscomFocuser), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry
                     ("New-Rotator", typeof(AscomRotator), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry
                     ("New-Switch", typeof(AscomSwitch), "ASCOMPowerShellCmdlets.dll-Help.xml"));
                }
                return _cmdlets;
            }
        }

    }
}
