#requires -Version 5.1
<#
.SYNOPSIS
  Generate the README module-summary SVG banners from tools/spec/modules.psd1.
  One banner per module, written to docs/banners/<module-id>.svg.
#>

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$specPath = Join-Path $PSScriptRoot 'spec\modules.psd1'
$outDir   = Join-Path $repoRoot 'docs\banners'

if (-not (Test-Path $outDir)) { $null = New-Item $outDir -ItemType Directory -Force }

# Display metadata: pretty name + one-line description per module id.
$meta = @{
    'mechanisation'   = @{ Name = 'Mechanisation';        Desc = 'Gear, flaps, air brake, bay & cockpit doors, tail hook, WEP, VTOL.' }
    'engine'          = @{ Name = 'Engine';               Desc = 'Throttle Max / Min, maneuverability mode.' }
    'weapons.aa'      = @{ Name = 'Weapons - Air-to-Air'; Desc = 'Missiles, cannons, MGuns, target lock, fire cycle, reload, fire-axis.' }
    'weapons.ag'      = @{ Name = 'Weapons - Air-to-Ground'; Desc = 'AGM, ATGM, bombs, rockets, GBU, laser designator, ballistic computers.' }
    'countermeasures' = @{ Name = 'Countermeasures';      Desc = 'Flares, IR projector, smoke screen.' }
    'radar.air'       = @{ Name = 'Radar - Air';          Desc = 'Power, mode, range, scan pattern, ACM, target lock & switch, type.' }
    'optics.air'      = @{ Name = 'Optics & Pod';         Desc = 'Thermal, NV, designate, lock at point, cue X / Y / Z, laser, cameras.' }
    'heli.mech'       = @{ Name = 'Heli Mechanisation';   Desc = 'Helicopter gear, flaps up / down, air brake.' }
    'heli.combat'     = @{ Name = 'Heli Combat';          Desc = 'Fire all weapons, flares, rocket series, BC, cycle, instructor.' }
    'heli.sensors'    = @{ Name = 'Heli Sensors';         Desc = 'Night vision, seeker camera, sensor lock, laser, target cam, cue X / Y.' }
    'tank.movement'   = @{ Name = 'Tank Movement';        Desc = 'Direction driving, suspension clearance / pitch / roll, reset.' }
    'tank.combat'     = @{ Name = 'Tank Combat';          Desc = 'Fire secondary & special, gun select, smoke, repair, hull aim, reload.' }
    'tank.sights'     = @{ Name = 'Tank Sights';          Desc = 'Rangefinder, zoom, NV, thermal, fuse, sight distance + / - / set.' }
    'radar.ground'    = @{ Name = 'Radar - Ground';       Desc = 'Tank radar power / mode / range / scan / lock, IRCM, APS lock.' }
    'ship.combat'     = @{ Name = 'Ship Combat';          Desc = 'Lock target, ship zoom max. Naval module is in early development.' }
    'view'            = @{ Name = 'View / Camera';        Desc = 'Toggle view, neutral, driver, binoculars, target & UAV cameras, X / Y center.' }
    'coms'            = @{ Name = 'Comms';                Desc = 'Voice messages, PTT, squad voice list, designate, support plane, wheel menu.' }
    'hud'             = @{ Name = 'HUD & System';         Desc = 'Hide HUD, pause, screenshot, flight menu, MP stats, action bars, control mode.' }
}

$spec = Import-PowerShellDataFile $specPath

function Encode-Xml($s) {
    $s -replace '&', '&amp;' -replace '<', '&lt;' -replace '>', '&gt;'
}

# Lighten an RGB triple for the gradient highlight.
function Lighten($rgb, [double]$f) {
    @(
        [Math]::Min(255, [int]($rgb[0] + (255 - $rgb[0]) * $f)),
        [Math]::Min(255, [int]($rgb[1] + (255 - $rgb[1]) * $f)),
        [Math]::Min(255, [int]($rgb[2] + (255 - $rgb[2]) * $f))
    )
}

foreach ($m in $spec.Modules) {
    $info = $meta[$m.Id]
    if (-not $info) {
        Write-Warning "No banner metadata for $($m.Id) - skipping"
        continue
    }

    $name  = Encode-Xml $info.Name
    $desc  = Encode-Xml $info.Desc
    $count = $m.Actions.Count
    $accent = '#{0:X2}{1:X2}{2:X2}' -f $m.Accent[0], $m.Accent[1], $m.Accent[2]
    $accentLight = Lighten $m.Accent 0.25
    $accentLightHex = '#{0:X2}{1:X2}{2:X2}' -f $accentLight[0], $accentLight[1], $accentLight[2]

    $w = 760
    $h = 88

    $svg = @"
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 $w $h" width="$w" height="$h" role="img" aria-label="$name - $count actions">
  <defs>
    <linearGradient id="bg" x1="0" y1="0" x2="0" y2="1">
      <stop offset="0" stop-color="#2a2a2e"/>
      <stop offset="1" stop-color="#15151a"/>
    </linearGradient>
    <linearGradient id="stripe" x1="0" y1="0" x2="0" y2="1">
      <stop offset="0" stop-color="$accentLightHex"/>
      <stop offset="1" stop-color="$accent"/>
    </linearGradient>
  </defs>
  <rect width="$w" height="$h" rx="12" fill="url(#bg)"/>
  <rect width="$w" height="$h" rx="12" fill="none" stroke="#3a3a40" stroke-width="1"/>
  <rect x="0" y="0" width="8" height="$h" rx="0" fill="url(#stripe)"/>
  <rect x="0" y="0" width="12" height="$h" fill="url(#stripe)" opacity="0.0"/>
  <text x="32" y="38" font-family="-apple-system, Segoe UI, system-ui, sans-serif" font-weight="700" font-size="22" fill="#ffffff" letter-spacing="0.2">$name</text>
  <text x="32" y="62" font-family="-apple-system, Segoe UI, system-ui, sans-serif" font-weight="400" font-size="13" fill="#9ca0a6">$desc</text>
  <text x="$($w - 28)" y="40" text-anchor="end" font-family="-apple-system, Segoe UI, system-ui, sans-serif" font-weight="700" font-size="34" fill="$accent">$count</text>
  <text x="$($w - 28)" y="60" text-anchor="end" font-family="-apple-system, Segoe UI, system-ui, sans-serif" font-weight="600" font-size="11" fill="#9ca0a6" letter-spacing="1.5">ACTIONS</text>
</svg>
"@

    $path = Join-Path $outDir "$($m.Id).svg"
    Set-Content -LiteralPath $path -Value $svg -Encoding UTF8
    Write-Host "  wrote $($m.Id).svg ($name, $count actions)"
}

Write-Host ""
Write-Host "Banners written to $outDir" -ForegroundColor Green
