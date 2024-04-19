// 事件列表
const eventCollection = new Map();
console.log('tauri-api.js loaded');

export async function addListenBind(eventName) {
    console.log('addListenBind', eventName);
    // 监听Tauri事件
    let unlisten = await __TAURI__.event.listen(eventName, (event) => {
        // 触发blazor中的事件
        DotNet.invokeMethodAsync('TauriApi', 'TauriEventCallback', eventName, event.payload);
        console.log('Event:'+eventName, event.payload)
    });
    eventCollection.set(eventName, unlisten);
}

export function removeListenBind(eventName) {
    // 取消监听Tauri事件
    const unlisten = eventCollection.get(eventName);
    if (unlisten) {
        unlisten();
        eventCollection.delete(eventName);
    }
}

export function getAppWindow() {
    return __TAURI__.window.appWindow;
}

export function getWebviewWindowLabel(appWindow) {
    return appWindow.label;
}

export async function addWindowListenBind(appWindow, eventName) {
    console.log('addWindowListenBind',appWindow, eventName);
    // 监听Tauri事件
    let unlisten = await appWindow.listen(eventName, (event) => {
        // 触发blazor中的事件
        DotNet.invokeMethodAsync('TauriApi', 'WindowEventCallback', appWindow.label, eventName, event.payload);
        console.log('Window:'+appWindow.label+' Event:'+eventName, event.payload)
    });
    // 保存监听
    let windowEventCollection = appWindow.eventCollection;
    if (!windowEventCollection) {
        windowEventCollection = new Map();
        appWindow.eventCollection = windowEventCollection;
    }
    windowEventCollection.set(eventName, unlisten);
}

export function removeWindowListenBind(appWindow, eventName) {
    //取消监听Window事件
    const windowEventCollection = appWindow.eventCollection;
    if (windowEventCollection) {
        const unlisten = windowEventCollection.get(eventName);
        if (unlisten) {
            unlisten();
            windowEventCollection.delete(eventName);
        }
    }
}