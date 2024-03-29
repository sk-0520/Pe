﻿Param(
	[Parameter(mandatory = $true)][string] $ProjectName,
	[Parameter(mandatory = $true)][version] $Version,
	[string] $ReleaseNoteUrl = 'https://excample.com/release?@VERSION@',
	[Parameter(mandatory = $true)][string] $ArchiveBaseUrl,
	[Parameter(mandatory = $true)][string] $ArchiveBaseName,
	[Parameter(mandatory = $true)][ValidateSet('zip')][string] $Archive,
	[Parameter(mandatory = $true)][string] $InputDirectory,
	[Parameter(mandatory = $true)][string] $Destination,
	[Parameter(mandatory = $true)][version] $MinimumVersion,
	[Parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1'
)
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path -Path $currentDirPath -ChildPath $scriptFileName
	. $scriptFilePath
}

$hashAlgorithm = 'SHA256'
$releaseTimestamp = (Get-Date).ToUniversalTime()
$revision = (git rev-parse HEAD)

function Get-VersionText {
	return @(
		'{0}' -f $Version.Major
		'{0:00}' -f $Version.Minor
		'{0:000}' -f $Version.Build
	) -join '.'
}

function Get-UpdateItem {
	param (
		[Parameter(mandatory = $true)][ValidateSet('x86', 'x64')][string] $Platform,
		[Parameter(mandatory = $true)][string] $ArchiveFilePath
	)

	return @{
		release = $releaseTimestamp.ToString('s')
		version = Get-VersionText $Version
		revision = $revision
		platform = $Platform
		minimum_version = Get-VersionText $MinimumVersion
		note_uri = $ReleaseNoteUrl.Replace('@VERSION@', (Get-VersionText))
		archive_uri = $ArchiveBaseUrl.Replace('@ARCHIVENAME@', (Split-Path $ArchiveFilePath -Leaf)).Replace('@VERSION@', (Get-VersionText))
		archive_size = (Get-Item -Path $ArchiveFilePath).Length
		archive_kind = $Archive
		archive_hash_kind = $hashAlgorithm
		archive_hash_value = (Get-FileHash -Path $ArchiveFilePath -Algorithm $hashAlgorithm).Hash
	}
}


$infoItems = @{
	items = @()
}
foreach ($platform in $Platforms) {
	$archiveFilePath = Join-Path -Path $InputDirectory -ChildPath "${ArchiveBaseName}_${platform}.${Archive}"
	$infoItems.items += Get-UpdateItem -Platform $platform -ArchiveFilePath $archiveFilePath
}

ConvertTo-Json -InputObject $infoItems |
	Set-Content -Path $Destination
