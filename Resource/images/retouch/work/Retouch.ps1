Param(
	[Parameter(Mandatory)]
	[ValidateNotNullOrWhiteSpace()]
	[string] $SourcePath,
	[Parameter(Mandatory)]
	[ValidateNotNullOrWhiteSpace()]
	[string] $OutputPath,
	[Parameter(Mandatory)]
	[ValidatePattern("^#[0-9a-f]{6}$")]
	[string] $Color
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$appXml = [xml](Get-Content $SourcePath -Raw -Encoding UTF8)

$nodes = @(
	$appXml.SelectSingleNode('//*[@id="backgroundLinearGradientStart"]')
	$appXml.SelectSingleNode('//*[@id="backgroundLinearGradientEnd"]')
)

$outputParentDirPath = Split-Path -Path $OutputPath -Parent
if(! (Test-Path -Path $outputParentDirPath -PathType Container)) {
	New-Item -Path $outputParentDirPath -ItemType Directory
}

foreach ($node in $nodes) {
	$style = $node.Attributes['style'].Value
	$node.Attributes['style'].Value = $style -replace 'stop-color:(#[0-9a-f]{6})', "stop-color:${Color}"
}
$appXml.Save($OutputPath)
