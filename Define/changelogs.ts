/** biome-ignore-all lint/suspicious/noTemplateCurlyInString: ${...} は普通に使用する */
// cSpell:ignore xunit, Binder, Abstractions, Physical, TestAdapter, Dapper, Cryptography
import type { ChangelogVersion } from "../Source/Help/types/changelog";
import ArchiveChangelogs from "./changelogs-archive";

const Changelogs: ChangelogVersion[] = [
	{
		date: "YYYY/MM/DD",
		version: "0.99.265+",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "",
						subject: "#1059: アップデート処理に失敗する",
						comments: [
							"GHA の変数と PowerShell の変数がごっちゃになってデプロイ成果物が死んでた",
						],
					},
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "",
						subject:
							"#836: ライブラリのライセンス周りは一括処理せな死ぬ",
						comments: [
							"C#(NuGet) パッケージは機械的に対応するようにした",
							"JavaScript(NPM) パッケージはそもそもまとめてない、いるのか？",
						],
					},
					{
						revision: "",
						subject:
							"#1056: 内部で使用している TS の実行処理はコマンドラインパラメータで実行できるようにしたい",
						comments: [
							"完全固定値の処理は現行のままにしておいた、面倒なので",
						],
					},
					{
						revision: "",
						class: "nuget",
						subject: "xunit.v3 3.2.1 -> 3.2.2",
						comments: ["xunit.v3.runner.msbuild を含む"],
					},
					{
						revision: "",
						class: "nuget",
						subject: "System.Management 10.0.0 -> 10.0.2",
					},
					{
						revision: "",
						class: "nuget",
						subject:
							"System.DirectoryServices.AccountManagement 10.0.0 -> 10.0.2",
					},
					{
						revision: "",
						subject: "",
					},
					{
						revision: "",
						subject: "",
					},
				],
			},
		],
	},
	{
		date: "2026/01/07",
		version: "0.99.265",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "550f6fb7adc97de26a88b988c5ec993c29ea196e",
						class: "compatibility",
						subject:
							"コマンドライン引数処理を調整したため一部互換性が失われました(#1053)",
						comments: [
							'"--key=value" 形式(オプション全体が " で括られた書式)はサポートされなくなりました',
							"Pe が内部的に生成するコマンドライン引数には影響しません",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "fadcba59f49d1e461f8fc8063c0b02104a482961",
						subject: "#1035: ノートをスクリーンキャプチャから外す",
						comments: [
							"デフォルトでスクリーンキャプチャから除外されます",
							"除外設定は 設定 -> 基本 -> ノート で変更可能です",
							"除外方法自体はアプリケーション構成ファイルによる変更が必要です",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "8dc8907e18c02d35f6a2dcf7a5c6fe6708fb81ec",
						subject: "#929: CI上でコードカバレッジを取得する(C)",
					},
					{
						revision: "b06210c603e455ea66b2a35a2a8185c8f6c68a74",
						subject:
							"#1049: プラグインテンプレートのパッケージバージョンを Pe に追従させる",
					},
					{
						revision: "d60c89413712f289e0722648830b799513cfb853",
						subject: "update npm",
					},
					{
						revision: "f5c11eba96f80574401d05dc730053480d510fb8",
						subject:
							"#1058: リリースページ作成処理は ドラフト → アセット配置 → 正式版 とする",
					},
				],
			},
		],
	},
	{
		date: "2025/12/12",
		version: "0.99.264",
		group: ".NET 10",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject: "本バージョンから .NET 10 で稼働します",
						comments: [
							"多分大丈夫だと思うけど一応互換性注意(.NET 9 移行時と同じ)",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "8409c6a03f07dab4ef894c9530c3b75ec08d31e8",
						subject: "#1034: クラッシュレポートの直近ログがない",
						comments: [
							"app-log-limit に対する仕様が変わりました",
							"(旧) 0 以下はデフォルトログ数",
							"(新) 0 未満はデフォルトログ数、0 は無効",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "e8fc2f3ed9e84e11216222a049904f540ad33454",
						subject: "#1026: .NET 10 へ移行",
					},
					{
						revision: "f2c01708eab9b1356f144774aad58e46c5cc6e7a",
						class: "nuget",
						subject: "BenchmarkDotNet 0.14.0 -> 0.15.6",
					},
					{
						revision: "c2e3a2939cfbdd505323627c1ef4869e18761841",
						class: "nuget",
						subject: "xunit.v3 3.1.0 -> 3.2.1",
						comments: [
							"xunit.v3 3.1.0 -> 3.2.0 (c2e3a2939cfbdd505323627c1ef4869e18761841)",
						],
					},
					{
						revision: "e96591c3845938741cab4407e3e8a27caacc1ac0",
						subject: "#1033: sln -> slnx",
					},
					{
						revision: "97fa0a059ab0ea4b4dacbfb5420de4db9fde479b",
						subject:
							"#1039: PR でプラグインテンプレートの @appsettings.debug.json に起因したエラーが発生する",
					},
					{
						revision: "fdb232deec1d8ff56f30df79a4a2e2631fa1b5bf",
						subject:
							"#1042: Powershell から外部コマンドを実行する際のエラーハンドリングを見直す",
					},
					{
						revision: "c3fd14cb1f847b1724d0e16acc7e9231ad47f878",
						subject:
							"#980: プラグインテンプレートのデプロイ処理が死んでる",
						comments: [
							"細かいのは全然だめだけどとりあえず動くのOK",
						],
					},
					{
						revision: "654ff2294e6972f9117ce61f1f8b2ad435636253",
						subject: "#1046: GHA の静的検証",
						comments: [
							"モジュール入れだすときりがないので一旦は actionlint のみ",
						],
					},
				],
			},
		],
	},
	...ArchiveChangelogs,
];

export default Changelogs;
