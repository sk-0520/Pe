Param(
	[Parameter(mandatory = $true)][ValidateSet('boot', 'main', 'plugins')][string] $Module,
	[switch] $ProductMode,
	[string] $BuildType,
	[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
	[switch] $Test
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"
Import-Module "${PSScriptRoot}/Modules/Command"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

# ビルド開始
$defines = @()
if ( $BuildType ) {
	$defines += $BuildType
}
if ( $ProductMode ) {
	$defines += 'PRODUCT'
}
# ; を扱う https://docs.microsoft.com/ja-jp/visualstudio/msbuild/msbuild-special-characters?view=vs-2015&redirectedfrom=MSDN
$define = $defines -join '%3B'

Write-Information "define: $define"

if ($Module -eq 'boot') {
	$configuration = 'Release'
	if ($Test) {
		$configuration = 'CI_TEST'
	}
	Start-Command -Command msbuild -ArgumentList @((Join-Path -Path (Get-SourceDirectory -Kind 'boot') -ChildPath 'Pe.Boot.slnx'), '/m', "/p:Configuration=$configuration", "/p:Platform=$Platform", "/p:DefineConstants=$define")
} elseif ($Module -eq 'main') {
	Start-Command -Command dotnet -ArgumentList @('publish', (Join-Path -Path (Get-SourceDirectory -Kind $Module) -ChildPath 'Pe.Main/Pe.Main.csproj'), '/m', '--verbosity', 'normal', '--configuration', 'Release', "/p:Platform=$Platform", "/p:DefineConstants=$define", '--runtime', "win-$Platform", '--output', "Output/Release/$Platform/Pe/bin", '--self-contained', 'true')
} elseif ($Module -eq 'plugins') {
	# プラグイン参考実装
	$pluginProjectFiles = Get-ApplicationProjectDirectory -Kind $Module |
		Get-ChildItem -File -Recurse -Include '*.csproj'

	if (($pluginProjectFiles | Measure-Object).Count -eq 0) {
		throw "build error: $Module - 0 build - " + (Get-ProjectDirectory -Kind $Module)
	}

	foreach ($pluginProjectFile in $pluginProjectFiles) {
		$name = $pluginProjectFile.BaseName

		Start-Command -Command dotnet -ArgumentList @('publish', $pluginProjectFile, '/m', '--verbosity', 'normal', '--configuration', 'Release', "/p:Platform=$Platform", "/p:DefineConstants=$define", '--runtime', "win-$Platform", '--output', "Output/Release/$Platform/Plugins/$name", '--self-contained', 'false')
	}
} else {
	throw "module error: $Module"
}
#$projectFiles = (Get-ChildItem -Path "Source\Pe\" -Recurse -Include *.csproj)


