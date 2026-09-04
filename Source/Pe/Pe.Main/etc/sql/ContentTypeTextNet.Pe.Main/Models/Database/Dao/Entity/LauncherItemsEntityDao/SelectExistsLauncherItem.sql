select
	COUNT(*) = 1
from
	LauncherItems
where
	LauncherItems.LauncherItemId = @LauncherItemId
