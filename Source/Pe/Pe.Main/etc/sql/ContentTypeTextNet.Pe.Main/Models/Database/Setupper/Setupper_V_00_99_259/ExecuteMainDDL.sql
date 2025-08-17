--// #733
create table NoteViewOffsets
(
	NoteId text not null, -- ノートID
	CreatedTimestamp datetime not null, -- 作成タイムスタンプ UTC
	CreatedAccount text not null, -- 作成ユーザー名
	CreatedProgramName text not null, -- 作成プログラム名
	CreatedProgramVersion text not null, -- 作成プログラムバージョン
	X real not null, -- X位置
	Y real not null, -- Y位置
	primary key
	(
		NoteId
	),
	foreign key(NoteId) references Notes(NoteId)
)
;
