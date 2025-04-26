import {Window, WindowOptions} from "@tauri-apps/api/window";
import {listen, UnlistenFn, Options} from "@tauri-apps/api/event";
import {DotNet} from "@microsoft/dotnet-js-interop";

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
