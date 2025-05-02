using Microsoft.JSInterop;

namespace TauriApi.Modules;

/// <summary>
/// The path module provides utilities for working with file and directory paths.
/// It is recommended to allowlist only the APIs you use for optimal bundle size and security.
/// </summary>
public class TauriPath
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.path";

    /// <summary>
    /// Inject TauriPath.
    /// </summary>
    public TauriPath(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Returns the path to the suggested directory for your app’s cache files.
    /// Resolves to <c>${cacheDir}/${bundleIdentifier}</c>, where <c>bundleIdentifier</c> is
    /// the <a href="https://v2.tauri.app/reference/config/#identifier">identifier</a> value
    /// configured in <c>tauri.conf.json</c>.
    /// </summary>
    public ValueTask<string> AppCacheDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.appCacheDir");
    }

    /// <summary>
    /// Returns the path to the suggested directory for your app’s config files.
    /// Resolves to <c>${cacheDir}/${bundleIdentifier}</c>, where <c>bundleIdentifier</c> is
    /// the <a href="https://v2.tauri.app/reference/config/#identifier">identifier</a> value
    /// configured in <c>tauri.conf.json</c>.
    /// </summary>
    public ValueTask<string> AppConfigDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.appConfigDir");
    }
    
    /// <summary>
    /// Returns the path to the suggested directory for your app’s data files.
    /// Resolves to <c>${cacheDir}/${bundleIdentifier}</c>, where <c>bundleIdentifier</c> is
    /// the <a href="https://v2.tauri.app/reference/config/#identifier">identifier</a> value
    /// configured in <c>tauri.conf.json</c>.
    /// </summary>
    public ValueTask<string> AppDataDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.appDataDir");
    }
    
    /// <summary>
    /// Returns the path to the suggested directory for your app’s local data files.
    /// Resolves to <c>${cacheDir}/${bundleIdentifier}</c>, where <c>bundleIdentifier</c> is
    /// the <a href="https://v2.tauri.app/reference/config/#identifier">identifier</a> value
    /// configured in <c>tauri.conf.json</c>.
    /// </summary>
    public ValueTask<string> AppLocalDataDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.appLocalDataDir");
    }

    /// <summary>
    /// Returns the path to the suggested directory for your app’s log files.
    /// <li>Linux: Resolves to <c>${configDir}/${bundleIdentifier}/logs</c>.</li>
    /// <li>macOS: Resolves to <c>${homeDir}/Library/Logs/{bundleIdentifier}</c>.</li>
    /// <li>Windows: Resolves to <c>${configDir}/${bundleIdentifier}/logs</c>.</li>
    /// </summary>
    public ValueTask<string> AppLogDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.appLogDir");
    }

}