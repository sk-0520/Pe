$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$projectFile = Join-Path -Path (Split-Path -Parent $PSScriptRoot | Split-Path -Parent | Split-Path -Parent) -ChildPath 'Source/Pe/Directory.Build.props'

<#
.SYNOPSIS
	アプリケーションのバージョンを取得。

.OUTPUTS
	アプリケーションのバージョン。
#>
function Get-ApplicationVersion {
	[OutputType([version])]
	Param()

	$projectXml = [XML](Get-Content -Path $projectFile -Encoding UTF8)
	$projectNav = $projectXml.CreateNavigator()
	$vesion = $projectNav.Select('/Project/PropertyGroup/Version').Value

	return [version]$vesion
}

<#
.SYNOPSIS
	バージョンを文字列化。

.PARAMETER Version
	バージョン。

.PARAMETER Separator
	結合文字列。
#>
function Convert-Version {
	[OutputType([string])]
	Param(
		[Parameter(mandatory = $true)][version] $Version,
		[Parameter(mandatory = $true)][AllowEmptyString()][string] $Separator
	)

	$values = @(
		'{0}' -f $Version.Major
		'{0:00}' -f $Version.Minor
		'{0:000}' -f $Version.Build
	)
	return $values -join $Separator
}

