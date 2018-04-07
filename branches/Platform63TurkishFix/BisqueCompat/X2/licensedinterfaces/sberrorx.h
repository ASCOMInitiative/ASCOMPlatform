//Copyright Software Bisque 2007

#ifndef SBERRORX_H
#define SBERRORX_H


#define SB_OK                                             0                   //No error.
#define ERR_NOERROR                                       0                   //No error.


#define ERR_COMMNOLINK                                    200                 //The operation failed because there is no connection to the device.
#define ERR_COMMOPENING                                   201                 //Could not open communications port.  The port is either in use by another application or not recognized by the system.
#define ERR_COMMSETTINGS                                  202                 //The communications port could not support the specified settings.
#define ERR_NORESPONSE                                    203                 //No response from the device.
#define ERR_MEMORY                                        205                 //Error: memory error.
#define ERR_CMDFAILED                                     206                 //Error: command failed.
#define ERR_DATAOUT                                       207                 //Transmit time-out.
#define ERR_TXTIMEOUT                                     208                 //Transmission time-out.
#define ERR_RXTIMEOUT                                     209                 //Receive time-out.
#define ERR_POSTMESSAGE                                   210                 //Post message failed.
#define ERR_POINTER                                       211                 //Pointer error.
#define ERR_ABORTEDPROCESS                                212                 //Process aborted.
#define ERR_AUTOTERMINATE                                 213                 //Error, poor communication, connection automatically terminated.
#define ERR_INTERNETSETTINGS                              214                 //Error, cannot connect to host.
#define ERR_NOLINK                                        215                 //No connection to the device.
#define ERR_PARKLOST                                      216                 //The mount was not able to unpark.
#define ERR_DRIVERNOTFOUND                                217                 //A necessary driver was not found.
#define ERR_LIMITSEXCEEDED                                218                 //Limits exceeded.
#define ERR_COMMANDINPROGRESS                             219                 //Command in progress.
#define ERR_DEVICENOTSUPPORTED                            220                 //Device not supported.
#define ERR_NOTPOINT                                      221                 //TPoint model not available.
#define ERR_MOUNTNOTSYNCED                                222                 //The operation failed because the mount not yet been synchronized to a known star.
#define ERR_USERASCLIENT                                  223                 //You must use the RASClient application to connect to a remote observatory.
#define ERR_THESKYNOTRUNNING                              224                 //The attempted operation requires the TheSky Level IV and it must be running.
#define ERR_NODEVICESELECTED                              225                 //No device has been selected.
#define ERR_CANTLAUNCHTHESKY                              226                 //Unable to launch TheSky.
#define ERR_NOTINITIALIZED                                227                 //Telescope not initialized. The telescope must be initialized in order to perform this operation.
#define ERR_COMMANDNOTSUPPORTED                           228                 //This command is not supported by the selected device.
#define ERR_LX200DESTBELOWHORIZ                           229                 //The Slew command failed because the LX200/Autostar reports that the destination coordinates are below the horizon.
#define ERR_LX200OUTSIDELIMIT                             230                 //The Slew command failed because the LX200/Autostar reports that slewing to the destination coordinates is not possible. Was the telescope synchronized?
#define ERR_MOUNTNOTHOMED                                 231                 //The operation failed because the mount is not yet homed.
#define ENUM_ERR_TPOINT_NOT_MAPPING                       232                 //TPoint not accepting mapping data.


#define FLASH_REPROGRAMMED                                3015                //Turn off power, move dip switches to off position, then turn power on and reconnect.
#define FLASH_NEEDSREPROGRAMMED                           3016                //Firmware needs re-programmed.  This will reset all limit minimum and maximum values.
#define FIRMWARE_NOT_SUPPORTED                            3017                //Firmware version is not supported.
#define FLASH_IN_PROGRAM_MODE                             3018                //The firmware in is program mode and cannot be communicated with.
#define FLASH_NOT_IN_PROGRAM_MODE                         3019                //The firmware is not in the correct state to be re-programmed.


#define ERR_OBJECTNOTFOUND                                250                 //Object not found.
#define ERR_OBJECTTOOLOW                                  251                 //Object too low.
#define ERR_MISSING_NIGHTVISIONMODE_THEME                 252                 //Setting Night Vision Mode failed.On Windows, make sure the required file 'TheSkyX Night Vision Mode.Theme' is available to the Windows Display Properties dialog.


