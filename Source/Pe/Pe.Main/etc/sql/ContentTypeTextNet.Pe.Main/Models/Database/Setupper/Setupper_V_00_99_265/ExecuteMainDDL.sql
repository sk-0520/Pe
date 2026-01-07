--// [#1035] 退避テーブル作成
create table
	AppNoteSetting2
as
	select
		*
	from
		AppNoteSetting
;

--// [#1035] 現行テーブル破棄
drop table
	AppNoteSetting
;

--// table: AppNoteSetting
create table AppNoteSetting
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
	FontId text not null, -- フォント
	TitleKind text not null, -- タイトル設定
	LayoutKind text not null, -- 位置種別
	ForegroundColor text not null, -- 前景色 #AARRGGBB
	BackgroundColor text not null, -- 背景色 #AARRGGBB
	IsTopmost boolean not null, -- 最前面
	CaptionPosition text not null, -- タイトル位置
	ExcludeScreenCapture boolean not null, -- スクリーンキャプチャから除外するか
	primary key
	(
		Generation
	),
	foreign key(FontId) references Fonts(FontId)
)
;

insert into
	[AppNoteSetting]
select
	[AppNoteSetting2].[Generation],
	[AppNoteSetting2].[CreatedTimestamp],
	[AppNoteSetting2].[CreatedAccount],
	[AppNoteSetting2].[CreatedProgramName],
	[AppNoteSetting2].[CreatedProgramVersion],
	[AppNoteSetting2].[UpdatedTimestamp],
	[AppNoteSetting2].[UpdatedAccount],
	[AppNoteSetting2].[UpdatedProgramName],
	[AppNoteSetting2].[UpdatedProgramVersion],
	[AppNoteSetting2].[UpdatedCount],
	[AppNoteSetting2].[FontId],
	[AppNoteSetting2].[TitleKind],
	[AppNoteSetting2].[LayoutKind],
	[AppNoteSetting2].[ForegroundColor],
	[AppNoteSetting2].[BackgroundColor],
	[AppNoteSetting2].[IsTopmost],
	[AppNoteSetting2].[CaptionPosition],
	1
from
	[AppNoteSetting2]
;

--// [#1035] 退避テーブル破棄
drop table [AppNoteSetting2]
;

