using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;

namespace TauriApi.Plugins;

/// <summary>
/// Provides APIs to show dialogs.
/// </summary>
public class TauriDialog : ITauriPlugin
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.dialog";
    private readonly TauriJsInterop _tauriJsInterop;

    /// <summary>
    /// Inject TauriDialog.
    /// </summary>
    public TauriDialog(IJSRuntime jsRuntime,
        TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
    }

    /// <summary>
    /// Open a file/directory selection dialog.
    /// <para/>The selected paths are added to the filesystem and asset protocol scopes.
    /// When security is more important than the easy of use of this API, prefer writing a dedicated command instead.
    /// <para/>Note that the scope change is not persisted, so the values are cleared when the application is restarted.
    /// You can save it to the filesystem using
    /// <a href="https://github.com/tauri-apps/tauri-plugin-persisted-scope">tauri-plugin-persisted-scope</a>.
    /// </summary>
    /// <param name="options">Open dialog options.</param>
    /// <returns>The selected path or null if the dialog was canceled.</returns>
    public async Task<string?> Open(OpenSingleDialogOptions options)
    {
        return await _jsRuntime.InvokeAsync<string?>($"{Prefix}.open", options);
    }

    /// <summary>
    /// Open a file/directory selection dialog.
    /// <para/>The selected paths are added to the filesystem and asset protocol scopes.
    /// When security is more important than the easy of use of this API, prefer writing a dedicated command instead.
    /// <para/>Note that the scope change is not persisted, so the values are cleared when the application is restarted.
    /// You can save it to the filesystem using
    /// <a href="https://github.com/tauri-apps/tauri-plugin-persisted-scope">tauri-plugin-persisted-scope</a>.
    /// </summary>
    /// <param name="options">Open dialog options.</param>
    /// <returns>A list of selected paths or null if the dialog was canceled.</returns>
    public async Task<List<string>?> Open(OpenMultipleDialogOptions options)
    {
        return await _jsRuntime.InvokeAsync<List<string>?>($"{Prefix}.open", options);
    }
}