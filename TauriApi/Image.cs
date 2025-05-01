using Microsoft.JSInterop;
using TauriApi.Interfaces;

namespace TauriApi;

/// <summary>
/// An RGBA Image in row-major order from top to bottom.
/// </summary>
public class Image : Resource, ITauriObject
{
    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }

    /// <inheritdoc />
    public override long Rid { get; }

    /// <inheritdoc />
    public override async Task Close()
    {
        await JsObjectRef.InvokeVoidAsync("close");
    }

    internal Image(IJSObjectReference jsObjectRef, long rid)
    {
        JsObjectRef = jsObjectRef;
        Rid = rid;
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