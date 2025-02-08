@echo With command line options - Start time: %time%
@msbuild /consoleLoggerParameters:ShowTimestamp buildplatform.msbuild > BuildLog.txt
@echo End time: %time%