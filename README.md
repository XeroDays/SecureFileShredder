
# Secure File Shredder

**Version 1.5** — A Windows desktop application for securely deleting sensitive files and folders beyond recovery.

## Overview

**Secure File Shredder** is a lightweight Windows Forms application built in C# on **.NET 8**. It provides a simple interface for securely deleting files using cryptographic random data overwrites, so sensitive data is much harder to recover than with a normal delete.

![image](https://github.com/user-attachments/assets/c75e5a4c-cc50-4fea-a892-80e5f4d02b0b)
![image](https://github.com/user-attachments/assets/b7ea2ee7-6a33-419b-aec2-57a015b81796)

The tool supports multiple overwrite passes and configurable buffer sizes. It is suited for permanently removing personal, confidential, or financial files you no longer need.

## Features

- **Drag-and-drop**: Add files or folders to the shred queue from the main window.
- **Windows context menu**: After installation, right-click any file or folder and choose **Shred Securely** to open the app with that item queued (uses the application logo as the menu icon).
- **Folder shredding**: Dropped or selected folders are expanded recursively; contained files are shredded and empty root folders are removed afterward.
- **Configurable overwrite passes**: Choose from presets (1, 3, 7, 12, 35, or 55 passes) labeled after common standards (e.g. DoD, Gutmann); each pass uses cryptographically random data.
- **Configurable buffer size**: Tune read/write chunk size from 1 KB up to 512 KB (default 4 KB) for performance tuning on large files.
- **Progress monitoring**: Progress bar with background processing so the UI stays responsive.
- **Cancellation**: Stop an in-progress shred from the close control.
- **Single-instance behavior**: Opening the app again (e.g. from the context menu while it is already running) sends new paths to the existing window instead of starting a second copy.
- **About dialog**: Product information and version from the info button on the main window.
- **Windows installer**: Inno Setup package for install, uninstall, and shell integration; release builds are published via GitHub Actions when a release is published.

## How It Works

1. **Queue**: Files (and folder contents) are collected into a shred queue via drag-and-drop, the context menu, or launching the app with paths.
2. **Overwrite**: Each file is overwritten multiple times with random bytes from a cryptographically secure generator; you choose how many passes to apply.
3. **Delete**: After overwriting, files are deleted from disk; dragged root folders are removed when applicable.
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
4. Watch the **progress bar**; use the close control to **cancel** if needed.
5. When finished, a confirmation message appears and the queue is cleared.

## Project Structure

| Area | Role |
|------|------|
| `Mainmenu` | Main UI, file queue, shred settings, background worker, deletion |
| `About` | About / version dialog |
| `Controllers/ShredderController` | Secure multi-pass file overwrite |
| `Program` | Application entry, single-instance mutex, inter-process file handoff |
| `SetupInstaller.iss` | Windows installer, context menu registry, bundled `Logo.ico` |
| `.github/workflows/build.yml` | Release build, zip, Inno Setup, upload to GitHub Releases |
| `ChangeLog.txt` | Release history notes |

## Release Notes (1.5)

Current release highlights:

- Version **1.5** across installer and About dialog
- Context menu icon updated to **Logo.ico** (aligned with application branding)
- Continued support for multi-pass shredding, folder queues, and shell **Shred Securely** integration

Earlier versions introduced the installer, GitHub release workflow, context menu, folder deletion, shredder controller, pass/buffer presets, and UI refinements — see `SecureFileShredder/ChangeLog.txt` for full history.

## Future Enhancements

Possible improvements:

1. Shredding files that are locked or in use by other processes
2. Metadata wiping (timestamps, alternate streams)
3. Multi-threaded shredding for faster batch jobs
4. Optional password protection before shredding
5. Shredding profiles to save favorite pass/buffer settings
6. File-type filters (e.g. only documents)
7. Detailed audit log of shredded items
8. Assembly / release version synced automatically from Git tags

## Disclaimer

This software is provided for educational purposes only. Use it to securely delete files that you own and are allowed to destroy.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.

## Contributions

Contributions are welcome. Open an issue or submit a pull request with improvements or bug fixes.
