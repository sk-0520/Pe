delete
from
	LauncherItemHistories
where
	LauncherItemHistories.LauncherItemId = @LauncherItemId
	and
	LauncherItemHistories.Kind = @Kind
	and
	LauncherItemHistories.Value = @Value

