--// #986
update
	NoteContents
set
	IsLink = 0
where
	IsLink = 1
	and
	Address = ''
;
