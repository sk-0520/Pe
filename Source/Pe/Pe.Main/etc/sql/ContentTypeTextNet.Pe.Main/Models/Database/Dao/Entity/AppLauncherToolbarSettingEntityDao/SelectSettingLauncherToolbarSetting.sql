select
	AppLauncherToolbarSetting.ContentDropMode,
	AppLauncherToolbarSetting.ShortcutDropMode,
	AppLauncherToolbarSetting.GroupMenuPosition,
	AppLauncherToolbarSetting.DuplicatedFileRegisterMode
from
	AppLauncherToolbarSetting
where
	AppLauncherToolbarSetting.Generation = (
		select
			MAX(AppLauncherToolbarSetting.Generation)
		from
			AppLauncherToolbarSetting
	)
