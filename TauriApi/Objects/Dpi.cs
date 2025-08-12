namespace TauriApi;

/// <summary>
/// A size represented in logical pixels.
/// Logical pixels are scaled according to the window's DPI scale.
/// Most browser APIs (i.e. <c>MouseEvent</c>'s <c>clientX</c>) will return logical pixels.
/// </summary>
/// <remarks>
/// For logical-pixel-based position, see <see cref="LogicalPosition"/>.
/// </remarks>
/// <since>2.0.0</since>
/// <param name="Width">The width in logical pixels.</param>
/// <param name="Height">The height in logical pixels.</param>
public record LogicalSize(int Width, int Height)
{
    /// <summary>
    /// The type of size representation.
    /// </summary>
    public string Type { get; init; } = "Logical";
};

/// <summary>
/// A size represented in physical pixels.
/// Physical pixels represent actual screen pixels, and are DPI-independent.
/// For high-DPI windows, this means that any point in the window on the screen
/// will have a different position in logical pixels (<see cref="LogicalSize"/>).
/// </summary>
/// <remarks>
/// For physical-pixel-based position, see <see cref="PhysicalPosition"/>.
/// </remarks>
/// <since>2.0.0</since>
/// <param name="Width">The width in physical pixels.</param>
/// <param name="Height">The height in physical pixels.</param>
public record PhysicalSize(int Width, int Height)
{
    /// <summary>
    /// The type of size representation.
    /// </summary>
    public string Type { get; init; } = "Physical";
};


/// <summary>
/// A position represented in logical pixels.
/// For an explanation of what logical pixels are, see description of <see cref="LogicalSize"/>.
/// </summary>
/// <since>2.0.0</since>
/// <param name="X">The X coordinate in logical pixels.</param>
/// <param name="Y">The Y coordinate in logical pixels.</param>
public record LogicalPosition(int X, int Y)
{
    /// <summary>
    /// The type of position representation.
    /// </summary>
    public string Type { get; init; } = "Logical";
};


/// <summary>
/// A position represented in physical pixels.
/// For an explanation of what physical pixels are, see description of <see cref="PhysicalSize"/>.
/// </summary>
/// <since>2.0.0</since>
/// <param name="X">The X coordinate in physical pixels.</param>
/// <param name="Y">The Y coordinate in physical pixels.</param>
public record PhysicalPosition(int X, int Y)
{
    /// <summary>
    /// The type of position representation.
    /// </summary>
    public string Type { get; init; } = "Physical";
};
