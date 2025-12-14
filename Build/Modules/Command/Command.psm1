function Start-Command {
	[CmdletBinding(SupportsShouldProcess = $true)]
	param (
		[Parameter(Mandatory = $true)][string] $Command,
		[Parameter(Mandatory = $false)][int[]] $SuccessCodes,
		[Parameter(Mandatory = $false)][string[]] $ArgumentList
	)

	if ($PSCmdlet.ShouldProcess($Command, $ArgumentList)) {
		Write-Verbose ('{0} {1}' -f $Command, $ArgumentList)
		& $Command $ArgumentList
		$LastExitCode = $LASTEXITCODE
		$hasError = $false
		if($SuccessCodes.Count -eq 0) {
			$hasError = $LastExitCode -ne 0
		} else {
			$hasError = -not ($SuccessCodes.Contains($LastExitCode))
		}
		if($hasError) {
			throw "${Command} ${ArgumentList}: ${LastExitCode}"
		}
	} else {
		Write-Verbose ('[DRY] 実行予定: {0} {1}' -f $Command, $ArgumentList)
	}
}
