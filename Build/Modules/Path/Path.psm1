$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

<#
.SYNOPSIS
	現在ディレクトリを変更しつつ処理して、その後変更前ディレクトリに移動。

.PARAMETER Path
	移動先ディレクトリ。

.PARAMETER ScriptBlock
	処理。
#>
function Invoke-Location {
	Param(
		[Parameter(mandatory = $true, Position = 0)][string] $Path,
		[Parameter(mandatory = $true, Position = 1)][ScriptBlock] $ScriptBlock
	)

	Push-Location -LiteralPath $Path
	try {
		Invoke-Command -ScriptBlock $ScriptBlock
	} finally {
		Pop-Location
	}
}
