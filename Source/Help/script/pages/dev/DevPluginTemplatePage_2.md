# デバッグ方法

テンプレートを使用した場合はVisual Studioのデバッグ実行で問題なく処理できる想定。

出来ない場合は以下のパラメータを付与したデバッグ実行を行えばできるはず。

`--debug-dev-mode --log=$(SolutionDir)data\log --force-log --full-trace-log --user-dir=$(SolutionDir)data\user --machine-dir=$(SolutionDir)data\machine --temp-dir=$(SolutionDir)data\temp  --test-plugin-name $(TargetName) --test-plugin-dir ".\$(ProjectName)\bin\$(Platform)\$(Configuration)/$(TargetFramework)"`

# 各種ファイル群

<dl>
  <dt>`Build\build-project.ps1`</dt>
  <dd>プロジェクトのビルド処理</dd>
  
  <dt>`Build\archive-plugin.ps1`</dt>
  <dd>ビルドされたプラグインを圧縮</dd>
  
  <dt>`Build\create-info.ps1`</dt>
  <dd>アーカイブをもとにプラグイン情報を生成</dd>
  
  <dt>`FullBuild.ps1`</dt>
  <dd>Buildディレクトリ内スクリプトを使用したビルド一括処理</dd>
</dl>

## FullBuild.ps1

あらかじめ用意しているビルド処理で基本的にこれで自動ビルドに対応できる。

以下は最低限書き換えが必要な個所。

<dl>
  <dt>`$minimumVersion`</dt>
  <dd>実行可能 Pe 最低バージョン</dd>
  
  <dt>`$archiveBaseUrl`</dt>
  <dd>配布アーカイブURL<br />`@ARCHIVENAME@` がアーカイブ名に置き換えられる</dd>
  
  <dt>`$releaseNoteUrl`</dt>
  <dd>いらんねんこれ<br />`@VERSION@` がバージョン値に置き換えられる</dd>
</dl>