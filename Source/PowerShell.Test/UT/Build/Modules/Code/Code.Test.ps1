$root = Split-Path -Path $PSScriptRoot -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent | Split-Path -Parent
Import-Module "$root/Build/Modules/Code" -Force

Describe 'Invoke-Using' {
	It 'dispose' {
		$script:callCount = 0
		class Test: System.IDisposable {
			Dispose() {
				$script:callCount += 1
			}
		}

		Invoke-Using -Object ([Test]::new()) {
			$script:callCount += 1
		}

		$script:callCount | Should Be 2
	}

	It 'object' {
		$script:callCount = 0

		{
			Invoke-Using -Object ([Object]::new()) {
				$script:callCount += 1
			}
		} | Should Throw

		$script:callCount | Should Be 0
	}
}
