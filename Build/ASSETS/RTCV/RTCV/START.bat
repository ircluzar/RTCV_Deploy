@echo off

taskkill /F /IM StandaloneRTC.exe > nul 2>&1
taskkill /F /IM WerFault.exe > nul 2>&1

start StandaloneRTC.exe
exit