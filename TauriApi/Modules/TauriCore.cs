using Microsoft.JSInterop;

namespace TauriApi.Modules;

public class TauriCore
{
    private readonly IJSRuntime _jsRuntime;
    private const string Prefix = "__TAURI__.core";

    public TauriCore(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    #region Invoke

    public async Task<T> Invoke<T>(string funcName, object paramObject)
    {
        return await _jsRuntime.InvokeAsync<T>($"{Prefix}.invoke", funcName, paramObject);
    }

    public async Task<T> Invoke<T>(string funcName)
    {
        return await _jsRuntime.InvokeAsync<T>($"{Prefix}.invoke", funcName);
    }

    public async Task Invoke(string funcName, object paramObject)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.invoke", funcName, paramObject);
    }

    public async Task Invoke(string funcName)
    {
        await _jsRuntime.InvokeVoidAsync($"{Prefix}.invoke", funcName);
    }

    #endregion

    public async Task<string> ConvertFileSrc(string filePath, string protocol = "asset")
    {
        return await _jsRuntime.InvokeAsync<string>($"{Prefix}.convertFileSrc", filePath, protocol);
    }

    public bool IsTauri()
    {
        var jsInProcess = (IJSInProcessRuntime)_jsRuntime;
        return jsInProcess.Invoke<bool>($"{Prefix}.isTauri");
    }
}