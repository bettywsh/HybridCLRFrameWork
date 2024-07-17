
::cs
XCOPY /y ..\proto\*.proto .\
::XCOPY /y ..\proto\cs\*.proto .\
FOR /F "delims==" %%i IN ('dir /b *.proto') DO (
	@echo %%i
	".\protogen\protogen.exe" --csharp_out=.\cs %%i
)


::拷贝到项目
XCOPY /y .\cs\*.cs .\..\..\client\Assets\Scripts\Hotfix\Protobuf

del/F /S /Q *.proto
::del/F /S /Q .\cs\*.cs
pause