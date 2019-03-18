insert into
	LauncherItemIcons
	(
		[LauncherItemId],
		[IconScale],
		[Sequence],
		[Image],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion]
	)
	values
	(
/* LauncherItemId           */ @LauncherItemId,
/* IconScale                */ @IconScale,
/* Sequence                 */ @Sequence,
/* Image                    */ @Image,

/* CreatedTimestamp         */ CURRENT_TIMESTAMP,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion
	)
