set WORKSPACE=.
set LUBAN_DLL=%WORKSPACE%\Luban\Luban.dll
set CONF_ROOT=%WORKSPACE%

dotnet %LUBAN_DLL% ^
    -t server ^
	-c go-json ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputCodeDir=..\..\Sever\dlzs\server\gamedata\cfgcode ^
	-x outputDataDir=..\..\Sever\dlzs\server\gamedata\json ^
	-x lubanGoModule=server/luban
pause