select
	LauncherFiles.LauncherItemId
from
	LauncherFiles
where
	RTRIM(REPLACE(LauncherFiles.File, '/', '\'), '\') = RTRIM(REPLACE(@File, '/', '\'), '\') collate nocase -- noqa: PRS
order by
	LauncherFiles.CreatedTimestamp,
	LauncherFiles.LauncherItemId
