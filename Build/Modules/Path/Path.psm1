$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

<#
.SYNOPSIS
	IDisposable に対する自動解放処理。

.PARAMETER Path
	IDisposable オブジェクト。

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
