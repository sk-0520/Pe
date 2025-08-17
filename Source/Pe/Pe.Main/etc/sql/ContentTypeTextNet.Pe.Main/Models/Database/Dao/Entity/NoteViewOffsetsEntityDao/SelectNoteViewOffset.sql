select
	NoteViewOffsets.X,
	NoteViewOffsets.Y
from
	NoteViewOffsets
where
	NoteId = @NoteId
