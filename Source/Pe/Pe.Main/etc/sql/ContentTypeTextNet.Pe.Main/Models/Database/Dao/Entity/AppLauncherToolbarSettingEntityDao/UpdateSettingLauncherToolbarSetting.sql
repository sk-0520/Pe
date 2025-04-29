update
	AppLauncherToolbarSetting
set
	ContentDropMode = @ContentDropMode,
	ShortcutDropMode = @ShortcutDropMode,
	GroupMenuPosition = @GroupMenuPosition,
	DuplicatedFileRegisterMode = @DuplicatedFileRegisterMode,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	AppLauncherToolbarSetting.Generation = (
		select
			MAX(AppLauncherToolbarSetting.Generation)
		from
			AppLauncherToolbarSetting
	)
