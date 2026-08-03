# Creates throwaway dummy files for Secure File Shredder testing.
# Run: right-click -> Run with PowerShell, or:
#   powershell -ExecutionPolicy Bypass -File .\CreateDummyFiles.ps1

$ErrorActionPreference = "Stop"

$extensions = @(".txt", ".bin", ".log", ".dat", ".tmp", ".csv", ".json", ".xml")
$outputDir = Join-Path $PSScriptRoot "DummyFiles"

Write-Host "Secure File Shredder - Dummy File Generator" -ForegroundColor Cyan
Write-Host "Output folder: $outputDir"
Write-Host ""

$countInput = Read-Host "How many files do you want to create"
$fileCount = 0
if (-not [int]::TryParse($countInput, [ref]$fileCount) -or $fileCount -lt 1) {
    Write-Host "Invalid count. Enter a positive whole number." -ForegroundColor Red
    exit 1
}

$sizeInput = Read-Host "File size (examples: 100KB, 5MB, 1024, or 2GB)"
$sizeInput = $sizeInput.Trim().ToUpperInvariant()

if ($sizeInput -match '^\s*(\d+)\s*(B|KB|MB|GB)?\s*$') {
    $value = [long]$Matches[1]
    $unit = if ($Matches[2]) { $Matches[2] } else { "B" }
    $multiplier = switch ($unit) {
        "B"  { 1L }
        "KB" { 1L -shl 10 }
        "MB" { 1L -shl 20 }
        "GB" { 1L -shl 30 }
    }
    $fileSize = $value * $multiplier
}
else {
    Write-Host "Invalid size. Use formats like 512, 100KB, 5MB, or 1GB." -ForegroundColor Red
    exit 1
}

if ($fileSize -lt 1) {
    Write-Host "File size must be at least 1 byte." -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir | Out-Null
}

Write-Host ""
Write-Host "Creating $fileCount file(s) of $fileSize byte(s) each..." -ForegroundColor Yellow

$bufferSize = [int][Math]::Min($fileSize, 1MB)
$buffer = New-Object byte[] $bufferSize
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()

function New-RandomFileName {
    param([string[]]$Exts)

    $chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
    $nameLength = Get-Random -Minimum 8 -Maximum 17
    $nameChars = for ($n = 0; $n -lt $nameLength; $n++) {
        $chars[(Get-Random -Maximum $chars.Length)]
    }
    $baseName = -join $nameChars
    $ext = $Exts[(Get-Random -Maximum $Exts.Length)]
    return "$baseName$ext"
}

try {
    $usedNames = @{}
    for ($i = 1; $i -le $fileCount; $i++) {
        do {
            $fileName = New-RandomFileName -Exts $extensions
        } while ($usedNames.ContainsKey($fileName))

        $usedNames[$fileName] = $true
        $filePath = Join-Path $outputDir $fileName

        $stream = [System.IO.File]::Create($filePath)
        try {
            $remaining = $fileSize
            while ($remaining -gt 0) {
                $chunk = [int][Math]::Min($remaining, $buffer.Length)
                $rng.GetBytes($buffer, 0, $chunk)
                $stream.Write($buffer, 0, $chunk)
                $remaining -= $chunk
            }
        }
        finally {
            $stream.Dispose()
        }

        Write-Host ("  [{0}/{1}] {2}" -f $i, $fileCount, $filePath)
    }
}
finally {
    $rng.Dispose()
}

Write-Host ""
Write-Host "Done. Created $fileCount file(s) in:" -ForegroundColor Green
Write-Host "  $outputDir"
Write-Host "Press Enter to exit."
[void](Read-Host)
