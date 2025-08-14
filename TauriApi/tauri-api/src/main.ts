import {Window, WindowOptions} from "@tauri-apps/api/window";
import {Webview, WebviewOptions} from "@tauri-apps/api/webview";
import {WebviewWindow} from "@tauri-apps/api/webviewWindow";
import {listen, once, UnlistenFn, Options} from "@tauri-apps/api/event";
import {DotNet} from "@microsoft/dotnet-js-interop";

export function getPropertyInObject(obj: any, propPath: string) {
    if (obj === null || obj === undefined) {
        return null;
    }
    
    // 解析属性路径，支持点号分隔和方括号索引
    const parsePropertyPath = (path: string): Array<string | number> => {
        const tokens: Array<string | number> = [];
        let current = '';
        let i = 0;

        while (i < path.length) {
            const char = path[i];

            if (char === '.') {
                if (current) {
                    tokens.push(current);
                    current = '';
                }
            } else if (char === '[') {
                if (current) {
                    tokens.push(current);
                    current = '';
                }

                // 找到匹配的右括号
                let bracketEnd = i + 1;
                let bracketCount = 1;
                while (bracketEnd < path.length && bracketCount > 0) {
                    if (path[bracketEnd] === '[') bracketCount++;
                    if (path[bracketEnd] === ']') bracketCount--;
                    bracketEnd++;
                }

                if (bracketCount > 0) {
                    throw new Error(`Unclosed bracket in property path: ${path}`);
                }

                // 提取括号内的内容
                const bracketContent = path.substring(i + 1, bracketEnd - 1);

                // 检查是否是字符串键（用引号包围）
                if ((bracketContent.startsWith('"') && bracketContent.endsWith('"')) ||
                    (bracketContent.startsWith("'") && bracketContent.endsWith("'"))) {
                    // 字符串键，去掉引号
                    tokens.push(bracketContent.slice(1, -1));
                } else {
                    // 数字索引
                    const index = parseInt(bracketContent, 10);
                    if (isNaN(index)) {
                        throw new Error(`Invalid index in property path: ${bracketContent}`);
                    }
                    tokens.push(index);
                }

                i = bracketEnd;
                continue;
            } else {
                current += char;
            }

            i++;
        }

        if (current) {
            tokens.push(current);
        }

        return tokens;
    };

    try {
        const props = parsePropertyPath(propPath);
        let current = obj;

        for (const prop of props) {
            if (current === null || current === undefined) {
                return null;
            }

            if (typeof prop === 'number') {
                // 数组索引访问
                if (!Array.isArray(current) && typeof current !== 'object') {
                    return null;
                }
                current = current[prop];
            } else {
                // 属性名访问
                if (typeof current !== 'object') {
                    return null;
                }
                current = current[prop];
            }

            if (current === undefined) {
                return null;
            }
        }

        return current;
    } catch (error) {
        console.error('Error parsing property path:', error);
        return null;
    }
}

export function constructWindow(label: string, options?: WindowOptions | null) {
    if (options === null || options === undefined) {
        options = undefined;
    }
    let window = new Window(label, options);
    console.log("Constructing window with label: " + label);
    return window;
}

export async function constructWebview(windowLabel: string, label: string, options: WebviewOptions) {
    let window = await Window.getByLabel(windowLabel);
    if (!window) {
        throw new Error('Could not find window with label: ' + windowLabel);
    }
    let webview = new Webview(window, label, options);
    console.log("Constructing webview with label: " + label + " in window: " + windowLabel);
    return webview;
}

export function constructWebviewWindow(label: string, windowOptions?: WindowOptions | null, webviewOptions?: WebviewOptions | null) {
    // 合并 windowOptions 和 webviewOptions，排除 webviewOptions 中的位置和尺寸属性
    let mergedOptions: Omit<WebviewOptions, 'x' | 'y' | 'width' | 'height'> & WindowOptions = {};

    // 首先添加 windowOptions
    if (windowOptions) {
        mergedOptions = {...mergedOptions, ...windowOptions};
    }

    // 然后添加 webviewOptions，但排除 x、y、width、height 属性
    if (webviewOptions) {
        const {x, y, width, height, ...filteredWebviewOptions} = webviewOptions;
        mergedOptions = {...mergedOptions, ...filteredWebviewOptions};
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
