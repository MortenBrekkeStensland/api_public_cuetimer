# CueTimer API

Remote-control and monitor [CueTimer](https://www.cuetimer.com/) from external devices and software. CueTimer speaks two protocols — pick the one that fits your setup:

| Protocol | Best for | Direction | Documentation |
| --- | --- | --- | --- |
| **TCP** | Full control + live state monitoring (Companion, Crestron, custom web apps) | Two-way: send commands, receive continuous state feedback | [docs/tcp.md](docs/tcp.md) |
| **OSC** | Triggering cues from show-control software (QLab, Resolume, Mitti) | Two-way: send commands in, receive event triggers out | [docs/osc.md](docs/osc.md) |

## Which should I use?

- **Need to read CueTimer's live state** (running time, schedule offset, which list is active, timer states)? Use **TCP** — it streams full state as JSON every 200 ms. OSC does not provide continuous state.
- **Just need to fire and cue timers from QLab/Resolume/Mitti**, or have those tools react to CueTimer events (alerts, overtime, end time)? **OSC** is simpler and purpose-built for that.
- **Building a full custom controller or integration** (Crestron panels, web apps)? Use **TCP**. This is the same API our [Bitfocus Companion module](https://github.com/bitfocus/companion-module-presentationtools-cuetimer) is built on.

The two protocols expose **different command sets** — they are not interchangeable. Check the relevant doc for what each supports.

## Test tool

The [`CueTimerAPIClient`](CueTimerAPIClient) folder contains a Windows app (source included) for testing TCP commands sent to and from CueTimer.

## Links

- CueTimer: https://www.cuetimer.com/
- Companion module: https://github.com/bitfocus/companion-module-presentationtools-cuetimer
