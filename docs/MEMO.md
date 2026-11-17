# MEMO

開発者用のメモ。自由に書き散らかしてよい領域。

> AI エージェントは指示のない限りこのファイルを読み込み専用として扱う。
> (初回の雛形のみ AI が作成)

## TODO / 気になっていること

* [ ] (ここに書く)

## 作業メモ

### YYYY-MM-DD

* (日々のメモをここに)

## クイックリファレンス

### よく使うコマンド

```powershell
# C# テスト (プロジェクト名は , 区切り / 全部は *)
.\Build\local-main-test.ps1 -Project Pe.Main.Test

# docfx によるドキュメントコメント生成
.\_tools\docfx docfx.json --template statictoc

# ヘルプ・リリースノート
npm run lint
npm test
npm run help-build
npm run generate-release-note
```

### リンク集

* リリース: https://github.com/sk-0520/Pe/releases
* 開発版 (nightly)
  * x64: https://nightly.link/sk-0520/Pe/workflows/build-ci-only/next/Pe_x64.zip
  * x86: https://nightly.link/sk-0520/Pe/workflows/build-ci-only/next/Pe_x86.zip
* インストーラ (別プロジェクト): https://github.com/sk-0520/Pe.Installer
* SonarCloud: https://sonarcloud.io/summary/new_code?id=sk-0520_Pe
* CodeFactor: https://www.codefactor.io/repository/github/sk-0520/pe

## 覚え書き

* `.NET` ツールは `<Pe>/_tools` にインストールされる(バージョンはあまり気にしない運用)
* ルート README のテストコマンド記載 `local-main-test-.ps1` は typo(実体は `local-main-test.ps1`)
