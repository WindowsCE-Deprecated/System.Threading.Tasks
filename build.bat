@echo off

set SolutionDir=%~dp0
set SolutionName=Tasks
set Project=System.Threading.Tasks
set ProjectDotNet=src\%Project%.WindowsCE

REM Cleanup output directory
rmdir /s/q "%SolutionDir%Output" 2> nul
mkdir "%SolutionDir%Output"

REM CALL %SolutionDir%tools\build-wince.bat %SolutionDir% %SolutionName% %Project% || EXIT /B 1
CALL %SolutionDir%tools\build-dotnet.bat %SolutionDir% %ProjectDotNet% net35-cf || EXIT /B 1

REM Rename generated resources
REM ren "%SolutionDir%Output\net35-cf\pt\System.Threading.Tasks.WindowsCE.resources.dll" System.Threading.Tasks.resources.dll

CALL %SolutionDir%tools\sign-resources.cmd %Project% true %Project%.WindowsCE || EXIT /B 1

echo build complete.
echo.
EXIT /B %ERRORLEVEL%
