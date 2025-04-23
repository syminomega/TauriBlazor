using Microsoft.JSInterop;
using TauriApi.Utilities;

namespace TauriApi.Modules;

public class TauriEvent
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.event";
    private readonly TauriJsInterop _tauriJsInterop;

    public TauriEvent(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
    }
    
}