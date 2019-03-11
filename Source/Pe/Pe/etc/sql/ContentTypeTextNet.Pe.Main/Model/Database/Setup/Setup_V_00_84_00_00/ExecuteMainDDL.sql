--// table: AppSystems
create table AppSystems(
[Key] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Value] text not null,
[Note] text not null,
primary key (
	[Key]
)
)
;

--// table: LauncherItems
create table LauncherItems(
[LauncherItemId] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Code] text not null unique,
[Name] text not null,
[Kind] text not null,
[Command] text not null,
[Option] text not null,
[WorkDirectory] text not null,
[IconPath] text not null,
[IconIndex] integer not null,
[IsEnabledCommandLauncher] boolean not null,
[IsEnabledCustomEnvVar] boolean not null,
[IsEnabledStandardOutput] boolean not null,
[IsEnabledStandardInput] boolean not null,
[Permission] text not null,
[CredentId] text not null,
[ExecuteCount] integer not null,
[LastExecuteTimestamp] text not null,
[Note] text not null,
primary key (
	[LauncherItemId]
)
)
;

--// table: LauncherEnvVars
create table LauncherEnvVars(
[LauncherItemId] text not null,
[EnvName] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Kind] text not null,
[EnvValue] text not null,
primary key (
	LauncherItemId,
	EnvName
)
)
;


--// table: LauncherTags
create table LauncherTags(
[LauncherItemId] text not null,
[TagName] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
primary key (
	[LauncherItemId],
	[TagName]
)
)
;





--// table: LauncherItemHistories
create table LauncherItemHistories(
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[LauncherItemId] text not null,
[Kind] text not null,
[Value] text not null unique,
[LastExecuteTimestamp] datetime not null
)
;


--// table: Credents
create table Credents(
[CredentId] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Name] text not null,
[Password] text not null,
primary key (
CredentId
)
)
;


--// table: Fonts
create table Fonts(
[FontId] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[FamilyName] text not null,
[Height] real not null,
[Bold] boolean not null,
[Italic] boolean not null,
[Underline] boolean not null,
[Strike] boolean not null,
primary key (
	FontId
)
)
;


--// table: LauncherGroups
create table LauncherGroups(
[LauncherGroupId] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Name] text,
[ImageName] text,
[ImageColor] text,
[Sort] integer not null,
primary key (
LauncherGroupId
)
)
;


--// table: LauncherGroupItems
create table LauncherGroupItems(
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[LauncherGroupId] text not null,
[LauncherItemId] text not null,
[Sort] integer not null
)
;

--// table: Toolbars
create table Toolbars(
[ToolbarId] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Screen] text not null,
[X] integer not null,
[Y] integer not null,
[Width] integer not null,
[Height] integer not null,
[LauncherGroupId] text not null,
[PositionKind] text not null,
[IconKind] integer not null,
[FontId] text not null,
[AutoHideTimeout] text not null,
[TextWidth] integer not null,
[IsVisible] boolean not null,
[IsTopmost] boolean not null,
[IsAutoHide] boolean not null,
[IsIconOnly] boolean not null,
primary key (
ToolbarId
)
)
;

--// table: Notes
create table Notes(
[NoteId] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Title] text not null,
[PositionKind] text not null,
[RelativeScreen] text not null,
[AbsoluteX] real not null,
[AbsoluteY] real not null,
[RelativeX] real not null,
[RelativeY] real not null,
[SizeKind] text not null,
[AbsoluteWidth] real not null,
[AbsoluteHeight] real not null,
[RelativeWidth] real not null,
[RelativeHeight] real not null,
[IsVisible] boolean not null,
[FontId] text not null,
[ForegdoundColor] text not null,
[BackgroundColor] text not null,
[IsLocked] boolean not null,
[IsTopmost] boolean not null,
[IsCompact] boolean not null,
[TextWrap] text not null,
[ContentKind] text not null,
primary key (
NoteId
)
)
;


--// table: NoteContents
create table NoteContents(
[NoteId] text not null,
[ContentKind] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[Content] text not null,
primary key (
	NoteId,
	ContentKind
)
)
;


--// table: NoteFiles
create table NoteFiles(
[NoteId] text not null,
[CreatedTimestamp] datetime not null, [CreatedAccount] text not null, [CreatedProgramName] text not null, [CreatedProgramVersion] text not null,
[UpdatedTimestamp] datetime not null, [UpdatedAccount] text not null, [UpdatedProgramName] text not null, [UpdatedProgramVersion] text not null, [UpdatedCount] integer not null,
[FileKind] text not null,
[FilePath] text not null,
[NoteFileId] text not null,
[Sort] integer not null,
primary key (
	NoteId
)
)
;

