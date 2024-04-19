#![cfg_attr(
all(not(debug_assertions), target_os = "windows"),
windows_subsystem = "windows"
)]

use tauri::Manager;
use tauri_blazor_demo::examples;

#[tauri::command]
fn greet(name: &str) -> String {
    format!("Hello, {}! You've been greeted from Rust!", name)
}

fn main() {
    tauri::Builder::default()
        .setup(|app|{
            app.listen_global("random-number", |msg| {
                println!("Received random number global: {:?}", msg.payload());
            });
            Ok(())
        })
        .invoke_handler(tauri::generate_handler![
            greet,
            examples::open_example_window,
            examples::emit_random_number,
        ])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
