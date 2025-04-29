select
	LauncherFiles.LauncherItemId
from
	LauncherFiles
where
	RTRIM(REPLACE(LauncherFiles.File, '/', '\'), '\') collate nocase = RTRIM(REPLACE(@File, '/', '\'), '\')
order by
	LauncherFiles.CreatedTimestamp,
	LauncherFiles.LauncherItemId
