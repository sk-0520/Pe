--// AppSystems
insert into
	AppSystems
	(
		[Key],
		[Value],
		[Note],
		/*CREATE*/ CreatedTimestamp, CreatedAccount, CreatedProgramName, CreatedProgramVersion, /*UPDATE*/ UpdatedTimestamp, UpdatedAccount, UpdatedProgramName, UpdatedProgramVersion, UpdatedCount
	)
	values
	(
		'last.version',
		'0.84.0.0',
		'',
		/*CREATE*/ @CreatedTimestamp, @CreatedAccount, @CreatedProgramName, @CreatedProgramVersion, /*UPDATE*/ @UpdatedTimestamp, @UpdatedAccount, @UpdatedProgramName, @UpdatedProgramVersion, 0
	)

;










