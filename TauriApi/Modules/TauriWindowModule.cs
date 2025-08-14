using Microsoft.JSInterop;
using TauriApi.Utilities;

namespace TauriApi.Modules;

/// <summary>
/// Provides APIs to create windows, communicate with other windows and manipulate the current window.
/// </summary>
public class TauriWindowModule
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.window";
    private readonly TauriJsInterop _tauriJsInterop;
    private readonly TauriEventModule _tauriEvent;

    /// <summary>
    /// Inject TauriWindow.
    /// </summary>
    public TauriWindowModule(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop, TauriEventModule tauriEvent)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
        _tauriEvent = tauriEvent;
    }

    /// <summary>
    /// Creates a new Window.
    /// </summary>
    /// <param name="label">The unique window label. Must be alphanumeric: a-zA-Z-/:_.</param>
    /// <param name="options"></param>
    /// <returns>The Window instance to communicate with the window.</returns>
    public async Task<ITauriWindow> CreateWindow(string label, WindowOptions? options)
    {
        var windowRef = await _tauriJsInterop.ConstructWindow(label, options);
        var window = new TauriWindow(windowRef, _tauriJsInterop, _tauriEvent);
        return window;
    }

    /// <summary>
    /// Gets the Window associated with the given label.
    /// </summary>
    /// <param name="label">The window label.</param>
    /// <returns>The Window instance to communicate with the window or null if the window doesn't exist.</returns>
    public async Task<ITauriWindow?> GetWindowByLabel(string label)
    {
        var allWindows = await GetAllWindows();
        foreach (var window in allWindows)
        {
            var windowLabel = await window.Label;
            if (windowLabel == label)
            {
                return window;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a list of instances of Window for all available windows.
    /// </summary>
    public async Task<ITauriWindow[]> GetAllWindows()
    {
        // 先获取数组长度
        var allWindows = await _jsRuntime.InvokeAsync<IJSObjectReference>($"{Prefix}.getAllWindows");
        var windowCount = await _tauriJsInterop.GetJsProperty<int>(allWindows, "length");

        var windows = new ITauriWindow[windowCount];
        for (var i = 0; i < windowCount; i++)
        {
            var windowRef = await _tauriJsInterop.GetJsProperty<IJSObjectReference>(allWindows, $"[{i}]");
            windows[i] = new TauriWindow(windowRef, _tauriJsInterop, _tauriEvent);
        }

        return windows;
    }

    /// <summary>
    /// Get an instance of Window for the current window.
    /// </summary>
    public async Task<ITauriWindow> GetCurrentWindow()
    {
        var windowRef = await _jsRuntime.InvokeAsync<IJSObjectReference>($"{Prefix}.getCurrentWindow");
        var window = new TauriWindow(windowRef, _tauriJsInterop, _tauriEvent);
        return window;
    }

    /// <summary>
    /// Gets the focused window.
    /// </summary>
    public async Task<ITauriWindow?> GetFocusedWindow()
    {
        var allWindows = await GetAllWindows();
        foreach (var window in allWindows)
        {
            var isFocused = await window.IsFocused();
            if (isFocused)
            {
                return window;
            }
        }

        return null;
    }
}