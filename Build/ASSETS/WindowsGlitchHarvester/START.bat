@echo off

taskkill /F /IM WindowsGlitchHarvester.exe > nul 2>&1

start WindowsGlitchHarvester.exe
exit