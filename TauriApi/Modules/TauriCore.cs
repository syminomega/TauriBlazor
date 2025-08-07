using Microsoft.JSInterop;

namespace TauriApi.Modules;

/// <summary>
/// Invoke your custom commands.
/// </summary>
public class TauriCore
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.core";

    /// <summary>
    /// Inject TauriCore.
    /// </summary>
    public TauriCore(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    #region Invoke

    /// <summary>
    /// Sends a message to the backend.
    /// </summary>
    /// <param name="cmd">The command name.</param>
    /// <param name="paramObject">The optional arguments to pass to the command.</param>
    /// <typeparam name="T">Response message type.</typeparam>
    /// <returns>The backend response.</returns>
    public async Task<T> Invoke<T>(string cmd, object paramObject)
    {
        return await _jsRuntime.InvokeAsync<T>($"{Prefix}.invoke", cmd, paramObject);
    }

    /// <summary>
    /// Sends a message to the backend.
    /// </summary>
    /// <param name="cmd">The command name.</param>
    /// <typeparam name="T">Response message type.</typeparam>
    /// <returns>The backend response.</returns>
    public async Task<T> Invoke<T>(string cmd)
    {
        return await _jsRuntime.InvokeAsync<T>($"{Prefix}.invoke", cmd);
    }

    /// <summary>
    /// Sends a message to the backend.
    /// </summary>
    /// <param name="cmd">The command name.</param>
    /// <param name="paramObject">The optional arguments to pass to the command.</param>
    public async Task Invoke(string cmd, object paramObject)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.invoke", cmd, paramObject);
    }

    /// <summary>
    /// Sends a message to the backend.
    /// </summary>
    /// <param name="cmd">The command name.</param>
    public async Task Invoke(string cmd)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.invoke", cmd);
    }

    #endregion

    /// <summary>
    /// Convert a device file path to a URL that can be loaded by the webview.
    /// Note that <c>asset:</c> and <c>http://asset.localhost</c> must be added to 
    /// <a href="https://v2.tauri.app/reference/config/#csp-1">app.security.csp</a> in <c>tauri.conf.json</c>.
    /// Example CSP value:
    /// <c>"csp": "default-src 'self' ipc: http://ipc.localhost; img-src 'self' asset: http://asset.localhost"</c>
    /// to use the asset protocol on image sources.
    /// <para>Additionally, <c>"enable" : "true"</c> must be added to
    /// <a href="https://v2.tauri.app/reference/config/#assetprotocolconfig">app.security.assetProtocol</a> in <c>tauri.conf.json</c>
    /// and its access scope must be defined on the <c>scope</c> array on the same <c>assetProtocol</c> object.</para>
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="protocol">The protocol to use. Defaults to asset. You only need to set this when using a custom protocol.</param>
    /// <returns>the URL that can be used as source on the webview.</returns>
    public async Task<string> ConvertFileSrc(string filePath, string protocol = "asset")
    {
        return await _jsRuntime.InvokeAsync<string>($"{Prefix}.convertFileSrc", filePath, protocol);
    }

    /// <summary>
    /// Checks if the app is running in Tauri.
    /// </summary>
    public ValueTask<bool> IsTauri()
    {
        try
        {
            return _jsRuntime.InvokeAsync<bool>($"{Prefix}.isTauri");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error checking if running in Tauri: {e.Message}");
            return new ValueTask<bool>(false);
        }
        
    }
}