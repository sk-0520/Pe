# AGENT

AI エージェント用の詳細データ。AI エージェントが自由に更新してよい。
基本方針・プロジェクト構造・アプリケーション仕様は [.github/copilot-instructions.md](../.github/copilot-instructions.md) が正となる。本ファイルはそれを補完する実務情報を記載する。

## 技術スタック

| 領域 | 技術 | バージョン参照元 |
|---|---|---|
| 本体 (`/Source/Pe`) | C# / .NET (WPF), `net10.0-windows` | [/Directory.Build.props](../Directory.Build.props) |
| 起動プログラム (`/Source/Pe.Boot`) | C (標準ライブラリ不使用), テストは C++ | `/Source/Pe.Boot/**/*.vcxproj` |
| ヘルプ・リリースノート (`/Source/Help`, `/Source/ReleaseNote`) | Node.js + TypeScript + Pug + Parcel | [/.node-version](../.node-version) |
| ビルドスクリプト | PowerShell 5 (pwsh ではなく Windows 標準 powershell) | [/Build/](../Build) |

* パッケージ管理: NuGet は [/Directory.Packages.props](../Directory.Packages.props) による集中管理、npm はリポジトリルートの [/package.json](../package.json)
* リンタ・フォーマッタ: `.editorconfig` が基本。TypeScript/JS は biome、HTML は markuplint、文章は textlint、スペルは cspell
* 静的解析: SonarCloud / CodeFactor

## ソリューション構成

スラッシュ区切りのソリューションファイル(`*.slnx`)がルート付近に存在する(ショートカット経由)。

* `Pe.slnx`: 本体
* `Pe.Boot.slnx`: 起動プログラム
* `BuildTools.slnx`: ビルドツール
* `Benchmark.slnx`: ベンチマーク

### `/Source/Pe` 主要プロジェクト

| プロジェクト | 役割 |
|---|---|
| `Pe.Main` | アプリケーション本体 |
| `Pe.Core` | 本体のコア処理 |
| `Pe.Bridge` | プラグインとの橋渡し(プラグイン公開 API) |
| `Pe.Embedded` | 埋め込みリソース関連 |
| `Pe.Library.*` | 汎用ライブラリ群 (Common / Database / DependencyInjection / CommandLine / Property / Provider など) |
| `Pe.Mvvm` | MVVM 基盤 |
| `Pe.PInvoke` | Win32 API 呼び出し |
| `Pe.Generator*` | ソースジェネレータ・アナライザ (Id / Throws) |
| `Pe.Plugins.DefaultTheme` | 標準テーマ |
| `Pe.Plugins.Reference.*` | プラグイン参考実装 (ClassicTheme / Clock / Eyes / FileFinder) |
| `Pe.CommonTest` | テスト共通基盤 |
| `*.Test` | 各プロジェクトに対応する単体テスト |

### `/Source/Pe.Boot` プロジェクト

* `Pe.Boot`: 起動プログラム本体
* `Pe.Library`: 起動プログラム用の C ライブラリ(CRT 非依存の自前実装を含む: `without_crt.c` など)
* `Pe.Script`: スクリプト処理
* `*.Test`: C++ による単体テスト

## よく使うコマンド

いずれもリポジトリルートで実行する。

### 初期セットアップ

```bat
initialize.bat
install-dotnet-tool.bat
```

* .NET ツールは `<Pe>/_tools` にインストールされる

### C# テスト

```powershell
.\Build\local-main-test.ps1 -Project Pe.Main.Test
```

* `Source\Pe` 内の `*.Test` プロジェクト名を指定する
* 複数指定は `,` 区切り、全指定は `*`
* ルートの `-local-test.bat` からも実行可能

### ドキュメントコメント生成 (docfx)

```powershell
.\_tools\docfx docfx.json --template statictoc
```

### Node.js (ヘルプ・リリースノート)

```powershell
npm run lint          # 各種リンタ並列実行 (type/markup/script/text)
npm test              # jest
npm run help-build    # ヘルプ HTML ビルド (Output/help/)
npm run generate-release-note  # リリースノート生成 (history/Pe.html)
```

### ルートのローカル用バッチ

`-local-*.bat` はローカル開発用の補助バッチ(beta ビルド・テスト・lint・ドキュメント生成など)。

## 更新履歴の記載

* [/Define/changelogs.ts](../Define/changelogs.ts) に追記する(型定義は `/Source/Help/types/changelog.ts`)
* Issue 起点の PR では元 Issue を subject にする
* `revision` はリリース前に整理されるため PR 時点では空欄でよい
* 過去分は [/Define/changelogs-archive.ts](../Define/changelogs-archive.ts) に分割されている

## 作業上の注意

* 出力・レビュー・コメントはすべて日本語で記載する
* typo はコード・ファイル名・フォルダ名を問わず指摘する
* PowerShell は Windows 標準の powershell (5.x) を前提とする(`/Source/PowerShell.Test` も同様)
* `Pe.Boot` の C コードは標準ライブラリ(CRT)を使用しない方針のため、安易に CRT 関数を導入しないこと
* シンボリックリンク風ディレクトリ(`_tools`, `_coverage`, `_doc-comment`, `.parcel-cache` など)や `Output/`, `@coverage/` は生成物・ツール領域であり、原則編集対象外
