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

Each module ships as its own `.streamDeckPlugin`. Click any category below to see every action it includes &mdash; the **icon you'll see on your deck**, the action name, and the underlying War Thunder binding ID.

### &#9992;&#65039; Aircraft

<details>
<summary><img src="https://img.shields.io/badge/Mechanisation-10_actions-FFB450?style=for-the-badge&labelColor=2D2D2D" alt="Mechanisation - 10 actions"/></summary>

> Gear, flaps, air brake, bay doors, cockpit doors, tail hook, WEP / boosters, VTOL.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/gear.png" width="56"/> | Toggle Gear | `ID_GEAR` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/flaps.png" width="56"/> | Toggle Flaps | `ID_FLAPS`, `ID_FLAPS_DOWN`, `ID_FLAPS_UP` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/airbrake.png" width="56"/> | Air Brake | `ID_AIR_BRAKE` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/air_reverse.png" width="56"/> | Air Reverse | `ID_AIR_REVERSE` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/bay_door.png" width="56"/> | Bomb Bay Door | `ID_BAY_DOOR` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/cockpit_door.png" width="56"/> | Cockpit Door | `ID_TOGGLE_COCKPIT_DOOR` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/trans_gear.png" width="56"/> | Carrier Tail Hook | `ID_TRANS_GEAR_DOWN` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/boosters.png" width="56"/> | Boosters / WEP | `ID_IGNITE_BOOSTERS` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/vtol_up.png" width="56"/> | VTOL Up | `vtol_rangeMax` |
| <img src="plugin/com.blacksiroghost.wt.mechanisation.sdPlugin/Images/Actions/vtol_down.png" width="56"/> | VTOL Down | `vtol_rangeMin` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Engine-3_actions-FF8030?style=for-the-badge&labelColor=2D2D2D" alt="Engine - 3 actions"/></summary>

> Throttle Max / Min, Maneuverability mode.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.engine.sdPlugin/Images/Actions/throttle_max.png" width="56"/> | Throttle Max | `throttle_rangeMax` |
| <img src="plugin/com.blacksiroghost.wt.engine.sdPlugin/Images/Actions/throttle_min.png" width="56"/> | Throttle Min | `throttle_rangeMin` |
| <img src="plugin/com.blacksiroghost.wt.engine.sdPlugin/Images/Actions/manuv_mode.png" width="56"/> | Maneuverability Mode | `ID_MANEUVERABILITY_MODE` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Weapons%20%E2%80%93%20A--A-12_actions-E05050?style=for-the-badge&labelColor=2D2D2D" alt="Weapons A-A - 12 actions"/></summary>

> Missiles (AAM), cannons, MGuns, additional guns, target lock, weapon lock, shooting cycle, reload, fire-axis.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/aam.png" width="56"/> | Air-to-Air Missile | `ID_AAM` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/fire_primary.png" width="56"/> | Fire Primary | `ID_FIRE_PRIMARY` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/fire_secondary.png" width="56"/> | Fire Secondary | `ID_FIRE_SECONDARY` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/fire_cannons.png" width="56"/> | Fire Cannons | `ID_FIRE_CANNONS` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/fire_mguns.png" width="56"/> | Fire MGuns | `ID_FIRE_MGUNS` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/fire_add_guns.png" width="56"/> | Fire Add. Guns | `ID_FIRE_ADDITIONAL_GUNS` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/lock_target.png" width="56"/> | Lock Target | `ID_LOCK_TARGET` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/weapon_lock.png" width="56"/> | Weapon Lock | `ID_WEAPON_LOCK` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/cycle_primary.png" width="56"/> | Cycle Primary | `ID_SWITCH_SHOOTING_CYCLE_PRIMARY` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/cycle_secondary.png" width="56"/> | Cycle Secondary | `ID_SWITCH_SHOOTING_CYCLE_SECONDARY` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/reload.png" width="56"/> | Reload Guns | `ID_RELOAD_GUNS` |
| <img src="plugin/com.blacksiroghost.wt.weapons.aa.sdPlugin/Images/Actions/fire_axis.png" width="56"/> | Fire (Axis) | `fire_rangeMax` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Weapons%20%E2%80%93%20A--G-15_actions-C040A0?style=for-the-badge&labelColor=2D2D2D" alt="Weapons A-G - 15 actions"/></summary>

