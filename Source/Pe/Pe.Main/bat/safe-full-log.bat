cd /d %~dp0

echo off

set DATA_DIR=safe-data

echo == �Z�[�t���[�h�N�� ==
echo.
echo �e��f�[�^�� %DATA_DIR% �f�B���N�g���ɕۑ�����܂��B
echo ���s��ɕs�v�ł���΍폜���Ă��������B
echo.
echo ���s���̓��e�͂��ׂă��O�ɏo�͂���܂��B
echo.
echo Pe 0.83.0.18060 ����̃f�[�^�������p���ۂ� Pe 0.83.0.18060 ���~���Ă�������
echo �� �t�@�C���̏ꏊ���w�肷��ɂ̓I�v�V�����ǉ����K�v�ł�
echo    --old-setting-root=%APPDATA%
echo                       (�ݒ�f�B���N�g���̐e�f�B���N�g��)
echo.
echo ���s���Ă��܂�...
echo ..\Pe.exe --log .\%DATA_DIR%\logs --force-log --full-trace-log --user-dir .\%DATA_DIR%\user --machine-dir .\%DATA_DIR%\machine --temp-dir .\%DATA_DIR%\temp
..\Pe.exe --log .\%DATA_DIR%\logs --force-log --full-trace-log --user-dir .\%DATA_DIR%\user --machine-dir .\%DATA_DIR%\machine --temp-dir .\%DATA_DIR%\temp

echo.
echo �L�[���͂Ńv�����v�g����܂��B
pause > nul

