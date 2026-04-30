#requires -Version 5.1
<#
.SYNOPSIS
  Build and (optionally) install every War Thunder Stream Deck plugin under plugin/.

.DESCRIPTION
  Iterates over each plugin/*.sdPlugin folder. For each one:
   - Reads manifest.json to find the assembly name (CodePath without .exe).
   - Publishes the matching src/<AssemblyName>/ project into dist/<id>.sdPlugin/.
   - Copies manifest.json, Images/, PI/ alongside the published binaries.
   - Optionally installs into %APPDATA%\Elgato\StreamDeck\Plugins\ and restarts Stream Deck.

.PARAMETER Configuration
  Debug or Release. Default: Release.

.PARAMETER NoInstall
  Build only; skip stopping Stream Deck and copying into the Plugins folder.

.PARAMETER Module
  Optional substring filter on plugin folder name. Default: install every plugin.
  Example: -Module mechanisation
#>

param(
  [ValidateSet('Debug', 'Release')]
  [string]$Configuration = 'Release',
  [switch]$NoInstall,
  [string]$Module = ''
)

$ErrorActionPreference = 'Stop'

$repoRoot   = Split-Path -Parent $PSScriptRoot
$pluginsDir = Join-Path $repoRoot 'plugin'
$srcRoot    = Join-Path $repoRoot 'src'
$distRoot   = Join-Path $repoRoot 'dist'
$installRoot= Join-Path $env:APPDATA 'Elgato\StreamDeck\Plugins'

if (-not (Test-Path $distRoot)) { $null = New-Item $distRoot -ItemType Directory -Force }

$plugins = Get-ChildItem $pluginsDir -Directory -Filter '*.sdPlugin'
if ($Module) {
  $plugins = $plugins | Where-Object { $_.Name -like "*$Module*" }
}
if ($plugins.Count -eq 0) {
  Write-Warning "No matching plugin folders under $pluginsDir"
  return
}

Write-Host "==> Targeting $($plugins.Count) plugin(s):"
$plugins | ForEach-Object { Write-Host "      $($_.Name)" }
Write-Host ""

# Stop Stream Deck once (if installing)
if (-not $NoInstall) {
  Write-Host "==> Stopping Stream Deck..."
  Get-Process -Name 'StreamDeck' -ErrorAction SilentlyContinue | Stop-Process -Force
  Start-Sleep -Milliseconds 500
}

$built = @()
foreach ($plugin in $plugins) {
  $pluginId = $plugin.Name
  Write-Host ""
  Write-Host "==> $pluginId" -ForegroundColor Cyan

  $manifest = Get-Content (Join-Path $plugin.FullName 'manifest.json') -Raw | ConvertFrom-Json
  $exeName  = $manifest.CodePathWin
  if (-not $exeName) { $exeName = $manifest.CodePath }
  $asmName  = $exeName.Replace('.exe', '')
  $projDir  = Join-Path $srcRoot $asmName
  $proj     = Join-Path $projDir "$asmName.csproj"

  if (-not (Test-Path $proj)) {
    Write-Warning "  csproj not found: $proj - skipping"
    continue
  }

  $distOut = Join-Path $distRoot $pluginId
  if (Test-Path $distOut) { Remove-Item $distOut -Recurse -Force }

  Write-Host "    publishing $asmName..."
  & dotnet publish $proj -c $Configuration -r win-x64 --self-contained true `
    -p:PublishSingleFile=false -p:DebugType=embedded `
    -o $distOut --nologo -v q | Out-Host
  if ($LASTEXITCODE -ne 0) { throw "publish failed for $proj ($LASTEXITCODE)" }

  Write-Host "    copying plugin assets..."
  Copy-Item (Join-Path $plugin.FullName 'manifest.json') $distOut -Force
  Copy-Item (Join-Path $plugin.FullName 'Images') (Join-Path $distOut 'Images') -Recurse -Force
  Copy-Item (Join-Path $plugin.FullName 'PI')     (Join-Path $distOut 'PI')     -Recurse -Force

  $built += [pscustomobject]@{ Id = $pluginId; Dist = $distOut }
}

if ($NoInstall) {
  Write-Host ""
  Write-Host "==> NoInstall set; built $($built.Count) plugin(s) to dist\."
  return
}

# Install all built plugins
Write-Host ""
Write-Host "==> Installing $($built.Count) plugin(s)..."
foreach ($b in $built) {
  $target = Join-Path $installRoot $b.Id
  if (Test-Path $target) { Remove-Item $target -Recurse -Force }
  $null = New-Item (Split-Path $target -Parent) -ItemType Directory -Force
  Copy-Item $b.Dist $target -Recurse -Force
  Write-Host "    -> $target" -ForegroundColor Green
}

# Restart Stream Deck
$streamDeckExe = Join-Path $env:ProgramFiles 'Elgato\StreamDeck\StreamDeck.exe'
if (Test-Path $streamDeckExe) {
  Write-Host ""
  Write-Host "==> Starting Stream Deck..."
  Start-Process $streamDeckExe
} else {
  Write-Host "Stream Deck.exe not found - start it manually."
}

Write-Host ""
Write-Host "Done. $($built.Count) plugin(s) installed."
