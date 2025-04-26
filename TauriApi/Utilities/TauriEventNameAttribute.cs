namespace TauriApi.Utilities;

/// <summary>
/// Attribute to specify the name of the build-in Tauri event.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class TauriEventNameAttribute: Attribute
{
    /// <summary>
    /// Constructor to set the event name.
    /// </summary>
    /// <param name="eventName"></param>
    public TauriEventNameAttribute(string eventName)
    {
        EventName = eventName;
    }
    /// <summary>
    /// Name of the event
    /// </summary>
    public string EventName { get; }
}