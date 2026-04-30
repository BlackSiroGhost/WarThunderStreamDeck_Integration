# War Thunder &times; Stream Deck

> Drive every War Thunder cockpit toggle, weapon trigger, and sensor switch from your Elgato Stream Deck &mdash; without rebinding a single key.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows-blue.svg)](#requirements)
[![Stream Deck SDK](https://img.shields.io/badge/Stream%20Deck-SDK%20v2-orange.svg)](https://docs.elgato.com/sdk)
[![Modules](https://img.shields.io/badge/modules-18-green.svg)](#the-18-modules)

The plugin reads your existing War Thunder `controls.blk`, looks up which keyboard key you bound to each in-game action, and presses that same key when you tap a Stream Deck button. **No new bindings, no rebinding, no losing your muscle memory.**

---

## Highlights

- **18 themed modules** &mdash; install only what you fly. Pure jet pilot? Skip the tank stuff. Heli main? Skip naval.
- **Native file picker** in the Property Inspector. Browse, Test, Save &mdash; done in five seconds.
- **Cross-plugin shared config.** Set the path once; every module you ever install reuses it automatically.
- **Live telemetry** for gear / flaps / air brake (read from `localhost:8111/state`) &mdash; the button shows the real position in&nbsp;%.
- **Fallback keys** (F13&ndash;F24, Pause, ScrollLock) for actions you haven't bound on keyboard yet.
- **Zero conflicts** with your existing keyboard. The plugin presses the *same key WT already knows* &mdash; if your G is "fire primary," tapping the deck just acts like another G.

---

## Quick start

1. Go to [Releases](../../releases/latest) and download the `.streamDeckPlugin` files for the modules you want.
2. **Double-click** any one. Stream Deck installs it.
3. Drag an action onto a key.
4. Property Inspector opens &rarr; **Browse** to your `machine.blk` &rarr; **Test** &rarr; **Save**.
5. Press the button. War Thunder reacts.

> Already have War Thunder at the standard install location? **Auto-detect handles it for free** &mdash; you never see the setup screen.

---

## How it works

```
+-------------+     +------------------+     +-------------------+     +------------------+
| Stream Deck | --> | WT plugin (.exe) | --> | Win32 SendInput   | --> | War Thunder      |
| button tap  |     |                  |     | virtual keypress  |     | (sees real key)  |
+-------------+     +-------+----------+     +-------------------+     +------------------+
                            |
                            | reads
                            v
                    +-----------------+
                    | machine.blk     |    "ID_GEAR -> G"
                    | (your bindings) |
                    +-----------------+
```

1. You configure the path to `machine.blk` once via the Property Inspector.
2. The path lives at `%LocalAppData%\WarThunderStreamDeck\config.json` &mdash; **shared across every module**.
3. When you tap a Stream Deck key, the plugin parses `machine.blk`, finds the keyboard scancode bound to e.g. `ID_GEAR`, and emits it via `SendInput`.
4. War Thunder receives an indistinguishable-from-real-keyboard key event and toggles the gear.

The plugin **never overwrites** your `.blk` and never injects bindings without your input. It is read-only against the controls file.

---

## The 18 modules

Each module ships as its own `.streamDeckPlugin` and lives in its own Stream Deck category. Pick what you need.

### Aircraft

| Module | What's inside | Accent |
|---|---|---|
| **Mechanisation** | Gear, Flaps, Air brake, Air reverse, Bay door, Cockpit door, Tail hook, WEP/Boosters, VTOL | <span style="color:#ffb450">amber</span> |
| **Engine** | Throttle Max/Min, Maneuverability mode | <span style="color:#ff8030">orange</span> |
| **Weapons - Air-to-Air** | Fire Primary/Secondary, Cannons, MGuns, AAM, Lock target, Cycle, Reload | <span style="color:#e05050">red</span> |
| **Weapons - Air-to-Ground** | AGM, ATGM, Bombs, Bomb series, Rockets, Rocket series, GBU drop & lock, Laser designator, Ballistic computers | <span style="color:#c040a0">magenta</span> |
| **Countermeasures** | Flares, IR projector, Smoke screen | <span style="color:#f0d040">yellow</span> |
| **Radar - Air** | Radar power, Mode, Range, Scan pattern, ACM, Target lock/switch, Type switch | <span style="color:#50d070">green</span> |
| **Optics & Pod** | Thermal polarity, Night vision, Designate target, Lock at point, Cue X/Y/Z, Laser, Target/UAV camera | <span style="color:#a070d0">purple</span> |

### Helicopter

| Module | What's inside | Accent |
|---|---|---|
| **Heli Mechanisation** | Heli gear, Flaps Up/Down, Air brake | <span style="color:#40c0a0">teal</span> |
| **Heli Combat** | Fire Primary/Secondary/MGuns/Cannons/Add-guns, Flares (single & series), Rocket series, Ballistic computers, Cycle Primary/Secondary, Instructor, Exit cycle | <span style="color:#d05050">red</span> |
| **Heli Sensors** | Night vision, Seeker camera, Sensor switch & lock, Laser, Target camera, Lock at point, Cue X/Y | <span style="color:#60a0e0">sky-blue</span> |

### Tank

| Module | What's inside | Accent |
|---|---|---|
| **Tank Movement** | Direction driving, Suspension up/down/pitch/roll/reset | <span style="color:#a07050">brown</span> |
| **Tank Combat** | Fire Secondary/Special, Select Primary/Secondary/MG/Reset, Smoke, Repair, Hull aiming, Reload | <span style="color:#d05050">red</span> |
| **Tank Sights** | Rangefinder, Targeting hold, Zoom hold/toggle, Crosshair light, Tank NV, Fuse mode, Thermal, Sight distance +/-/= | <span style="color:#a0e040">lime</span> |
| **Radar - Ground** | Tank sensor switch/mode/range/scan/lock, View switch, Weapon lock, IRCM, APS lock | <span style="color:#88b040">olive</span> |

### Naval

| Module | What's inside | Accent |
|---|---|---|
| **Ship Combat** | Lock target, Ship zoom (in development) | <span style="color:#6090e0">navy</span> |

### Cross-vehicle

| Module | What's inside | Accent |
|---|---|---|
| **View / Camera** | Toggle view, Heli view, Camera neutral/down, Driver, Binoculars, Target/UAV camera, Cam X/Y center | <span style="color:#e080b0">pink</span> |
| **Comms** | Voice messages 1, 2, 5, 7, 8, PTT, Squad list, Squad designate, Support plane, Plane orbit, Wheel menu | <span style="color:#5070d0">blue</span> |
| **HUD & System** | Hide HUD, Pause, Screenshot, Flight menu/setup, MP stats, Instructor, Action bar 5-9, Continue, Control mode (incl. UAV), Shot freq, Exit cycle | <span style="color:#808088">gray</span> |

> Full per-action breakdown: [`CATEGORIES.md`](CATEGORIES.md). Source-of-truth spec: [`tools/spec/modules.psd1`](tools/spec/modules.psd1).

---

## Setup walkthrough

### First plugin you install

1. Drag any action onto a deck key.
2. Property Inspector opens. Top section is **Controls file (.blk)** with a `SHARED` badge.
3. Click **Browse&hellip;** &mdash; a native Windows file picker opens, defaulting to `Documents\My Games\WarThunder\Saves\`.
4. Pick `machine.blk` (or your custom controls preset).
5. Click **Test**. Status flips to green: `OK - 134 bindings loaded from ...`.
6. Click **Save**. The path is written to the shared config; every WT plugin you have or will install picks it up.
7. Tap the deck key. WT reacts.

### Every plugin you install after that

Buttons just work. No PI. No setup. The shared config already knows your path.

### Per-action fallback key (optional)

If a specific WT action isn't bound to any keyboard key in your `.blk`, tell the plugin which spare key to press instead:

1. Select the action's PI.
2. Choose **F13** (or any key from F13&ndash;F24, Pause, ScrollLock).
3. Save.
4. In WT, bind that same key to the matching action.

These keys are curated to be ones no physical keyboard has &mdash; **they collide with nothing.**

---

## Requirements

- **Windows 10/11** (Stream Deck plugin SDK is Windows-first; the plugin uses Win32 `SendInput`).
- **Elgato Stream Deck app** &ge; 6.5.
- **War Thunder** with the in-game **localhost telemetry server** enabled (it's on by default; reachable at `http://localhost:8111/state`).
- A `machine.blk` in your WT user folder (any keyboard binding you've ever set creates it).

The published plugins are **self-contained** &mdash; they include the .NET 9 runtime. No extra installs.

---

## Building from source

You only need this if you're modifying the plugin. End users just download from Releases.

```powershell
# Prereqs: .NET 9 SDK, PowerShell 5.1+, Stream Deck app
git clone https://github.com/BlackSiroGhost/WarThunderStreamDeck_Integration
cd WarThunderStreamDeck_Integration

# 1. Regenerate per-module C# projects + manifests + icons from the spec
./tools/generate.ps1

# 2. Build & install all 18 plugins to your local Stream Deck
./tools/deploy.ps1

# OR: build & pack into shareable .streamDeckPlugin archives
./tools/pack.ps1 -Version 0.5.0   # outputs to dist/
```

### Repository layout

```
WarThunderStreamDeck_Integration/
├── src/
│   ├── WarThunderStreamDeck.Core/           Shared library: BlkParser, key sender, telemetry,
│   │                                         BindingsCache, SharedConfig, WarThunderBindingAction
│   ├── WarThunderStreamDeck.Mechanisation/  Module exe (one per category)
│   ├── WarThunderStreamDeck.Engine/         ...
│   └── ... (16 more)
├── plugin/
│   ├── com.blacksiroghost.wt.mechanisation.sdPlugin/   manifest, icons, PI
│   └── ... (17 more)
├── tools/
│   ├── spec/modules.psd1     ⟵ source-of-truth: every action, every binding, every label
│   ├── generate.ps1          ⟵ codegen: spec → C# + manifests + icons
│   ├── deploy.ps1            ⟵ build + install + restart Stream Deck
│   └── pack.ps1              ⟵ produces .streamDeckPlugin archives
├── dist/                      (generated; gitignored)
├── CATEGORIES.md             Architecture & module taxonomy
├── INSTALL.md                End-user install guide
└── README.md
```

### Adding an action

Edit [`tools/spec/modules.psd1`](tools/spec/modules.psd1) and re-run `tools/generate.ps1`:

```powershell
@{ N='Eject Pilot';   U='eject';   T='Simple';   B=@('ID_EJECT_PILOT');   L='EJECT' }
```

That single line yields: a generated `EjectAction` C# class, an entry in the manifest, a generated PNG button icon with the `EJECT` label rendered at 72/144 px, and a Property Inspector binding. **No copy-paste, no manual JSON, no manual icon work.**

### Adding a module

Add a new `@{ Id=...; Asm=...; Cat=...; Accent=...; Actions=@(...) }` block to `modules.psd1`. The generator handles everything else.

---

## FAQ

**Q: Will my keyboard binds still work?**
Yes. The plugin sends the same key your keyboard sends. Both work in parallel.

**Q: Can two actions fire from one button press?**
Only if you bound two WT actions to the same key in WT itself. The plugin doesn't cause that &mdash; it just sends keys. To guarantee no collisions, use the F13&ndash;F24 fallback keys (one unique per action).

**Q: Does it cheat / read game memory?**
No. It reads the publicly-documented `localhost:8111/state` HTTP endpoint (live telemetry the game itself exposes). No memory access, no DLL injection, no protocol fakery. War Thunder itself ships this server for third-party tools.

**Q: Will Gaijin ban me?**
This is functionally identical to using a streamdeck-style HOTAS profile. The plugin presses real keys. Gaijin permits this category of tooling. **Use at your own discretion;** I take no responsibility for account actions.

**Q: My `.blk` is in a non-standard place.**
Use the **Browse&hellip;** button in the PI. Path resolution priority: shared config &rarr; per-plugin Stream Deck settings &rarr; auto-discover (`Documents\My Games\WarThunder\Saves\last\production\machine.blk`).

**Q: Does it work with custom control presets (`Steuerung.blk` etc.)?**
Yes. Browse to whichever `.blk` is your active loadout. The plugin reparses on file change.

**Q: Mac/Linux support?**
Not currently. Stream Deck SDK + WT both target Windows.

---

## Architecture deep-dive

For module taxonomy, the colour palette, the codegen rationale, and the friend-share UX design: see [`CATEGORIES.md`](CATEGORIES.md).

For the end-user install flow (the version you'd send to a friend along with a `.streamDeckPlugin` file): see [`INSTALL.md`](INSTALL.md).

---

## Contributing

Issues and PRs welcome. The codegen-from-spec design means most additions are a one-line edit to [`tools/spec/modules.psd1`](tools/spec/modules.psd1) followed by `./tools/generate.ps1`.

When opening a PR for a new action:
- Source the action ID from your own `machine.blk` (so you've verified WT actually exposes it).
- Pick a 4-character abbreviation (`L=`) that won't collide with siblings.
- Place it in the most thematically coherent existing module before considering a new one.

---

## License

[MIT](LICENSE) &mdash; do whatever you want, just keep the copyright notice.

War Thunder is a trademark of Gaijin Entertainment. This project is not affiliated with or endorsed by Gaijin.
