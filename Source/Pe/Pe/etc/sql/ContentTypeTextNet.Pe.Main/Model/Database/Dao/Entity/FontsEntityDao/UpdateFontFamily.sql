update
	Fonts
set
	FamilyName = @FamilyName
where
	FontId = @FontId
	and
	FamilyName != @FamilyName

