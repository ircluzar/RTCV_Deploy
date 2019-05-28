@echo off

taskkill /F /IM FileStub.exe > nul 2>&1

start FileStub.exe
cd ../RTCV
start START.bat
exit