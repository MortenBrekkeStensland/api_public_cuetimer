> **Looking for OSC instead?** → [docs/osc.md](osc.md). OSC is a smaller, trigger-focused command set for show-control software (QLab, Resolume, Mitti). The TCP API below is the full-featured option with continuous state feedback.

# CueTimer TCP API

## Protocol

CueTimer runs a TCP server that waits for a client connection on a default port (**31601**), configurable from the app settings. Once a connection is established between the client and CueTimer, it continuously listens to incoming messages from that client.

Commands are sent as an ASCII string with `$` as a termination character at the end (e.g. `FireNext$`).

## Targeting a specific Cue List (CueTimer v4+)

- By default, CueTimer uses the currently active list for both commands and feedback.
- To target a specific list, send the command `select_list#<guid>$`.
- If the provided GUID is invalid or missing, CueTimer continues using the active list.
- You can retrieve available GUIDs from the `lists` feedback.
- Examples:
  - `select_list#a926123d-593b-4b53-913c-febeacc06f1c$` — selects the list with the given GUID.
  - `select_list#$` — uses the active list for routing commands and feedback.
  - `select_list#some_invalid_guid$` — falls back to the active list.

## Commands

| # | Command | Description |
| --- | --- | --- |
| 1 | `FireNext` | |
| 2 | `CueNext` | |
| 3 | `CueCurrent` | |
| 4 | `Pause` | |
| 5 | `Restart` | |
| 6 | `Blackout` | Toggle blackout mode |
| 7 | `AddMinute` | Add 1 minute |
| 8 | `SubMinute` | Subtract 1 minute |
| 9 | `AddSpeed` | Increase speed by 5% |
| 10 | `SubSpeed` | Decrease speed by 5% |
| 11 | `Fullscreen` | Fullscreen mode |
| 12 | `NDI` | Toggle NDI |
| 13 | `Message` | Toggle Message visibility |
| 14 | `Clock` | Toggle clock screen |
| 15 | `MoveNextUp` | Move next indicator one row up |
| 16 | `MoveNextDown` | Move next indicator one row down |
| 17 | `FireTimerWithID` | Start a timer with its id, passed after `#`, e.g. `FireTimerWithID#1$`. Note `#` can't be used in a timer ID |
| 18 | `CueTimerWithID` | Move next indicator to a timer with its id, passed after `#`, e.g. `CueTimerWithID#1$`. Note `#` can't be used in a timer ID |
| 19 | `STM` | Toggle Single timer mode |
| 20 | `SetDuration` | Set the duration of the active timer, format `hh:mm:ss`, e.g. `SetDuration#00:10:00$` |
| 21 | `AddXMinutes` | Add minutes to the active timer, e.g. `AddXMinutes#5$` adds 5 minutes |
| 22 | `SubXMinutes` | Subtract minutes from the active timer, e.g. `SubXMinutes#5$` subtracts 5 minutes |
| 23 | `InitList` | Clear the whole list & create a new timer with the properties of the default new timer |
| 24 | `Preview` | Preview mode |
| 25 | `Presenter` | Presenter mode |
| 26 | `select_list` | Links current connection to a list with the given GUID, e.g. `select_list#7cff78a1-46cf-4a2f-8820-a04cce550726$` |
| 27 | `ActivateNextList` | Switch to the next list |
| 28 | `ActivatePreviousList` | Switch to the previous list |
| 29 | `ActivateListByGUID` | Activates the list associated with the given GUID, e.g. `ActivateListByGUID#7cff78a1-46cf-4a2f-8820-a04cce550726$` |
| 30 | `MultitimerPreview` | Multitimer Preview mode |
| 31 | `MultitimerFullscreen` | Multitimer Fullscreen mode |
| 32 | `MultitimerNDI` | Toggle Multitimer NDI |

### Output window command notes

