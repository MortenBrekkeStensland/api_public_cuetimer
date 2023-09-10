## CueTimer API

### Protocol:
CueTimer has a TCP server that waits for a client connection on a default port (4778) and can be configured from the app settings. Once a connection is instantiated between the client and CueTimer it will continuously listen to the incoming messages from that client.
The commands should be sent as an ASCII string with `$` as a termination char at
the end of it (e.g. FireNext$).


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
 11. `Fullscreen`: Togglle Fullscreen mode
 12. `NDI`: Togglle NDI
 13. `Message`: Togglle Message visibilty
 14. `Clock`: Toggle clock screen
 15. `MoveNextUp`: Move next indicator one row up
 16. `MoveNextDown`: Move next indicator one row down
 17. `FireTimerWithID`: Start a timer with its id, the id can be passed after `#`, example `FireTimerWithID#1$`, please note that `#` can't be used in timer ID
 18. `CueTimerWithID`: Move next indicator to a timer with its id, the id can be passed after `#`, example `CueTimerWithID#1$`, please note that `#` can't be used in timer ID
 19. `STM`: Togglle Single timer mode

### Feedback:
Once the client is connected to CueTimer it will continuously receive info every 200ms. This will be JSON string that has a `$` as a termination char at the very end.

```JSON
{
  "h": "0",
  "m": "31",
  "s": "8",
  "fg": "#FFFFFFFF",
  "bg": "#FF696969",
  "speed": "100",
  "name": "",
  "endTime": "22:12:37",
  "Fullscreen": false,
  "NDI": false,
  "Message": false,
  "STM": false,
  "Clock": false,
  "Pause": false,
  "Blackout": false,
  "nextTimerName": "",
  "nextTimerDuration": "01:00:00",
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


- `h`: hours 
- `m`: minutes 
- `s`: seconds 
- `fg`: Foreground color in ARGB format (send #FF000000 if user clicked blackout)
- `bg`: Background color in ARGB format (send #FF000000 if user clicked blackout)
- `speed`: The percentage speed
- `name`: Timer name
- `endTime`: in ["hh:mm:ss"] format. (send "" if it has no value).
- `Fullscreen`: State of Fullscreen mode (Boolean)
- `NDI`: State of  (Boolean)
- `Message`: State of Message visibility (Boolean)
- `STM`: State of Single timer mode (Boolean)
- `Clock`: State of Clock screen (Boolean)
- `Pause`: State of pause button (Boolean)
- `Blackout`: State of Blackout button (Boolean)
- `nextTimerName`
- `nextTimerDuration`
- `timers`: name, duration & background of the timers in the list, please not that you will only receive data of timers having their IDs set to a value.