> AGM, ATGM, bombs, rockets (single & series), guided bombs (drop & lock), laser designator, ballistic computers, target lock / unlock.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/agm.png" width="56"/> | Air-to-Ground Missile | `ID_AGM` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/agm_lock.png" width="56"/> | AGM Lock | `ID_AGM_LOCK` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/atgm.png" width="56"/> | ATGM | `ID_ATGM` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/bombs.png" width="56"/> | Drop Bomb | `ID_BOMBS` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/bombs_series.png" width="56"/> | Bomb Series | `ID_BOMBS_SERIES` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/rockets.png" width="56"/> | Fire Rockets | `ID_ROCKETS` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/rockets_series.png" width="56"/> | Rocket Series | `ID_ROCKETS_SERIES` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/gbu_drop.png" width="56"/> | Drop Guided Bomb | `ID_GUIDED_BOMBS` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/gbu_lock.png" width="56"/> | Guided Bomb Lock | `ID_GUIDED_BOMBS_LOCK` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/laser.png" width="56"/> | Laser Designator | `ID_TOGGLE_LASER_DESIGNATOR` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/rocket_bc.png" width="56"/> | Rocket Ballistics | `ID_TOGGLE_ROCKETS_BALLISTIC_COMPUTER` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/bc.png" width="56"/> | Combined BC | `ID_TOGGLE_CANNONS_AND_ROCKETS_BALLISTIC_COMPUTER` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/designate.png" width="56"/> | Designate Target | `ID_DESIGNATE_TARGET` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/lock_pt.png" width="56"/> | Lock Point | `ID_LOCK_TARGETING_AT_POINT` |
| <img src="plugin/com.blacksiroghost.wt.weapons.ag.sdPlugin/Images/Actions/unlock_pt.png" width="56"/> | Unlock Point | `ID_UNLOCK_TARGETING_AT_POINT` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Countermeasures-3_actions-F0D040?style=for-the-badge&labelColor=2D2D2D" alt="Countermeasures - 3 actions"/></summary>

> Flares, IR projector, smoke screen.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.countermeasures.sdPlugin/Images/Actions/flares.png" width="56"/> | Flares | `ID_FLARES` |
| <img src="plugin/com.blacksiroghost.wt.countermeasures.sdPlugin/Images/Actions/ir_proj.png" width="56"/> | IR Projector | `ID_IR_PROJECTOR` |
| <img src="plugin/com.blacksiroghost.wt.countermeasures.sdPlugin/Images/Actions/smoke.png" width="56"/> | Smoke Screen | `ID_SMOKE_SCREEN` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Radar%20%E2%80%93%20Air-10_actions-50D070?style=for-the-badge&labelColor=2D2D2D" alt="Radar Air - 10 actions"/></summary>

> Radar power, mode switch, range, scan pattern, ACM mode, target lock & switch, type switch, lock / unlock designation.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/sensor_switch.png" width="56"/> | Radar Power | `ID_SENSOR_SWITCH` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/mode_switch.png" width="56"/> | Mode Switch | `ID_SENSOR_MODE_SWITCH` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/range_switch.png" width="56"/> | Range Switch | `ID_SENSOR_RANGE_SWITCH` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/scan_switch.png" width="56"/> | Scan Pattern | `ID_SENSOR_SCAN_PATTERN_SWITCH` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/acm.png" width="56"/> | ACM Mode | `ID_SENSOR_ACM_SWITCH` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/target_lock.png" width="56"/> | Target Lock | `ID_SENSOR_TARGET_LOCK`, `ID_LOCK_TARGET`, `ID_LOCK_TARGETING` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/target_switch.png" width="56"/> | Target Switch | `ID_SENSOR_TARGET_SWITCH` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/type_switch.png" width="56"/> | Type Switch | `ID_SENSOR_TYPE_SWITCH` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/lock_des.png" width="56"/> | Lock Designation | `ID_LOCK_TARGETING` |
| <img src="plugin/com.blacksiroghost.wt.radar.air.sdPlugin/Images/Actions/unlock_des.png" width="56"/> | Unlock Designation | `ID_UNLOCK_TARGETING` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Optics%20%26%20Pod-16_actions-A070D0?style=for-the-badge&labelColor=2D2D2D" alt="Optics - 16 actions"/></summary>