#define ERR_DISPLAY_PROPS_ALREADY_RUNNING                 253                 //The Windows Display Properties dialog is open.  Please close it and try again.
#define ERR_THEME_NOT_SAVED                               254                 //Night Vision cannot be invoked because the current theme has been modified without being saved. Please save the current theme by clicking Start, Control Panel, Display, and from the Themes tab, click Save As.
#define ERR_NOOBJECTSELECTED                              255                 //The command failed because there is no target.  Find or click on a target.


#define ERR_BADPACKET                                     300                 //Bad packet.
#define ERR_BADCHECKSUM                                   301                 //Bad checksum.
#define ERR_UNKNOWNRESPONSE                               302                 //Unknown response.
#define ERR_UNKNOWNCMD                                    303                 //Unknown command.
#define ERR_BADSEQUENCENUM                                304                 //Bad sequence number.
#define ERR_ENCRYPTION                                    305                 //Packet encryption failed.


#define ERR_TASHIFT                                       400                 //Track and Accumulate Shift Error.
#define ERR_TAACCUM                                       401                 //Track and Accumulate Accumulation Error.
#define ERR_TACENTROID                                    402                 //Track and Accumulate Centroid Error.
#define ERR_TAREMOVEPEDESTAL                              403                 //Track and Accumulate Pedestal Error.
#define ERR_TASUBOFFSET                                   404                 //Track and Accumulate Subtract Offset.
#define ERR_TARESIZEIMAGE                                 405                 //Track and Accumulate Resize Error.
#define ERR_TACLEARBUF                                    406                 //Track and Accumulate Clear Buffer.
#define ERR_TAFINDMINMAX                                  407                 //Track and Accumulate find min/max error.
#define ERR_TASTARBRTDOWN50                               408                 //Track and Accumulate star brightness down 50%.
#define ERR_TAUSERRECTNOTFOUND                            409                 //Track and Accumulate rectangle not found.


#define ERR_COMBINE_BPP                                   500                 //Combine not available for the image bits-per-pixel.
#define ERR_COMBINE_FILETYPE                              501                 //Incorrect file type for this combine function.
#define ERR_COMBINE_READTRKLST                            502                 //Error reading track list.
#define ERR_OUTOFDISKSPACE                                503                 //Out of disk space.
#define ERR_SATURATEDPIXELS                               504                 //Cannot proceed, saturated pixels found. If possible lower your exposure time.
#define ERR_FILEAREREADONLY                               505                 //Unable to complete operation because file(s) are read only.
#define ERR_PATHNOTFOUND                                  506                 //Unable to create or access the folder.
#define ERR_FILEMUSTBESAVED                               507                 //Please save the image before using this command.


#define ERR_STARTOODIM1                                   550                 //Star too dim.  Lost during +X move.
#define ERR_STARTOODIM2                                   551                 //Star too dim.  Lost during -X move.
#define ERR_STARTOODIM3                                   552                 //Star too dim.  Lost during +Y move.
#define ERR_STARTOODIM4                                   553                 //Star too dim.  Lost during -Y move.
#define ERR_MOVEMENTTOOSMALL1                             554                 //Motion too small during +X move.  Increase calibration time.
#define ERR_MOVEMENTTOOSMALL2                             555                 //Motion too small during -X move.  Increase calibration time.
#define ERR_MOVEMENTTOOSMALL3                             556                 //Motion too small during +Y move.  Increase calibration time.
#define ERR_MOVEMENTTOOSMALL4                             557                 //Motion too small during -Y move.  Increase calibration time.
#define ERR_STARTOOCLOSETOEDGE1                           558                 //Star too close to edge after +X move.
#define ERR_STARTOOCLOSETOEDGE2                           559                 //Star too close to edge after -X move.
#define ERR_STARTOOCLOSETOEDGE3                           560                 //Star too close to edge after +Y move.
#define ERR_STARTOOCLOSETOEDGE4                           561                 //Star too close to edge after -Y move.
#define ERR_AXISNOTPERPENDICULAR1                         562                 //Invalid motion in X axis.
#define ERR_AXISNOTPERPENDICULAR2                         563                 //Invalid motion in Y axis.
#define ERR_BOTHAXISDISABLED                              564                 //Unable to calibrate, both axis are disabled.  At least one axis must be enabled to calibrate.
#define ERR_RECALIBRATE                                   565                 //Re-calibrate required.  Declination at calibration is unknown.
#define ERR_NOBRIGHTOBJECTFOUND                           566                 //No bright object found on image.


