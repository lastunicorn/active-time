# Active Time - Use Cases (Overview)

## Present Overview

**actor**: user

**action**: ask to display overview information for specific time interval

**parameters**:

- First Day
- Last Day

**steps**:

- Retrieve all `DayRecord`s for the specified interval.
- Retrieve all `TimeRecord`s for each `DayRecord`.
- Returns an `OverviewReport`.

