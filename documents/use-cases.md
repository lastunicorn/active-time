# Active Time - Use Cases

## Start Recording

**actor**: user

**action**: ask to start recording

**steps**:

- Create new `TimeRecord` starting with current time and add it to repository.
- Start the the `RecorderTimer`.
- Raise the `RecorderStarted` event.

## Stamp

**actor**: `RecorderTimer`

**action**: Timer rings

**steps**:

- Retrieve the last `TimeRecord` from the repository.
- Update the `EndDate` of the `TimeRecord` to be the current time.

## Stop Recording

**actor**: user

**action**: ask to stop recording

**steps**:

- Stop the the `RecorderTimer`.
- Raise the `RecorderStopped` event.

## Present Information for Current Date 

**actor**: user

**action**: ask to display information for the current date

**steps**:

- Retrieve the current date.
- Retrieve `DayRecord` for current date.
- Retrieve all `TimeRecord`s for the current date.
- Calculate start time and end time.
- Present the day comments, start time and end time.

## Present time information for Current Date 

**actor**: user

**action**: ask to display time information for the current date

**steps**:

- Retrieve the current date.
- Retrieve all `TimeRecord`s for the current date.
- Calculate start time and end time.
- Calculate the active and inactive time intervals.
- Present the list of active and inactive time intervals, the start time and the end time.

## Set Current Date

**actor**: user

**action**: ask to set the current date

**steps**:

- Retrieve the current date.
- Set the new current date.
- Raise the `CurrentDateChanged` event.

## Go to Next Date

**actor**: user

**action**: ask to change current date to next day

**steps**:

- Retrieve the current date.
- Set the current date to current date +1 day.
- Raise the `CurrentDateChanged` event.

## Go to Previous Date

**actor**: user

**action**: ask to change current date to previous day

**steps**:

- Retrieve the current date.
- Set the current date to current date -1 day.
- Raise the `CurrentDateChanged` event.

