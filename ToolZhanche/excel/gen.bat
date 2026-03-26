set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\Luban\Luban.dll
set CONF_ROOT=%WORKSPACE%

dotnet %LUBAN_DLL% ^
    -t client ^
	-c cs-simple-json ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=..\..\Client\hsmxw\Assets\Scripts\Hotfix\Config^
	-x outputDataDir=..\..\Client\hsmxw\Assets\App\Config
pause