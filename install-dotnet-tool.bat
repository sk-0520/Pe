cd /d %~dp0

set INSTALL_DIR=_tools

dotnet tool install dotnet-reportgenerator-globaltool --tool-path "%INSTALL_DIR%"
dotnet tool install docfx --tool-path "%INSTALL_DIR%"

