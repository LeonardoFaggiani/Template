import { open } from "@tauri-apps/plugin-dialog";
import { exists } from '@tauri-apps/plugin-fs';

export async function showDialog(): Promise<string | null> {
  const selected = await open({ directory: true, multiple: false });
  return typeof selected === "string" ? selected : null;
}

export async function checkIfProjectExists(projectLocation:string, projectName:string) : Promise<boolean> {

  if(projectLocation && projectName){
    const fullPath = `${projectLocation}\\${projectName}`;
    return await exists(fullPath);
  }
  return false;
}