#define ERR_DSSNAMETOLONG                                 600                 //The file name and/or path is too long.
#define ERR_DSSNOTINITED                                  601                 //The DSS is not properly initialized, please check DSS Setup parameters.
#define ERR_DSSSYSERROR                                   602                 //System error.  Please verify DSS Setup parameters and make sure you have the correct disk in the CD-ROM drive.
#define ERR_DSSWRONGDISK                                  603                 //Wrong Disk.
#define ERR_DSSNOIMAGE                                    604                 //No image found to extract.
#define ERR_DSSINVALIDCOORDINATE                          605                 //Invalid coordinate(s).
#define ERR_DSSINVALIDSIZE                                606                 //Invalid size.
#define ERR_DSSDLLOLD                                     607                 //The file DSS_DLL.DLL is old and not compatible with this program. Please obtain the latest DSS_DLL.DLL.
#define ERR_DSSCDROM                                      608                 //Unable to access the CD-ROM drive specified in DSS Setup.  Make sure the drive is valid and/or there is a CD-ROM in the drive.
#define ERR_DSSHEADERSPATH                                609                 //Unable to access the headers path specified in DSS Setup.  Please correct the path.
#define ERR_DSSNODSSDISK                                  610                 //A DSS CD-ROM disk is not present in the CD-ROM drive.  Please insert one before continuing.
#define ERR_DSSNOTINSURVEY                                611                 //Not in survey.
#define ERR_SE_INTERNAL_ERROR                             612                 //An error occured within Source Extraction.


#define ERR_ILINK_NOSCALE                                 650                 //Image link has no image scale.
#define ERR_ILINK_TOOFEWBMP                               651                 //Image link failed because there are not enough stars in the image.  Try adjusting the image's background and range.
#define ERR_ILINK_TOOFEWSKY                               652                 //Image Link failed because there are an insufficient number of matching cataloged stars.  There must be at least eight cataloged stars in each image to perform an Image Link. Verify which star databases are active.
#define ERR_ILINK_NOMATCHFOUND                            653                 //Image Link failed, no pattern matching found.  Check image coordinates and image scale.
#define ERR_ILINK_NOIMAGE                                 654                 //Image Link failed because there is no image to match.
#define ERR_ILINK_ERR_ASTROM_SOLN_FAILED                  655                 //Astrometry solution failed.
#define ERR_ILINK_TOO_FEW_PAIRS                           656                 //Not enough image-catalog pairs for solution.
#define ERR_ILINK_INVALID_SCALE                           657                 //Solution returned an invalid image scale.
#define ERR_ILINK_SOLN_QUESTIONABLE                       658                 //Solution appears invalid.
#define ERR_ILINK_RMS_POOR                                659                 //Solution RMS appears invalid.
#define ERR_ILINK_WRITING_INTERMEDIATE_FILE               660                 //Error writing intermediate astrometry file.


#define ERR_SKIPIMAGE                                     700                 //Skip image error.
#define ERR_BADFORMAT                                     701                 //Unrecognized or bad file format.
#define ERR_OPENINGFILE                                   702                 //Unable to open file.
#define ERR_FEATURENAINLEVEL                              703                 //This edition does not support the requested feature.
#define ERR_SOCKETEXCEPTION                               704                 //An error occurred during a network call.
#define ERR_CANTCREATETHREAD                              705                 //Unable to create a new thread.


