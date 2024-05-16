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
	$projectDirItems = Get-TestProjectDirectory -Kind $Module

	foreach ($projectDirItem in $projectDirItems) {
		$testDirPath = Join-Path -Path $projectDirItem.FullName -ChildPath 'bin' | Join-Path -ChildPath $Configuration | Join-Path -ChildPath $Platform
		$testFileName = $projectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path -Path $testDirPath -ChildPath $testFileName

		if([string]::IsNullOrEmpty($CppTestRunner)) {
			& "${CppTestRunner}" "${testFilePath}" /InIsolation /Platform:$Platform
		} else {
			OpenCppCoverage --sources $projectDirItem.BaseName -- "${CppTestRunner}" "${testFilePath}" /InIsolation /Platform:$Platform
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
