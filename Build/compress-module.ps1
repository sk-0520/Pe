
param(
	[Parameter(mandatory = $true)][string] $TargetDirectory,
	[Parameter(mandatory = $true)][string] $OutputFileBaseName,
	[Parameter(mandatory = $true)][ValidateSet('zip', '7z', 'tar')][string] $Archive
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"
Import-Module "${PSScriptRoot}/Modules/Command"


#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

Write-Verbose "TargetDirectory = $TargetDirectory"
Write-Verbose "OutputFileBaseName = $OutputFileBaseName"

$outputFileName = $OutputFileBaseName + '.' + $Archive

try {
	Push-Location $TargetDirectory

	switch ($Archive) {
		'7z' {
			Start-Command -Command 7z -ArgumentList @('a', '-t7z', '-m0=lzma2', '-mx=9', '-mfb=64', '-md=64m', '-ms=on', '-mmt=on', "$outputFileName", '*', '-r', '-bsp1')
		}
		'zip' {
			Start-Command -Command 7z -ArgumentList @('a', '-tzip', "$outputFileName", '*', '-r', '-bsp1')
		}
		'tar' {
			Start-Command -Command 7z -ArgumentList @('a', '-ttar', "$outputFileName", '*', '-r', '-bsp1')
		}
		Default {
			throw "error: $Archive"
		}
	}

	Move-Item -Path $outputFileName -Destination (Get-RootDirectory) | Out-Null
} finally {
	Pop-Location
}
