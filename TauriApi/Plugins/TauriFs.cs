using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;

namespace TauriApi.Plugins;

/// <summary>
/// Access the file system.
/// </summary>
public class TauriFs : ITauriPlugin
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.fs";
    private readonly TauriJsInterop _tauriJsInterop;

    /// <summary>
    /// Inject TauriFs.
    /// </summary>
    public TauriFs(IJSRuntime jsRuntime,
        TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
    }

    /// <summary>
    /// Copies the contents and permissions of one file to another specified path,
    /// by default creating a new file if needed, else overwriting.
    /// </summary>
    /// <param name="fromPath"></param>
    /// <param name="toPath"></param>
    /// <param name="options"></param>
    public async Task CopyFile(string fromPath, string toPath, CopyFileOptions? options = null)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.copyFile", fromPath, toPath, options);
    }

    /// <summary>
    /// Creates a file if none exists or truncates an existing file
    /// and resolves to an instance of <see cref="FileHandler"/>.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<FileHandler> Create(string path, CreateOptions? options = null)
    {
        var jsObjectRef = await _jsRuntime.InvokeAsync<IJSObjectReference>($"{Prefix}.create", path, options);
        var rid = await _tauriJsInterop.GetJsProperty<long>(jsObjectRef, "rid");
        return new FileHandler(jsObjectRef, rid);
    }
    
    /// <summary>
    /// Check if a path exists.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<bool> Exists(string path, ExistsOptions? options = null)
    {
        return await _jsRuntime.InvokeAsync<bool>($"{Prefix}.exists", path, options);
    }
}