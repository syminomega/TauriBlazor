namespace TauriApi;

/// <summary>
/// A position represented in physical pixels.
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
public record PhysicalPosition(int X, int Y);

/// <summary>
/// A size represented in physical pixels.
/// </summary>
/// <param name="Width"></param>
/// <param name="Height"></param>
public record PhysicalSize(int Width, int Height);