using Microsoft.JSInterop;
using TauriApi.Utilities;

namespace TauriApi.Modules;

/// <summary>
/// An RGBA Image in row-major order from top to bottom.
/// </summary>
public class TauriImage
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.image";
    private readonly TauriJsInterop _tauriJsInterop;

    /// <summary>
    /// 注入TauriImage
    /// </summary>
    public TauriImage(IJSRuntime jsRuntime, TauriJsInterop tauriJsInterop)
    {
        _jsRuntime = jsRuntime;
        _tauriJsInterop = tauriJsInterop;
    }

    /// <summary>
    /// Creates a new Image using RGBA data, in row-major order from top to bottom,
    /// and with specified width and height.
    /// </summary>
    public async Task<Image> New(byte[] rgba, int width, int height)
    {
        var imageRef = await _jsRuntime.InvokeAsync<IJSObjectReference>($"{Prefix}.Image.new", rgba, width, height);
        var rid = await _tauriJsInterop.GetJsProperty<long>(imageRef, "rid");
        return new Image(imageRef, rid);
    }

    /// <summary>
    /// Creates a new image using the provided path.
    /// Only ico and png are supported (based on activated feature flag).
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<Image> FromPath(string path)
    {
        var imageRef = await _jsRuntime.InvokeAsync<IJSObjectReference>($"{Prefix}.Image.fromPath", path);
        var rid = await _tauriJsInterop.GetJsProperty<long>(imageRef, "rid");
        return new Image(imageRef, rid);
    }

    /// <summary>
    /// Creates a new image using the provided bytes by inferring the file format.
    /// If the format is known, prefer [@link Image.fromPngBytes] or [@link Image.fromIcoBytes].
    /// Only ico and png are supported (based on activated feature flag).
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public async Task<Image> FromBytes(byte[] bytes)
    {
        var imageRef = await _jsRuntime.InvokeAsync<IJSObjectReference>($"{Prefix}.Image.fromBytes", bytes);
        var rid = await _tauriJsInterop.GetJsProperty<long>(imageRef, "rid");
        return new Image(imageRef, rid);
    }
}