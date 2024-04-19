$root = Split-Path -Path $PSScriptRoot -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent
Import-Module "$root/Build/Modules/Path" -Force

Describe 'Invoke-Using' {
	$workDir = Split-Path -Path $PSScriptRoot -Parent | Join-Path -ChildPath 'work'
	if (Test-Path -Path $workDir) {
		Remove-Item -LiteralPath $workDir -Recurse -Force
	}
	New-Item -Path $workDir -ItemType Directory

	It "test" {
		$current = Get-Location
		Invoke-Location -Path $workDir {
			Get-Location | Should Be "$workDir"
		}
		Get-Location | Should Be "$current"
	}
}
