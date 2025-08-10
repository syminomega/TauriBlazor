import {Window, WindowOptions} from "@tauri-apps/api/window";
import {WebviewOptions} from "@tauri-apps/api/webview";
import {WebviewWindow} from "@tauri-apps/api/webviewWindow";
import {listen, once, UnlistenFn, Options} from "@tauri-apps/api/event";
import {DotNet} from "@microsoft/dotnet-js-interop";

export function getPropertyInObject(obj: any, propPath: string) {
    if (obj === null || obj === undefined) {
        return null;
    }
    const props = propPath.split('.');
    let current = obj;
    for (const p of props) {
        if (current[p] === undefined) {
            return null;
        }
        current = current[p];
    }
    return current;
}

export function constructWindow(label: string, options?: WindowOptions | null) {
    if (options === null || options === undefined) {
        options = undefined;
    }
    let window = new Window(label, options);
    console.log("Constructing window with label: " + label);
    return window;
}

export function constructWebviewWindow(label: string, windowOptions?: WindowOptions | null, webviewOptions?: WebviewOptions | null) {
    // 合并 windowOptions 和 webviewOptions，排除 webviewOptions 中的位置和尺寸属性
    let mergedOptions: Omit<WebviewOptions, 'x' | 'y' | 'width' | 'height'> & WindowOptions = {};
    
    // 首先添加 windowOptions
    if (windowOptions) {
        mergedOptions = { ...mergedOptions, ...windowOptions };
    }
    
    // 然后添加 webviewOptions，但排除 x、y、width、height 属性
    if (webviewOptions) {
        const { x, y, width, height, ...filteredWebviewOptions } = webviewOptions;
        mergedOptions = { ...mergedOptions, ...filteredWebviewOptions };
    }
    
    let webviewWindow = new WebviewWindow(label, mergedOptions);
    console.log("Constructing webview window with label: " + label);
    return webviewWindow;
}

class UnlistenHandler {
    unlistenFn: UnlistenFn;
    callbackHandler: DotNet.DotNetObject

    constructor(unlistenFn: UnlistenFn, callbackHandler: DotNet.DotNetObject) {
        this.unlistenFn = unlistenFn;
        this.callbackHandler = callbackHandler;
    }

    public unlisten() {
        this.unlistenFn();
        this.callbackHandler.dispose();
    }
}

export async function listenEvent(eventName: string, callbackHandler: DotNet.DotNetObject, options?: Options): Promise<UnlistenHandler> {
    const unlistenFn = await listen(eventName, async (event) => {
        let payload = null;
        if (event.payload) {
            payload = event.payload;
        }
        await callbackHandler.invokeMethodAsync("InvokeEvent", payload);
    }, options);

    return new UnlistenHandler(unlistenFn, callbackHandler);
}

export async function onceEvent(eventName: string, callbackHandler: DotNet.DotNetObject, options?: Options): Promise<UnlistenHandler> {
    const unlistenFn = await once(eventName, async (event) => {
        let payload = null;
        if (event.payload) {
            payload = event.payload;
        }
        await callbackHandler.invokeMethodAsync("InvokeEvent", payload);
        // dispose the callback handler after the first event
        callbackHandler.dispose();
    }, options);

    return new UnlistenHandler(unlistenFn, callbackHandler);
}
