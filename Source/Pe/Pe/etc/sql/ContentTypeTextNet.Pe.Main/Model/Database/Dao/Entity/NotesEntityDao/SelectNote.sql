select
	Notes.NoteId,
	Notes.Title,
	Notes.ScreenName,
	Notes.LayoutKind,
	Notes.IsVisible,
	Notes.FontId,
	Notes.ForegdoundColor,
	Notes.BackgroundColor,
	Notes.IsLocked,
	Notes.IsTopmost,
	Notes.IsCompact,
	Notes.TextWrap,
	Notes.ContentKind
from
	Notes
where
	Notes.NoteId = @NoteId
