
VERSION_PATH='Pe/PeMain/Properties/AssemblyInfo.cs'

# �o�[�W������������
#[assembly: AssemblyVersion("0.33.9.*")]
VERSION_REV=`git rev-parse HEAD`
sed -E -i "s/^\[\s*assembly\s*:\s*\AssemblyInformationalVersion\s*\(\s*\"\s*(revision)\s*\"\s*\)\s*\]/[assembly: AssemblyInformationalVersion(\"$VERSION_REV\")]/" $VERSION_PATH

# �r���h
cmd.exe //c  build.bat

# �o�[�W�����߂�
git reset --hard

read a

