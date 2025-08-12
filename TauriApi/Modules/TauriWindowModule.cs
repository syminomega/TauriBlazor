using Microsoft.JSInterop;
using TauriApi.Interfaces;
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

    /// <summary>
    /// Inject TauriWindow.
    /// </summary>
    public TauriWindowModule(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
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
        var window = new Window(windowRef, _tauriJsInterop);
        return window;
    }

    /// <summary>
    /// Gets a list of instances of Window for all available windows.
    /// </summary>
    /// <returns></returns>
    // TODO: Test this method.
    private async Task<ITauriWindow[]> GetAllWindows()
    {
        var windowRefs = await _jsRuntime.InvokeAsync<IJSObjectReference[]>($"{Prefix}.getAllWindows");
        var windows = new ITauriWindow[windowRefs.Length];
        for (var i = 0; i < windowRefs.Length; i++)
        {
            windows[i] = new Window(windowRefs[i], _tauriJsInterop);
        }
        return windows;
    }

    /// <summary>
    /// Get an instance of Window for the current window.
    /// </summary>
    /// <returns></returns>
    public async Task<ITauriWindow> GetCurrentWindow()
    {
        var windowRef = await _jsRuntime.InvokeAsync<IJSObjectReference>($"{Prefix}.getCurrentWindow");
        var window = new Window(windowRef, _tauriJsInterop);
        return window;
    }
}