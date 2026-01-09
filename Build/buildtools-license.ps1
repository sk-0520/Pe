Param(
	[Parameter(mandatory = $true)][string] $BuildToolsLicense,
	[Parameter(mandatory = $true)][string] $InputFile,
	[Parameter(mandatory = $true)][string] $OutputFile
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"
Import-Module "${PSScriptRoot}/Modules/Command"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

$centralPackagePath = Join-Path -Path (Get-RootDirectory) -ChildPath 'Directory.Packages.props'

Write-Information 'License'
if (Test-Path $OutputFile) {
	Remove-Item -Path $OutputFile
}
Start-Command -Command "$BuildToolsLicense" -ArgumentList @("--central-package", $centralPackagePath, "--base", $InputFile, "--output", $OutputFile)
