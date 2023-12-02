// src-backend/main.ts
 import { app, BrowserWindow } from "electron";
 import * as path from "path";

 let mainWindow: Electron.BrowserWindow;

 app.on("ready", () => {
     mainWindow = new BrowserWindow({
             icon: path.join(__dirname, "../dist/clock-angular-in-electron/assets/icon.png"),
                     webPreferences: {
                                 nodeIntegration: true, // Allows IPC and other APIs
                                         }
                                             });
                                                 mainWindow.loadFile(path.join(__dirname, "../dist/clock-angular-in-electron/browser/index.html"));
                                                 });

                                                 app.on("window-all-closed", () => {app.quit()});
