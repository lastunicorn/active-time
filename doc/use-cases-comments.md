# Active Time - Use Cases (Comments)

## Present Comments

**actor**: user

**action**: comments tab is selected

**steps**:

- Use `Scribe` to stamp a new record.
- Start the `RecorderJob`.
- Raise the `Recorder.Started` event.
- Set status text to "Recorder started.".