select
	LauncherBadges.IsVisible,
	LauncherBadges.Display,
	LauncherBadges.Shape,
	LauncherBadges.Background
from
	LauncherBadges
where
	LauncherBadges.LauncherItemId = @LauncherItemId
