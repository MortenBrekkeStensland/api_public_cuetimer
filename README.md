## CueTimer API

### Protocol:
CueTimer has a TCP server that waits for a client connection on a default port (31601) and can be configured from the app settings. Once a connection is instantiated between the client and CueTimer it will continuously listen to the incoming messages from that client.
The commands should be sent as an ASCII string with `$` as a termination char at
the end of it (e.g. FireNext$).


### Targeting a Specific List (CueTimer v4+)

- By default, CueTimer uses the currently active list for both commands and feedback.
- To target a specific list, send the command `select_list#<guid>$`.
- If the provided GUID is invalid or missing, CueTimer will continue using the active list.
- You can retrieve available GUIDs from the `lists` feedback.
- Examples:
  - `select_list#a926123d-593b-4b53-913c-febeacc06f1c$`: selects the list with the given GUID.
  - `select_list#$`: uses the active list for routing commands and feedback.
  - `select_list#some_invalid_guid$`: falls back to the active list.



### Commands:

 1. `FireNext`
 2. `CueNext`
 3. `CueCurrent`
 4. `Pause`
 5. `Restart`
 6. `Blackout`: Togglle blackout mode
 7. `AddMinute`: Add 1 minute.
 8. `SubMinute`: Subtract 1 minute.
 9. `AddSpeed`: Increase speed by 5%.
 10. `SubSpeed`: Decrease speed by 5%.
 11. `Fullscreen`: Fullscreen mode
 12. `NDI`: Togglle NDI
 13. `Message`: Togglle Message visibilty
 14. `Clock`: Toggle clock screen
 15. `MoveNextUp`: Move next indicator one row up
 16. `MoveNextDown`: Move next indicator one row down
 17. `FireTimerWithID`: Start a timer with its id, the id can be passed after `#`, example `FireTimerWithID#1$`, please note that `#` can't be used in timer ID
 18. `CueTimerWithID`: Move next indicator to a timer with its id, the id can be passed after `#`, example `CueTimerWithID#1$`, please note that `#` can't be used in timer ID
 19. `STM`: Togglle Single timer mode
 20. `SetDuration`: Set the duration of the active timer to the passed value, format `hh:mm:ss`, example `SetDuration#00:10:00$`
 21. `AddXMinutes`: Add minutes to the active timer, example `AddXMinutes#5$` will add 5 minutes
 22. `SubXMinutes`: Subtract minutes from the active timer, example `SubXMinutes#5$` will subtract 5 minutes
 23. `InitList`: Clear the whole list & Create a new timer which has the properties of the Default new timer
 24. `Preview`: Preview mode
 25. `Presenter`: Presenter mode
 26. `select_list`: Links current connection to a list with the given GUID `select_list#7cff78a1-46cf-4a2f-8820-a04cce550726$`.
 27. `ActivateNextList`: Switch to the next list
 28. `ActivatePreviousList`: Switch to the previous list
 29. `ActivateListByGUID`: Activates the list associated with the given GUID `ActivateListByGUID#7cff78a1-46cf-4a2f-8820-a04cce550726$`

#### Output windows commands notes:
- `Fullscreen`, `Preview`, and `Presenter` commands accept additional parameter that can be passed after `#`
- accepted values are (`toggle` | `on` | `off`)
- for backward backward compatibility, CueTimer also supports `undefined` and empty string as a value and they will act as toggle. 
- Examples: 
  - `Fullscreen#on$`
  - `Fullscreen#off$`
  - `Fullscreen#toggle$`, `Fullscreen$`, and `Fullscreen#undefined$` will act as toggle


### Feedback:
Once the client is connected to CueTimer it will continuously receive info every 200ms. This will be JSON string that has a `$` as a termination char at the very end.

```JSON
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
  "NDI": false,
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

- `listNumber`: Indicates the list number (e.g., `1`, `2`, etc.) from which the feedback is coming.
- `listGUID`: The GUID of the list that generated the feedback.
- `lists`: An **ordered** array containing all available list objects:
  - `title`: The name of the list (without any numeric prefix such as `1-`, `2-`, etc.).
  - `guid`: The unique identifier (GUID) of the list.
  - `isActive`: Boolean indicating whether the list is currently active in the UI
- `h`: hours 
- `m`: minutes 
- `s`: seconds 
- `fg`: Foreground color in ARGB format (send #FF000000 if user clicked blackout)
- `bg`: Background color in ARGB format (send #FF000000 if user clicked blackout)
- `speed`: The percentage speed
- `name`: Timer name
- `endTime`: in ["hh:mm:ss"] format. (send "" if it has no value).
- `Fullscreen`: State of Fullscreen mode (Boolean)
- `Preview`: State of Preview mode (Boolean)
- `Presenter`: State of Presenter mode (Boolean)
- `NDI`: State of  (Boolean)
- `Message`: State of Message visibility (Boolean)
- `STM`: State of Single timer mode (Boolean)
- `Clock`: State of Clock screen (Boolean)
- `Pause`: State of pause button (Boolean)
- `Blackout`: State of Blackout button (Boolean)
- `nextTimerName`
- `nextTimerDuration`
- `scheduleOffset`: shows the difference between the end-time in the current red timer, and the next start-time in format `hh:mm:ss`. It can be +ve or -ve
- `scheduleOffsetStatus`: These commands could be useful if you want to have specific behaviour when the show is delayed or ahead it can have one of the 3 values `Delayed`, `Ahead`, `NA`
- `timers`: name, duration & background of the timers in the list, please not that you will only receive data of timers having their IDs set to a value.