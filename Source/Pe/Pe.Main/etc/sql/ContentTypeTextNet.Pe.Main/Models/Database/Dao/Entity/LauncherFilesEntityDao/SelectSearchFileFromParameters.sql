select
	LauncherFiles.LauncherItemId
from
	LauncherFiles
where
	RTRIM(REPLACE(LauncherFiles.File, '/', '\'), '\') = RTRIM(REPLACE(@File, '/', '\'), '\') collate nocase -- noqa: PRS
/*{{*//*OPTION
ON:CODE
	and
	LauncherFiles.Option = @Option
*//*}}*/
order by
	LauncherFiles.CreatedTimestamp,
	LauncherFiles.LauncherItemId