> Thermal polarity, night vision, designate target, lock at point, cue X / Y / Z (max / min / center), laser designator, target & UAV cameras.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/thermal.png" width="56"/> | Thermal Polarity | `ID_THERMAL_WHITE_IS_HOT` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/night_vision.png" width="56"/> | Night Vision | `ID_PLANE_NIGHT_VISION` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/designate.png" width="56"/> | Designate Target | `ID_DESIGNATE_TARGET` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/lock_pt.png" width="56"/> | Lock at Point | `ID_LOCK_TARGETING_AT_POINT` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_x_max.png" width="56"/> | Cue X + | `sensor_cue_x_rangeMax` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_x_min.png" width="56"/> | Cue X &minus; | `sensor_cue_x_rangeMin` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_x_set.png" width="56"/> | Cue X Center | `sensor_cue_x_rangeSet` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_y_max.png" width="56"/> | Cue Y + | `sensor_cue_y_rangeMax` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_y_min.png" width="56"/> | Cue Y &minus; | `sensor_cue_y_rangeMin` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_y_set.png" width="56"/> | Cue Y Center | `sensor_cue_y_rangeSet` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_z_max.png" width="56"/> | Cue Z + | `sensor_cue_z_rangeMax` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_z_min.png" width="56"/> | Cue Z &minus; | `sensor_cue_z_rangeMin` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/cue_z_set.png" width="56"/> | Cue Z Center | `sensor_cue_z_rangeSet` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/laser.png" width="56"/> | Laser Designator | `ID_TOGGLE_LASER_DESIGNATOR` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/tgt_cam.png" width="56"/> | Target Camera | `ID_TARGET_CAMERA` |
| <img src="plugin/com.blacksiroghost.wt.optics.air.sdPlugin/Images/Actions/uav_cam.png" width="56"/> | UAV Camera | `ID_TOGGLE_UAV_CAMERA` |

</details>

### &#128642; Helicopter

<details>
<summary><img src="https://img.shields.io/badge/Heli%20Mechanisation-4_actions-40C0A0?style=for-the-badge&labelColor=2D2D2D" alt="Heli Mech - 4 actions"/></summary>

> Helicopter gear, flaps Up / Down, air brake.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.heli.mech.sdPlugin/Images/Actions/gear.png" width="56"/> | Heli Gear | `ID_GEAR_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.mech.sdPlugin/Images/Actions/flaps_up.png" width="56"/> | Heli Flaps Up | `ID_FLAPS_UP_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.mech.sdPlugin/Images/Actions/flaps_down.png" width="56"/> | Heli Flaps Down | `ID_FLAPS_DOWN_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.mech.sdPlugin/Images/Actions/airbrake.png" width="56"/> | Heli Air Brake | `ID_AIR_BRAKE_HELICOPTER` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Heli%20Combat-14_actions-D05050?style=for-the-badge&labelColor=2D2D2D" alt="Heli Combat - 14 actions"/></summary>