- `Fullscreen`, `Preview`, `Presenter`, `MultitimerPreview`, and `MultitimerFullscreen` accept an additional parameter passed after `#`.
- Accepted values are `toggle` | `on` | `off`.
- For backward compatibility, CueTimer also accepts `undefined` and an empty string as the value — both act as `toggle`.
- Examples:
  - `Fullscreen#on$`
  - `Fullscreen#off$`
  - `Fullscreen#toggle$`, `Fullscreen$`, and `Fullscreen#undefined$` all act as toggle.

## Feedback

Once the client is connected, CueTimer continuously sends info every 200 ms. This is a JSON string with `$` as a termination character at the very end.

```json
{
  "listNumber": "1",
  "listGUID": "a926123d-593b-4b53-913c-febeacc06f1c",
  "lists": [
    {
        "guid":"a926123d-593b-4b53-913c-febeacc06f1c",
        "title":"List 1",
        "isActive": false
    },
    {
        "guid":"7cff78a1-46cf-4a2f-8820-a04cce550726",
        "title":"List 2",
        "isActive": true
    },
    {
        "guid":"130b1132-9ea7-4669-97ec-1c3c623e5acd",
        "title":"List 3",
        "isActive": false
    }
  ],
  "h": "0",
  "m": "31",
  "s": "8",
  "fg": "#FFFFFFFF",
  "bg": "#FF696969",
  "speed": "100",
  "name": "",
  "endTime": "22:12:37",
  "Fullscreen": false,
  "Preview": false,
  "Presenter": false,
  "MultitimerPreview": false,
  "MultitimerFullscreen": false,
  "NDI": false,
  "MultitimerNDI": false,
  "Message": false,
  "STM": false,
  "Clock": false,
  "Pause": false,
  "Blackout": false,
  "nextTimerName": "",
  "nextTimerDuration": "01:00:00",
  "scheduleOffset": "-00:05:01",
  "scheduleOffsetStatus": "Ahead",
  "timers": {
    "1": {
      "name": "",
      "duration": "00:10:00",
      "bg": "#000000"
    },
    "2": {
      "name": "",
      "duration": "01:00:00",
      "bg": "#FF0000"
    }
  }
}$
```

| Field | Description |
| --- | --- |
| `listNumber` | The list number (e.g. `1`, `2`) the feedback is coming from |
| `listGUID` | The GUID of the list that generated the feedback |
| `lists` | An **ordered** array of all available list objects (see below) |
| `lists[].title` | The name of the list (without any numeric prefix such as `1-`, `2-`) |
| `lists[].guid` | The unique identifier (GUID) of the list |
| `lists[].isActive` | Boolean — whether the list is currently active in the UI |
| `h` | Hours |
| `m` | Minutes |
| `s` | Seconds |
| `fg` | Foreground color in ARGB format (`#FF000000` if blackout is active) |
| `bg` | Background color in ARGB format (`#FF000000` if blackout is active) |
| `speed` | Speed percentage |
| `name` | Timer name |
| `endTime` | `hh:mm:ss` format (`""` if no value) |
| `Fullscreen` | State of Fullscreen mode (Boolean) |
| `Preview` | State of Preview mode (Boolean) |
| `Presenter` | State of Presenter mode (Boolean) |
| `NDI` | State of NDI (Boolean) |
| `MultitimerPreview` | State of Multitimer Preview mode (Boolean) |
| `MultitimerFullscreen` | State of Multitimer Fullscreen mode (Boolean) |
| `MultitimerNDI` | State of Multitimer NDI (Boolean) |
| `Message` | State of Message visibility (Boolean) |
| `STM` | State of Single timer mode (Boolean) |
| `Clock` | State of Clock screen (Boolean) |
| `Pause` | State of pause button (Boolean) |
| `Blackout` | State of Blackout button (Boolean) |
| `nextTimerName` | Name of the next timer |
| `nextTimerDuration` | Duration of the next timer |
| `scheduleOffset` | Difference between the end time of the current red timer and the next start time, in `hh:mm:ss`. Can be positive or negative |
| `scheduleOffsetStatus` | Useful for behaviour when the show is delayed or ahead. One of `Delayed`, `Ahead`, `NA` |
| `timers` | Name, duration & background of the timers in the list. You only receive data for timers that have their IDs set to a value |
