Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[switch] $ProductMode,
	[string] $BuildType,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][string] $Configuration,
	[string] $Logger
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

		Write-Verbose "VSTest.Console $testFilePath /InIsolation /Platform:$Platform"
		VSTest.Console $testFilePath /InIsolation /Platform:$Platform
		if (-not $?) {
			throw "test error: $Module"
		}
	}
} elseif ($Module -eq 'main' -or $Module -eq 'plugins') {
	$projectDirItems = Get-TestProjectDirectory -Kind $Module

	if ($Platform -eq 'x86') {
		# For x86 testing, use VSTest.Console to avoid architecture mismatch issues
		foreach ($projectDirItem in $projectDirItems) {
			$testDirPath = Join-Path -Path $projectDirItem.FullName -ChildPath 'bin' | Join-Path -ChildPath $Configuration | Join-Path -ChildPath $Platform
			$testFileName = $projectDirItem.BaseName + '.dll'
			$testFilePath = Join-Path -Path $testDirPath -ChildPath $testFileName

			$vstestArgs = @($testFilePath, '/InIsolation', "/Platform:$Platform")
			if (![string]::IsNullOrEmpty($Logger)) {
				$vstestArgs += "/Logger:$Logger"
			}

			Write-Verbose "VSTest.Console $($vstestArgs -join ' ')"
			VSTest.Console @vstestArgs
			if (-not $?) {
				throw "test error: $Module - $projectDirItem"
			}
		}
	} else {
		# For x64 testing, use dotnet test as before
		$loggerArg = ''
		if (![string]::IsNullOrEmpty($Logger)) {
			$loggerArg = "--logger:$Logger"
		}

		foreach ($projectDirItem in $projectDirItems) {
			Push-Location -Path $projectDirItem
			try {
				dotnet test /p:Platform=$Platform --runtime win-$Platform --configuration $Configuration --collect:"XPlat Code Coverage" --test-adapter-path:. $loggerArg
				if (-not $?) {
					throw "test error: $Module - $projectDirItem"
				}
			} finally {
				Pop-Location
			}
		}
	}
} else {
	throw "module error: $Module"
}
