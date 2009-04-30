Attribute VB_Name = "Startup"
'---------------------------------------------------------------------
' Copyright © 2007, The ASCOM Initiative
'
' Permission is hereby granted to use this Software for any purpose
' including combining with commercial products, creating derivative
' works, and redistribution of source or binary code, without
' limitation or consideration. Any redistributed copies of this
' Software must include the above Copyright Notice.
'
' THIS SOFTWARE IS PROVIDED "AS IS". THE ASCOM INITIATIVE MAKES NO
' WARRANTIES REGARDING THIS SOFTWARE, EXPRESS OR IMPLIED, AS TO ITS
' SUITABILITY OR FITNESS FOR A PARTICULAR PURPOSE.
'---------------------------------------------------------------------
'
'   ===========
'   STARTUP.BAS  Helper server startup module
'   ===========
'
' Written:  03-Apr-2007   Robert B. Denny <rdenny@dc3.com>
'
' Edits:
'
' When      Who     What
' --------- ---     --------------------------------------------------
' 03-Apr-07 rbd     Initial edit
' --------------------------------------------------------------------
Option Explicit

'---------------------------------------------------------------------
'
' Main() - Helper server main entry point
'
'---------------------------------------------------------------------
Sub Main()
    
    If App.PrevInstance Then End            ' only run one copy at a time
    InitConfig                              ' Sets "not loaded" sentinel
    
End Sub

