cd /d %~dp0

echo off

set DATA_DIR=beta-data

echo == ���ŋN�� ==
echo.
echo �g�p�����͋����I�ɓ��ӂ��ꂽ���̂Ƃ��܂��B
echo.
echo �e��f�[�^�� %DATA_DIR% �f�B���N�g���ɕۑ�����܂��B
echo ���s��ɕs�v�ł���΍폜���Ă��������B
echo.
echo ���s���̓��e�͂��ׂă��O�ɏo�͂���܂��B
echo.
echo ���s���Ă��܂�...
echo ..\Pe.exe --skip-accept --beta-version --log .\%DATA_DIR%\logs --force-log --full-trace-log --user-dir .\%DATA_DIR%\user --machine-dir .\%DATA_DIR%\machine --temp-dir .\%DATA_DIR%\temp
..\Pe.exe --skip-accept --beta-version --log .\%DATA_DIR%\logs --force-log --full-trace-log --user-dir .\%DATA_DIR%\user --machine-dir .\%DATA_DIR%\machine --temp-dir .\%DATA_DIR%\temp

echo.
echo �L�[���͂Ńv�����v�g����܂��B
pause > nul

