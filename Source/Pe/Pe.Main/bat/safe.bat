cd /d %~dp0

echo off

set DATA_DIR=safe-data

echo == �Z�[�t���[�h�N�� ==
echo.
echo �e��f�[�^�� %DATA_DIR% �f�B���N�g���ɕۑ�����܂��B
echo ���s��ɕs�v�ł���΍폜���Ă��������B
echo.
echo ���s���Ă��܂�...
..\Pe.exe --log .\%DATA_DIR%\logs --force-log --user-dir .\%DATA_DIR%\user --machine-dir .\%DATA_DIR%\machine --temp-dir .\%DATA_DIR%\temp

echo.
echo �L�[���͂Ńv�����v�g����܂��B
pause > nul