> Fire primary / secondary / MGuns / cannons / additional, rocket series, flares (single & series), ballistic computers, shooting cycle, instructor, exit cycle.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/fire_primary.png" width="56"/> | Heli Fire Primary | `ID_FIRE_PRIMARY_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/fire_secondary.png" width="56"/> | Heli Fire Secondary | `ID_FIRE_SECONDARY_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/fire_mguns.png" width="56"/> | Heli Fire MGuns | `ID_FIRE_MGUNS_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/fire_cannons.png" width="56"/> | Heli Fire Cannons | `ID_FIRE_CANNONS_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/fire_add_guns.png" width="56"/> | Heli Fire Add Guns | `ID_FIRE_ADDITIONAL_GUNS_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/rockets_series.png" width="56"/> | Heli Rocket Series | `ID_ROCKETS_SERIES_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/flare_single.png" width="56"/> | Heli Flare | `ID_FLARES_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/flares.png" width="56"/> | Heli Flare Series | `ID_FLARES_SERIES_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/rocket_bc.png" width="56"/> | Heli Rocket BC | `ID_TOGGLE_ROCKETS_BALLISTIC_COMPUTER_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/bc.png" width="56"/> | Heli Combined BC | `ID_TOGGLE_CANNONS_AND_ROCKETS_BALLISTIC_COMPUTER_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/cycle_primary.png" width="56"/> | Heli Cycle Primary | `ID_SWITCH_SHOOTING_CYCLE_PRIMARY_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/cycle_secondary.png" width="56"/> | Heli Cycle Sec | `ID_SWITCH_SHOOTING_CYCLE_SECONDARY_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/instructor.png" width="56"/> | Heli Instructor | `ID_TOGGLE_INSTRUCTOR_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.combat.sdPlugin/Images/Actions/exit_cycle.png" width="56"/> | Heli Exit Cycle | `ID_EXIT_SHOOTING_CYCLE_MODE_HELICOPTER` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Heli%20Sensors-11_actions-60A0E0?style=for-the-badge&labelColor=2D2D2D" alt="Heli Sensors - 11 actions"/></summary>

> Night vision, seeker camera, sensor switch & lock, laser designator, target camera, lock at point, cue X / Y axes.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/night_vision.png" width="56"/> | Heli Night Vision | `ID_HELI_GUNNER_NIGHT_VISION` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/seeker_cam.png" width="56"/> | Seeker Camera | `ID_CAMERA_SEEKER_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/sensor_switch.png" width="56"/> | Sensor Switch | `ID_SENSOR_SWITCH_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/sensor_lock.png" width="56"/> | Sensor Lock | `ID_SENSOR_TARGET_LOCK_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/laser.png" width="56"/> | Heli Laser | `ID_TOGGLE_LASER_DESIGNATOR_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/tgt_cam.png" width="56"/> | Heli Target Cam | `ID_TARGET_CAMERA_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/lock_pt.png" width="56"/> | Heli Lock Point | `ID_LOCK_TARGETING_AT_POINT_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/cue_x_max.png" width="56"/> | Heli Cue X + | `helicopter_sensor_cue_x_rangeMax` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/cue_x_min.png" width="56"/> | Heli Cue X &minus; | `helicopter_sensor_cue_x_rangeMin` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/cue_y_max.png" width="56"/> | Heli Cue Y + | `helicopter_sensor_cue_y_rangeMax` |
| <img src="plugin/com.blacksiroghost.wt.heli.sensors.sdPlugin/Images/Actions/cue_y_min.png" width="56"/> | Heli Cue Y &minus; | `helicopter_sensor_cue_y_rangeMin` |

</details>

### &#128666; Tank

<details>
<summary><img src="https://img.shields.io/badge/Tank%20Movement-8_actions-A07050?style=for-the-badge&labelColor=2D2D2D" alt="Tank Movement - 8 actions"/></summary>

