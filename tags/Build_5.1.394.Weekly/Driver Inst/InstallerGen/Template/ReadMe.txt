Inno Setup script generator template update of 24-Jan-2008
----------------------------------------------------------

This update adds a launch check for the ASCOM Platform 5 and the two helper components. If no Platform is installed, or if a Platform other than 5.x is installed, or if the DriverHelper and DriverHelper2 objects cannot be created, the installation process is stopped with a popup message. This allows driver installers to assure that a working Platform 5.x is installed before starting installation.

INSTALLATION
------------

Copy the enclosed DriverInstallTemplate.iss file to C:\Program Files\ASCOM\InstallGen\Resources, replacing the existing one. BE SURE YOU SEE THE REPLACE-FILE "ARE YOU SURE?" POPUP TO ASSURE YOU ARE REALLY REPLACING THE OLD TEMPLATE.
