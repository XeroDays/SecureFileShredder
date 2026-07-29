# Keeps locked.txt open with exclusive write access until you press Enter.
# Run: right-click -> Run with PowerShell, or:
#   powershell -ExecutionPolicy Bypass -File .\LockFile.ps1

$ErrorActionPreference = "Stop"
$filePath = Join-Path $PSScriptRoot "locked.txt"

if (-not (Test-Path $filePath)) {
    Write-Host "File not found: $filePath" -ForegroundColor Red
    exit 1
}

$stream = [System.IO.File]::Open(
    $filePath,
    [System.IO.FileMode]::Open,
    [System.IO.FileAccess]::ReadWrite,
    [System.IO.FileShare]::None
)

Write-Host "Locked (exclusive write): $filePath" -ForegroundColor Yellow
Write-Host "Leave this window open, then shred files in Secure File Shredder."
Write-Host "Press Enter here to release the lock and exit."
[void](Read-Host)

$stream.Close()
$stream.Dispose()
Write-Host "Lock released." -ForegroundColor Green
