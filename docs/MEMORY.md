# MEMORY

AI エージェント用メモリ。AI エージェントが自由に追記・整理してよい。

## 運用ルール

* セッションをまたいで引き継ぎたい知見・決定事項・ハマりどころを記録する
* 1 エントリ 1 事実で簡潔に書く(日付・関連パス付き)
* 古くなった情報は削除または修正してよい

## 記録

### 2026-11-08

* `docs/` ディレクトリを新規構築(MEMO.md / AGENT.md / MEMORY.md)
* ルート README の typo `local-main-test-.ps1` は開発者により修正済み(現在は `Build/local-main-test.ps1` と一致)
* 主要処理に対する拡張メソッドは利便性と責務分離のために使用する。特に IO・DB では責務分離が重要([IO の例](../Source/Pe/Pe.Library.Provider/FileSystemProviderExtensions.cs)、[DB の例](../Source/Pe/Pe.Library.Database/IDatabaseExecutorExtensions.cs))
* `FileSystemProvider` は差し替えの要となる操作を本体に集め、`FileSystemInfo` 系への対応や便利な組み合わせを拡張メソッドに分けている([本体](../Source/Pe/Pe.Library.Provider/FileSystemProvider.cs)、[拡張](../Source/Pe/Pe.Library.Provider/FileSystemProviderExtensions.cs))
* C# の Extension Members を使用していない理由は、2025年時点で SonarCube が対応していなかったため。現在の対応状況は未確認
