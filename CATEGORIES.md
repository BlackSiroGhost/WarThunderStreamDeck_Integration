# War Thunder Stream Deck — module categorisation

This is the design plan for breaking the plugin into **per-category installable extensions**.
Each row in the table below becomes one Stream Deck plugin (one `.sdPlugin` folder, one
exe, one entry in your Stream Deck's plugin list). All of them share a single
`WarThunderStreamDeck.Core` class library; the per-module exe only contains the action
classes that belong to that module.

---

## 1. Architectural shape

```
WarThunderStreamDeck_Integration/
├── WarThunderStreamDeck.sln
├── src/
│   ├── WarThunderStreamDeck.Core/                  ← shared class library
│   │   ├── Actions/WarThunderBindingAction.cs      ← base class (worker queue, polling)
│   │   ├── Input/, KeyBindings/, Settings/,
│   │   ├── Telemetry/, Diagnostics/
│   │   └── (no Program.cs — never built standalone)
│   ├── Mechanisation/                              ← per-module exe project
│   │   ├── Program.cs, GearAction.cs, FlapsAction.cs, AirBrakeAction.cs, ...
│   │   └── WarThunderStreamDeck.Mechanisation.csproj  (refs Core)
│   ├── RadarAir/
│   │   └── ...
│   ├── RadarGround/
│   └── ...
├── plugin/
│   ├── com.blacksiroghost.wt.mechanisation.sdPlugin/
│   │   ├── manifest.json (only mechanisation actions)
│   │   ├── Images/, PI/
│   ├── com.blacksiroghost.wt.radar.air.sdPlugin/
│   └── ...
└── tools/
    └── deploy.ps1 -Module Mechanisation|RadarAir|All
```

### Shared `.blk` path across plugins

Each plugin process is independent (Stream Deck launches one exe per `.sdPlugin`).
To avoid making the user paste the controls path into every plugin's PI, the path
moves out of Stream Deck's per-plugin global settings into a **shared config file**:

```
%LocalAppData%\WarThunderStreamDeck\config.json
```

`Core.Settings.SharedConfig` reads/writes this file. Any plugin's PI writes to it;
all plugins watch it (via `FileSystemWatcher`) and reload `BindingsCache` when it
changes. Set the path once, every installed module sees it.

### Versioning

`Core` is versioned independently. Plugins reference Core by NuGet (publish locally) or
by ProjectReference. Each plugin's `manifest.json` carries its own version. Bumping
Core does not require bumping every plugin — only those that adopt new APIs.

---

## 2. The module taxonomy

Categorisation principle: **by system function, not by vehicle class**. Vehicle-specific
sub-systems get their own modules (e.g. helicopter collective is its own module from
aircraft mechanisation), so the user installs only what they fly.

### 🛩 Aircraft modules

| # | Module                      | Plugin UUID                                   | What's in it                                                           | Telemetry feeds                                |
|---|-----------------------------|-----------------------------------------------|-------------------------------------------------------------------------|------------------------------------------------|
| 1 | **Mechanisation**           | `com.blacksiroghost.wt.mechanisation`         | `ID_GEAR`, `ID_FLAPS{,_UP,_DOWN}`, `ID_AIR_BRAKE`, `ID_AIR_REVERSE`, `ID_BAY_DOOR`, `ID_TOGGLE_COCKPIT_DOOR`, `ID_IGNITE_BOOSTERS`, `vtol_rangeMin/Max` | `gear, %`, `flaps, %`, `airbrake, %`           |
| 2 | **Engine**                  | `com.blacksiroghost.wt.engine`                | `throttle_rangeMax/Min`, `ID_MANEUVERABILITY_MODE`                                                                                                       | `throttle N, %`, `RPM N`, `oil temp N, C`, `manifold pressure N, atm`, `thrust N, kgs`, `Mfuel, kg` |
| 3 | **Weapons – Air-to-Air**    | `com.blacksiroghost.wt.weapons.aa`            | `ID_AAM`, `ID_FIRE_CANNONS`, `ID_FIRE_MGUNS`, `ID_FIRE_PRIMARY`, `ID_FIRE_SECONDARY`, `ID_FIRE_ADDITIONAL_GUNS`, `ID_LOCK_TARGET`, `ID_WEAPON_LOCK`, `ID_SWITCH_SHOOTING_CYCLE_PRIMARY/SECONDARY`, `ID_RELOAD_GUNS` | (none reliable — fire status not exposed) |
| 4 | **Weapons – Air-to-Ground** | `com.blacksiroghost.wt.weapons.ag`            | `ID_AGM`, `ID_AGM_LOCK`, `ID_ATGM`, `ID_BOMBS`, `ID_BOMBS_SERIES`, `ID_ROCKETS`, `ID_GUIDED_BOMBS_LOCK`, `ID_TOGGLE_LASER_DESIGNATOR`, `ID_TOGGLE_ROCKETS_BALLISTIC_COMPUTER`, `ID_TOGGLE_CANNONS_AND_ROCKETS_BALLISTIC_COMPUTER` | `weapon2`, `weapon4` (suspension counts in `/indicators`) |
| 5 | **Countermeasures**         | `com.blacksiroghost.wt.countermeasures`       | `ID_FLARES`, `ID_IR_PROJECTOR`, `ID_SMOKE_SCREEN`                                                                                                       | (none)                                          |
| 6 | **Radar – Air**             | `com.blacksiroghost.wt.radar.air`             | `ID_SENSOR_SWITCH`, `ID_SENSOR_MODE_SWITCH`, `ID_SENSOR_RANGE_SWITCH`, `ID_SENSOR_SCAN_PATTERN_SWITCH`, `ID_SENSOR_ACM_SWITCH`, `ID_SENSOR_TARGET_LOCK`, `ID_SENSOR_TARGET_SWITCH`, `ID_SENSOR_TYPE_SWITCH`, `ID_LOCK_TARGETING`, `ID_UNLOCK_TARGETING` | (radar mode not exposed; user reads icon)      |
| 7 | **Optics & Pod**            | `com.blacksiroghost.wt.optics.air`            | `ID_THERMAL_WHITE_IS_HOT`, `ID_PLANE_NIGHT_VISION`, `ID_DESIGNATE_TARGET`, `ID_LOCK_TARGETING_AT_POINT`, `ID_UNLOCK_TARGETING_AT_POINT`, `sensor_cue_x/y/z_rangeMax/Min/Set` | (none reliable)                                 |

### 🚁 Helicopter modules

| #  | Module                       | Plugin UUID                              | What's in it                                                                                                                                                                                  |
|----|------------------------------|------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 8  | **Helicopter Mechanisation** | `com.blacksiroghost.wt.heli.mech`        | `ID_GEAR_HELICOPTER`, `ID_FLAPS_UP_HELICOPTER`, `ID_FLAPS_DOWN_HELICOPTER`, `ID_AIR_BRAKE_HELICOPTER`, `helicopter_collective_rangeMax/Min`, `helicopter_pedals_rangeMax/Min`, `helicopter_cyclic_pitch_rangeMax/Min`, `helicopter_cyclic_roll_rangeMax/Min` |
| 9  | **Helicopter Combat**        | `com.blacksiroghost.wt.heli.combat`      | `ID_FIRE_PRIMARY_HELICOPTER`, `ID_FIRE_SECONDARY_HELICOPTER`, `ID_FIRE_MGUNS_HELICOPTER`, `ID_FIRE_CANNONS_HELICOPTER`, `ID_FIRE_ADDITIONAL_GUNS_HELICOPTER`, `ID_FLARES_SERIES_HELICOPTER`, `ID_TOGGLE_ROCKETS_BALLISTIC_COMPUTER_HELICOPTER`, `ID_TOGGLE_CANNONS_AND_ROCKETS_BALLISTIC_COMPUTER_HELICOPTER`, `ID_SWITCH_SHOOTING_CYCLE_PRIMARY/SECONDARY_HELICOPTER`, `ID_TOGGLE_INSTRUCTOR_HELICOPTER` |
| 10 | **Helicopter Sensors**       | `com.blacksiroghost.wt.heli.sensors`     | `ID_HELI_GUNNER_NIGHT_VISION`, `ID_CAMERA_SEEKER_HELICOPTER`, `ID_SENSOR_SWITCH_HELICOPTER`, `ID_SENSOR_TARGET_LOCK_HELICOPTER`, `ID_TOGGLE_LASER_DESIGNATOR_HELICOPTER`, `ID_TARGET_CAMERA_HELICOPTER`, `helicopter_sensor_cue_x/y_rangeMax/Min` |

### 🛡 Tank modules

| #  | Module                       | Plugin UUID                              | What's in it                                                                                                                                                                                                                          |
|----|------------------------------|------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 11 | **Tank Movement**            | `com.blacksiroghost.wt.tank.movement`    | `ID_ENABLE_GM_DIRECTION_DRIVING`, `ID_SUSPENSION_CLEARANCE_UP/DOWN`, `ID_SUSPENSION_PITCH_UP/DOWN`, `ID_SUSPENSION_ROLL_UP/DOWN`, `ID_SUSPENSION_RESET`, `gm_steering_rangeMax/Min`, `gm_throttle_rangeMax/Min`                          |
| 12 | **Tank Combat**              | `com.blacksiroghost.wt.tank.combat`      | `ID_FIRE_GM_SECONDARY_GUN`, `ID_FIRE_GM_SPECIAL_GUN`, `ID_SELECT_GM_GUN_PRIMARY/SECONDARY/MACHINEGUN/RESET`, `ID_SMOKE_SCREEN_GENERATOR`, `ID_REPAIR_TANK`, `ID_ENABLE_GM_HULL_AIMING`                                                  |
| 13 | **Tank Sights**              | `com.blacksiroghost.wt.tank.sights`      | `ID_RANGEFINDER`, `ID_TARGETING_HOLD_GM`, `ID_ZOOM_HOLD_GM`, `ID_ZOOM_TOGGLE`, `ID_TOGGLE_GM_CROSSHAIR_LIGHTING`, `ID_TANK_NIGHT_VISION`, `ID_TANK_SWITCH_FUSE_MODE`, `ID_THERMAL_WHITE_IS_HOT`, `gm_sight_distance_rangeMax/Min/Set` |
| 14 | **Radar – Ground**           | `com.blacksiroghost.wt.radar.ground`     | `ID_SENSOR_SWITCH_TANK`, `ID_SENSOR_MODE_SWITCH_TANK`, `ID_SENSOR_RANGE_SWITCH_TANK`, `ID_SENSOR_SCAN_PATTERN_SWITCH_TANK`, `ID_SENSOR_TARGET_LOCK_TANK`, `ID_SENSOR_TARGET_SWITCH_TANK`, `ID_SENSOR_VIEW_SWITCH_TANK`, `ID_WEAPON_LOCK_TANK`, `ID_IRCM_SWITCH_TANK`, `ID_LOCK_TARGETING_AT_POINT_SHIP` (misclassified by Gaijin — actually used by APS-equipped tanks) |

### ⚓ Ship / sub modules

| # | Module                        | Plugin UUID                              | What's in it (placeholder — needs more research with naval gameplay)                          |
|---|-------------------------------|------------------------------------------|-----------------------------------------------------------------------------------------------|
| 15 | **Ship Combat**              | `com.blacksiroghost.wt.ship.combat`      | (TBD — main battery, secondary, AA, torpedoes, depth charges; many controls are axis-based)   |

### 🎮 Cross-vehicle modules

| #  | Module                  | Plugin UUID                            | What's in it                                                                                                                                                                                              |
|----|-------------------------|----------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 16 | **View & Camera**       | `com.blacksiroghost.wt.view`           | `ID_TOGGLE_VIEW`, `ID_TOGGLE_VIEW_HELICOPTER`, `ID_CAMERA_NEUTRAL`, `ID_CAMERA_VIEW_DOWN`, `ID_CAMERA_DRIVER`, `ID_CAMERA_BINOCULARS`, `ID_TARGET_CAMERA`, `ID_TOGGLE_UAV_CAMERA`, `camx/y_rangeSet`     |
| 17 | **Multiplayer & Coms**  | `com.blacksiroghost.wt.coms`           | `ID_VOICE_MESSAGE_2`, `ID_VOICE_MESSAGE_7`, `ID_PTT`, `ID_SHOW_VOICE_MESSAGE_LIST_SQUAD`, `ID_SQUAD_TARGET_DESIGNATION`, `ID_START_SUPPORT_PLANE`, `ID_SUPPORT_PLANE_ORBITING`, `ID_SHOW_MULTIFUNC_WHEEL_MENU` |
| 18 | **HUD & System**        | `com.blacksiroghost.wt.hud`            | `ID_HIDE_HUD`, `ID_GAME_PAUSE`, `ID_SCREENSHOT`, `ID_FLIGHTMENU`, `ID_FLIGHTMENU_SETUP`, `ID_MPSTATSCREEN`, `ID_TOGGLE_INSTRUCTOR`, `ID_ACTION_BAR_ITEM_5/6/7/8/9`, `ID_CONTINUE`, `ID_CONTROL_MODE`, `ID_CHANGE_SHOT_FREQ` |

---

## 3. Why this split

- **Install cost is small** for the user. A jet pilot installs Mechanisation + Radar Air +
  Weapons AA + Countermeasures + Optics; doesn't need any tank or naval plugin polluting
  the action list. A tanker installs Tank Movement + Tank Combat + Tank Sights and ignores
  flight stuff.
- **Stream Deck "Categories" panel stays clean.** Each plugin shows up as its own category in the
  right-hand sidebar; you don't have to scroll past 80 actions to find the four you use.
- **Per-module versions and updates.** Add a new radar mode binding in WT? Bump only Radar Air.
  Other modules don't need to ship a release.
- **Per-module UX policy.** Mechanisation cycles use next-press preview (you already chose this);
  Weapons probably want fire-on-press with no state preview; Sensors want a status/title focus.
  Each module is free to make these decisions independently.

## 4. Implementation phases

| Phase | Outcome                                                      |
|-------|--------------------------------------------------------------|
| 1 (next turn) | Solution split: `Core` class library + `Mechanisation` exe project. Existing `Gear`/`Flaps` move to Mechanisation. Plugin folder renamed to `com.blacksiroghost.wt.mechanisation.sdPlugin`. `deploy.ps1` takes `-Module Mechanisation`. Verify install still works. |
| 2 | Add `WarThunderStreamDeck.RadarAir` + plugin folder. Two simple actions inside (`SensorSwitch`, `TargetLock`) to prove two extensions coexist with the shared config file. |
| 3 | Add `Weapons.Aa`, `Countermeasures`, `Optics.Air`. Aircraft baseline complete. |
| 4 | Helicopter triple (`Heli.Mech`, `Heli.Combat`, `Heli.Sensors`). |
| 5 | Tank quartet. |
| 6 | Ship + naval research. |
| 7 | Cross-vehicle (`View`, `Coms`, `Hud`). |

## 5. Naming convention

- Plugin UUID: `com.blacksiroghost.wt.<module>` (lowercase, dot-separated)
- Action UUID: `com.blacksiroghost.wt.<module>.<action>`
  e.g. `com.blacksiroghost.wt.mechanisation.gear`, `com.blacksiroghost.wt.radar.air.scan_mode`
- Csproj: `WarThunderStreamDeck.<Module>` (PascalCase, no `wt.` prefix in code)
- Display name in manifest: human-readable, can be German or English (`Mechanisation`, `Radar - Luft`)

## 6. Icon system

Stream Deck plugins use **three** sizes of art, each with different rules
([Elgato Style Guide](https://docs.elgato.com/sdk/plugins/style-guide)):

| Use | File size (1x / 2x) | Format | Rules |
|---|---|---|---|
| **Action icon** (left-panel listing) | 20 × 20 / 40 × 40 | SVG preferred, PNG ok | **Monochromatic white silhouette**, transparent background. Stream Deck auto-tints. No solid bg. |
| **Key image** (the actual button on the deck) | 72 × 72 / 144 × 144 | PNG | Full colour OK. Must read at 72 px. Two states common. |
| **Plugin icon** (settings list) | 256 × 256 / 512 × 512 | PNG | Square, the plugin's brand mark. |
| **Category icon** (right-side filter) | 28 × 28 / 56 × 56 | SVG/PNG | Monochrome white silhouette. |

### Module colour palette (key images)

Each module uses a consistent accent so users glance at the deck and know
which system they're touching:

| Module | Primary accent | Secondary | Notes |
|---|---|---|---|
| Mechanisation | Amber `#FFB450` (extend) | Cyan `#78C8E6` (retract) | next-press preview semantic |
| Engine | Orange `#FF8030` | — | heat / power |
| Weapons - AA | Red `#E04050` | — | fire / lock |
| Weapons - AG | Magenta `#C040A0` | — | distinct from AA |
| Countermeasures | Yellow `#F0D040` | — | caution / decoys |
| Radar - Air | Green `#50D070` | — | sensor active |
| Radar - Ground | Olive `#88B040` | — | distinct from air |
| Optics & Pod | Purple `#A070D0` | — | vision systems |
| Helicopter | Teal `#40C0A0` | — | distinguishes from fixed-wing |
| Tank Movement | Brown `#A07050` | — | armour |
| Tank Combat | Dark red `#A03030` | — | gunnery |
| Tank Sights | Lime `#A0E040` | — | optics-on-armour |
| View / Camera | Pink `#E080B0` | — | perspective |
| Comms | Blue `#5070D0` | — | network / squad |
| HUD / System | Gray `#808088` | — | utility |

State coding inside a module: amber/yellow = will-deploy, cyan/blue = will-retract,
green = active/locked, red = warning/alert, gray = neutral.

### Sourcing silhouettes

The current generation uses procedurally-drawn GDI primitives (gear strut + tire,
airfoil + flap, fuselage + airbrake panel). For production, swap these for licensed
silhouettes from:

- [game-icons.net](https://game-icons.net) — CC-BY 3.0; large aviation/military set
- [Noun Project](https://thenounproject.com/browse/icons/term/aviation/) — free with attribution
- [flaticon.com](https://www.flaticon.com/free-icons/landing-gear) — 547 landing-gear icons
- [icons8.com](https://icons8.com/icons/set/aviation) — free with attribution

Action-list icons (20 × 20) should be reduced to a single white silhouette
on transparent background; key images can keep colour and add the next-press
arrow + label.

## 7. Distribution

Each module ships as a `.streamDeckPlugin` file (a renamed zip of the
`.sdPlugin` directory; format is identical for SDK v2). Build with:

```powershell
./tools/pack.ps1 -Version 0.3.0
```

This produces `dist\<plugin-id>-v<Version>.streamDeckPlugin`. Friends download
the file, double-click, and Stream Deck loads it. No installer, no admin rights.

For the eventual multi-module rollout, `pack.ps1` iterates over every
`plugin/*.sdPlugin` folder and produces one `.streamDeckPlugin` per module.
Users install only the modules they fly.

## 8. First-run setup UX

Stream Deck plugins **cannot** show a modal pop-up on install — the SDK
exposes no programmatic way to force the Property Inspector open. The
idiomatic substitute is a *setup-needed visual state* on the action button:

- When `BindingsCache.CurrentPath` is `null`, every action sets its title to
  `SETUP\n<TITLE>` (e.g. `SETUP\nGEAR`) on each poll tick.
- The user clicks the button → Stream Deck selects it → PI appears below.
- PI presents a clean two-section setup wizard: (1) `.blk` path with
  auto-detect; (2) per-action fallback key dropdown.
- Once a path is saved, the title flips to live telemetry (`GEAR\n0%`).

## 9. Unbound `.blk` IDs

Many WT actions have no keyboard binding in a typical user's `.blk` —
either empty (`ID_FLAPS{}`) or joystick-only (`ID_BAY_DOOR{ joyButton:i=7 }`).
For those, the manifest still lists the action so it appears on the deck,
and the per-action **fallback key** mechanism kicks in:

1. Plugin reads action's per-action settings via `GetSettingsAsync<PerActionSettings>()`.
2. If the `.blk` has no keyboard binding for the primary ID, the plugin sends
   the user-chosen fallback VK (typically `F13`–`F24`, `Pause`, or `ScrollLock`).
3. The user binds that same key in War Thunder. After WT saves, the binding
   appears in `machine.blk` and the fallback bridge is no longer needed —
   but it stays as a safety net.

The fallback list is curated to keys that physical keyboards don't have
(F13–F24) so they collide with nothing.

## 10. Open questions before phase 1

- Plugin pack: do we ship one master `.streamDeckPlugin` archive (Stream Deck Marketplace
  format) per module, or a single zip with installers? Marketplace format is the standard.
- Auto-update: SD plugins typically don't auto-update. Each module ships independently;
  user re-runs `deploy.ps1`.
- Shared config schema versioning: bump `config.json`'s `schemaVersion` field if we add fields,
  so old plugins gracefully degrade.
