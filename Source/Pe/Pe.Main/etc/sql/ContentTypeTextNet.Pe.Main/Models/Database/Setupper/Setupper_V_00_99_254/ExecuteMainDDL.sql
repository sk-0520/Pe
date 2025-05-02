--// table: LauncherBadges
create table LauncherBadges
(
	LauncherItemId text not null, -- ランチャーアイテムID
	CreatedTimestamp datetime not null, -- 作成タイムスタンプ UTC
	CreatedAccount text not null, -- 作成ユーザー名
	CreatedProgramName text not null, -- 作成プログラム名
	CreatedProgramVersion text not null, -- 作成プログラムバージョン
	UpdatedTimestamp datetime not null, -- 更新タイムスタンプ UTC
	UpdatedAccount text not null, -- 更新ユーザー名
	UpdatedProgramName text not null, -- 更新プログラム名
	UpdatedProgramVersion text not null, -- 更新プログラムバージョン
	UpdatedCount integer not null, -- 更新回数 0始まり
	IsVisible boolean not null, -- 表示するか
	Display text not null, -- 表示文言
	Shape text not null, -- 図形
	Background text not null, -- 待機時間 #AARRGGBB
	primary key
	(
		LauncherItemId
	),
	foreign key(LauncherItemId) references LauncherItems(LauncherItemId)
)
;
