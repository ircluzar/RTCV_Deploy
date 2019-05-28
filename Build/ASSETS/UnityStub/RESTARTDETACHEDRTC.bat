@echo off

taskkill /F /IM UnityStub.exe > nul 2>&1

start UnityStub.exe
exit