
VERSION_PATH='Pe/PeMain/Properties/AssemblyInfo.cs'

# �o�[�W������������
#[assembly: AssemblyVersion("0.33.9.*")]
VERSION_REV=`git rev-parse HEAD`
#sed -E "s/^\[\s*assembly\s*:\s*\AssemblyInformationalVersion\s*\(\s*\"\s*(.*)\.(.*)\.(.*)\.(.*)\s*\"\s*\)\s*\]/[assembly: AssemblyVersion(\"\1.\2.\3.$VERSION_REV\")]/" $VERSION_PATH
sed -E -i "s/^\[\s*assembly\s*:\s*\AssemblyInformationalVersion\s*\(\s*\"\s*(revision)\s*\"\s*\)\s*\]/[assembly: AssemblyInformationalVersion(\"$VERSION_REV\")]/" $VERSION_PATH
#sed -E "s/^\[\s*assembly\s*:\s*\AssemblyInformationalVersion\s*\(\s*\"\s*(.*)\.(.*)\.(.*)\.(.*)\s*\"\s*\)\s*\]/[assembly: AssemblyInformationalVersion(\"\1.\2.\3.$VERSION_REV\")]/" $VERSION_PATH

# �r���h
cmd.exe //c  build.bat

# �o�[�W�����߂�
git revert HEAD

read a

