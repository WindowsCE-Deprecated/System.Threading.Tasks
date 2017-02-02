@echo off
setlocal enabledelayedexpansion

set ScriptDir=%~dp0
set AssemblyName=%~1
set RedirectOutput=%~2
set AssemblyOldName=%~3

REM ============================================================================
REM IL Disassembler
REM ============================================================================
set ILDasmBin="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\ildasm.exe"
if not exist %ILDasmBin% (
	set ILDasmBin="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\ildasm.exe"
)
if not exist %ILDasmBin% (
	set ILDasmBin="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\ildasm.exe"
)
if not exist %ILDasmBin% (
	set ILDasmBin="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\ildasm.exe"
)
if not exist %ILDasmBin% (
	echo Error trying to find IL Disassembler executable
	EXIT /B 1
)

REM ============================================================================
REM IL Assembler
REM ============================================================================
set ILAsmBin="C:\Windows\Microsoft.NET\Framework\v4.0.30319\ilasm.exe"
if not exist %ILAsmBin% (
	set ILAsmBin="C:\Windows\Microsoft.NET\Framework\v2.0.50727\ilasm.exe"
)
if not exist %ILAsmBin% (
	echo Error trying to find IL Assembler executable
	EXIT /B 1
)

set keypairFile=%ScriptDir%..\tools\keypair.snk
if not exist %keypairFile% (
	echo Error trying to find key pair file
	EXIT /B 1
)

if "%RedirectOutput%" == "" (
    pushd %ScriptDir%..\%AssemblyName%
) else (
    pushd %ScriptDir%..\Output
)
set CurrentAssemblyName=%AssemblyName%
if NOT "%AssemblyOldName%" == "" (
    set CurrentAssemblyName=%AssemblyOldName%
)

for /f "delims=|" %%f in ('dir /s/b %CurrentAssemblyName%.resources.dll') do (
    set stripfolder=%%~pf
    set stripfolder=!stripfolder:~-22!
    echo Signing file: ...!stripfolder!%%~nf
    del %%~pf%%~nf.il > nul 2>&1
    del %%~pf%%~nf.*.resources > nul 2>&1
    del %%~pf%%~nf.dll.unsigned > nul 2>&1
    %ILDasmBin% %%f /out:%%~pf%%~nf.il > nul || EXIT /B 1
    ren "%%~pf%%~nf.dll" %%~nf.dll.unsigned > nul || EXIT /B 1
    
    if NOT "%AssemblyOldName%" == "" (
        powershell -Command "(gc %%~pf%%~nf.il) -replace '%AssemblyOldName%.resources', '%AssemblyName%.resources' | Out-File %%~pf%%~nf.il"
        
        REM for %%# in (%%~pf*.resources) do (
            REM set "File=%%~nx#"
            REM ren "%%#" "!File:%AssemblyOldName%=%AssemblyName%!"
        REM )
    )
    
    %ILAsmBin% %%~pf%%~nf.il /dll /key=%keypairFile% > nul || EXIT /B 1
    del %%~pf*.il > nul 2>&1
    del %%~pf*.resources > nul 2>&1
    del %%~pf*.unsigned > nul 2>&1
    
    if NOT "%AssemblyOldName%" == "" (
        ren %%~pf%%~nf.dll %AssemblyName%.resources.dll
    )
)
popd
