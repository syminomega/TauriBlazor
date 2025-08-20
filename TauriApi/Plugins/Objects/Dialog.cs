using System.Text.Json.Serialization;

namespace TauriApi.Plugins;

/// <summary>
/// Options for the open dialog.
/// </summary>
/// <remarks>Since 2.0.0</remarks>
public abstract class OpenDialogOptions
{
    /// <summary>
    /// The title of the dialog window (desktop only).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; }

    /// <summary>
    /// The filters of the dialog.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DialogFilter[]? Filters { get; set; }

    /// <summary>
    /// Initial directory or file path.
    /// If it's a directory path, the dialog interface will change to that folder.
    /// If it's not an existing directory, the file name will be set to the dialog's file name input and the dialog will be set to the parent folder.
    /// <para>
    /// On mobile the file name is always used on the dialog's file name input.
    /// If not provided, Android uses <c>(invalid).txt</c> as default file name.
    /// </para>
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultPath { get; set; }

    /// <summary>
    /// Whether the dialog is a directory selection or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Directory { get; set; }

    /// <summary>
    /// If <see cref="Directory"/> is true, indicates that it will be read recursively later.
    /// Defines whether subdirectories will be allowed on the scope or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Recursive { get; set; }

    /// <summary>
    /// Whether to allow creating directories in the dialog. Enabled by default. <strong>macOS Only</strong>
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CanCreateDirectories { get; set; }
}

/// <inheritdoc />
public class OpenSingleDialogOptions : OpenDialogOptions
{
    /// <summary>
    /// Whether the dialog allows multiple selection or not.
    /// </summary>
    [JsonInclude]
    public bool Multiple => false;
}

/// <inheritdoc />
public class OpenMultipleDialogOptions : OpenDialogOptions
{
    /// <summary>
    /// Whether the dialog allows multiple selection or not.
    /// </summary>
    [JsonInclude]
    public bool Multiple => true;
}

/// <summary>
/// Extension filters for the file dialog.
/// </summary>
/// <remarks>Since 2.0.0</remarks>
public class DialogFilter
{
    /// <summary>
    /// Filter name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Extensions to filter, without a `.` prefix.
    /// <example>
    /// <code>
    /// Extensions = new[] { "svg", "png" }
    /// </code>
    /// </example>
    /// </summary>
    public string[] Extensions { get; set; } = Array.Empty<string>();
}
