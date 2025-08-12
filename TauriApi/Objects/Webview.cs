using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using TauriApi.Interfaces;
using TauriApi.Utilities;
using TauriApi.Utilities.JsonConverters;

namespace TauriApi;

/// <inheritdoc />
public class Webview : ITauriWebview
{
    internal Webview(IJSObjectReference webviewRef, TauriJsInterop tauriJsInterop)
    {
        _tauriJsInterop = tauriJsInterop;
        JsObjectRef = webviewRef;
    }

    private readonly TauriJsInterop _tauriJsInterop;

    /// <inheritdoc />
    public IJSObjectReference JsObjectRef { get; }

    /// <inheritdoc />
    public ValueTask<string> Label => _tauriJsInterop.GetJsProperty<string>(JsObjectRef, "label");
}

/// <summary>
/// Webview Methods
/// </summary>
public static class WebviewExtensions
{
    /// <summary>
    /// Emits an event to all targets.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task Emit(this ITauriWebview webview, string eventName, object? payload = null)
    {
        await webview.JsObjectRef.InvokeVoidAsync("emit", eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given target label.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="targetLabel">Label of the target Window/Webview/WebviewWindow</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task EmitTo(this ITauriWebview webview, string targetLabel, string eventName,
        object? payload = null)
    {
        await webview.JsObjectRef.InvokeVoidAsync("emitTo", targetLabel, eventName, payload);
    }

    /// <summary>
    /// Emits an event to all targets matching the given <see cref="EventTarget"/>.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="target">Raw <see cref="EventTarget"/> object.</param>
    /// <param name="eventName">Event name. Must include only alphanumeric characters, -, /, : and _.</param>
    /// <param name="payload">Event payload. Can be serialized to JSON.</param>
    public static async Task EmitTo<T>(this ITauriWebview webview, T target, string eventName, object? payload = null)
        where T : EventTarget
    {
        await webview.JsObjectRef.InvokeVoidAsync("emitTo", target, eventName, payload);
    }

    /// <summary>
    /// Set webview zoom level.
    /// </summary>
    /// <param name="webview"></param>
    /// <param name="scaleFactor"></param>
    public static async Task SetZoom(this ITauriWebview webview, double scaleFactor)
    {
        await webview.JsObjectRef.InvokeVoidAsync("setZoom", scaleFactor);
    }
}

#region Interfaces

/// <summary>
/// Options for creating a webview.
/// </summary>
public record WebviewOptions
{
    /// <summary>
    /// Remote URL or local file path to open.
    /// </summary>
    /// <remarks>
    /// <para>URL such as <c>https://github.com/tauri-apps</c> is opened directly on a Tauri webview.</para>
    /// <para>data: URL such as <c>data:text/html,&lt;html&gt;...</c> is only supported with the <c>webview-data-url</c> Cargo feature for the <c>tauri</c> dependency.</para>
    /// <para>local file path or route such as <c>/path/to/page.html</c> or <c>/users</c> is appended to the application URL (the devServer URL on development, or <c>tauri://localhost/</c> and <c>https://tauri.localhost/</c> on production).</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; init; }

    /// <summary>
    /// The initial vertical position.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? X { get; init; }

    /// <summary>
    /// The initial horizontal position.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Y { get; init; }

    /// <summary>
    /// The initial width.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Width { get; init; }

    /// <summary>
    /// The initial height.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Height { get; init; }

    /// <summary>
    /// Whether the webview is transparent or not.
    /// </summary>
    /// <remarks>
    /// <para>Note that on <c>macOS</c> this requires the <c>macos-private-api</c> feature flag, enabled under <c>tauri.conf.json &gt; app &gt; macOSPrivateApi</c>.</para>
    /// <para><strong>WARNING:</strong> Using private APIs on <c>macOS</c> prevents your application from being accepted to the <c>App Store</c>.</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Transparent { get; init; }

    /// <summary>
    /// Whether the webview should have focus or not.
    /// </summary>
    /// <remarks>
    /// Since version 2.1.0
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Focus { get; init; }

    /// <summary>
    /// Whether the drag and drop is enabled or not on the webview. By default, it is enabled.
    /// Disabling it is required to use HTML5 drag and drop on the frontend on Windows.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DragDropEnabled { get; init; }

    /// <summary>
    /// Whether clicking an inactive webview also clicks through to the webview on macOS.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AcceptFirstMouse { get; init; }

    /// <summary>
    /// The user agent for the webview.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserAgent { get; init; }

    /// <summary>
    /// Whether the webview should be launched in incognito mode.
    /// </summary>
    /// <remarks>
    /// <para><strong>Platform-specific:</strong></para>
    /// <para><strong>Android:</strong> Unsupported.</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Incognito { get; init; }

    /// <summary>
    /// The proxy URL for the WebView for all network requests.
    /// </summary>
    /// <remarks>
    /// <para>Must be either a <c>http://</c> or a <c>socks5://</c> URL.</para>
    /// <para><strong>Platform-specific:</strong></para>
    /// <para><strong>macOS:</strong> Requires the <c>macos-proxy</c> feature flag and only compiles for macOS 14+.</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProxyUrl { get; init; }

    /// <summary>
    /// Whether page zooming by hotkeys is enabled.
    /// </summary>
    /// <remarks>
    /// <para><strong>Platform-specific:</strong></para>
    /// <para><strong>Windows:</strong> Controls WebView2's <c>IsZoomControlEnabled</c> setting.</para>
    /// <para><strong>MacOS / Linux:</strong> Injects a polyfill that zooms in and out with <c>ctrl/command</c> + <c>-/=</c>, 20% in each step, ranging from 20% to 1000%. Requires <c>webview:allow-set-webview-zoom</c> permission.</para>
    /// <para><strong>Android / iOS:</strong> Unsupported.</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ZoomHotkeysEnabled { get; init; }

    /// <summary>
    /// Sets whether the custom protocols should use <c>https://&lt;scheme&gt;.localhost</c> instead of the default <c>http://&lt;scheme&gt;.localhost</c> on Windows and Android. Defaults to <c>false</c>.
    /// </summary>
    /// <remarks>
    /// <para><strong>Note:</strong></para>
    /// <para>Using a <c>https</c> scheme will NOT allow mixed content when trying to fetch <c>http</c> endpoints and therefore will not match the behavior of the <c>&lt;scheme&gt;://localhost</c> protocols used on macOS and Linux.</para>
    /// <para><strong>Warning:</strong></para>
    /// <para>Changing this value between releases will change the IndexedDB, cookies and localstorage location and your app will not be able to access them.</para>
    /// <para>Since version 2.1.0</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? UseHttpsScheme { get; init; }

    /// <summary>
    /// Whether web inspector, which is usually called browser devtools, is enabled or not. Enabled by default.
    /// </summary>
    /// <remarks>
    /// <para>This API works in <strong>debug</strong> builds, but requires <c>devtools</c> feature flag to enable it in <strong>release</strong> builds.</para>
    /// <para><strong>Platform-specific:</strong></para>
    /// <para><strong>macOS:</strong> This will call private functions on <strong>macOS</strong>.</para>
    /// <para><strong>Android:</strong> Open <c>chrome://inspect/#devices</c> in Chrome to get the devtools window. Wry's <c>WebView</c> devtools API isn't supported on Android.</para>
    /// <para><strong>iOS:</strong> Open Safari &gt; Develop &gt; [Your Device Name] &gt; [Your WebView] to get the devtools window.</para>
    /// <para>Since version 2.1.0</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Devtools { get; init; }

    /// <summary>
    /// Set the window and webview background color.
    /// </summary>
    /// <remarks>
    /// <para><b>Platform-specific:</b></para>
    /// <para><b>macOS / iOS:</b> Not implemented.</para>
    /// <para><b>Windows:</b></para>
    /// <para>- On Windows 7, alpha channel is ignored.</para>
    /// <para>- On Windows 8 and newer, if alpha channel is not 0, it will be ignored.</para>
    /// <para>Since version 2.1.0</para>
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Color? BackgroundColor { get; init; }

    /// <summary>
    /// Change the default background throttling behaviour.
    /// </summary>
    /// <remarks>
    /// By default，browsers use a suspend policy that will throttle timers and even unload
    /// the whole tab (view) to free resources after roughly 5 minutes when a view became
    /// minimized or hidden. This will pause all tasks until the documents visibility state
    /// changes back from hidden to visible by bringing the view back to the foreground.
    /// <para><b>Platform-specific:</b></para>
    /// <para><b>Linux / Windows / Android:</b> Unsupported. Workarounds like a pending WebLock transaction might suffice.</para>
    /// <para><b>iOS:</b> Supported since version 17.0+.</para>
    /// <para><b>macOS:</b> Supported since version 14.0+.</para>
    /// see https://github.com/tauri-apps/tauri/issues/5250#issuecomment-2569380578
    /// Since version 2.3.0
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BackgroundThrottlingPolicy? BackgroundThrottling { get; init; }

    /// <summary>
    /// Whether we should disable JavaScript code execution on the webview or not.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? JavascriptDisabled { get; init; }

    /// <summary>
    /// On macOS and iOS there is a link preview on long pressing links, this is enabled by default.
    /// see https://docs.rs/objc2-web-kit/latest/objc2_web_kit/struct.WKWebView.html#method.allowsLinkPreview
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowLinkPreview { get; init; }

    /// <summary>
    /// Allows disabling the input accessory view on iOS.
    /// The accessory view is the view that appears above the keyboard when a text input element is focused.
    /// It usually displays a view with "Done", "Next" buttons.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableInputAccessoryView { get; init; }
}

#endregion

#region Type Aliases

/// <summary>
/// An RGBA color. Each value has minimum of 0 and maximum of 255.
/// It can be either a string <c>#ffffff</c>, an array of 3 or 4 elements or an object.
/// </summary>
[JsonConverter(typeof(ColorFormatConverter))]
public class Color
{
    /// <summary>
    /// Red, must be between 0 and 255.
    /// </summary>
    public byte R { get; set; }

    /// <summary>
    /// Green, must be between 0 and 255.
    /// </summary>
    public byte G { get; set; }

    /// <summary>
    /// Blue, must be between 0 and 255.
    /// </summary>
    public byte B { get; set; }

    /// <summary>
    /// Alpha, must be between 0 and 255.
    /// </summary>
    public byte A { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> class with the specified RGB values and optional alpha value.
    /// </summary>
    /// <param name="r">Red component, must be between 0 and 255.</param>
    /// <param name="g">Green component, must be between 0 and 255.</param>
    /// <param name="b">Blue component, must be between 0 and 255.</param>
    /// <param name="a">Optional alpha component, must be between 0 and 255. If not provided, defaults to 255.</param>
    public Color(byte r, byte g, byte b, byte? a = null)
    {
        R = r;
        G = g;
        B = b;
        A = a ?? 255;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Color"/> class from a hexadecimal color string in the format #RRGGBB or #RRGGBBAA.
    /// </summary>
    /// <param name="hex"></param>
    /// <exception cref="ArgumentException"></exception>
    public Color(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length != 7 && hex.Length != 9)
        {
            throw new ArgumentException("Hex color must be in the format #RRGGBB or #RRGGBBAA", nameof(hex));
        }

        try
        {
            R = Convert.ToByte(hex.Substring(1, 2), 16);
            G = Convert.ToByte(hex.Substring(3, 2), 16);
            B = Convert.ToByte(hex.Substring(5, 2), 16);
            A = hex.Length == 9 ? Convert.ToByte(hex.Substring(7, 2), 16) : (byte)255;
        }
        catch (Exception e)
        {
            throw new ArgumentException("Hex color must be in the format #RRGGBB or #RRGGBBAA", nameof(hex), e);
        }
    }
}

#endregion