<#
.SYNOPSIS
Pe プラグインのテンプレート作成処理。


.DESCRIPTION
Pe リポジトリからいい感じのあれこれを取ってきてあれこれするんよ。


.PARAMETER ProjectDirectory
プラグインテンプレートプロジェクトのルートディレクトリパス

.PARAMETER PluginName
プロジェクト名

.PARAMETER PluginId
プラグインID

.PARAMETER DefaultNamespace
名前空間

.PARAMETER AppTargetBranch
対象 Pe のブランチ(原則指定しない, 開発内部的な使用を目的としている)

.PARAMETER AppRevision
対象 Pe のリビジョン(原則指定しない, 開発内部的な使用を目的としている)

.PARAMETER AppRepositoryUrl
対象 Pe のリポジトリURL(原則指定しない, PR で CI から渡されることを想定している)

.PARAMETER GitPath
環境変数PATH に割り当てる git がインストールされているパス

.PARAMETER DotNetPath
環境変数PATH に割り当てる dotnet がインストールされているパス

.LINK
https://github.com/sk-0520/Pe
#>
# Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
[CmdletBinding(SupportsShouldProcess)]
param(
	[Parameter(mandatory = $true)][string] $ProjectDirectory,
	[Parameter(mandatory = $true)][string] $PluginName,
	[Guid] $PluginId,
	[string] $DefaultNamespace,
	[string] $AppTargetBranch = 'master',
	[string] $AppRevision = '',
	[string] $AppRepositoryUrl = '',
	[string] $GitPath = '%PROGRAMFILES%\Git\bin',
	[string] $DotNetPath = '%PROGRAMFILES%\dotnet\'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$suppressBuild = $false
$suppressScm = $false

#===================================================
#プラグインIDのチェック・採番
class PluginIdentity {
	[Guid] $PluginId
	[string] $PluginName

	PluginIdentity([Guid] $pluginId, [string] $pluginName) {
		$this.PluginId = $pluginId
		$this.PluginName = $pluginName
	}
}
$reservedPluginIds = [PluginIdentity[]]@(
	[PluginIdentity]::new('4524FC23-EBB9-4C79-A26B-8F472C05095E', 'ContentTypeTextNet.Pe.Plugins.DefaultTheme'),
	# ------------------------------------------
	[PluginIdentity]::new('67F0FA7D-52D3-4889-B595-BE3703B224EB', 'ContentTypeTextNet.Pe.Plugins.Reference.ClassicTheme'),
	[PluginIdentity]::new('2E5C72C5-270F-4B05-AFB9-C87F3966ECC5', 'ContentTypeTextNet.Pe.Plugins.Reference.Clock'),
	[PluginIdentity]::new('799CE8BD-8F49-4E8F-9E47-4D4873084081', 'ContentTypeTextNet.Pe.Plugins.Reference.Eyes'),
	[PluginIdentity]::new('9DCF441D-9F8E-494F-89C1-814678BBC42C', 'ContentTypeTextNet.Pe.Plugins.Reference.FileFinder'),
	[PluginIdentity]::new('4FA1A634-6B32-4762-8AE8-3E1CF6DF9DB1', 'ContentTypeTextNet.Pe.Plugins.Reference.Html')
)

$customPluginId = $PluginId
$enabledGuid = $false
while (!$enabledGuid) {
	$guid = [System.Guid]::empty
	$isGuid = [System.Guid]::TryParse($customPluginId, [System.Management.Automation.PSReference]$guid)

	if ($isGuid) {
		$reservedIds = $reservedPluginIds | Select-Object -ExpandProperty 'PluginId'
		if ($reservedIds.Contains([Guid]$customPluginId)) {
			Write-Warning ('予約済みプラグインID: ' + $customPluginId)
		} else {
			$enabledGuid = $true
		}
	} else {
		Write-Warning ('プラグインIDとして不正: ' + $customPluginId)
	}

	if (!$enabledGuid) {
		$generatedGuid = $false
		do {
			Write-Information 'プラグインIDを生成方法: [1] 自動, [2] 手動'
			switch ((Read-Host)) {
				1 {
					$customPluginId = New-Guid
					$generatedGuid = $true
				}
				2 {
					$customPluginId = Read-Host -Prompt 'GUID入力'
					$generatedGuid = $true
				}
			}
		} while (!$generatedGuid)
	}
}

$reservedNames = $reservedPluginIds | Select-Object -ExpandProperty 'PluginName'
if ($reservedNames.Contains($PluginName)) {
	throw ('予約済みプラグイン名: ' + $PluginName)
}

#===================================================

function Start-Command {
	[CmdletBinding(SupportsShouldProcess = $true)]
	param (
		[Parameter(Mandatory = $true)][string] $Command,
		[Parameter(Mandatory = $false)][string[]] $ArgumentList
	)

	if ($PSCmdlet.ShouldProcess($Command, $ArgumentList)) {
		# $?/$LASTEXITCODE はコマンドに左右されるので呼び出し側で判定する
		& $Command $ArgumentList
	} else {
		Write-Verbose ('[DRY] 実行予定: {0} {1}' -f $Command, $ArgumentList)
	}
}

function Start-Git {
	[CmdletBinding(SupportsShouldProcess = $true)]
	param (
		[Parameter(Mandatory = $true)][string[]] $ArgumentList
	)
	Start-Command -Command $parameters.git -ArgumentList $ArgumentList
	Write-Verbose "git: $LASTEXITCODE"
	if ($LASTEXITCODE -ne 0) {
		throw "git ${ArgumentList}: $LASTEXITCODE"
	}
}

function Start-DotNet {
	[CmdletBinding(SupportsShouldProcess = $true)]
	param (
		[Parameter(Mandatory = $true)][string[]] $ArgumentList
	)
	Start-Command -Command $parameters.dotnet -ArgumentList $ArgumentList
	Write-Verbose "dotnet: $LASTEXITCODE"
	if ($LASTEXITCODE -ne 0) {
		throw "dotnet ${ArgumentList}: $LASTEXITCODE"
	}
}

#===================================================

# 各種諸々の生成
$parameters = @{
	pluginName = $PluginName
	directory = [System.IO.DirectoryInfo][Environment]::ExpandEnvironmentVariables($ProjectDirectory)
	git = [Environment]::ExpandEnvironmentVariables((Join-Path -Path $GitPath -ChildPath 'git.exe'))
	dotnet = [Environment]::ExpandEnvironmentVariables((Join-Path -Path $DotNetPath -ChildPath 'dotnet.exe'))
	pluginId = $customPluginId
	source = [System.IO.DirectoryInfo][Environment]::ExpandEnvironmentVariables((Join-Path -Path $ProjectDirectory -ChildPath 'Source'))
	repository = @{
		application = @{
			path = 'Source/Pe'
			url = if (![string]::IsNullOrEmpty($AppRepositoryUrl)) {
				[uri]$AppRepositoryUrl
			} elseif ($AppTargetBranch -eq 'ci-test') {
				# 通常フローでここに入ることはない
				# ci-test 処理でリリース処理試験を行う場合のみ通る想定
				[uri]'https://github.com/sk-0520/Pe_ci-test'
			} else {
				[uri]'https://github.com/sk-0520/Pe.git'
			}
		}
	}
}

Write-Verbose 'パラメータ:'
Write-Verbose "`tProjectDirectory: ${ProjectDirectory}"
Write-Verbose "`tPluginId: ${PluginId}"
Write-Verbose "`tAppTargetBranch: ${AppTargetBranch}"
Write-Verbose "`tGitPath: ${GitPath}"
Write-Verbose "`tDotNetPath: ${DotNetPath}"

Write-Information '情報:'
$parameters | Format-Table -AutoSize
Write-Information ('git: ' + (Start-Git -ArgumentList --version))
Write-Information ('dotnet: ' + (Start-DotNet -ArgumentList --version))

#---------------------------------------------------


function Convert-TemplateValue {
	[OutputType([string])]
	param(
		[string] $Value
	)

	return [Regex]::Replace($Value, '\bTEMPLATE_([\w\d_]+)\b', {
			$namespace = 'TEMPLATE_Namespace'
			if (![string]::IsNullOrEmpty($DefaultNamespace)) {
				$namespace = $DefaultNamespace.Trim()
			}

			$pluginShortName = $parameters.pluginName
			if ($parameters.pluginName.Contains('.')) {
				$pluginShortName = $parameters.pluginName.Split('.')[-1]
			}

			$map = @{
				'Namespace' = $namespace
				'PluginName' = $parameters.pluginName
				'PluginShortName' = $pluginShortName
				'PluginId' = $parameters.pluginId
			}

			return $map[$args.Groups[1].Value]
		})
}

function Update-TemplateFileContent {
	[CmdletBinding(SupportsShouldProcess)]
	param(
		[Parameter(Mandatory = $true)][System.IO.FileInfo] $File
	)

	Write-Verbose $File.FullName

	$encoding = switch ($File.Extension.ToLowerInvariant()) {
		'.bat' {
			'oem'
		}
		Default {
			'UTF8'
		}
	}

	$newContents = Get-Content -Path $File.FullName -Encoding $encoding | ForEach-Object { Convert-TemplateValue -Value $_ }
	if ($PSCmdlet.ShouldProcess('File', "$($File.FullName) のテンプレート文字列を置き換え")) {
		Set-Content -Path $File.FullName -Value $newContents -Encoding $encoding
	} else {
		Write-Verbose "`[DRY`] 書き換え後文字列: $newContents"
	}
}

function Rename-TemplateFileName {
	param(
		[Parameter(Mandatory = $true)][System.IO.DirectoryInfo] $ParentDirectory,
		[Parameter(Mandatory = $true)][string] $Name
	)

	$newName = Convert-TemplateValue -Value $Name
	if ($newName.StartsWith('__.')) {
		$newName = $newName.SubString(2)
	}
	if ($Name -ne $newName) {
		Write-Verbose "Rename-TemplateFileName: [$($ParentDirectory.FullName)] $Name -> $newName"
		$src = (Join-Path -Path $ParentDirectory.FullName -ChildPath $Name)
		$dst = (Join-Path -Path $ParentDirectory.FullName -ChildPath $newName)
		Move-Item -Path $src -Destination $dst
	}
}

function Rename-Directory([System.IO.DirectoryInfo] $directory) {
	Write-Verbose "Rename-Directory: $($directory.FullName)"
	$dirs = Get-ChildItem -Path $directory.FullName -Directory
	foreach ($dir in $dirs) {
		Rename-Directory($dir)
		Rename-TemplateFileName -ParentDirectory $directory -Name $dir.Name
	}

	$files = Get-ChildItem -Path $directory.FullName -File
	foreach ($file in $files) {
		Update-TemplateFileContent -File $file
		Rename-TemplateFileName -ParentDirectory $directory -Name $file.Name
	}
}

#===================================================
# プロジェクトディレクトリ構築
$parameters.directory.Refresh()
if (!$parameters.directory.Exists) {
	$parameters.directory.Create()
}
if ((Get-ChildItem -Path $parameters.directory -Recurse -Force | Measure-Object).Count -ne 0) {
	throw '指定ディレクトリが空じゃない'
}

Write-Information "プロジェクトディレクトリ生成: $($parameters.directory)"
if (!$suppressScm) {
	Start-Git -ArgumentList init, $parameters.directory
}

Copy-Item -Path (Join-Path -Path $PSScriptRoot -ChildPath 'Template\*') -Destination ($parameters.directory.FullName + '\') -Force -Recurse

Rename-Directory $parameters.directory

function New-Submodule {
	[CmdletBinding(SupportsShouldProcess)]
	param (
		[string] $Path,
		[uri] $Uri,
		[string] $Branch,
		[string] $Revision
	)

	try {
		Push-Location $parameters.directory

		$targetPath = Join-Path -Path $parameters.directory -ChildPath $Path
		Write-Information "サブモジュール親ディレクトリ生成: $targetPath"
		if ($PSCmdlet.ShouldProcess('Path', "$Path のテンプレート文字列を置き換え")) {
			Start-Git -ArgumentList submodule, add, --branch, $Branch, $Uri, $Path

			if (${Revision}) {
				Push-Location -LiteralPath $Path
				if (![string]::IsNullOrEmpty($Branch)) {
					$currentBranch = Start-Git -ArgumentList @('-C', '.', 'rev-parse', '--abbrev-ref', 'HEAD')
					if ($currentBranch -eq $Branch) {
						Start-Git -ArgumentList reset, --hard, "${Revision}"
					} else {
						Start-Git -ArgumentList branch, --force, $Branch, "${Revision}"
						Start-Git -ArgumentList checkout, $Branch
					}
				} else {
					Start-Git -ArgumentList checkout, "${Revision}"
				}
				Pop-Location
				Start-Git -ArgumentList add, .
			}
		} else {
			Write-Verbose "`[DRY`] $($parameters.git) submodule add --branch $Branch $Uri $Path"
		}

	} finally {
		Pop-Location
	}
}

# Pe をサブモジュールとしてとってくる
if (!$suppressScm) {
	New-Submodule -Path $parameters.repository.application.path -Uri $parameters.repository.application.url -Branch $AppTargetBranch -Revision $AppRevision
}

try {
	Push-Location $parameters.source

	$solutionFileName = "${PluginName}.slnx"
	$solutionPathName = Join-Path -Path $parameters.source -ChildPath $solutionFileName

	Write-Verbose 'Peを追加'
	$appDir = Join-Path -Path $parameters.source -ChildPath 'Pe' | Join-Path -ChildPath 'Source' | Join-Path -ChildPath 'Pe'
	$items = @(
		@{
			project = 'Pe.Generator.Id'
			directory = 'Pe\generator'
		},
		@{
			project = 'Pe.Generator.Exception'
			directory = 'Pe\generator'
		},
		@{
			project = 'Pe.Bridge'
			directory = 'Pe\bridge'
		},
		@{
			project = 'Pe.Embedded'
			directory = 'Pe\bridge'
		},
		@{
			project = 'Pe.Library.Args'
			directory = 'Pe\lib\library'
		},
		@{
			project = 'Pe.Library.Provider'
			directory = 'Pe\lib\library'
		},
		@{
			project = 'Pe.Library.Common'
			directory = 'Pe\lib\library'
		},
		@{
			project = 'Pe.Library.Property'
			directory = 'Pe\lib\library'
		},
		@{
			project = 'Pe.Library.Database'
			directory = 'Pe\lib\library'
		},
		@{
			project = 'Pe.Library.Database.Sqlite'
			directory = 'Pe\lib\library'
		},
		@{
			project = 'Pe.Library.DependencyInjection'
			directory = 'Pe\lib\library'
		},
		@{
			project = 'Pe.Mvvm'
			directory = 'Pe\lib'
		},
		@{
			project = 'Pe.PInvoke'
			directory = 'Pe\lib'
		},
		@{
			project = 'Pe.Core'
			directory = 'Pe\lib'
		},
		@{
			project = 'Pe.Plugins.DefaultTheme'
			directory = 'Pe\lib\plugins'
		},
		@{
			project = 'Pe.Main'
			directory = 'Pe'
		}
	)
	foreach ($item in $items) {
		$projectFilePath = Join-Path -Path $appDir -ChildPath $item.project | Join-Path -ChildPath ($item.project + '.csproj')
		Start-DotNet -ArgumentList sln, add, $projectFilePath, --solution-folder, $item.directory
	}

	# <Project> の子要素 <Platform> が追加されないプロジェクトを補正
	$fixPlatformPaths = @(
		'Pe/Source/Pe/Pe.Generator.Id/Pe.Generator.Id.csproj'
	)
	$solutionXml = [xml]::new()
	$solutionXml.Load($solutionPathName)
	foreach ($fixPlatformPath in $fixPlatformPaths) {
		$projectNode = $solutionXml.SelectSingleNode("//Project[@Path='$fixPlatformPath']")
		$platforms = @('x86', 'x64')
		foreach ($platform in $platforms) {
			$platformNode = $solutionXml.CreateElement('Platform')
			$platformNode.SetAttribute('Solution', "*|$platform")
			$platformNode.SetAttribute('Project', $platform)
			$projectNode.AppendChild($platformNode) | Out-Null
		}
	}
	$solutionXml.Save($solutionPathName)

	# プラグインプロジェクトを補正・追加
	$pluginTargets = @(
		Join-Path -Path $parameters.source -ChildPath $PluginName
		Join-Path -Path $parameters.source -ChildPath "${PluginName}.Test"
	)

	$packagePath = Join-Path -Path $parameters.source -ChildPath 'Pe' | Join-Path -ChildPath 'Directory.Packages.props'
	$packageVersions = Select-Xml -Path $packagePath -XPath '//PackageVersion'

	$packageMap = @{}
	foreach ($packageVersion in $packageVersions) {
		$i = $packageVersion.Node.GetAttribute('Include')
		$v = $packageVersion.Node.GetAttribute('Version')
		Write-Verbose "${i}: ${v}"
		$packageMap[$i] = $v
	}

	foreach ($pluginTarget in $pluginTargets) {
		# プロジェクトの NuGet パッケージバージョンを Pe に合わせる
		$pluginProject = Join-Path -Path $pluginTarget -ChildPath ((Split-Path -Path $pluginTarget -Leaf) + '.csproj')
		$projectXml = [xml]::new()
		$projectXml.Load($pluginProject)

		$isUpdate = $false
		$nodes = $projectXml.SelectNodes('//PackageReference')
		foreach ($node in $nodes) {
			$packageName = $node.GetAttribute('Include')
			$packageVersion = $node.GetAttribute('Version')
			if ($packageMap.ContainsKey($packageName) -and ($packageVersion -ne $packageMap[$packageName])) {
				$version = $packageMap[$packageName]
				Write-Verbose "`t${packageName}: ${packageVersion} -> ${version}  [変更あり]"
				$node.SetAttribute('Version', $version)
				$isUpdate = $true
			} else {
				Write-Verbose "`t${packageName}: ${packageVersion} [変更なし]"
			}
		}
		if ($isUpdate) {
			Write-Verbose "プロジェクトのパッケージを更新: $pluginTarget"
			$projectXml.Save($pluginProject)
		}

		Write-Verbose "プロジェクトを追加: $pluginTarget"
		Start-DotNet -ArgumentList sln, add, $pluginTarget
	}


	# Write-Verbose "ソリューションからAnyCPUを破棄"
	# $solutionFileName = Convert-TemplateValue 'TEMPLATE_PluginShortName.slnx'
	# $solutionContent = Get-Content -Path $solutionFileName `
	# 	| Out-String -Stream `
	# 	| Where-Object { !$_.Contains('Any CPU') }
	# Set-Content -Path $solutionFileName -Value $solutionContent

	# スタートアップ設定
	$solutionXml.Load($solutionPathName)
	foreach ($fixPlatformPath in $fixPlatformPaths) {
		$projectNode = $solutionXml.SelectSingleNode("//Project[@Path='${PluginName}/${PluginName}.csproj']")
		$projectNode.SetAttribute('DefaultStartup', 'true')
	}
	$solutionXml.Save($solutionPathName)

	Write-Verbose 'NuGet 復元'
	if (!$suppressBuild) {
		Start-DotNet -ArgumentList restore
	}

	Write-Verbose 'プラグイン起動設定追加'
	$pluginPropertyDir = Join-Path -Path $parameters.source -ChildPath $PluginName | Join-Path -ChildPath 'Properties'
	Copy-Item -Path (Join-Path -Path $pluginPropertyDir -ChildPath 'dev-launchSettings.json') -Destination (Join-Path -Path $pluginPropertyDir -ChildPath 'launchSettings.json')

	Write-Verbose 'アプリケーションデバッグ構成ファイル適用'
	$appEtcDir = Join-Path -Path $appDir -ChildPath 'Pe.Main' | Join-Path -ChildPath 'etc'
	Copy-Item -Path (Join-Path -Path $appEtcDir -ChildPath '@appsettings.debug.json') -Destination (Join-Path -Path $appEtcDir -ChildPath 'appsettings.debug.json')

	Write-Verbose 'ソリューションのショートカットをルートに作成'
	$wsShell = New-Object -ComObject WScript.Shell
	$shortcut = $wsShell.CreateShortcut((Join-Path -Path $parameters.directory -ChildPath ($parameters.pluginName + '.lnk')))
	$shortcut.TargetPath = $solutionPathName
	$shortcut.Save()

	if (!$suppressBuild) {
		Write-Verbose 'とりあえずのデバッグ全ビルド'
		Start-DotNet -ArgumentList build, --configuration, Debug, /p:Platform=x64
	}

	if (!$suppressScm) {
		Write-Verbose 'はいコミット'
		Start-Git -ArgumentList add, --all
		Start-Git -ArgumentList commit, --message, "initialize $PluginName"
	}
} finally {
	Pop-Location
}
