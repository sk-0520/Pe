Param(
	[Parameter(mandatory = $true)][string] $ArtifactDirectoryPath,
	[Parameter(mandatory = $true)][string] $OutputDirectoryPath
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$types = @(
	'release',
	'debug',
	'beta',
	'plugin'
)

if (!(Test-Path $OutputDirectoryPath)) {
	New-Item -ItemType Directory -Path $OutputDirectoryPath | Out-Null
}
foreach ($type in $types) {
	$iconPath = Join-Path -Path $ArtifactDirectoryPath -ChildPath "@data" | Join-Path -ChildPath $type | Join-Path -ChildPath "App.ico"
	Copy-Item -Path $iconPath -Destination (Join-Path -Path $OutputDirectoryPath -ChildPath "App-${type}.ico")
}
