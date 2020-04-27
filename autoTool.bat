rem @echo off
set BatPath=%~dp0
set exeDir=%BatPath%ConfigTool\bin\Debug\netcoreapp2.0\win10-x64\ConfigTool.exe
set excelDir=%BatPath%ConfigTool\excelDir\
set configDir=%BatPath%ConfigTool\configDir\
set classDir=%BatPath%ConfigTool\classDir\
%exeDir% 1 3 %excelDir% %configDir% %classDir%
@echo on
pause