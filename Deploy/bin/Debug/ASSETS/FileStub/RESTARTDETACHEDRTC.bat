@echo off

taskkill /F /IM FileStub.exe > nul 2>&1

start FileStub.exe
exit