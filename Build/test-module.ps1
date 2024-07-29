Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[switch] $ProductMode,
	[string] $BuildType,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][string] $Configuration,
	[string] $Logger,
	[string] $CppTestRunner = ''
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

if ($Module -eq 'boot') {
	$testProjectDirItems = Get-TestProjectDirectory -Kind $Module

	foreach ($testProjectDirItem in $testProjectDirItems) {
		$testDirPath = Join-Path -Path $testProjectDirItem.FullName -ChildPath 'bin' | Join-Path -ChildPath $Configuration | Join-Path -ChildPath $Platform
		$testFileName = $testProjectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path -Path $testDirPath -ChildPath $testFileName

		if([string]::IsNullOrEmpty($CppTestRunner)) {
			VSTest.Console "${testFilePath}" /InIsolation /Platform:$Platform
		} else {
			$sourceDirPath = $testProjectDirItem.FullName.SubString(0, $testProjectDirItem.FullName.Length - ".Test".Length)
			#OpenCppCoverage --sources "$($testProjectDirItem.FullName)" -- "${CppTestRunner}" "${testFilePath}" /InIsolation /Platform:$Platform
			#OpenCppCoverage --sources "$($testProjectDirItem.FullName)" -- "${CppTestRunner}" /InIsolation /Platform:$Platform "${testFilePath}"
			OpenCppCoverage --sources "${sourceDirPath}" -- "${CppTestRunner}" /InIsolation /Platform:$Platform "${testFilePath}"
			#OpenCppCoverage --modules "$($testProjectDirItem.FullName)" -- "${testFilePath}"
		}
		if (-not $?) {
			throw "test error: $Module"
		}
	}
} elseif ($Module -eq 'main' -or $Module -eq 'plugins') {
	$loggerArg = ''
	if (![string]::IsNullOrEmpty($Logger)) {
		$loggerArg = "--logger:$Logger"
	}

	$projectDirItems = Get-TestProjectDirectory -Kind $Module


	foreach ($projectDirItem in $projectDirItems) {
		Push-Location -Path $projectDirItem
		try {
			dotnet test /p:Platform=$Platform --runtime win-$Platform --configuration Debug --collect:"XPlat Code Coverage" --test-adapter-path:. $loggerArg
			if (-not $?) {
				throw "test error: $Module - $projectDirItem"
			}
		} finally {
			Pop-Location
		}
	}
} else {
	throw "module error: $Module"
}
