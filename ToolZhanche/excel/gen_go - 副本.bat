set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\Luban\Luban.dll
set CONF_ROOT=%WORKSPACE%

dotnet %LUBAN_DLL% ^
    -t all ^
	-c go-json ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=..\..\Server\dlzs\server0628\gamedata\cfgcode ^
	-x outputDataDir=..\..\Server\dlzs\server0628\gamedata\json ^
	-x lubanGoModule=server/luban
pause