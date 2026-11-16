
::cs
::XCOPY /y ..\proto\*.proto .\
::XCOPY /y ..\proto\cs\*.proto .\
FOR /F "delims==" %%i IN ('dir /b *.proto') DO (
	@echo %%i
	".\protogen\protogen.exe" --csharp_out=.\cs %%i
)

del/F /S /Q .\..\..\client\hsmxw\Assets\Scripts\Hotfix\Protobuf\*.cs
::拷贝到项目
XCOPY /y .\cs\*.cs .\..\..\client\hsmxw\Assets\Scripts\Hotfix\Protobuf

::del/F /S /Q *.proto
::del/F /S /Q .\cs\*.cs
pause