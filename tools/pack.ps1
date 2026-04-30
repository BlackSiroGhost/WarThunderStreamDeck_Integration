#requires -Version 5.1
<#
.SYNOPSIS
  Build and package every plugin under plugin/ into a .streamDeckPlugin file
  that friends can double-click to install.

.DESCRIPTION
  - Runs deploy.ps1 -NoInstall to publish each plugin into dist/<id>.sdPlugin/
  - Wraps each .sdPlugin folder into a zip and renames to .streamDeckPlugin
  - Outputs to dist/<id>-v<Version>.streamDeckPlugin

.PARAMETER Version
  Version string used in the output filename (default: 0.3.0).

.PARAMETER Configuration
  Build configuration (default: Release).
#>

param(
  [string]$Version = '0.3.0',
  [ValidateSet('Debug', 'Release')]
  [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'

$repoRoot   = Split-Path -Parent $PSScriptRoot
$pluginsDir = Join-Path $repoRoot 'plugin'
$distDir    = Join-Path $repoRoot 'dist'

if (-not (Test-Path $distDir)) {
  $null = New-Item $distDir -ItemType Directory -Force
}

# Build first (publishes into dist/<id>.sdPlugin/)
Write-Host "==> Building..." -ForegroundColor Cyan
& (Join-Path $PSScriptRoot 'deploy.ps1') -NoInstall -Configuration $Configuration

Write-Host ""
Write-Host "==> Packaging..." -ForegroundColor Cyan

$packaged = @()
Get-ChildItem $pluginsDir -Directory -Filter '*.sdPlugin' | ForEach-Object {
  $pluginId = $_.Name
  $built    = Join-Path $distDir $pluginId
  if (-not (Test-Path $built)) {
    Write-Warning "Build artifact missing for $pluginId at $built - skipping"
    return
  }

  $base    = $pluginId.Replace('.sdPlugin', '') + '-v' + $Version
  $zipPath = Join-Path $distDir ($base + '.zip')
  $sdpPath = Join-Path $distDir ($base + '.streamDeckPlugin')

  if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
  if (Test-Path $sdpPath) { Remove-Item $sdpPath -Force }

  # Stream Deck expects the .sdPlugin folder to be the top-level entry inside the archive
  Compress-Archive -Path $built -DestinationPath $zipPath -CompressionLevel Optimal -Force
  Rename-Item $zipPath $sdpPath

  $sizeMb = [math]::Round((Get-Item $sdpPath).Length / 1MB, 1)
  Write-Host ("  -> {0}  ({1} MB)" -f $sdpPath, $sizeMb) -ForegroundColor Green
  $packaged += $sdpPath
}

Write-Host ""
Write-Host ("Packaged {0} plugin(s)." -f $packaged.Count)
Write-Host "Send any .streamDeckPlugin file to a friend; they double-click it to install in Stream Deck."
