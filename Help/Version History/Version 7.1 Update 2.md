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
* Alpaca Dynamic Driver log files are now named ASCOM.DynamicDriver... rather than ASCOM.AlpacaSim...
* Update checker messages now refer to 'Updates' rather than 'Service Packs'.
## Issues Fixed in 7.1 Update 2 - For Everyone
* Fixed a Platform installer issue that caused some installs to stop while registering the Omni-Simulators.
* Revised the Platform installer validation tests to prevent errors being reported inappropriately.
* The Alpaca Dynamic Clients no longer use the HTTP 100-Continue protocol when sending data to Alpaca devices.
Commands are now sent directly in one packet, eliminating an un-necessary network round trip.
* The <codeEntityReference linkText="Utilities.Serial.ReceiveTerminatedBinary">M:ASCOM.Utilities.Serial.ReceiveTerminatedBinary(System.Byte[])</codeEntityReference>
method now operates correctly in locales that use double byte character sets such as Japan, Korea and China.
## Changes in 7.1 Update 2 - For Developers
* None
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