#define ERR_F_DOESNOTEXIST                                709                 //The file or folder does not exist.
#define ERR_F_ACCESS_WRITE                                707                 //Access denied. You do not have write access to the file or folder or item.
#define ERR_F_ACCESS_READ                                 706                 //Access denied. You do not have read access to the file or folder or item.
#define ERR_F_ACCESS_RW                                   708                 //Access denied. You do not have read/write access to the file or folder or item.
#define ERR_CHANGE_PASSWORD                               710                 //You are required to change your password before you can access this site.
#define ERR_OPENGL_NOT_COMPAT                             711                 //A newer version of OpenGL is required to run this application.
#define ERR_OP_REQUIRES_OPENGL                            712                 //This operation requires OpenGL and you are not running OpenGL.
#define ERR_INDEX_OUT_OF_RANGE                            713                 //The index is out of range.
#define ERR_TRIAL_EXPIRED                                 714                 //The trial period has expired.
#define ERR_INVALID_SNUM                                  715                 //Invalid serial number.


#define ERR_SGSTARBRTDOWN50                               800                 //Self-guide star brightness down 50%.
#define ERR_SGNEXT                                        801                 //Self-guide next error.
#define ERR_SGNEXT2                                       802                 //Self-guide next two error.


#define ERR_MNCPFIRSTERROR                                900                 //MNCP first error.


#define ERR_MNCPLASTERROR                                 999                 //MNCP last error.


#define ERR_AUTOSAVE                                      1130                //Auto-save error.


#define ERR_UPLOADNOTST6FILE                              1150                //Unable to load ST-6 file.
#define ERR_NOHEADADJNEEDED                               1151                //No head adjustment needed.
#define ERR_NOTCFW6A                                      1152                //Not a CFW 6A.
#define ERR_NOINTERFACE                                   1153                //No interface has been selected.
#define ERR_CAMERANOTFOUND                                1154                //Camera not found.
#define ERR_BAUDSWITCHFAILED                              1155                //Baud switch failed.
#define ERR_CANNOTUPLOADDARK                              1156                //Unable to upload dark frame.
#define ERR_SKIPPINGDARK                                  1157                //Skipping dark.
#define ERR_SKIPPINGLIGHT                                 1158                //Skipping light.
#define ERR_SELFGUIDENA                                   1159                //Self guide not available.
#define ERR_TRACKLOGNA                                    1160                //Tracking log not available.
#define ERR_AOREQUIREST78                                 1161                //AO not available for this camera.
#define ERR_CALIBRATEAONOTON                              1162                //AO not calibrated.
#define ERR_WRONGCAMERAFOUND                              1163                //A camera was detected, but it does not match the one selected.
#define ERR_PIXEL_MATH_OPERAND                            1164                //Cannot multiply or divide the image pixels by an operand less than 0.001.
#define ERR_IMAGE_SIZE                                    1165                //Enlarged image would exceed maximum image size. Try cropping it first.
#define ERR_CANNOT_COLORGRAB                              1166                //There is not a color filter wheel attached.
#define ERR_WRONGCFWFOUND                                 1167                //A filter was detected, but it does not match the one selected.


#define ERR_APOGEECFGNAME                                 1200                //A required initialization file was not found.  Go to Camera, Setup, and press the Settings button to choose the correct file.
#define ERR_APOGEECFGDATA                                 1201                //Error in Apogee INI file.
#define ERR_APOGEELOAD                                    1202                //Error transferring APCCD.INI data to camera.


#define ERR_APOGEEOPENOFFSET                              1220                //Invalid base I/O address passed to function.
#define ERR_APOGEEOPENOFFSET1                             1221                //Register access operation error.
#define ERR_APOGEEOPENOFFSET2                             1222                //Invalid CCD geometry.
#define ERR_APOGEEOPENOFFSET3                             1223                //Invalid horizontal binning factor.
#define ERR_APOGEEOPENOFFSET4                             1224                //Invalid vertical binning factor.
#define ERR_APOGEEOPENOFFSET5                             1225                //Invalid AIC value.
#define ERR_APOGEEOPENOFFSET6                             1226                //Invalid BIC value.
#define ERR_APOGEEOPENOFFSET7                             1227                //Invalid line offset value.
#define ERR_APOGEEOPENOFFSET8                             1228                //CCD controller sub-system not initialized.
#define ERR_APOGEEOPENOFFSET9                             1229                //CCD cooler failure.
#define ERR_APOGEEOPENOFFSET10                            1230                //Failure reading image data.
#define ERR_APOGEEOPENOFFSET11                            1231                //Invalid buffer pointer specified.
#define ERR_APOGEEOPENOFFSET12                            1232                //File not found or not valid.
#define ERR_APOGEEOPENOFFSET13                            1233                //Camera configuration data is invalid.
#define ERR_APOGEEOPENOFFSET14                            1234                //Invalid CCD handle passed to function.
#define ERR_APOGEEOPENOFFSET15                            1235                //Invalid parameter passed to function.


