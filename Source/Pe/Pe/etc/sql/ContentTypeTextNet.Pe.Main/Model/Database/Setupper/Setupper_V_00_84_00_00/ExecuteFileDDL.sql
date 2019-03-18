--// TABLE: LauncherItemIcons
create table LauncherItemIcons(
[LauncherItemId] text not null,
[IconScale] text not null,
[Sequence] integer not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[Image] blob,
primary key (
	[LauncherItemId],
	[IconScale],
	[Sequence]
)
)
;

--// TABLE: NoteFiles
create table NoteFiles(
[NoteFileId] text not null,
[Sequence] integer not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[Content] blob,
primary key (
	[NoteFileId],
	[Sequence]
)
)
;

