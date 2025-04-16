// Learn more about Tauri commands at https://tauri.app/develop/calling-rust/

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {

    let builder = {
        #[cfg(debug_assertions)]
        {
            let devtools = tauri_plugin_devtools::init();
            tauri::Builder::default().plugin(devtools)
        }

        #[cfg(not(debug_assertions))]
        {
            tauri::Builder::default()
        }
    };
    
    builder
    .plugin(tauri_plugin_log::Builder::new().build())
    .plugin(tauri_plugin_fs::init())
    .plugin(tauri_plugin_shell::init())
    .plugin(tauri_plugin_opener::init())
    .plugin(tauri_plugin_dialog::init())
    .run(tauri::generate_context!())
    .expect("error while running tauri application");

}
