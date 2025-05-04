using Microsoft.JSInterop;

namespace TauriApi;

/// <summary>
/// An RGBA Image in row-major order from top to bottom.
/// </summary>
public class Image : Resource
{
    internal Image(IJSObjectReference jsObjectRef, long rid) : base(jsObjectRef, rid)
    {
    }

    /// <summary>
    /// Returns the size of this image.
    /// </summary>
    public ValueTask<ImageSize> Size()
    {
        return JsObjectRef.InvokeAsync<ImageSize>("size");
    }

    /// <summary>
    /// Returns the RGBA data for this image, in row-major order from top to bottom.
    /// </summary>
    public ValueTask<byte[]> Rgba()
    {
        return JsObjectRef.InvokeAsync<byte[]>("rgba");
    }
}

/// <summary>
/// ImageSize
/// </summary>
public record ImageSize(int Width, int Height);