---
uid: Version_7.1.3
title: ASCOM Platform 7.1 Update 3
tocTitle: Version 7.1 Update 3
# linkText: Optional Text to Use For Links
# keywords: keyword, term 1, term 2, "term, with comma"
# alt-uid: optional-alternate-id
# summary: Optional summary abstract
---
In line with previous Platform installers, Platform 7.1 Update 3 is a cumulative release that contains all enhancements and fixes from previous 
Platform releases and service packs. The Platform environment pre-requisites are listed 
here <link xlink:href="7d9253c2-fdfd-4c0d-8225-a96bddb49731#PreReqs70">Platform Prerequisites</link>.

## Changes in 7.1 Update 3 - For Everyone
* Logging is now more reliable and slightly faster.
* A new option has been added to the Diagnostics application (`Use TraceLogger mutex synchronisation`) so you can revert to previous 
logging behaviour if necessary.

## Issues Fixed in 7.1 Update 3 - For Everyone
* Fixed - Widely reported issues where TraceLogger returned `ProfilePersistenceException - Timed out waiting for TraceLogger mutex` exceptions
causing many clients and drivers to fail. This issue first appeared in Platform 7.1 Update 2.

## Changes in 7.1 Update 3 - For Developers
* The default locking mechanic in TraceLogger has been changed to use an instance local lock() instead of a global mutex.
  * Since its introduction in 2009, TraceLogger has used a global Windows mutex to synchronise writing to log files. 
The impact was that only one log message could be written at a time across all instances of TraceLogger and all applications.
  * The default locking mechanic has been changed to an instance local lock() because this is sufficient to protect the file being written and allows concurrent writing of log files.
  * It is strongly recommended that the new default lock() synchronisation is used as it is faster and does not cause application issues due to mutex timeouts.
* A new TraceLogger property <codeEntityReference linkText="TraceLogger.UseMutexSynchronisation">P:ASCOM.Utilities.TraceLogger.UseMutexSynchronisation</codeEntityReference>
has been added so the original global mutex synchronisation can be re-enabled if required for use cases where logging synchronisation between processes is required.
* A new option has been added to the Diagnostics application that enables end-users to revert all ASCOM TraceLogger activity to the original 
global mutex synchronisation without requiring code changes.

## Issues Fixed in 7.1 Update 3 - For Developers
* None

## Breaking Changes (For Developers) in Platform 7.1 Update 3
* None are expected, but developers should be aware of the change to the default locking mechanic in TraceLogger described above.

## Changes in the previous update
See this link for changes and fixes in <link xlink:href="Version_7.1.2">Platform 7.1 Update 2</link>.