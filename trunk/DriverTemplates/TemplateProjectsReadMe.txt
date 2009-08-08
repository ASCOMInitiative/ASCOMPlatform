ASCOM Template Projects in Platform 5.1

Platform 5.1 uses a different approach to template projects. Rather than having
a template project for each type of driver, just a single template is provided
for each supported language (that is, one for C# and one for VB).

A Template Wizard is also provided and installed in the GAC by the installer.

When the user creates a new driver project, the Template Wizard is executed and
displays a form to gather various parameters from the user. These values are
added to the replacement disctionary for the new project. The items set by the
wizard are: $deviceid$, $deviceclass$, $devicename$, $projectname$,
$safeprojectname$.

[planned]
In addition, the Template Wizard expands the interface implementation and
wires up (or unwires) the SettingsProvider component.