#define ERR_GPSTFPNOTRUNNING                              1300                //Shutter timing is enabled, but the GPSTFP application is not running.


#define ERR_IMAGECALWRONGBPP                              5000                //Unable to reduce. The image being reduced doesn't have the same bits per pixel as the reduction frames.
#define ERR_IMAGECALWRONGSIZE                             5001                //Unable to reduce. The image being reduced is larger than the reduction frames.
#define ERR_IMAGECALWRONGBIN                              5002                //Unable to reduce. The image being reduced doesn't have the same bin mode as the reduction frames.
#define ERR_IMAGECALWRONGSUBFRAME                         5003                //Unable to reduce. The image being reduced doesn't entirely overlap the reduction frames. Make sure the subframes overlap.
#define ERR_IMAGECALGROUPINUSE                            5004                //Unable to proceed. The reduction group is currently in use.
#define ERR_IMAGECALNOSUCHGROUP                           5005                //Unable to proceed. The selected reduction group no longer exists.
#define ERR_IMAGECALNOFRAMES                              5006                //Unable to proceed. The selected reduction group does not contain any reduction frames.


#define ERR_WRONGBPP                                      5020                //Unable to proceed. The images don't have the same bits per pixel.
#define ERR_WRONGSIZE                                     5021                //Unable to proceed. The images don't have the same dimensions.
#define ERR_WRONGTYPE                                     5022                //Unable to proceed. The images don't have the same format.


#define ERR_NOIMAGESINFOLDER                              5050                //Unable to proceed. The folder doesn't contain any readable images.
#define ERR_NOPATTERNMATCH                                5051                //The files could not be aligned. No pattern match was found.


#define ERR_NOTFITS                                       5070                //This operation requires a FITS file.


#define ERR_KVW_NOMINIMA                                  6000                //KVW_NOMINIMA.
#define ERR_KVW_DETERMINANTZERO                           6001                //KVW_DETERMINANTZERO.
#define ERR_KVW_DIVISIONBYZERO                            6002                //KVW_DIVISIONBYZERO.
#define ERR_KVW_NOTENOUGHPOINTS                           6003                //KVW_NOTENOUGHPOINTS.


#define ERR_AF_ERRORFIRST                                 7000                //@Focus error.
#define ERR_AF_DIVERGED                                   7001                //@Focus diverged. 
#define ERR_AF_UNDERSAMPLED                               7003                //Insufficient data to measure focus, increase exposure time. 