> Direction driving toggle, suspension clearance up / down, pitch up / down, roll left / right, reset.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.tank.movement.sdPlugin/Images/Actions/dir_drive.png" width="56"/> | Direction Driving | `ID_ENABLE_GM_DIRECTION_DRIVING` |
| <img src="plugin/com.blacksiroghost.wt.tank.movement.sdPlugin/Images/Actions/susp_clr_up.png" width="56"/> | Suspension Up | `ID_SUSPENSION_CLEARANCE_UP` |
| <img src="plugin/com.blacksiroghost.wt.tank.movement.sdPlugin/Images/Actions/susp_clr_dn.png" width="56"/> | Suspension Down | `ID_SUSPENSION_CLEARANCE_DOWN` |
| <img src="plugin/com.blacksiroghost.wt.tank.movement.sdPlugin/Images/Actions/susp_pit_up.png" width="56"/> | Pitch Up | `ID_SUSPENSION_PITCH_UP` |
| <img src="plugin/com.blacksiroghost.wt.tank.movement.sdPlugin/Images/Actions/susp_pit_dn.png" width="56"/> | Pitch Down | `ID_SUSPENSION_PITCH_DOWN` |
| <img src="plugin/com.blacksiroghost.wt.tank.movement.sdPlugin/Images/Actions/susp_rol_up.png" width="56"/> | Roll Right | `ID_SUSPENSION_ROLL_UP` |
| <img src="plugin/com.blacksiroghost.wt.tank.movement.sdPlugin/Images/Actions/susp_rol_dn.png" width="56"/> | Roll Left | `ID_SUSPENSION_ROLL_DOWN` |
| <img src="plugin/com.blacksiroghost.wt.tank.movement.sdPlugin/Images/Actions/susp_reset.png" width="56"/> | Suspension Reset | `ID_SUSPENSION_RESET` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Tank%20Combat-10_actions-D05050?style=for-the-badge&labelColor=2D2D2D" alt="Tank Combat - 10 actions"/></summary>

> Fire secondary & special, gun selection (primary / secondary / MG / reset), smoke, repair, hull aiming, reload.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/fire_secondary.png" width="56"/> | Fire Secondary | `ID_FIRE_GM_SECONDARY_GUN` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/fire_special.png" width="56"/> | Fire Special | `ID_FIRE_GM_SPECIAL_GUN` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/sel_primary.png" width="56"/> | Select Primary | `ID_SELECT_GM_GUN_PRIMARY` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/sel_secondary.png" width="56"/> | Select Secondary | `ID_SELECT_GM_GUN_SECONDARY` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/sel_mg.png" width="56"/> | Select MG | `ID_SELECT_GM_GUN_MACHINEGUN` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/sel_reset.png" width="56"/> | Reset Selection | `ID_SELECT_GM_GUN_RESET` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/smoke.png" width="56"/> | Smoke Screen | `ID_SMOKE_SCREEN_GENERATOR` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/repair.png" width="56"/> | Repair | `ID_REPAIR_TANK` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/hull_aim.png" width="56"/> | Hull Aiming | `ID_ENABLE_GM_HULL_AIMING` |
| <img src="plugin/com.blacksiroghost.wt.tank.combat.sdPlugin/Images/Actions/reload.png" width="56"/> | Reload Guns | `ID_RELOAD_GUNS` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Tank%20Sights-11_actions-A0E040?style=for-the-badge&labelColor=2D2D2D" alt="Tank Sights - 11 actions"/></summary>

> Rangefinder, targeting hold, zoom hold / toggle, crosshair light, night vision, fuse mode, thermal polarity, sight distance + / &minus; / set.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/rangefinder.png" width="56"/> | Rangefinder | `ID_RANGEFINDER` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/targ_hold.png" width="56"/> | Targeting Hold | `ID_TARGETING_HOLD_GM` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/zoom_hold.png" width="56"/> | Zoom Hold | `ID_ZOOM_HOLD_GM` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/zoom_toggle.png" width="56"/> | Zoom Toggle | `ID_ZOOM_TOGGLE` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/xhair_light.png" width="56"/> | Crosshair Light | `ID_TOGGLE_GM_CROSSHAIR_LIGHTING` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/tank_nv.png" width="56"/> | Tank NV | `ID_TANK_NIGHT_VISION` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/fuse_mode.png" width="56"/> | Fuse Mode | `ID_TANK_SWITCH_FUSE_MODE` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/thermal.png" width="56"/> | Thermal Polarity | `ID_THERMAL_WHITE_IS_HOT` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/sight_max.png" width="56"/> | Sight Distance + | `gm_sight_distance_rangeMax` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/sight_min.png" width="56"/> | Sight Distance &minus; | `gm_sight_distance_rangeMin` |
| <img src="plugin/com.blacksiroghost.wt.tank.sights.sdPlugin/Images/Actions/sight_set.png" width="56"/> | Sight Distance = | `gm_sight_distance_rangeSet` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Radar%20%E2%80%93%20Ground-10_actions-88B040?style=for-the-badge&labelColor=2D2D2D" alt="Radar Ground - 10 actions"/></summary>

