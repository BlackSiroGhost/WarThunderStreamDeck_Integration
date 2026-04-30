# WarThunderStreamDeck_Integration

Stream Deck plugin that drives War Thunder cockpit toggles (gear, flaps, ...) by reading
your existing controls `.blk` and pressing the same key you've already bound in-game.

## What it does

- You point the plugin at your War Thunder `controls.blk` once (Property Inspector → paste path → Save).
- The plugin parses the `hotkeys{}` section, looks up the action ID (e.g. `ID_GEAR`), and sends the bound
  scancode via Win32 `SendInput`.
- The button icon flips between two states based on `/state` telemetry from the in-game localhost server
  (`http://localhost:8111/state`).

This release ships two actions: **Toggle Gear** and **Toggle Flaps**. More follow the same template.

## Layout

```
WarThunderStreamDeck.sln                 <- solution at repo root
WarThunderStreamDeckPlugin/              <- C# source
  Actions/                               <- WarThunderBindingAction + GearAction + FlapsAction
  KeyBindings/                           <- BlkParser + BindingMap
  Input/                                 <- SendInput keyboard, DIK scan codes
  Telemetry/                             <- WT /state JSON client
  Settings/                              <- global settings + bindings cache
plugin/com.blacksiroghost.warthunder.sdPlugin/
  manifest.json                          <- Stream Deck SDK v2 manifest
  Images/                                <- icon assets
  PI/controls.html                       <- Property Inspector
tools/deploy.ps1                         <- build + install + restart Stream Deck
dist/                                    <- (generated) deployable plugin folder
```

## Build / install

```powershell
# from repo root, in PowerShell
./tools/deploy.ps1                 # Release publish + install + restart Stream Deck
./tools/deploy.ps1 -NoInstall      # build only into dist\
./tools/deploy.ps1 -Configuration Debug
```

After install, Stream Deck will pick the plugin up from
`%AppData%\Elgato\StreamDeck\Plugins\com.blacksiroghost.warthunder.sdPlugin\`.

## First-run setup

1. Drag **Toggle Gear** (or **Toggle Flaps**) from the *War Thunder* category onto a key.
2. In the Property Inspector, paste the full path to your `controls.blk`
   (typically `%USERPROFILE%\Documents\My Games\WarThunder\Config\controls.blk`,
    or any saved preset like the bundled
    `WarThunderStreamDeckPlugin/Neue Steuerung mit VKB Stick (PC).blk`).
3. Hit **Save & reload bindings**.
4. Press the key. The plugin presses the keyboard scancode bound to `ID_GEAR` /
   `ID_FLAPS` (with `ID_FLAPS_DOWN` / `ID_FLAPS_UP` as fallbacks).

## Adding a new action

A new action is ~10 lines:

```csharp
[StreamDeckAction("com.blacksiroghost.warthunder.airbrake")]
public sealed class AirBrakeAction : WarThunderBindingAction
{
    protected override string PrimaryBindingId => "ID_AIR_BRAKE";

    protected override Task<int?> ProbeStateAsync(IWarThunderTelemetry t)
        => Task.FromResult<int?>(null); // no telemetry, single-state icon
}
```

Then add a corresponding entry to `manifest.json`.

## Sharing with friends

Build `.streamDeckPlugin` distributables (one per module folder under `plugin/`):

```powershell
./tools/pack.ps1 -Version 0.3.0
```

Each output file in `dist/` is a self-contained installer. A friend downloads
the file, **double-clicks it**, and Stream Deck loads the module — no admin
rights, no extra runtime install. See [INSTALL.md](INSTALL.md) for the
end-user setup flow.

## Module roadmap

The eventual structure is one `.sdPlugin` per system category (Mechanisation,
Radar - Air, Radar - Ground, Weapons - AA, etc.) — friends install only the
modules they fly. See [CATEGORIES.md](CATEGORIES.md) for the full module
taxonomy, icon system, and architecture plan.

## Status

Pre-alpha. Designed for personal use; current single plugin contains gear,
flaps and air brake under "Mechanisation". Splitting into per-module
extensions is in progress.