#define ERR_FLICCD_E_FIRST                                8000                //ERR_FLICCD_E_FIRST
#define ERR_FLICCD_E_NOTSUPP                              8001                //ERR_FLICCD_E_NOTSUPP
#define ERR_FLICCD_E_INVALID_PARAMETER                    8002                //ERR_FLICCD_E_INVALID_PARAMETER
#define ERR_FLICCD_E_INVALID_COMPORT                      8003                //ERR_FLICCD_E_INVALID_COMPORT
#define ERR_FLICCD_E_COMPORT_ERROR                        8004                //ERR_FLICCD_E_COMPORT_ERROR
#define ERR_FLICCD_E_FAILED_RESET                         8005                //ERR_FLICCD_E_FAILED_RESET
#define ERR_FLICCD_E_COMMTIMEOUT                          8006                //ERR_FLICCD_E_COMMTIMEOUT
#define ERR_FLICCD_E_BADDATA                              8007                //ERR_FLICCD_E_BADDATA
#define ERR_FLICCD_E_NOCALIBRATE                          8008                //ERR_FLICCD_E_NOCALIBRATE
#define ERR_FLICCD_E_DEVICE_NOT_CONFIGURED                8009                //ERR_FLICCD_E_DEVICE_NOT_CONFIGUR
#define ERR_FLICCD_E_COMMWRITE                            8010                //ERR_FLICCD_E_COMMWRITE
#define ERR_FLICCD_E_INVALID_DEVICE                       8011                //ERR_FLICCD_E_INVALID_DEVICE
#define ERR_FLICCD_E_FUNCTION_NOT_SUPPORTED               8012                //ERR_FLICCD_E_FUNCTION_NOT_SUPPORTED
#define ERR_FLICCD_E_BAD_BOUNDS                           8013                //ERR_FLICCD_E_BAD_BOUNDS
#define ERR_FLICCD_E_GRABTIMEOUT                          8014                //ERR_FLICCD_E_GRABTIMEOUT
#define ERR_FLICCD_E_TODATAHB                             8015                //ERR_FLICCD_E_TODATAHB
#define ERR_FLICCD_E_TODATALB                             8016                //ERR_FLICCD_E_TODATALB
#define ERR_FLICCD_E_ECPNOTREADY                          8017                //ERR_FLICCD_E_ECPNOTREADY
#define ERR_FLICCD_E_ECPREADTIMEOUTHB                     8018                //ERR_FLICCD_E_ECPREADTIMEOUTHB
#define ERR_FLICCD_E_ECPREADTIMEOUTLB                     8019                //ERR_FLICCD_E_ECPREADTIMEOUTLB
#define ERR_FLICCD_E_ECPREADTIMEOUT                       8020                //ERR_FLICCD_E_ECPREADTIMEOUT
#define ERR_FLICCD_E_ECPREVERSETIMEOUT                    8021                //ERR_FLICCD_E_ECPREVERSETIMEOUT
#define ERR_FLICCD_E_ECPWRITETIMEOUTHB                    8022                //ERR_FLICCD_E_ECPWRITETIMEOUTHB
#define ERR_FLICCD_E_ECPWRITETIMEOUTLB                    8023                //ERR_FLICCD_E_ECPWRITETIMEOUTLB
#define ERR_FLICCD_E_ECPWRITETIMEOUT                      8024                //ERR_FLICCD_E_ECPWRITETIMEOUT
#define ERR_FLICCD_E_FORWARDTIMEOUT                       8025                //ERR_FLICCD_E_FORWARDTIMEOUT
#define ERR_FLICCD_E_NOTECP                               8026                //ERR_FLICCD_E_NOTECP
#define ERR_FLICCD_E_FUNCTIONNOTSUPP                      8027                //ERR_FLICCD_E_FUNCTIONNOTSUPP
#define ERR_FLICCD_E_NODEVICES                            8028                //ERR_FLICCD_E_NODEVICES
#define ERR_FLICCD_E_WRONGOS                              8029                //ERR_FLICCD_E_WRONGOS
#define ERR_TEMMA_RAERROR                                 8030                //Slew/sync error: Temma reports the right ascension is incorrect for go to or synchronization.
#define ERR_TEMMA_DECERROR                                8031                //Slew/sync error: Temma reports the declination is incorrect for go to or synchronization.
#define ERR_TEMMA_TOOMANYDIGITS                           8032                //Slew/sync error: Temma reports the format error for go to or synchronization.
#define ERR_TEMMA_BELOWHORIZON                            8033                //Slew/sync error: Temma reports the object is below the horizon.
#define ERR_TEMMA_STANDBYMODE                             8034                //Slew error: Temma reports the mount is in standby mode.


#define ERR_ACLUNDEFINEDERR                               1                   //ACL undefined error.
#define ERR_ACLSYNTAX                                     2                   //ACL syntax error.


#define ERR_ACLTYPEMISMATCH                               10                  //ACL type mismatch error.
#define ERR_ACLRANGE                                      11                  //ACL range error.
#define ERR_ACLVALREADONLY                                12                  //ACL value is read only.
#define ERR_ACLCMDUNSUPPORTED                             13                  //ACL command is unsupported.
#define ERR_ACLUNSUPPORTID                                14                  //ACL unsupported id.
#define ERR_ACLCMDINACTIVE                                15                  //ACL command inactive.


#define ERR_ACLGOTOILLEGAL                                100                 //ACL illegal go to command.
#define ERR_ACLGOTOBELOWHRZ                               101                 //ACL error: destination is below the horizon.
#define ERR_ACLGOTOLIMITS                                 102                 //ACL go to limit.


