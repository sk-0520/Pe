select
	LauncherGroups.LauncherGroupId,
	LauncherGroups.Name,
	LauncherGroups.ImageName,
	LauncherGroups.ImageColor,
	LauncherGroups.Sort
from
	LauncherGroups
where
	LauncherGroups.LauncherGroupId = @LauncherGroupId
