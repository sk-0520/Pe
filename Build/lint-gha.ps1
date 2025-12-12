param()
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$images = @{
	actionlint = @{
		image = 'rhysd/actionlint'
		version = 'latest'
	}
}

Import-Module "${PSScriptRoot}/Modules/Project"
Import-Module "${PSScriptRoot}/Modules/Command"

$rootDir = Get-RootDirectory

$workflowDir = Join-Path -Path $rootDir -ChildPath '.github/workflows'

$yamlFiles = Get-ChildItem -Path $workflowDir -Filter '*.yml'

Write-Information "actionlint"
foreach ($file in $yamlFiles) {
	$image = "$($images.actionlint.image):$($images.actionlint.version)"
	$path = "/workflows/$($file.Name)"
	Start-Command -Command docker -ArgumentList @('run', '--rm', '--mount', "type=bind,source=${workflowDir},target=/workflows,readonly", $image, $path)
}
