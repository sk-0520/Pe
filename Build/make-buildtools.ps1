Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"
Import-Module "${PSScriptRoot}/Modules/Command"


$builToolDirPath = Join-Path -Path (Get-RootDirectory) -ChildPath 'Output' | Join-Path -ChildPath 'tools'

Start-Command -Command dotnet -ArgumentList @('build', 'Source/BuildTools/SqlPack/SqlPack.csproj', '--verbosity', 'normal', '--configuration', 'Debug', '/p:Platform=x64', '--runtime', 'win-x64', '--output', $builToolDirPath, '--no-self-contained')
if (-not $?) {
	throw 'build error: Build Tool'
}

