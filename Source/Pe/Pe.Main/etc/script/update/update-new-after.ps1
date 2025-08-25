﻿# アップデート後に最新版として実施される後処理（ローカルじゃなくて更新モジュールが対象）
[System.Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingWriteHost', '')]
Param(
	[Parameter(mandatory = $true)][System.IO.DirectoryInfo] $DestinationDirectory,
	[Parameter(mandatory = $true)][version] $CurrentVersion,
	[Parameter(mandatory = $true)][ValidateSet("x64")][string] $Platform
)
$ErrorActionPreference = "Stop"

Write-Host "最新アップデート後スクリプト実施!!"

Write-Host "DestinationDirectory: $DestinationDirectory"
Write-Host "CurrentVersion: $CurrentVersion"
Write-Host "Platform: $Platform"

