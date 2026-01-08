Param(
	[Parameter(mandatory = $true)][string] $BuildToolsLicense,
	[Parameter(mandatory = $true)][string] $OutputFile
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"
Import-Module "${PSScriptRoot}/Modules/Command"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

$sourceDirectory = Get-SourceDirectory -Kind 'main'
$centralPackagePath = Join-Path -Path (Get-RootDirectory) -ChildPath 'Directory.Packages.props'
$componentFilePath = Join-Path -Path $sourceDirectory -ChildPath 'Pe.Main' | Join-Path -ChildPath 'doc' | Join-Path -ChildPath  'component.json'

Write-Information 'License'
if (Test-Path $outputFilePath) {
	Remove-Item -Path $outputFilePath
}
Start-Command -Command "$BuildToolsLicense" -ArgumentList @("--central-package", $centralPackagePath, "--base", $componentFilePath, "--output", $outputFilePath)
