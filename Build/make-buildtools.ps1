Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"
Import-Module "${PSScriptRoot}/Modules/Command"


$builToolDirPath = Join-Path -Path (Get-RootDirectory) -ChildPath 'Output' | Join-Path -ChildPath 'tools'

$projects = @(
	'Source/BuildTools/SqlPack/SqlPack.csproj',
	'Source/BuildTools/License/License.csproj'
)
foreach ($project in $projects) {
	Write-Information "Building Build Tool: $project"
	Start-Command -Command dotnet -ArgumentList @('build', $project, '--verbosity', 'normal', '--configuration', 'Debug', '/p:Platform=x64', '--runtime', 'win-x64', '--output', $builToolDirPath, '--no-self-contained')
}