> Tank radar power, mode, range, scan, target lock & switch, view switch, weapon lock, IRCM, APS lock.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/sensor_switch.png" width="56"/> | Tank Sensor Switch | `ID_SENSOR_SWITCH_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/mode_switch.png" width="56"/> | Tank Mode Switch | `ID_SENSOR_MODE_SWITCH_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/range_switch.png" width="56"/> | Tank Range Switch | `ID_SENSOR_RANGE_SWITCH_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/scan_switch.png" width="56"/> | Tank Scan Pattern | `ID_SENSOR_SCAN_PATTERN_SWITCH_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/target_lock.png" width="56"/> | Tank Target Lock | `ID_SENSOR_TARGET_LOCK_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/target_switch.png" width="56"/> | Tank Target Switch | `ID_SENSOR_TARGET_SWITCH_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/view_switch.png" width="56"/> | Tank View Switch | `ID_SENSOR_VIEW_SWITCH_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/weapon_lock.png" width="56"/> | Weapon Lock Tank | `ID_WEAPON_LOCK_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/ircm.png" width="56"/> | IRCM Switch | `ID_IRCM_SWITCH_TANK` |
| <img src="plugin/com.blacksiroghost.wt.radar.ground.sdPlugin/Images/Actions/aps_lock.png" width="56"/> | APS Lock | `ID_LOCK_TARGETING_AT_POINT_SHIP` |

</details>

### &#9875;&#65039; Naval

<details>
<summary><img src="https://img.shields.io/badge/Ship%20Combat-2_actions-6090E0?style=for-the-badge&labelColor=2D2D2D" alt="Ship Combat - 2 actions"/></summary>

> Lock target, ship zoom max. _(Naval module is in early development; more actions coming.)_

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.ship.combat.sdPlugin/Images/Actions/lock_pt.png" width="56"/> | Lock Target | `ID_LOCK_TARGETING_AT_POINT_SHIP` |
| <img src="plugin/com.blacksiroghost.wt.ship.combat.sdPlugin/Images/Actions/zoom_max.png" width="56"/> | Ship Zoom Max | `ship_zoom_rangeMax` |

</details>

### &#127916; Cross-vehicle

<details>
<summary><img src="https://img.shields.io/badge/View%20%2F%20Camera-10_actions-E080B0?style=for-the-badge&labelColor=2D2D2D" alt="View - 10 actions"/></summary>

> Toggle view, helicopter view, camera neutral / down, driver / binoculars / target / UAV cameras, camera X / Y center.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/toggle.png" width="56"/> | Toggle View | `ID_TOGGLE_VIEW` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/toggle_heli.png" width="56"/> | Heli View | `ID_TOGGLE_VIEW_HELICOPTER` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/neutral.png" width="56"/> | Camera Neutral | `ID_CAMERA_NEUTRAL` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/view_down.png" width="56"/> | View Down | `ID_CAMERA_VIEW_DOWN` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/driver.png" width="56"/> | Driver Camera | `ID_CAMERA_DRIVER` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/binoc.png" width="56"/> | Binoculars | `ID_CAMERA_BINOCULARS` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/tgt_cam.png" width="56"/> | Target Camera | `ID_TARGET_CAMERA` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/uav.png" width="56"/> | UAV Camera | `ID_TOGGLE_UAV_CAMERA` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/cam_x.png" width="56"/> | Cam X Center | `camx_rangeSet` |
| <img src="plugin/com.blacksiroghost.wt.view.sdPlugin/Images/Actions/cam_y.png" width="56"/> | Cam Y Center | `camy_rangeSet` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/Comms-11_actions-5070D0?style=for-the-badge&labelColor=2D2D2D" alt="Comms - 11 actions"/></summary>

> Voice messages 1 / 2 / 5 / 7 / 8, push-to-talk, squad voice list, squad designate, support plane, plane orbit, multifunction wheel menu.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/voice1.png" width="56"/> | Voice Msg 1 | `ID_VOICE_MESSAGE_1` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/voice2.png" width="56"/> | Voice Msg 2 | `ID_VOICE_MESSAGE_2` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/voice5.png" width="56"/> | Voice Msg 5 | `ID_VOICE_MESSAGE_5` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/voice7.png" width="56"/> | Voice Msg 7 | `ID_VOICE_MESSAGE_7` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/voice8.png" width="56"/> | Voice Msg 8 | `ID_VOICE_MESSAGE_8` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/ptt.png" width="56"/> | Push to Talk | `ID_PTT` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/voice_squad.png" width="56"/> | Squad Voice List | `ID_SHOW_VOICE_MESSAGE_LIST_SQUAD` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/squad_des.png" width="56"/> | Squad Designate | `ID_SQUAD_TARGET_DESIGNATION` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/support.png" width="56"/> | Support Plane | `ID_START_SUPPORT_PLANE` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/orbit.png" width="56"/> | Plane Orbit | `ID_SUPPORT_PLANE_ORBITING` |
| <img src="plugin/com.blacksiroghost.wt.coms.sdPlugin/Images/Actions/wheel.png" width="56"/> | Wheel Menu | `ID_SHOW_MULTIFUNC_WHEEL_MENU` |

</details>

<details>
<summary><img src="https://img.shields.io/badge/HUD%20%26%20System-17_actions-808090?style=for-the-badge&labelColor=2D2D2D" alt="HUD - 17 actions"/></summary>

> Hide HUD, pause, screenshot, flight menu / setup, MP stats, instructor, action bar 5&ndash;9, continue, control mode (incl. UAV), shot frequency, exit cycle.

| Icon | Action | War Thunder binding |
|:---:|---|---|
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/hide_hud.png" width="56"/> | Hide HUD | `ID_HIDE_HUD` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/pause.png" width="56"/> | Pause | `ID_GAME_PAUSE` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/screenshot.png" width="56"/> | Screenshot | `ID_SCREENSHOT` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/flightmenu.png" width="56"/> | Flight Menu | `ID_FLIGHTMENU` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/flightsetup.png" width="56"/> | Flight Setup | `ID_FLIGHTMENU_SETUP` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/mp_stat.png" width="56"/> | MP Stat Screen | `ID_MPSTATSCREEN` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/instructor.png" width="56"/> | Instructor | `ID_TOGGLE_INSTRUCTOR` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/ab5.png" width="56"/> | Action Bar 5 | `ID_ACTION_BAR_ITEM_5` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/ab6.png" width="56"/> | Action Bar 6 | `ID_ACTION_BAR_ITEM_6` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/ab7.png" width="56"/> | Action Bar 7 | `ID_ACTION_BAR_ITEM_7` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/ab8.png" width="56"/> | Action Bar 8 | `ID_ACTION_BAR_ITEM_8` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/ab9.png" width="56"/> | Action Bar 9 | `ID_ACTION_BAR_ITEM_9` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/continue.png" width="56"/> | Continue | `ID_CONTINUE` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/ctrl_mode.png" width="56"/> | Control Mode | `ID_CONTROL_MODE` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/ctrl_mode_uav.png" width="56"/> | Control Mode UAV | `ID_CONTROL_MODE_HUMAN_UAV` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/shot_freq.png" width="56"/> | Shot Frequency | `ID_CHANGE_SHOT_FREQ` |
| <img src="plugin/com.blacksiroghost.wt.hud.sdPlugin/Images/Actions/exit_cycle.png" width="56"/> | Exit Cycle | `ID_EXIT_SHOOTING_CYCLE_MODE` |

</details>

> Source-of-truth spec: [`tools/spec/modules.psd1`](tools/spec/modules.psd1). Architecture: [`CATEGORIES.md`](CATEGORIES.md).

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
