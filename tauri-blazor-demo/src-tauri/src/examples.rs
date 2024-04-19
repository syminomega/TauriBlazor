use rand::Rng;
use tauri::{LogicalSize, Manager};

#[tauri::command]
pub fn open_example_window(handle: tauri::AppHandle) {
    let local_window = tauri::WindowBuilder::new(
        &handle,
        "examples",
        tauri::WindowUrl::App("/Examples".into()),
    ).build().unwrap();
    local_window.set_title("Examples").unwrap();
    local_window.set_size(LogicalSize::new(600, 450)).unwrap();
}

#[derive(Clone, serde::Serialize)]
pub struct TestData {
    pub name: String,
    pub rand_num: i32,
}

#[tauri::command]
pub fn emit_random_number(window: tauri::window::Window) {
    //输出当前窗口label
    println!("current window: {}", window.label());
    let mut rng = rand::thread_rng();
    let data = TestData {
        name: "Rust".to_string(),
        rand_num: rng.gen_range(1..101),
    };
    window.emit("random-number", data).unwrap();
}