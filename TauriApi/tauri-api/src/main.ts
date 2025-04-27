import {Window, WindowOptions} from "@tauri-apps/api/window";
import {listen, UnlistenFn, Options} from "@tauri-apps/api/event";
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

export function constructWindow(label: string, options?: WindowOptions) {
    return new Window(label, options);
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
