using Microsoft.JSInterop;
using TauriApi.Utilities;

namespace TauriApi.Modules;

public class TauriWindow
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.window";
    private readonly TauriJsInterop _tauriJsInterop;

    public TauriWindow(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
    }

    public async Task<Window> CreateWindow(string label, WindowOptions? options)
    {
        var windowRef = await _tauriJsInterop.ConstructWindow(label, options);
        var window = new Window(windowRef);
        return window;
    }

    public async Task<Window> GetCurrentWindow()
    {
        var windowRef = await _jsRuntime.InvokeAsync<IJSObjectReference>($"{Prefix}.getCurrentWindow");
        var window = new Window(windowRef);
        return window;
    }
}