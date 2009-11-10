ASCOM.Platform.DependencyBuilder

Purpose
=======

This project does not generate any usable code, it is a
build-time utility that gathers up all of the core components
and copies them into the 
[Drivers and Simulators\PlatformDependencies] folder.

*** Note that the PlatformDependencies folder always
*** contains debug versions.

The project has dependencies on all of the relevant core
components to ensure they are built before the copy occurs.

The PlatformDependencies folder should occasionally be
committed to the version control store so that the core
platform components are exposed to developers.

Removing this project from the build will effectively
freeze the platform dependencies for external developers.

Tim Long <Tim@tigranetworks.co.uk>
TiGra Networks
2009-07-05