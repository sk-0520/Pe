insert into
	Toolbars
	(
		[ToolbarId],
		[Screen],
		[X],
		[Y],
		[Width],
		[Height],
		[LauncherGroupId],
		[PositionKind],
		[IconKind],
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
/* ToolbarId                */ @ToolbarId,
/* Screen                   */ @Screen,
/* X                        */ @X,
/* Y                        */ @Y,
/* Width                    */ @Width,
/* Height                   */ @Height,
/* LauncherGroupId          */ '00000000-0000-0000-0000-000000000000',
/* PositionKind             */ 'right',
/* IconKind                 */ 'normal',
/* FontId                   */ '00000000-0000-0000-0000-000000000000',
/* AutoHideTimeout          */ '00:00:03',
/* TextWidth                */ 80,
/* IsVisible                */ true,
/* IsTopmost                */ true,
/* IsAutoHide               */ false,
/* IsIconOnly               */ false,

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
