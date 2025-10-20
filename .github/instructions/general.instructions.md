---
applyTo: '**'
---

* 文章は日本語で出力せよ
* typoなどはコード・ファイル名・フォルダ名に関わらず指摘せよ
* 不明な点があれば、質問を投げかけること
* 可能な限り関連ソースを精査せよ
* PowerShell は最新の pwsh ではなく Windows 標準の powershell を使用する

プロジェクト構造は以下となる

```
Build/
  ビルド処理ツール・スクリプト
Define/
  更新履歴(changelogs.ts)やテーブル構成(table-*.md), プログラム本体にはあまり関係ないが無いとそれはそれで困る
Resource/
  リソースの生データ
Source/Benchmark
  ベンチマーク用のソースコード, 成果物には基本的に関係ないが、nuget などの依存ライブラリは追従させておきたい
Source/BuildTools/
  ビルド時に使用されるプログラム
Source/Help/
  ヘルプドキュメントのソース, 最終的には HTML を出力し、サーバーを使用せずブラウザだけで閲覧するヘルプとなる
Source/Pe/
  本体プログラム
Source/Pe.Boot/
  本体プログラム起動用プログラム
Source/Plugin.Template/
  プラグイン用のテンプレートプロジェクト
Source/PowerShell.Test/
  PowerShell のテスト, PowerShell は windows 10/11 で標準インストールされているバージョン 5 を使用する前提
Source/ReleaseNote/
  リリースノート生成用プログラム
```

特定のファイルについては以下となる

```
Define/changelogs.ts
  更新履歴を TypeScript で定義している
  記述内容は Source/Help/types/changelog.ts を参照のこと
  Issue から PR を作成する場合は元となった Issue が subject になる
  revision はリリース前に整理するので、PR 時点では空欄でよい
```
