using Microsoft.JSInterop;

namespace TauriApi.Plugins;

/// <summary>
/// The Tauri abstraction for reading and writing files.
/// </summary>
public class FileHandler : TauriResource
{
    internal FileHandler(IJSObjectReference jsObjectRef, long rid) : base(jsObjectRef, rid)
    {
    }
}

#region Interfaces

/// <summary>
/// Copy file options.
/// </summary>
/// <param name="FromPathBaseDir">Base directory for <c>fromPath</c>.</param>
/// <param name="ToPathBaseDir">Base directory for <c>toPath</c>.</param>
public record CopyFileOptions(
    BaseDirectory? FromPathBaseDir = null,
    BaseDirectory? ToPathBaseDir = null
);

/// <summary>
/// Create file options.
/// </summary>
/// <param name="BaseDir">Base directory for <c>path</c>.</param>
public record CreateOptions(
    BaseDirectory? BaseDir = null
);

/// <summary>
/// Debounce watch options.
/// </summary>
/// <param name="DelayMs">Debounce delay</param>
public record DebouncedWatchOptions(int? DelayMs = null) : WatchOptions;

//TODO: DirEntry

/// <summary>
/// Exists options.
/// </summary>
/// <param name="BaseDir">Base directory for <c>path</c>.</param>
public record ExistsOptions(
    BaseDirectory? BaseDir = null
);

/// <summary>
/// Watch options.
/// </summary>
/// <param name="BaseDir">Base directory for <c>path</c>.</param>
/// <param name="Recursive">Watch a directory recursively</param>
public record WatchOptions(
    BaseDirectory? BaseDir = null,
    bool? Recursive = null
);

#endregion