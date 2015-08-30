cd /d %~dp0\..\

for /F "skip=2 tokens=3* delims= " %%a in ('reg query "HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders" /v Desktop') do set DesktopFolder=%%a
for /F "usebackq" %%s in (`echo %DesktopFolder%`) do set DESKTOP=%%s

set BETA=Pe-beta
set BETA_ROOT_DIR=%DESKTOP%\%BETA%
set BETA_DIR=%BETA_ROOT_DIR%\Pe

set RELEASE_DIR=%APPDATA%\Pe

if exist "%BETA_ROOT_DIR%" (
	rem �x�[�^�p�f�B���N�g��������̂ŃR�s��Ȃ�
) else (
	rem �����f�[�^���p�N���Ă���
	mkdir "%BETA_DIR%"  2>NUL
	copy  "%RELEASE_DIR%\*.xml"      "%BETA_DIR%" /Y
	copy  "%RELEASE_DIR%\*.sqlite3"  "%BETA_DIR%" /Y
	copy  "%RELEASE_DIR%\*.gz"       "%BETA_DIR%" /Y
)

start PeMain.exe /setting-root=%BETA_ROOT_DIR% /log=%BETA_ROOT_DIR%\logs /mutex=Pe_beta


