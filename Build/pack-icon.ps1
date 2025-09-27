Param(
	[Parameter(mandatory = $true)][string] $ArtifactDirectoryPath,
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$types = @(
	'release',
	'debug',
	'beta',
	'plugin'
)

$packedDir = "packed"
if (!(Test-Path $packedDir)) {
    New-Item -ItemType Directory -Path $packedDir | Out-Null
}
foreach ($type in $types) {
	$src = Get-ChildItem -Path $ArtifactDirectoryPath -Recurse -Filter App.ico | Where-Object { $_.FullName -match "@data\\$type\\App.ico$" } | Select-Object -First 1
	if ($src) {
		Copy-Item $src.FullName (Join-Path $packedDir "App-$type.ico")
	}
}
