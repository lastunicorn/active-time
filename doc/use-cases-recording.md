# Active Time - Use Cases (Main)

## Start Recording

**actor**: user

**action**: ask to start recording

**steps**:

- Use `Scribe` to stamp a new record.
- Start the `RecorderJob`.
- Raise the `Recorder.Started` event.
- Set status text to "Recorder started.".

## Stop Recording

**actor**: user

**action**: ask to stop recording

**steps**:

- If "DeleteLastRecord" was requested, use `Scribe` to delete the current record.
- If "DeleteLastRecord" was not requested, use `Scribe` to stamp current record.
- Stop the the `RecorderJob`.
- Raise the `Recorder.Stopped` event.
- Set status text to "Recorder stopped.".

## Stamp

**actor**: `RecorderJob`

**action**: Recorder Job is executed

**steps**:

- Set status text to "Updating record.".
- Use `Scribe` to stamp current record.
- Set status text to "Updated record.".

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

