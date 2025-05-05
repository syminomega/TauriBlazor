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

    /// <summary>
    /// Returns the path to the user’s executable directory.
    /// <li>Linux: Resolves to <c>$XDG_BIN_HOME/../bin</c> or
    /// <c>$XDG_DATA_HOME/../bin</c> or <c>$HOME/.local/bin</c>.</li>
    /// <li>macOS: Not supported.</li>
    /// <li>Windows: Not supported.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> ExecutableDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.executableDir");
    }

    /// <summary>
    /// Returns the extension of the <c>path</c>.
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> ExtName()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.extname");
    }

    /// <summary>
    /// Returns the path to the user’s font directory.
    /// <li>Linux: Resolves to <c>$XDG_DATA_HOME/fonts</c> or <c>$HOME/.local/share/fonts</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Library/Fonts</c>.</li>
    /// <li>Windows: Not supported.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> FontDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.fontDir");
    }

    /// <summary>
    /// Returns the path to the user’s home directory.
    /// <li>Linux: Resolves to <c>$HOME</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_Profile}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> HomeDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.homeDir");
    }

    /// <summary>
    /// Returns whether the path is absolute or not.
    /// </summary>
    /// <returns></returns>
    public ValueTask<bool> IsAbsolute()
    {
        return _jsRuntime.InvokeAsync<bool>($"{Prefix}.isAbsolute");
    }

    /// <summary>
    /// Joins all given <c>path</c> segments together using the platform-specific separator as a delimiter,
    /// then normalizes the resulting path.
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    // TODO: not tested
    public ValueTask<string> Join(params string[] paths)
    {
        var pathList = paths.ToList();
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.join", pathList);
    }

    /// <summary>
    /// Returns the path to the user’s local data directory.
    /// <li>Linux: Resolves to <c>$XDG_DATA_HOME</c> or <c>$HOME/.local/share</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Library/Application Support</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_LocalAppData}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> LocalDataDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.localDataDir");
    }

    // ReSharper disable once GrammarMistakeInComment
    /// <summary>
    /// Normalizes the given <c>path</c>, resolving <c>'..'</c> and <c>'.'</c> segments and resolve symbolic links.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public ValueTask<string> Normalize(string path)
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.normalize", path);
    }

    /// <summary>
    /// Returns the path to the user’s picture directory.
    /// <li>Linux: Resolves to <a href="https://www.freedesktop.org/wiki/Software/xdg-user-dirs/">xdg-user-dir</a>'s
    /// <c>XDG_PICTURES_DIR</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Pictures</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_Pictures}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> PictureDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.pictureDir");
    }

    /// <summary>
    /// Returns the path to the user’s public directory.
    /// <li>Linux: Resolves to <a href="https://www.freedesktop.org/wiki/Software/xdg-user-dirs/">xdg-user-dir</a>'s
    /// <c>XDG_PUBLICSHARE_DIR</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Public</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_Public}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> PublicDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.publicDir");
    }

    /// <summary>
    /// Resolves a sequence of <c>paths</c> or <c>path</c> segments into an absolute path.
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    // TODO: not tested
    public ValueTask<string> Resolve(params string[] paths)
    {
        var pathList = paths.ToList();
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.resolve", pathList);
    }

    /// <summary>
    /// Resolve the path to a resource file.
    /// </summary>
    /// <param name="resourcePath">The path to the resource. Must follow the same syntax as defined in
    /// <c>tauri.conf.json > bundle > resources</c>, i.e. keeping subfolders and parent dir components (../).</param>
    /// <returns>The full path to the resource.</returns>
    public ValueTask<string> ResolveResource(string resourcePath)
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.resolveResource", resourcePath);
    }

    /// <summary>
    /// Returns the path to the application’s resource directory.
    /// To resolve a resource path, see the [[resolveResource | resolveResource API]].
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> ResourceDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.resourceDir");
    }

    /// <summary>
    /// Returns the path to the user’s runtime directory.
    /// <li>Linux: Resolves to <c>$XDG_RUNTIME_DIR</c>.</li>
    /// <li>macOS: Not supported.</li>
    /// <li>Windows: Not supported.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> RuntimeDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.runtimeDir");
    }

    /// <summary>
    /// Returns the platform-specific path segment separator:
    /// <li><c>\</c> on Windows</li>
    /// <li><c>/</c> on POSIX</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> Sep()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.sep");
    }

    /// <summary>
    /// Returns a temporary directory.
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> TempDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.tempDir");
    }

    /// <summary>
    /// Returns the path to the user’s template directory.
    /// <li>Linux: Resolves to <a href="https://www.freedesktop.org/wiki/Software/xdg-user-dirs/">xdg-user-dir</a>'s
    /// <c>XDG_TEMPLATES_DIR</c>.</li>
    /// <li>macOS: Not supported.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_Templates}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> TemplateDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.templateDir");
    }

    /// <summary>
    /// Returns the path to the user’s video directory.
    /// <li>Linux: Resolves to <a href="https://www.freedesktop.org/wiki/Software/xdg-user-dirs/">xdg-user-dir</a>'s
    /// <c>XDG_VIDEOS_DIR</c>.</li>
    /// <li>macOS: Resolves to <c>$HOME/Movies</c>.</li>
    /// <li>Windows: Resolves to <c>{FOLDERID_Videos}</c>.</li>
    /// </summary>
    /// <returns></returns>
    public ValueTask<string> VideoDir()
    {
        return _jsRuntime.InvokeAsync<string>($"{Prefix}.videoDir");
    }
}