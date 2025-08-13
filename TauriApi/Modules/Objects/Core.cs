using Microsoft.JSInterop;
using TauriApi.Interfaces;

namespace TauriApi;

/// <summary>
/// A rust-backed resource stored through tauri::Manager::resources_table API.
/// The resource lives in the main process and does not exist in the Javascript world,
/// and thus will not be cleaned up automatically except on application exit.
/// If you want to clean it up early, call <see cref="TauriResource.Close"/>
/// </summary>
public abstract class TauriResource : ITauriObject
{
    /// <summary>
    /// Creates a new resource from a JSObjectReference and a resource id.
    /// </summary>
    /// <param name="jsObjectRef"></param>
    /// <param name="rid"></param>
    protected TauriResource(IJSObjectReference jsObjectRef, long rid)
    {
        JsObjectRef = jsObjectRef;
        Rid = rid;
    }

    /// <summary>
    /// ResourceId
    /// </summary>
    public long Rid { get; }

    /// <summary>
    /// Destroys and cleans up this resource from memory.
    /// <b>You should not call any method on this object anymore and should drop any reference to it.</b>
    /// </summary>
    public async Task Close()
    {
        await JsObjectRef.InvokeVoidAsync("close");
    }

    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }
}