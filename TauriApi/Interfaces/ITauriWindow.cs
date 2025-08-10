namespace TauriApi.Interfaces;

/// <summary>
/// Create new window or get a handle to an existing one.
/// Windows are identified by a label a unique identifier that can be used to reference it later.
/// It may only contain alphanumeric characters a-zA-Z plus the following special characters -, /, : and _.
/// </summary>
public interface ITauriWindow : ITauriObject
{
    /// <summary>
    /// The window label. It is a unique identifier for the window, can be used to reference it later.
    /// </summary>
    public ValueTask<string> Label { get; }
    
    // TODO: listeners
}