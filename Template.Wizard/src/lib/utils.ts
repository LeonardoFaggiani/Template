import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"


export  const errorCodeMessages = new Map<number, string>();
errorCodeMessages.set(2, "Failed dotnet new CustomApiTemplate command, try again...");


export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function ErrorMessageMappings(errorCode:number) {
  const message = errorCodeMessages.get(errorCode) ? errorCodeMessages.get(errorCode) : "An unknown error occurred.";
  return message;
}
