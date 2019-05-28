@echo off

set filetarget=%1
if not defined filetarget set filetarget=RTC_Launcher.exe

if not exist ..\..\..\%filetarget% goto error

taskkill /F /IM %filetarget%
taskkill /F /FI "RTC Launcher"
copy RTC_Launcher.exe ..\..\..\%filetarget%
cd..
cd..
cd..
start %filetarget%

exit

:error
cls
echo.
echo Error trying to update RTC Launcher:
echo File %filetarget% could not be found
echo.
echo The program will now exit.
echo.
echo.
pause
exit