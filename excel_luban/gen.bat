set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\luban\Luban.dll
set CONF_ROOT=%WORKSPACE%

dotnet %LUBAN_DLL% ^
    -t all ^
	-c cs-bin ^
    -d bin ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=code ^
	-x outputDataDir=output	
pause