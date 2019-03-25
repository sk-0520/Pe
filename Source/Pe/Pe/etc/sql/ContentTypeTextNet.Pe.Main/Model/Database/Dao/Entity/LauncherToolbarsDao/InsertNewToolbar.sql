insert into
	LauncherToolbars
	(
		[LauncherToolbarId],
		[ScreenName],
		[LauncherGroupId],
		[PositionKind],
		[IconScale],
		[FontId],
		[AutoHideTimeout],
		[TextWidth],
		[IsVisible],
		[IsTopmost],
		[IsAutoHide],
		[IsIconOnly],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion],
		[UpdatedTimestamp],
		[UpdatedAccount],
		[UpdatedProgramName],
		[UpdatedProgramVersion],
		[UpdatedCount]
	)
	values
	(
/* ToolbarId                */ @LauncherToolbarId,
/* ScreenName               */ @ScreenName,
/* LauncherGroupId          */ '00000000-0000-0000-0000-000000000000',
/* PositionKind             */ 'right',
/* IconScale                */ 'normal',
/* FontId                   */ '00000000-0000-0000-0000-000000000000',
/* AutoHideTimeout          */ '00:00:03',
/* TextWidth                */ 80,
/* IsVisible                */ 1,
/* IsTopmost                */ 1,
/* IsAutoHide               */ 0,
/* IsIconOnly               */ 1,

/*                          */
/* CreatedTimestamp         */ CURRENT_TIMESTAMP,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ CURRENT_TIMESTAMP,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	)
