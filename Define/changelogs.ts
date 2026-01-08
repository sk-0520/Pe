/** biome-ignore-all lint/suspicious/noTemplateCurlyInString: ${...} は普通に使用する */
// cSpell:ignore xunit, Binder, Abstractions, Physical, TestAdapter, Dapper, Cryptography
import type { ChangelogVersion } from "../Source/Help/types/changelog";

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
				type: "developer",
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
						revision: "",
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
	{
		date: "2025/11/18",
		version: "0.99.263",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject: "次バージョンで .NET 10 に移行します",
						comments: [
							".NET 9 と同じくランタイムは同梱するので問題ないはず",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "1e1c610dba6173d082ee83b0c65a95b04169e4c9",
						subject: "sqlfluff 3.4.2 -> 3.5.0",
					},
					{
						revision: "df42844012ee4cb7ee108359ae47aabd56eb1833",
						subject:
							"nuget を中央管理 + テストで使う nuget は共通化",
					},
					{
						revision: "5a0858c3cda7947e81a9f3b4004f46e499a1b156",
						subject:
							"#1031: actions/setup-dotnet を 5 にバージョンアップ",
					},
					{
						revision: "f9bae2ff5cee9bc80667c3294f16112b64ff9139",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2025/11/02",
		version: "0.99.262",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "ライブラリとか開発用のあれこれのみ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "f8d699343a52ad074be3b330b85877277077095c",
						subject: "#1019: textlint の導入",
					},
					{
						revision: "9a5eca8ce756b16c4a55aea6dfe97ca98d378149",
						subject:
							"#987: EXE用アイコン作成系の諸々を GHA で処理できるようにする",
					},
					{
						revision: "be82e724c987ab9ae434b3dc4abe03e7b14e478d",
						subject: "#1021: PR で走る CI が失敗する問題を修正",
					},
					{
						revision: "e2fa4bdbd5ecd0026569c4603576d505e7790bd8",
						subject: "#1023: Code scanning alerts 12",
					},
					{
						revision: "467e1612ec9023e7fa915fe042c8bf7937c2168c",
						subject:
							"#1024: DB 接続用の Sqlite 処理を別プロジェクトに切り離す",
					},
					{
						revision: "49f7bc00702eeccbeae9d04812ffb1cc94e79017",
						subject: "#1025: update nodejs 24",
						comments: ["24.11.0"],
					},
					{
						revision: "36d37aef513a422bf578a2195cba48781a1bba6e",
						subject: "xunit.v3 3.0.1 -> 3.1.0",
						class: "nuget",
						comments: ["xunit.runner.visualstudio 3.1.4 -> 3.1.5"],
					},
					{
						revision: "5d8ddadaf4534c9c9093f8c5d9d51ce6fb236a75",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.14.1 -> 18.0.0",
					},
					{
						revision: "02ff38e3f9d315001647f4200b524faa03b0f5b5",
						class: "nuget",
						subject: "System.Management 9.0.9 -> 9.0.10",
					},
					{
						revision: "383b151bfedf0348d58dd5fa689785c2bfb9e108",
						class: "nuget",
						subject: "Microsoft.Extensions.Logging 9.0.9 -> 9.0.10",
					},
					{
						revision: "544bd48838dc9e17e8606276e929bf465fbe5f5f",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Logging.Abstractions 9.0.9 -> 9.0.10",
					},
					{
						revision: "d98b9dccfd27a83d10d4c7ef02ef61420a89add1",
						class: "nuget",
						subject: "NLog.Extensions.Logging 6.0.4 -> 6.0.5",
					},
					{
						revision: "f482859749786ee1f9f5270af825c8a64391534c",
						class: "nuget",
						subject:
							"System.DirectoryServices.AccountManagement 9.0.9 -> 9.0.10",
					},
					{
						revision: "25f90ff0a04e7412097bff037af75f83ec4f6d93",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.Binder 9.0.9 -> 9.0.10",
					},
					{
						revision: "7a25f97006eedf29cabe19c882761d5704e6afd5",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.Json 9.0.9 -> 9.0.10",
					},
					{
						revision: "c0bddf320a6c15e3c731d1db97064ad4feb96c3f",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.3485.44 -> 1.0.3537.50",
					},
					{
						revision: "8697d666d5d3b4923cee3a30dd9cc8ad9d7664eb",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2025/09/17",
		version: "0.99.261",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "主にライブラリ更新",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "1cb03350bf37034234d656552d9c9e4722d32a2f",
						subject: "#1013: changelogs.json を TS にしたい",
					},
					{
						revision: "b09655a5685b5043024932b4e803580482011511",
						subject: "sqlfluff/sqlfluff 3.4.0 -> 3.4.2",
					},
					{
						revision: "e96da1fefe714720c700b9d45b09096db8c48fce",
						subject: "update npm",
					},
					{
						revision: "4497f47c67bcc6adf735aa9ed79338596e89d680",
						class: "nuget",
						subject: "Microsoft.Extensions.* 9.0.8 -> 9.0.9",
						comments: [
							"Microsoft.Extensions.Logging.Abstractions",
							"Microsoft.Extensions.Logging",
							"Microsoft.Extensions.Configuration.Binder",
							"Microsoft.Extensions.Configuration.Json",
						],
					},
					{
						revision: "cf01c36151a98e95b73e79b5ef38ffa7715ba7a2",
						class: "nuget",
						subject:
							"System.DirectoryServices.AccountManagement 9.0.8 -> 9.0.9",
					},
					{
						subject: "System.Management 9.0.8 -> 9.0.9",
						class: "nuget",
						comments: ["どこかのコミットに埋もれた"],
					},
					{
						revision: "9f8221bf06f5908efa57a993e707129561446a1b",
						class: "nuget",
						subject:
							"System.Text.Encoding.CodePages 9.0.8 -> 9.0.9",
					},
					{
						revision: "a53cd221c19589ea9bbc2d1edf5f8d5130bf6f5f",
						class: "nuget",
						subject: "NLog.Extensions.Logging 6.0.3 -> 6.0.4",
					},
					{
						revision: "60c6e4a947c01b6dd9fd7e447901d77c19b36efa",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.3405.78 -> 1.0.3485.44",
					},
				],
			},
		],
	},
	{
		date: "2025/08/22",
		version: "0.99.260",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "86d60014c1d90927f925e50bd46f3325f182e145",
						subject:
							"#1009: ノートの位置変更中もメニューを半透明にする",
					},
					{
						revision: "78ae45fe7e19d2baa28a5491f4d2d0eb06eede0b",
						subject: "#682: 環境変数編集エディタの拡張",
						comments: [
							"Windows が受け付けないような変数に対するシンタックスハイライトを破棄",
							"削除用環境変数において Ctrl + Space で現在の環境変数一覧が表示されます",
							"ヘルプは本コミットにおいて面倒なので未対応",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "24af281a7b21d731ff52dc2b7024a530c5ab36f8",
						subject: "#1007: xunit v2 -> v3",
					},
					{
						revision: "d55b8b4a167c4e150bb45735a5a146ce1c96e55f",
						subject:
							"#1012: changelogs.json 影響で開発中のヘルプ作成がつらい",
					},
					{
						revision: "fa9d43c04063d863577d54621dd1577987c261bf",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2025/08/17",
		version: "0.99.259",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "beca598928b29bc50181fb0cacfbddcdcb243a24",
						subject: "#733: ノートのスクロール位置を保存する",
						comments: [
							"とりあえず無条件で保存するようにした",
							"超過とかは WPF まかせ。細かいのはしらん",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "28b996680d3286e437197ffcab9ed489ec10ff62",
						class: "nuget",
						subject: "Microsoft.Extensions.* 9.0.7 -> 9.0.8",
						comments: [
							"Microsoft.Extensions.Configuration.Binder",
							"Microsoft.Extensions.Configuration.Json",
							"Microsoft.Extensions.Logging.Abstractions",
							"Microsoft.Extensions.Logging",
						],
					},
					{
						revision: "925bc9fbec036897e05e6df3da6cf524b2d57c97",
						class: "nuget",
						subject: "System.* 9.0.7 -> 9.0.8",
						comments: [
							"System.DirectoryServices.AccountManagement",
							"System.Management",
							"System.Text.Encoding.CodePages",
						],
					},
					{
						revision: "5a215da464cd958713c17f7b705a2b2868d96b36",
						class: "nuget",
						subject: "NLog.Extensions.Logging 6.0.2 -> 6.0.3",
					},
					{
						revision: "ed535156db9bad7d963722b95999323c2300db05",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.3351.48 -> 1.0.3405.78",
					},
					{
						revision: "6936946e2333ba7510f0d40c2a6c4ea9636cb3a6",
						class: "nuget",
						subject: "xunit.runner.visualstudio 3.1.3 -> 3.1.4",
					},
				],
			},
		],
	},
	{
		date: "2025/07/29",
		version: "0.99.258",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "203aa48648de95210018d1707ed49e6a79d12d1f",
						subject: "#1001: クラッシュレポート動いてない気がする",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "85387f814ae5663302467513c9709e9d7076cd9d",
						class: "nuget",
						subject: "NLog.Extensions.Logging 6.0.1 -> 6.0.2",
					},
				],
			},
		],
	},
	{
		date: "2025/07/22",
		version: "0.99.257",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "依存ライブラリ系アップデートのみ",
					},
					{
						subject:
							"CI で x86 のテストがうまくいかないので x86 自動テスト系は封印中",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "c0313ef073e2cc4c46712a6dbe133cb5a05f9854",
						class: "nuget",
						subject: "Microsoft.Extensions.* 更新",
						comments: [
							"Microsoft.Extensions.Configuration.Binder 9.0.5 -> 9.0.7",
							"Microsoft.Extensions.Configuration.Json 9.0.5 -> 9.0.7",
							"Microsoft.Extensions.Logging 9.0.5 -> 9.0.7",
							"Microsoft.Extensions.Logging.Abstractions 9.0.5 -> 9.0.7",
						],
					},
					{
						revision: "74b9a1c5ebb84d50baf9efa29d98d2355a26bcf5",
						class: "nuget",
						subject:
							"System.DirectoryServices.AccountManagement 9.0.5 -> 9.0.7",
					},
					{
						revision: "edd81cc5f95710a40a2dfd4e321337a0d90acbb1",
						class: "nuget",
						subject: "System.Management 9.0.5 -> 9.0.7",
					},
					{
						revision: "0ccc2924decec67b00963d2b823f6deae9bb3f69",
						class: "nuget",
						subject:
							"System.Text.Encoding.CodePages 9.0.5 -> 9.0.7",
					},
					{
						revision: "",
						class: "nuget",
						subject:
							"System.Text.Encoding.CodePages 9.0.5 -> 9.0.7",
					},
					{
						revision: "2cb684e560807bebf036dbd63cc50d7f7711ae6c",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.4.0 -> 6.0.1",
						comments: ["？つけまくってもうわからん"],
					},
					{
						revision: "8e2fddbf0d5ca68837432fac466efcc6f8519633",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.13.0 -> 17.14.1",
					},
					{
						revision: "71bbc542e2a3097bcf53d596417ec021527f3cf6",
						class: "nuget",
						subject: "xunit.runner.visualstudio 3.1.0 -> 3.1.3",
					},
					{
						revision: "4a63db1e6114cc3d6390b9d0a46475d2601f3dcf",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.3240.44 -> 1.0.3351.48",
					},
					{
						revision: "aa32ad76cd9baf95db51c576e846f70d0db5bb19",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2025/06/01",
		version: "0.99.256",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "4f0bd29e1923f37e9bf1d170687c659b11740a9f",
						subject:
							"#992: [FB:24] ツールバーのグループ切替について",
						comments: [
							"ランチャーツールバー上の Pe アプリケーションボタンでマウスホイールをコロコロするとグループが切り替わります",
							"ちなみに前後のグループ変更はマウスの4,5ボタンでも変更可能です(多分 0.84.0 くらいから, わからんけど)",
							"あんまり設定で制御するのも忙しいのでアプリケーション構成ファイルで以下を変更可能にしています",
							"ツールチップ表示までの時間: $.launcher_toolbar.temporary_group_tooltip_initial_show_delay",
							"ツールチップ表示時間: $.launcher_toolbar.temporary_group_tooltip_show_duration",
							"一時的なグループ選択の適用までの時間: $.launcher_toolbar.temporary_group_apply_delay_time",
							"※RFC 9535",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "5d091fb5e677f59e4bf9e6fb74e5b8ef1b8f375b",
						subject: "#995: 更新履歴差分の N.000 がダメ",
					},
					{
						revision: "d61fe652722a522b5597da47b00ab9d7fd30cddf",
						subject: "ヘルプ内の検索ページはルートに移動",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "531da8067166273154ea35f043121774e4e1fc7f",
						subject: "update npm",
					},
					{
						revision: "9631cc26f3f5d5239083d4c3b1865e29935f1714",
						class: "nuget",
						subject: "Microsoft.Extensions.* 9.0.4 -> 9.0.5",
						comments: [
							"Microsoft.Extensions.Configuration",
							"Microsoft.Extensions.Configuration.Abstractions",
							"Microsoft.Extensions.Configuration.Binder",
							"Microsoft.Extensions.Configuration.FileExtensions",
							"Microsoft.Extensions.Configuration.Json",
							"Microsoft.Extensions.DependencyInjection",
							"Microsoft.Extensions.DependencyInjection.Abstractions",
							"Microsoft.Extensions.FileProviders.Abstractions",
							"Microsoft.Extensions.FileProviders.Physical",
							"Microsoft.Extensions.FileSystemGlobbing",
							"Microsoft.Extensions.Logging",
							"Microsoft.Extensions.Logging.Abstractions",
							"Microsoft.Extensions.Options",
							"Microsoft.Extensions.Primitives",
						],
					},
					{
						revision: "fce72076a090eedec52f8c63fb3f4edbc493d66d",
						class: "nuget",
						subject: "System.* 9.0.4 -> 9.0.5",
						comments: [
							"System.CodeDom",
							"System.Management",
							"System.Configuration.ConfigurationManager",
							"System.Diagnostics.EventLog",
							"System.DirectoryServices",
							"System.DirectoryServices.AccountManagement",
							"System.DirectoryServices.Protocols",
							"System.Security.Cryptography.ProtectedData",
							"System.Text.Encoding.CodePages",
						],
					},
				],
			},
		],
	},
	{
		date: "2025/05/21",
		version: "0.99.255",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "729c9450ffd2b5111af670512cc1465dfd3c8bae",
						subject: "#989: 更新履歴から前回分との差分を表示する",
					},
					{
						revision: "cf54e9a1a0ccc48be7d2ba2a4c1d3b9ad719d9b1",
						subject:
							"#653: コマンド入力で PageUP/PageDown を有効にする",
					},
					{
						revision: "d15bf249d99c17d2f24d5de70f3ac73484cb78de",
						subject:
							"#952: [FB:13] ツールバーのアイテムをドラッグアンドドロップで並び順を変更可能にする",
						comments: [
							"ツールバー上のアイテムを Shift クリックで D&D が実行可能です",
							"🆗 ファイルアイテムの D&D は問題ありません",
							"✴️ プラグインアイテムは透明度に依存します",
							"🆖 セパレータの D&D は出来ません(現実装ではちょっと面倒)",
							"根本対応は #993 で実施予定",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "9e9043e16d0182330a6c4f452a70102aa27f620a",
						subject:
							"#988: 更新履歴のコメント部分に対する font-family が未指定",
					},
					{
						revision: "00763287d72dee53ce4ca0e92ac5835677580d12",
						subject: "memcmp バグ対応",
					},
				],
			},
		],
	},
	{
		date: "2025/05/06",
		version: "0.99.254",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "7e250feee9043a562111b4eeecc3937a8360ad9e",
						subject:
							"#793: ランチャーアイコンにバッジ的なものを追加できるようにする",
						comments: [
							"動的なバッジではなくアイテム自体に設定した静的なバッジとして使用する想定",
							"ランチャーアイテム設定 -> 共通 -> バッジ",
							"所謂 SJIS 2Byte 程度を表示する前提",
							"絵文字表示のためにライブラリ入れたけど、つっらいなぁこれ",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "bdcf54498d9f077ff03ad86254c0f4b79b8721ac",
						subject: "#984: DB設計のインデックス追加が死んでる",
					},
					{
						revision: "8f09a28c973e9287bf4fa444bfd00c6c7ac2218c",
						subject:
							"#985: セパレータの設定項目名がファイルになってる",
					},
					{
						revision: "0dade130c5bf067b2903b66808fbe2b889ebbc2c",
						subject:
							"#986: ノートのリンクを切断した際にリンク状態が維持されている",
					},
					{
						revision: "eff86a925bfd74cd4f0859f000462bf64c6d7b7f",
						subject:
							"ディスプレイ変更検知を SystemEvents.DisplaySettingsChanged に一元化し、WM_DEVICECHANGE は破棄",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "43a7235a90c7a77ff5cedaca42907eb645c5de95",
						class: "nuget",
						subject: "xunit.runner.visualstudio 3.0.2 -> 3.1.0",
					},
					{
						revision: "0e9f99a22b4f2fd68aff4b9208bfe884b48b0706",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.3179.45 -> 1.0.3240.44",
					},
				],
			},
		],
	},
	{
		date: "2025/04/29",
		version: "0.99.253",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "0cf8f797690a95905918ca187f8d9ea4400bbe03",
						subject:
							"#983: [FB:22] 同一アイテムが重複して登録される件について",
						comments: [
							"設定 -> 基本 -> ツールバー -> ボタンへのD&D -> D&Dファイルの重複判定",
							"デフォルトではパスのみ",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "4944567023b2ff6423d33febe6888ca8dbed3eae",
						subject: "CD の最後のバージョン設定APIを修正",
					},
					{
						revision: "05e0452fbb9279fe332e24636aaf2018dcc641ba",
						subject: "sqlfluff/sqlfluff 3.1.1 -> 3.3.1",
					},
					{
						revision: "2ade0c5c56116086a69d2ef38a6bb1fdb30478f9",
						class: "nuget",
						subject: "System.* 9.0.3 -> 9.0.4",
					},
					{
						revision: "a1a395bcacf6a4074e1130a1695386c78e15582d",
						class: "nuget",
						subject: "Microsoft.Extensions.* 9.0.3 -> 9.0.4",
					},
					{
						revision: "b27cf28d5d5016b081c228bef1eaa027bd0d6219",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.3124.44 -> 1.0.3179.45",
					},
					{
						revision: "78b6bc86f0c82a4e369180be139a8e721fb2fb65",
						class: "nuget",
						subject: "AvalonEdit 6.3.0.90 -> 6.3.1.120",
					},
				],
			},
		],
	},
	{
		date: "2025/03/16",
		version: "0.99.252",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "1c400f1c71578c525fd3ee95fddc79a142df6a23",
						subject:
							"#982: [CR:32] PlatformInformationCollector の取得処理が死んでる",
					},
					{
						revision: "987b9dfea04c3380d05c0ed803705686222e2438",
						subject: "#981: ヘルプの alert 表記が動いてない",
						comments: [
							"頑張ればできたかもしれんけど頑張る気力なし、mdxの表記でふわふわ対応",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "e858a184a7c0f3708e9e74c464215327c169c659",
						subject: "update npm",
					},
					{
						revision: "c3f2157b01b013c5e269fe4f6b2a868dc4d850b4",
						class: "nuget",
						subject: "System.* 9.0.2 -> 9.0.3",
						comments: [
							"System.Text.Encoding.CodePages",
							"System.Management",
							"System.DirectoryServices.AccountManagement",
						],
					},
					{
						revision: "65a05d96b9a94c24c3171fe5b41a0ba4d4859a88",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Logging.Abstractions.* 9.0.2 -> 9.0.3",
					},
					{
						revision: "639bb6fcfcd9a11605ab37d8d805c7abce4d451b",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.* 9.0.2 -> 9.0.3",
					},
					{
						revision: "2d8c54d8dc97d59e9671d6490e4eff44668c6f88",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.3065.39 -> 1.0.3124.44",
					},
				],
			},
		],
	},
	{
		date: "2025/02/24",
		version: "0.99.251",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "ライブラリ周りの更新だけ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "fc24fdfc3e99d946f611271a6202ede0964930cb",
						subject: "Prism 脱却の第一歩",
						comments: ["当分無理だけど組み込んでおく"],
					},
					{
						revision: "0f179dca086a68f2fe379c4e53c711dda17bd3fe",
						class: "nuget",
						subject: "Dapper 2.1.35 -> 2.1.66",
					},
					{
						revision: "7c169116eea9288309f8c7a5fdba29bf8c57eff9",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.* 9.0.1 -> 9.0.2",
					},
					{
						revision: "94d96ca2afe2b075bc6c456a819c0e701f41ff9a",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Logging.* 9.0.1 -> 9.0.2",
					},
					{
						revision: "7e823de85663b8e87de88a672ac777c5333cc87c",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.2957.106 -> 1.0.3065.39",
					},
					{
						revision: "12001ab0d8041e88d31e20d291d74d5310cb6e92",
						class: "nuget",
						subject:
							"System.DirectoryServices.AccountManagement 9.0.1 -> 9.0.2",
					},
					{
						revision: "be0257589a666132db6aa090065ba8551722f295",
						class: "nuget",
						subject: "System.Management 9.0.1 -> 9.0.2",
					},
					{
						revision: "687f3e040ec670883fe241d45145d65ceab4d0d9",
						class: "nuget",
						subject:
							"System.Text.Encoding.CodePages 9.0.1 -> 9.0.2",
					},
					{
						revision: "6166c9bce6cf8c5e087e971bea4abd51c472508f",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.15 -> 5.4.0",
					},
					{
						revision: "850646f2f41b4a4fbb7e54350b1f19bb730a84a5",
						class: "nuget",
						subject: "テスト系のなんか",
					},
					{
						revision: "0f07cac9afeb3fc2ad3f552366faf584145897f4",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2025/01/13",
		version: "0.99.250",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "6f03f098aa48173751482b8ad5fab5ee2ee2e909",
						subject:
							"#979: プラグインインストールにステータスの確認を行う",
						comments: [
							"有効・チェック失敗をインストール対象とした",
							"無効・予約はインストール対象外",
							"チェック失敗はサーバー側の未確定処理なので現状OKとしておく",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "66ca3d56059e80b6a380df34a86f204a6d9e2998",
						subject: "UT周りのライブラリ更新",
						class: "nuget",
						comments: ["ライブラリが多い、つらい"],
					},
					{
						revision: "24a3449f38f5c2e393af55892aaef1cb143c8983",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.14 -> 5.3.15",
					},
					{
						revision: "331727764b8259869818d0cc5de6ca2327cf62c0",
						class: "nuget",
						subject: "SevenZipExtractor 1.0.17 -> 1.0.19",
					},
					{
						revision: "1ee78e74be4755bb4a7d9f1cc381da2fd0cd2c25",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2024/11/22",
		version: "0.99.249",
		group: ".NET 9",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject: "本バージョンから .NET 9 で稼働します",
						comments: [
							"多分大丈夫だと思うけど一応互換性注意(.NET 8 移行時と同じ)",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "67a8885352f7b733f3c70a358c365c8efd87e2e6",
						subject: "#976: .NET 9 へ移行",
					},
					{
						revision: "bf04e84eeabf9d336e73368c0e7558713dff9661",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.2849.39 -> 1.0.2903.40",
					},
				],
			},
		],
	},
	{
		date: "2024/11/17",
		version: "0.99.248",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"コマンドライン引数は --option 形式でのみ受け付けるように変更しました",
						comments: [
							"/形式と-形式はもうダメです",
							"-形式は前からダメです",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "c51bab10fe817272a9602155b47fcf27f664f9ba",
						subject: "#690: どこかしらで WebView を表示可能にする",
						comments: [
							"とてもいろいろありました",
							"修正: DI コンテナの登録解除処理が動いてない",
							"修正: リリースノート表示時に WebView2 の初期化処理が動いていない",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "3ce5716aa29f15671ca22af1cc1ecb106679fc03",
						subject: "なんだこのコピペミスは",
					},
					{
						revision: "6d37a63d693c251e0d10a26ecd8bc6da05fc75fa",
						subject:
							"#928 のリビジョンが正しくない 06ab8d1881 -> e8338b20dc",
						comments: ["何も解決してなかった"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "cb9842028c52fd133f6882a532741a2cdbb5c61c",
						subject: "#975: node のバージョンを v22 に変更する",
						comments: ["v22.11.0"],
					},
					{
						revision: "0a576c53681bc61ce0549179e83685bb86d5bb3a",
						subject:
							"#883: コマンドラインオプションパース処理をまともにする",
						comments: [
							"別にまともでもなんでもないけど --key 形式のみを受け付けるようにした",
							"テストが弱いのともうしんどいのとでクソ適当実装",
							"C -> C# への引数移送がもうわからんのでリリース。バグってたら今後でなおす",
						],
					},
					{
						revision: "d9163a91ea3a2861f389cd695ad0efc0f3fa1b54",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2024/10/29",
		version: "0.99.247",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "b32b1ec6c5179edb7709688c9f4560623ba34950",
						subject:
							"#928 のリビジョンが正しくない 06ab8d1881 -> e8338b20dc",
					},
					{
						revision: "29c19133ade144ebc5106391aa629a2f33607a0a",
						subject: " #898: ヘルプの左側がなんかこう、広い",
						comments: [
							"あんまよく分かんなかったのでサイドバー自体の拡縮を追加した",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "c822650e4629e910793b592fed43650d7b9f931c",
						subject: "おうちサイトのURL変更",
					},
					{
						revision: "6a41bf323c9cd7b5aefa2b2494ca3b6cd471e06f",
						class: "nuget",
						subject: "Hardcodet.NotifyIcon.Wpf 1.1.0 -> 2.0.1",
						comments: [
							"2.0.0 は高 DPI だと動かなかったので待ったでござる",
							"#530 が解決出来たらうれしいなっていう思い",
						],
					},
					{
						revision: "fbad30c209e126feaf3beaab94360beff25188a2",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.2792.45 -> 1.0.2849.39",
					},
				],
			},
		],
	},
	{
		date: "2024/10/20",
		version: "0.99.246",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "e8338b20dcf1a016d7db85c194c089549e8300ad",
						subject:
							"#928: アップデート処理中にアプリケーション実行されてる気がする",
					},
				],
			},
		],
	},
	{
		date: "2024/10/16",
		version: "0.99.245",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject: "またしてもサーバー周りのドメイン変更",
						comments: ["次回アップデートが成功すればOK"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "45040431ee969b15f7076ea0b9ac3805d9d1784b",
						subject: "#974: APIサーバー変更",
						comments: [
							"peserver.site -> pe.content-type-text.org",
							"調子乗って org を取ってしまった悲しみ",
							"サブドメインで戦えるように欲張った悲しみ",
							"遂にお財布開いた悲しみ",
							"一ヶ月でビール飲める値段🍺。サーバーは全部金問題",
							"悲しさしかない",
						],
					},
				],
			},
		],
	},
	{
		date: "2024/10/14",
		version: "0.99.244",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "017c6c94498a9f3deb072b0cadfb61dd3f44b608",
						subject: "#970: リリースノートの謎横スクロールを消す",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "a873c6cc1165803e8c59b7579d8c84aae70a0ef0",
						subject:
							"#972: .NET Standard 基準のソースはもう .NET n に移したい",
						comments: [
							"Pe 側で使ってる .NET 8 に変更",
							"Pe.Standard.* -> Pe.Library.*",
						],
					},
					{
						revision: "5a0712460c1d2a5a4849836501997c4a5349ddf5",
						subject: "#973: GHA 権限整理",
						comments: [
							"今が何の権限で動いてんのかわっかんねぇんよ",
						],
					},
					{
						revision: "e4164b853161c198fa339d49367b50b723e25195",
						class: "nuget",
						subject: "Microsoft.Extensions.Logging 8.0.0 -> 8.0.1",
					},
					{
						revision: "10a2aadfc00e7e9b6bba13bc40c787f274483c99",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Logging.Abstractions 8.0.1 -> 8.0.2",
					},
					{
						revision: "c5a3ae47c6f01d7c7230c54874414c21f0233b65",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.Json 8.0.0 -> 8.0.1",
					},
					{
						revision: "275b30abdcdf70bf627f5cb79e0c9083507332f5",
						class: "nuget",
						subject:
							"System.DirectoryServices.AccountManagement 8.0.0 -> 8.0.1",
					},
					{
						revision: "2a6de9af983966dc8d624d8b99741fff430a0a67",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2024/10/06",
		version: "0.99.243",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "5524166987884da82150012ca833b82ae8e28978",
						subject:
							"#968: リリースノート生成処理をヘルプの更新履歴処理に差し替える",
						comments: [
							"プレーンなHTMLが死んじゃうけど WebView2 がよしなにしてくれると信じとく",
						],
					},
					{
						revision: "f99f664c6da8efd91b7425eadf0b65a65f315da9",
						subject: "#967: 現在設定をファイル出力する",
						comments: [
							"個人的には多分これでOK",
							"HTML生成処理とかくっそ変な部分もあるけどそれは別課題で対応する",
						],
					},
					{
						revision: "42ccfe9d4fbf87c320e17db77a5ebe0074659899",
						subject: "#966: MessageBox を TaskDialog に置き換える",
						comments: [
							"今のインターフェイスで可能な限りできる範囲で、それっぽく",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "d54f7e8693cd2c7e20929adba969129c44b4ca5e",
						subject:
							"色選択UIで色一覧自体にフォーカスできる不備の修正",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "007475e19f97e08abffaa9eca43299b32e9f212c",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.12 -> 5.3.14",
					},
					{
						revision: "e138367046e0d266917f354644e06b743e70c195",
						subject: "update npm",
					},
					{
						revision: "13becb946097ef8fddebc37d36aa295a71fe4025",
						class: "nuget",
						subject: "xunit 2.9.0 -> 2.9.2",
					},
					{
						revision: "5647d848627279e10d3530db21e748ea88629c79",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.2739.15 -> 1.0.2792.45",
					},
					{
						revision: "6106726c69390230fb55ee96ae5bae9d24361111",
						class: "nuget",
						subject: "System.Data.SQLite.Core 1.0.118 -> 1.0.119",
					},
				],
			},
		],
	},
	{
		date: "2024/09/12",
		version: "0.99.242",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "しょうもないのだけ",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "104fbf94f53de9a38b9950e002c5a5ed0876bfda",
						subject: "#937: ヘルプファイルの再々作成",
						comments: [
							"細々したのを全部まとめ保守性を高められた気がする",
							"アップデート時のリリースノートにも適用したかったけど力尽きた",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "fe31fcd6c23da62ba19d30c5b186b0069f9f23df",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.11.0 -> 17.11.1",
					},
					{
						revision: "b1912b2e19cfb57095f8414bc4a5d98c052d98a8",
						class: "nuget",
						subject: "Moq 4.20.70 -> 4.20.72",
					},
					{
						revision: "5c93865beae80284ffc9844ba839baa9ffe0cdc1",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.2651.64 -> 1.0.2739.15",
					},
				],
			},
		],
	},
	{
		date: "2024/08/22",
		version: "0.99.241",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "a2267a973ffc3e5138a200ebf6100729bd967ec3",
						subject:
							"#953: [FB:14] ショートカットの登録処理方法を固定可能にする",
					},
					{
						revision: "48165e0cc8cd7cd88925f6c1bcee1fa740143f13",
						subject:
							"#958: コマンド型ランチャーのタグ検索設定を破棄する",
						comments: [
							"ON/OFF切り替える意義があんまりなさそうなので判定処理自体なくした",
						],
					},
					{
						revision: "6b7c8791b536819f4a374781f84ddfcd1cae174f",
						subject:
							"#947: 表示中のランチャーアイテムアドオンを実行した際にアクティブ化させる",
						comments: [
							"ソース上 TODO だったので機能追加よりバグ修正に近い",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "3fb50cfd8f149178804048defca616c6d0b254d9",
						subject:
							"#959: バイナリ保存データの分割処理は多分メモリ的に意味ないからわかりやすくすべし",
						comments: [
							"ユーザー機能的には影響ないはずだけどダメだったらデータ消すなりなんなりしてね、っていう気持ちでバグ対応に記載",
						],
					},
					{
						revision: "2196a50f26ec3688e5604e85223d955ebc3520ac",
						subject:
							"#964: コマンド型ランチャーのウィンドウサイズは設定アイコンサイズを無視するべき",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "26fac5d7012914347cfd0740fee0b2eb398c6b5f",
						subject: "#963: SQL に対して Lint を実行する",
						comments: [
							"インデント・改行が一切解決できていないけど仕組みを構築したので OK",
						],
					},
					{
						revision: "b5c82cd63b3f2b93049561f8c2934897515d9559",
						subject:
							"#965: たぶん LauncherItemIconStatus はもういらない",
					},
					{
						revision: "b1bf850540917f020d9863ec227ca2b8a42b97a8",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.10.0 -> 17.11.0",
					},
					{
						revision: "510f591ca3e685770e47695901faca0c8805db61",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.2592.51 -> 1.0.2651.64",
					},
					{
						revision: "d0ef11bef5fc6bb21ee3cda7ff2edaa8f3724e5d",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.11 -> 5.3.12",
					},
					{
						revision: "13a21f231b6b19356fa99c03f4088fc61a79cf21",
						class: "nuget",
						subject: "Prism.Wpf 8.1.97 -> 9.0.537",
						comments: [
							"しれっと変えていいライブラリ(メジャーバージョン変わってるし)じゃないけどなんとなく動かした感じ動いたからもういいわ",
							"早いこと #843 やりたい気持ちだけが動く想い",
						],
					},
				],
			},
		],
	},
	{
		date: "2024/08/12",
		version: "0.99.240",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"特に何もしてないけど一旦ここでバージョン上げておきたい感",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "736901f5432e82b1981ffd3b67e8ed1f22b95c36",
						subject: "#957: ヘルプに対して全文検索を追加する",
						comments: ["適当実装なのでくっそ甘い"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "c265222f4455954115d50d1800b9b3834dc594ce",
						subject: "#956: 設定ファイル位置がヘルプにない",
					},
					{
						revision: "d7cd3aae47d91b5294c9655338a56bdcaff25996",
						subject:
							"#955: コマンドランチャーからの例外送出をなんらか整理する",
					},
					{
						revision: "9be909df5e65e948645fdb4f8dcff558fec1cf06",
						subject:
							"Windows 11 だとスクロールバーが消えるのでヘルプのメニュー部分が不明瞭なので適当になんやした",
						comments: ["ださい"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "0fc9f12b922c5883f35ee9925b326d0689113d3c",
						subject:
							"Vector 提出用 EXE 作成処理の成果物を1アーカイブにまとめた",
					},
					{
						revision: "7bdcc414936b38e1fb9ea72bb3f1b19f8471e961",
						subject: "update npm",
					},
					{
						revision: "b4c9c6ff764e8b230b9d78486b98e1d2079d56c5",
						subject: "update npm",
						comments: ["二回やってる時点でもうダメだと思った"],
					},
				],
			},
		],
	},
	{
		date: "2024/07/17",
		version: "0.99.239",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "2cc299438092ec4709862101dcb332ea2c01a1aa",
						subject: "指定して実行のオプションラベルの指定漏れ対応",
					},
					{
						revision: "0284bb95418454f24b934cf35c1fbe82d31a7f7e",
						subject:
							"#950: バージョン情報に git のリビジョンが自動付与されている",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d08fcdcc2a2184ae9ef0f56c686b29e28ab8f10f",
						subject: "eslint 周りを biome に差し替え",
					},
					{
						revision: "4d23bc968cce56bdb662ae0ab539611b4dc694f7",
						subject: "update npm",
					},
					{
						revision: "bff32028559d554b5576b85e5cc546cfd37ea3f9",
						subject:
							"#946: テストの共通処理はテスト用プロジェクトにしておく",
					},
					{
						revision: "b216abfd07d46101c85789fc734b006630502002",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.Binder 8.0.1 -> 8.0.2",
					},
					{
						revision: "b348c50afc515219a15f6714eab1e11fc3bb6b7d",
						class: "nuget",
						subject: "xunit 2.8.1 -> 2.9.0",
					},
					{
						revision: "df8524e519673b18f513ff21e95d40a61cb73f2b",
						subject:
							"#948: プラグインのデバッグ処理が出来なくなってる疑惑",
					},
				],
			},
		],
	},
	{
		date: "2024/06/23",
		version: "0.99.238",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "8dff0ff7fe7ae9b9aef904fb6fe07a4c7693d747",
						subject: "#945: CodeQL GithubActions 最新化",
					},
					{
						revision: "a506e1154d4e15c52741c4f1204ef25f221d90e1",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.2535.41 -> 1.0.2592.51",
					},
				],
			},
		],
	},
	{
		date: "2024/06/18",
		version: "0.99.237",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "c55f26206b6df69ec6cf944ea56b37c6ebf94488",
						subject:
							"#936: ランチャーアイテムのコードって多分いらない",
						comments: [
							"べつにバグじゃないけど機能追加でもないから修正とする",
						],
					},
					{
						revision: "71f13eb316f8dbef60339497bed1473ddba55971",
						subject:
							"#942: DB上一意なランチャータグに対する同一文言の登録が可能で挙句死ぬ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "788e411a044938d2c0cb80fb34a508dba74a727b",
						subject:
							"#941: node のバージョンを v20.14.0 に変更する",
					},
				],
			},
		],
	},
	{
		date: "2024/06/16",
		version: "0.99.236",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "b405e193ae4b71b3f4addd6b7d85fd293b00d4aa",
						subject:
							"#288: ランチャーアイテムのボタン間に区切りを表示",
						comments: [
							"遅延読み込みせず即時読み込みでもうちょっといい感じにできるとは思うけどまぁまぁ、諦め",
							"※グループアイテムに紐づけるかランチャーアイテムとするかで結構悩んだが工数の少ないランチャーアイテムに落ち着いた感あるけどどうなんですかねこれ",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "7b1dced96452800ec353d1f077f6240ae8e1c11a",
						subject:
							"#939: グループ設定画面で選択されているランチャーアイテムの移動に表示位置を追従させる",
					},
					{
						revision: "715ab5a5402d4d0ce83cf2b8b354ff23e5eee32d",
						subject:
							"#938: ツールバーに置いている RepeatButton/TogleButton のデザインなんとかなんないですかね",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "4e0880774ea621afc5d991522351672007210a9c",
						subject:
							"#935: ワークフロー Build CI/CD が PR で動いている",
					},
					{
						revision: "4969e9d62284853e9b50c210cdbfa48f6b7e0068",
						subject: "更新履歴のプルダウンに大まかなグループを追加",
					},
				],
			},
		],
	},
	{
		date: "2024/06/07",
		version: "0.99.234",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "f6a169faa1973965e18adfc23734b383475690f4",
						subject:
							"#933: DI の型よわよわ生成処理を Factory 作って早め対応できるようにする",
						comments: [
							"リファクタしないと無理なので気になるところだけ対応",
						],
					},
					{
						revision: "3433ca1faf00fd4e201c2f81d7e902542d6b2ce8",
						subject:
							"Microsoft.Web.WebView2 1.0.2478.35 -> 1.0.2535.41",
					},
				],
			},
		],
	},
	{
		date: "2024/05/26",
		version: ["0.99.232", "0.99.233"],
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "機能追加・バグ修正なしだったけどバグ修正あり",
						comments: [
							"ライブラリの更新とコード周りのテスト追加とそれに伴うソース修正",
							"0.99.232 でHTTPアクセス時のストリーム周りが想定外のものでつらかった",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "1e5c806bf0f1a77a85a3fa5e60166b42f117274c",
						subject:
							"#932: post 処理の応答は send に寄せたことで stream がシーク不可",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "9fe24f76eec2f7a8ac48a528775332359142068c",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.9.0 -> 17.10.0",
					},
					{
						revision: "5d38412fb3a18dfd4ad85065a04ace817b7da669",
						class: "nuget",
						subject: "xunit 2.8.0 -> 2.8.1",
					},
				],
			},
		],
	},
	{
		date: "2024/05/10",
		version: "0.99.231",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "8cee873999de85ed2be5539f2c201e95b1441089",
						subject: "#546: CI上でコードカバレッジを取得する",
					},
					{
						revision: "f5fbd79d3a44d886fb24f042158d6cb3161950f5",
						subject:
							"#930: CI にドキュメントコメントの出力機能を追加する",
						comments: [
							"諸々おかしいけど完全な異常は修正したので一旦セーフとする",
						],
					},
					{
						revision: "80ceeaa447e51358873336dfa7b45b532ff244fb",
						subject:
							"プロパティへのアクセス系処理をプロジェクト独立",
					},
					{
						revision: "7aa4f83b2a6192de0343a1a575f93e692e1197d7",
						class: "nuget",
						subject:
							"Microsoft.Web.WebView2 1.0.2420.47 -> 1.0.2478.35",
					},
					{
						revision: "6caf27d75110db25ee9183b6685a9fb36cb3c497",
						class: "nuget",
						subject: "xunit 2.7.0 -> 2.8.0",
					},
					{
						revision: "c6a5bf74ed960b5eeee7acfb3b3a84982a7842aa",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.8 -> 5.3.11",
					},
				],
			},
		],
	},
	{
		date: "2024/04/22",
		version: "0.99.230",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "e0bf9443bc31adf73f6d1dbb6780e74dc5ea981f",
						subject:
							"#921: グループ設定時にグループ最終ランチャーアイテムに対して選択可能ランチャーアイテムを D&D すると落ちる",
					},
					{
						revision: "dbd4c79e47e61d45afec6944b7045c4b1d2d0023",
						subject:
							"スピンコントロールの初期表示におけるボタンの活性状態を修正",
					},
				],
			},
		],
	},
	{
		date: "2024/04/17",
		version: "0.99.229",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						class: "compatibility",
						subject:
							"#717 対応により WebView2 ランタイムが必要になります",
						comments: [
							"Windows 11 では対応不要です",
							"Windows 10 では状況次第で必要ですがたぶんまぁ大丈夫。ただし状況(Windows Update 未適用等)に応じてランタイムインストールが必要です(以下URLを参照)",
							"https://developer.microsoft.com/microsoft-edge/webview2",
							"https://jpdsi.github.io/blog/internet-explorer-microsoft-edge/webview2-faq/",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "b93e51212301d089efc0d977016483d701d1462c",
						subject: "#717: 内蔵ブラウザをWebView2にする",
					},
				],
			},
		],
	},
	{
		date: "2024/04/09",
		version: "0.99.228",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "11cc85fa42a89fe91163781d3f76099bc5149ffa",
						subject:
							"#927: アップデート処理のコピー処理に不具合ありそう",
						comments: ["なかったなぁ……"],
					},
				],
			},
		],
	},
	{
		date: "2024/04/08",
		version: "0.99.227",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "d48e27afda47c080fb2bf25393cd620d8dc4c211",
						subject: "#906: HTTP アクセス時に UA 未設定",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "dbb83f459f88e84779e8927689bfe29533c5e161",
						subject: "#925: API周りのソースコードURIがふるい",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "91a1cf8e5160a4388c1e3e737c53199f80d5028f",
						subject: "#890: 諸々を async 処理に置き換える",
						comments: [
							"Task.Wait() 入り乱れる謎実装になってしまった",
							"たぶんうごく。たぶん",
						],
					},
					{
						revision: "52302e29c52545eae52dce497d23b633fbe38bac",
						class: "nuget",
						subject: "Dapper 2.1.28 -> 2.1.35",
					},
					{
						revision: "74e320e05a72dc0160f4663c498d5ef06962562b",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Logging.Abstractions 8.0.0 -> 8.0.1",
					},
					{
						revision: "0c8fec415ee46ba86abe2730133832f41e60a66b",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 120.2.70 -> 123.0.60",
					},
					{
						revision: "01b47c80f1ed7e8b0a7cb515001bee9c6289eaa7",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: ["2024/04/01", "2024/04/02"],
		version: ["0.99.223", "0.99.224", "0.99.225", "0.99.226"],
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "とりあえず一旦リリースしたい気持ち",
						comments: ["master ブランチのルールをミスった悲しみ"],
					},
					{
						subject: "本バージョンアップも Pe から実行不可の悲しみ",
						comments: ["appsettings.json のテストいるなぁ……"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "f23c17b5da2acc7991a03f4955ac6f992a251dd5",
						subject: "#923: アプリアップデート機能が死んでる",
						comments: [
							"appsettings.json",
							"version_check_uris -> version_check_url_items",
						],
					},
					{
						revision: "50a9ff5dca1bbeafc1e55b8dc49b737b784ac0a8",
						subject: "#924: 成果物に doc/license が含まれていない",
						comments: [
							"CI再構築した際にミスっていた模様",
							"ファイルない状態で情報開くと落ちるっていうね",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "bc5387ea09fef5fd093b736a100ad4b97b9216ce",
						subject: "#920: Stop-Transcript って別にいらない",
					},
					{
						revision: "f8e60a674233527e950486e28a75ff7714397b22",
						subject: "#922: テストフレームワークの変更",
						comments: ["MSTest -> xUnit"],
					},
				],
			},
		],
	},
	{
		date: "2024/02/08",
		version: "0.99.222",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "本バージョンアップは Pe から実行不可の悲しみ",
						comments: [
							"Pe の API 周りのドメインを変更",
							"peserver.gq -> peserver.site",
							"つらい。ほんとつらい。めたぁ",
							"次のバージョンアップがうまくいけば万歳",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "e1c070080547923a981d17212b3698d94d7f3b7b",
						subject: "#919: ドメインを変更する",
						comments: ["くっそつらい修正", "なんだこれもうまじで"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "23d9b7627dd6b535313fd35c0715ca8d876bbc68",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.8.0 -> 17.9.0",
					},
				],
			},
		],
	},
	{
		date: "2024/02/07",
		version: ["0.99.220", "0.99.221"],
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "fe41eeac6152624b09269362431df2c92e08d832",
						subject: "#797: パスワード入力欄をマスク表示する",
						comments: ["PasswordBox2 っていうほぼ無名クラス"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "9a523464291740757e9faa581c7f8eae78632bf4",
						subject:
							"#912: Fix code scanning alert - Overly permissive regular expression range",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "0717ee46f28e4f00b16aa25a8f8a015f2ac9cc8b",
						subject: "#915: Node.js 16 actions are deprecated",
						comments: [
							"actions/upload-artifact@v4 の速度アップは未調査のため zip 圧縮展開処理は継続",
							"Release ページへのアップロード処理 svenstaro/upload-release-action は本件では未対応、 #916 で対応する",
						],
					},
					{
						revision: "662dab7eb6e0ed90233a04a6324c30ea65474ef9",
						subject:
							"#916: Node.js 16: リリース物アップロード処理対応",
						comments: [
							"プラグインテンプレートが古いけどこれ以上はあれなので #917 で対応する",
						],
					},
					{
						revision: "6c8a7ae98ad26e53ea9164eb08351aca475c1461",
						subject:
							"#918: 続 PowerShell のソースを機械的にキレイにしたい",
					},
					{
						revision: "8f9d19d3697d48bafc36ef9ad62675560f781642",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 120.1.110 -> 120.2.70",
					},
					{
						revision: "00c7efd88045c4a061854f6f05677165eea1e355",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.Binder 8.0.0 -> 8.0.1",
					},
					{
						revision: "0dac96005b8f0240cd14571edfd36ff8cd8ed58b",
						class: "nuget",
						subject: "MSTest.* 更新",
						comments: [
							"MSTest.TestAdapter 3.1.1 -> 3.2.0",
							"MSTest.TestFramework 3.1.1 -> 3.2.0",
						],
					},
				],
			},
		],
	},
	{
		date: "2024/01/08",
		version: "0.99.219",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "5bf11b70ae9c0f38c227273c72e34e925b854cb4",
						subject:
							"#914: デフォルトテーマプラグインが動いてないのに動いてる",
						comments: [
							"無駄にややこしい無駄な処理がつらい",
							"こういうのどうやってテスト作ればいいのか",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "150e6980773b864e3f81936291c53454b4a0fa23",
						subject: "update npm",
					},
					{
						revision: "9ad4310f9cc3b62c81e298824734275e06031ae2",
						class: "nuget",
						subject: "Dapper 2.1.24 -> 2.1.28",
					},
					{
						revision: "29a5eaf03b3d7c5d99d723ba2c1a2e6f08df55c1",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.7 -> 5.3.8",
					},
					{
						revision: "743af58861d72025cc556e6a08ee2b8663ee3352",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 120.1.80 -> 120.1.110",
					},
				],
			},
		],
	},
	{
		date: "2023/12/27",
		version: "0.99.218",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "e6a4823b4fab1077512ea4ea924fd94cf1e9e59c",
						subject: "#913: アイコンの色がおかしい",
						comments: ["たぶんCI処理を変えたあたりから狂ってた"],
					},
				],
			},
		],
	},
	{
		date: "2023/12/12",
		version: "0.99.217",
		group: ".NET 8",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject: "本バージョンから .NET 8 で稼働します",
						comments: ["多分大丈夫だと思うけど一応互換性注意"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "f5478d716aaf5d7dd426c71595f97218aafd946d",
						subject: "#911: .NET 8 へ移行",
					},
					{
						revision: "7b42504a5e3d0daba125467f5f7afa5c49022531",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 117.2.40 -> 120.1.80",
					},
					{
						revision: "c5570fcc77a8029631645262da9c9812f346a50c",
						class: "nuget",
						subject: "Dapper 2.1.15 -> 2.1.24",
					},
					{
						revision: "dd94656b55f9e162c0e67755ff8a29335ae8923a",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.5 -> 5.3.7",
					},
					{
						revision: "b70bcc973b0745d65febe9ac0e646439cef62505",
						class: "nuget",
						subject: "Moq 4.20.69 -> 4.20.70",
						comments: ["使ってないけどまぁ一応"],
					},
				],
			},
		],
	},
	{
		date: "2023/11/16",
		version: ["0.99.215", "0.99.216"],
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "0.99.215 はあかんかった",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "a9028fdab25178ea3762147feb6f77670b0dc0d8",
						subject:
							"#910: 自己解凍形式生成処理、なんか落ちてますよ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "583e2d89b2996c3a47d63b29e01deea00753aabc",
						subject: "#909: node を 20.* へバージョンアップ",
						comments: ["各モジュール系もついでに更新"],
					},
					{
						revision: "6f64a93395d2d986cc2721b6cb7c0169025c9e9c",
						subject:
							"#907: PowerShell のソースを機械的にキレイにしたい",
					},
				],
			},
		],
	},
	{
		date: "2023/11/07",
		version: "0.99.214",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"もうなんでもいいから #900 はリリースしておきたいのです",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "4ead3e1c471c80c91074279a10aed85a8091b707",
						subject:
							"#900: CI/CD のテストはもう単独実行でいいでしょ",
						comments: ["CI/CD 周りの処理をがっつり変更した"],
					},
				],
			},
		],
	},
	{
		date: "2023/10/23",
		version: "0.99.213",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "依存ライブラリ系のみ更新",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "f0cae27fd91d19106ab999214c7898ed1515248e",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 116.0.230 -> 117.2.40",
					},
					{
						revision: "693afcb8fb5d3b900988084109c29d31a9bf4105",
						class: "nuget",
						subject: "Dapper 2.0.151 -> 2.1.15",
					},
					{
						revision: "5f6add5b2e7d79b2a65af9ad2a1ea818ff8dae0d",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.4 -> 5.3.5",
					},
					{
						revision: "5f881f840231c36db204fa641a442143b33abb84",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2023/09/29",
		version: "0.99.212",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "依存ライブラリとかCI側だけ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "1515579ef73852627366cd11eda268d8d1a38656",
						subject: "#905: Pe.Publish は Pe に統合すべき",
						comments: [
							"Pe.Publish から無理やり持ってきた。きちゃない",
						],
					},
					{
						revision: "ab09c3c8865f1c042a6919f21b15e777fd950fb0",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.3 -> 5.3.4",
					},
					{
						revision: "becee34347c4b4cdc7cf988b76f902a9134c1f04",
						class: "nuget",
						subject:
							"System.DirectoryServices.AccountManagement 7.0.0 -> 7.0.1",
					},
					{
						revision: "5f731811c06046886bf935f65cc7e8d50b39ffac",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 114.2.120 -> 116.0.230",
					},
					{
						revision: "b333c3086011b892a5b52c6b0777c801823937e3",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2023/09/03",
		version: "0.99.211",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "e356019f398c361175b8138abfdbc1910dd1f711",
						subject:
							"#901: プロセス間通信時にログ出力をなんらかハンドリングする必要あり",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "29929a624029de9ada06997afc31e21537d7feb4",
						subject: "プラグインテンプレートの生成処理を修正",
					},
					{
						revision: "af66d1cf5951c0a0d91b21e4c4f650d891b28a48",
						subject:
							"#903: プロセス間通信時の呼び出された側で非GUI処理を行う場合にダイアログは表示させない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "ab47424cc0d71ced47925abf3b3435758ea7b452",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.7.0 -> 17.7.2",
					},
					{
						revision: "b83824026edb4a37c9fde72c274078e7fc51f17a",
						class: "nuget",
						subject: "Dapper 2.0.143 -> 2.0.151",
					},
					{
						revision: "2a7d1404b0732e7762872feafe624d66a72ce90b",
						subject:
							" #904: プラグインテンプレートの launchSettings.json は git 管理外とする",
					},
				],
			},
		],
	},
	{
		date: "2023/09/01",
		version: "0.99.210",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "プラグイン作成が難しくてつらい",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "884c7b7542ae26f85f60dfc44fcff88eb1a1abb3",
						subject:
							"#902: プラグインアーカイブから対象プラグイン取得に際して複数DLLを考慮する",
						comments: [
							"プラグインモジュールから頑張って探すのは無理があるのでプラグイン側で指定するように修正",
						],
					},
				],
			},
		],
	},
	{
		date: "2023/08/23",
		version: "0.99.209",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "e5a97bf277339e6720c68db02515c85db4efdcee",
						subject:
							"#897: デバッグ用にプラグイン起動処理を追加する",
					},
					{
						revision: "32abda47dd7ffd68c6fc96b1e5846be64dc265e4",
						subject:
							"#899: View で使用しているアイコン系を Bridge に移動する",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "bf4d5c04987e6216a349b67d878ca68d6b5b454a",
						subject: "update npm",
					},
					{
						revision: "ede342cb481b05bd086aa5ccef64651d4bcac403",
						subject:
							"#891: プラグインテンプレートにテストとかを追加する",
					},
				],
			},
		],
	},
	{
		date: "2023/08/13",
		version: ["0.99.206", "0.99.207", "0.99.208"],
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "0.99.208へ諸々統合",
						comments: [
							"0.99.206 はプラグイン処理に問題があるので 0.99.207 として統合",
							"0.99.207 はプラグイン処理に問題があるので 0.99.208 として統合",
							"master ブランチ過労死",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "360911dcd4d48a241650a7f4d2ce65b8592f05a9",
						subject:
							"#893: プラグインのアセンブリ解決に使用するパスに対してディレクトリ補正は不要",
					},
					{
						revision: "151496a93877b780cdf42a2551c965a49273ad78",
						subject:
							"#894: プラグイン自体の依存読み込み時にローダーがもう死んでる",
						comments: [
							"自信ないけど多分これで大丈夫。アップデート処理とかはもうわかんない",
						],
					},
					{
						revision: "d6fab47c6835b4377d838ee3d030055d0634adf7",
						subject:
							"#895: 初期化諸々のタイミングでももうお亡くなりローダー",
						comments: ["#894 が結局ダメだった"],
					},
					{
						revision: "7d9a6c8d33eae0e7e25df923c27e82097ca0ccc8",
						subject: "#896: #895 対応が初期化実行時に死ぬ",
						comments: ["#895 が結局ダメだった"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "36eeb7a985002814f587ff7a3e3d1fcfcf88dd8f",
						subject: "nuget 確認不要そうなもの更新",
					},
				],
			},
		],
	},
	{
		date: "2023/08/04",
		version: "0.99.205",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "663bbda761ac1d8f8d37a0c178e220fc9e03db5e",
						subject: "プラグイン: キー一覧取得処理の追加",
					},
					{
						revision: "aeb9ea917080b167e5708cd269ec5d2d8e35971e",
						subject:
							"プラグイン: ランチャーアイテム処理時にDB保存処理制御",
					},
				],
			},
		],
	},
	{
		date: "2023/07/23",
		version: "0.99.204",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "プラグイン用の細かいあれこれ",
					},
				],
			},
		],
	},
	{
		date: "2023/07/22",
		version: "0.99.203",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"#862 対応の影響により管理者権限で実行中はプラグインを読み込まなくなりました",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "be0919128501ff16d22fde7a32da2d6d875cf8ee",
						subject:
							"#862: 管理権限で実行している場合にプラグインは読み込まないようにする",
						comments: [
							"起動時に読み込むプラグインが Pe 専用のものに限定されます",
							"管理権限でプラグインが自由に動くことを回避するための処置です",
						],
					},
				],
			},
		],
	},
	{
		date: "2023/07/20",
		version: "0.99.202",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "72f7ef2d699885f98f364f6978a3457f2e922e16",
						subject: "#889: バックアップアーカイブが腐ってる",
					},
				],
			},
		],
	},
	{
		date: "2023/07/16",
		version: "0.99.201",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "e908580bcc23de1ff16b9fbd0df37246e7e611ef",
						subject: "#888: プラグインテンプレートを整理する",
						comments: [
							"プラグインID生成は API に移譲",
							"各種データパスは変数に置き換え",
						],
					},
					{
						revision: "067e4c2af94caf75e86e7c0d5cd4a06ac45e4c51",
						class: "nuget",
						subject: "MSTest.* 更新",
						comments: [
							"MSTest.TestAdapter 3.0.4 -> 3.1.1",
							"MSTest.TestFramework 3.0.4 -> 3.1.1",
						],
					},
					{
						revision: "700d1fa94b8d153df35c20da83902012319d3920",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2023/07/07",
		version: "0.99.200",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "たなばた",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "3cead801e40861eea9ab5c6c445b3c93c2744420",
						subject: "#885: プラグインテンプレートがだめだめ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "26ff9fab832a0e9b660f454e2f3df7188a150f3e",
						subject: "とりあえず Moq 突っ込み",
						comments: [
							"なんも考えてないけど、とりあえず、とりあえず",
						],
					},
					{
						revision: "cc6b2eea89188a8e37c74b789ec1ff6d77f051ec",
						subject: "#52: ドキュメントコメント",
						comments: [
							"永久に終わらないからもうこれでいったん終了",
						],
					},
					{
						revision: "e47adddde8c100d3c126c87300713f50e7770d34",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.3.0 -> 5.3.2",
					},
					{
						revision: "a0ecf57f6e6e5941e7bb84f001f96ec070b4d7ff",
						class: "nuget",
						subject:
							"CefSharp.Wpf.NETCore 114.2.100 -> 114.2.114.2.120",
					},
				],
			},
		],
	},
	{
		date: "2023/07/02",
		version: "0.99.199",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "8bce1026f28a26337fb8a75f4fae617bf4822e01",
						subject: "#796: ノートの本文を簡易検索可能にする",
						comments: ["検索: Ctrl + F", "次検索: F3"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "5770a462de23f882dec91ab3abe086a398f2a43c",
						subject:
							"npm run help-watch のパスが腐っていたのを修正",
					},
					{
						revision: "4e303192c11031a78a21b4af7542ccbe07620df7",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.6.2 -> 17.6.3",
					},
					{
						revision: "e4702f50e7c561ca02bbc6a1d3821644fc488960",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2023/06/25",
		version: "0.99.198",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "開発用諸々のみ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						class: "compatibility",
						revision: "339d34c8296dcba6a5cf1a22780d07a9688bae8d",
						subject:
							"#880: コマンドラインオプションの '-' を破棄する",
						comments: ["--option 形式だけ使っといてください"],
					},
					{
						revision: "86696e5e46ca265a3b26865599f8a597a4fd73c7",
						subject: "#742: #737 互換処理を破棄する",
					},
					{
						revision: "a9d19078f96392364edf0e303075c79629cd8a92",
						subject: "#882: nodeのバージョンをいい感じにしておく",
						comments: [".node-version を主とする"],
					},
					{
						revision: "0190593b56782324525bc31ac80c925121741172",
						subject: "#815: 機能削除: API-#812",
					},
					{
						revision: "19c604ab0190caa5add2441e99de926cffbe1fda",
						subject: "#775: #735 残対応",
					},
					{
						revision: "7e138df38add97505b9b2c7686d2382ccaff96a2",
						subject: "Dapper 2.0.138 -> 2.0.143",
					},
				],
			},
		],
	},
	{
		date: "2023/06/18",
		version: "0.99.197",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"次バージョンからコマンドラインオプションの '-' を用いた長いオプションは破棄します",
						comments: [
							"前回(0.99.196)に次回って言ってたけど、延期",
							"明示的に使ってる部分はないので問題ないはず",
							"ちょっとこれつらいのです",
							"とはいえ対応するにしてもC側処理めんどいなぁ",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "109a2d9330ecc336fa133de3e785564aeb81b43f",
						subject: "#881: ヘルプファイルの更新履歴が死んでる",
					},
					{
						revision: "8a6e5d4937440d8fb1dac0d0136423be27e3fd89",
						subject:
							"#878: 設定のプラグイン→Web選択時にせめてリンクを表示する",
						comments: ["WPF をさわるのが久々すぎて難しかった"],
					},
				],
			},
		],
	},
	{
		date: "2023/06/15",
		version: "0.99.196",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"次バージョンからコマンドラインオプションの '-' を用いた長いオプションは破棄します",
						comments: [
							"明示的に使ってる部分はないので問題ないはず",
							"ちょっとこれつらいのです",
							"とはいえ対応するにしてもC側処理めんどいなぁ",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "a211cee872d9c7f979c411ace215a5099ddadd85",
						subject:
							"#783: リリースノートに空項目があればビルド処理で落とす",
					},
					{
						revision: "a279fa481d99e2469468fe42e119c8fe67afbdf9",
						subject: "#879: 更新履歴の年月日が所々ミスってる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d5960f9199a4957ed075d7a571f5dd8e5c4e8173",
						subject: "update npm",
					},
					{
						revision: "a211cee872d9c7f979c411ace215a5099ddadd85",
						subject:
							"#783: リリースノートに空項目があればビルド処理で落とす",
						comments: ["俯瞰用にプルダウンも追加"],
					},
					{
						revision: "08a393afaf4d02b497d1956be246c2882802a216",
						subject: "develop ブランチを開発主軸ブランチにした",
					},
					{
						revision: "91243748f38562ebf85c9251c1dd69d63eecd36a",
						class: "nuget",
						subject: "Dapper 2.0.123 -> 2.0.138",
					},
					{
						revision: "05734b58262c48878c2388734a0d70670fef0eb4",
						class: "nuget",
						subject: "System.Data.SQLite.Core 1.0.117 -> 1.0.118",
					},
					{
						revision: "9c1d13b5b639f6050c5b632eccc4dff29a38852c",
						class: "nuget",
						subject: "System.Management 7.0.1 -> 7.0.2",
					},
					{
						revision: "4673d5869aac28dbf7e3bcfcab58fba4bf7d8738",
						class: "nuget",
						subject:
							"ログ周り更新(NLog, Microsoft.Extensions.Logging)",
						comments: [
							"NLog.Extensions.Logging 5.2.5 -> 5.3.0",
							"Microsoft.Extensions.Logging.Abstractions 7.0.0 -> 7.0.1",
						],
					},
					{
						revision: "cf08770c43f44a8c62d9067527e70fe119b74c25",
						class: "nuget",
						subject: "MSTest.* 更新",
						comments: [
							"MSTest.TestAdapter 3.0.3 -> 3.0.4",
							"MSTest.TestFramework 3.0.3 -> 3.0.4",
							"Microsoft.NET.Test.Sdk 17.6.0 -> 17.6.2",
						],
					},
					{
						revision: "3fb644c7d704cca60b6e0518990ab5928a8d8f65",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 113.1.40 -> 114.2.100",
					},
				],
			},
		],
	},
	{
		date: "2023/05/26",
		version: "0.99.195",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "主に依存ライブラリ更新",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "64ed0f9463c5d159b9b54d3dd7a680ca5078c4db",
						subject:
							"#876: プラグインのアップデート処理は実装済みなのでヘルプが古い",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "54d0a8e990a0bd9be34660143211cbaaf9d58c41",
						subject: "#863: CI のテストログをいい感じに取りたい",
						comments: [
							"結局のところログインしてないと見れないっていうね",
						],
					},
					{
						revision: "a447c527ddc4cb89d196d3f584e439fe6419263c",
						subject:
							"#877: CodeQLで抑制しているC#処理はスケジュール処理する",
						comments: [
							"わからんけどこの課題はいったん終わらす",
							"特定ブランチ限定(いまのところ next のみ)で何かあれば個別に対応する",
						],
					},
					{
						revision: "75b280ca5fef58de893fb0ecf69f2d1c7c9bad14",
						class: "nuget",
						subject: "MSTest.* 更新",
						comments: [
							"MSTest.TestAdapter 3.0.2 -> 3.0.3",
							"MSTest.TestFramework 3.0.2 -> 3.0.3",
							"Microsoft.NET.Test.Sdk 17.5.0 -> 17.6.0",
						],
					},
					{
						revision: "d5eed64011804bd3bb2c2184c8552d1a007bf6f2",
						class: "nuget",
						subject: "System.Management 7.0.0 -> 7.0.1",
					},
					{
						revision: "5bd36e3d3a40dd60a31766575781fd6f542b32b2",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.2.2 -> 5.2.5",
					},
					{
						revision: "636d405e744385230e1d08c36d9495681ecb6480",
						class: "nuget",
						subject: "AvalonEdit 6.2.0.78 -> 6.3.0.90",
					},
					{
						revision: "91f5a5106caf16160c050acaf71a005e59a0feaf",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 111.2.20 -> 113.1.40",
					},
				],
			},
		],
	},
	{
		date: "2023/03/16",
		version: "0.99.194",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "034d997842774a8549e4ed6fef75920692e47dc0",
						subject:
							"#845: 自動的に隠したランチャーツルバーの展開に対して待機時間を設定する",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "7b4e238a2372384e11aa114b5ed0be2beac173d1",
						subject: "#816: コマンド入力で上下にサイズ変更可能",
					},
					{
						revision: "7bc4ce03a4fd2296873611df22ec23f680e67c50",
						subject:
							"#765: インデックス命名規則が虫食いを考慮できていない",
						comments: [
							"やりたいことはいろいろあるがまずは虫食い対策",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "69256fee86d77482feffdd75ace5334018cb883f",
						subject: "update npm",
					},
					{
						revision: "e022dbc8333838774906a00c8878d2e1ba8aef35",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.Binder 7.0.3 -> 7.0.4",
					},
					{
						revision: "573695373740f3192b3f79d96dac563fa69972e6",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 110.0.300 -> 111.2.20",
					},
				],
			},
		],
	},
	{
		date: "2023/03/04",
		version: "0.99.192",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "ライブラリ周りの更新のみ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "1152885fe58c189836b50525de1c9cc7525776ca",
						subject:
							"#872: .NET Standard に分離できるものは分離する",
						comments: ["意外といろいろ移せた"],
					},
					{
						revision: "27a51f823d6b72a5e40760e996b58cdb03d4e077",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.4.1 -> 17.5.0",
					},
					{
						revision: "38b6d65fe3d748cf7a997fb07e56002b5b9dd40f",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.Binder 7.0.2 -> 7.0.3",
					},
					{
						revision: "41bf5132a8d4b3fecd09564e2fc92d723bd470cf",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.2.1 -> 5.2.2",
					},
					{
						revision: "f182065621e82ba2a12acdf1b2d696fde6522e61",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 110.0.250 -> 110.0.300",
					},
				],
			},
		],
	},
	{
		date: "2023/02/13",
		version: "0.99.191",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"こういう次に何が起きるかわからん系のバグは早めリリースなのです",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "0ea029e933d561c78b298f600bcd6e4542681c83",
						subject:
							"#871: ノートの最小化時におけるタイトルバー位置変更は無効でないと変になる",
						comments: ["無効化した"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d15eec4293e1b548f0ea5402c06228f3ace904da",
						subject: "Node.js 18.12.1 -> 18.14.0",
						comments: ["ついでに NPM 周りも更新"],
					},
				],
			},
		],
	},
	{
		date: "2023/02/12",
		version: "0.99.190",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "819feed3c15392319741657e0c19c5763a30fb45",
						subject:
							"#751: ノートのタイトルバー位置で横配置を可能にする",
					},
					{
						revision: "1bdc8916e2bb0dc2f524a16876e27610371fd33b",
						subject: "#366: ノートにファイルを添付する",
						comments: [
							"ノートに対してファイルをD&Dすることでファイルへのリンクをノートに添付します",
							"ファイルリンクを削除するにはコンテキストメニュー(右クリック)から操作してください",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d367e82f8eb2d73778e21dfcfe261971af7d8cd9",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 109.1.110 -> 110.0.250",
					},
				],
			},
		],
	},
	{
		date: "2023/02/05",
		version: "0.99.189",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "d6e2f809c05aa56eed1a558bfb6d4600f7b0d8bf",
						subject:
							"#866: 参考実装プラグインのバージョン制限を行う",
						comments: ["System.Version の比較が手遅れ感すごい"],
					},
					{
						revision: "5ac680c17fa50617c53753cb8ae7fa7648adc7a6",
						subject:
							"#757: コマンドランチャー表示時にIMEを制御する",
						comments: ["アクティブ時っぽい動きだけど一応これで"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "8d93fa2a3bb811d0bced79ca7d1092b1df4a0ddb",
						subject:
							"#867: obj をキャッシュしとけば CI ちょっとは早くなるのじゃないじゃろうか",
						comments: ["気分だけ速くなった。正直なんも変わらん"],
					},
					{
						revision: "d9cb7be24535db36b496f234f60e340403b18cb9",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2023/02/01",
		version: "0.99.188",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"本バージョンで Windows 7, 8, 8.1 をサポート対象外とします",
						comments: [
							"win7 って 2009 年生まれなのでもういいでしょ",
						],
					},
					{
						revision: "fcfd9f9d4cccfba2677b0e9d905f6bd51705f3b9",
						subject: "#865: 0.99.187 は死んでる",
						comments: [
							"インフラの構造変更によりプラグインが死にます",
							"このドキュメントのためにバージョン繰り上げ",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "d30a3a8b9c22b65ef3fd123212d798891cbe6e91",
						subject:
							"#859: プロキシ設定の有効無効を本体コマンドに組み込む",
					},
					{
						revision: "85950d405765148159a4abbbcdf2d6af38e615fc",
						subject:
							"#763: 表示領域が満員時のランチャーツールバーへのD&Dがメインアイコンの時は登録可能にする",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "34041a801d5191c03865f627a2cbc72db08a1edf",
						subject:
							"#858: ノートの所属ディスプレイはユーザーによる移動・サイズで変更する",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "fc8ff5362e99f3113d6c2c19ea56bf12e3e702a5",
						subject: "#841: Windows 7/8/8.1 対応を終了",
						comments: ["Windows RID(dotnet --runtime)のみの変更"],
					},
					{
						revision: "625177bae9699f3580ab40771289bbf7c83eb77d",
						subject: "#849: 弱参照イベントを組み込む",
						comments: [
							"ぱっと見えた範囲のみ実装対応。インフラとしての実装なので今後対応していく",
							"追加対応: 01f946c432b1dc5eaa3ac7e0a9695b50bf64f8dc",
						],
					},
					{
						revision: "2ea2fb3d39bb9e72d9df7f0ca66911d64e6b7b89",
						subject:
							"#861: 課題対応ブランチでは CodeQL を実行しないようにする",
						comments: ["next ブランチか PR で動かすようにした"],
					},
					{
						revision: "0849103dadef0692908e3044024cba6aaacceb79",
						subject: "#850: 謎データ構造を破棄する",
					},
				],
			},
		],
	},
	{
		date: "2023/01/23",
		version: "0.99.186",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"次で Windows 7, 8, 8.1 をサポート対象外とします",
						comments: ["#841: Windows 7/8/8.1 対応を終了"],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "e203677d787b90d779aff85c183349bff69fdafd",
						subject:
							"#852: ノートのウィンドウ操作中はステータス部分を目立たなくする",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "614e53046458546e7c7555e14ca57cfb74d99160",
						subject: "#846: Pe.Server に対するドキュメントの追加",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "6c6fd36f56992c64bd2fea0b91b88fb51ca2161c",
						subject: "#853: CI のキャッシュに乗せるために歪んでる",
						comments: [
							"RestorePackagesWithLockFile は忘れる",
							"packages.lock.json も破棄",
						],
					},
					{
						revision: "6530ab94b9e7f10de045c91352d0cdef8c696327",
						subject: "#854: リリース版に PDB は不要",
						comments: [
							"残った子たちは消すのも面倒なので残しておく(アップデートスクリプトに突っ込むのもしんどい)",
						],
					},
					{
						revision: "6ee7fc534870a7e3a3e69152b644919ec71821de",
						subject:
							"#855: リリース時の PeServer 通知処理はなくすべきでは",
					},
					{
						revision: "9625d676857879ee279a37f737ff2bf99c9cfd36",
						subject: "update npm",
					},
					{
						revision: "e5b5ef6489630376216a1fd510e6036b78176c3e",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Configuration.Binder 7.0.1 -> 7.0.2",
					},
					{
						revision: "d533946e57ad4ea717ec49fb79ca95bab733d0f2",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 108.4.130 -> 109.1.110",
					},
					{
						revision: "54015a3eae18629ab434977c08d74a24f65b838a",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.2.0 -> 5.2.1",
					},
				],
			},
		],
	},
	{
		date: "2023/01/09",
		version: "0.99.185",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"次の次で Windows 7, 8, 8.1 をサポート対象外とします",
						comments: ["#841: Windows 7/8/8.1 対応を終了"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "8e0031b73a41c285797ab09a67d3897c214c6d74",
						subject: "C17適用",
					},
					{
						revision: "8be028c23e51f48cc59c925723cb6b595113ab1f",
						subject: "#718: ランチャーアイテムの削除処理を一元化",
					},
				],
			},
		],
	},
	{
		date: "2022/12/28",
		version: "0.99.184",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "依存ライブラリ系の更新",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "80a6c6a51e4d8ffa1c6eb5067c6a953e52f7b0fc",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 107.1.120 -> 108.4.130",
					},
					{
						revision: "19cea839522f9d689babdcf297c82dba42cb2ea9",
						class: "nuget",
						subject: "MSTest.Test* 更新(2)",
						comments: [
							"MSTest.TestAdapter 3.0.0 -> 3.0.2",
							"MSTest.TestFramework 3.0.0 -> 3.0.2",
						],
					},
					{
						revision: "8a65f822341bf578e10868bfc4258b82365a3eb2",
						subject: "NLog.Extensions.Logging 5.1.0 -> 5.2.0",
					},
					{
						revision: "d1d23736b488b64149d6b03b1e8ff56037926064",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2022/12/19",
		version: "0.99.183",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "7612e4536e78dc77d8e2be955a0296c42e1ed02d",
						subject:
							"#839: 指定して実行の初期フォーカスをオプションに割り当てる",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "e0d0c97d465e6bdd7524810733a1a8a47a303820",
						subject:
							'#838: Fix code scanning alert - Arbitrary file write during zip extraction ("Zip Slip")',
						comments: [
							"これあかんかったら次リリースが死ぬっていう結構な対応",
						],
					},
					{
						revision: "dfb616f08c95d1b49fcba8cb5e192e551961807b",
						subject:
							"#840: 指定して実行のオプションとか開くと閉じたときに死ぬ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "daf1ba57753765746b91c2d86e32231e237dbfec",
						subject: "#831: AppVeyor 処理設定を破棄する",
					},
					{
						revision: "4bc96e418ee4fa4a96c593a75ead8985c603d0af",
						subject: "Node.js 18.4.0 -> 18.12.1",
					},
					{
						revision: "342cf49292de99f060af80aa4cb5db6aa2531c46",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.4.0 -> 17.4.1",
					},
				],
			},
		],
	},
	{
		date: "2022/12/16",
		version: ["0.99.181", "0.99.182"],
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "0.99.181 が死んだので 0.99.182 と統合",
					},
					{
						subject:
							"特に意味はないけど依存ライブラリの都合上 更新だけしときたい",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "856d2bff21d75be7833e94afd453573e9755cf46",
						subject: "#830: CI で Nuget のキャッシュできてない",
						comments: [
							"nuget にロックファイルがあると初めて知った冬の日",
						],
					},
					{
						revision: "3201627e86c7dc0d0a2bf7baf3e066252386d044",
						subject: "#835: ヘルプファイルのリンクが腐ってる",
						comments: [
							"parcel がつらい。 でも webpack に移るんもつらい。 かといって eleventy は ts ダメっぽいから全部つらい",
						],
					},
					{
						revision: "0c567e21ed20c65adcf61594c55da76b050835be",
						subject:
							"#834: CI で JSON のなんか知らん警告を解決する",
					},
					{
						revision: "783898d15c93b490517da0867e0abf570b5c0ad1",
						subject: "#837: #834 対応の JSON が死んでる つらい",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "6ddb88e1faf57f7bcd53bd361265a24e10e3c4ea",
						subject: "update npm",
					},
					{
						revision: "9861a118f161f7c903bf2c1918abfa82c73af134",
						subject:
							"#789: bitbucketにソーシャルログインできるしフォーラムを畳む",
					},
					{
						revision: "99749dcedc605e8d65a36b4fe9beacd2c1e24131",
						subject:
							"#833: Fix code scanning alert - Unsafe expansion of self-closing HTML tag",
					},
					{
						revision: "3a3e0b0cde8fe689856956fc16512ce746a990f7",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 107.1.90 -> 107.1.120",
					},
					{
						revision: "d4c33175244e4a6a6657071fb2a64f0f4580634b",
						class: "nuget",
						subject: "AvalonEdit 6.1.3.50 -> 6.2.0.78",
					},
					{
						revision: "c955b67de0a747036132a2e411219b4d3c010336",
						class: "nuget",
						subject: "MSTest.Test* 更新",
						comments: [
							"MSTest.TestAdapter 2.2.10 -> 3.0.0",
							"MSTest.TestFramework 2.2.10 -> 3.0.0",
						],
					},
				],
			},
		],
	},
	{
		date: "2022/12/13",
		version: "0.99.180",
		group: ".NET 7",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: ".NET 7!!",
						comments: ["動くかは知らん!"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "75517fc062cb03c1c38ddb79d6d0d7e229b814b6",
						subject: "#828: .NET 7 へ移行",
					},
					{
						revision: "b873f9ed2a709638e79acfbbb3ac857cb704f9f3",
						class: "nuget",
						subject: "Microsoft.Extensions.* 系を更新",
						comments: [
							"Microsoft.Extensions.Logging.Abstractions 6.0.2 -> 7.0.0",
							"Microsoft.Extensions.Logging 6.0.2 -> 7.0.0",
							"System.Management 6.0.0 -> 7.0.0",
							"Microsoft.Extensions.Configuration.Binder 6.0.0 -> 7.0.0",
							"Microsoft.Extensions.Configuration.Json 6.0.0 -> 7.0.0",
							"System.DirectoryServices.AccountManagement 6.0.0 -> 7.0.0",
							"System.Text.Encoding.CodePages 6.0.0 -> 7.0.0",
						],
					},
					{
						revision: "0e7198360983d8a6964a6de058981b775c50d896",
						class: "nuget",
						subject: "System.Data.SQLite.Core 1.0.116 -> 1.0.117",
					},
					{
						revision: "feaa54536d904f66ebd179e7f6087cbca8f88de5",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.3.1 -> 17.4.0",
					},
					{
						revision: "3e2e1b2607df826a68c3456e6dd22e164e5d9ce1",
						subject: "#826: Code Scanning を使ってみる",
					},
					{
						revision: "290c5f040cc5c2b11d25af1451104b6ca8a370f0",
						subject:
							"darenm/Setup-VSTest@v1 -> darenm/Setup-VSTest@v1.2",
					},
					{
						revision: "95a140c38d18d1fc50858a742b8e032aafaed385",
						subject: "CI処理をまとめた",
					},
					{
						revision: "f0cfa896f9090e4c31963f71bd5f5174e3495a51",
						subject: "#825: ヘルプファイル系の再作成",
						comments: ["併せて #730 もがんあげ"],
					},
				],
			},
		],
	},
	{
		date: "2022/11/25",
		version: "0.99.179",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"GitHub 移行後の初アップデートリリースなのでこわい",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "3e36e9083ad8878f18b0187acc423831af238e5b",
						subject:
							"#818: リリース時にPe.Serverに対してバージョンアップ通知が行えていない",
					},
					{
						revision: "463da86efff464c54bb1688f1373f3e1a3591781",
						subject:
							"#819: アップデート時のリリースノートが成果物に含まれていない",
					},
					{
						revision: "405cee2317be454604e6d0d43d17b305ce627bfa",
						subject: "#827: GHA: set-output は非推奨",
						comments: ["依存系は未対応"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "c8f592400c9fb3487a478edf15fb120277baf83f",
						subject:
							"#821: リリース版CIのキャッシュは行わないようにする",
					},
					{
						revision: "08fca26ae5e73c797b525259d4f0c26a11beb00a",
						subject: "#822: インストーラの調整が必要",
						comments: [
							"ドキュメント周り整理",
							"インストーラ作成中に分かったけどリリースの直近版をとれるなら #817 で Pe.Server 巻き込んであれこれした意味よ",
						],
					},
					{
						revision: "fd83be738a1ae188b9ed594e09e863c0a70d1b91",
						subject:
							"#823: リポジトリに対するライセンスファイル追加",
					},
					{
						revision: "d083047ba38913dd35748f276b9f57b145c07348",
						subject:
							"#824: 各種ドキュメント・ソースの Bitbucket を github へ置き換える",
					},
					{
						revision: "3166bccc55f60f2941a777e0ea3f1430905bd950",
						subject: "#787: CI上のnpmはinstallではなくciすべき",
						comments: [
							"コマンド変更より CI 上での諸々処理変更が一番しんどかった",
						],
					},
					{
						revision: "7fe392dec6ebbf6fd156a0871baf5ec0df61b387",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.0.4 -> 5.1.0",
					},
					{
						revision: "5aed2c4f8085cdd5df050831bc8bd38405fd01fb",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 105.3.390 -> 107.1.90",
					},
				],
			},
		],
	},
	{
		date: "2022/10/08",
		version: "0.99.178",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						class: "compatibility",
						subject:
							"色んなものを github に移動中で色んなものがうまく動かないかも",
						comments: [
							"Pe.Server との兼ね合いもあり本体アップデートは危うい",
							"参考実装プラグインはもっと危うい",
							"取り急ぎ対応なので各ドキュメント等は bitbucket 状態",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "b0c34ac6e51f58ca238b5da72abe422a3317fc9f",
						subject: "#817: github に移行できるか調査",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "2617b9877576948cd0fe9e4fdf822f50a3166a83",
						subject:
							"#813: 32bit 実行ファイルがコマンドランチャー起動時に死ぬ",
						comments: [
							"GetWindowLong(Ptr)/SetWindowLong(Ptr) の問題がまた今ここに",
						],
					},
					{
						revision: "0e7eb01f1af9d0e4dde1358f1e259d9fa60a8b03",
						subject:
							"#811: ノートの最小化・復元がWindowsのシステムメニューに準拠していない",
						comments: [
							"コマンドウィンドウは今回ガン無視",
							"Window.SourceInitialized の時点で反映されないしなんなのこれ",
							"FrameworkElement.Initialized ならいけたんかもしれんけど泥臭い手法で対応した",
						],
					},
				],
			},
		],
	},
	{
		date: "2022/09/20",
		version: "0.99.177",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject: "普段操作しない部分が死んでるのほんとつらつら",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "6a165d45f7d9ec2b5a126c4d39a8d313edb921b0",
						subject:
							"#814: ランチャーツールバーの「自動的に隠す」切り替えで死ぬ？",
						comments: ["#807から死んでる疑惑"],
					},
				],
			},
		],
	},
	{
		date: "2022/09/19",
		version: "0.99.176+",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "71e361fcd9ba04d996e8c7d0635587d7486c0cc1",
						subject: "0.99.176 は内部的に 0.99.176+ を正とします！",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "547d3e3452da76a9e952eca534fd917911198eac",
						subject:
							"#812: API に GAS 使ってると世話し忘れたときに権限で死ぬ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "2a9643b772c0b535276e1d4f1376544f37576bb0",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.0.1 -> 5.0.4",
					},
					{
						revision: "b620063858eeb6d4bc5f641ae5df60712ab32fef",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 102.0.100 -> 105.3.390",
						comments: [
							"chromiumembeddedframework.runtime.win-arm64 102.0.10 -> 105.3.39",
							"chromiumembeddedframework.runtime.win-x64 102.0.10 -> 105.3.39",
							"chromiumembeddedframework.runtime.win-x86 102.0.10 -> 105.3.39",
						],
					},
					{
						revision: "47250df7dfcb8233f5cb94612813251c8f5a850b",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Logging.Abstractions 6.0.1 -> 6.0.2",
					},
					{
						revision: "a67e4d796bd9456887accbdd0e2b44a1b8179834",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.2.0 -> 17.3.1",
					},
				],
			},
		],
	},
	{
		date: "2022/06/26",
		version: "0.99.175",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "cf8aedc29c53d3649e985097c7283cec0597802f",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 101.0.180 -> 102.0.100",
					},
					{
						revision: "505de09688e97fe97c8a2f30a48b81af3fc9e42b",
						class: "nuget",
						subject: "NLog.Extensions.Logging 5.0.0 -> 5.0.1",
					},
					{
						revision: "4228e0ebc8c0f527dfe17c662d8176d5a8873ee0",
						class: "nuget",
						subject: "System.Data.SQLite.Core 1.0.115.5 -> 1.0.116",
					},
				],
			},
		],
	},
	{
		date: "2022/05/17",
		version: "0.99.174",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject:
							"プラグインデバッグのVS 17.2.0設定が\\をエスケープせずにJSONで使うので辛い",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "d8aadcb40de2e3fb730098012a3afb2f212a058b",
						subject:
							"#810: ファイルランチャーアイテム起動時にウィンドウ状態を選択可能にする",
					},
					{
						revision: "731ebfe8efe90483a1fcb1b4193e695941f917df",
						subject:
							"#806: プロキシ有効・無効を通知領域メニューから切り替えられるようにする",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d6d4dee0ba4821aa729293f921010bbcb1b7f01b",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.7.4 -> 5.0.0",
					},
					{
						revision: "75aeed1613a21c772f29e4d84d9c36bc061c5532",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 101.0.150 -> 101.0.180",
					},
					{
						revision: "c3474019535032b0e1170002de628391d11b5c71",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.1.0 -> 17.2.0",
					},
				],
			},
		],
	},
	{
		date: "2022/05/10",
		version: "0.99.173",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"#808 死んでるし #807 も怪しいけど #808 が危なすぎる",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "7f5443ddb164cd18c9ade70e45b8311f8a277ec3",
						subject:
							"#792: プラグインテンプレートからいい感じにプロジェクトが生成できない",
					},
					{
						revision: "9ed8dea46b1e8d0d35138fbcbb2460a599a6dde0",
						subject:
							"#808: ランチャープラグインアイテムとして設定可能なプラグインが本体設定時に死ぬ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d74633e9b734d0925b833445ae51889301b71c78",
						subject: "#807: SQL処理に制約を追加",
					},
				],
			},
		],
	},
	{
		date: "2022/05/08",
		version: "0.99.172",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"外部プラグイン(interfaceとPe.Embedded)のためのリリース",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "940ec5ae889bc624bed452b5eed3fa75e9aecf2b",
						subject:
							"#749: ノートリンクで元ファイルが掴まれている場合、死ぬ",
					},
					{
						revision: "b6c88e2f1c367ed8d73d132f170f5aa5f0fca9df",
						subject: "#804: 専用型を拡張する",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "2a9adba632c8ef122b5919cb44f9f200d64f20e9",
						class: "nuget",
						subject: "CefSharp.Wpf.NETCore 100.0.230 -> 101.0.150",
						comments: [
							"あとなんか知らんけど chromiumembeddedframework.runtime.* が入った",
							"Nuget 利用者的には知らんけど",
						],
					},
					{
						revision: "d2e17498a2aa8e125fd4ff202db642d6e181bb88",
						subject: "MSTest系更新",
						comments: [
							"MSTest.TestAdapter 2.2.9 -> 2.2.10",
							"MSTest.TestFramework 2.2.9 -> 2.2.10",
						],
					},
				],
			},
		],
	},
	{
		date: "2022/04/22",
		version: "0.99.171",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "このバージョンは動くかどうかよくわからん",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "e0a91e3c49b211bc122ac8fca8e68b560024f615",
						subject:
							"#803: プラグインをWebからインストール可能にする",
						comments: ["動くかどうかは知らん"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "7c300ea378f2e226ac1041f2fcdf738d30ff8c9a",
						subject: "#802: テーマ選択ができていない",
					},
					{
						revision: "16927da11c417e60bc36e2628f3b90083436e2e5",
						subject:
							"#801: Pe 情報コピーでコピーライトの年表示がおかしい",
						comments: ["おかしいも何も固定文字列だった"],
					},
					{
						revision: "f007ada3012531ed414c1d4057af4c28607cadf4",
						subject: "#794: ノートのタブインデックスが狂ってる",
					},
					{
						revision: "b60ed63402db3379d44f6d6e5f59fd089ac74b6e",
						subject:
							"#722: 通知ログがウィンドウアクティブ状態をまだまだまだまだ奪う",
						comments: ["そろそろコレしんどい"],
					},
					{
						revision: "40b928bf3a446ae23e5662f5c6f8c2e5eec97c29",
						subject:
							"#772: コマンドランチャーのアイコンがプラグインIF経由時に壊れる",
						comments: [
							"めんどいのでアイコンを破棄した",
							"ソースのコメントでもいらんっぽかったし",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "602531069fc0f6a8bdf68d12346b2f43bebb5f90",
						subject: "#706: プラグイン配布サイトの構築",
						comments: [
							"https://peserver.gq/plugin",
							"とりあえず作るだけ作った",
							"運用云々は後で考えるとして Pe 本体の課題からは外す",
							"更新用URI処理もえいやで取り込んだ",
						],
					},
					{
						revision: "a6c52c8fbd1e533798dbd5e088a7fa41c9b5eb88",
						class: "nuget",
						subject: "MSTest系更新",
						comments: [
							"MSTest.TestAdapter 2.2.8 -> 2.2.9",
							"MSTest.TestFramework 2.2.8 -> 2.2.9",
						],
					},
					{
						revision: "46e47ad39571171c215201cfc3b5dde1365dba06",
						class: "nuget",
						subject: "SevenZipExtractor 1.0.16 -> 1.0.17",
					},
					{
						revision: "a770e264650d5540f322b339f1247bc090e63b1a",
						subject:
							"CefSharp を CefSharp.Wpf から CefSharp.Wpf.NETCore に変更",
						comments: [
							"CefSharp.Wpf 86.0.241 -> CefSharp.Wpf.NETCore 100.0.230",
						],
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
		date: "2022/03/28",
		version: "0.99.170",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"0.99.169(2022/03/27 21:04 65983c21bfe68d834f5d3e77ae1705d4fbf67020)は致命的にバグってるので除外",
					},
					{
						class: "compatibility",
						subject:
							"#778 対応によりプロキシ設定は本体設定機能に取り込まれました",
						comments: ["既存のプロキシ設定は無効化されます"],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "0f75a88e690fef216ef5fb6332e137bbaf938cb0",
						subject:
							"#778: プロキシをアプリケーション設定から編集可能にする",
						comments: [
							"残課題は #797 で対応する",
							"既存プロキシ設定はすべて無視",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "07448dfb753985cc29e5c6366799ab44a760e117",
						subject: "#799: プラグイン更新の通信処理が腐ってる",
						comments: [
							"IPC(プラグイン処理)の場合にDBデータを取得できずに死んでいた",
							"プラグインをインストールしていない場合は無関係だけどまぁ",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "4a5e6d54456eed5a6981862813a345867a329e0f",
						subject: "#798: IDの型を専用にする ",
					},
					{
						revision: "8e635213f723db5b8ab93b915e38513d42a70add",
						class: "nuget",
						subject:
							"Microsoft.Extensions.Logging.Abstractions 6.0.0 -> 6.0.1",
					},
				],
			},
		],
	},
	{
		date: "2022/03/13",
		version: "0.99.168",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "機能的には特に何もないリリース。",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "421f89eea122ff04e166da7623415a30935fa157",
						subject:
							"#795: hash_map/linked_list の値解放処理型が腐ってる",
						comments: [
							"久しぶり過ぎて何が正しいのかもう何もわからない",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "83e5488401e9dfcc711c0c4a8a4bab743eb2eab9",
						subject: "update npm",
					},
					{
						revision: "794d92324bdb42b9fe206883d579f886774acaeb",
						class: "nuget",
						subject: "AvalonEdit 6.1.2.30 -> 6.1.3.50",
					},
					{
						revision: "27cef0ae10a59c1f71d8a69a5ebf2af055d32345",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 17.0.0 -> 17.1.0",
					},
					{
						revision: "80ffcf3d7ba368800440534640dc3db6e1c544cf",
						class: "nuget",
						subject: "SevenZipExtractor 1.0.15 -> 1.0.16",
					},
				],
			},
		],
	},
	{
		date: "2021/12/07",
		version: "0.99.167",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "f1b43e8ae52c5c1500e0b636f876f12a40c48def",
						subject:
							"#791: 端末がスリープから復帰時にフック処理が有効になっていない",
					},
				],
			},
		],
	},
	{
		date: "2021/11/28",
		version: "0.99.166",
		group: ".NET 6",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"実行基盤を .NET 5 から .NET 6 にバージョンアップ",
						comments: [
							"たぶん大丈夫だと思うけどテスト全然してないのでなんとも",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "8df5ebdcb506899ca295d12da0c9be8e1ac6b079",
						subject: "#782: VS 2022+.NET 6+C#10への移行",
						comments: [
							"Microsoft.Extensions.Logging.Abstractions 5.0.0 -> 6.0.0",
							"Microsoft.Extensions.Logging 5.0.0 -> 6.0.0",
							"Microsoft.Extensions.Configuration.Binder 5.0.0 -> 6.0.0",
							"Microsoft.Extensions.Configuration.Json 5.0.0 -> 6.0.0",
							"System.DirectoryServices.AccountManagement 5.0.0 -> 6.0.0",
							"System.Text.Encoding.CodePages 5.0.0 -> 6.0.0",
							"System.Management 5.0.0 -> 6.0.0",
							"Microsoft.NET.Test.Sdk 16.11.0 -> 17.0.0",
							"System.Data.SQLite.Core 1.0.115 -> 1.0.115.5",
							"Dapper 2.0.90 -> 2.0.123",
						],
					},
					{
						revision: "76aacc7dd4d1f0c5bfc0773dcb7a2e1a260d7095",
						subject: "#782 実施後にテストフレームワーク再更新",
						comments: [
							"MSTest.TestAdapter 2.2.7 -> 2.2.8",
							"MSTest.TestFramework 2.2.7 -> 2.2.8",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "a87759b68c0ea6677bf3d38769aa935356014196",
						subject:
							"#784: プラグインテンプレートの取得ダメくない？",
						comments: [
							"正直ダメでよかった",
							"他に問題がいっぱいあったしまだある",
						],
					},
					{
						revision: "727409cc2fe086544b4c19c28dddc6a18ef8e0af",
						subject:
							"#786: プラグインのテンプレで隠しファイル(.ファイル)がアーカイブされていない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "6dd5f27c8fc8e9e4aedb7460a2cfb1d28bbd6d93",
						subject: "アーカイブ作成時の上書き問題対応",
					},
					{
						revision: "53a01f75305f23a71813912188a5704d317d335d",
						subject: "ビルド時警告の抑制",
					},
					{
						revision: "8404eed06f39e1d58b2bbba4a7b9f9ebbaa4cf2a",
						subject: "[継続課題] #730: node/npm のバージョンアップ",
						comments: [
							"メジャーバージョン上げたら死ぬのでマイナーバージョンを最新化",
							"各種パッケージモジュールも更新",
						],
					},
				],
			},
		],
	},
	{
		date: "2021/11/16",
		version: "0.99.165",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "6189ea9e4aea8fad6b740ed575b6d0305cd866a5",
						subject: "表示要素リセット時にフックを抑制した",
					},
					{
						revision: "9e195424a687eb2dcb045b56e906fa27d1fbdc0b",
						subject:
							"ヘルプ: テーブル一覧のinputの幅を親サイズに収めた",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d575acf5cfa81193193499218e8f7694c75746b5",
						subject: "#777: プラグイン実装テンプレートの作成",
						comments: [
							"いろいろおかしいところあるし失敗するしでハチャメチャだけど一応大丈夫なのでリリースにのせた",
							"ヘルプドキュメントを参照のこと",
						],
					},
					{
						revision: "d66e07578efd46f7ad812311cde2f3a442dbac7d",
						subject: "#780: 開発中CIの圧縮を選択可能にする",
					},
				],
			},
		],
	},
	{
		date: "2021/10/09",
		version: "0.99.162",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "このバージョンちょっと怪しいかも",
						comments: [
							"2021/10/09版はアップデートURLの不備により2021/10/10に再リリース(ドキュメントの日付・バージョンはそのまま)",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "e47231c06fc1264be420cfd9ddfdb9ec32da9c8f",
						subject:
							"#735: プラグイン参照実装を自動アップデート対象にする",
						comments: [
							"全パターン検証できてないけど！",
							"実装中に分かったけど、実行中アセンブリの上書きが誰も幸せにならくて辛いっていうか、そもそもプラグイン機構としてなんなのMSもうマジでもうマジで。doc見る限り同一(次バージョン)アセンブリの話ししてなかったやん！",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "b0cf6184f4ecbea6cf7815de27c5a5522848feea",
						subject:
							"#773: 文字列フルフル64bitを考慮する必要ないでしょ",
					},
					{
						revision: "3d68ef771907fc1faa825a65d80393350385a747",
						class: "nuget",
						subject: "MSTest系更新",
					},
					{
						revision: "87d481a76b025df52b2cedd80f7d3adf5b734160",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.7.3 -> 1.7.4",
					},
					{
						revision: "112e8c4f9fa2e40629fbad55c21e82b53bc35c06",
						class: "nuget",
						subject: "System.Data.SQLite.Core 1.0.114.3 -> 1.0.115",
					},
				],
			},
		],
	},
	{
		date: "2021/08/04",
		version: "0.99.158",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "044fec4522372dced95bf9a001f870aad9fe5ef9",
						subject: "#527: 本体起動用処理をもうちっときれいにする",
						comments: [
							"軽い感じで綺麗にするつもりが壮大な作り直しプロジェクトになった",
							"本体を動かす程度は動くけどいまだ未完成(printf実装とかもうしんどい)",
						],
					},
					{
						revision: "7427c3df835cedd1ce1cef7b6c4b1721cd794151",
						subject:
							"#769: ランチャーアイテム自動取り込み後に取り込んだ旨のメッセージを表示する",
						comments: [
							"チェックマークを付けるようにしつつスタートアップも同じようにした",
						],
					},
					{
						revision: "b6bf99b19e1c3b0166913f6e2db212d7ef6b1f53",
						subject:
							"#767: β版実行時の警告メッセージにバッチから動かす旨を記載する",
					},
					{
						revision: "e001f7ff46078dce5fe81da1dc7cbc93e068d61b",
						subject:
							"[継続課題] #766: アップデート時に事前にEXEを一通り動かす",
						comments: [
							"一旦は失敗しても処理を進めるようにした",
							"次の次くらいのリリースで失敗処理をきちんとする",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "25f76f2d57d629d7c79b79d579b986b21fab1ced",
						subject:
							"#770: 使用可能前にダイアログ系(スタートウィンドウとか)を動かすとスケジュール処理再起動で死ぬ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "728aed2da2938b44c7ee04f037d44096289fe2a6",
						class: "nuget",
						subject: "MSTest系更新",
						comments: [
							"Microsoft.NET.Test.Sdk 16.9.4 -> 16.10.0",
							"MSTest.TestAdapter 2.2.3 -> 2.2.5",
							"MSTest.TestFramework 2.2.3 -> 2.2.5",
						],
					},
					{
						revision: "aa17bb39e838e7779f1e3e78f54f111c47a9ded7",
						subject: "#771: CI上のテストをスクリプトファイルにする",
					},
				],
			},
		],
	},
	{
		date: "2021/06/29",
		version: "0.99.154",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "9dc9d3d047ac811efe1fa54a584983e6067468f4",
						subject:
							"#758: ランチャーツールバーから新規グループを作成可能にする",
					},
					{
						revision: "029208941d827d1d6e002d7c30dbf36b6cf9d8a7",
						subject:
							"ランチャーツールバーメニューの表示状態をグループ化",
					},
					{
						revision: "5b3e954d3f96174298e942e1620a79d91966bb6f",
						subject:
							"#745: ノートの自動的に隠す方法の時間を設定可能にする",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "a602d8004416b5e304d10bc5639a35ce1a5a114f",
						subject: "#761: 作業ディレクトリを開く機能死んでね？",
					},
					{
						revision: "c01a3d48ebb6b45ae8424742e70902a6d9f662f4",
						subject:
							"#764: 設定のバックアップにバージョン情報を付与する 二回目",
						comments: ["一回目の #470 が #484 で死んだ"],
					},
					{
						revision: "6e0ca7121c008b7d2fa4babf5303aa6f6b8b74b7",
						subject: "#760: ノートの新規追加時に前面に表示されない",
					},
					{
						revision: "9f9902716f40b59468c4195785206419063df8ec",
						subject:
							"#759: ランチャーツールバーへのD&Dで指定して実行ダイアログを開いた際に前面表示されない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "bee9bcb3d8ed708c11c90de7ef26f86859665e3d",
						class: "nuget",
						subject:
							"System.Data.SQLite.Core 1.0.114.2 -> 1.0.114.3",
					},
					{
						revision: "dd5ea7febc409db086c8a9efcc6f4556180adccd",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.7.2 -> 1.7.3",
					},
				],
			},
		],
	},
	{
		date: "2021/06/17",
		version: "0.99.143",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "9a22c9aedaafa6db2d2390ae5909b81a3067c184",
						subject: "#743: ノートのキャレット色を前景に合わせる",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "796994923e8ae4918f012ed25b890b651585fa05",
						subject:
							"#748: ノートリンクを切断時にファイル内容を本文に割り当てられていない",
					},
					{
						revision: "78f52fbb56083f521f2724214624d275feedecd8",
						subject: "#756: ノートリンクRTFでやたらとゴミが入る",
						comments: [
							"RTFが目立つってだけでプレーンテキストもごみってた",
						],
					},
					{
						revision: "21bb47b1262d33f219adf9d8575fc9f5677f2c95",
						subject:
							"#755: プラグインインストール時にアーカイブ名の x86/x64 で探しに行こうとするので該当プラグインが見つからない",
						comments: [
							"拡張子を抜いたファイル名末尾 _x86, _64, _AnyCPU を無視するようにした(AnyCPUは意味あるか知らん)",
						],
					},
				],
			},
		],
	},
	{
		date: "2021/06/06",
		version: "0.99.137",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "658cd107e56b5fcd1ff988f9a9352f71a3566356",
						subject:
							"#741: 指定して実行の履歴アイテムを破棄できるようにする",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "02d65a0b5be0901078159e127515b6d33c3463d1",
						subject:
							'#738: アンインストール時のスタートアップ処理に対して " の付与が過剰',
					},
					{
						revision: "b631b39b0707e7b61efa7c9b8d1fa07ac162f546",
						subject:
							"#739: フィードバック送信後のコンテンツ領域リンクが見づらい",
					},
					{
						revision: "92a93f4c1f3708c0494b0737271b19e7dd5fb002",
						subject:
							"#736: ノート リンク ファイルダイアログのエンコーディングに utf8bom が二件表示される",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "edf585a6fac1bef8fd6f80da0c89ae923d5fe4f1",
						class: "nuget",
						subject: "AvalonEdit 6.1.1 -> 6.1.2.30",
					},
					{
						revision: "2cd4b784fcca951b81b81049a27faa78b22163c9",
						class: "nuget",
						subject: "Prism.Wpf 8.0.0.1909 -> 8.1.97",
					},
					{
						revision: "3eda349d262ecb5cef233112c015dfebde43026b",
						class: "nuget",
						subject:
							"System.Data.SQLite.Core 1.0.113.7 -> 1.0.114.2",
					},
					{
						revision: "44c302a48804738f5fe326a0359318fafb42a56d",
						subject:
							"#744: アップデート通知用ファイルをバージョンごとに生成する",
					},
					{
						revision: "fa181e0e6b91926d7e76700048d5de3d07b91261",
						subject:
							"#737: 起動処理に渡すコマンドライン引数を本体側で将来的に絶対に競合しないものにする",
					},
				],
			},
		],
	},
	{
		date: "2021/05/24",
		version: "0.99.126",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "b91bf956224ad1a6b7c54e73aa13e2f7c3b3b578",
						subject:
							"#732: アプリケーション構成ファイルのログ出力は独自実装ではなく.NET提供の出力方法を使用する",
					},
					{
						revision: "cb8153a7dfd3812be19d7a5e7b971f8e18e53833",
						subject:
							"#734: ファイルダイアログのフィルターがローカライズ未対応",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "d2084778d9542e7e76671232756a5a076e5bed67",
						subject: "#731: クラッシュレポートが死んでる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "e3f802865b326f7520042e206f6eb8712691798e",
						subject: "update npm",
					},
					{
						revision: "e4209f76ce113309be1da6a98fb27a95eef1a21e",
						subject: "#727: プラグイン参照実装をCIにのせる",
						comments: [
							"CIに乗せたのでCDでダウンロードページまで移送されることになった",
							"参照実装プラグインの扱いについてはヘルプを参照のこと",
						],
					},
				],
			},
		],
	},
	{
		date: "2021/05/14",
		version: "0.99.119",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"0.99.118 は条件により確実に死ぬので 0.99.119 に統合",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "82c99121a5c96e9e955020e7ef2f05fc3eb5640f",
						subject:
							"#729: ディスプレイ設定変更を検知した場合に待機処理を挟むようにする",
						comments: [
							"待機時間云々以前にいきなり処理が走っていたのが問題かと思われ",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "9777185271a9c3c3fb47bc78ffd129babf22252c",
						subject: "#728: プロキシ設定の構成を独立させる",
					},
					{
						revision: "554b04e0ba14a46dd4a967e6198bf9441bf70a5c",
						class: "nuget",
						subject:
							"#430: Hardcodet.Wpf.TaskbarNotificationの更新",
						comments: [
							"Hardcodet.NotifyIcon.Wpf.NetCore から Hardcodet.NotifyIcon.Wpf に変更",
						],
					},
					{
						revision: "f23f571caad5842ec90c451980c2fa3c976dc90a",
						class: "nuget",
						subject: "Dapper 2.0.78 -> Dapper 2.0.90",
					},
				],
			},
		],
	},
	{
		date: "2021/04/19",
		version: "0.99.112",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "ec41e0eb2ae6c969b3bcf7f4540a2b4ac6b0151d",
						subject: "#709: プロキシ設定を追加",
						comments: [
							"内容についてはヘルプ -> その他 -> プロキシ を参照",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "df82b87f5fb31fb88656b33cf3b9d7ce03ee0980",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 16.9.1 -> 16.9.4",
					},
					{
						revision: "4943f4251564ec49c0ba8c90b24777e47cdd3bb3",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.7.1 -> 1.7.2",
					},
				],
			},
		],
	},
	{
		date: "2021/03/22",
		version: "0.99.107",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "993b1427b65a10be43a92504068f448230aadc07",
						subject: "DB読み込み専用処理の不具合対応",
						comments: [
							"したはいいけど書き込み処理の異常さが際立つ",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "74725b7503da17adb8ba1ccecd4749348ba635b7",
						class: "nuget",
						subject: "MSTest系更新",
					},
					{
						revision: "0cc4b5f730ab1b87b511bb374b647cadf6dfaf3a",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.7.0 -> 1.7.1",
					},
					{
						revision: "fb070cbbe3f237b90230d148dbd8efb9966d42b4",
						class: "nuget",
						subject: "AvalonEdit 6.0.1 -> 6.1.1",
					},
				],
			},
		],
	},
	{
		date: "2021/02/07",
		version: "0.99.102",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "b4f8cdbe8b0516f579b604b0a4e4e0e8407e1ea4",
						subject:
							"#714: 文言の途中で ... ってなるテキスト表示コントロールを作成する",
						comments: ["スーパー妥協の産物が生まれた"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "e2ff01287167847d99b6760162e2af7253bc5137",
						subject:
							"#725: ノートの遅延書き込み処理が何かしらの状況で遅延処理破棄により書き込まれない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "592c9c7723c7bb588f4bbc7721fa6af8ff67b606",
						subject:
							"#726: 開発時に使用する Nodejs/NPM をバージョンアップ",
						comments: ["Nodejs: 15.6.0", "npm: 7.4.0"],
					},
					{
						revision: "f6b66b21a934efd0d99ca269831ee0e6f4184657",
						class: "nuget",
						subject:
							"Hardcodet.NotifyIcon.Wpf.NetCore 1.0.17-> 1.0.18",
					},
					{
						revision: "1b8f955e75555972dce0131134011100b4c3ba01",
						subject: "update npm",
					},
				],
			},
		],
	},
	{
		date: "2021/01/13",
		version: "0.99.095",
		group: ".NET 5",
		contents: [
			{
				type: "developer",
				logs: [
					{
						revision: "d0328f9042055339a98a0ed937c1363bf5c4aa6c",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.6.5 -> 1.7.0",
					},
					{
						revision: "cbfb18c3a5c99b065ff40707d61c34db7216eea1",
						class: "nuget",
						subject:
							"System.Data.SQLite.Core 1.0.113.6 -> 1.0.113.7",
					},
					{
						revision: "c31e3c83335a17e2e3ef98abb31fe438633b28dc",
						class: "nuget",
						subject:
							"Hardcodet.NotifyIcon.Wpf から Hardcodet.NotifyIcon.Wpf.NetCore に変更",
					},
					{
						revision: "fe3a0a172f0dcbc0e2091e65d4409e19349fbd6f",
						subject:
							"使用コンポーネントの .NET Core 3.1 を .NET 5 に変更",
					},
					{
						revision: "e81b59ab3153c9d9af77558c8918b254474f830e",
						subject: "#719: SonarAnalyzer.CSharp の常用をやめる",
						comments: ["常用というか使用をやめた"],
					},
				],
			},
		],
	},
	{
		date: "2020/12/27",
		version: "0.99.087",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "aeb0cdb3f8790d71c616ccff70e823771ddac06a",
						subject:
							"#716: プラグインサポート最低バージョン・最高バージョンが両方とも制限なしの場合の表記を短くする",
					},
					{
						revision: "c405142465dc63a91c9ea7fa29f5d88b99c16f16",
						subject: "#631: 固定幅フォントの列挙",
						comments: [
							"列挙というより判定処理になった",
							"固定幅を [ フォント名 ] として表示するようにした",
							"特定の対象だけ判定(コマンド, 標準入出力)",
						],
					},
					{
						revision: "edab1437067c4705ecd975c30b1268fce48adf24",
						subject:
							"#723: ノートのキャプションをホイールクリックで最小化を切り替える",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "c5e133011f706586be2d0be81c2c783805df1041",
						subject:
							"#715: アンインストールできないプラグインに対して再起動注意文言表示が眩し過ぎる",
					},
					{
						revision: "daa9034c0c77668df5cf00a5ea23126a9ddb8809",
						subject: "[継続課題] #305: メモリ消費を抑える",
						comments: ["D&D用インフラに起因するメモリ解放漏れ対応"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "118f276acfffa4ad86db444e31ead04c7c4674a6",
						subject:
							"プラグインインストールボタンの文言を「手動」から「ローカル」に変更",
					},
					{
						revision: "dd845a66658dc95c7b51624233de3b5fbe2b8c76",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 16.8.0 -> 16.8.3",
					},
					{
						revision: "ce2f82f8db3eaab6d83154cfe57fb60397e9c599",
						subject:
							"#724: Pe.Bridge の分離はもうあんま意味なさそう",
					},
				],
			},
		],
	},
	{
		date: "2020/12/01",
		version: "0.99.078",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						class: "compatibility",
						subject:
							"#712 対応によりバックアップデータ周りのファイル名が変更されます",
						comments: [
							"yyyy-MM-dd_HH-mm-ss -> yyyy-MM-ddTHH-mm-ss",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "d01af844ba2c3badc58a300c444ed4c5d0ddeab2",
						subject:
							"#710: バージョン情報にCLRのバージョンを表示する",
					},
					{
						revision: "ccc2f52b9153b678e4ad3aac205fbdfbdbac5410",
						subject:
							"#712: タイムスタンプを使用するファイル名の年月と時間の間を _ じゃなくて T にする",
					},
					{
						revision: "ad2ac657f750677b69d4b3db4862860c875b6921",
						subject:
							"#705: プラグインの手動インストールを半自動化する",
						comments: [
							"理想的なアーカイブのみインストール可能",
							"インストールした状態でアンインストール予約したりとかはかなりというか全く試験していない",
							"アップデート処理も全く試験していない",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "8a955b8a3bce496c152384b864866103f9053f67",
						subject: "#711: 自動選択言語が☆言語不明☆",
					},
					{
						revision: "4bcee0780460b0caac248b7a2b21c8c290a4dc0d",
						subject:
							"#713: 通知メッセージが設定画面経由で生きっぱなしのゾンビになる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "81b82c750fd82671203ba878922747793ef38fd2",
						class: "nuget",
						subject: "Microsoft.Extensions.* の .NET 5 対応",
						comments: [
							"Microsoft.Extensions.Configuration.Binder [NEW] 5.0.0",
							"Microsoft.Extensions.Configuration.Json 3.1.9 -> 5.0.0",
							"Microsoft.Extensions.Logging 3.1.9 -> 5.0.0",
							".NET 5 にたぶん完全移行できた",
						],
					},
					{
						revision: "c7f2e30a2b5697b1a0d3ce1f48642cadbbb25f47",
						subject: "update npm",
					},
					{
						revision: "68753bfc69780be61b43ff8051e96268f3fa949f",
						class: "nuget",
						subject:
							"SonarAnalyzer.CSharp 8.14.0.22654 -> 8.15.0.24505",
					},
					{
						revision: "7e07beb7f4575c4b88618985d9a1d5e6dc88a1c1",
						class: "nuget",
						subject: "Dapper 2.0.35 -> 2.0.78",
					},
					{
						revision: "1205ba84e84b80e46d9c0a94e8c193b917bf411d",
						class: "nuget",
						subject: "CefSharp.Wpf 85.3.130 -> 86.0.241",
					},
				],
			},
		],
	},
	{
		date: "2020/11/15",
		version: "0.99.070",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "NET 5 に対応しました",
						comments: [
							".NET Core の時もそうだったけど単一実行は .NET Framework に比べてクッソらくちん",
							"NuGet の一部は .NET Core 3 のものを流用",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "cbc391569d7bf75fad1d7ebc33c050b1f1a5df5b",
						subject: "#681: プラグインのアンインストール処理追加",
						comments: [
							"キー設定したランチャーアイテム周りの破棄はクソほど怪しい",
							"ユーザー操作は Pe インフラとしてのプラグイン インストール・アップデート処理周りが実装完了するまではなんもしない方がいい",
						],
					},
					{
						revision: "c330e8074dbe96500f724024cfa1c2d2dc675e74",
						subject:
							"#698: アンインストール用の記録データ破棄処理を構築する",
					},
					{
						revision: "8fe9240481e9fbe927f9def81d080c5583da0b5b",
						subject: "#701: .NET 5 対応",
						comments: [
							"ロギング処理・アプリケーション構成ファイルのアップデートはこのタイミングだと無理無理",
							"時間あるときに対応する",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "94ce76001560798d35c2d91a1088df3ea264971d",
						class: "nuget",
						subject: "Microsoft.NET.Test.Sdk 16.7.1 -> 16.8.0",
					},
					{
						revision: "19eda9e825bf622fed0232c4b74e3f3cec4ec8ae",
						class: "nuget",
						subject:
							"System.Data.SQLite.Core 1.0.113.5 -> 1.0.113.6",
					},
					{
						revision: "38c8e34e5dd6d00f28f3a8153e18f59d8b93105c",
						class: "nuget",
						subject:
							"System.Text.Encoding.CodePages 4.7.1 -> 5.0.0",
					},
					{
						revision: "377f4e6946dc5ccf951e502ccd68505bf7ead2fd",
						class: "nuget",
						subject: "System.Management 4.7.0 -> 5.0.0",
					},
					{
						revision: "23a8a82e040eb238efa8bd94df047faa9b920799",
						class: "nuget",
						subject:
							"System.DirectoryServices.AccountManagement 4.7.0 -> 5.0.0",
					},
				],
			},
		],
	},
	{
		date: "2020/11/03",
		version: "0.99.063",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "7347408315b6a4a44fd0aac1e708fb1d327eb6f2",
						subject:
							"#700: メニュー表示されるユーザー設定のキー操作は使用頻度の高いものを表示させる",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "db16ee6af65fd0e470b325eeeb716c87d2ad2ed7",
						subject:
							"#702: [Pe-CrashReport] System.ComponentModel.Win32Exception (1461): モニター ハンドルが無効です",
					},
					{
						revision: "b87667dca2380d5deaaf057d308f17ce87fd073a",
						subject: "#704: キーボード設定の並び順が不明",
						comments: [
							"押下系はSQL側とアプリ側が混在しているので諦め",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "9fbb3e5f43599da8ce9360e6e7cb8eec79f68925",
						class: "nuget",
						subject:
							"#699: Prism.Wpf を 85.3.130 にバージョンアップする",
						comments: ["DI 周りはむりでしたー"],
					},
					{
						revision: "1a51e2990bc819215c3888234389a1b5b69bf860",
						class: "nuget",
						subject:
							"System.Data.SQLite.Core 1.0.113.2 -> 1.0.113.5",
					},
				],
			},
		],
	},
	{
		date: "2020/10/25",
		version: "0.99.056",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "7ba1874aa3eefc1b5bccf5498af19c7555139067",
						subject:
							"#691: ユーザー設定のキー操作をメニュー等々に表示する",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "eaf580341b867e2cbb5f5ee518337f1f0e399f82",
						class: "nuget",
						subject: "MS関係パッケージ更新",
						comments: [
							"Microsoft.Extensions.Configuration.Json 3.1.8 -> 3.1.9",
							"Microsoft.Extensions.Logging 3.1.8 -> 3.1.9",
							"Microsoft.Extensions.Logging.Abstractions 3.1.8 -> 3.1.9",
						],
					},
					{
						revision: "8af11c7f748225c91b608fb7ae0b5f1534a3fbbb",
						class: "nuget",
						subject:
							"System.Data.SQLite.Core 1.0.113.1 -> 1.0.113.2",
					},
					{
						revision: "a1e57d528d10b4d37fcbb409e58602fb7d79d21d",
						class: "nuget",
						subject:
							"SonarAnalyzer.CSharp 8.13.1.21947 -> 8.14.0.22654",
					},
					{
						revision: "f4dcdd1ac448b32acc4f658a246be90de5e80874",
						class: "nuget",
						subject: "CefSharp.Wpf 84.4.10 -> 85.3.130",
					},
				],
			},
		],
	},
	{
		date: "2020/10/11",
		version: "0.99.049",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "34010dc25b7207ff356d6f46b19f4ebe643f2879",
						subject:
							"#695: ランチャーツールバーのコンテキストメニューにアイコンを付与する",
					},
					{
						revision: "00ecdfd0a1f72ab123cf9b6d9487815b4c822248",
						subject: "#693: ヘルプファイル再構築",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "eac9d0dec92c4d701d9295c12c38ce3d05647fa5",
						subject: "#689: β版実行時の警告メッセージが💩",
					},
					{
						revision: "106c6b8ec57ea87da1785b5b215afa79572c297c",
						subject: "#692: ノートの RTF リンク時にエラる",
					},
					{
						revision: "c7945bb7ad32cb791beaadd4fef183a3cdf3bb16",
						subject:
							"#696: ノートのリンク状態表示中に非アクティブになった際はリンク状態表示を非表示にすべき",
						comments: [
							"リンク状態表示中に加え種別変更も対象とした",
						],
					},
					{
						revision: "eed5767ba5e8495876d9eed4fe391c735d1d471b",
						subject:
							"#677: リリース物がないのにリリースノートを出そうとして死ぬ？",
					},
					{
						revision: "91667923210a5d4df47e5e4cfb166d00b2ae43a5",
						subject:
							"#697: ランチャーツールバーのフォーカスが二重にあたっている",
					},
				],
			},
		],
	},
	{
		date: "2020/09/28",
		version: "0.99.040",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "35d75ce6409abfdcce37a364055215a5c6bdb84f",
						subject:
							"#694: 高DPI環境でノートのタイトルバーD&Dによる位置移動ができなくなっている",
					},
				],
			},
		],
	},
	{
		date: "2020/09/27",
		version: "0.99.038",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "85c473de5c59e0a8939471c549c67a0a1b4af582",
						subject:
							"#685: ノートのタイトルバー位置を変更可能にする",
					},
					{
						revision: "5959bd732d63950950ff1113977c335480c2e6a3",
						subject: "#676: ノートにスクロールバーを付けたい",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "2146c3bed505e8100e06dc18d1004c6b565c6345",
						subject: "#675: 既知の問題: ツールバーフォントの適用",
					},
					{
						revision: "bcd6ad5badf83f58094d05cdeed06e930659f2a0",
						subject:
							"アイコン取得時のメモリ解放漏れを修正(関連: #305)",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "5c3527dd413cf39b4ade3391e3ad8a2d6bebf170",
						subject:
							"#686: デプロイ処理周りで ps で頑張ってる部分を node に置き換えていきたい",
						comments: ["あかんかった！"],
					},
				],
			},
		],
	},
	{
		date: "2020/09/22",
		version: "0.99.032",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject:
							"継続中案件 -> #684: [Pe-Feedback] 99-013及び020は機能せず",
						comments: [
							"ログが欲しいです",
							"フォーラム・課題・メールに連絡もらえると助かります",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "67b467891f0a39ad254e884d21b8d8fff6f5592c",
						subject:
							"#466: 指定して実行ウィンドウへのファイルD&Dを実装",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "8d872fbbceb22c1307108fe32844a80ce65c1233",
						subject: "#678: 設定画面にて揮発性DBの複製を行う",
					},
					{
						revision: "d23e4a034dd4f9ad3e51b30c5788580f55e1cf0c",
						subject:
							"#679: フルスクリーン強制終了時のイベント取得に失敗しランチャーツールバーが隠れたままの状態がある",
						comments: ["かなり環境依存な問題っぽい"],
					},
					{
						revision: "ec75bb4a10b4b764a6bfb15c5849090c740c72c2",
						subject:
							"#683 指定して実行の履歴タイムスタンプが☆U☆T☆C☆",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "b9400a5a2db55c4e9697005f0fc3ccbdbaf60588",
						subject: "#666: CIビルドの短縮化",
						comments: [
							"あんまり変わらんかったのでエラー系をなるはや検知できるようにした",
						],
					},
					{
						revision: "29c3954e1e0d9ee6137328e7ad9578fb9bde1bc7",
						subject: "#632: 固定値を外部データに置き換える",
						comments: [
							"ちょっと探すのがしんどいので地道にやっていく",
							"もともと保守の問題で億劫だった原因である内部的処理を保守しやすくしたので一応は目標達成",
						],
					},
					{
						revision: "8d29c6ab34ae9a8e0e5369c3c7686bd4e9d35d19",
						subject: "#687: フィードバックに WebView いらんでしょ",
					},
					{
						revision: "b04daa30fc3b81e28e1b4c3003e128c32859fe1e",
						class: "nuget",
						subject: "MS関係パッケージ更新",
						comments: [
							"Microsoft.Extensions.Configuration.Json 3.1.7 -> 3.1.8",
							"Microsoft.Extensions.Logging 3.1.7 -> 3.1.8",
							"Microsoft.Extensions.Logging.Abstractions 3.1.7 -> 3.1.8",
						],
					},
					{
						revision: "d9fddde51700edc9968282e65bc41e676c9d8759",
						class: "nuget",
						subject:
							"SonarAnalyzer.CSharp 8.12.0.21095 -> 8.13.1.21947",
					},
					{
						revision: "ac1e0c3db50a8e2c2c3ddfe24aa3fa14b7e85163",
						subject:
							"Microsoft Visual C++ 再頒布可能パッケージ更新",
					},
				],
			},
		],
	},
	{
		date: "2020/09/02",
		version: "0.99.020",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "e1b625de9f9b38ab08e667cd5c2d5ef5fe95c683",
						subject:
							"カラーパレット選択時のカーソル・視覚状態を変更",
					},
					{
						revision: "b0239d91baa24d16c1a9ecd8a20b96ffb7179bbc",
						subject:
							"#674: 設定画面のフォント用コントロールのバインド問題に対応する ",
					},
					{
						revision: "519eb138802d51a9b77caf8359ffc7cfa48ed161",
						subject: "#660: ランチャーアイテムアドオンの実装",
						comments: [
							"かなり処理が甘いけどこれ以上労力はかけられんのでいったんOKとする",
							"参照実装としては Pe.Plugins.Reference.Clock が対象",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "a4f5be6596d862596d2746c64defe02b73d1b3af",
						subject:
							"#670: ランチャーアイテム自動登録で取り込みボタン連打すると死ぬ💀",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "18ef510509e590be0fa74eec1defa700fd1b4b29",
						subject:
							"#668: SQLの実行ログにてスペース破棄とか行番号追加とか、いる？",
					},
					{
						revision: "5d6b58dc5a3b1909fe0c4d7a00b0ffb5cbc2fa6d",
						subject:
							"#673: 同一のテーマプラグインを選択している状態で本体設定を保存すると二重読み込みで死ぬ",
						comments: [
							"正確には Pe のバグでなくプラグイン側のバグなんだけど参照実装の基底処理なんでなんとも",
						],
					},
					{
						revision: "70b37f006cf47b16e8b060436ba31681df96cddb",
						subject: "#672: ランチャーアイコンの処理周りを整理する",
						comments: [
							"#660 作業時に対応",
							"整理するどころかさらに混乱を生み出した",
						],
					},
					{
						revision: "7a082894e17571bea00c1dcb820704562d55cf7d",
						class: "nuget",
						subject: "CefSharp.Wpf 83.4.20 -> 84.4.10",
					},
					{
						revision: "31fc05c2136ef84ee128267dd85c31fd8623c75c",
						class: "nuget",
						subject: "MS関係パッケージ更新",
						comments: [
							"Microsoft.Extensions.Logging 3.1.5 -> 3.1.7",
							"Microsoft.Extensions.Configuration.Json 3.1.5 -> 3.1.7",
							"Microsoft.NET.Test.Sdk 16.6.1 -> 16.7.1",
						],
					},
					{
						revision: "5c9f9cad0799c13df6540e8f26b99070a861bc6f",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.6.4 -> 1.6.5",
					},
					{
						revision: "42c62759fdd8c66e3dacfb5de6ff7c54b9548ddf",
						class: "nuget",
						subject:
							"SonarAnalyzer.CSharp 8.9.0.19135 -> 8.12.0.21095",
					},
				],
			},
		],
	},
	{
		date: "2020/07/09",
		version: "0.99.013",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "c0ea1fe0bc8eb35e9a44751275fc69c799b015f3",
						subject: "#671: ノートが自動的に隠れなくなっている ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "71368427c5f8fc92af3e058deacbb618260e371a",
						subject:
							"#667: Visual Studio 2019 Image Library の使用を明記する",
					},
				],
			},
		],
	},
	{
		date: "2020/07/07",
		version: "0.99.010",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "2905eecdf7571684c20a7f3d02c987a7a98e7718",
						subject:
							"#655: ランチャーアイテム自動登録にてアンインストールファイルと思しきファイル名は登録対象外とする",
					},
					{
						revision: "f7bef111cad2bad7aa64e118b86f95b10402b9b0",
						subject:
							"#662: ランチャーアイテム自動登録で登録時ではなくプレビュー時にショートカットを展開する",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "db840540ea24cc00404149cc3d68c0dfbc5bf23e",
						subject:
							"#659: CI 処理時のコミットリビジョン埋め込み処理をスキップしている",
					},
					{
						revision: "846999268fa3e8a6e774768e3bdf62c9e6ad035c",
						subject:
							"#663: ランチャーアイテム自動登録でサブディレクトリが読み込めていない",
					},
					{
						revision: "8527b589cb5e9d58e372ed2ab4b11d0452568230",
						subject:
							"#661: アイコン取得時に基本サイズ以外にDPIスケールも考慮する",
						comments: [
							"DPI が取れたり取れなかったりのヤケクソ DPI スケール反映",
							"環境によるけど 20px とか 24px とか 40px とかのアイコンをとってくるので対象が該当アイコンサイズを持っていなければ結局ぼけるっていうね",
						],
					},
					{
						revision: "6bce56ab96d2c491ab4a3eebb3eb6152bec87366",
						subject:
							"#634: 設定画面を開く際にやたらめったら時間がかかる",
						comments: ["かなり手を入れたのでバグってたらめんご"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "27afe5cd24dfb4879f9559890ce8cbd2048b8a08",
						class: "nuget",
						subject: "CefSharp.Wpf 81.3.100 -> 83.4.20",
						comments: [
							"WebView処理周りで透過効かなくなってるっぽいなー",
						],
					},
					{
						revision: "034b6f79c849e418f75685a5757426cc7faaad02",
						class: "nuget",
						subject:
							"SonarAnalyzer.CSharp 8.8.0.18411 -> 8.9.0.19135",
					},
				],
			},
		],
	},
	{
		date: "2020/07/03",
		version: "0.99.001",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "60ed1438fab4d615d1f5ac25978eaf74ad3f2c51",
						subject:
							"#658: ランチャーアイテム更新間隔にて分が毎分になっている",
					},
				],
			},
		],
	},
	{
		date: "2020/07/02",
		version: "0.99.000",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "プラグインを実装した",
						comments: [
							"今のところかなり限定的で気楽に試せるようなものではないけど一区切り",
							"プラグイン共通ライブラリ(Pe.Bridge)バージョンは Pe と連動しない",
							"将来的にはインストール・アンインストールを Pe 側からできるようにしたりする予定",
							"参照実装: <Pe.git>/Source/Plugins",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "b3e624137a3aadebf85cfb0399d35fe6f0578ab7",
						subject: "#509: プラグイン機構の構築",
						comments: [
							"まだまだ甘いし達成できてない目標もあるけど実運用しながら機能拡張できるようにしていきたいのでリリース",
							"ドキュメントもまだ全然かけてないのでソースが正。んで頻繁に互換性が失われる想定",
						],
					},
					{
						revision: "fe77f8d8e95e42df01d492306492506f6b1c04ce",
						subject: "#550: 定期的にアイコン情報を更新する",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "84d12a472783693eea5fb863bfb9a7ccef461126",
						subject:
							"#623: アプリケーション内で使用する Pe アイコンをもうちときれいに表示する",
					},
					{
						revision: "37f6d64b3b62b9a91d3ec762101a0fffb321896b",
						subject:
							"#649: ランチャーアイテム再試行処理のキャンセルが効いていない",
						comments: [
							"色々試したけどアクティブ→非アクティブを連続するとダメっぽいので初っ端から非アクティブにした",
							"副次的効果として #654 に対応",
						],
					},
					{
						revision: "37f6d64b3b62b9a91d3ec762101a0fffb321896b",
						subject:
							"#654: 通知ログがウィンドウアクティブ状態をまだまだ奪う",
					},
					{
						revision: "410f71ec484e3bcc3a8de783cc1117c5968cd9e5",
						subject:
							"#651: %PATH% から設定されたランチャーアイテムのコンテキストメニューの活性処理で %PATH% を考慮する",
					},
					{
						revision: "e6ae31d10d40ac86f70f1f584d52c900422eeb08",
						subject:
							"#652: バージョン情報表示中はコマンド表示できないようにする",
						comments: [
							"スタート・設定・バージョン情報を表示した際にフック等の処理を停止するようにした",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "a3503a67c914f7b40e726e2810e29e8144a11022",
						subject:
							"#650: 内蔵ブラウザのリソース取得をC#処理からCefSharpで直接行う",
					},
				],
			},
		],
	},
	{
		date: "2020/06/21",
		version: "0.98.001",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "パッケージ周りの更新",
						comments: [
							"プラグイン周り実装を入れたいんだけどアセンブリ周りの解決処理がうまくいかないのでスキップ",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "56ff4b82e73a26ec247688f61583e06381fb89f0",
						class: "nuget",
						subject: "MS関係パッケージ更新",
						comments: [
							"Microsoft.Extensions.Logging.Abstractions 3.1.4 -> 3.1.5",
							"Microsoft.Extensions.Logging 3.1.4 -> 3.1.5",
							"Microsoft.Extensions.Configuration.Json 3.1.4 -> 3.1.5",
							"MSTest.TestAdapter 2.1.1 -> 2.1.2",
							"MSTest.TestFramework 2.1.1 -> 2.1.2",
						],
					},
					{
						revision: "d53e0ef0f070c5ad029a6780e5cfdda91b4aac9e",
						class: "nuget",
						subject:
							"SonarAnalyzer.CSharp 8.7.0.17535 -> 8.8.0.18411",
					},
					{
						revision: "efa6c10a3cf595b834f48a69c030827d2c5816a2",
						class: "nuget",
						subject:
							"System.Data.SQLite.Core 1.0.112.2 -> 1.0.113.1",
					},
					{
						revision: "7e69657f18a645ece0d3de14645ac3c1d812dffd",
						class: "nuget",
						subject: "CefSharp.Wpf 79.1.360 -> 81.3.100",
					},
				],
			},
		],
	},
	{
		date: "2020/05/24",
		version: "0.98.000",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"デスクトップPCがぶっ壊れたのでノートPCから意味もなくアップデート",
						comments: [
							"データのバックアップ大事",
							"全部吹っ飛んだわ。全ドライブ死ぬとかどうなってんの",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "88fc9fcca83f0a88eff7a9ede26e9107870a7677",
						subject:
							"#642: フィードバックより ->ランチャーツールバーへのファイルD&D処理の標準挙動",
						comments: [
							"設定 -> 基本 の「ツールバー」の「ボタンへのD&D」により変更",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "b2cf4bd6c49695df4a83f0469437cab500e7c81c",
						subject: "#645: 本体コマンド 再起動 死んでるやん！",
						comments: [
							"#641, #644 との合わせ技で心折れたので #576 の優先度を一つ上げた",
						],
					},
					{
						revision: "5aa21a3c074d622f689b99281d9b82e8ec3fcd0e",
						subject:
							"AppStandardInputOutputSetting.IsTopmost の型が TEXT",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "c814ae8832590d86c1f548828a662b9255773192",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.6.3 -> 1.6.4",
					},
					{
						revision: "31086ffcf54cc1f4128ef64e38d6e3390ca848be",
						subject:
							"強制フル GC 時に LOH をコンパクションするようにした",
					},
				],
			},
		],
	},
	{
		date: "2020/05/20",
		version: "0.97.000",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject:
							"自動バージョンアップ処理不具合(#641)に対応しました",
						comments: [
							"本バージョンを用いた次回バージョンアップ移行で有効になるため、今までダメだった場合は手動ダウンロードが必要です",
						],
					},
					{
						revision: "",
						class: "compatibility",
						subject:
							"通常使用の場合影響はしませんがコマンドライン引数の不具合修正により一部挙動が変わる可能性があります",
						comments: [
							"Pe.exe に対して半角スペースを含むコマンドライン引数を渡した際に、本バージョン以前では最後の一文字が破棄されていました",
							"(前バージョン) Pe.exe --user-data=\"dir path\" -> 'dir pat' と解釈されていた",
							"(本バージョン) Pe.exe --user-data=\"dir path\" -> 'dir path' と解釈される",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "47fa77de01d6bee8697164059e266f40ee0c4a67",
						subject:
							"#640: 通知ログがウィンドウアクティブ状態を奪う ",
					},
					{
						revision: "7714338edaf8950ffa295e9d24eaff537d04e7a7",
						subject: "#641: フィードバックより -> アップデート失敗",
						comments: [
							"ディレクトリパスに半角スペースが存在する場合に PowerShell の引数・変数が上手く扱えず失敗していた",
							"本体配置ディレクトリのパスに半角スペースが存在する場合はアップデートスクリプトの処理中に異常終了",
							"データ配置ディレクトリのパスに半角スペースが存在する場合はアップデートスクリプトの起動に失敗",
							"コマンドライン引数に半角スペースが存在する場合はアップデートスクリプトの起動に失敗",
							"関連して Pe.exe 処理に半角スペースを含んだコマンドライン引数を渡した場合に Pe.Main.exe に最後の一文字が渡されたない不具合の修正",
						],
					},
					{
						revision: "0b00c03a07e95f540725affd6b00b5d12acb66e2",
						subject:
							"#644: 本体コマンドの再起動処理で本体配置パス・コマンドラインの各種データディレクトリにスペースがあると再起動できない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "128c9b206ae99d52b5679093417d884255a9658c",
						subject: "#635: デバッグ用初回起動データ構築処理の実装",
					},
				],
			},
		],
	},
	{
		date: "2020/05/17",
		version: "0.96.000",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "978d63b4a9b030065d2caf822f57474baa84c59c",
						subject: "#525: 環境変数編集機能の色付けを行う",
					},
					{
						revision: "ce97b1eac0e12e2e60e3116c840059658c939dd5",
						subject:
							"#627: コマンドで二種類に分かれるアプリケーションコマンドは拡張キーで切り替える",
					},
					{
						revision: "0502f2ff851b33bf5bd93d49b0cfd16ab0610e7a",
						subject:
							"#625: ノートを非表示にした際に元に戻すをサポートする",
						comments: ["以下操作のみを対象とする", "Alt + F4", "×"],
					},
					{
						revision: "e1e639d6fc5ef47f80a130fb8ea9af24bf1a7acf",
						subject:
							"#624: ツールバーを提供UI以外から閉じたときに元に戻すをサポートする",
						comments: ["以下操作のみを対象とする", "Alt + F4"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "04afcca9081dafc6d22cd04421f608134b937be0",
						subject:
							"#622: 通知領域コンテキストメニューのフック状態の切り替えがチェック反映されていない",
					},
					{
						revision: "3e32bc99b1aa15e51211b8653d6b669e148388e4",
						subject:
							"#530: 通知領域右クリックが死んでるときがある。",
						comments: [
							"たぶんね、たぶん",
							"ダメだったら起票します。。。",
						],
					},
					{
						revision: "37351d1f96aa86376d04cf3eeb1082c50cc8dc41",
						subject:
							"#617: 本体設定完了時にランチャーアイテムのアイコンキャッシュが全部削除される既知の問題",
						comments: [
							"調査の結果ランチャーアイテム変更時にも発生していた模様",
						],
					},
					{
						revision: "7af32665d4d73e54f5904325285142bf1f6b8293",
						subject:
							"#626: ツールバーのハンバーガーメニュー表示をフェードさせる",
					},
					{
						revision: "f1e459de74bf544605ba4661225e4f5c569476cd",
						subject:
							"#633: ランチャーグループ名に _ が存在するとアクセスキー扱いとなっている",
					},
					{
						revision: "3cb441c7b30a875e0e74730b34ea877d6e99b5b6",
						subject:
							"#636: 通知ログがカーソル位置指定で通知ログウィンドウにクリック可能なアイテムがある場合は常時追従してはいけない",
					},
					{
						revision: "b288e997badaa01455dc56e66e249b6e6f0cf9a3",
						subject:
							"#628: 出来立てほやほやのノート位置情報が保存されていない",
					},
					{
						revision: "47f291f81e56c95f4b513a9fe559925aa6981b80",
						subject:
							"#638: コマンド検索時の0件ヒット文字列表記をまともにする",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "2aadad27e1afc6e0c14952f635fc9eb970a5540a",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.6.2 -> 1.6.3",
					},
					{
						revision: "f38039737ddecb610979d34a85a622e8d20189c9",
						class: "nuget",
						subject: "MS関係パッケージ更新",
						comments: [
							"Microsoft.Extensions.Logging 3.1.3 -> 3.1.4",
							"Microsoft.Extensions.Logging.Abstractions 3.1.3 -> 3.1.4",
							"Microsoft.Extensions.Configuration.Json 3.1.3 -> 3.1.4",
							"System.Text.Encoding.CodePages 4.7.0 -> 4.7.1",
						],
					},
					{
						revision: "cd6e3c97ec8b68c26eafc8167deac3f4adfdd33f",
						subject: "コマンドウィンドウにデバッグ・β版印を付与",
					},
					{
						revision: "e23aa0c880f0699da0ed8ad5c56b37dea8da6443",
						subject:
							"#620: Clr Heap Allocation Analyzer を VS 拡張機能から Nuget に移し替える",
					},
					{
						revision: "b9035485416401d075b05fb5c82b5f154939ac89",
						subject: "SonarAnalyzer.CSharp の導入",
					},
					{
						revision: "01ee026a1a40917c8915a93c54da7ec155b4aa6a",
						subject:
							"#637: 更新履歴の元ファイルがでかすぎるので分割したい",
					},
				],
			},
		],
	},
	{
		date: "2020/05/09",
		version: "0.95.000",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "6f2f4f6729441b57ce3e6fe5963bfd8e0bdd9d98",
						subject:
							"#603: マウスクリックでキーボード入力待機を解除する",
					},
					{
						revision: "93b3df1af98152dc7b87f8104ddbd2b156ef7ae0",
						subject: "#531: 本体用特別コマンドの実装",
						comments: [
							"コマンド入力時に先頭が「.」の場合に本体用コマンドとして扱うようにした",
						],
					},
					{
						revision: "8a646840355b1d79f6957d6753809bc703a033c3",
						subject: "#613: ノート内でタブを入力できるようにする",
					},
					{
						revision: "d05797710abb575c8c141cc8a328a2716ae0e66e",
						subject:
							"#602: キーボード設定をキー入力から行えるようにする",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "76d8addc5d42c0417694c18696836355f784433f",
						subject:
							"#601: コマンド型ランチャーの横幅が保存されてない",
						comments: [
							"正確には保存されてたんだけど保存値がちょっと頭おかしかった",
						],
					},
					{
						revision: "3e03ec1a35538ed9b12f4a39a5891011f159d7f4",
						subject:
							"#607: ヘルプのメニュースクロール位置がリンク遷移時にリセットされる",
					},
					{
						revision: "9862776bd0a0c63cdce79cdd5f1ca0ad0f625694",
						subject:
							"#610: アイコンのあるコントロール系UIが二重のタブ移動対象になっている",
					},
					{
						revision: "fc09c9b8e1f0a6ef91a19b1aef17587d4ac73023",
						subject: "#604: 文言をもうちっと分かり易くする",
					},
					{
						revision: "b7e7e86052a226c9ce8c9fa22ebdb0438338fdb9",
						subject:
							"#606: 毎月1日のクッソしょうもないアイコン切り替えが常時稼働状態だと切り替わらない",
					},
					{
						revision: "c59fdf04ac39dd4000d04bc673df0a5538147f72",
						subject:
							"#614: ランチャーアイコンが保存されていない疑惑",
					},
					{
						revision: "86b7817b0f649fb5ecbdafb58c9a097a37405558",
						subject:
							"#615: 本体ディレクトリ読み込み時に不要なディレクトリが作成される",
					},
					{
						revision: "60119f359108eb67945ec06d68ff8929a618f50d",
						subject:
							"#605: ランチャーアイテム修正時にコマンド型ランチャーに即時反映されない",
					},
					{
						revision: "c2bedc4ae10af111a0b1e59e676ab2a98efba739",
						subject: "アイコン制御処理SQLが上手くいってなかった",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "ea659d24ad4f329f09fc15b713e10a5aee0106fc",
						subject: "フック処理の登録処理を初期化から若干ずらした",
					},
					{
						revision: "ee01edf5db43db7d66f233f7361bbb42feda86a4",
						subject: "#608: UserControl のバインド周りを調整",
					},
					{
						revision: "8f0bc8a95e3cc8b57121c108f026a45cb897b81a",
						class: "nuget",
						subject:
							"System.Data.SQLite.Core: 1.0.112.1 -> 1.0.112.2",
					},
					{
						revision: "4b11f25409c3a27f462e8895993cad8302e49340",
						subject:
							"#584: 0.95.000 公開時時に 0.83.0-0.90.000 からのアップデートサポートを破棄",
					},
					{
						revision: "60e254596d40fc626f698d8e0bbb9044c959d876",
						subject:
							"#616: Dao と内部実装SQL読み込み処理に対する事故防止対策委員会",
					},
					{
						revision: "518650078c359bfabe454b7db2e2cb32cb850b28",
						subject:
							"過去バージョンはもう tag から適当に再現してくれ",
					},
				],
			},
		],
	},
	{
		date: "2020/04/26",
		version: "0.94.000",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "cd37c118ca03deac74f2d1209b34e2f9fc8d6373",
						subject: "#593: 通知用UIの作成",
					},
					{
						revision: "ca412643b2736bd7516fd54e74d62ee9a396a42b",
						subject: "#592: 起動失敗アイテムを頑張って起動させる",
					},
					{
						revision: "257ffd2248c278c8b20213174213c3220e9c5105",
						subject:
							"#591: ノートの内容を時間経過で非表示にするとか視認性を悪くする",
					},
					{
						revision: "770ddd944acf0b890984182408fd86b96e71be60",
						subject: "#507: キーボード入力待ちの通知を行う",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "596bd618c738a3d4e72d0fb0064748066472fe1d",
						subject:
							"#566: 設定でランチャーアイテムからアイテムを削除した場合にグループ内に該当ランチャーアイテムが残ってる",
					},
					{
						revision: "55720ff987110c7ffc911a3adc546ca51d4ebf99",
						subject:
							"#594: 初回仕様バージョンの記録が 0.84.0 固定になってる",
					},
					{
						revision: "",
						subject: "#595: クラッシュレポートの云々ってなんやねん",
					},
					{
						revision: "af523ee7a9269ea3e926116b65731e9015d08aef",
						subject: "型名設定できてなかった",
						comments: [
							"型変更に table 作り直ししか手がなさそうなので一応初期構築には正しい型を設定したうえで、既存は無視する",
							"出来んことはなさそうだけど手間がかかるので気が向いたら何とかしてみる",
						],
					},
					{
						revision: "e5a16c180de56c3d56c4787e616ac7791b36af5d",
						subject: "#596: 実行回数記録されてなくない？",
					},
					{
						revision: "47d0cc577b7406ce4d7e6399f3ec8e5d0fe1d992",
						subject:
							"#598: ツールバーで登録したてのアイテムを編集したら死ぬ疑惑",
					},
					{
						revision: "717833705217355c2cfd79647ec8382ef5b54194",
						subject:
							"#600: 初回起動時に作成される表のうち型指定していないものがある",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "112bcd3a2badbc13afc0f1583fc9c9d4bf87b523",
						subject:
							"#569: @appsettings.debug.json 消していいでしょ",
					},
					{
						revision: "e544ddd1b040286eaf0dab17fa5ceb1667ad09a4",
						class: "nuget",
						subject: "Dapper 2.0.30 -> 2.0.35",
					},
					{
						revision: "e544ddd1b040286eaf0dab17fa5ceb1667ad09a4",
						class: "nuget",
						subject: "System.Data.SQLite.Core 1.0.112 -> 1.0.112.1",
					},
					{
						revision: "350e11430b8e0c7665d630fce7957e057210b5ed",
						subject:
							"#597: CURRENT_TIMESTAMP を使わずにアプリ側から時刻を設定する",
					},
				],
			},
		],
	},
	{
		date: "2020/03/29",
		version: "0.93.000",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "a82b25c30e9cd2a702590408b71d9dfb3da5c195",
						subject: "標準入出力死んでるやん！",
					},
					{
						revision: "ec1b8821431a945b0b9eb9da894e5ced9aec0580",
						subject:
							"#589: ヘルプドキュメントのメニュー部が使いにくい",
					},
					{
						revision: "fc9cb7ba7427b6569c3274ee4a6fa1b75c0c1e94",
						subject:
							"#590: フィードバックのプレビューでインターネット世界に旅立てる",
					},
					{
						revision: "52aa2db9d89786a503100b46d4caaf85c8baa156",
						subject:
							"#587: 非実行可能アイテムを指定して実行で標準入出力を受け取ると死ぬ",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d2118258ba9baadc7d3130b7a8a98b95728b3eb4",
						subject: "#493: DI に名前付きがほしい",
						comments: ["恐らく使うことはない"],
					},
					{
						revision: "02306f920cce8f2357205f1202fd97651eb2e2bb",
						class: "nuget",
						subject: "Microsoft.Extensions.* 更新",
						comments: [
							"Microsoft.Extensions.Configuration.Json 3.1.2 -> 3.1.3",
							"Microsoft.Extensions.Logging 3.1.2 -> 3.1.3",
							"Microsoft.Extensions.Logging.Abstractions 3.1.2 -> 3.1.3",
						],
					},
				],
			},
		],
	},
	{
		date: "2020/03/22",
		version: "0.92.000",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "0832fd551d021da3c16ad13b5aa5d7cc2cb9199c",
						subject: "#503: 無言で死ぬのを何とかする",
						comments: [
							"アプリケーション側で検知可能な未処理例外に対してクラッシュレポートを表示",
							"MnMn でやってたようなことを整理して再実装",
							"クラッシュ時に生データを出力してクラッシュレポート側で再調整みたいな感じ",
						],
					},
					{
						revision: "1672c0723532c8f36b677d077fd0bd390bf3d0dd",
						subject: "#506: フィードバック機能の再実装",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "8ad089ea862928c798376538b918e88d3a1d5f71",
						subject:
							"#588: ディスプレイ設定変更後に強制終了する時がある",
					},
					{
						revision: "2591d4ae145b4ed6250aaa68a380cdb4039a2a74",
						subject:
							"ヘルプのコマンドライン引数 app-log-limit の説明書きが値無しになっていたのを修正",
					},
					{
						revision: "072b8b618f454e7f280d53146ef878215eb61da0",
						subject: "#586: ログが二重に出力されている",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "5fecb71fefb2f389dfee63943765cf5dc1e5901d",
						subject:
							"#574: 0.92.0 公開時に 0.91.0 以上をアップデート可能対象にする",
					},
					{
						revision: "dd8c64fdf09c3d03121a2b27ba1d9e520a598444",
						class: "nuget",
						subject: "NLog.Extensions.Logging 1.6.1 -> 1.6.2",
					},
				],
			},
		],
	},
	{
		date: "2020/03/13",
		version: "0.91.000",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"バージョン 0.83.0 以下のデータコンバート処理を破棄しました",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "1e6f0af99d9a25d6eaa7faeba1b5c32f5fcaeaa3",
						subject:
							"#579: ノートの書式付き操作ツールバーを操作メニュー側に一元化する",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "2b1005f6965629112d504a353dc1c7711b3eab80",
						subject:
							"#572: アップデートスクリプトのアップデート前後処理スクリプトが既存スクリプトを使用している",
						comments: [
							"関連: #574: 0.92.0 公開時に 0.91.0 以上をアップデート可能対象にする",
						],
					},
					{
						revision: "21348cd12d35fcde3634a7431eb91ef22c8047b8",
						subject:
							"ノートの種別変更で同一種別に変更しようとすると内部的に例外が飛んでいた事象の修正",
					},
					{
						revision: "de0be7a47b4e1feb602a01fceb7c2507354e6e56",
						subject:
							"#551: もしかしてだけど Pe から Pe.Main に --wait が飛んでる？",
					},
					{
						revision: "4dffd989953d6b9ab889def88c42011e5e1c15b0",
						subject:
							"#554: アクセントカラーがなーんかまだ透明っぽいときがある",
					},
					{
						revision: "4dffd989953d6b9ab889def88c42011e5e1c15b0",
						subject:
							"#575: テキスト <-> 書式付きの変換処理で改行が取り払われる",
					},
					{
						revision: "7b56a24a5a74e5b1878257bf1f5b9fcd7a5786d5",
						subject:
							"エクスプローラ補正のキャッシュ数指定が 0 になってる不具合修正",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "a201feb9f96bb41fb6925155e11673ea304b77fa",
						subject:
							"#573: 自前でプロパティ変更通知している処理の高速化",
						comments: [
							"これまた遅くなった気がしないでもないけど気にしない",
						],
					},
					{
						revision: "b947c6b31734dd1b4031db3c8f27c290bb0fca8b",
						subject: "#570: Dispatcher.Invoke を滅ぼしましょう",
					},
					{
						revision: "a0823b0101df750fc0b2c8424c2352368f0f6d0e",
						subject: "#543: 0.83.0 からのデータ変換処理を破棄する",
					},
					{
						revision: "03bde067f136480e03031e4583bc3b64a27c2526",
						subject: "#582: 本体内部にログの一部を保持する",
					},
					{
						revision: "e401a6afd0ab769e319ac60cf6d210bdab852e0b",
						subject:
							"#578 ノートの「書式付き」をリッチテキストに変更する ",
					},
				],
			},
		],
	},
	{
		date: "2020/03/08",
		version: "0.90.000",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "notice",
						subject:
							"バージョン 0.91.000 で 0.83.0 のデータコンバート処理を破棄します",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "ded9c606ccc1394846f2b813ddfdd455a9ffa2f9",
						subject: "#562: コマンド型ランチャーを ESC で閉じる",
					},
					{
						revision: "6e771610a6d0f5f2188615424c8cd1100996dc16",
						subject:
							"#487: デプロイ時にSQLファイルを一つにまとめる",
						comments: [
							"思ってたより意味がないというか若干遅くなったけど気にしない",
						],
					},
					{
						revision: "6449c91f3235d08b042dd58fe197bd3f563991b6",
						subject: "#508: ノートの書式付きを操作できるようにする",
						comments: [
							"本課題の副産物としてカラーピッカーに既定の色を追加",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "d582c63e3b89505e729af9113de102e6c9b7c1a0",
						subject:
							"#563: コマンドランチャーで 1 文字だけの入力だと初期選択が行われない場合がある",
					},
					{
						revision: "3f959aa37dc4d384ab9e0f183033a5318260ce63",
						subject: "#564: RDP 復帰後に落ちることがある",
					},
					{
						revision: "f9bb81f4a6590cf1087acfed9448162e40797079",
						subject:
							"書式付きノートを最小化/元に戻すをガチャガチャした場合に落ちるの対応",
					},
					{
						revision: "35a1221d36edfd22f4f6617f85e10742d6cbe56a",
						subject:
							"#558: 標準入出力のタイトルバーがアイテム名じゃなくてパスになってる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "29a41075eb122c509d9823f78e08d657d6ae868c",
						subject: "#542: ログ出力周りの整理",
					},
					{
						revision: "ab435c7f58ea01c50b3c8d065996debf118c3360",
						subject:
							"#557: 絶対に動的じゃないとダメでない SQL はファイルとして外に出す",
					},
					{
						revision: "7135ae9fcc8cdc451a7615fb8f591ed25ce30263",
						subject:
							"#567: アプリケーション構成ファイルをバージョン専用で使用できるようにする",
					},
					{
						revision: "a5926acff7ddca674dc5321387c13b5a134683dd",
						subject:
							"#568: デバッグ時のノートがデバッグ用として見てわかるようにする",
					},
					{
						revision: "9f3def2684720169010fe6a637eb57520ac95fc6",
						subject:
							"#565: コマンドライン引数のドキュメントを作成する",
					},
				],
			},
		],
	},
	{
		date: "2020/03/04",
		version: "0.89.000",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "notice",
						subject:
							"バージョン 0.91.000 で 0.83.0 のデータコンバート処理を破棄します",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "1c3d38a7864df733511166519d1b1653b2138e3e",
						subject: "#524: コマンド入力のスコア評価を改善する",
					},
					{
						revision: "9f8bf74cc6115c185605976e0db6df247f9e9a1e",
						subject: "#502: コマンドランチャーで待機時間は不要",
						comments: [
							"待機時間をなくすとともに列挙したアイテムの表示処理を改善",
						],
					},
					{
						revision: "3abcddddb765cc3518a782b9605d5fac8e711b83",
						subject:
							"#556: マウスの戻る・進むボタンでグループの切り替えを行う",
					},
					{
						revision: "b009fecc49f444949c526768a353c3f292a1fbb4",
						subject:
							"#548: 自動的に隠すツールバーを強制的に隠した場合の表示条件該当に制限を入れる",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "5b5f2cd74188eb8eb78807d201cd547e8c8514e5",
						subject:
							"#544: コマンド型ランチャー入力時の色設定が完全にデバッグ用なので適当にいい感じにする",
					},
					{
						revision: "75a38b0cb9ff06646627c162a8013bd8743e04e6",
						subject:
							"#522: ツールバーの初期グループ選択設定が未選択の場合に選択されているものとして扱われる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "81ea3b586a491bb96bdab6d328de08ebca402980",
						subject: "#518: 配布形式を 7z にする",
					},
					{
						revision: "851425c7aacab4dce068424b636da3df09b9c95c",
						subject:
							"#545: リリースビルド処理で node_modules のキャッシュは外す",
					},
					{
						revision: "b5cfd2810b064bbd244684fe2e834643b87c8d28",
						subject:
							"#555: Pe の所有しているディレクトリに対しては安全にアクセスできる仕組みを作る",
					},
					{
						revision: "bcfcb271b5eb08179f5c8e03dab9d598e86eeac2",
						subject:
							"#552: メインアイコンを 3 つも持つ必要なくない？",
						comments: [
							"CI ビルド時に切り替えるようにして *.ico 自体はリポジトリ管理にした",
						],
					},
					{
						revision: "dcf88dd4ef9a72558923cbc03e948dfad93907e3",
						subject:
							"#547: デプロイ処理の対象サービスでアーカイブ配布先とタグ付けが一緒になってる",
					},
				],
			},
		],
	},
	{
		date: "2020/03/01",
		version: "0.88.000",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "notice",
						subject:
							"バージョン 0.91.000 で 0.83.0 のデータコンバート処理を破棄します",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "a5d5c96f6a86b1c31364c783e738111d4879eaf1",
						subject:
							"#534: ネットワークドライブのコードが取得できない",
					},
					{
						revision: "05bb0111d70b6ad17b69c0fecbaf0e70f5cb2013",
						subject:
							"#540: 特に指定のないアイテムの並び順が謎極まる",
					},
					{
						revision: "1c73b4f1fd4f649d5db4ce8ffcd64d36c36b098b",
						subject:
							"#541: アイコン表示に失敗すると連鎖的に全部失敗してる感",
					},
					{
						revision: "89ed09c7567367b7559dc1697e87b3dc1193cc00",
						subject:
							"#538: ネットワーク接続時のキャッシュ暫定回避をきちんと対応する",
					},
				],
			},
		],
	},
	{
		date: "2020/02/29",
		version: "0.87.001",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject:
							"起動条件によりアップデートに失敗することが分かったので緊急リリースです",
						comments: [
							"コマンドライン引数なしで起動した場合にアップデートスクリプトが実行できないのでダミーで引数を付けるか、<Pe>\\bat\\safe.bat で一時的に起動するか、手動でアップデートが必要です",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "4f34892038d792af8c3b114a35861455d93541d3",
						subject:
							"#539: コマンドライン引数無しで実行した EXE のアップデートが行えない",
						comments: [
							"ずーっと --log 付きで実行してたから全然検知できていなかった",
						],
					},
				],
			},
		],
	},
	{
		date: "2020/02/29",
		version: "0.87.000",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "140fdfe6e90055d9e4101d2455555d5a11a3dac9",
						subject: "#532: 設定画面で落ちる",
					},
					{
						revision: "430861596a61385a4ea68ca3d337d5fa50dc6358",
						subject: "#535: RDP接続で落ちる",
						comments: [
							"解像度変更が主に死んでる",
							"ある程度は改善できたと思う。思う。。。",
						],
					},
					{
						revision: "81813b0243611a62d8d1b33a71fd0d66874bf8d5",
						subject:
							"#519: システムのアクセントカラーが透明なときがある",
					},
					{
						revision: "aa339950fbef98ea0bbc9203956ae2f299a9cf6c",
						subject:
							"#523: 内臓ブラウザでリンクを標準ブラウザで開く制限ページにも関わらずホイールクリックによりインターネット世界に羽ばたいていく",
					},
					{
						revision: "c3ab878b1a8c9ed4510483d1d9459e7558b22016",
						subject:
							"#537: 設定画面の「ユーザーIDと使用統計情報」のリンクが完全に置物",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "31ed7edf55b90ffa90b9c2c11b029e7f061c4c44",
						subject: "#517: CLR って v4.0.30319 でいいの？",
						comments: [
							"修正ついでに長い情報に RuntimeInformation を追加",
						],
					},
					{
						revision: "b6fd37ea44b7e024225c0a279cbff1627a7fca51",
						subject:
							"#533: Microsoft Visual C++ 再頒布可能パッケージは Pe.Main プロジェクトから除外する",
					},
					{
						revision: "a7e58267cb21809f5ec906907d1850d257e3a5bf",
						subject:
							"#501: 過去バージョンのダウンロード先を明記する",
					},
				],
			},
		],
	},
	{
		date: "2020/02/26",
		version: "0.86.000",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"🙇 32bit 版は手動でアップデートしてください🙇",
						comments: ["大きめの不具合だし早めにリリース"],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "f19f850b0adfab1627789468bb3b12c701257543",
						subject:
							"#512: スタートアップ登録時に引数も登録できるようにする",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "626553bfb5727d6f2e31b3e67a9fb02fad052867",
						subject:
							"#526: アップデート時に PowerShell が実行できない",
						comments: [
							"32bit 版で実行できなかった",
							"x86,x64 のみを受け付けるようにしていたところを x32,x64 を受け付けるようにしていて x86 を渡していたから死んだ",
							"x32 て。。。",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "77c3761e3a307b6c82d55b27a9b7849c45b3795d",
						subject: "クッソしょうもないお絵かきが楽しい年ごろ",
					},
					{
						revision: "260ccf574072d8e539ca509a18078ebfb31a6051",
						subject:
							"VMからテーマUI要素をごにょごにょするところはなんも考えなくていいはず",
					},
				],
			},
		],
	},
	{
		date: "2020/02/26",
		version: "0.85.000",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "98575402e5956db442cff82752bd3d344ca0e1f3",
						subject: "#504: ヘルプファイルの再作成 ",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "af68ed61adea4e411c79df3d666c7d92fa9d7715",
						subject:
							"#514: 初回起動時に ArgumentNullException で落ちる",
					},
					{
						revision: "90ab38bbefc98a8285cf00e6c83f9bcf623e8a10",
						subject:
							"#516: Microsoft Visual C++ 再頒布可能パッケージ のインストールを不要にする",
						comments: [
							"対応として再頒布可能パッケージを同梱し、 Pe.exe (PeMain.exe) 起動時に PATH に <Pe>\\bin\\lib\\Redist.MSVC.CRT\\<CPU> を追加するようにした",
							"インストールされてればそれを使用するし、インストールされてなければ同梱版が使われるのでたぶん大丈夫",
							"たぶん Windows 10 なら問題ないと思うんだけどクリーン環境で試してなくて、未サポートの Windows 7 環境で試したから根本的に何か間違ってるかも",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "b35a7e67468c60f30d5650487180c1f47e83856a",
						subject: "CIのバッジが開発用向いてた",
					},
					{
						revision: "af68ed61adea4e411c79df3d666c7d92fa9d7715",
						subject:
							"#515: CefSharp を使用するために必要な要件をきちんと調べる",
					},
				],
			},
		],
	},
	{
		date: "2020/02/24",
		version: "0.84.000",
		group: ".NET Core 3.1",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "78ce8c309c5efc1e586fc209560063734094b792",
						subject: "#484: 設計練り直して作り直し",
						comments: [
							"今回の主要アップデートで他のは付随してきただけの課題です",
							"色々足りないけど自動更新機能が動けば何とかなる思い",
						],
					},
					{
						subject: "根っこからめっちゃくちゃ実装を変えました",
						comments: [
							".NET Framework から .NET Core に移行したので環境依存に関する制限がある程度なくなりました",
							"内蔵ブラウザを .NET Core(Forms, WPF) のシステム依存から Chromium 実装の CefSharp に切り替えました",
							"ただしこれらの対応によりファイルサイズがすっごい大きくなっています(100MB超)",
						],
					},
					{
						class: "compatibility",
						subject: "実装変更に伴い互換性が結構なくなります",
						comments: [
							"Windows 10 を主軸にしたことで隠しファイル・拡張子切り替え機能の廃止(Explorerですぐに実施できるため)",
							"-> Windows7 では動きません(CefSharp が死ぬ)",
							"ノートの「書式付き」は現状操作できません",
							"グループ選択とランチャーアイテムコンテキストメニューを統合(ここに至るまで色々あったが全部忘れたものとする)",
							"標準入出力ウィンドウの機能を削りました。時間あるときに戻します",
							"コマンドラインオプションの互換性を破棄しました",
							"ランチャーアイテムの種別 コマンド を破棄しました",
							"実行形式の配置場所・名称等の関係の互換性を破棄(.NET Core 移行に伴う bin ディレクトリのわちゃわちゃ感)",
							"ユーザー情報送信機能は手が足りなかったので確認はしますが機能しません",
							"テンプレート機能を廃止しました",
							"Windows がクリップボード頑張っているのでクリップボード機能を廃止しました",
							"ランチャーウィンドウのフロート表示を破棄しました",
							"ログウィンドウを破棄しました",
							"ヘルプドキュメントは作ってる途中です",
						],
					},
					{
						class: "compatibility",
						subject:
							"0.83.4 からライセンスを GPL 3 から WTFPL 2 に変更します",
						comments: [
							"0.83.0 以下は GPL 3 扱いでリポジトリから取得可能です",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject: "#459: アップデート処理の実装周りを整理",
					},
					{
						subject: "#485: 高 DPI 対応",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "#472: ノートの斜め方向リサイズ領域を広げる",
					},
					{
						subject: "#469: 自動的に隠す状態のツールバーが云々",
					},
					{
						subject:
							"#452: ツールバーが自動的に隠れる設定でアイコンが非表示になる",
					},
					{
						subject:
							"#439: グループ名変更時に変更用入力UIの位置が変",
					},
					{
						subject:
							"#425: 一意に識別される設定項目はその一意な値を表示する",
					},
					{
						subject: "#417: 列挙体の保存値を数値から名称にする",
					},
					{
						subject:
							"#380: ランチャーアイテムがネットワーク越しのファイルだとアホみたいに遅い",
					},
					{
						subject:
							"#369: ノートのタイトルバーについてるボタンをもうちょっと見栄え良くする",
					},
					{
						subject:
							"#313: 四辺に配置したツールバーをシステムメニューから移動すると大変なことになる",
					},
					{
						subject: "#300: メッセージボックスがダサい",
					},
					{
						subject:
							"#112: HTMLレンダリングコンポーネントを変えたい",
						comments: ["CefSharp に全権委任"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "dfed410e2ec4a9bcc637bdefa5f4cf94ba482287",
						subject:
							"#480: myget: SharedLibrary から Pe 限定の処理を抜き出し ",
						comments: ["更新履歴には一応乗せるけど完全に死に項目"],
					},
				],
			},
		],
	},
	{
		date: "2017/06/11",
		version: "0.83.0",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "8dea8d185cd900776b76aaa37b68d2905aa8f75b",
						subject: "#482: 完全透明状態は設定できないようにする",
					},
					{
						revision: "99ad70bbff987819e7a185004915229d5f745f58",
						subject:
							"#483: コマンドのパスが不正な際にツールバーからアイテムメニューを開くと落ちる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "",
						subject: "nuget, myget 周りアップデート",
					},
				],
			},
		],
	},
	{
		date: "2016/12/31",
		version: "0.82.1",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject: "0.82.0 は 0.82.1 に統合",
						comments: ["ミス大杉"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "26d84b3286f16713a3b2e2c70a50a8e5c55755f6",
						subject:
							"#481: セッション終了時にデスクトップサイズをシステムに返却する",
						comments: [
							"セッション終了時にツールバーを一旦破棄するようにした",
						],
					},
					{
						revision: "2cc7141f145df536031a3923e0e6f37251d5be1b",
						subject:
							"#437: windows10でツールバーの色をwindowsの設定に合わせる",
						comments: [
							"レジストリ調べきってないので追従できてない",
							"とりあえず Windows10 でツールバーが透明になる問題に対応が主、追々別課題でまた対応する",
						],
					},
					{
						revision: "5fca089afbc10a5953ff96bf09404d881440c180",
						subject: "#479: クリップボード取り込み時に落ちる",
						comments: ["再現できず。とりあえず lock で逃げる"],
					},
					{
						revision: "7e6ebc583dd7e4736c30f19f0dfd79cf23d30598",
						subject:
							"64bit版プロセスでアクセントカラーが自動取得の場合に OverflowException が発生する",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "2ae0c328694f629ac955f4a81fa11f5191627980",
						class: "nuget",
						subject: "Extended.Wpf.Toolkit 2.9 -> 3.0",
					},
				],
			},
		],
	},
	{
		date: "2016/08/17",
		version: "0.81.1",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "",
						subject: "[PR] ニコニコ見るツール作った",
						comments: ["https://bitbucket.org/sk_0520/mnmn"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "4bb40c7cdd12417bd68891f5f256a72746ad64ca",
						subject:
							"ヘルプページの先頭が general でなかったリンクミスの修正",
					},
					{
						revision: "0f65ea0d88fb786c6bf6b87e5d4d1fc82e036037",
						subject: "#475: ランチャーアイテムの履歴が保存されない",
					},
					{
						revision: "09763b1274ae206e1728ace9816d22cf55e98703",
						subject:
							"クリップボードのフィルタ入力部分の位置が変だった",
						comments: [
							"全然ダメだったので 0.81.0 -> 0.81.1 への急遽リリース",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "230151dd075a83391cdbd05762a7e70449bff231",
						class: "nuget",
						subject: "Network.Json を 9.0.1 に更新",
					},
					{
						revision: "230151dd075a83391cdbd05762a7e70449bff231",
						class: "nuget",
						subject:
							"Extended WPF Toolkit™ Community Edition を 2.9.0 に更新",
					},
					{
						revision: "e98a5275da66a3862acc064e20e588f7975483a5",
						subject: "#464: 設定モデル複製処理の自動化",
						comments: ["本対応で #475 修正が無意味になった"],
					},
					{
						revision: "b0f5c995b9a7f74d849ead2523bd18dea0d9d311",
						subject:
							"開発環境を Visual Studio 2015 Update 3 に変更",
					},
					{
						revision: "230151dd075a83391cdbd05762a7e70449bff231",
						class: "nuget",
						subject: "NUnit3TestAdapter を 3.4.1 に更新",
					},
					{
						revision: "36ccdfb47bd04db39695687a76386adceb3ec470",
						subject: "ソースディレクトリの変更",
						comments: ["/Pe -> /Source"],
					},
				],
			},
		],
	},
	{
		date: "2016/06/12",
		version: "0.80.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "938d3a6a63b0825b1200ead75097be3b07c35d2d",
						subject: "#448: ノートに書式を持たせる",
						comments: [
							"RichTextそのままだと直感的でなくなるためワードパッドとは扱いが若干異なります",
							"主に段落関連を除外しています",
							"あと書式付きの場合は背景色を細かく設定しないほうがいいです",
						],
					},
					{
						revision: "11bc0bfe11a97edbd8187760c1bdcef90516dd57",
						subject: "#413: ヘルプファイルに更新履歴を表示する",
						comments: [
							"今バージョンから更新履歴を確認するにはヘルプ(通知領域メニュー→情報→ヘルプ→更新履歴)から確認してください",
							"情報→更新履歴は廃止です",
							"IE依存してたしアップデート用の出力処理と確認用の表示処理が二重で地味に負担だったのですよ",
						],
					},
					{
						revision: "f667c0d30400278b9539180badf86cf83234d32c",
						subject:
							"#440: クリップボード・テンプレートのリストダブルクリック操作",
					},
					{
						revision: "f85ff85c9426d058572cffbedf050611aa416860",
						subject:
							"#470: 設定のバックアップにバージョン情報を付与する",
					},
					{
						revision: "70d8b04addf6e9a8fa89eac3960a28e01e5d8340",
						subject:
							"#465: ウィンドウを強制的に隠す操作にマウスも追加する",
						comments: [
							"ツールバーの設定[自動的に隠す]が有効な場合にツールバーの空いている領域をダブルクリックするとツールバーを隠れた状態にします",
							"ボタン上でも出来ちゃうけどそこはまぁ運用回避で",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "b6ded0bf968b8f02ac0244d89cfd351521cae3e0",
						subject:
							"#461: Windows8, 8.1, 10 でのツールバーがきもい",
						comments: ["妥協の産物"],
					},
					{
						revision: "c52547879b58eb7aaa8bee0c674dd75191970d35",
						subject: "#462: GridHelpersのリンク先が間違っている",
						comments: [
							"修正はしたんだけど別ライブラリに移動させたので記述から消えた",
						],
					},
					{
						revision: "e1eabcb733eba9a79f50b541fcafcf2544d0f78e",
						subject:
							"#458: クリップボード取り込み待機時間の設定UIが直感的でない",
					},
					{
						revision: "b09ce6688e129db95d41b752f1f259858609f361",
						subject: "#451: 設定項目のUIが直観的ない部分を調整する",
					},
					{
						revision: "450e1c5d1aa119e3234c1122e5965fae7405d0de",
						subject:
							"#471: 構成ファイル backup-archive が使用されていない",
					},
					{
						revision: "4c74f93ef4e7ff4cc311f34df67ab3c114796cca",
						subject:
							"#468: ノート最前面表示をホットキーから実施するとできたりできなかったりする",
						comments: [
							"非アクティブ縛りで変に泥臭いことになってしまったけど多分動くよ",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "8c5ba7fa84f50a3e9f69b4a8a4c9e26b5e491e4b",
						class: "nuget",
						subject: "ICSharpCode.AvalonEdit を 5.0.3 に更新",
					},
					{
						revision: "016e48184a6f3e3bd872bc18aa79f7deb2c13578",
						subject: "データ補正処理を統一",
					},
					{
						revision: "11bc0bfe11a97edbd8187760c1bdcef90516dd57",
						subject: "#284: 更新履歴の空白データ要素を表示しない",
						comments: ["#413実装時に同時解消"],
					},
					{
						revision: "400b969282d589e992a512eed6c0ec10ff469085",
						subject: "XAMLの名前空間を整備",
					},
				],
			},
		],
	},
	{
		date: "2016/05/18",
		version: "0.79.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "17f2f45d998dbf266ad0c5c40a6771d4f9ec1afe",
						subject:
							"#350: ツールバーのメニューボタン「▼」の今後の身の振り方について",
						comments: [
							"ツールバー設定からランチャーのメニューボタンを表示するかどうか設定可能にしました",
							"メニューボタン非表示状態でメニューを表示するには「マウス中央ボタン」、「Shift + 右クリック」のどちらかの操作が必要です",
						],
					},
					{
						revision: "3cb7beb228d348d0557e2c33d35e03255fd10931",
						subject:
							"#449: ランチャーアイテムの並び順を保存時にソートする",
					},
					{
						revision: "018e9be333d09bb4732be0c3195e465bbb76f115",
						subject: "#455: ツールバーを強制的に隠した状態にする",
						comments: [
							"ツールバーの「自動的に隠す」が設定されている場合に ESC キーを二回入力すると表示中のツールバーが隠れた状態になります",
						],
					},
					{
						revision: "ab7dcc91053c6c622c76f550e633bce6649a31c0",
						subject: "#454: F1抑制機能を付ける",
						comments: [
							"F1キーの誤入力を防ぐ機能です",
							"本機能が有効な時にF1を入力したい場合は F1 を二回入力してください",
							"設定箇所: 設定→本体設定→システム環境→F1キーを抑制",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "e60fdbe111507ab6f0be5bdf0f968710f93d0d58",
						subject:
							"#383: ツールバーに表示しきれずはみ出したアイテムに救いの手を差し伸べる",
						comments: [
							"ツールバーのコンテキストメニューからはみ出したアイテムを選択できます",
							"ただしこの場合は通常実行の挙動に限定されます",
						],
					},
					{
						revision: "46f4de86ff4829edd6d637e04331b49dc8062751",
						subject:
							"#447: グループ内ランチャーアイテムを上下矢印UIで移動させると落ちる",
						comments: [
							"ランチャーアイテム側で削除したアイテムがグループ配下側で内部的に保持されたまま不可視だったことが原因なのでこれを可視化した",
							"ついでに #456 対応",
						],
					},
					{
						revision: "bea456a776693cc4331dbd639fe8d730a448927c",
						subject:
							"#456: グループ設定でノード操作時のちらつきを抑制",
					},
					{
						revision: "dfcdc936844ae25d8c94d7bdc9acb2f1519349bd",
						subject:
							"#450: ノートのサイズ変更枠ダブルクリックでもサイズ変更出来てしまう",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"#446: CIビルド時にpdbファイルは削除しないようにする",
						comments: ["なんか急にしたくなっただけ"],
					},
					{
						revision: "b5446023f209716ec684fd0f91ac34d1ca4773e9",
						subject: "[nuget] Extended.Wpf.Toolkit を 2.8.0 に更新",
					},
					{
						revision: "17e9a097b60c769ef71c5660ff5b3771be43d9c5",
						subject: "#457: グローバルフック実装",
						comments: [
							"Forms 版で使用していた Gma.System.MouseKeyHook を使用",
						],
					},
				],
			},
		],
	},
	{
		date: "2016/05/08",
		version: "0.78.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "46e2b6b4383e448e2d232d201ded0a636dad9615",
						subject:
							"#436: コマンド入力で列挙するかどうか条件設定を追加する",
						comments: [
							"コマンド入力保管対象を設定することで切り替えます(デフォルトで有効)",
							"入力補完時に有効設定アイテムのみ列挙し、入力が完全一致する場合は設定が無効でも列挙されます",
							"0.77.0 以下からのアップデートは全アイテムがデフォルト値に強制されます",
						],
					},
					{
						revision: "11fa3c838fdd8da9712267c7580c3413f929f380",
						subject: "#339: グループにアイコンを設定する",
						comments: [
							"使用可能なアイコンは Pe 組み込みのアイコンのみに制限されます(ネットワーク上のアイコンとか使うと遅いので)",
						],
					},
					{
						revision: "f0c5f9fedd871003676c76c2d4df369694139569",
						subject:
							"#443: ノートのキャプションバーをダブルクリックでタイトル入力を行う",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "0fedad90fe8fc77ff45396a40d2e3192fce134cc",
						subject: "#438: ノートの移動ができない",
					},
					{
						revision: "2f91c0c618d69d6fb1cf3f921c9a079656605f86",
						subject:
							"#317: 数値の範囲指定入力欄のテキストボックスをアップダウンコントロールにする",
					},
					{
						revision: "df865b16d902ac5d0bbb28facd73aa2f62dde466",
						subject:
							"#318: 時間の範囲指定入力欄のテキストボックスをアップダウンコントロールにする",
					},
					{
						revision: "94e9872eec5a070a8fd8baf178824b3592eb8565",
						subject:
							"#444: ノート設定の標準スタイルに折り返しを追加する",
						comments: ["折り返しと最前面を追加した"],
					},
					{
						revision: "feb09bba3193d9eb109353449c1ba3e32451551b",
						subject:
							"#445: 指定して実行ウィンドウの初期フォーカスをオプションに設定すべき",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "de0e98d3692a86165feceb89dbefce7eeca545df",
						subject:
							"開発環境を Visual Studio 2015 Update 2 に変更",
					},
					{
						revision: "8f28de02237d714724c8f7e4619e13f22f71d441",
						subject: "[nuget] Extended.Wpf.Toolkit を 2.7.0 に更新",
					},
					{
						revision: "a4620d2ee9c5a0f5214da04d4c75a5cd841b307f",
						subject:
							"[nuget] Hardcodet.Wpf.TaskbarNotification を 1.0.8 に更新",
					},
				],
			},
		],
	},
	{
		date: "2016/04/07",
		version: "0.77.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"ひさびさリリース。ランチャーと関係ないツール作ったりダークソウル3したりで忙しいのです",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "91bbd100ea168425f2dd48fdb4b01f548bd10535",
						subject:
							"#428: 各種データのアーカイブ化タイミングを再検討",
						comments: [
							"端末が一定時間アイドル状態であれば各データをアーカイブするよう変更",
							"設定値はApp.config(PeMain.exe.config)",
							"変更に伴いクリップボードのアーカイブ間隔を変更",
							"アイドル監視時間: 8分",
							"アイドル判定時間: 70分",
							"クリップボード閾値: 3時間",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "95a0ed24751b74615f10cb768b1c8302f35ee447",
						subject:
							"#426: ヘルプファイルのファイルとディレクトリアイコンが出力ディレクトリに出力されていない",
					},
					{
						revision: "762ceb36f86045633c129a8cedb44ae98526f8df",
						subject: "#432: ノートの標準フォント設定が反映されない",
					},
					{
						revision: "105dee68a5329f80b4febf68c6d63da48ec48cd7",
						subject:
							"#429: フィルタリング中のクリップボードコピーで落ちる",
						comments: [
							"例外捕まえただけの暫定対応",
							"原因調査してないので今後やっていく",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "84cde1568d8eb67a2aa6860e2dc7d9e995209103",
						subject: "名前空間とかライセンス表記とかがさっと修正",
					},
					{
						revision: "f3add1826547df139e487c4e9446ed71d89bd196",
						subject:
							"[myget/NuGet] ShaerdLibrary更新に伴い関連ライブラリの更新",
						comments: [
							"#431: PeからSharedLibraryへ統合した処理に委譲",
						],
					},
				],
			},
		],
	},
	{
		date: "2016/01/18",
		version: "0.76.1",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"[緊急] 0.76.0 で設定ウィンドウの保存実行後に Pe が落ちる問題に対応したため 0.76.0 と 0.76.1 は統合",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject: "#408: GUID重複に備える",
						comments: [
							"Peの開発が続いている間にこの処理が日の目を見ることは100%ないだろなぁ",
						],
					},
					{
						revision: "22b018037285e9837520b463ef1a450ee8d8a27a",
						subject:
							"#420: Extended WPF Toolkit™ Community Editionのバージョンアップ",
						comments: ["2.5 → 2.6"],
					},
					{
						revision: "9863916ffe321091f3ba2f75a7fcbce67591aa7d",
						subject: "#421: Json.NETのバージョンアップ",
						comments: ["8.01 → 8.02"],
					},
					{
						revision: "3d58546d546d42157abe6401ab303fa2567c58db",
						subject: "#364: App.configの設定値をキャッシュする",
					},
					{
						revision: "4366619ab4277922fd8ff5acdd11c66caa8ef2d7",
						subject:
							"#423: HTMLクリップボードのURIを規定プログラムで開く",
					},
					{
						revision: "c925ad58ff0c3a344fe892ab9f53e0781e31c6ea",
						subject: "#362: App.config(PeMain)の説明",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "ea0a8ccaf4ca30ac4335ee094fd53dd18c8ee9d7",
						subject:
							"#353: Windowsの終了・ユーザーのログオフを妨げる",
						comments: [
							"調査結果としては設定ファイルのバックアップ、特にクリップボード全件保存の場合に各データファイルのバックアップに時間がかかっていた",
							"クリップボード・ノート・テンプレートの実データが閾値に該当するものをアーカイブした状態で保持するように変更",
							"クリップボード閾値: 更新が 3 日前で 256KB 以下",
							"ノート閾値: 更新が 7 日前で 1KB 以下",
							"テンプレート閾値: 更新が 10 日前で 4KB 以下",
							"閾値は App.config(PeMain.config) で定義されてるので内容については #364 を参照してください",
							"Pe 終了時にバックアップ→本処理の流れで実施されるためアップデート後の二回目終了時に効力が現れます",
							"デバッグ版ではうまくいったよ！デバッグ版ではね！",
						],
					},
					{
						revision: "73c2e58b179dbf215f632dd69591134ab80c68fb",
						subject: "#375: 起動時に各UI制御を行う",
					},
					{
						revision: "84ba9542fc5fb57e573b62065c33eb8880cdf820",
						subject: "#427: 設定保存時に死ぬ",
						comments: [
							"内部的に掴んでいるファイルをさらに掴もうとしていました",
							"一部ややこしい問題もありました",
							"根本的に処理変えたところもありました",
							"おちこんだりもしたけれど、私はげんきです",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "a0afdd9051b074b019d90e8c0b53a87d3db1517d",
						subject: "#418: 独立可能なライブラリを独立させる",
					},
					{
						revision: "678005664f3a275d022d1b094720eb142c097a8d",
						subject:
							"#419: 開発に関する諸々をヘルプファイルに記載する",
					},
				],
			},
		],
	},
	{
		date: "2016/01/04",
		version: "0.75.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "あけおめ",
					},
					{
						class: "compatibility",
						subject:
							"#415の影響によりユーザー設定ディレクトリ(標準だと %APPDATA%Pesettings あたり)の *.tmp ファイルが削除対象となりました",
						comments: [
							"Pe の設定ファイルが配置されるディレクトリなのでユーザー側でどうこうしてるとは思えないけど一応周知",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "8fd925990917f64b7324b01740b61de81ac02a67",
						subject:
							"#397: 言語ファイル読み込みにはdefault.xmlを親とする",
						comments: [
							"そもそも条件的にdefault.xmlしか読めてなかったぜ！",
						],
					},
					{
						revision: "b3ba79bd33e51aabc0cc80864d72a03beb049d91",
						subject: "#237: テンプレート入力エディタを高機能にする",
						comments: [
							"AvalonEditを使用",
							"今回実装分では単純に色設定のみ",
						],
					},
					{
						revision: "65717cf3e0b63971a383c92cb40d2f0117af6d54",
						subject:
							"#415: 設定ファイルへの書き込みはデータ出力後にファイルを置き換える",
					},
					{
						revision: "dd26b9ee23c5cc45794741be9af2bffbd49d7d11",
						subject: "#411: Json.NETを 7.0.1 → 8.0.1 にする",
						comments: ["おっきな対応は#412で実施"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "1662115601ef2c0fc4f0c4d63f5ede8d8867f598",
						subject: "#410: ログのファイル出力うまくいってない？",
						comments: [
							"#393, #355でどうにもうまくいかなかった原因",
							"なんかロジック的には正しかったけど内部使用しているパラメータの扱いがミスってた",
						],
					},
					{
						revision: "48ba67dbea1926477bb85fa9ff3763511b7ef84d",
						subject: "#402: ウィンドウの背景色をシステムに合わせる",
						comments: ["コントロールの色に合わせた"],
					},
					{
						revision: "ac36b67564191a961462fdf93420a1c9d9f93d36",
						subject:
							"#412: HashItemModel.Code の保存形式を変換する",
						comments: ["今回リリースで一番の不安処理"],
					},
					{
						revision: "6c53ec47572fc29a78dda532bbc278544957a335",
						subject:
							"#414: パース出来ない設定ファイルの読込で落ちる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "bdca07370be06b8919af45c5b5703622a82fc3b1",
						subject:
							"#416: シリアライズ処理に使用した元ストリームは呼び出し側で面倒を見る",
					},
				],
			},
		],
	},
	{
		date: "2015/12/23",
		version: "0.74.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "e7fe3b78cd68c8ec7b41b8c6f2966ed18a9a3488",
						subject: "#25: ヘルプファイルの記述",
						comments: [
							"通知領域コンテキストメニュー→情報→ヘルプ",
							"ひっさしぶりに生のHTML/CSS書いた",
							"読み込み時の細かい処理は追々調整する",
						],
					},
					{
						revision: "82221341f524f7ac80bdf75935f3bbf5349c07b5",
						subject:
							"#392: ホットキー処理を実施した際のトースト(バルーン)表示を選択制にする",
						comments: [
							"設定→本体設定→操作通知",
							"Windows 10 で出まくるの鬱陶しいので初期値は「なし」に設定",
						],
					},
					{
						revision: "a383696689fde08f647937b5361a60a3a3901c5c",
						subject:
							"#370: クリップボードHTMLデータからクリップボード名を算出できない場合はテキストから取得する",
					},
					{
						revision: "a3819ed98e56e3f45ff44cea38c267d3145b5bde",
						subject:
							"#393: ログ出力をコマンドライン指定でなくGUI側でいつでも出力できるようにする",
						comments: [
							"#355の逆襲",
							"本実装に伴いファイルへのログ出力実装を変更したけ通常使用には無関係",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "a2ecce5d3cadfeffae6e0f33f059b1dc75563cd7",
						subject:
							"#406: ファイルのローテート処理で最新ファイルを削除している",
					},
					{
						revision: "db117745015f1f5f9c672b9a19bf4c42242a5e41",
						subject: "0.73.0の変更履歴にリビジョン記入してなかった",
					},
					{
						revision: "d82438892c577157aeb4df9f53a9cdb3164d3696",
						subject:
							"#405: ホットトラックの色算出に黒・白・灰色は計算に含まないように変更する",
						comments: ["色の勉強しないと難しいなぁ"],
					},
					{
						revision: "9c649e1814181584f6ae510ccf22cc6071efcf57",
						subject:
							"#387: ランチャーアイテム登録中にアイコンの反映が行われない",
						comments: [
							"コマンド項目修正は毎回ディスク見に行くのアホっぽいから500msの遅延更新",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"開発環境を Microsoft Visual Studio Community 2015 Update 1 に変更",
					},
				],
			},
		],
	},
	{
		date: "2015/12/06",
		version: "0.73.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "de74c412a997761664c9b76cb5c113fea0e694a9",
						class: "compatibility",
						subject: "#335: .NET Frameworkのバージョンを4.6に変更",
						comments: [
							"本バージョンから .NET Framework 4.6 が必要になります",
							".NET Framework 4.6 は https://www.microsoft.com/ja-jp/download/details.aspx?id=48130 からダウンロードできます",
						],
					},
					{
						subject:
							"本バージョンからアップデートチェックに使用するアドレスが変更となります",
						comments: [
							"XML -> https://bitbucket.org/sk_0520/pe/downloads/update.xml",
							"HTML(Release) -> https://bitbucket.org/sk_0520/pe/downloads/update-release.html",
							"HTML(RC) -> https://bitbucket.org/sk_0520/pe/downloads/update-rc.html",
							"bitbucketのダウンローダーはレスポンスに`Content-Disposition: attachment;`があるけど大丈夫だろ",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "138734edf8e7cbc14169022e95de481ef3251a6c",
						subject:
							"#367: バージョンチェック用XMLと更新履歴用XMLのURI変更",
						comments: [
							"数世代はcontent-type-text.net側も保守するつもり",
						],
					},
					{
						revision: "fe883254caa678c861f3444be15d405d514354b0",
						subject:
							"#395: ログウィンドウに個別の出力・コピー・削除機能を設ける",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "7e111254355b9c9c88a9bcd74a3d9bfb2d745cc5",
						subject:
							"#398: 自動アップデート時の最終調整スクリプトが実行されない",
						comments: [
							"いつからおかしくなっていたかは未調査だけどパス間違ってた",
						],
					},
					{
						revision: "d57c692a9c443570e941bfae5900c134ae1adb66",
						subject:
							"#401: クリップボードの取込対象・制限のON/OFFが効いていない",
						comments: [
							"1. 設定UIでの制御ができていなかった",
							"2. 設定補正時に強制ONになっていた",
						],
					},
					{
						revision: "05af853ddf4ad923f967d75f25857ff47cbf4028",
						subject:
							"#368: 環境によりツールバー設定の項目がはみ出る",
						comments: ["どの環境でもはみ出てた"],
					},
					{
						revision: "637e4bc45dec70801710630344d22161eaf320d7",
						subject:
							"#403: 情報ダイアログに旧フィードバックリンクが残ってる",
					},
					{
						revision: "a30390e605e378c4d9d1a1211d1b6ed0f5beaca8",
						subject:
							"#399: ネットワーク接続ができない状態でユーザー情報送信を許可した場合に落ちる",
						comments: [
							"アップデート確認と同じ処理方法だと思ってたら全然違ってた",
							"みんな大好き try ... catch(Exception) で対応",
						],
					},
					{
						revision: "3464fde3b34c700868429b87c06ad76c5000f3aa",
						subject:
							"#400: フィードバック入力ウィンドウをモードレスにする",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "de74c412a997761664c9b76cb5c113fea0e694a9",
						subject: "#335: .NET Frameworkのバージョンを4.6に変更",
						comments: [
							"やっとこさ nameof が使えるようになったので目についた範囲を修正",
							"実装の移行はのんびりやっていく",
						],
					},
					{
						subject: "#25: ヘルプファイルの記述",
						comments: ["次回バージョンで記載しますん"],
					},
				],
			},
		],
	},
	{
		date: "2015/11/30",
		version: "0.72.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"[事前通知]: #335: .NET Frameworkのバージョンを4.6に変更",
						comments: [
							"0.73.0 で .NET Framework の対応バージョンを を 4.5.1 から 4.6 に変更します",
							"nameof! nameof! nameof!",
							"#355の影響で0.72.0→0.73.0に先延ばし",
						],
					},
					{
						subject:
							"[プライバシー]: #179, #297実装で設定によりインターネット通信の発生する可能性があるためアップデート後に使用許諾が表示されます",
						comments: [
							"送信データを破棄したい場合は DATA-ID をお伝えください",
						],
					},
					{
						subject:
							"[悩み中] #381: 匿名で課題作成を行えるようにする",
						comments: [
							"課題への記入を匿名でも行えるようにするか悩み中です",
							"フィードバック機能も実装したし賛成・反対意見をもらえるとありがたいです",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "6f51246421c002bc4111f89be8a0e11acc9d0a7d",
						subject: "#378: ユーザー識別子を作成・保持する",
						comments: [
							"フィードバック等のユーザ情報収集時に使用されます",
							"設定画面の本体設定タブで再設定が可能です",
							"規定値ではユーザー環境のユーザー名・OS・CPU・メモリをコネコネしてMD5を算出します",
							"ランダム生成した時間は現在時間からMD5を算出します",
							"UI追加に伴いランチャーD&D設定はランチャータブへ移動しました",
						],
					},
					{
						revision: "e12fb70455ac9c9b6cd68bc4892c219b5f3782d2",
						subject: "#297: フィードバックをPe内で気楽に入力",
						comments: [
							"通知領域Peアイコンコンテキストメニュー 情報→フィードバックで入力できます",
							"入力データの送信にはインターネット接続が必要になります",
							"データに関しては追々ヘルプ書きます",
							"入力データを破棄したい場合は DATA-ID をお伝えください",
							"本対応によりレジストリ情報の一部に書き込みが行われます。Pe起動時に書き込まれ終了時に破棄されます。キーは下記になります",
							"HKEY_CURRENT_USERSoftwareMicrosoftInternet ExplorerMainFeatureControlFEATURE_BROWSER_EMULATION",
							"HKEY_CURRENT_USERSoftwareMicrosoftInternet ExplorerMainFeatureControlFEATURE_DOCUMENT_COMPATIBLE_MODE",
							"過去に記入して頂いたフィードバックは破棄します",
							"ていうか余裕なくてあんまり見れてませんでした。ごめんちゃい",
						],
					},
					{
						revision: "69dde0aac6b6c9337963bfce803becb7905d5575",
						subject: "#373: 初回起動時の情報を保持する",
						comments: [
							"本バージョンからの設定項目追加なので古いバージョン情報は持てません",
						],
					},
					{
						revision: "d92cc38c8e24c965659efc20b857b32195941107",
						subject:
							"#390: コマンドランチャーでディレクトリパスを入力した際に親ディレクトリも表示する",
					},
					{
						revision: "3db332ff1e05dc55cbe81a94b47fdedb50dd731f",
						subject:
							"#389: ランチャーアイテム選択リストのフィルタリング機能の一致方法を改善する",
						comments: [
							"コマンドランチャーとランチャーアイテム一覧で実装が分かれていたのを統合しました",
							"入力文字列の先頭1文字で検索方法が変わります",
							"大文字: 前方一致 + 大文字小文字を区別する",
							"小文字: 部分一致 + 大文字小文字を区別しない",
						],
					},
					{
						revision: "645424bc282f00ec958fe56b12011b1b528d81e0",
						subject: "#179: 使用ユーザー情報の収集",
						comments: [
							"#297での実装と環境を流用してユーザー情報を収集します",
							"今のところ実行タイミングは起動時・セッション開始時になります(自動アップデート確認と同じタイミング)",
							"設定→プライバシー から「ユーザー情報送信を許可」が有効になっている場合のみ送信処理が行われます",
							"送信内容はヘルプに記載する予定ですがまだヘルプが書けていません",
							"内容を確認したい場合はログを確認してください。要求・応答メッセージが出力されています",
							"送信データを破棄したい場合は DATA-ID をお伝えください",
							"本機能実装により本バージョンアップデートには使用許諾が表示されます",
						],
					},
					{
						revision: "8c4546823c1ea43d24e95df3991340af95246ad7",
						subject: "アイテム起動時のログ内容をまともにした",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "2ce7e8c9f6f7869980752736b0b9f6632b68b9b7",
						subject:
							"#384: 短い報告用情報のビルド種別の項目名をTypeからBuildTypeに変更する",
						comments: ["CLIもCLRに変更"],
					},
					{
						revision: "ec6efd3d9c19ed1619421b36181ecfc87d130e73",
						subject:
							"#372: 情報ダイアログは最前面表示にする必要ない",
					},
					{
						revision: "86031a0fbf868d2734ee229f5fa2c7399b38f4c0",
						subject:
							"#376: クリップボード・テンプレレートの転送にクリップボードを経由した場合にクリップボードオープンのエラーが発生する可能性あり",
						comments: [
							"本改修に伴いクリップボードが空であれば転送後も空にするように修正",
						],
					},
					{
						revision: "b4ef13c1afe0ec2e8f37bda937043db37a5d9b02",
						subject:
							"#377: テンプレートの置換処理(文字列orT4)でクリップボードを使用した場合にクリップボードオープンのエラーが発生する可能性あり",
						comments: [
							"本改修に伴いコピー操作の再試行を実装(全処理に影響)",
						],
					},
					{
						subject:
							"#394: システム環境情報取得時の取得エラーの例外キャッチをやめる",
					},
					{
						revision: "cbe3ae2273912e7337104e55cfa5b023ed517305",
						subject:
							"#385: コマンド型アイテムのツールバーメニューに作業ディレクトリがない",
					},
					{
						revision: "a6b6c4b54d6e8aae0736e7ff177f887e7d135333",
						subject:
							"#386: ランチャー登録時に新規作成したアイテムを選択状態にする",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "b1f2262d22cd9f350df4818832e06c173ceb578b",
						subject: "#355: 終了時にログを強制出力する",
						comments: [
							"うまくいかんし#355自体は対応やめます",
							"#393で頑張りますん",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/11/15",
		version: "0.71.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"[事前通知]: #335: .NET Frameworkのバージョンを4.6に変更",
						comments: [
							"0.73.0 で .NET Framework の対応バージョンを を 4.5.1 から 4.6 に変更します",
							"nameof! nameof! nameof!",
							"#355の影響で0.72.0→0.73.0に先延ばし",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "48e14eea7bd107e1ff299d29973074940bd3f4b6",
						subject:
							"#360: キャンセルボタンとESCキーをリンクさせる",
					},
					{
						revision: "43183959f9c4e3e127ed9272117770b0095e9091",
						subject: "#303: 設定ファイル更新を頻繁に行わない",
						comments: [
							"ノート・テンプレート・クリップボードの一覧データ保存時に一定時間待機するように改修しました",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "54f075926e318317e9b4617c485bd63f137e77fd",
						subject: "0.70.0修正内容の各Revision記載漏れ",
					},
					{
						revision: "771b804cc396463d68c84945d5007ba4213a7b82",
						subject: "#355: 終了時にログを強制出力する",
						comments: [
							"0.70.0での実装は色々残念だった",
							">ログ保存位置を指定していない場合(標準動作)は %APPDATA%logs に強制出力ログファイル(session-ending.log)が作成されていました",
							">>%APPDATA%直下に汎用的な名前でディレクトリを作っちゃったのでPe側では削除できません",
							">>>気になるのであれば削除しといてください",
							"まぁそもそも出力失敗してるから0byteファイルなんですけどね！",
						],
					},
					{
						revision: "041d0f3e46a5ee30e925035925c2598b0a6d14ea",
						subject:
							"#320: クリップボードの一覧アイテムの横幅とリストの横幅を合わせる",
						comments: [
							"修正の簡易さからアイコンとタイムスタンプを左寄せにした",
							"ついでにテンプレートも同じスタイルに設定",
						],
					},
					{
						revision: "a2498f3b825a320d3aa53e7405675cd558323ab0",
						subject: "#352: アイコン+文字列のスタイル整理",
					},
					{
						revision: "c30b577b7ccd0d1974f0c63d190b48cca9a92d1e",
						subject:
							"#357: クリップボードの最上位移動アイテムの作成タイムスタンプを更新するのおかしいっすよね",
						comments: [
							"今までは作成日時を元に並び替えてたけどソート用の項目で並び替えるようにしたので作成日のタイムスタンプは保たれるようになりました",
						],
					},
					{
						revision: "3dfa48c8831e4f3434d89dde92dd09545041b038",
						subject:
							"#361: クリップボード重複判定で範囲指定した場合になんか変",
						comments: [
							"実装見ると変ではなかったけど直観的ではなかったので動作変更",
							"範囲指定した場合、今までは一番古いものを基準としたが本バージョンから新しいものを基準とするように変更",
							"でもまぁ#363に食われるだろうけど",
						],
					},
					{
						revision: "43183959f9c4e3e127ed9272117770b0095e9091",
						subject:
							"#363: クリップボード重複判定の初期値を全件対象にする",
						comments: [
							"本バージョンへアップデートした際に重複判定件数が 50(0.70.0以下の規定値) であれば -1(全件) に変換されます",
							"0.71.0から -1 が規定値になります",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "ad28f66eeea13df30319465b6dff233c36340067",
						subject:
							"アップデートコンソールの最後に少しだけ待ち時間(5秒)を設定",
						comments: [
							"有効になるのは次回アップデート時です",
							"これと言ってユーザー側に意味はありません",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/11/12",
		version: "0.70.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"#346: Forms版→WPF版用データコンバーターの廃止",
						comments: [
							"本バージョンを持ってForms版からのデータ引き継ぎサポートを終了します",
						],
					},
					{
						class: "compatibility",
						subject:
							"#104による0.39.0 未満のアップデートチェック用URI互換を破棄",
						comments: ["事前通知なしに消しても誰も困らんだろ……"],
					},
					{
						subject:
							"[事前通知]: 0.72.0 で .NET Framework の対応バージョンを を 4.5.1 から 4.6 に変更します",
						comments: ["nameof! nameof! nameof!"],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "2d414ef51fb09e3346d1f775da19a7f712bff648",
						subject: "#322: ノート本文の自動改行を設定可能にする",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"#349: HTMLクリップボード内のスクリプトエラーを無視する",
					},
					{
						revision: "0c18841b28e87d85a510c0501328ada6b74c31a0",
						subject: "#305: メモリ消費を抑える",
						comments: ["到達不能な破棄処理を有効にした"],
					},
					{
						revision: "82289f6d51a41a3746ef859e3464f6da347017b1",
						subject:
							"#348: 情報ダイアログの「短い情報」に不要な'_'が存在する",
					},
					{
						revision: "825eb2b7bd5229ed1758727c1b3031bd5ea5fbf0",
						subject:
							"#356: クリップボード取込失敗→再取込で失敗しなかった場合はエラーを表示しない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "0df522de1a43b6f562dfca085d629b7343810e57",
						subject: "#351: データ複製の事故防止",
						comments: [
							"基本ロジックと一部データには適用",
							"全データに設定するのは労力的にしんどいので追々適用していく",
						],
					},
					{
						revision: "cc4defd86c3103b3aea443cbaeb186d59c716776",
						subject: "IDE0001の抑制",
						comments: [
							"usingする名前空間をVS2013スタイルで留める",
							"親以降の名前空間が同じ名称結構多くて完全修飾の方が分かりやすいのですよ",
						],
					},
					{
						revision: "008f6c8ba23142fecd4830c635f67f837761bf57",
						subject:
							"#347: 一旦外していた使用許諾のユーザー操作再設定を復帰する",
					},
					{
						revision: "c16e57c6f551b4106ed59f2ca1dfe9cda6fb99d0",
						subject: "#354: ログ出力用ストリームをごにょごにょ",
					},
					{
						revision: "36cf532b7ad735efc7009bf777bc0239f21d80c7",
						subject: "#355: 終了時にログを強制出力する",
						comments: [
							"#353のため#354の下準備から#355まで実装",
							"一番親元の作業オブジェクトに追加したのでView側で固まってたら再調査が必要なので保留とする",
							"ステータスは課題を参照のこと",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/11/03",
		version: "0.69.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"[事前通知]: 0.70.0 で Forms 版データ引き継ぎ処理を廃止します",
						comments: [
							"実装は残しててもいいんだけど名前空間被っててコーディングしんどいのですよ",
						],
					},
					{
						subject:
							"[事前通知]: 0.72.0 で .NET Framework の対応バージョンを を 4.5.1 から 4.6 に変更します",
						comments: ["nameof! nameof! nameof!"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "cc1b9e1b348cbfd8f016b181a62da6de722f0e7e",
						subject: "#341: メインボタンのアイコンがなんかずれてる",
					},
					{
						subject: "#305: メモリ消費を抑える",
						comments: [
							"使いまわせる ViewModel は再生成を抑える",
							"意味あんのか知らんけど一部バインドを初回のみに変更",
						],
					},
					{
						revision: "a5c46b69fd1872f0c69325f2604c193f6d0ea86c",
						subject:
							"#342: クリップボードやテンプレートの項目を一定数選択していくと古い選択アイテムが表示されなくなる",
						comments: ["#311と#305で死んでしもうてた"],
					},
					{
						revision: "87b43b7580856087ea3dfb25a73bfe2331e678db",
						subject:
							"#298: メニュー・ラベルにショートカットキーを表示する",
						comments: ["WPF版作成時に未実装だった"],
					},
					{
						revision: "59b025d28ae3026b9eb8a944b01c2f4ee273a79b",
						subject:
							"#343: ランチャー自動登録ボタンのアイコンが環境に表示できない",
						comments: ["旗マークも単色なんでふちどりしておいた"],
					},
					{
						revision: "b20ae7d9a4d6e759b264f5355cd1cf45c2e3b2e1",
						subject: "#345: グループ名変更UIが邪魔",
						comments: [
							"左側に出すように変更",
							"OSの利き手設定によって表示方向が右だったりするけど気にしない",
						],
					},
					{
						revision: "81ff02dcbe710f93077f3e09901d2ee787643d18",
						subject:
							"#344: ホットキーコントロールをWindows提供(HOTKEY_CLASS)の挙動に合わせる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "c2b66c7720199c68b4bb9538203534cd51763f19",
						subject: "#340: masterからdevelopmentマージはFFする",
					},
				],
			},
		],
	},
	{
		date: "2015/10/18",
		version: "0.68.1",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"[事前通知]: 0.70.0 で Forms 版データ引き継ぎ処理を廃止します",
						comments: [
							"実装は残しててもいいんだけど名前空間被っててコーディングしんどいのですよ",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "1b17cbf88727190b88d7d0dd45868c1651fc2039",
						subject:
							"#289: ランチャーアイテムを設定画面へ遷移せずに削除する",
						comments: [
							"削除用ボタンのUI実装によりノート側の削除ボタンも変更",
						],
					},
					{
						subject: "#305: メモリ消費を抑える",
						comments: [
							"焼け石に水かもだけど ViewModel を破棄した時に Model の参照を外す",
							"ツールバーのGUI構築方法を改善",
						],
					},
					{
						revision: "220788077df6490e37381ace0db75aecf537882a",
						subject: "0.68.0が動かない",
						comments: ["依存プロパティ実装修正の確認漏れ"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "42390940f9be507dba8087a5645d34500109f3e5",
						subject:
							"#337: コマンド入力後に再度コマンド入力すると前回入力値が残っている",
					},
					{
						revision: "c5171876096b8c063d0a0fb4df4e8173a2bd0089",
						subject:
							"ツールバーの設定「メニューボタンを調整する」は有効を規定値にした",
						comments: [
							"旧バージョンからのバージョンアップには影響しません",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "0a9554ea3fd284d790ec6f0877efc54ef7b430b0",
						subject: "#334: 開発環境をVS2013からVS2015に変更",
					},
					{
						revision: "f142bc9b0cc232c77d3f0b41ee9f15df84713473",
						subject:
							"#274: 各ソースファイルにライセンス情報を記載する",
					},
					{
						revision: "f142bc9b0cc232c77d3f0b41ee9f15df84713473",
						subject: "#336: コーディング規約変更: TAB -> SPACE",
					},
				],
			},
		],
	},
	{
		date: "2015/10/12",
		version: "0.67.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "出張おわったー、ちまちま修正できるー",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "3cf6990dd4c4ebdda3cd73a0beefc469f5dd924e",
						subject:
							"#329: クリップボード設定「重複アイテムをリストの最上部に移動する」が保存されない",
					},
					{
						revision: "5a07c456577c1eadb2a50c87c51ec70f51602ece",
						subject:
							"設定ウィンドウのツールバー項目において「表示時間」とそれ以降の項目が重なっていた",
					},
					{
						revision: "aeedf9d364628f7a0c7af9377aff6e03ee9c8391",
						subject:
							"#330: リスト最上部へ移動したクリップボードアイテムを選択状態にする",
					},
					{
						revision: "30dcf8f94ed52a6aa802ff1925e20e324239c8bd",
						subject:
							"#332: ホットキーからノートの前面移動を行っても前面移動しない",
						comments: ["出来たりできなかったり。。"],
					},
					{
						revision: "39c8f9cdea51d327c9a6aea405264d3f73b4a6c4",
						subject:
							"#331: 自動的に隠す状態のツールバーが表示された際にアクティブウィンドウ云々",
						comments: ["たぶんなおった、もう勘弁してください"],
					},
					{
						revision: "53998305f8990f54f00ad054016af46f78e76e94",
						subject:
							"#321: クリップボードの取り込み処理で失敗すれば再試行する",
						comments: ["☆ 突 貫 工 事 ☆"],
					},
					{
						revision: "bdf5bfe81e2d912e077ba72db00789e348ce75d6",
						subject:
							"#312: ツールバー設定画面のランチャーアイテムが選択されている状態でスクロールバーがスクロールできない",
					},
					{
						revision: "f6ca1c3a1345c6d895300ccee5823c6fa4bee05e",
						subject: "#305: メモリ消費を抑える",
						comments: ["継続課題のため終了はしない"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "1ece2f0d33addf2ffd2cce1e16581733322563f6",
						subject: "ログの保持上限数を持たせた",
					},
					{
						revision: "9a1153b4dd0a5beab92c295e2c213859ce78974a",
						subject:
							"#325: 可能な限り標準提供されているConverterを使用する",
						comments: [
							"BooleanToVisibilityConverter くらいしかなかった",
							"今後見つけ次第修正していく",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/09/28",
		version: "0.66.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "出張が終わらない。おうち帰らせて",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "230f76f39afee3fa64c673af2df71390834d269d",
						subject:
							"#327: リサイズ可能なウィンドウでデザインに問題なければリサイズグリップをつける",
					},
					{
						revision: "2d9bec4652ef1457356d1bd95c2095f9c982c5c2",
						subject:
							"#319: 重複したクリップボードをリストの上位に移動させる",
						comments: [
							"0.65.0以下からアップデートした場合、本機能は規定値(有効)に設定されます",
						],
					},
					{
						revision: "8e97ff6e76e8326e721dbf5045b2d22a94f97c4c",
						subject:
							"#314: 各ウィンドウのタイトルバーにそれっぽい値を設定する",
						comments: ["ユーザー視点的に何の影響もない"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "9b1fec690e71e599aa505495d5a6ecf59fe6caef",
						subject:
							"#315: 隠しファイルを表示するホットキー通知のタイトルがローカライズされていない",
					},
					{
						revision: "b481cf35c57dc0494e13944e7abe8be9d9bb1657",
						subject:
							"#307: ログ追加時にログウィンドウが表示された場合に項目が選択されていない",
					},
					{
						revision: "20af45367aabb78aa58dfa0ee41385707636d307",
						subject:
							"#326: 起動時にクリップボード取込処理を実施する",
					},
					{
						revision: "14612accee7d5d837eb32e5a5d70fec6ff389b52",
						subject:
							"#302: 各アイテムの更新日時等をきちんと更新する",
						comments: [
							"見える範囲で実装",
							"正直ハンドリングしてない部分までは無理",
						],
					},
					{
						revision: "6fdafabb76e13fdd4f57ab7c5feff5744e7bec0c",
						subject:
							"#311: インデックスデータ統括クラスのデータ破棄処理を政治家的に有耶無耶にしたい",
						comments: [
							"ヘッダ部とデータ部で管理されているノート・クリップボード・テンプレートのメモリ管理方法が改善されました",
							"特にクリップボードの画像データによるメモリ圧迫が改善された気がします",
						],
					},
					{
						revision: "710cc8fc398faf32245604488ce8d60897ebba63",
						subject:
							"#316: 自動的に隠す状態のツールバーが表示されたときにアクティブウィンドウがツールバーになる",
						comments: ["わっけわかんねぇわ"],
					},
					{
						revision: "d22cdf872ce1686e2f28c485d234a1b571dc6692",
						subject:
							"テンプレートアイテムの置き換え方法変更時にリスト表示部分が追従していないかった",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "487fe490847d6c987ad9e5eaf4e82348a569243b",
						subject:
							"#309: ログデータ保持に生データを持たないようにする",
					},
				],
			},
		],
	},
	{
		date: "2015/09/19",
		version: "0.65.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "b561d8baa4e4433b021f9e90b2c12bd7d319b19a",
						subject:
							"#287: ツールバーアイコンの開始位置を変更可能にする",
						comments: [
							"フロート状態以外で最上位(上 or 左, デフォルト)・中央・最下位(下 or 右)にツールバーのボタンを寄せます",
						],
					},
					{
						revision: "51e07c2071b7f6cd841aab500be6a4ceb0856c9a",
						subject:
							"#290: ツールバーのアイコン上にファイルをD&Dした際の挙動を変更可能にする",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "cd73f95fbea85bb3feb8b7759bdedf4c8ff7b3ea",
						subject: "#294: アップデート確認用文言が重複してる",
					},
					{
						revision: "91539fe4e31c3b453e0f11e6ac84464d811dfe59",
						subject:
							"#306: テンプレートウィンドウ表示切替時にアクティブ化されない",
						comments: [
							"初回表示時の対応をクリップボード, ノート, コマンドウィンドウにも適用",
						],
					},
					{
						revision: "a1ed556bb986f9a720d1203fee5876a8ea3490cc",
						subject:
							"コマンドウィンドウのリスト項目描画方法を他のリストに合わせた",
					},
					{
						revision: "c19fca46eb915a09ea17d35638686eca40393860",
						subject:
							"ツールバーへファイルD&Dを行い、指定して実行ウィンドウを表示すると前面に表示されない不具合の修正",
					},
					{
						revision: "c6b8719644cb947355bca808dfd75e348ffc15f0",
						subject:
							"#310: 自動的に隠す状態のツールバーを表示した際にZ位置が下位に存在する",
					},
					{
						revision: "ef80cfa3e9677d507e6dca32e4fdcc4d75dc9506",
						subject:
							"#301: 自動的に隠す状態のツールバーがシステム的に復帰したとき描画されていない",
					},
					{
						revision: "363bd30fcdf946bb2cb1748476bc984dbdfad37d",
						subject:
							"#308: 設定ダイアログのランチャー項目にファイルのD&Dでアイテム登録できない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "fa987ecba4c62c0fe4c35b06be72adb1c2bfcbaf",
						subject: "ソース管理を git 1 から git 2 に変更",
					},
					{
						revision: "70a855acb0d0a980b0506a4ccff1028f3c42ca05",
						subject: "#295: 未補足の例外を受け取る",
					},
					{
						revision: "d9f9204da73a4bb6244cefc93d641ef129647f51",
						subject:
							"#304: beta版実行用バッチファイルがWPF版の設定データ構成に未対応",
					},
				],
			},
		],
	},
	{
		date: "2015/09/14",
		version: "0.64.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"簡単だけど放置するのもなんだかなぁ課題を早めに解消",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "d6fca24face6fd17b58fa3ca145d958953cbc283",
						subject: "#291: 各ノートの設定が反映されない",
						comments: [
							"データ補正時に固定・最小化を無効にしてた",
							"多分初期化でやりたかった内容が補正側に入ってた",
						],
					},
					{
						revision: "23ccf7333b12b872df02a94f9ad5712bd188dbb5",
						subject:
							"#292: アップデート更新内容表示ウィンドウのデザインが適当",
						comments: [
							"XAMLだけ修正したので次回更新内容表示時に反映されてるはず",
						],
					},
					{
						revision: "fa0e1bbfeca222aa184fde19d6709ec452797fff",
						subject: "#293: 個人設定テーマ変更時に落ちる",
					},
				],
			},
		],
	},
	{
		date: "2015/09/13",
		version: "0.63.0",
		group: "WPF",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "本バージョンからWPF版になります",
						comments: [
							"基本的な機能はForms版の踏襲ですがあくまで似ているだけです",
							"今後の機能追加・保守はWPF版のみになります",
							"本バージョンはあくまでForms→WPFへの移植で溜まっていた課題への対応は次回バージョンから頑張る",
							"実装期間長かったー！",
							"出張先からのリリースなのでWPF版でのアップデート試験してないけどいけるさ、大丈夫さ、気にするな",
						],
					},
					{
						class: "compatibility",
						subject:
							"Forms版とWPF版の設定データに互換性はありませんが一部設定のみ引き継がれます",
						comments: [
							"!注意! バグバグしてそうな本バージョンへのアップデートを見送るユーザーもいそうなので、下記引き継ぎ機能は未来バージョン数世代はサポートします",
							"引き継ぎ処理はWPF版本体設定が存在せずForms版本体設定が存在する場合に実施されます",
							"引き継がれる設定: 基本設定、ランチャーアイテム、グループ",
							"引き継がれない設定: 各ノートデータ、各クリップボードデータ、各テンプレートデータ",
							"ランチャーアイテム互換性: ディレクトリアイテムはファイルアイテムに変換されます(内部実装として地味に予約しているので要望があれば検討します)",
							"ランチャーアイテム互換性: 組み込みアイテムは引き継ぎ対象外になります(将来的にはまたサポートしますが今は休止)",
						],
					},
					{
						subject: "ツールバー",
						comments: [
							"Aero Glass を使用しなくなりました",
							"「自動的に隠す」設定時に隠れる際のアニメーションを廃止しました(実装時に設計をミスった)",
							"ランチャーアイテムのメニューからファイル一覧メニューがなくなりました",
							"ランチャーアイテムのメニューからアイテム編集が行えるようになりました",
							"コマンドアイテムもファイルアイテムのように実行できるようにしました",
							"ALTキー押下によるアイテム並び替え機能が廃止されました(現実装だとちっと難しそうなので後回し)",
							"ESCキー二回押下でツールバーを隠す機能は一旦廃止しました(実装忘れてた)",
						],
					},
					{
						subject: "ノート",
						comments: [
							"各種設定編集をメニューからでなく一元的に操作できるようになりました",
							"通知領域からの一覧表示メニューが表示中・非表示に分離されました",
						],
					},
					{
						subject: "コマンド",
						comments: [
							"URL入力機能を廃止しました(実装忘れてた)",
							"候補一覧にアイコンを表示しました",
						],
					},
					{
						subject: "クリップボード",
						comments: [
							"データ保持方法を変更。Forms版で大きな画像ばかり取り込んだ際に落ちる不具合が解消されたと思います。思うだけで試してません",
							"クリップボードの各種データ形式に対して取込制限設定を追加しました",
							"取込種類・保存種類を統合しました",
						],
					},
					{
						subject: "テンプレート",
						comments: ["クリップボードウィンドウから独立しました"],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject:
							"ウィンドウ位置保存機能に最大化・最小化をサポート",
					},
					{
						subject:
							"ウィンドウ位置保存設定をUI上から変更できるようにしました",
					},
					{
						subject:
							"ツールバーのメニューボタン位置を特定条件で変更する機能の追加",
						comments: [
							"ツールバーを右側表示した時にメニューボタンを左に表示します",
						],
					},
					{
						subject:
							"ツールバーの自動的に隠すまでの時間をUI上から変更できるようにしました",
					},
					{
						subject: "使用許諾再表示バッチ機能の一時廃止",
					},
					{
						subject: "あとなんか色々",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"Forms版のソリューションは [Pe]/Pe-Forms に配置されます",
					},
					{
						subject:
							"以下IssuesはForms版で上がっていてWPF版から解決されたであろう課題。詳しく見てないけどなんとなく解決できたんじゃねってやつです",
					},
					{
						subject:
							"#245: テンプレートって「システム環境」と違う気がする",
					},
					{
						subject:
							"#243: ツールバーの自動的に隠す状態への遷移時間",
					},
					{
						subject: "#248: 高DPI環境での表示不具合",
					},
					{
						subject: "#275: クリップボード取込時にサイズ制限を行う",
					},
					{
						subject: "#286: Aero AutoColorを適用させる",
					},
					{
						subject: "#137: 大きなファイル読み込みで死ぬ",
					},
					{
						subject: "#210: クリップボードアイテム名を変更する",
					},
				],
			},
		],
	},
	{
		date: "2015/08/30",
		version: "0.62.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"WPF版の開発に注力していたのでForms版ひさびさのリリースです",
					},
					{
						subject:
							"本バージョンを持ってForms版としては最終リリースになります",
						comments: [
							"今後はWPF版としてリリースされる予定です",
							"未決定ですがWPF版とForms版での設定データに互換性はありません",
							">変換処理実装に割く時間が無いかもなのです",
							">>互換性を持たせるための処理・検証よりWPF版リリースを優先してそこから発生した不具合の修正を優先したいのです",
							"よっぽどおかしな処理があればForms版でも修正入れますが、ほぼほぼ無いです",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "ショートカット処理でリソースリーク",
					},
					{
						subject:
							"コマンド入力時のTAB, Shift + TABでの次候補選択順序が逆",
					},
					{
						subject:
							"#285: クリップボード/テンプレートのアイテムリストの件数が更新されない",
					},
				],
			},
		],
	},
	{
		date: "2015/05/31",
		version: "0.61.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"#282実装によりランチャーアイテム起動時の作業ディレクトリが変更されました",
						comments: [
							"作業ディレクトリが設定されている場合の挙動は0.60.0以前と変わりありません",
							"作業ディレクトリが設定されていない場合、実行パスの親ディレクトリが作業ディレクトリとして使用されます",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "2b8f3d0a2fd249f00ddcf38e193025d3bcf10be9",
						subject: "#276: コマンド入力の補完を行う",
						comments: [
							"入力中に[TAB]キーを押下することにより補完を行います",
							"ノリと勢いだけで実装したので細かい挙動は気にしないでいただきたいなぁと思ってる、と書いとけばいいって予防線の張り方",
						],
					},
					{
						revision: "08d4b77a59e81f3fe2687351e05418f334033c65",
						subject:
							"#281: データ保存を任意タイミングで行う'タスクトレイコンテキストメニューの拡張メニュー表示(Shift + 右クリック)で項目が表示されます",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "bceb10e9d6ba6462082a245989a9b2f515bdc427",
						subject: "#272: 設定項目にある「ディスプレイ」",
					},
					{
						revision: "1324613256ac33438437366fdf3ad81ad3018c53",
						subject: "#279: 起動時に例外",
					},
					{
						revision: "859c4366ebcec32053c4cbb704d7bafa726d6224",
						subject:
							'#277: コマンド入力でファイルパスの場合に""を入力した場合、それが最終ファイルだと警告が出力される',
					},
					{
						revision: "37acab0a0780d719087b21d8ac62b2edef9c49b0",
						subject:
							"#280: コマンド入力で開けないファイルを実行した際に例外が発生する",
					},
					{
						revision: "8e251f966f002d38d3436adcdcbca42c18474b02",
						subject:
							"#282: ランチャーアイテム起動時に作業ディレクトリが指定されていない場合、起動パスの親ディレクトリとする",
					},
					{
						revision: "a2b672e40ae768d7890d6f52715d964377a6f85e",
						subject: "#283: Peがフルスクリーンになるのを邪魔する",
						comments: [
							"ゲームしようとフルスクリーンにするとツールバー状態のPeがそれを解除しようとしてゲーム進行が止まるのですよ",
							"Crysis2の時は大丈夫だったんだけど昨日買ったCrysis3だと止まるんだよ！ だれだよこんなソース書いたやつは！",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/05/24",
		version: "0.60.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "コマンド型ランチャー機能を実装しました",
						comments: [
							"一応メニューから表示できますが実装としてはホットキーから表示する思想です",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "bbe615b060f72f00de83788fc2282bca98b726cc",
						subject:
							"#244: 長らくほったらかしのコマンド型ランチャー作ろうべさ",
						comments: [
							"過去バージョンから設定を引き継ぐ場合、タグ・ファイル検索機能は無効になってます",
							">設定補正だるかったし否定形の設定項目作るのに気が引けたのよ",
							"アイコン設定は実装してるけど下記事情によりリスト上のアイコン描画処理は将来実装",
							">>描画そのものは出来るんすよ",
							">>>出来るけど ComboBox の TextBox 部分のサイズ(高さ)がかなり残念なことになる",
							">>>>色々やってはみたけどこれだけのためにリリース伸ばすのもかったるかった",
							"はよWPFに移りたい、しんどい",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "6715bc48465c7df23851ac911275391b8678e761",
						subject: "#232: 標準入出力で出力系の改行を待たない",
						comments: [
							"それっぽくは動くけど勇んで走り出したTaskの行方は誰も知らない",
							"正直なところ白旗",
						],
					},
					{
						revision: "ff9fd7f27ddaefd8e3f8b98bb9c8777739d3a334",
						subject:
							"#265: ノート一覧のプレビューをもうちっとうまいこと表示する",
					},
					{
						revision: "4ad88a0b04ad08f9f468a3515c577c4230b1c064",
						subject:
							"ツールバーのツールチップ描画処理をちょっと改善",
					},
					{
						revision: "953c589f3348053f37c819c28d42a10623c0ae9f",
						subject:
							"アイコンパスがファイルとして存在するが無効パスと判定された場合に動作が不安定になる不具合の修正",
					},
					{
						revision: "096c4d0d2d399d2e384b3febef41664320e0d86d",
						subject: "#271: 設定ダイアログ保存時に例外発生",
						comments: [
							"設定ダイアログ保存時にタスクトレイ右クリック連打したら再現できた",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/05/12",
		version: "0.59.3",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"0.59.0, 0.59.1, 0.59.2 を潜り抜けた #239 が生んだ奇跡の#270",
						comments: [
							"同じようなのが次発覚しても0.60.0と統合する",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "fb37c851c9a83b053f71391027b9888abc8f6048",
						subject:
							"#270: クリップボード履歴のアイテム名が保存されない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"developmentブランチ作らんと急なリリースしんどいなぁ",
					},
				],
			},
		],
	},
	{
		date: "2015/05/12",
		version: "0.59.2",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "0.59.0, 0.59.1 を経てなお #239 が死んでた",
						comments: ["ごめりんこ☆（ゝω・）vｷｬﾋﾟ"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "5c609c1ebf0473cf194349f7c70b5f971889a050",
						subject: "#269: テキストテンプレート名が保存されない",
					},
				],
			},
		],
	},
	{
		date: "2015/05/11",
		version: "0.59.1",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "666ba18350db2b4f3cba71c96e24e1dbb6fe0e47",
						subject: "ごっめん！ 0.59.0 で #239 死んでた！！",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "#268: ランチャーアイテム保存できない",
					},
				],
			},
		],
	},
	{
		date: "2015/05/10",
		version: "0.59.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "#239が超心配！",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "5fc6d90edeaf354cdbf1df0cf5f2148b6090294a",
						subject:
							"#264: クリップボードのファイル一覧で選択ファイルにコンテキストメニューをつける",
					},
					{
						revision: "184425d4782c094f88123867a72e2b7acd023db9",
						subject:
							"ツールバーのメインボタンに表示するツールバー位置選択項目を視覚的にした",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "d39fcc84fc87d9607737668520476b34a907e860",
						subject:
							"#266: ノートの選択したフォントが反映されない場合がある",
						comments: ["ちゃんとできてるのかちと不安"],
					},
					{
						revision: "7e0e3e51da5d34c4875ac304ca69723f770e7ee3",
						subject:
							"#251: イメージアイテムチェック時にAccessViolationException",
						comments: [
							"あっかん、再現できん",
							"例外ガン無視する",
							"Exception 捕まえずに try { ... } catch(AccessViolationException) { ... } で自重した私をほめてください",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "a4233e81ee1e413f596ec24b6016678129a0f779",
						subject:
							"#239: 設定ウィンドウ構築処理が初期実装継ぎ接ぎで開発側泣きそう",
					},
				],
			},
		],
	},
	{
		date: "2015/05/05",
		version: "0.58.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "10c9f48b0860aa8df504800534065fb8c116e7ad",
						subject:
							"#256: クリップボード・テンプレートのテキスト転送方法を常時切り替え可能にする",
					},
					{
						revision: "71c380a0ea3b5abbec19015af67598858b688c70",
						subject:
							"#236: クリップボード/テンプレートウィンドウの分割領域を保持する",
					},
					{
						revision: "ebe3c60521acee7d864fd8345bcc8070bf51ea8c",
						subject:
							"#255: ノート一覧から該当ノートアイテムをプレビュー",
					},
					{
						revision: "cab4083d7fd19413655d9f5e8743e77bd8e48286",
						subject: "#238: T4エラー時に行番号も出力する",
					},
					{
						revision: "8dcab3775ddaf286e7653004c7ef4bacc5161b46",
						subject:
							"#263: クリップボード重複判定でファイルの場合はソートする",
						comments: [
							"さすがに過去分までは補正しないので本バージョンから取り込んだものが対象となります",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "813d9ca878215c9c3c422cd86b6cabf0229150f5",
						subject:
							"クリップボード/テンプレートの保存・削除ボタンをアイコンのみに変更",
					},
					{
						revision: "58f43289bb7c9a96844e317c1c64c97b47ad1679",
						subject:
							"#257: ファイルアイテムに上位ディレクトリが存在しない場合にもファイルメニューを表示させる",
					},
					{
						revision: "80598bf089add78653fb6489ec0edaa4297a88e3",
						subject:
							"#261: 起動時に出力されるログがUIスレッドに影響する",
					},
					{
						revision: "333dab558438827c361fc3ce7e3451667b677416",
						subject:
							"#260: ノートのタイトル入力後、前回の入力内容が微妙に残って汚い",
						comments: [
							"あっれぇ再現しないぞぉ",
							"でも実装したから完璧っすよ",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/04/12",
		version: "0.57.0",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "77aaf9dc0e54c9198d66651e9af52871dd035b9c",
						subject:
							"#254: テキストテンプレート置き換えプレビューがRTFじゃない",
					},
					{
						revision: "0c8bb809d3d6c810417278d4c7d3782b197ca7c6",
						subject:
							"置き換え処理を行わないテンプレートアイテムが選択状態でテンプレートウィンドウ初回表示時に不要なリスト部分が表示されていた",
					},
					{
						revision: "40541a787b9500f4a15a86168c67d5e9d980693d",
						subject: "#253: ノートの本文が保存されないことがある",
					},
					{
						revision: "e48147467c97155015f64733953635dd99102cc5",
						subject:
							"ツールバーのツールチップ表示時にツールバーを全面に移動する",
					},
				],
			},
		],
	},
	{
		date: "2015/03/29",
		version: "0.56.0",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "275e831cc4243da25eef41f2f1739e02fbfd5f35",
						subject:
							"#249: 情報ウィンドウでスクロールバーが表示される",
					},
					{
						revision: "c7e85157d5250cbe5dcbd83213e3cab706265aa2",
						subject:
							"#252: ノートのタイトル入力時にフォーカスが外れる",
					},
					{
						revision: "40f47369124402ec7304bdbcd5b4b4f4aa76af71",
						subject: "#250: イメージアイテム削除時に例外",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"ツールバー「自動的に隠す」実行でシステム的に成功したか否かに関わらず隠すように変更",
						comments: [
							"Windows8で隠れないらしいので暫定的に対処",
							"再現環境がないので何とも言えない",
							"#182に干渉するけどまぁ、うん",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/03/07",
		version: "0.55.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "フィードバック用ページを作りました",
						comments: [
							"タスクトレイコンテキストメニュー → 情報 → フィードバック から遷移できます",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "ac5959d98883d26dad53ca20a942c3e0a3b99839",
						subject: "#168: DBの論理削除後始末",
					},
					{
						revision: "98e707adb5d3e12c9758e45b6e1c71911c703412",
						subject: "#169: DBのアナライズ",
						comments: [
							"一定タイミングで REINDEX, ANALYZEを指定なし実行",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "91a535b1b963dc92e4ec535f454700aada3988dd",
						subject:
							"#247: 画面解像度を二回以上切り替えるとツールバー・ノートの位置・サイズがおかしい",
						comments: [
							"ディスプレイ関係は切り替えやすいラップトップで作業してて分かったけど、tpscrex.exe(ThinkPadの解像度変更用ユーティリティ)を使用した場合にのみ強制的にリサイズされノートのサイズがおかしくなる",
							"ツールバーも同じようにサイズ・位置が強制的に変更されていたがデスクトップツールバーとしてPe側でさらに強制的にリサイズしていたので表面化しなかった模様",
							"なので発生し得る環境がかなり限定されるが修正箇所自体は全環境に恩恵があると思うのでマージした",
						],
					},
					{
						revision: "a0108537a5ec63f3a52eb9d1e5da24700ec33920",
						subject:
							"#246: クリップボード/テンプレートのアイテムリスト上コマンドボタンが消えない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "#242: 古いブランチいらねーんじゃねーの？",
						comments: [
							"タグは全履歴あるしrcブランチ全部消してもいいけど過去3世代くらい残しとく",
						],
					},
					{
						revision: "ff1b9793ff8d78bd188f57a58ce44c61d515075f",
						subject: "#240: フォーラムへの書き込み",
						comments: [
							"フォーラムへの書き込みはメールアドレスが必須になってくるのでGoogle フォームを用いた方式にした",
						],
					},
					{
						revision: "34e3066e6e913d5c42681b81c25191f2da1807cd",
						subject:
							"SQLite を 1.0.94.0 から 1.0.96.0 にバージョンアップ",
					},
					{
						revision: "756dc38de309638bac4c04755b4ea14e89981f9e",
						subject:
							"ショートカット登録処理でツールバーと設定画面の重複部分を統一",
					},
				],
			},
		],
	},
	{
		date: "2015/02/28",
		version: "0.54.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"使わない人は一生使わないであろう機能を頑張って実装",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "0d41712756053148048e0b0bfab350b44b180918",
						subject:
							"#222: クリップボード/テンプレートアイテム一覧にフィルタor検索機能追加",
						comments: [
							"フィルタリング機能を追加しました",
							"フィルタリング中はテンプレートアイテムの追加・移動が抑制されます",
						],
					},
					{
						revision: "89cde990d7b393ecdb2de95dd496a99c76818f50",
						subject: "#233: テキストテンプレート拡張",
						comments: [
							"置き換え処理が有効であればさらにT4テンプレートエンジンを使用した置き換えを行えます",
							"プログラム書ける人でかつ大規模なテンプレートを書かない人が対象です",
							"T4はMono.TextTemplatingを使用しているためMS製T4と動作が違うかもです",
							'暗黙的に <#@ template language="C#" hostspecific="true" culture="使用言語の言語コード" #> が先頭行に挿入されます',
							"Pe側で __host(内部使用), app(IReadOnlyDictionary<string,object>) を予約します。Peの提供するデータには app[string] でアクセスしてください",
							"将来的にはもうちっと頑張ろうと思いますがとりあえず#233実装はここまで",
						],
					},
					{
						revision: "a68f08edf4921eff8b339778ad30e19be0d11168",
						subject: "#235: β版をとりあえずすぐ試せるようにする",
						comments: [
							"<Pe>/bat/beta.bat を実行すると現行バージョンに影響することなくβバージョンを実行することができます",
							"beta.bat 実行時に デスクトップ/Pe-beta ディレクトリが存在しなければ現行バージョンの設定データ(デフォルトパス)を デスクトップ/Pe-beta ディレクトリにコピーします",
							"あくまでβバージョンとして動作させるための機能ですのでリリース版で実行する意味はありません",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "9b1d3dc746847260a64425985c4a038b7b199c8f",
						subject:
							"設定データのバックアップファイル拡張子が ..zip となっていた不具合の修正",
					},
				],
			},
		],
	},
	{
		date: "2015/02/21",
		version: "0.53.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"開発中のリリース構成アーカイブをCIに追加しました",
						comments: [
							"詳細はプロジェクトページを参照してください",
						],
					},
					{
						class: "compatibility",
						subject:
							"アップデートに限り標準入出力のフォント設定が本バージョン初回起動時にリセットされます",
						comments: [
							"標準入出力関係の設定データを内部的に独立させました",
							"元の設定項目が一つだけでロジックに影響せずUIだけが影響されるものであるため下位互換を維持するだけの価値がないのでバッサリ切りました",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "00618396318a5197cdd977dc1df08614b048076e",
						subject: "#219: 画像データのクリップボード重複判定",
					},
					{
						revision: "4ad62e273c4477e5d944c0032270e375b4f2be45",
						subject: "#228: 標準入出力画面に色を設定する",
					},
					{
						revision: "9619719ab7e2c2d70e3a7c11257c04e9b12cc711",
						subject:
							"#229: スピンコントロールにデフォルト値を示すようにする",
						comments: [
							"コンテキストメニューでデフォルト値に戻します",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "4983d06b4b23773fdc8b698a970f1db2f368e2d8",
						subject: "#230: ログウィンドウが地味に身長伸びてね？",
					},
					{
						revision: "0ae956498965c3e1e550a1690614af0f5a753d0a",
						subject:
							"#231: 言語ファイルに clipboard/wait/message が未定義",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "d8d9bfec547304962930674e67e035c56c004180",
						subject: "β版出力をCIに追加",
					},
				],
			},
		],
	},
	{
		date: "2015/02/18",
		version: "0.52.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"本バージョン(Pe 0.52.0)未満でのアップデート処理は詳しく調査すると笑顔で地雷原を走り回っている状態でした",
						comments: [
							"バージョンアップに失敗し、プログラムが強制終了した場合はPeを再起動してタスクトレイコンテキストメニュー → Pe情報 → アップデートを実行してみてくださいよ！",
							"どうしようもなくアップデートできない場合は https://bitbucket.org/sk_0520/pe/downloads からダウンロードしてください。。。Vectorは公開依頼してから公開までが遅いのですよ！",
							"もう大丈夫だ、大丈夫、これで落ちない、大丈夫大丈夫。大丈夫だから忘れよう",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "69800b89e653f7d52952c922175dff628b3bc5b2",
						subject:
							"#225: クリップボード/テンプレートのアイテムをクリップボード経由でテキスト転送した後クリップボードの履歴が取り込めない",
						comments: [
							"対応に伴い設定 → クリップボード/テンプレート → Pe操作猶予時間 を廃止しました",
						],
					},
					{
						revision: "f60c25a3be357ccb77773b50f0ee0dbe52f9da69",
						subject:
							"#223: 標準入出力をファイルに保存した時改行がLFになる",
					},
					{
						revision: "3085e5cf2f5377e9865b1f4bf9ed011ce18f13a9",
						subject:
							"#226: 標準入出力の出力クリア後に標準入力が行えない",
					},
					{
						revision: "c54593bf8850c44340097f7cabfd6f7355a47e9c",
						subject:
							"#224: タスクトレイコンテキストメニューのツールバーアイコンがWin7以下とWin8以上で意味合いが異なる",
						comments: ["Win8以上のアイコンに合わせる"],
					},
					{
						revision: "5b73080306b3b600efcafea820a31056ada21e6a",
						subject: "#227: アップデートチェック時に死ぬ、再び",
						comments: [
							"たまに報告いただいてたなーんもしてないのに落ちたってのは恐らくこれが原因かと思われます",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/02/17",
		version: "0.51.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"スリープだとかロックだとか休止状態だとかでアップデートチェック用のタイミングがボロボロだったのでログオンのみに限定しました",
					},
					{
						subject:
							"Pe 0.44.0-0.50.0でアップデートチェックからの自動アップデートで死ぬかもなのでご注意を",
						comments: [
							"本バージョンで対応したつもりですよ",
							"アップデート用スレッドとか無関係そうなツールバーのリソース処理とか色々あったぽいのですよ",
							"バージョンアップに失敗し、プログラムが強制終了した場合はPeを再起動して(Windowsセッション接続維持中に)タスクトレイコンテキストメニュー → Pe情報 → アップデートを実行してみてください",
							"どうしようもなくアップデートできない場合は https://bitbucket.org/sk_0520/pe/downloads からダウンロードしてください。。。Vectorは公開依頼してから公開までが遅いのです",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "60b5f56f14fe02c3574a1393aa87e765aeaf2258",
						subject: "#220: クリップボード重複判定に範囲を含める",
						comments: [
							"履歴数やその内容によって負荷が異なりますのでユーザー設定で対応してください",
							"Pe 0.50.0の挙動と同じにするには値を 1 に設定してください",
						],
					},
					{
						revision: "62eb5eede80f08c3ede1305fa70cceb17d883a4d",
						subject:
							"#221: クリップボード/テンプレートウィンドウからの選択データを前回フォーカスウィンドウに転送する",
					},
					{
						revision: "a2e6f85f50724a8df49acdf9d61bbd61d094bde7",
						subject: "#181: 標準入出力ウィンドウをもうちっとこう……",
					},
					{
						revision: "5b7e5f5a73e30c7d9fcf737aa223b69bfcd1e29f",
						subject:
							"ランチャーアイテム種類がコマンドの場合に種類がファイルの時と同じような動作を行う",
						comments: [
							"コマンドアイテムでも標準入出力を操作できるようになりました",
							"管理者として実行するにはきちんとファイルアイテムで登録してください",
							"指定コマンドが実行可能か、設定パラメーターが伝搬するかはコマンドに左右されるため注意して下さい",
							"コマンドアイテムはパラメーターが設定済みであることを期待します。その都度パラメーターを変更する用途にはファイルアイテムが適切です",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "e5cf1ba9246ae0051d0f6fc68069493b69361339",
						subject: "#101: DPIが開発環境と異なる場合に色々と残念",
						comments: [
							"とりあえず、とりあえず動くようにした",
							"本対応に伴い情報ウィンドウのレイアウトが変更となりました",
						],
					},
					{
						revision: "b95b2c0a88c2fa65a9e27280947a2861d746cd25",
						subject: "#218: アップデートチェック時に死ぬ",
						comments: [
							"セッションを引き金とするアップデートチェックをログオン時に限定",
							"ツールバー破棄後のウィンドウハンドルアクセスも同時に修正",
						],
					},
					{
						revision: "a4effdd845104b71dcc4b44377beafd6f0bda8c3",
						subject:
							"クリップボード/メニューウィンドウの切り替えメニューにチェックをつける",
					},
					{
						revision: "e3455b380051480a5e34dd5456c8ae5ec2c3b20f",
						subject:
							"クリップボード/テンプレートのアイテム一覧をマウスホイールでアイテムに紐付くコントロールが再描画されない不具合の修正",
						comments: ["これ前に対応した気がする"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "78e314c6cfd0c13c95e697ac39fd17df186dc7e1",
						subject:
							"別スレッドからのクリップボート・テンプレートのリスト変更を安全にする",
						comments: [
							"今のところ呼び出し自体はUIスレッドだから不具合にはなってないはず",
							"……はず",
						],
					},
				],
			},
		],
	},
	{
		date: "2015/02/15",
		version: "0.50.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"#216対応によりランチャーアイテム名の重複を許容しなくなりました。",
						comments: [
							"旧バージョンや手動設定でランチャーアイテム名を重複させた場合に動作が不安定になる可能性があります",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "0062f3516b199b5219ac5ff1f6aefc6e1c5ad49f",
						subject:
							"#217: 取得したクリップボードが直近のクリップボードアイテムと同じであれば履歴に追加しない",
						comments: [
							"画像の判定はちょっと厳しいので後回し",
							"画像は暇な時にビット深度が固定・変動するのか調べて実装する",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "9211586bc2b66d7a20ce61e3ca85dd00d86551a5",
						subject:
							"#215: ランチャーアイテム設定画面のコントロールのアイコンが笑ってる",
					},
					{
						revision: "1e4d5b862b9817d1caf528c7edef475546ac88bb",
						subject:
							"#214: 設定→クリップボード/テンプレートのタブインデックスが狂ってる",
					},
					{
						revision: "10af64c6dc453a942136a7da9997e8822d00f509",
						subject:
							"#216: ランチャーアイテム名が重複していても登録できる",
					},
					{
						revision: "7972ea041fbef24aff4cf090b204d829c9fafba5",
						subject:
							"ランチャーアイテム変更時にグループ内アイテムも追従させる",
					},
					{
						revision: "ca580dc9c26948838934067e5a049453fb84fe32",
						subject:
							"AppbarForm.Dispose中にInvalidOperationException",
					},
					{
						revision: "2c9c6d75b07090a3d32dee8491e1976acda09987",
						subject:
							"#213: 画像を含むクリップボードデータの取得に失敗する",
					},
				],
			},
		],
	},
	{
		date: "2015/02/13",
		version: "0.49.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "クリップボード周りが癌細胞化してる",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "166f3b7a796f7517b13525ddd1023c869a75ccba",
						subject: "#184: クリップボード履歴の保存",
						comments: [
							"圧縮してはいるものの、データよっては保存ファイルのサイズが大きくなるので保存機能はデフォルトでは無効になっています",
							"保存機能を有能しても保存種別が未チェックであればデータは保存されません",
							"保存種別(特に画像)によっては保存ファイルサイズが肥大化しますので注意してください",
						],
					},
					{
						revision: "8771dac472311aae4f1a1f2fee692b44205023ae",
						subject: "#209: 自動的に隠す状態でのD&D",
					},
					{
						revision: "e8a5c4791cc5e7100e44aba19e949f19998f5a9b",
						subject: "#206: クリップボードプレビュー画面の機能改善",
						comments: [
							"HTMLクリップボードはクリップボード取得時のURIの表示・コピーを追加しました",
							"画像は初期状態でウィンドウサイズに合わせて表示されます",
							"画像を原寸表示している場合に左クリックのD&Dでスクロールします",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "f7ee22c7b013c085540afe8abb54787da97b835b",
						subject:
							"#205: ツールバーのグループ選択コンテキストメニューに対するチェックマーク",
					},
					{
						revision: "e199016d9f61204f65accf0c10dc3530f535099e",
						class: "compatibility",
						subject: "#144: UpdateScriptをPe/etc/scriptに移動する",
						comments: [
							"多分大丈夫なんでPe 0.44.0からの下位互換維持を打ち切り",
						],
					},
					{
						revision: "3cc54f61292f9b88f4229c7cb9cefe6c370c30e7",
						subject:
							"#207: ツールバーメインボタンのホイールクリックでメニューが表示されない",
					},
					{
						revision: "7b1c94a748eeffaed50f84a703666482de8d6d8a",
						subject:
							"#208: クリップボードが空で保持クリップボードも空の場合にタブ移動で表明違反",
					},
					{
						revision: "a3e3842468a1e2e5e5f20d082c6c0f2be9640f85",
						subject: "#211: 使用許諾ウィンドウのボタンがなんか変",
					},
					{
						revision: "ce755ce057ebfbad3003f47a65d5c5dfd61d196c",
						subject:
							"使用許諾でキャンセルするとNullReferenceExceptionが投げられる嫌がらせみたいな不具合の修正",
					},
					{
						revision: "5029100f2ef16deeb7629ab897f4ee78c658060e",
						subject:
							"クリップボード/テンプレートアイテムのリストアイテム選択変更時におけるちらつきを抑制",
					},
					{
						revision: "f8e255c719d4638adbe3b53f8532dc7c3732cd99",
						subject:
							"#212: クリップボード/テンプレートウィンドウで画像を含むデータを個別で二回以上破棄すると例外",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "fcaddc12f59f9de232ea59e2dc4315ed11c57c35",
						subject: "#204: Control.Tagの絶滅",
						comments: [
							"ToolStripUtility.AttachmentOpeningMenuInScreen(Control)だけは諸事情により無理",
						],
					},
					{
						revision: "df2f5fa6f0eab035dfa15370193f5b5ab8204530",
						subject:
							"System.TimeSpanのシリアライズ・デシリアライズ統一",
					},
					{
						revision: "800662f938031704d7bbda79f75a77796c1bce50",
						subject: "スキン置き換え画像はPeMainで保持しない",
					},
				],
			},
		],
	},
	{
		date: "2015/02/11",
		version: "0.48.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "c2586268379c8eda8592699b18caa332afcd4952",
						subject:
							"#203: 設定画面のツールバーグループ設定を直観的にする",
						comments: [
							"グループノード選択時にランチャーアイテム一覧をダブルクリックすると選択中ランチャーアイテムが追加されます",
							"各ノードをD&Dで移動出来るようになりました",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "733fb123f24d29a6c2fa4a7763c0a4bce5c3991b",
						subject:
							"#159: イベントに割り当てたラムダ式のメモリ解放",
						comments: [
							"とりあえず見える範囲でキャプチャ切ったから勘弁して",
						],
					},
					{
						revision: "49286e2c2acd9587f56e5bdc02e41fee7da13e65",
						subject:
							"#202: グループ作成で名前に重複がある場合に例外が発生する",
					},
					{
						revision: "71940970976125a393c0a186e1cfcf462dd6a7c3",
						subject:
							"ファイルメニュー展開後に読み込み終了したアイコンを待機中イメージから置き換える",
						comments: [
							"全項目に適用するとアホみたいに遅くなるし現在表示項目数が取得できないので上位項目に適用する",
							"ファイル数が多いとクッソ怪しい動作",
							"system32なんて誰も表示しないだろうという一握の望み",
						],
					},
					{
						revision: "e24fa1d27cb81f2e7e39bb8920a2d51e421082b0",
						subject:
							"ログウィンドウで一部のログが表示できない不具合の修正",
					},
					{
						revision: "589affc8d5d144791fda6d4f80e3e0859a326617",
						subject:
							"大きなアイコン取得時にIImageListが生成できずInvalidCastExceptionがブン投げられる",
						comments: [
							"再現性皆無でデバッグ時しか発生を確認できていないのでとりあえず空catch",
						],
					},
					{
						revision: "0182cf25e210342ef2dbf72a80b275827a36678c",
						subject:
							"クリップボード/テンプレートのタブ一覧が設定ウィンドウ確定後一時的に無効になる不具合の修正",
					},
					{
						revision: "f95bb3b6afa6f859db0f0d254d64b1dfa2b0e2a8",
						subject:
							"タスクトレイコンテキストメニューに表示されるクリップボード/テキストテンプレート項目表示文字列をウィンドウ名に合わせる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "5fdaf73ea51d2a1e9b638833655f14a8a0f3eb04",
						subject:
							"ホームダイアログのボタンがなんかちっさくなってたんで考えることを放棄してDock.Fillした",
					},
				],
			},
		],
	},
	{
		date: "2015/02/08",
		version: "0.47.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "バグ修正！",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "5b5bc48e8ca1acfe43b61a05f48a747d58dc44e6",
						subject: "#99: オーバーフロー時のツールチップ表示",
					},
					{
						revision: "c98046857eae6e790a48ac2845f94711b8a4d8b7",
						subject:
							"オーバーフロー表示されたランチャーアイテムの描画をスーパークラスに委譲する",
					},
					{
						revision: "a10fef85a324e360368e513758e672ec5b5c5715",
						subject:
							"#182: ツールバーの自動的に隠す状態はタスクバーに干渉するべきでない",
					},
					{
						revision: "664ebe458e7a5ac24afdc4cb220a73e567881efc",
						subject:
							"#201: 自動的に隠す非フロート状態からフロート状態にすると表明に引っ掛かる",
					},
					{
						revision: "2819d8a092d3aedbc809ee623ca3111c6db1a4e1",
						subject: "#200: 指定して実行ウィンドウのアイコンが汚い",
					},
					{
						revision: "f0bc4c1e0bd489e86cf2156602a2ec6989260a35",
						subject: "複数のツールバーを表示での各不具合を修正",
						comments: [
							"ツールバーへファイルをD&Dで登録した際に対象ツールバー以外のグループがクリアされる",
							"ランチャーアイテムをD&Dして並べ替えた際に他のツールバーのアイテム順序が追従しない",
						],
					},
					{
						revision: "8cbd6f6c7cfeec80b19fe8a4846c9d725f2e05d2",
						subject:
							"ツールバーのランチャーアイテムをホイールクリックしてもランチャーアイテムメニューが表示できなくなっていた",
					},
				],
			},
		],
	},
	{
		date: "2015/02/06",
		version: "0.46.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"テンプレートの置き換え書式 VER-* が#198対応で書式変更となりました。下位互換は#199により保たれます",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "8419d4e468725416f96435de485b62f6d7900b58",
						subject:
							"#197: テンプレートにクリップボード置き換えを追加",
						comments: [
							"CLIP",
							"CLIP:NOBREAK",
							"CLIP:HEAD",
							"CLIP:TAIL",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "c7499cc0c72f442a61faee79447d3d346ffc38dd",
						subject:
							"#196: クリップボード/テンプレートウィンドウがリサイズできない",
					},
					{
						revision: "0e92137eaf4d0c9722584c5e712e3da3fab3d2d5",
						subject:
							"#195: 非インターネット接続環境だと毎回アップデートチェックに失敗するのにログが出てウザい",
					},
					{
						revision: "7aa5eb0dec14806a6a82d51ed83d9ea1b1d4287e",
						subject:
							"#198: テンプレートのバージョン書式を他の項目に合わせる",
						comments: [
							"VER-FULL -> VER",
							"VER-NUMBER -> VER:NUMBER",
							"VER-HASH -> VER:HASH",
						],
					},
					{
						revision: "1dea15676f0727295106dfa429ac8a6f8f4aed4d",
						subject: "#193: クリップボード取得が重い",
						comments: [
							"難しい、開発機だと再現できん。ロジックは少し変えたけど再発したらまた考える",
						],
					},
					{
						revision: "379ef83c9b645ce8e9e017f5a4d087fa3115d533",
						subject: "#171: SystemSkinのでっかいリソース",
						comments: [
							"PeMainに組み込んでいたSystemSkinをリソースと供にDLL化した",
						],
					},
					{
						revision: "db81154964d2105023190f87132b556ad43b3c67",
						subject:
							"設定ダイアログ内でノート各行の高さを本文に合わせる",
					},
					{
						revision: "2a81a822af45849a87fd1f0ea9aed78413d867c1",
						subject: "#191: タブインデックス整理",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "e389103cb2f69e10df08106585d103fe7756fdbc",
						subject:
							"標準CSSに開発時のゴミが混入していたので消しといた",
					},
				],
			},
		],
	},
	{
		date: "2015/02/04",
		version: "0.45.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"クリップボードウィンドウにテキストテンプレート(#154)機能が追加されました",
						comments: [
							"定型文をアイテムとして保存、それをクリップボードへコピーします",
							"置き換え処理を使用すれば実行時の年月日などを設定できます",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "35b28628c3c076867594504a5d50e2490be7ac6b",
						subject: "#154: 定型文のテンプレート",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "77a41f42b8272e1960557bd509ac123b14f97125",
						subject:
							"設定データのXML出力時に改行のCRLFがLFになっていた不具合の修正",
					},
					{
						subject:
							"#154対応により「クリップボード」ウィンドウの表示文言を「クリップボード/テンプレート」に変更",
					},
					{
						revision: "34f1db6c15679c757ae7b1206dd41cfc177fe726",
						subject:
							"クリップボードリストの最終アイテムを削除した際にArgumentOutOfRangeException初回例外が発生する",
						comments: [
							"触った感じデバッガ噛ませた場合だけだと信じてる",
						],
					},
					{
						revision: "9fe970b2f13eaf5c8bba43a02bd08556e0a2483d",
						subject:
							"#188: ツールバーへのD&Dでメインメニューへカーソルを合わせると落ちる",
					},
					{
						revision: "b8a17da5257ea78ea1921a5eb8aeb82cefc08437",
						subject:
							"ツールバーへのD&D時にカーソル下のランチャーアイテムが組み込み・ディレクトリの場合もD&D不可とする",
					},
					{
						revision: "0feb176f9729f2f0d74250f9930334aa30b8de40",
						subject:
							"ツールバーへのD&D終了後にメッセージボックスを表示した際、メッセージボックスが背面に表示されるUIを改善",
					},
					{
						revision: "3152fe5638ec901a11e05f052f57c4e4afe3aebd",
						subject: "#190: clean.bat消してない",
					},
					{
						revision: "040e6b47d01b61339c534d5d5a46249df9363dba",
						subject: "クリップボードウィンドウ周りの文言を変更",
					},
					{
						revision: "0b06e1f09a492ea92921ecf4d98418d76ddb0e0d",
						subject:
							"#185: ファイルメニューに表示するアイコンがシステムのものでスキン所属ではない",
					},
					{
						revision: "b2396cb9ff97a1c03b8acf980e73fb0f93cacb7d",
						subject: "#185対応により一部アイコン使用部分まで伝搬",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "9681d49628e58b861729ac73ae46e08c235296d8",
						subject: "COMオブジェクトをラップする",
					},
					{
						subject:
							"0.44.0の更新履歴#186にリビジョンが設定されていなかったので追加",
					},
					{
						revision: "c20c62c8192240e53348ba97be2c432b5932a248",
						subject: "#194: 非リリース版のリビジョンバージョン変更",
					},
					{
						revision: "28c638381428009492ee9cc5680378a2337b2861",
						subject: "あんまり関係ないどうでもいい変更",
						comments: [
							"sbin/Updaterが戻り値を返す",
							"CLIでのキー押せ催促を統一",
						],
					},
					{
						revision: "4a8935ba3110993533457cbac259820bb3387c97",
						subject:
							"#189: UpdaterScript.cs実行時にmscorlibを読み込むか",
						comments: ["大丈夫、いけるいける信じろって"],
					},
				],
			},
		],
	},
	{
		date: "2015/01/31",
		version: "0.44.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"大量のリソースリークが発生していたため一生懸命修正したのです",
						comments: ["(´◔౪◔) 反省してまーす"],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "da4da48ea40e16fbeb1a6321820025a352bc07bb",
						subject: "#75: アップデート確認を定期的に行う",
						comments: [
							"とりあえず現状では以下のタイミングで処理する",
							"起動",
							"設定保存",
							"ホームダイアログ終了",
							"セッション接続接続",
							"ロック解除",
							"システム再開",
						],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "447134f2e1da7933d3e286474705acfabaab012c",
						subject:
							"#180: メニューの罫線クリックでメニューが閉じる",
					},
					{
						revision: "40af874b72280096acc55f0dedf358e97021ac71",
						subject: "#144: UpdateScriptをPe/etc/scriptに移動する",
						comments: [
							"スクリプト自体は移動したが下位互換等のため/sbin/Updater, /etc/script/Updater を同時配布して整合性を保つ",
						],
					},
					{
						revision: "1aa8fa57442132abe9e08f5e6a4deaf3b967f54b",
						subject:
							"48px以上のアイコンを読み込み時にリソースを持つファイルでリソース境界範囲外にアクセスする不具合の修正",
					},
					{
						revision: "9f5a11833cbce15707872ed25975e49d65ae4ac0",
						subject: "#183: ファイルメニュー構築処理を速度改善する",
					},
					{
						revision: "a27f68b64d054e24d0a3ac3f165f6d5e04a0ad2b",
						subject: "#187: COMの参照が解放されない",
					},
					{
						revision: "fe0aebfc230e0d6459a715a117eadd782bf72638",
						subject: "#186: GDIオブジェクトが解放されない",
						comments: [
							"アイコン取得時に一回のアクセスで取得できない場合があるので数回アクセスするように変更",
							"通常アイコン取得時、API成功でもアイコンハンドルが取得できてない場合に後続処理を行わない",
						],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "0489176fc54f793dbdac09effb524d09d7dfb6f7",
						subject: "#172: changelog.xmlの補足事項",
						comments: ["構成自体をざっくり修正"],
					},
					{
						revision: "f403270f80000b5675dfad32222199b1d34104c2",
						subject: "IF適応の漏れを修正",
					},
					{
						revision: "3c264cd5b1b3c524dcebc3d09e85c041127c0727",
						subject: "DBManager担当処理を分割",
					},
				],
			},
		],
	},
	{
		date: "2015/01/25",
		version: "0.43.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"ファイル・ディレクトリアイテムのファイルメニューからディレクトリを開くには下位メニューから「ここを開く」を選択してください",
					},
					{
						subject:
							"ファイルアイテムのファイルメニュー第一階層目はパスメニューから代用できるため「ここを開く」はありません",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "c65b4ed19b2eacc8b93ae130ce779ad363b6d54d",
						subject:
							"#175: 設定画面のディスプレイ識別を視覚的に行う",
					},
					{
						revision: "97ea508e7ab1e34ec988da8b53ff73fe49338d5a",
						subject:
							"ファイルメニューを表示する際にシステムが隠しファイルを表示する設定であれば該当ファイルを半透明で表示する",
					},
					{
						revision: "2c9de6cfd57c2a85ff2535dcd539de1c6cd36890",
						subject:
							"SystemSkin: ツールバー文字列描画にシステムのタイトルバー描画処理を使用する",
					},
					{
						revision: "c63c8d0188451395a33d3ca3278b0efb8d5f061e",
						subject:
							"#173: クリップボードウィンドウ表示切り替え時にバルーンを表示する",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "2d432b805313cf5cd6938404c54e387cc4a7c59e",
						subject: "#174: ファイルメニューが表示されない",
					},
					{
						revision: "f35f8146e262a58783de927b801ca58e1d3aed79",
						subject:
							"#176: ノートの文字列がURLの場合にオートリンクされる",
					},
					{
						revision: "8f1e77dac716127e4362f8ff2464411f0e1943ce",
						subject: "#162: 自動アップデート失敗時にPeが復帰しない",
					},
					{
						revision: "fa03d1e087b4b2b8a04f7b645f1a80e6958830ff",
						subject:
							"#177: ノートを本文入力状態で最小化しても本文入力が可能",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "76d69b05c5208a53b2bf180ad166a0b8cf855f24",
						subject: "各種UIの共通処理をまとめる",
					},
					{
						subject:
							"0.42.1の更新履歴で「リリースビルドバッチ整理」が#160になっていたのを#165に修正",
					},
				],
			},
		],
	},
	{
		date: "2015/01/22",
		version: "0.42.1",
		contents: [
			{
				type: "features",
				logs: [
					{
						revision: "7a38fc15197655ea9a941744f4c0e24860ff5d28",
						subject:
							"#167: 標準入出力ダイアログのフォントを設定項目に表示する",
					},
					{
						revision: "0387abfea5c3af98acba2948271b523240a377c5",
						subject: "#158: 肥大化するarchiveディレクトリ",
					},
					{
						revision: "3092a80fd5f4f217d8b1133d444375607192b4b3",
						subject: "#170: accept.batを同期実行させない",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "bbb2f76b4ae4a92931906718cbde3b8acf2b31a1",
						subject:
							"#161: 固定したノートでマウスカーソルが点滅する",
						comments: ["詳細はIssuesを参照のこと"],
					},
					{
						revision: "853a055234c33476e8fd61d6db517c88c43695f6",
						subject: "#163: クリップボードのUnicodeな文字列",
					},
					{
						revision: "1544ed313cd5a936dc3af284bfb38c86c0c8a3ed",
						subject: "Hashのヘルプファイルをリポジトリ参照へ変更",
					},
					{
						revision: "ad40f830b93720415224e8c874bbceeb46f9bbd0",
						subject: "一部文言の修正",
					},
					{
						revision: "8967a05e4fc44ada7ba393d4e2e7b923ac693117",
						subject:
							"#68: UNICODEを含むショートカットファイルの読み込みに失敗",
					},
					{
						subject:
							"0.42.0のビルド変更により設定項目にデバッグ用UIが表示されていた",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "bbf8b9f7ef559eebd0236fcd4261fd1dd9a0c1d5",
						subject: "#165: リリースビルドバッチ整理",
					},
					{
						revision: "e3ec638ee1ca1f8aec93f5aa1eb2878106873185",
						subject: "#164: switchのDebug.Assert",
					},
					{
						revision: "34960ebefb5d0820a2d2f7f023bc5f0858773240",
						subject: "#166: readme.txtをMarkdownにする",
					},
					{
						revision: "11f6e6e7e3e9f55ec8f7c7729038bfee64490163",
						subject:
							"#160: リリースビルド時に変なdefineがある場合にエラーとする",
					},
				],
			},
		],
	},
	{
		date: "2015/01/20",
		version: "0.41.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"#55対応によりノートの本文入力方法が変わりました。Windowsの付箋に近くなった感じです",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "c523a87135a248d192384cfa5ab4bda299e86169",
						subject: "ノートのサイズ・位置変更中は透明にする",
					},
					{
						revision: "9bf0cd67718bec973bb1411ba3ad6845782d7a68",
						subject: "#150: システムスキンをきちんと環境に合わせる",
						comments: ["まぁアイコンだけ"],
					},
					{
						revision: "6694402d6870d38bbf6ee9465e0da97462836311",
						subject: "#153: ノート編集時の初期状態",
					},
					{
						revision: "799106280493668be468ae6120c171d85e9e2221",
						subject:
							"32px以上のアイコンの場合に通常ファイルアイコンにサムネイルを使用",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "f4e785b8e4dacf26bca7fc54f8e020e3853fadb4",
						subject: "#55: ノートの改行表示",
					},
					{
						revision: "4d8241d562904003be368d54d57fb2fe12aa2b0b",
						subject: "#155: デバッグログの条件式",
					},
					{
						revision: "3f944c4a02e3e24466dd05678c8fb0b9d6d31f1c",
						subject:
							"#156: 指定して実行ダイアログをタスクバーに表示する←？",
					},
					{
						revision: "de599fae36a8824e6d124200c2fb40352ddab396",
						subject:
							"標準入出力ウィンドウのツールバーボタンを非アクティブでも有効にする",
					},
					{
						revision: "799106280493668be468ae6120c171d85e9e2221",
						subject: "#2: アイコン取得",
					},
					{
						revision: "47c521ea8aae5df4313f0fccc98b3ae82d05240c",
						subject:
							"ホームダイアログで処理後に言語情報が吹っ飛ぶのを修正(多分開発ブランチでしか起こらないけど混入コミット探すのしんどい)",
					},
					{
						revision: "ea64a3a87d8fda05b0f0ed580e58ad9ae4b0c8f9",
						subject: "#157: 各ウィンドウ表示時に前面へ移動させる",
					},
					{
						revision: "e031432aaf5a4947e0f12c65c367780e58805655",
						subject:
							"アップデート処理で binPeUpdater.exe.config の削除処理が抜けていた",
					},
				],
			},
		],
	},
	{
		date: "2015/01/15",
		version: "0.40.1",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "指定して実行ダイアログをタスクバーに表示する",
					},
					{
						revision: "029c51b7fb9514b2d8594ca3a40a750f57eee0b1",
						subject: "#143: 各種イメージリソースをISkinで管理する",
					},
					{
						revision: "19bd29aefc541502c5f8d3756b70dfd4a0bf368e",
						subject: "#44: スキン切り替え",
						comments: ["実装のみで切り替えモジュールは含めてない"],
					},
					{
						revision: "dbedd8185a43d391e7ae241269563341887269dd",
						subject: "#148: ランチャーボタンのメニュー表示方法追加",
					},
					{
						revision: "3fa85c221ea546fab3a01e95d218362c96f13465",
						subject: "#145: ログにデバッグ出力",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"指定して実行ダイアログのオプションでディレクトリ選択ボタンがはるか彼方に消えていたのを修正",
					},
					{
						revision: "987a88a61201f9a6865c5a8378a6e8b41447b81a",
						subject:
							"ツールバーのツールチップがRDP等の非Aero環境でわけ分からん描画になっていた不具合の修正",
					},
					{
						revision: "534ac11a1e56f42099809120d688eb5090e0de51",
						subject:
							"#149: クリップボードプレビューでテキストフォントが変わる",
					},
					{
						revision: "c493dd1b0d6b4c3d423747cf188312a4c90e4413",
						subject:
							"#151: フォント設定UIで現在選択フォントを初期値とする",
					},
					{
						revision: "e730ed868ecc61b9e4d0c3079ab1be5136516cd7",
						subject:
							"#146: クリップボード切り替えホットキーの表示が言語ファイル通してない",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "36c19df96db15c6b1c082cd4270dd40cf8b5ae93",
						subject: "#147: NuGetパッケージをまとめる",
					},
					{
						revision: "113d6f59d0b1a1cd4d5b100043b4c4b389c9e0d7",
						subject:
							"準備だけして使いもしていなかったマウスフックを無効にした",
					},
				],
			},
		],
	},
	{
		date: "2015/01/11",
		version: "0.39.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"本バージョンからアップデートチェックに使用するアドレスが変更となります",
					},
					{
						subject:
							"XML -> http://content-type-text.net/document/software/pe-update/update.xml",
					},
					{
						subject: "詳細はオンラインヘルプを参照してください",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "7e5149cbb741c03214f9b5d1a95fd240921c0ba4",
						subject:
							"#104: アップデート定義ファイルをリポジトリから外す",
					},
					{
						revision: "9e6e24366567961f56a3e4de03ade9acfae8ee5c",
						subject: "#135: コマンド(URI)アイテムに対する引数",
					},
					{
						revision: "782040d7832c3bf0f3faf40d0c14dd0300e73e41",
						subject:
							"コマンドアイテム設定時、プルダウンに環境変数PATHの実行ファイルをリストアップする",
					},
					{
						revision: "e234d1c035cc4e9a421397e78c26f53b689eb44a",
						subject: "#134: アップデート実行時にスクリプト実行",
					},
					{
						revision: "62ba9830a6afdd7986bd6ffa5cf530721fb7d340",
						subject: "#139: クリップボードウィンドウのホットキー",
					},
					{
						revision: "452b69ff948c0a46297cc80a1e1d765fccd6137c",
						subject: "#140: 組み込みアイテムの一覧",
					},
					{
						revision: "db83e73bc16fd58a54137b0e9db8b3ebb3b563e5",
						subject: "#142: Hash機能強化",
					},
					{
						revision: "5fe681ad3f86409322635b13fcfd0d0e91f353f6",
						subject:
							"ファイルアイテムの親フォルダを開く際にファイルを選択した状態でエクスプローラを開く",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "7bd3b5202a7b842b0bcd357f345bd679454fa15a",
						subject: "#141: ホームダイアログの文言",
					},
					{
						revision: "27f1b25c3efba2b667c8a82a5b6abab2fc6bc740",
						subject: "#138: 組み込みアイテムの起動後後処理",
					},
					{
						revision: "e89286a60a585d79d907a054114066d91b636d1d",
						subject: "情報ダイアログのバックアップボタン削除",
					},
					{
						revision: "5fe681ad3f86409322635b13fcfd0d0e91f353f6",
						subject:
							"ファイルアイテムの作業フォルダに環境変数を含んでいる場合に、展開前パスがディレクトリパスとでない場合に正常にフォルダを開けない不具合の修正",
					},
					{
						revision: "13e7be2ba5ce325c942992d3f26d9cbbc1cd72b2",
						subject:
							"バックアップファイル世代対象を*.zipに限定し、バックアップ対象をディレクトリまで広げる",
					},
				],
			},
		],
	},
	{
		date: "2015/01/03",
		version: "0.38.1",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "d42011a11cf4e9eba9379e669b1506b2134d821d",
						class: "compatibility",
						subject: "#41: アイテムの種類",
						comments: [
							"種別「URI」を廃止して「コマンド」を追加しました。下位互換のためURIアイテムの読み込みはサポートされますがコマンドアイテムへ変換されます",
						],
					},
					{
						revision: "bf46dfbbd00221f810f2954bc2ba3b6e9f241404",
						subject:
							"組み込みアイテムを追加しましたが#118対応でのIF試験的意味合いが強いため該当プログラムの機能は弱いです",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "556e62fe00d948335c82f2b548648af6b17515e6",
						subject:
							"#130: クリップボード履歴ダブルクリック時に保持データをすべてコピーする",
					},
					{
						revision: "777bd5716c9b9c1a1f522ffe25f11acbac542e37",
						subject: "クリップボード監視の切り替え機能を追加",
					},
					{
						revision: "bf46dfbbd00221f810f2954bc2ba3b6e9f241404",
						subject: "#41: アイテムの種類",
						comments: ["組み込みアイテムの追加"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"0.38.0で何をどう頑張っても落ちる不具合の修正。緊急のため0.38.0と0.38.1は統合",
					},
					{
						revision: "29ff41ed68c8f966c784fec5d05b1fae50d38bba",
						subject:
							"#131: HTMLクリップボードの読み取り元が日本語を含む場合に範囲計算が狂う",
					},
					{
						revision: "c5b893be60bd911c67992af4037bd57d62bdec4c",
						subject:
							"#129: HTMLクリップボードデータをファイル保存時にクリップボードデータとして保存している",
					},
					{
						revision: "3beedc59b897a80cbf5bcba4a66ffd592e349c6e",
						subject: "#132: RTFの書式が吹っ飛ぶ",
					},
					{
						revision: "5cf1a43a9d5b503ee2448997d59263a59d16f465",
						subject:
							"クリップボード履歴一覧からカーソルが外れた際にボタン一覧を非表示にする",
					},
					{
						revision: "645ee77f26749c255b64af6ddb62c9af6770d943",
						subject: "#133: PrintScreenでクリップボードに入んねー",
					},
					{
						revision: "ed6b3b7cb7cb6333071383ed6646e087a40971d7",
						subject:
							"クリップボード履歴一覧の描画がホイールスクロールで変になる",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "2f513e7e823cd0d1399306dff08045bb357ca43d",
						subject: "#124: GUIコンポーネントのソースをまとめる",
					},
					{
						revision: "bf46dfbbd00221f810f2954bc2ba3b6e9f241404",
						subject: "#118: 自前プログラム呼び出し方法",
					},
				],
			},
		],
	},
	{
		date: "2014/12/23",
		version: "0.37.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"0.36.0でクリップボード処理がバグりまくっていたので修正しました",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "9bc1cb51706097f0c4aeb7ad35468c91aadedb27",
						subject: "#128: クリップポ－ドの待機時間を延ばす",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "7e3d641206d9d681a1d19f614799bbe8f3a50472",
						subject: "#127: HTML形式クリップボードデータ保持",
					},
					{
						revision: "f1b45e68725c536ebcc9ac7b9c41982b244e7ef7",
						subject: "#126: クリップボード履歴の項目ボタンが不思議",
					},
					{
						revision: "14eb786e20344ae907656f79f177c3f409139730",
						subject:
							"#125: ファイルをクリップボードへ取り込んだ後ファイル削除→プレビュー表示で例外",
					},
					{
						subject:
							"クリップボードアイテムがファイルの場合にファイルが存在しない場合はコピー対象としない",
					},
				],
			},
		],
	},
	{
		date: "2014/12/21",
		version: "0.36.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "0b63f714daec6126e46322bc94b692a6bad6071c",
						class: "compatibility",
						subject: "#65: 下位互換@IconItem",
						comments: [
							"Pe 0.29.0 からの下位互換をサポートしなくなりました",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "469eadb5d031563d83fac904cadeee2092fc5b51",
						subject: "#115: ノートのタイトル入力を直観的に",
					},
					{
						revision: "97945be931fb097ecdc831580cbbff57fd389a4d",
						subject:
							"#113: 指定して実行ダイアログをモードレスにする",
					},
					{
						revision: "955cefea4f81c0c8127be3dde423ad900ba92e01",
						subject:
							"#120: ノートの削除を非拡張コンテキストメニューでも表示する",
					},
					{
						revision: "1bbe16ee46986aba7663e66e1e59ead95196081a",
						subject: "#119: クリップボード監視",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "bbb2492c77690d64e1b7be07f38ee9ae4ad05213",
						subject: "#116: #106のバッチがBOM",
					},
					{
						revision: "31b395cf4005b2a317b9db0bacce8a7eddc21f65",
						subject:
							"#121: DwmGetColorizationColor() が大きめの値を返すと SetVisualStyle() で System.OverflowException",
					},
					{
						revision: "d7ecb37158ce17dd9829155708752bd28d99d4fc",
						subject:
							"#122: ログダイアログの表示位置とサイズが保存されてない",
					},
					{
						revision: "1cfae55dae07910d523ebb35510f325a0cc99ccd",
						subject: "#117: ツールバーのツールチップを消す",
					},
					{
						revision: "97945be931fb097ecdc831580cbbff57fd389a4d",
						subject:
							"#114: ウィンドウを親依存でなく独立して保持する",
						comments: ["#113も解決"],
					},
				],
			},
		],
	},
	{
		date: "2014/12/06",
		version: "0.35.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						revision: "7e2cd76929891a874fb3494899d7b251d3de232c",
						class: "compatibility",
						subject:
							"#54に関連して各種パスを変更しました。ユーザー操作に影響する部分として [Pe]/bin/PeUpdater.exe を [Pe]/sbin/Updater/Updater.exe に変更したためファイアウォール、アンチウイルスソフト等の設定変更が必要な可能性があります",
					},
					{
						subject:
							"各種パス変更に伴い過去バージョンの不要ファイルが含まれます。削除するには [Pe]/bat/clean.bat を実行してください",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "d85cf3d12e420d65babf6e64599eaab83b60ed48",
						subject: "#79: メニューからのツールバー位置を視覚化",
					},
					{
						revision: "7a6f206fef9bf358c01307d2069f3b82bd3523a6",
						subject: "#108: ツールバーをユーザー操作で強制的に隠す",
					},
					{
						revision: "2361a267964b772aa117c2c46745cd3c140746ea",
						subject: "#110: 言語ファイルのデフォルト",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "93ac22d2e9902221af0cdd36c61b576a5e5e9209",
						subject: "#56: マルチディスプレイ環境の切り替え",
						comments: ["ディスプレイ位置変更時に一応追従"],
					},
					{
						revision: "17785cfd712d9036a15b6a340c820e674f743b65",
						subject: "#106: #102対応のバッチファイル含み漏れ",
					},
					{
						revision: "bb3438add614a2876a141344a2e271b78cb7958f",
						subject: "#105: 設定→ツールバーの項目順",
					},
					{
						revision: "c138646841450de048868d33085f2864a79dee5f",
						subject:
							"#111: タスクトレイコンテキストメニューが自動的に隠すツールバーに連動して閉じる",
					},
					{
						revision: "c3ce164c8d39266aa1bb574741333f6252fe2d49",
						subject:
							"自動的に隠すツールバーが隠れたときに前回フォアグラウンドウィンドウをフォアグラウンドウィンドウに設定",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "ff86166b83fe66b94389599100633dd1f0aa9647",
						subject: "#109: build.shの文字コード",
					},
					{
						revision: "7e2cd76929891a874fb3494899d7b251d3de232c",
						subject:
							"#54: 名前空間と各名称がプログラム名(Pe)と直結してる",
					},
					{
						revision: "33c9b96cd5465fb4ba9a656f09aa4ad5412a1110",
						subject:
							"#54によりアップデート後のアセンブリ解決のため PInvoke.dll から PlatformInvoke.dll に名称変更",
					},
					{
						revision: "cac67b69ba9b8d082ec6cddfc43f9166400d1f35",
						subject: "各種アセンブリのAssemblyCopyrightを設定",
					},
					{
						subject: "バージョン 0.33.0 での開発環境変更を追記",
					},
				],
			},
		],
	},
	{
		date: "2014/11/30",
		version: "0.34.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"#26対応によりバージョン表記を a.b.c.d から a.b.c.d-xxxx... に変更しました。ユーザー操作に影響はありませんが報告用情報の内容が変更されます",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						revision: "57ce440b369426c073965b71c49a2141398ffe53",
						subject:
							"#100: 情報ダイアログからコピーする報告用情報に罫線",
					},
					{
						revision: "5a519d4305a269af7bdf505c2ca4e1834e0b9972",
						subject: "#97: コンポーネント情報整理",
					},
					{
						revision: "bcb2155a0c7905a65b3d4e33756c43a0743bb631",
						subject:
							"#102: 使用許諾ダイアログをユーザー意志で再表示",
					},
					{
						revision: "8c9b33f16ba69bad9036abbd934d3191e035ae65",
						subject: "#26: git commit hash",
					},
					{
						revision: "05fb65035835bc4fad48885c3fd3ca0f8109712c",
						subject: "#63: 自動アップデート時に優しく殺す",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						revision: "0d8c9432db5b921669eb6a0343f1ff8cb2460c9b",
						subject:
							"#98: 「現在選択中グループ」のツールチップ文字列",
					},
					{
						revision: "022e666df7daddb560dc3319bce175c798ddfb0c",
						subject:
							"#103: 更新履歴のリビジョンを行末に。ついでにスタイルシートちょこっと設定",
					},
					{
						revision: "7f5a04985721d44741030dd699fdca68749c3265",
						subject:
							"ログ表示処理が非リリース構成で例外になる不具合の修正",
					},
					{
						revision: "2a01315de61388e0b99bb26f01ba4c9bcfb826e6",
						subject:
							"設定→ランチャ→その他の入力項目をウィンドウサイズ可変に対応できていなった不具合の修正",
					},
					{
						revision: "8c9b33f16ba69bad9036abbd934d3191e035ae65",
						subject:
							"#26によりバージョン情報ダイアログの表示項目順、バージョン情報・構成情報を入れ替え",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "4f56e98cbfa2680ffb7ed2b7ebcbca8af6b3557f",
						subject: "#87: DBManager使用時のスパゲッティ具合",
					},
					{
						revision: "5c6d563da2cb6a30ce3cadbc9176628e881b0f12",
						subject: "#64: app.config の切り替え",
					},
					{
						revision: "82d8307979942b749d0e3607464c5d6a1aee5c8f",
						subject: "PeMain以外の各種アセンブリバージョン修正",
					},
				],
			},
		],
	},
	{
		date: "2014/11/24",
		version: "0.33.0",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						revision: "65c905ae5b74109082263b1972aead5d7a6cda30",
						subject:
							"#92: 環境変数で指定したファイル アイテムのプロパティが表示されない",
					},
					{
						revision: "2d6a501969e86fb4776b31777b61731cf725b713",
						subject:
							"#93: ファイルアイテムが環境変数を含む場合にファイルメニューが非活性",
					},
					{
						subject:
							"#92, #93に関連して環境変数を含む親ディレクトリ、作業ディレクトリのパスコピー・表示の不具合修正",
					},
					{
						revision: "20f4d6b558d59e70b8aa8f635364d8b5fa003406",
						subject: "#91: ツールチップがメニューを覆う",
						comments: ["#78を含む"],
					},
					{
						revision: "5f532dd9c09c3d01d0af55c81712e1e2ce029371",
						subject:
							"#62: メニューに表示するホットキーが頭おかしい",
					},
					{
						revision: "888da4e58a8ad763ba8fd73a642e812b0fb31c41",
						subject:
							"#95: よろしくないホットキーのメニューショットカット割り当てで例外",
					},
					{
						revision: "9cc9b57861d742d72e77ee8a8989d386732578f4",
						subject:
							"#96: 認証が必要なネットワークでの更新履歴取得失敗",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						revision: "288f2be6be13360028f79515eb571ed5e6e33b36",
						subject: "#69: ユニットテスト書こうぜ！",
					},
					{
						revision: "a70ce895c81d7261a93e3345ea0299a3121ce737",
						subject: "#89: ソース整理",
					},
					{
						revision: "ef51dcde150cff0a4c3e7b781c43f31312f999a9",
						subject: "#94: 変更履歴にコミットのリビジョンを含める",
					},
					{
						subject:
							"開発環境を SharpDevelop 5.0 から Microsoft Visual Studio Community 2013 に変更",
					},
				],
			},
		],
	},
	{
		date: "2014/11/19",
		version: "0.32.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "#82: ポーズ時のタスクトレイアイコン",
					},
					{
						subject:
							"#74: ランチャーアイテムをツールバー上で移動させる",
					},
					{
						subject:
							"#84: ツールバーメインボタンでグループ切り替え",
					},
					{
						subject: "#85: ショートカットファイルの登録処理",
					},
					{
						subject: "#41: アイテムの種類",
						comments: ["URI追加"],
					},
					{
						subject: "#86: 使用者の環境情報を定型として出力",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "#80: toolbar/main/tips の ${version-release}",
					},
					{
						subject:
							"#77: ShellFolder アイテムのドロップダウン表示で Unhandled exception",
						comments: ["#41により対応不要"],
					},
					{
						subject: "#83: バッチ ファイルにパラメタが渡らない",
					},
					{
						subject:
							"ツールバーのボタンサイズに左側余白を若干追加しました",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "Visual Studio Community使いたいでありんす",
					},
				],
			},
		],
	},
	{
		date: "2014/11/13",
		version: "0.31.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"グループ名の重複を許容しなくなりました。旧バージョンや手動設定でグループ名を重複させた場合に動作が不安定になる可能性があります",
					},
					{
						subject:
							"設定ファイルのバックアップアーカイブから戻しを行う場合はグループ名の重複に注意してください",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject:
							"#71: 環境情報出力時にディスプレイ情報を出力する",
					},
					{
						subject:
							"#72: ディレクトリのD&D登録時にアイテム種別の選択",
					},
					{
						subject: "#73: ツールバーに対する初期グループの設定",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"#70: ディレクトリアイテムで環境変数が展開されない",
					},
					{
						subject:
							"設定ダイアログのランチャーアイコン設定処理のインデックス関連を改善",
					},
					{
						subject:
							"#73の影響によりグループ名の重複時に連番を自動採番するように変更",
					},
					{
						subject: "#73の影響によりグループ名編集時に trim",
					},
					{
						subject:
							"#77: ShellFolder アイテムのドロップダウン表示で Unhandled exception",
						comments: ["暫定対応により例外握り潰し"],
					},
				],
			},
		],
	},
	{
		date: "2014/11/09",
		version: "0.30.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "#67: ランチャアイテムの自動登録",
						comments: [
							"基盤処理実装、細かい修正や defagroupt-launcher.xml の定義が必要",
						],
					},
					{
						subject:
							"設定ダイアログで新規グループ作成時の初期グループ名に連番を設定する",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "#66: UNC 環境での SQLite オープン",
					},
					{
						subject: "ランチャー種別切り替え時の挙動を修正",
					},
					{
						subject:
							"マルチディスプレイ環境でホームダイアログが非プライマリディスプレイに表示されることがあったためプライマリディスプレイに固定表示するように変更",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "すっかり忘れていた Hotkey Control",
						comments: [
							"SpnotetButton をコンポーネント情報に追加と名前空間整理",
						],
					},
				],
			},
		],
	},
	{
		date: "2014/11/05",
		version: "0.29.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						class: "compatibility",
						subject:
							"将来的な拡張に備えられるよう launcher-items.xml が変更されます。IconPath",
						comments: [
							"IconIndex 要素は IconItem 要素の子として Path, Index 要素に置き換わります。古い各要素は手動設定を考慮して互換性のため保持されますが将来バージョンでは排除されます",
						],
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject: "アップデート処理前に設定データを保存",
					},
					{
						subject: "#61: ${env}を編集する",
					},
					{
						subject:
							"リリースビルドのバッチを修正。x86版で[Pe]/x64, x64版で[Pe]/x86を除外した",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "ランチャーアイテムのアイコンデータ整理",
					},
					{
						subject:
							"使用許諾、アップデートチェック画面のリンク選択時にIEでなくシステムの標準のブラウザでリンクを開くように変更",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"開発環境を SharpDevelop 4.4 から SharpDevelop 5.0 に変更",
					},
				],
			},
		],
	},
	{
		date: "2014/11/01",
		version: "0.28.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject:
							"システム環境のアイコンを Windows ちっくに置き換え",
					},
					{
						subject:
							"タスクトレイコンテキストのノートに現在有効なノート一覧を表示する",
					},
					{
						subject:
							"ノートのコンテキストメニューに拡張メニュー実装",
					},
					{
						subject:
							"ノートのコンテキストメニュー項目、最小化にアイコン設定",
					},
					{
						subject:
							"画面上のウィンドウ表示位置を保存・復帰させる機能の追加",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"タスクトレイ Pe アイコンダブルクリック時の処理がデバッグコードのままだった",
					},
					{
						subject:
							"タスクトレイ Pe アイコンコンテキストメニューのツールバーアイコンを変更",
					},
					{
						subject: "#59: TimeSpanがシリアライズされない",
					},
					{
						subject: "システムイベントのメモリリークを修正",
					},
					{
						subject: "#58: メニューに表示するホットキーが近い",
					},
					{
						subject: "#17: Aero描画の切り替え",
					},
					{
						subject: "ツールバーの文字列幅を制限",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"changelog.xml の 0.27.0 公開日が10/25になっていたので10/26に直した",
					},
					{
						subject:
							"[PE]/etc/style, [PE]/etc/script を追加、それに伴い関連部分を色々と変更",
					},
				],
			},
		],
	},
	{
		date: "2014/10/26",
		version: "0.27.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "ツールバーメインメニューに非表示を追加",
					},
					{
						subject:
							"#8, #34とかの成果として、タスクトレイのコンテキストメニューを .NET Framework の推奨である ContextMenuStrip に変更(Forms非推奨？ 聞こえんなぁ)",
					},
					{
						subject:
							"ContextMenuStrip への変更にあたりアイコンを設定",
					},
					{
						subject:
							"ノートのカスタムカラーアイコンを非選択時は固定のアイコンを表示するように変更",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "ログフォームのタイトルがアホになっていた",
					},
					{
						subject:
							"#57: ツールバーのコンテキストメニューでカーソルが移動用",
					},
					{
						subject:
							"通常スキンでノートのキャプションボタンを密着するよう変更",
					},
					{
						subject:
							"Windowsからのユーザー切り替え時に表示UIの再描画処理を見直し",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"SystemEvents.UserPreferenceChanged イベントを受信",
					},
					{
						subject: "SystemEvents.SessionSwitch イベントを受信",
					},
				],
			},
		],
	},
	{
		date: "2014/10/25",
		version: "0.26.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"バージョン 0.23.0, 0.24.0, 0.25.0 の 64bit版を使用している場合、アップデートは下記URLから手動で行ってください",
					},
					{
						subject:
							"https://bitbucket.org/sk_0520/pe/downloads/Pe_0-26-0_x64.zip",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"64bit版 PeUpdater が 32bit で生成されていたため旧プロセスを殺せなかった",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "PeUodaterプロジェクト設定見直し",
					},
					{
						subject: "0.1.0 - 0.9.0 までのタグを削除",
					},
				],
			},
		],
	},
	{
		date: "2014/10/25",
		version: "0.25.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject:
							"ノートの有無によってタスクトレイPeコンテキストメニュー内容の有効・無効切り替え",
					},
					{
						subject:
							"ツールバーのグリップ部、ノートのキャプションにカーソルを持って行ったときに移動を示すカーソルに変更",
					},
					{
						subject:
							"ノートのコンテキストメニューにアイコンべたべたはっつけてみた",
						comments: ["最小化のアイコンは未定"],
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "前面表示処理が死んでた",
					},
					{
						subject:
							"ツールバー最小化時に設定状態に関係なく最前面に表示するよう変更",
					},
					{
						subject:
							"ドッキング状態により自動的に隠すメニューの有効無効を切り替える",
					},
					{
						subject:
							"ツールバー位置のメニュー項目のチェックは丸で表示する",
					},
				],
			},
		],
	},
	{
		date: "2014/10/21",
		version: "0.24.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "ノートの現在選択色を親メニューにも表示",
					},
					{
						subject:
							"システムレジューム時にアップデートチェック実施",
					},
					{
						subject: "#20関連として入力処理をサポート",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "ノートの前面表示が常時最前面表示となっていた",
					},
					{
						subject: "#38: 多言語によるUIの自動調整",
						comments: ["設定/本体 気が付けば終わってた"],
					},
					{
						subject:
							"使用許諾ダイアログ、ホームダイアログ、アップデートダイアログの前面移動を実装",
					},
					{
						subject:
							"初回起動時のホームダイアログがウィンドウプロシージャを経由していない不具合を力技修正()",
					},
					{
						subject:
							"#20: 準出力取得時に取得ウィンドウを閉じると例外",
						comments: ["閉じないようにした"],
					},
					{
						subject:
							"標準出力ダイアログのタブに当たる言語設定が古かった",
					},
					{
						subject:
							"標準出力ダイアログをツールダイアログから通常ウィンドウへ変更",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "#53: デフォルト引数なくしたい",
						comments: ["ﾋｬｯﾎｰｲ!!"],
					},
				],
			},
		],
	},
	{
		date: "2014/10/18",
		version: "0.23.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"フォーラム(https://groups.google.com/d/forum/pe_development)作成",
					},
					{
						subject:
							"PeUpdate.exe のパスが [Pe]/PeUpdate.exe から [Pe]/bin/ 以下へ移動しました。ファイアウォール、アンチウイルスソフト等の設定変更が必要な可能性があります",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject: "スタートアップへの登録機能を設定画面に追加",
					},
					{
						subject:
							"ホームダイアログの実装。アイテム検索機能は煮詰まるまで無効化",
					},
					{
						subject: "情報ダイアログのリンクにグループを追加",
					},
					{
						subject:
							"タスクトレイアイコンのコンテキストメニューにヘルプ追加",
					},
					{
						subject: "#6: 言語設定",
					},
					{
						subject: "ノートを一括で前面表示する機能追加",
					},
					{
						subject:
							"ホットキー操作で行われる処理内容をバルーン表示する",
					},
					{
						subject: "#56: マルチディスプレイ環境の切り替え",
						comments: [
							"位置は知らんけどディスプレイ数を検知するよう修正",
						],
					},
					{
						subject: "#51: タスクトレイダブルクリック機能実装",
					},
					{
						subject:
							"初回起動時(使用許諾ダイアログとは別ロジック)にホームダイアログを表示",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"情報ダイアログのリンク押下時に訪問済みに色変更されていなかった",
					},
					{
						subject:
							"アイコン表示ダイアログのアイコンインデックスを指定出来ていない不具合",
					},
					{
						subject:
							"操作性が悪かったためノートの色選択機能をプルダウンからサブメニューへ変更",
					},
					{
						subject:
							"使用許諾ダイアログ、アップデート実行ダイアログの誤操作を防ぐため Enter キー押下によるダイアログ標準動作を抑制",
					},
					{
						subject:
							"アップデートダイアログのコントロール類を広げた",
					},
					{
						subject:
							"初回起動時にフロートだとどこにあるのか分からんということでツールバーの初期状態をデスクトップの右側に変更",
					},
					{
						subject:
							"ツールバーが自動的に隠す状態で表示する際にタスクバー位置を考慮していなかった",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "app.config の key 変更",
					},
					{
						subject:
							"言語設定 [note/style/color] を [note/menu/color] に変更",
					},
					{
						subject: "使用許諾の各URIを app.config で置き換える",
					},
				],
			},
		],
	},
	{
		date: "2014/10/13",
		version: "0.22.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject:
							"ノートの前景色・背景色をノートコンテキストメニューから変更可能に",
					},
					{
						subject:
							"全ウィンドウ非表示状態で非表示ウィンドウから表示された場合に復帰させないようにした",
					},
					{
						subject: "ノートの内容を出力(入力は未実装)",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"ノート最小化時の描画処理で本文が描画されないように修正",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "デバッグ時のデバッグ設定見直し",
					},
				],
			},
		],
	},
	{
		date: "2014/10/05",
		version: "0.21.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "ObjectDumper使用",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject:
							"アップデート処理後に異常処理がなければコンソール画面を閉じる",
					},
					{
						subject: "情報ダイアログに更新履歴表示ボタン追加",
					},
					{
						subject:
							"情報ダイアログから手動アップデートチェックを行う際に確認ダイアログを表示",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"アップデート確認ダイアログに最新バージョンが表示されない不具合",
					},
					{
						subject: "#41: アイテムの種類",
						comments: [
							"ファイルとディレクトリで分岐させる。その他は未実装",
						],
					},
					{
						subject:
							"ファイルランチャーメニューのファイルで列挙されたディレクトリを選択した際にディレクトリが存在しなければ例外",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "紆余曲折あった更新履歴の原本はXMLで統一",
					},
					{
						subject:
							"SQLiteのx86/x64切り替え処理が自動化されたのかなんか知らんけど両方のDLLが含まれるようになってデブくなった",
					},
				],
			},
		],
	},
	{
		date: "2014/10/03",
		version: "0.20.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"#49未対応版が本バージョンを正常に落とせるように一時的にRC版をアップデート確認リソースから外す。そのためバージョン0.22.0までRC版は配布しない",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject:
							"アップデートの結果ログとログ出力内容をまとめた",
					},
					{
						subject:
							"ツールバーのボタンへのD&Dで指定して実行ダイアログを表示",
					},
					{
						subject:
							"ログダイアログの詳細部分表示方法を全面と分割の切り替え",
					},
					{
						subject: "#48: 全ノートに対する操作でロック状態は省く",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"使用許諾ダイアログ内の文言が他のUIテキストと異なっていた",
					},
					{
						subject:
							"過去バージョンからの強制使用許諾表示が使用設定より優先される不具合",
					},
					{
						subject:
							"マルチディスプレイで自動的に隠したツールバーの隠れた位置と表示位置が変な不具合",
					},
					{
						subject: "#9: ディスプレイ名を分かりやすく",
					},
					{
						subject:
							"#49: アップデートチェック処置でRC版のチェックが死んでる",
					},
					{
						subject:
							"#50: アップデートチェック時にキャッシュされたデータを参照する",
					},
					{
						subject:
							"#3: ツールバーメニューチェック表示がアイコンサイズに依存",
					},
					{
						subject:
							"領域内に収まらないランチャーアイテムのメニュー表示で例外発生",
					},
					{
						subject:
							"領域内に収まらないランチャーアイテムのファイル一覧がどんな設定でもやたらスリム",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "ランチャーアイテム種別選択を実装まで無効化",
					},
				],
			},
		],
	},
	{
		date: "2014/10/01",
		version: "0.19.0",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "0.18.0対応としての高速リリース",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "ツールバーでコンテキストが表示できないの修正",
					},
					{
						subject:
							"#34: 付箋フォームのコンテキストメニューがマルチディスプレイで（ｒｙ",
						comments: ["再修正"],
					},
					{
						subject: "#35: 付箋の再描画",
						comments: ["再修正"],
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "PeUpdater大幅改修",
					},
				],
			},
		],
	},
	{
		date: "2014/10/01",
		version: "0.18.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "サードパーティコンポーネント一覧追加",
					},
					{
						subject:
							"ノートのタイトル描画フォントをデフォルトではシステムのキャプションバーフォントに変更",
					},
					{
						subject:
							"フォント設定をシステムのダイアログのデフォルトに変更",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"タスクトレイのコンテキストメニュー表示時にログウィンドウの表示状態が反映されていない不具合の修正",
					},
					{
						subject:
							"ログウィンドウがタスクマネージャのアプリケーションに表示されないように変更",
					},
					{
						subject: "#35: 付箋の再描画",
					},
					{
						subject:
							"#34: 付箋フォームのコンテキストメニューがマルチディスプレイで（ｒｙ",
					},
					{
						subject: "ダイアログのカーソル自動移動処理がバグってた",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "PeMain.Data.Item関連のDisposeをあれやこれや",
					},
					{
						subject:
							"アップデート用チェンジログをリリース/RC版で分離、0.20.0で現行チェンジログ削除予定",
					},
				],
			},
		],
	},
	{
		date: "2014/09/28",
		version: "0.17.1",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject:
							"次回バージョンアップを兼ねて少しだけバージョンアップ",
					},
					{
						subject: "非RCだがリリース版ではない微妙な立ち位置",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject: "#45: プログラムの自動更新",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"言語適用順を ${...} -> @[...] から @[...] -> ${...} に変更",
					},
					{
						subject:
							"言語設定のキーが存在しなかった場合に<key>としていた処理から<>を付与しないように変更",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject:
							"/Pe/changelog.js 追加。 changelog.xml から最新バージョンを取得して /Pe/Update/update.html を作成",
					},
					{
						subject: "change.log -> changelog.xml",
					},
				],
			},
		],
	},
	{
		date: "2014/09/23",
		version: "0.17.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject:
							"標準出力取得ウィンドウに最前面固定の切り替え機能を追加",
					},
					{
						subject: "#42: ホットキーの表示",
					},
					{
						subject:
							"ノートのコンテキストメニューから「削除」を取り除く",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"Pe 情報ダイアログに表示されるリンクにメールアドレスを追加",
					},
					{
						subject:
							"使用許諾ダイアログ内のリンク遷移を外部で行うように修正",
					},
					{
						subject: "#21: 標準出力取得ウィンドウの更新",
					},
					{
						subject:
							"標準出力取得ウィンドウの更新ツールチップに対して文言が設定されていなかった",
					},
					{
						subject:
							"#46: 設定ダイアログから設定保存後の再起動で使用許諾ダイアログ表示",
					},
				],
			},
		],
	},
	{
		date: "2014/09/21",
		version: "0.16.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "64bit 対応版配布開始",
					},
					{
						subject:
							"情報ダイアログにデバッグ・リリースと対象プロセッサーを表示",
					},
					{
						subject: "#43: 初回起動時の承認画面",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"ノートのコンテキストメニュー[フォント/変更]を現在選択されているフォントを表示するよう変更",
					},
					{
						subject: "[Pe]/doc/readme-ja.txt 修正",
					},
					{
						subject: "設定/使用言語を#6完了まで非活性に変更",
					},
				],
			},
			{
				type: "developer",
				logs: [
					{
						subject: "リリースビルド用に /Pe/build.bat 追加",
					},
					{
						subject: "配布アーカイブの圧縮形式を 7z -> zip へ変更",
					},
				],
			},
		],
	},
	{
		date: "2014/09/15",
		version: "0.15.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "#22: 標準出力取得ウィンドウの機能実装",
					},
					{
						subject: "#37: 本体設定時におけるノートの各種設定",
					},
					{
						subject:
							"タスクトレイコンテキストメニューからウィンドウメニューをルートに移行",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "情報ウィンドウのタブインデックスを直感的に",
					},
					{
						subject: "クリアアイコンの追加",
					},
					{
						subject:
							"#20: 標準出力取得時に取得ウィンドウを閉じると例外",
					},
					{
						subject:
							"内部実装: 出力取得でエラー取得時に標準出力とマークされていた",
					},
				],
			},
		],
	},
	{
		date: "2014/09/11",
		version: "0.14.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject:
							"情報ウィンドウで各種ディレクトリを開くボタンの追加",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "#39: タブインデックスの順序を直観的にする",
					},
					{
						subject:
							"ツールバーが自動的に隠す状態でメニュー(コンテキスト/ボタン)表示した際にツールバーが隠れる不具合の修正",
					},
					{
						subject: "設定ウィンドウ/ランチャのリサイズ処理を修正",
					},
					{
						subject:
							"設定ウィンドウからツールバー位置変更で位置とサイズが変になる不具合の修正",
					},
				],
			},
		],
	},
	{
		date: "2014/09/07",
		version: "0.13.0",
		contents: [
			{
				type: "features",
				logs: [
					{
						subject: "指定して実行ダイアログへのD&Dで値設定",
					},
					{
						subject:
							"指定して実行ダイアログの作業フォルダ欄へのD&Dで値設定",
					},
					{
						subject: "#1: 情報ダイアログ実装",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject:
							"doc/change.log の出力設定を PreserveNewest に設定",
					},
					{
						subject:
							"タスクトレイメニューからノート作成でスクリーン中央に表示する",
					},
					{
						subject: "#38: 多言語によるUIの自動調整",
						comments: ["設定/本体は機能確定まで未定"],
					},
					{
						subject:
							"プログラムのアイコンとタスクトレイアイコンを統合",
					},
					{
						subject: "#40: ノート最小化時における本文編集",
					},
					{
						subject: "ツールバーメインアイコンの修正",
					},
					{
						subject:
							"設定保存時にログウィンドウの言語設定に失敗する不具合の修正",
					},
					{
						subject:
							"ログウィンドウを閉じた際に設定項目を非表示とするように修正",
					},
					{
						subject:
							"ToolStripItemへの言語設定でツールチップ設定がちょっと変だったのを修正",
					},
				],
			},
		],
	},
	{
		date: "2014/08/29",
		version: "0.12.0",
		contents: [
			{
				type: "fixes",
				logs: [
					{
						subject: "#11: ツールバー メインアイコン",
					},
					{
						subject: "タスクトレイアイコンを変更",
					},
				],
			},
		],
	},
	{
		date: "2014/08/28",
		version: "0.11.0",
		group: "Windows Forms",
		contents: [
			{
				type: "note",
				logs: [
					{
						subject: "お小遣い帳レベルで更新履歴をつけてみる",
					},
				],
			},
			{
				type: "features",
				logs: [
					{
						subject: "#15: ログダイアログの機能実装",
					},
				],
			},
			{
				type: "fixes",
				logs: [
					{
						subject: "付箋とノートの文言をノートに統一",
					},
				],
			},
		],
	},
];

export default Changelogs;
