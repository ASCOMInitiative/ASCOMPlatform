---
uid: Version_7.1.2
title: ASCOM Platform 7.1 Update 2
tocTitle: Version 7.1 Update 2
# linkText: Optional Text to Use For Links
# keywords: keyword, term 1, term 2, "term, with comma"
# alt-uid: optional-alternate-id
# summary: Optional summary abstract
---
In line with previous Platform installers, Platform 7.1 is a cumulative release that contains all enhancements and fixes from previous 
Platform releases and service packs. The Platform environment pre-requisites are listed 
here <link xlink:href="7d9253c2-fdfd-4c0d-8225-a96bddb49731#PreReqs70">Platform Prerequisites</link>.

## Changes in 7.1 Update 2 - For Everyone
* Platform help is now an online link to the ASCOM website rather than a local file.
* Alpaca Dynamic Driver log files are now named ASCOM.DynamicDriver... rather than ASCOM.AlpacaSim...
* Update checker messages now refer to 'Updates' rather than 'Service Packs'.

## Issues Fixed in 7.1 Update 2 - For Everyone
* Fixed an issue, introduced in Platform 7.1, that caused drivers that use ASCOM COM objects to fail in some environments, including Windows ARM 64bit.
The issue also caused the 64bit Platform installer validation test to report a -1073741819 error.
* Fixed an installer issue that caused some installs to stop while registering the Omni-Simulators.
* Fixed an installer issue where an attempt was made to register the 64bit JustAHub driver on a 32bit system.
* Fixed the Alpaca Dynamic Clients so they no longer use the HTTP 100-Continue protocol when sending commands to Alpaca devices.
HTTP PUT commands are now sent directly in one packet, eliminating an un-necessary network round trip.
* Fixed the <codeEntityReference linkText="Utilities.Serial.ReceiveTerminatedBinary">M:ASCOM.Utilities.Serial.ReceiveTerminatedBinary(System.Byte[])</codeEntityReference>
method so it now operates correctly in locales that use double byte character sets such as Japan, Korea and China.
* Fixed the Profile Explorer about dialogue so that it displays correct information about the version installed.
* Fixed a camera simulator array index out of bounds error when returning images smaller than 601 pixels wide that was introduced in Platform 7.1.

## Changes in 7.1 Update 2 - For Developers
* Developer and Library help are now online links to the ASCOM website rather than local files.

## Issues Fixed in 7.1 Update 2 - For Developers
* None

## Breaking Changes (For Developers) in Platform 7.1 Update 2
* None

## Known Limitations - Developers
The components in the Astrometry.NOVAS namespace are fully usable from .NET languages but are not fully
accessible through COM from scripting languages because a number of parameters are passed by reference or use
structures that do not pass across the COM interface. Two work rounds are available:
  * Use the Transform component in the ASCOM.Astrometry.Transform namespace, it is fully COM compatible.
  * Create your own COM presentation component that encapsulates the SOFA or NOVAS 3.1 functions that you require and presents them in 
a COM compatible fashion to suit your needs. This component can them be consumed by your scripting application.