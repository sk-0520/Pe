Param(
	[Parameter(Mandatory)]
	[ValidateSet("release", "debug", "beta", "plugin")]
	[string] $Mode
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$iconMappings = @{
	"release" = @{
		color = '#ffffff'
	}
	"debug" = @{
		color = '#ff8888'
	}
	"beta" = @{
		color = '#8888ff'
	}
	"plugin" = @{
		color = '#88ff88'
	}
}
$icon = $iconMappings[$Mode]

. ./$PSScriptRoot/Retouch.ps1 `
	-SourcePath "/data/App.svg" `
	-OutputPath "/data/@data/${Mode}/App.svg" `
	-Color $icon['color']


