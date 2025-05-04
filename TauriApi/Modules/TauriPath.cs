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

    /// <summary>
    /// Returns the path to the user’s audio directory.
    /// <li>Linux: Resolves to <a href="https://www.freedesktop.org/wiki/Software/xdg-user-dirs/">xdg-user-dir</a>'s
    /// <c>XDG_MUSIC_DIR</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Music</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_Music}</c>.</li>
    /// </summary>
    public ValueTask<string> AudioDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.audioDir");
    }
    
    /// <summary>
    /// Returns the last portion of a <c>path</c>. Trailing directory separators are ignored.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="ext">An optional file extension to be removed from the returned path.</param>
    /// <returns></returns>
    public ValueTask<string> BaseName(string path, string? ext = null)
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.basename", path, ext);
    }

    /// <summary>
    /// Returns the path to the user’s cache directory.
    /// <li>Linux: Resolves to <c>$XDG_CACHE_HOME</c> or <c>$HOME/.cache</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Library/Caches</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_LocalAppData}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> CacheDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.cacheDir");
    }

    /// <summary>
    /// Returns the path to the user’s config directory.
    /// <li>Linux: Resolves to <c>$XDG_CONFIG_HOME</c> or <c>$HOME/.config</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Library/Application Support</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_RoamingAppData}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> ConfigDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.configDir");
    }

    /// <summary>
    /// Returns the path to the user’s data directory.
    /// <li>Linux: Resolves to <c>$XDG_DATA_HOME</c> or <c>$HOME/.local/share</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Library/Application Support</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_RoamingAppData}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> DataDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.dataDir");
    }

    /// <summary>
    /// Returns the platform-specific path segment delimiter:
    /// <li><c>;</c> on Windows</li>
    /// <li><c>:</c> on POSIX</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> Delimiter()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.delimiter");
    }

    /// <summary>
    /// Returns the path to the user’s desktop directory.
    /// <li>Linux: Resolves to <a href="https://www.freedesktop.org/wiki/Software/xdg-user-dirs/">xdg-user-dir</a>'s
    /// <c>XDG_DESKTOP_DIR</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Desktop</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_Desktop}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> DesktopDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.desktopDir");
    }

    /// <summary>
    /// Returns the directory name of a <c>path</c>. Trailing directory separators are ignored.
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> Dirname()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.dirname");
    }

    /// <summary>
    /// Returns the path to the user’s document directory.
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> DocumentDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.documentDir");
    }

    /// <summary>
    /// Returns the path to the user’s download directory.
    /// <li>Linux: Resolves to <a href="https://www.freedesktop.org/wiki/Software/xdg-user-dirs/">xdg-user-dir</a>'s
    /// <c>XDG_DOWNLOAD_DIR</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Downloads</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_Downloads}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> DownloadDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.downloadDir");
    }
}