[assembly: Xunit.Trait("Category", "UI")]
// 実 UI のテストは EXE を起動するため二重起動抑制処理等の影響を受けるので並列実行を禁止する
[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]
