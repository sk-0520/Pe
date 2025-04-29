--// [#983] 退避テーブル作成
create table
	AppLauncherToolbarSetting2
as
	select
		*
	from
		AppLauncherToolbarSetting
;

--// [#983] 現行テーブル破棄
drop table
	AppLauncherToolbarSetting
;

--// table: AppLauncherToolbarSetting
create table AppLauncherToolbarSetting
(
	Generation integer not null, -- 世代 最大のものを使用する
	CreatedTimestamp datetime not null, -- 作成タイムスタンプ UTC
	CreatedAccount text not null, -- 作成ユーザー名
	CreatedProgramName text not null, -- 作成プログラム名
	CreatedProgramVersion text not null, -- 作成プログラムバージョン
	UpdatedTimestamp datetime not null, -- 更新タイムスタンプ UTC
	UpdatedAccount text not null, -- 更新ユーザー名
	UpdatedProgramName text not null, -- 更新プログラム名
	UpdatedProgramVersion text not null, -- 更新プログラムバージョン
	UpdatedCount integer not null, -- 更新回数 0始まり
	PositionKind text not null, -- 表示位置 上下左右
	Direction text not null, -- 方向 アイコンの並びの基点
	IconBox text not null, -- アイコンサイズ
	FontId text not null, -- フォント
	DisplayDelayTime text not null, -- 表示するまでの抑制時間
	AutoHideTime text not null, -- 自動的に隠す時間
	TextWidth integer not null, -- 文字幅
	IsVisible boolean not null, -- 表示
	IsTopmost boolean not null, -- 最前面
	IsAutoHide boolean not null, -- 自動的に隠す
	IsIconOnly boolean not null, -- アイコンのみ
	ContentDropMode text not null, -- ツールバーへのD&D処理
	GroupMenuPosition text not null, -- グループメニュー表示位置
	ShortcutDropMode text not null, -- ショートカットD&D時の挙動
	DuplicatedFileRegisterMode text not null, -- 重複ファイルアイテム登録方法
	primary key
	(
		Generation
	),
	foreign key(FontId) references Fonts(FontId)
)
;

insert into
	AppLauncherToolbarSetting
select
	[AppLauncherToolbarSetting2].[Generation],
	[AppLauncherToolbarSetting2].[CreatedTimestamp],
	[AppLauncherToolbarSetting2].[CreatedAccount],
	[AppLauncherToolbarSetting2].[CreatedProgramName],
	[AppLauncherToolbarSetting2].[CreatedProgramVersion],
	[AppLauncherToolbarSetting2].[UpdatedTimestamp],
	[AppLauncherToolbarSetting2].[UpdatedAccount],
	[AppLauncherToolbarSetting2].[UpdatedProgramName],
	[AppLauncherToolbarSetting2].[UpdatedProgramVersion],
	[AppLauncherToolbarSetting2].[UpdatedCount],
	[AppLauncherToolbarSetting2].[PositionKind],
	[AppLauncherToolbarSetting2].[Direction],
	[AppLauncherToolbarSetting2].[IconBox],
	[AppLauncherToolbarSetting2].[FontId],
	[AppLauncherToolbarSetting2].[DisplayDelayTime],
	[AppLauncherToolbarSetting2].[AutoHideTime],
	[AppLauncherToolbarSetting2].[TextWidth],
	[AppLauncherToolbarSetting2].[IsVisible],
	[AppLauncherToolbarSetting2].[IsTopmost],
	[AppLauncherToolbarSetting2].[IsAutoHide],
	[AppLauncherToolbarSetting2].[IsIconOnly],
	[AppLauncherToolbarSetting2].[ContentDropMode],
	[AppLauncherToolbarSetting2].[GroupMenuPosition],
	[AppLauncherToolbarSetting2].[ShortcutDropMode],
	'file-path-only'
from
	[AppLauncherToolbarSetting2]
;

--// [#983] 退避テーブル破棄
drop table [AppLauncherToolbarSetting2]
;

