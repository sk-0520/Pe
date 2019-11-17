﻿insert into
	LauncherGroups
	(
		[LauncherGroupId],
		[Name],
		[Kind],
		[ImageName],
		[ImageColor],
		[Sequence],

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
/* LauncherGroupId          */ @LauncherGroupId,
/* Name                     */ @Name,
/* Kind                     */ @Kind,
/* ImageName                */ @ImageName,
/* ImageColor               */ @ImageColor,
/* Sequence                 */ @Sequence,
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
