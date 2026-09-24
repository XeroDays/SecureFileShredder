# Secure File Shredder

**Version 1.8** — A Windows desktop application for securely deleting sensitive files and folders beyond recovery.

## Overview

**Secure File Shredder** is a lightweight Windows Forms application built in C# on **.NET 8**. It provides a simple interface for securely deleting files using cryptographic random data overwrites, so sensitive data is much harder to recover than with a normal delete.


<img width="492" height="354" alt="Screenshot_1" src="https://github.com/user-attachments/assets/a60e8e09-4402-4b3a-a395-a69c39f3090a" />
 <img width="359" height="263" alt="Screenshot_2" src="https://github.com/user-attachments/assets/b12350de-3e52-4b97-b514-5f25268059ee" />

![image](https://github.com/user-attachments/assets/b7ea2ee7-6a33-419b-aec2-57a015b81796)

The tool supports multiple overwrite passes and configurable buffer sizes. It is suited for permanently removing personal, confidential, or financial files you no longer need.

## Features

- **Add, remove, and clear**: Use Add files, Add folder, Remove, Clear, drag-and-drop, or the Delete key. The queue shows a file count and total size.
- **Windows context menu**: After installation, right-click any file or folder and choose **Shred Securely** to open the app with that item queued (uses the application logo as the menu icon).
- **Folder shredding**: Dropped or selected folders are expanded recursively; contained files are shredded and empty root folders are removed afterward.
- **Pass patterns**: Presets run 1, 3, 7, 12, 35, or 55 passes. Fixed patterns are used where the preset defines them. Gutmann’s fixed passes target old magnetic disks. The 12-pass preset keeps the older NSA label and is not a published NSA procedure.
- **Configurable buffer size**: Tune read/write chunk size from 1 KB up to 512 KB (default 4 KB) for performance tuning on large files.
- **Progress monitoring**: Chunk-level progress with percent, speed, and estimated time remaining. The active file is highlighted, then fades out when it finishes.
- **Stop shredding**: The start button becomes **Stop Shredding** while a job runs; use it to cancel the operation.
- **Minimize to tray**: While shredding, minimize to the system tray; the tray icon shows live progress percentage.
- **Close after finish**: The close button is hidden during shredding; when the job ends, confirm the result message, then close the app when you are ready.
- **Locked files**: Files that cannot be shredded (for example in use) are skipped; the rest of the batch continues and failed items stay in the queue.
- **Single-instance behavior**: Opening the app again (e.g. from the context menu while it is already running) sends new paths to the existing window instead of starting a second copy.
- **Settings and history**: Save profiles, a dark theme, and the last window size. A history view lists shredded paths on this PC.
- **Free-space wipe**: A separate screen fills free space down to a 256 MB reserve, overwrites that file, and deletes it. It asks twice before starting.
- **About dialog**: Product information and version from the info button on the main window.
- **Windows installer**: Inno Setup package for install, uninstall, and shell integration; release builds are published via GitHub Actions when a release is published.

## How It Works

1. **Queue**: Files (and folder contents) are collected into a shred queue via drag-and-drop, the context menu, or launching the app with paths.
2. **Overwrite**: Each file is overwritten using the selected pass pattern. Random passes use a cryptographic generator. Fixed passes write the preset’s byte pattern.
3. **Delete**: After overwriting, successfully shredded files are deleted from disk; dragged root folders are removed when applicable.
4. **Feedback**: Progress is shown during the operation; success, cancel, and error states are reported in dialogs.

Pass preset names select both the pass count and the byte pattern. Gutmann’s fixed patterns target old magnetic disks. On SSDs, wear leveling can leave old data on retired flash cells.

## Getting Started

### Prerequisites

- **Windows** (64-bit compatible)
- **.NET 8** runtime (included with the published installer build target `net8.0-windows`)

### Installation

1. Download the **Secure Shredder** installer from this repository’s **GitHub Releases** page (built automatically when a release is published).
2. Run the installer and follow the prompts.
3. Use **Shred Securely** from the right-click menu on files or folders, or launch **Secure File Shredder** from the Start menu / desktop shortcut.

### Usage

1. Add items by **dragging and dropping** onto the window, using **Shred Securely** in Explorer, or opening the app with paths already supplied.
2. Select **overwrite passes** and **buffer size** if you want something other than the defaults.
3. Click **Start Shredding to bits** and confirm when prompted.
4. Watch the **progress bar**. To cancel, click **Stop Shredding**. To keep working elsewhere, **minimize** to the system tray (progress % shows on the tray icon).
5. When finished, a confirmation message appears. Click **Close** when you want to exit the app.

## Project Structure

| Area | Role |
|------|------|
| `Mainmenu` | Main UI, file queue, shred settings, background worker, tray, deletion |
| `About` | About / version dialog |
| `Controllers/ShredderController` | Secure multi-pass file overwrite, stream wipe, and verify |
| `Services/ShredSession` | Parallel shred run, cancel, and progress |
| `SettingsForm`, `HistoryForm`, `FreeSpaceForm` | Profiles, audit log, free-space wipe |
| `Assets/TaskbarIcon` | Tray progress badge icons (`1%`–`100%`) |
| `Program` | Application entry, single-instance mutex, inter-process file handoff |
| `SetupInstaller.iss` | Windows installer, context menu registry, bundled `Logo.ico` |
| `.github/workflows/build.yml` | Release build, zip, Inno Setup, upload to GitHub Releases |
| `ChangeLog.txt` | Release history notes |

## Release Notes (1.8)

Current release highlights:

- Queue add, remove, and clear, with a live file count and size
- Chunk progress, speed, ETA, row highlight, and a completion summary
- Saved profiles, dark theme, shred history, and a resizable window
- Pass patterns, alternate-stream wipe, verify, rename, and timestamp randomization
- Parallel shredding and a separate free-space wipe

## Future Enhancements

Possible later work:

1. A published standard for the 12-pass preset if one is adopted
2. Per-drive free-space schedules
3. Optional password confirmation before shredding

Earlier versions introduced the installer, GitHub release workflow, context menu, folder deletion, shredder controller, pass/buffer presets, and UI refinements — see `SecureFileShredder/ChangeLog.txt` for full history.

## Disclaimer

This software is provided for educational purposes only. Use it to securely delete files that you own and are allowed to destroy.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.

## Contributions

Contributions are welcome. Open an issue or submit a pull request with improvements or bug fixes. See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.
