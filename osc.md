> **Looking for the full API?** → [docs/tcp.md](tcp.md). The TCP API exposes the complete command set (~30 commands) and a continuous JSON state feed. OSC below is a focused subset for triggering cues from show-control software and sending event triggers out.

# CueTimer OSC API

You can control CueTimer through OSC, and have CueTimer send OSC out on certain events. This makes it easy to integrate with external devices and show-control software like **QLab**, **Resolume**, and **Mitti**. OSC is sent over the local LAN.

OSC has two directions, configured separately:

- **OSC In** — external software sends commands *to* CueTimer (fire, cue, pause, restart).
- **OSC Out** — CueTimer sends a message *to* a destination of your choice when an event happens (a timer fires, a countdown reaches an alert threshold or overtime, etc.).

> **Scope note:** OSC is a focused command set, not a replacement for the [TCP API](tcp.md). If you need CueTimer's full live state — running time, schedule offset, active list, per-timer states — use TCP, which streams complete state as JSON every 200 ms. OSC Out sends individual event triggers, not continuous state.

---

## OSC In — controlling CueTimer

### Port and IP address

Port **4779** is used for incoming OSC. Make sure this port is not blocked by a firewall.

The OSC message sent to CueTimer must use this port along with the IP address of the computer running CueTimer. To send OSC from another application on the same machine, use the localhost address `127.0.0.1`.

### Global commands

The commands from the program interface (Fire next, Cue next, Pause, Restart) can be triggered with OSC. These addresses accept any type or argument and will still work; for simplicity we recommend a string, float, or integer argument.

| Description | OSC address |
| --- | --- |
| FireNext | `/global/fire` |
| CueNext | `/global/cue` |
| Pause | `/global/pause` |
| Restart | `/global/restart` |

### Individual timer commands

You can also fire and cue individual timers in the Cue List. First, set a unique ID for the timer using the **ID** field in the CueTimer Cue List. Then use that same ID in the OSC argument.

Two actions are available for individual timers:

- **Fire** — the timer starts from the beginning. (Sending the command twice restarts it.)
- **Cue** — the timer is set as the next to fire when the fire button is next pushed.

| Description | OSC address | OSC type | OSC argument |
| --- | --- | --- | --- |
| Fire timer | `/timer/fire` | String | `{id}` |
| Cue timer (set next) | `/timer/cue` | String | `{id}` |

> **Note on IDs:** CueTimer will not accept `#` in the ID field. If `#` is used as part of the argument and ID, the OSC message will not work. The OSC protocol also does not allow spaces in the argument field — so instead of an ID like `keynote speaker`, use `keynote_speaker`.

---

## OSC Out — CueTimer sending OSC

CueTimer can send an OSC message when certain events occur. This is configured in the **OSC** tab of CueTimer's settings.

### Setup

- **Destination IP address** — the IP of the device or software that should receive the messages.
- **Port** — the port that destination is listening on.

Each event below has its own configurable OSC **address**, **type**, and **argument**, set in the settings panel. The addresses are not fixed — you define them to match whatever your receiving software expects.

| Event | Fires when | Default type |
| --- | --- | --- |
| Fire Next | A new timer starts | integer |
| Alert 1 threshold | The countdown reaches alert 1 | integer |
| Alert 2 threshold | The countdown reaches alert 2 | integer |
| Overtime | The countdown reaches 0 | integer |
| Cue Next | A Cue Next command is issued | integer |
| End Time | Sends the End Time of the current countdown, as a string | string |

To set this up: open the OSC tab, enter the destination IP and port, then for each event you want to use, fill in the OSC address your receiving software listens on (for example `/cuetimer/overtime`) and adjust the type and argument as needed.

> ⚠️ **Items below should be verified against the current build before publishing** — they are not fully confirmed by the source material:
> - Exact argument/type semantics for each outgoing event (whether the integer argument is a fixed value, the timer ID, or a count).
> - Whether the outgoing events respect the active Cue List or send globally.
> - Whether `End Time` sends once when the timer turns red, or updates on change.

---

## See also

- [TCP API](tcp.md) — full command set and continuous JSON state feedback
- [Bitfocus Companion module](https://github.com/bitfocus/companion-module-presentationtools-cuetimer) — built on the TCP API
