Param(
    [string]$OutputPath = "Source/Pe/Pe.Main/doc/license/components.json"
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"

Write-Host "ライセンス情報の自動生成を開始します..."

# メインプロジェクトのPackageReferenceを収集（テストプロジェクトは除外）
function Get-PackageReferences {
    $rootDir = Get-RootDirectory
    $projectFiles = Get-ChildItem -Path "$($rootDir.FullName)/Source/Pe" -Filter "*.csproj" -Recurse | 
        Where-Object { $_.Name -notmatch '\.Test\.' -and $_.Name -notmatch '\.Test$' -and $_.Name -ne 'Pe.CommonTest.csproj' }
    
    $packages = @{}
    
    foreach ($projectFile in $projectFiles) {
        Write-Host "プロジェクトを解析中: $($projectFile.Name)"
        
        [xml]$xml = Get-Content $projectFile.FullName
        
        # XPathを使用してPackageReferenceを取得
        $packageRefNodes = $xml.SelectNodes("//PackageReference[@Include]")
        
        foreach ($pkg in $packageRefNodes) {
            $name = $pkg.GetAttribute("Include")
            $version = $pkg.GetAttribute("Version")
            $privateAssets = $pkg.GetAttribute("PrivateAssets")
            
            # PrivateAssetsやビルドツール系を除外
            if ($privateAssets -or $name -match '^(Microsoft\.NET\.Test|coverlet|xunit|Moq)') {
                continue
            }
            
            if ($name -and $version) {
                $packages[$name] = $version
            }
        }
    }
    
    return $packages
}

# NuGet APIからパッケージ情報を取得
function Get-PackageInfo {
    param(
        [string]$PackageName,
        [string]$Version
    )
    
    try {
        $apiUrl = "https://api.nuget.org/v3-flatcontainer/$($PackageName.ToLower())/$Version/$($PackageName.ToLower()).nuspec"
        
        Write-Host "  取得中: $PackageName $Version"
        
        # WebClientを使用してnuspecファイルを取得
        $webClient = New-Object System.Net.WebClient
        $nuspecXml = $webClient.DownloadString($apiUrl)
        
        [xml]$nuspec = $nuspecXml
        $metadata = $nuspec.package.metadata
        
        # ライセンス情報の取得
        $licenseInfo = @{
            name = ""
            uri = ""
        }
        
        try {
            if ($metadata.license) {
                if ($metadata.license.type -eq "expression") {
                    $licenseInfo.name = $metadata.license.InnerText
                    # 一般的なライセンス表記を統一
                    if ($licenseInfo.name -eq "MIT") {
                        $licenseInfo.name = "MIT License"
                    } elseif ($licenseInfo.name -match "Apache-2\.0") {
                        $licenseInfo.name = "Apache License 2.0"
                    }
                } elseif ($metadata.license.type -eq "file") {
                    $licenseInfo.name = "See package"
                }
            } elseif ($metadata.licenseUrl) {
                $licenseInfo.uri = $metadata.licenseUrl
                # ライセンスURLからライセンス名を推測
                if ($metadata.licenseUrl -match "mit") {
                    $licenseInfo.name = "MIT License"
                } elseif ($metadata.licenseUrl -match "apache") {
                    $licenseInfo.name = "Apache License 2.0"
                } elseif ($metadata.licenseUrl -match "bsd") {
                    $licenseInfo.name = "BSD License"
                }
            }
        } catch {
            Write-Warning "    ライセンス情報の解析に失敗: $($_.Exception.Message)"
        }
        
        # プロジェクトURL
        $projectUrl = ""
        if ($metadata.projectUrl) {
            $projectUrl = $metadata.projectUrl
        } elseif ($metadata.repositoryUrl) {
            $projectUrl = $metadata.repositoryUrl
        }
        
        return @{
            name = $PackageName
            uri = $projectUrl
            license = $licenseInfo
            description = if ($metadata.description) { $metadata.description } else { "" }
        }
    }
    catch {
        Write-Warning "  パッケージ情報の取得に失敗: $PackageName - $($_.Exception.Message)"
        return @{
            name = $PackageName
            uri = ""
            license = @{
                name = "Unknown"
                uri = ""
            }
            description = ""
        }
    }
}

# Microsoftパッケージを.NET 9として統合
function Get-DotNetPackageInfo {
    return @{
        name = ".NET 9"
        uri = "https://dotnet.microsoft.com/download/dotnet/9.0"
        license = @{
            name = "MIT License"
            uri = "https://github.com/microsoft/dotnet/blob/master/LICENSE"
        }
        comment = "Microsoft.Extension.*, (MS)System.*, Microsoft.Web.WebView2.* を含む"
    }
}

# メイン処理
$packages = Get-PackageReferences
Write-Host "発見されたパッケージ数: $($packages.Count)"

$libraryList = @()

# Microsoft系パッケージは統合
$microsoftPackages = $packages.Keys | Where-Object { $_ -match '^Microsoft\.' -or ($_ -match '^System\.' -and $_ -ne 'System.Data.SQLite.Core') }
if ($microsoftPackages.Count -gt 0) {
    $libraryList += Get-DotNetPackageInfo
    Write-Host "Microsoft系パッケージを.NET 9として統合: $($microsoftPackages.Count)個"
}

# その他のパッケージを個別に処理
$otherPackages = $packages.Keys | Where-Object { $_ -notmatch '^Microsoft\.' -and ($_ -notmatch '^System\.' -or $_ -eq 'System.Data.SQLite.Core') }

foreach ($packageName in $otherPackages) {
    $version = $packages[$packageName]
    
    # 特別なパッケージの手動ライセンス情報
    if ($packageName -eq "SevenZipExtractor") {
        $libraryItem = @{
            name = "SevenZipExtractor"
            uri = "https://github.com/adoconnection/SevenZipExtractor"
            license = @{
                name = "MIT"
                uri = "https://github.com/adoconnection/SevenZipExtractor/blob/master/LICENSE"
            }
            comment = "7z"
        }
    } elseif ($packageName -eq "Hardcodet.NotifyIcon.Wpf") {
        $libraryItem = @{
            name = "Hardcodet NotifyIcon for WPF"
            uri = "https://github.com/hardcodet/wpf-notifyicon"
            license = @{
                name = "CPOL"
                uri = "https://github.com/hardcodet/wpf-notifyicon/blob/master/LICENSE"
            }
            comment = ""
        }
    } elseif ($packageName -eq "System.Data.SQLite.Core") {
        $libraryItem = @{
            name = "System.Data.SQLite"
            uri = "https://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki"
            license = @{
                name = "Public Domain"
                uri = "https://www.sqlite.org/copyright.html"
            }
            comment = "System.Data.SQLite.Core"
        }
    } elseif ($packageName -eq "NLog.Extensions.Logging") {
        $libraryItem = @{
            name = "NLog"
            uri = "https://nlog-project.org/"
            license = @{
                name = 'BSD 2-Clause "Simplified" License'
                uri = "https://github.com/NLog/NLog.Extensions.Logging/blob/master/LICENSE"
            }
            comment = "NLog.Extensions.Logging"
        }
    } elseif ($packageName -eq "Emoji.Wpf") {
        $libraryItem = @{
            name = "Emoji.Wpf"
            uri = "https://github.com/samhocevar/emoji.wpf"
            license = @{
                name = "WTFPLv2"
                uri = "https://github.com/samhocevar/emoji.wpf/blob/main/COPYING"
            }
            comment = "絵文字"
        }
    } elseif ($packageName -eq "Prism.Wpf") {
        $libraryItem = @{
            name = "Prism"
            uri = "https://github.com/PrismLibrary/Prism"
            license = @{
                name = "MIT License"
                uri = "https://github.com/PrismLibrary/Prism/blob/v7.2.0.1422/LICENSE"
            }
            comment = "Prism.Wpf"
        }
    } else {
        # 通常のNuGet API取得
        $packageInfo = Get-PackageInfo -PackageName $packageName -Version $version
        
        $libraryItem = @{
            name = $packageInfo.name
            uri = $packageInfo.uri
            license = $packageInfo.license
            comment = ""
        }
    }
    
    $libraryList += $libraryItem
}

# xUnit は手動で追加（テストでは使用されているが、メインには含まれないため）
$libraryList += @{
    name = "xUnit.net"
    uri = "https://xunit.net/"
    license = @{
        name = "Apache License 2.0"
        uri = "https://github.com/xunit/xunit"
    }
    comment = "xunit, xunit.runner.visualstudio"
}

# リソース情報（既存の手動管理部分を保持）
$resourceList = @(
    @{
        name = "Visual Studio 2017/2019/2022 Image Library"
        uri = "https://docs.microsoft.com/ja-jp/visualstudio/designers/the-visual-studio-image-library"
        license = @{
            name = "MICROSOFT SOFTWARE LICENSE TERMS"
            uri = "http://msdn.microsoft.com/ja-jp/library/ms246582.aspx"
        }
        comment = ""
    },
    @{
        name = "Material Design Icons"
        uri = "https://materialdesignicons.com"
        license = @{
            name = ""
            uri = ""
        }
        comment = ""
    }
)

# components.json生成
$componentsData = [ordered]@{
    library = $libraryList
    resource = $resourceList
}

$rootDir = Get-RootDirectory
$outputFilePath = Join-Path -Path $rootDir.FullName -ChildPath $OutputPath

# 出力ディレクトリを作成
$outputDir = Split-Path -Parent $outputFilePath
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

Write-Host "出力ファイル: $outputFilePath"

# JSONに変換（読みやすい形式で）
$jsonOutput = $componentsData | ConvertTo-Json -Depth 10
# JSONの整形（タブでインデント）
$jsonOutput = $jsonOutput -replace '  ', "`t"

# BOMなしUTF-8で保存
$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
[System.IO.File]::WriteAllText($outputFilePath, $jsonOutput, $utf8NoBom)

Write-Host "ライセンス情報の生成が完了しました: $outputFilePath"
Write-Host "生成されたライブラリ数: $($libraryList.Count)"