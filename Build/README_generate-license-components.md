# ライセンス情報生成ツール

## 概要

このスクリプトは、NuGetパッケージから自動的にライセンス情報を収集し、`components.json`ファイルを生成します。

## 使用方法

```powershell
# デフォルトの出力先に生成
.\generate-license-components.ps1

# カスタム出力先を指定
.\generate-license-components.ps1 -OutputPath "path/to/components.json"
```

## 機能

1. **プロジェクトファイルの解析**: Source/Pe内のすべての非テストプロジェクトを解析し、PackageReferenceを抽出
2. **NuGet API連携**: 各パッケージのライセンス情報をNuGet APIから自動取得
3. **Microsoft系パッケージの統合**: Microsoft系パッケージを.NET 9として統合
4. **手動ライセンス情報**: 特定のパッケージについては手動で正確な情報を提供
5. **JSON生成**: 既存のcomponents.json形式を維持したまま自動生成

## 除外されるパッケージ

- テスト関連パッケージ（xunit, Moq, coverlet.collector など）
- PrivateAssetsが設定されているパッケージ
- ビルドツール系パッケージ

## 統合されるパッケージ

以下のMicrosoft系パッケージは「.NET 9」として統合されます：
- Microsoft.Extensions.*
- System.* （System.Data.SQLite.Coreを除く）
- Microsoft.Web.WebView2

## 特別処理されるパッケージ

以下のパッケージは手動で正確なライセンス情報が設定されます：
- SevenZipExtractor
- Hardcodet.NotifyIcon.Wpf
- System.Data.SQLite.Core
- NLog.Extensions.Logging
- Emoji.Wpf
- Prism.Wpf

## 依存関係

- PowerShell 5.0 以上
- インターネット接続（NuGet APIアクセス用）
- Project.psm1 モジュール（Build/Modules/Project/Project.psm1）

## 実行例

```
PS> .\generate-license-components.ps1
ライセンス情報の自動生成を開始します...
プロジェクトを解析中: Pe.Main.csproj
プロジェクトを解析中: Pe.Core.csproj
...
発見されたパッケージ数: 16
Microsoft系パッケージを.NET 9として統合: 8個
  取得中: AvalonEdit 6.3.1.120
  取得中: Dapper 2.1.66
...
ライセンス情報の生成が完了しました: /path/to/components.json
生成されたライブラリ数: 10
```

## トラブルシューティング

### NuGet APIアクセスエラー
- インターネット接続を確認してください
- 一時的なNuGet APIの問題の場合は、しばらく待ってから再実行してください

### パッケージ情報が不正確
- 特定のパッケージについては、スクリプト内の手動ライセンス情報を更新してください
- 新しいパッケージが追加された場合は、手動処理の追加を検討してください

### 出力ディレクトリが存在しない
- スクリプトが自動的に出力ディレクトリを作成します
- 権限エラーが発生する場合は、適切な権限で実行してください