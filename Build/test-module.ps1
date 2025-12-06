Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[switch] $ProductMode,
	[string] $BuildType,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[Parameter(mandatory = $true)][string] $Configuration,
	[string] $Logger,
	[switch] $EnableCoverage
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

if ($Module -eq 'boot') {
	$projectDirItems = Get-TestProjectDirectory -Kind $Module
	$sourceDir = Get-SourceDirectory -Kind $Module

	foreach ($projectDirItem in $projectDirItems) {
		$testDirPath = Join-Path -Path $projectDirItem.FullName -ChildPath 'bin' | Join-Path -ChildPath $Configuration | Join-Path -ChildPath $Platform
		$testFileName = $projectDirItem.BaseName + '.dll'
		$testFilePath = Join-Path -Path $testDirPath -ChildPath $testFileName

		if ($EnableCoverage) {
			# OpenCppCoverageを使用してカバレッジを取得
			$coverageDir = Join-Path -Path $projectDirItem.FullName -ChildPath 'TestResults'
			$coverageFile = Join-Path -Path $coverageDir -ChildPath 'coverage.cobertura.xml'
			
			if (-not (Test-Path $coverageDir)) {
				New-Item -ItemType Directory -Path $coverageDir -Force | Out-Null
			}

			# Pe.LibraryとPe.Bootのソースをカバレッジ対象として指定
			$peLibraryPath = Join-Path -Path $sourceDir -ChildPath 'Pe.Library'
			$peBootPath = Join-Path -Path $sourceDir -ChildPath 'Pe.Boot'
			
			Write-Verbose "OpenCppCoverage with VSTest.Console $testFilePath /InIsolation /Platform:$Platform"
			Write-Verbose "Coverage output: $coverageFile"
			
			& OpenCppCoverage.exe `
				--sources "$peLibraryPath" `
				--sources "$peBootPath" `
				--export_type cobertura:$coverageFile `
				--cover_children `
				-- VSTest.Console.exe $testFilePath /InIsolation /Platform:$Platform
			
			if (-not $?) {
				throw "test error with coverage: $Module"
			}
		} else {
			Write-Verbose "VSTest.Console $testFilePath /InIsolation /Platform:$Platform"
			VSTest.Console $testFilePath /InIsolation /Platform:$Platform
			if (-not $?) {
				throw "test error: $Module"
			}
		}
	}
}elseif ($Module -eq 'main' -or $Module -eq 'plugins') {
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
