using Microsoft.JSInterop;

namespace TauriApi;

public class Window : IAsyncDisposable
{
    internal Window(IJSObjectReference windowRef)
    {
        _ref = windowRef;
    }

    private readonly IJSObjectReference _ref;

    /// <summary>
    /// Centers the window.
    /// </summary>
    public async Task Center()
    {
        await _ref.InvokeVoidAsync("center");
    }

    /// <summary>
    /// Clear any applied effects if possible.
    /// </summary>
    public async Task ClearEffects()
    {
        await _ref.InvokeVoidAsync("clearEffects");
    }

    /// <summary>
    /// Closes the window.
    /// Note this emits a closeRequested event so you can intercept it. To force window close, use <see cref="Destroy"/>.
    /// </summary>
    public async Task Close()
    {
        await _ref.InvokeVoidAsync("close");
    }

    /// <summary>
    /// Destroys the window. Behaves like <see cref="Close"/> but forces the window close instead of emitting a closeRequested event.
    /// </summary>
    public async Task Destroy()
    {
        await _ref.InvokeVoidAsync("destroy");
    }

    public ValueTask DisposeAsync()
    {
        return _ref.DisposeAsync();
    }
}

#region Interfaces

public class Monitor
{
}

public record WindowOptions
{
    public bool? AlwaysOnBottom { get; init; }

    public bool? AlwaysOnTop { get; init; }

    // backgroundColor
    // backgroundThrottling
    public bool? Center { get; init; }
    public bool? Closable { get; init; }
    public bool? ContentProtected { get; init; }
    public bool? Decorations { get; init; }
    public bool? Focus { get; init; }
    public bool? Fullscreen { get; init; }
    public int? Height { get; init; }
    public bool? HiddenTitle { get; init; }
}

#endregion

#region Type Aliases

public enum Theme
{
    Light,
    Dark,
    System
}

#endregion