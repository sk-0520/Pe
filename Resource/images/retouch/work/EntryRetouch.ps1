Param(
	[Parameter(Mandatory)]
	[ValidateSet("release", "debug", "beta", "plugin")]
	[string] $Mode
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$iconMappings = @{
	"release" = @{
		name = 'App-release'
		color = '#ffffff'
	}
	"debug" = @{
		name = 'App-debug'
		color = '#ff8888'
	}
	"beta" = @{
		name = 'App-beta'
		color = '#8888ff'
	}
	"plugin" = @{
		name = 'App-plugin'
		color = '#88ff88'
	}
}
$icon = $iconMappings[$Mode]


. ./$PSScriptRoot/Retouch.ps1 `
	-SourcePath "/data/App.svg" `
	-OutputPath "/data/@data/$($icon['name']).svg" `
	-Color $icon['color']


