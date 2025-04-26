using Microsoft.JSInterop;

#pragma warning disable CS1591

namespace TauriApi.Utilities;

/// <summary>
/// TauriEventManager is responsible for managing the event handlers.
/// </summary>
public class TauriEventManager
{
    private readonly List<ITauriEventHandler> _eventHandlers = new();

    public void RemoveEventHandler(ITauriEventHandler handler)
    {
        _eventHandlers.Remove(handler);
    }

    public TauriEventHandler CreateEventHandler(Action callback)
    {
        var eventHandler = new TauriEventHandler(this, callback);
        _eventHandlers.Add(eventHandler);
        return eventHandler;
    }

    public TauriEventHandler<T> CreateEventHandler<T>(Action<T> callback)
    {
        var eventHandler = new TauriEventHandler<T>(this, callback);
        _eventHandlers.Add(eventHandler);
        return eventHandler;
    }
}

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
    public void InvokeEvent(object? payload);

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
    internal TauriEventHandler(TauriEventManager eventManager, Action callback)
    {
        _eventManager = eventManager;
        _callback = callback;
    }

    /// <inheritdoc />
    public IJSObjectReference? HandlerRef { get; set; }

    private readonly TauriEventManager _eventManager;


    [JSInvokable]
    public void InvokeEvent(object? payload)
    {
        _callback.Invoke();
    }

    private readonly Action _callback;

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
            await HandlerRef.InvokeVoidAsync("unlisten");
            _eventManager.RemoveEventHandler(this);
            await HandlerRef.DisposeAsync();
        };
}

/// <inheritdoc />
public class TauriEventHandler<T> : ITauriEventHandler
{
    internal TauriEventHandler(TauriEventManager eventManager, Action<T> callback)
    {
        _eventManager = eventManager;
        _callback = callback;
    }

    /// <inheritdoc />
    public IJSObjectReference? HandlerRef { get; set; }

    private readonly TauriEventManager _eventManager;

    [JSInvokable]
    public void InvokeEvent(T payload)
    {
        _callback.Invoke(payload);
    }

    void ITauriEventHandler.InvokeEvent(object? payload)
    {
        if (payload is T typedPayload)
        {
            InvokeEvent(typedPayload);
        }
        else
        {
            throw new InvalidCastException($"Cannot cast payload to type {typeof(T)}.");
        }
    }

    private readonly Action<T> _callback;

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
            await HandlerRef.InvokeVoidAsync("unlisten");
            _eventManager.RemoveEventHandler(this);
            await HandlerRef.DisposeAsync();
        };
}