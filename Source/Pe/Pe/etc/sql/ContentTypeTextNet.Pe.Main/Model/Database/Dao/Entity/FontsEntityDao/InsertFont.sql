insert into
	Fonts
	(
		[FontId],
		[FamilyName],
		[Height],
		[Bold],
		[Italic],
		[Underline],
		[Strike],

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
/* FontId                   */ @FontId,
/* FamilyName               */ @FamilyName,
/* Height                   */ @Height,
/* Bold                     */ @Bold,
/* Italic                   */ @Italic,
/* Underline                */ @Underline,
/* Strike                   */ @Strike,
/*                          */
/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ @UpdatedTimestamp,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	)
