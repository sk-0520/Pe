cd /d %~dp0
rem 1. *.tt ��ύX�����ۂɂ��̃o�b�`���N�����ă\�[�X�R�[�h�𐶐�����
rem 2. t4 �̋����Ɉˑ����Ă��邪�o�͊g���q����Őݒ肵�At4���̂� .c.tt, .h.tt �Ƃ��Ĕz�u����
rem 3. ��`�ϐ���ύX����ꍇ�� %CUSTOM_SETTING_FILE% �𓯈�f�B���N�g���ɔz�u���ĕϐ����㏑�����邱��

set CUSTOM_SETTING_FILE=@build-t4-setting.bat

set DevEnvDir=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\

if exist "%CUSTOM_SETTING_FILE%" call "%CUSTOM_SETTING_FILE%"

for /d %%d in (*) do (
	for %%f in (%%d\*.tt) do (
		"%DevEnvDir%TextTransform.exe"  "%%f"
	)
)
