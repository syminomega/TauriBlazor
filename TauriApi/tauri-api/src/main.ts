import {Window, WindowOptions} from "@tauri-apps/api/window";

export function constructWindow(label: string, options?: WindowOptions) {
    return new Window(label, options);
}