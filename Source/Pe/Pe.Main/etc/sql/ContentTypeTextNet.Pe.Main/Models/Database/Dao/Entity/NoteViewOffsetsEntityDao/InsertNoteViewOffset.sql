insert into
	NoteViewOffsets
	(
		NoteId,
		X,
		Y,

		CreatedTimestamp,
		CreatedAccount,
		CreatedProgramName,
		CreatedProgramVersion
	)
	values
	(
/* NoteId                */ @NoteId,
/* X                     */ @X,
/* Y                     */ @Y,

/* CreatedTimestamp      */ @CreatedTimestamp,
/* CreatedAccount        */ @CreatedAccount,
/* CreatedProgramName    */ @CreatedProgramName,
/* CreatedProgramVersion */ @CreatedProgramVersion
)
