select
	IFNULL(MAX(LauncherGroupItems.Sort), 0)
from
	LauncherGroupItems
where
	LauncherGroupItems.LauncherGroupId = @LauncherGroupId
