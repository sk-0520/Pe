﻿insert into
	LauncherFiles
	(
		[LauncherItemId],
		[File],
		[Option],
		[WorkDirectory],
		[IsEnabledCustomEnvVar],
		[IsEnabledStandardIo],
		[StandardIoEncoding],
		[RunAdministrator],

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
/* LauncherItemId           */ @LauncherItemId,
/* File                     */ @File,
/* Option                   */ @Option,
/* WorkDirectory            */ @WorkDirectory,
/* IsEnabledCustomEnvVar    */ false,
/* IsEnabledStandardIo      */ false,
/* StandardIoEncoding       */ @StandardIoEncoding,
/* RunAdministrator         */ false,

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
