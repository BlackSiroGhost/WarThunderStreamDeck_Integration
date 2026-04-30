# Installing — for friends

These plugins drive War Thunder cockpit toggles from your Elgato Stream Deck.
Each module ships as a single `.streamDeckPlugin` file. You only need to install
the modules you actually want.

## Requirements

- Windows 10 or 11
- Stream Deck app 6.5 or newer
- War Thunder installed (any branch — Steam or Gaijin launcher)

## 1. Install a module

1. Download the `.streamDeckPlugin` file from the friend who shared it (or
   from the Releases page).
2. **Double-click the file.** The Stream Deck app will load it and show the
   plugin under its own category in the right-hand action list.
3. Drag any of the actions onto a Stream Deck button.

That's it for installation. Stream Deck shows the new buttons immediately.

## 2. First-run setup

Each Stream Deck button installed by the plugin will show **`SETUP`** in its
title until you point it at your War Thunder controls file. Click any of the
buttons; the Property Inspector at the bottom of the Stream Deck app will show
the setup form.

### Step 1 — set the controls file path

The plugin reads your `.blk` to learn which keyboard key you've bound for each
action. Three options, in order of convenience:

| Option | Path |
|---|---|
| **Use auto-detected** (click that button) | leaves the field empty; the plugin scans the standard `Documents\My Games\WarThunder\Saves\last\production\machine.blk` etc. on every refresh. |
| **Paste your live in-game controls** | `%USERPROFILE%\Documents\My Games\WarThunder\Saves\last\production\machine.blk` (this stays in sync as you change keys in WT) |
| **Paste an exported preset** | any `.blk` you've saved from WT's "Save preset" UI |

Click **Save path**. Within a second the button title flips from `SETUP` to
e.g. `GEAR\n0%` and the icon switches to the live state.

### Step 2 — fallback key (only if needed)

Some War Thunder actions don't have a keyboard key bound — for example
`ID_FLAPS{}` (empty) or `ID_BAY_DOOR{ joyButton:i=7 }` (joystick only). The
button still appears on your deck, but pressing it would show a yellow alert
triangle because the plugin has nothing to send.

To fix this, scroll down in the Property Inspector to the
**This button — fallback key** section:

1. Pick a key you don't use elsewhere — `F13` through `F24` are the safest
   (they don't appear on physical keyboards so nothing else uses them).
2. Click **Save fallback key**.
3. Open War Thunder → Controls → find the matching action → bind the
   same key (e.g. `F13`) to it.
4. Save the controls in WT. The plugin will instantly pick up the new
   binding from `machine.blk` and use it directly. The fallback key
   acts as a one-time bridge while WT learns the binding.

## 3. What's on the button

The button title is two lines:

```
GEAR
0%
```

The top is the system label; the bottom is the live percentage from War
Thunder's `/state` endpoint, polled four times per second.

The icon previews **what the next press will do**, not the current state.
For gear: a ▼ arrow + `LOWER` means pressing the button will extend the
gear; ▲ + `RAISE` means pressing will retract. For flaps it's
similar (`EXTEND`/`RETRACT`).

Spam-pressing is supported — every press is queued and dispatched serially
with a 30 ms hold per tap (the minimum War Thunder seems to accept).

## Removing a module

Stream Deck → bottom-left More icon → Plugins → find "War Thunder - …" → Remove.

## Troubleshooting

- **Buttons stuck on `SETUP`**: the path is wrong or the file doesn't exist.
  The PI's auto-detect button is the safest fallback.
- **Pressing the button does nothing in War Thunder**:
  - Check the deck is the *physical* button. Clicking the on-screen preview in
    the Stream Deck app sends the key to the Stream Deck app, not War Thunder.
  - Make sure War Thunder has focus (alt-tab into the game, then press).
  - Check you're not at supersonic speed — flaps and gear physically don't
    extend at very high IAS.
- **Icon updates feel sluggish**: increase poll rate by editing the plugin
  source (`PollInterval` in `WarThunderBindingAction.cs`); 4 Hz / 250 ms is
  the default.

## Diagnostics

Each plugin appends to a log at:

```
%LOCALAPPDATA%\WarThunderStreamDeckPlugin\plugin.log
```

Open it in any text editor. Each press adds a line showing whether the
keystroke went out, which scancode, and whether the foreground window
was actually War Thunder.
