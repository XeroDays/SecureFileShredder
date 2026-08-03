# Secure File Shredder

**Version 1.7** — A Windows desktop application for securely deleting sensitive files and folders beyond recovery.

## Overview

**Secure File Shredder** is a lightweight Windows Forms application built in C# on **.NET 8**. It provides a simple interface for securely deleting files using cryptographic random data overwrites, so sensitive data is much harder to recover than with a normal delete.


<img width="492" height="354" alt="Screenshot_1" src="https://github.com/user-attachments/assets/a60e8e09-4402-4b3a-a395-a69c39f3090a" />
 <img width="359" height="263" alt="Screenshot_2" src="https://github.com/user-attachments/assets/b12350de-3e52-4b97-b514-5f25268059ee" />

![image](https://github.com/user-attachments/assets/b7ea2ee7-6a33-419b-aec2-57a015b81796)

The tool supports multiple overwrite passes and configurable buffer sizes. It is suited for permanently removing personal, confidential, or financial files you no longer need.

## Features

- **Drag-and-drop**: Add files or folders to the shred queue from the main window.
- **Windows context menu**: After installation, right-click any file or folder and choose **Shred Securely** to open the app with that item queued (uses the application logo as the menu icon).
- **Folder shredding**: Dropped or selected folders are expanded recursively; contained files are shredded and empty root folders are removed afterward.
- **Configurable overwrite passes**: Choose from presets (1, 3, 7, 12, 35, or 55 passes) labeled after common standards (e.g. DoD, Gutmann); each pass uses cryptographically random data.
- **Configurable buffer size**: Tune read/write chunk size from 1 KB up to 512 KB (default 4 KB) for performance tuning on large files.
- **Progress monitoring**: Progress bar with background processing so the UI stays responsive.
- **Stop shredding**: The start button becomes **Stop Shredding** while a job runs; use it to cancel the operation.
- **Minimize to tray**: While shredding, minimize to the system tray; the tray icon shows live progress percentage.
- **Close after finish**: The close button is hidden during shredding; when the job ends, confirm the result message, then close the app when you are ready.
- **Locked files**: Files that cannot be shredded (for example in use) are skipped; the rest of the batch continues and failed items stay in the queue.
- **Single-instance behavior**: Opening the app again (e.g. from the context menu while it is already running) sends new paths to the existing window instead of starting a second copy.
- **About dialog**: Product information and version from the info button on the main window.
- **Windows installer**: Inno Setup package for install, uninstall, and shell integration; release builds are published via GitHub Actions when a release is published.

## How It Works

1. **Queue**: Files (and folder contents) are collected into a shred queue via drag-and-drop, the context menu, or launching the app with paths.
2. **Overwrite**: Each file is overwritten multiple times with random bytes from a cryptographically secure generator; you choose how many passes to apply.
3. **Delete**: After overwriting, successfully shredded files are deleted from disk; dragged root folders are removed when applicable.
4. **Feedback**: Progress is shown during the operation; success, cancel, and error states are reported in dialogs.

Pass preset names (DoD, NSA, Gutmann, etc.) indicate how many overwrite rounds run; the same secure random-byte method is used for every pass.

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
| `Controllers/ShredderController` | Secure multi-pass file overwrite |
| `Assets/TaskbarIcon` | Tray progress badge icons (`1%`–`100%`) |
| `Program` | Application entry, single-instance mutex, inter-process file handoff |
| `SetupInstaller.iss` | Windows installer, context menu registry, bundled `Logo.ico` |
| `.github/workflows/build.yml` | Release build, zip, Inno Setup, upload to GitHub Releases |
| `ChangeLog.txt` | Release history notes |

## Release Notes (1.7)

Current release highlights:

- Minimize to tray while shredding with live % on tray icon
- Close button hidden during shred; app stays open until user closes
- Start button becomes **Stop Shredding** to cancel the job
- Hint label hides while running and shows again when done

Earlier versions introduced the installer, GitHub release workflow, context menu, folder deletion, shredder controller, pass/buffer presets, and UI refinements — see `SecureFileShredder/ChangeLog.txt` for full history.

## Future Enhancements

Possible improvements:

1. Metadata wiping (timestamps, alternate streams)
2. Multi-threaded shredding for faster batch jobs
3. Optional password protection before shredding
4. Shredding profiles to save favorite pass/buffer settings
5. File-type filters (e.g. only documents)
6. Detailed audit log of shredded items
7. Assembly / release / installer version synced automatically from Git tags

## Disclaimer

This software is provided for educational purposes only. Use it to securely delete files that you own and are allowed to destroy.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.

## Contributions

Contributions are welcome. Open an issue or submit a pull request with improvements or bug fixes. See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.
