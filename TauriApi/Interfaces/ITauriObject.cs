using Microsoft.JSInterop;

namespace TauriApi.Interfaces;

/// <summary>
/// Base interface for all Tauri objects.
/// </summary>
public interface ITauriObject : IAsyncDisposable
{
    /// <summary>
    /// The JS object reference from tauri api.
    /// </summary>
    public IJSObjectReference JsObjectRef { get; }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await JsObjectRef.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}