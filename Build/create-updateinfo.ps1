﻿Param(
	[Parameter(mandatory = $true)][ValidateSet("bitbucket")][string] $TargetRepository,
	[Parameter(mandatory = $true)][version] $MinimumVersion,
	[Parameter(mandatory = $true)][string] $ArchiveBaseUrl,
	[Parameter(mandatory = $true)][string] $NoteBaseUrl,
	[Parameter(mandatory = $true)][string] $ReleaseDirectory,
	[Parameter(mandatory = $true)][ValidateSet("zip", "7z")][string] $Archive,
	[Parameter(mandatory = $true)][string[]] $Platforms
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'command.ps1',
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirPath = Split-Path -Parent $currentDirPath
$outputDirectory = Join-Path $rootDirPath 'Output'

$version = GetAppVersion
$hashAlgorithm = "SHA256"
$releaseTimestamp = (Get-Date).ToUniversalTime()
$revision = (git rev-parse HEAD)

function OutputJson([object] $json, [string] $outputPath) {
	ConvertTo-Json -InputObject $json `
	| ForEach-Object { [Text.Encoding]::UTF8.GetBytes($_) } `
	| Set-Content -Path $outputPath -Encoding Byte
}

function CreateUpdateItem([string] $archiveFilePath, [string] $noteName, [version] $minimumVersion) {
	return @{
		release            = $releaseTimestamp.ToString("s")
		version            = $version
		revision           = $revision
		platform           = $platform
		minimum_version    = $minimumVersion
		note_uri           = $NoteBaseUrl.Replace("@NOTENAME@", $noteName)
		archive_uri        = $ArchiveBaseUrl.Replace("@ARCHIVEAME@", $archiveFilePath)
		archive_size       = (Get-Item -Path $archiveFilePath).Length
		archive_kind       = $Archive
		archive_hash_kind  = $hashAlgorithm
		archive_hash_value = (Get-FileHash -Path $archiveFilePath -Algorithm $hashAlgorithm).Hash
	}
}

# アップデート情報の作成
$updateJson = Get-Content -Path (Join-Path $currentDirPath "update.json") | ConvertFrom-Json
foreach ($platform in $Platforms) {
	$targetName = ConvertAppArchiveFileName $version $platform $Archive
	$targetPath = Join-Path $ReleaseDirectory $targetName

	$item = CreateUpdateItem $targetPath (ConvertReleaseNoteFileName $version) $MinimumVersion

	$updateJson.items += $item
}

$outputUpdateFile = Join-Path $outputDirectory 'update.json'
OutputJson $updateJson $outputUpdateFile
#Get-Content $outputUpdateFile

$pluginProjectDirectoryPath = Join-Path $rootDirPath 'Source/Pe'

$pluginProjectNames = Get-ChildItem -Path $pluginProjectDirectoryPath -Filter ('Pe.Plugins.Reference.*') -Directory | Select-Object -Property Name
foreach($pluginProjectName in $pluginProjectNames) {
	# $lasIndex = $pluginArchiveFile.Basename.LastIndexOf('_')
	# $baseName = $pluginArchiveFile.Basename.Substring(0, $lasIndex);
	# $baseName
	# $targetName = ConvertFileName $baseName $version $platform $archive
	# $targetName
	$items = @()
	foreach ($platform in $Platforms) {
		$pluginFileName = $pluginProjectName.Name + '_' + $platform + '.' + $Archive
		$pluginFilePath = Join-Path $outputDirectory $pluginFileName

		$npteName = (ConvertFileName $pluginProjectName.Name $version '' 'html')
		$item = CreateUpdateItem $pluginFilePath $npteName $version

		$items += $item
	}

	if(0 -lt $items.Count) {
		$pluginFiles = @{
			items = $items
		}
		$outputUpdateFile = Join-Path $outputDirectory ('update-' + $pluginProjectName.Name + '.json')
		OutputJson $pluginFiles $outputUpdateFile
	}
}

switch ($TargetRepository) {
	'bitbucket' {
		$tagJson = @{
			name   = $version
			target = @{
				hash = $revision
			}
		}
		$bitbucketTagApiFile = Join-Path $outputDirectory "bitbucket-tag.json"
		OutputJson $tagJson $bitbucketTagApiFile
		#Get-Content $bitbucketTagApiFile
	}
}
