@echo off

taskkill /F /IM CemuStub.exe > nul 2>&1

start CemuStub.exe
cd ../RTCV
start START.bat
exit