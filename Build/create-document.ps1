Param(
	[parameter(mandatory = $true)][string] $Platform,
	[parameter(mandatory = $true)][string] $OutputDirectory
)
$ErrorActionPreference = 'Stop'
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirectoryPath = Split-Path -Parent $currentDirPath

$documentDirectoryPath = Join-Path $rootDirectoryPath 'Source\Documents'
$outputDirectoryPath = Join-Path $documentDirectoryPath 'build'

try{
	Push-Location $documentDirectoryPath
	npm install
	npm run build

	robocopy /MIR /PURGE /R:3 /S "$outputDirectoryPath" "Output\Release\$Platform\Pe\doc\help"

} finally {
	Pop-Location
}