#define ERR_NOT_IMPL                                      11000               //This command is not supported.
#define ERR_NOT_IMPL_IN_MODEL                             11001               //This command is not implemented in the model.
#define ERR_OPENING_FOVI_FILES                            11002               //One of the Field of View Indicator database files cannot be found. (Abnormal installation.)
#define ERR_NO_IRIDIUM_SATELLITES                         11003               //No Iridium satellite two-line elements are currently loaded.
#define ERR_ACCESS_DENIED                                 11004               //Access is denied.  Check your username and or password.
#define ERR_ALL_TLES_DATE_REJECTED                        11005               //All TLEs were date rejected, so no satellites will be loaded. Check the date of the TLEs and make sure TheSkyX's date is within 45 days of this date.


#define ERR_SBSCODEBASE                                   1000                //Base offset for creating wire safe scodes


#define ERR_SBIGST7FIRST                                  30000               //SBIG ST7 first error.


#define ERR_SBIGCCCFWFIRST                                31000               //SBIG first cfw error.


#define ERR_CUSTOMAPIFIRST                                1400                //Custom api error code first.


#define ERR_CUSTOMAPILAST                                 1499                //Custom api error code last.
#define ERR_IPLSUITEERR                                   1500                //IPL suite error first


#define ERR_GDIERR_BASE                                   1600                //GDI error base


#define ERR_SBIGTCEEXTFIRST                               1050                //SBIG TCE error first.


#define ERR_SBIGTCEEXTLAST                                1099                //SBIG TCE error last.
#define ERR_SBIGSERIALFIRST                               1100                //SBIG serial error first.


#define ERR_SBIGSERIALLAST                                1125                //SBIG serial error last.


#define ERR_MKS_ERROR_FIRST                               20000               //MKS first error.


#define ERR_MKS_ERROR_LAST                                25000               //MKS last error.


#define ERR_GEMINI_OBJECT_BELOW_HORIZON                   275                 //Gemini - Object below the horizon.
#define ERR_GEMINI_NO_OBJECT_SELECTED                     276                 //Gemini - No object selected.
#define ERR_GEMINI_MANUAL_CONTROL                         277                 //Gemini - Hand paddle is in manual control mode or the Prevent Slews option is turned on.
#define ERR_GEMINI_POSITION_UNREACHABLE                   278                 //Gemini - Position is unreachable.
#define ERR_GEMINI_NOT_ALIGNED                            279                 //Gemini - Gemini not aligned.
#define ERR_GEMINI_OUTSIDE_LIMITS                         280                 //Gemini - Outside slew limits.
#define ERR_GEMINI_VERSION_NOT_SUPPORTED                  281                 //Gemini - Version 4 and later are required. Please update your Gemini firmware.


#define ERR_VIXEN_UNKNOWN                                 290                 //Star Book - Unknown error accessing mount.
#define ERR_VIXEN_URLNOTSET                               291                 //Star Book - The specified URL appears to be invalid.
#define ERR_VIXEN_STATUSINVALID                           292                 //Star Book - No or invalid data received.
#define ERR_VIXEN_STATUSNOTAVAILABLE                      293                 //Star Book - Error reading mount status.
#define ERR_VIXEN_ILLEGALSTATE                            294                 //Star Book - Mount in wrong state to accept this command.
#define ERR_VIXEN_SETRADECERROR                           295                 //Star Book - Error when trying to set RA/Dec.  Make sure the new alignment object is more than 10 degrees from the previous alignment object.
#define ERR_VIXEN_INVALIDFORMAT                           296                 //Star Book - Command incorrectly formatted.
#define ERR_VIXEN_BELOWHORIZON                            297                 //Star Book - Target below the horizon.
#define ERR_VIXEN_HOMEERROR                               298                 //Star Book - Error with HOME command.


#define ENUM_ERR_OPEN_NV_THEME                            11101               //Error opening TheSkyX Night Vision Mode Theme.  Click the Night Vision Mode Setup command on the Display menu and verify that the Night Vision Mode them file name is correct and the theme exists.
#define ENUM_ERR_OPEN_STANDARD_THEME                      11102               //Error opening the Standard Theme.  Click the Night Vision Mode Setup command on the Display menu and verify that the Standard Theme file name is correct and the theme exists.



#endif // SBERRORX_H