$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

<#
.SYNOPSIS
	IDisposable に対する自動解放処理。

.PARAMETER Object
	IDisposable オブジェクト。

.PARAMETER ScriptBlock
	処理。
#>
function Invoke-Using {
	Param(
		[Parameter(mandatory = $true, Position = 0)][System.IDisposable] $Object,
		[Parameter(mandatory = $true, Position = 1)][ScriptBlock] $ScriptBlock
	)

	try {
		Invoke-Command -ScriptBlock $ScriptBlock
	} finally {
		if ($null -ne $Object) {
			$Object.Dispose()
		}
	}
}
