select
	NoteContents.Content
from
	NoteContents
where
	NoteContents.NoteId = @NoteId
	and
	NoteContents.ContentKind = @ContentKind

