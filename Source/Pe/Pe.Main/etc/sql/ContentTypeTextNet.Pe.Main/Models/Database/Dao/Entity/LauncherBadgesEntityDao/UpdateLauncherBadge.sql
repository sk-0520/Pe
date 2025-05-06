update
	LauncherBadges
set
	IsVisible = @IsVisible,
	Display = @Display,
	Shape = @Shape,
	Background = @Background,

	UpdatedTimestamp = @UpdatedTimestamp,
	UpdatedAccount = @UpdatedAccount,
	UpdatedProgramName = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount = UpdatedCount + 1
where
	LauncherItemId = @LauncherItemId
