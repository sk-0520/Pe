select
	COUNT(*) = 1
from
	Plugins
where
	Plugins.PluginId = @PluginId
