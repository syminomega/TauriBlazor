using Microsoft.JSInterop;
using TauriApi.Interfaces;

namespace TauriApi;

public class Channel<T> : ITauriObject
{
    public IJSObjectReference JsObjectRef { get; }
}

/// <summary>
/// A rust-backed resource stored through tauri::Manager::resources_table API.
/// The resource lives in the main process and does not exist in the Javascript world,
/// and thus will not be cleaned up automatically except on application exit.
/// If you want to clean it up early, call <see cref="Resource.Close"/>
/// </summary>
public abstract class Resource
{
    /// <summary>
    /// ResourceId
    /// </summary>
    public abstract long Rid { get; }

    /// <summary>
    /// Destroys and cleans up this resource from memory.
    /// <b>You should not call any method on this object anymore and should drop any reference to it.</b>
    /// </summary>
    public abstract Task Close();
}