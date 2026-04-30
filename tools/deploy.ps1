#requires -Version 5.1
<#
.SYNOPSIS
  Build the WarThunder Stream Deck plugin and install it into Stream Deck.

.DESCRIPTION
  - Publishes the C# project (self-contained, win-x64) into dist\com.blacksiroghost.warthunder.sdPlugin\
  - Copies manifest.json, Images\, and PI\ from plugin\com.blacksiroghost.warthunder.sdPlugin\ next to the binaries
  - Stops the Stream Deck app if running, copies the .sdPlugin folder to %AppData%\Elgato\StreamDeck\Plugins\,
    and restarts Stream Deck.

.PARAMETER Configuration
  Debug or Release. Default: Release.

.PARAMETER NoInstall
  Build only; skip stopping Stream Deck and copying into the Plugins folder.
#>

param(
  [ValidateSet('Debug', 'Release')]
  [string]$Configuration = 'Release',
  [switch]$NoInstall
)

$ErrorActionPreference = 'Stop'

$repoRoot   = Split-Path -Parent $PSScriptRoot
$pluginId   = 'com.blacksiroghost.warthunder.sdPlugin'
$projectDir = Join-Path $repoRoot 'WarThunderStreamDeckPlugin'
$projectFile= Join-Path $projectDir 'WarThunderStreamDeckPlugin.csproj'
$pluginSrc  = Join-Path $repoRoot ('plugin\' + $pluginId)
$distDir    = Join-Path $repoRoot ('dist\' + $pluginId)
$installDir = Join-Path $env:APPDATA ('Elgato\StreamDeck\Plugins\' + $pluginId)

Write-Host "Repo root: $repoRoot"
Write-Host "Configuration: $Configuration"

if (Test-Path $distDir) { Remove-Item $distDir -Recurse -Force }
$null = New-Item $distDir -ItemType Directory -Force

Write-Host "Publishing project..."
& dotnet publish $projectFile -c $Configuration -r win-x64 --self-contained true `
  -p:PublishSingleFile=false -p:DebugType=embedded `
  -o $distDir | Out-Host
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed ($LASTEXITCODE)" }

Write-Host "Copying plugin assets (manifest, images, PI)..."
Copy-Item (Join-Path $pluginSrc 'manifest.json') $distDir -Force
Copy-Item (Join-Path $pluginSrc 'Images') (Join-Path $distDir 'Images') -Recurse -Force
Copy-Item (Join-Path $pluginSrc 'PI')     (Join-Path $distDir 'PI')     -Recurse -Force

Write-Host "Build artifact: $distDir"

if ($NoInstall) {
  Write-Host "NoInstall set; stopping here."
  return
}

Write-Host "Stopping Stream Deck..."
Get-Process -Name 'StreamDeck' -ErrorAction SilentlyContinue | ForEach-Object {
  $_ | Stop-Process -Force
}
Start-Sleep -Milliseconds 500

if (Test-Path $installDir) {
  Write-Host "Removing previous install at $installDir"
  Remove-Item $installDir -Recurse -Force
}

$null = New-Item (Split-Path $installDir -Parent) -ItemType Directory -Force
Copy-Item $distDir $installDir -Recurse -Force
Write-Host "Installed to $installDir"

$streamDeckExe = Join-Path $env:ProgramFiles 'Elgato\StreamDeck\StreamDeck.exe'
if (Test-Path $streamDeckExe) {
  Write-Host "Starting Stream Deck..."
  Start-Process $streamDeckExe
} else {
  Write-Host "Stream Deck.exe not found at $streamDeckExe - start it manually."
}

Write-Host "Done."
