select
	Fonts.FontId,
	Fonts.FamilyName,
	Fonts.Height,
	Fonts.Bold,
	Fonts.Italic,
	Fonts.Underline,
	Fonts.Strike
from
	Fonts
where
	Fonts.FontId = @FontId
