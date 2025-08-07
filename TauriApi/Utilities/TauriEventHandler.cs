using Microsoft.JSInterop;

#pragma warning disable CS1591

namespace TauriApi.Utilities;

/// <summary>
/// Interface for TauriEventHandler.
/// </summary>
public interface ITauriEventHandler
{
    /// <summary>
    /// Reference to the JavaScript object that handles the event.
    /// </summary>
    public IJSObjectReference? HandlerRef { get; }

    /// <summary>
    /// Invokes the event callback.
    /// </summary>
    /// <param name="payload"></param>
    public Task InvokeEvent(object? payload);

    /// <summary>
    /// Unlisten function to remove the event handler.
    /// </summary>
    /// <exception cref="ObjectDisposedException"></exception>
    public UnlistenFn Unlisten { get; }

    /// <summary>
    /// Indicates whether the event handler has been disposed.
    /// </summary>
    public bool Disposed { get; }
}

/// <inheritdoc />
public class TauriEventHandler : ITauriEventHandler
{
    internal TauriEventHandler(Func<Task> callback, bool once)
    {
        _callback = callback;
        _once = once;
    }

    /// <inheritdoc />
    public IJSObjectReference? HandlerRef { get; set; }
    

    private readonly bool _once;
    private bool _onceTriggered;

    [JSInvokable]
    public async Task InvokeEvent(object? payload)
    {
        await _callback.Invoke();
        if (_once)
        {
            _onceTriggered = true;
            if (HandlerRef == null)
            {
                throw new NullReferenceException("HandlerRef is null. Did you forget to set it?");
            }

            await HandlerRef.InvokeVoidAsync("unlisten");
            await HandlerRef.DisposeAsync();
        }
    }

    private readonly Func<Task> _callback;

    /// <inheritdoc />
    public bool Disposed { get; private set; }

    /// <inheritdoc />
    public UnlistenFn Unlisten =>
        async () =>
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(TauriEventHandler),
                    "This event handler has already been disposed.");
            }

            if (HandlerRef == null)
            {
                throw new NullReferenceException("HandlerRef is null. Did you forget to set it?");
            }

            Disposed = true;
            // Trigger unlisten in tauri
            if (!_onceTriggered)
            {
                await HandlerRef.InvokeVoidAsync("unlisten");
                await HandlerRef.DisposeAsync();
            }
        };
}

/// <inheritdoc />
public class TauriEventHandler<T> : ITauriEventHandler
{
    internal TauriEventHandler(Func<T, Task> callback, bool once)
    {
        _callback = callback;
        _once = once;
    }

    /// <inheritdoc />
    public IJSObjectReference? HandlerRef { get; set; }

    private readonly bool _once;
    private bool _onceTriggered;

    [JSInvokable]
    public async Task InvokeEvent(T payload)
    {
        await _callback.Invoke(payload);
        if (_once)
        {
            _onceTriggered = true;
            if (HandlerRef == null)
            {
                throw new NullReferenceException("HandlerRef is null. Did you forget to set it?");
            }

            await HandlerRef.InvokeVoidAsync("unlisten");
            await HandlerRef.DisposeAsync();
        }
    }

    async Task ITauriEventHandler.InvokeEvent(object? payload)
    {
        if (payload is T typedPayload)
        {
            await InvokeEvent(typedPayload);
        }
        else
        {
            throw new InvalidCastException($"Cannot cast payload to type {typeof(T)}.");
        }
    }

    private readonly Func<T, Task> _callback;

    /// <inheritdoc />
    public bool Disposed { get; private set; }

    /// <inheritdoc />
    public UnlistenFn Unlisten =>
        async () =>
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(TauriEventHandler),
                    "This event handler has already been disposed.");
            }

            if (HandlerRef == null)
            {
                throw new NullReferenceException("HandlerRef is null. Did you forget to set it?");
            }

            Disposed = true;
            // Trigger unlisten in tauri
            if (!_onceTriggered)
            {
                await HandlerRef.InvokeVoidAsync("unlisten");
                await HandlerRef.DisposeAsync();
            }
        };
}