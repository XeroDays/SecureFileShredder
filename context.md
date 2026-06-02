# AI Context Index — Secure File Shredder

> AI-only navigation index. Not user documentation. Update on feature, workflow, architecture, or file moves.

---

## Project Summary

| Field | Detail |
|-------|--------|
| Purpose | Windows desktop app to securely overwrite and delete files/folders beyond standard delete |
| Architecture | Monolithic WinForms; partial controller extraction (`Controllers/`) |
| Framework | .NET 8 (`net8.0-windows`), Windows Forms |
| Language | C# |
| Database | None |
| External services | None (local OS APIs only) |
| Deployment | Inno Setup installer; GitHub Release CI on `release: published` |
| Solution | [SecureFileShredder.sln](SecureFileShredder.sln) — single project |
| Repo metadata | [.gitignore](.gitignore), [.gitattributes](.gitattributes) |

---

## Architecture Rules

- **WinForms partial class**: UI in `*.Designer.cs` + `*.resx`; behavior in matching `*.cs`
- **Primary form**: [Mainmenu.cs](SecureFileShredder/Mainmenu.cs) — shredding UI; **secondary form**: [About.cs](SecureFileShredder/About.cs) — about/branding dialog
- **Shredding algorithm** in [ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs); **queue, config, progress, deletion** stay in `Mainmenu`
- **Static shared state**: `PASSES`, `Buffer_Size` on `Mainmenu`, set at shred start, consumed by `ShredderController`
- **Overwrite then delete**: `BackgroundWorker` overwrites; `File.Delete` / `Directory.Delete` in `RunWorkerCompleted` on UI thread
- **Single instance**: Mutex `"SecureShredder"` in [Program.cs](SecureFileShredder/Program.cs); second instance uses `WM_COPYDATA` IPC
- **Borderless chrome**: `Mainmenu` and `About` use `FormBorderStyle.None`; close via `PictureBox` + `icons8_close_50`
- **Branding assets**: `Logo.ico` (application icon, shell context-menu icon, installer bundle), `LogoPng` (header/about logo) via [Properties/Resources.resx](SecureFileShredder/Properties/Resources.resx)
- **Shell context menu**: label, icon, and launch command configured only in [SetupInstaller.iss](SetupInstaller.iss) `[Registry]` — not in C#
- **No DI, repository layer, service interfaces, or automated tests**
- **Pass presets are labels only**: DoD/Gutmann/etc. change pass count only; all passes use `RNGCryptoServiceProvider` random bytes

---

## Feature Registry

### Application Bootstrap

Purpose: Entry point, WinForms host, single-instance gate.

Entry Points:
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs) — `Main(string[] args)`

Primary Files:
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)

Related Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)
- [SecureFileShredder/SecureFileShredder.csproj](SecureFileShredder/SecureFileShredder.csproj)

Dependencies: Windows Forms, `user32.dll` (IPC)

Workflow: `Program.Main` → Mutex → first instance runs `Mainmenu` / second instance sends paths via IPC

---

### Single-Instance and IPC

Purpose: One app window; route CLI/context-menu launches to existing instance.

Entry Points:
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs) — `SendFilesToExistingInstance`
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs) — `WndProc` (`WM_COPYDATA`)

Primary Files:
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

Related Files:
- [SetupInstaller.iss](SetupInstaller.iss) — passes `%1` as CLI args

Dependencies: Win32 `FindWindow`, `SendMessage`, `COPYDATASTRUCT`

Workflow: Second process → `FindWindow("Mainmenu")` → `WM_COPYDATA` pipe-delimited paths → `updateListWithFiles`

---

### File and Folder Queue

Purpose: Collect files to shred; expand directories recursively.

Entry Points:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs) — `updateListWithFiles`, drag-drop handlers, constructor args

Primary Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

Related Files:
- [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs) — `listBoxFiles`, drag-drop
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)

Dependencies: `System.IO` directory/file APIs

Workflow: Input paths → track root dirs in `listOfDirectories` → flatten files into `listofPaths` → display in listbox

---

### Shred Configuration

Purpose: User selects pass count and buffer size before shred.

Entry Points:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs) — `setupPassesCombo`, `setupBufferSizeCombo`, `btnStartDeleting_Click`

Primary Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

Related Files:
- [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs) — `cmbPasses`, `cmbBufferSize`

Dependencies: Combo label string parsing at runtime

Workflow: User selects presets → confirm dialog → set `PASSES`, `Buffer_Size` → start worker

---

### Secure Overwrite

Purpose: Multi-pass cryptographic random overwrite of file content.

Entry Points:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs) — `BackgroundWorker_DoWork`, `ShredFile`
- [SecureFileShredder/Controllers/ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs) — `ShreddFile`

Primary Files:
- [SecureFileShredder/Controllers/ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs)

Related Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

Dependencies: `RNGCryptoServiceProvider`, `BackgroundWorker` progress reporting

Workflow: Per file → N passes → random bytes written in buffer chunks → `ReportProgress` per pass

---

### Post-Shred Deletion

Purpose: Remove overwritten files and empty dragged root folders.

Entry Points:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs) — `BackgroundWorker_RunWorkerCompleted`

Primary Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

Related Files:
- [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs) — `progressBar`

Dependencies: `File.Delete`, `Directory.Delete(recursive)`

Workflow: Worker completes → delete all `listofPaths` → delete `listOfDirectories` roots → success dialog → clear UI state

---

### Shell Integration (Installer)

Purpose: Windows context menu **"Shred Securely"** for files (`*`) and folders (`Directory`).

Entry Points:
- [SetupInstaller.iss](SetupInstaller.iss) — `[Registry]` (menu label, icon, command) and `[Files]` (`Logo.ico`)

Primary Files:
- [SetupInstaller.iss](SetupInstaller.iss)

Related Files:
- [SecureFileShredder/Logo.ico](SecureFileShredder/Logo.ico) — context-menu icon source
- [SecureFileShredder/SecureFileShredder.csproj](SecureFileShredder/SecureFileShredder.csproj) — `CopyToOutputDirectory` for `Logo.ico`
- [.github/workflows/build.yml](.github/workflows/build.yml)
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)

Dependencies: Inno Setup, HKCR keys `*\shell\SecureFileShredder`, `Directory\shell\SecureFileShredder`

Configuration (all in `SetupInstaller.iss`):
- Menu text: `(default)` = `"Shred Securely"` on both file and directory keys
- Menu icon: `Icon` = `{app}\Logo.ico` (installed via `[Files]` from Release output)
- Command: `SecureFileShredder.exe "%1"` → [Program.cs](SecureFileShredder/Program.cs) `args` → [Mainmenu.cs](SecureFileShredder/Mainmenu.cs) queue

Workflow: Install → registry + `Logo.ico` in app dir → Explorer context menu → exe with path → Program/Mainmenu queue flow

---

### About Dialog

Purpose: Borderless about window with product name and logo.

Entry Points:
- [SecureFileShredder/About.cs](SecureFileShredder/About.cs) — `About()` constructor
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs) — `btnInfo_Click` opens dialog
- [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs) — `btnInfo`

Primary Files:
- [SecureFileShredder/About.cs](SecureFileShredder/About.cs)
- [SecureFileShredder/About.Designer.cs](SecureFileShredder/About.Designer.cs)

Related Files:
- [SecureFileShredder/About.resx](SecureFileShredder/About.resx)
- [SecureFileShredder/Properties/Resources.resx](SecureFileShredder/Properties/Resources.resx) — `LogoPng`, `icons8_close_50`

Dependencies: WinForms, embedded bitmap resources

Workflow: Intended: `btnInfo` click → show `About` → close disposes form (`btnClose_Click`)

---

### License Controller (Stub)

Purpose: Placeholder for future licensing; unused.

Entry Points: None

Primary Files:
- [SecureFileShredder/Controllers/LicenseController.cs](SecureFileShredder/Controllers/LicenseController.cs)

Related Files: None

Dependencies: None

Workflow: Not implemented

---

## Workflow Registry

### App Startup

Trigger: User launches exe (direct, shortcut, or with CLI args).

Flow: `Program.Main` → Mutex `SecureShredder` → new instance: `Application.Run(Mainmenu(args))` / existing: `SendFilesToExistingInstance`

Files:
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

---

### Add Files to Queue

Trigger: Drag-drop, CLI args, shell context menu, or second-instance IPC.

Flow: Paths received → `updateListWithFiles` → dirs tracked → files recursively collected → `listofPaths` deduplicated → listbox updated

Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)
- [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs)
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)
- [SetupInstaller.iss](SetupInstaller.iss)

---

### Start Shredding

Trigger: User clicks Start after queue populated.

Flow: Confirm dialog → parse passes/buffer from combos → validate queue → hide start button → `BackgroundWorker.RunWorkerAsync` → per file `ShredderController.ShreddFile` → progress updates → remove item from listbox

Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)
- [SecureFileShredder/Controllers/ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs)
- [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs)

---

### Complete Shredding

Trigger: BackgroundWorker finishes without cancel/error.

Flow: `RunWorkerCompleted` → `File.Delete` each queued file → `Directory.Delete` each root folder → success MessageBox → reset progress/UI/lists

Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

---

### Cancel or Exit

Trigger: User closes window during shred.

Flow: `btnClose_Click` → `cancelRunner` → `CancelAsync` → worker breaks → cancelled MessageBox → UI restored

Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)
- [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs)
- [SecureFileShredder/Properties/Resources.resx](SecureFileShredder/Properties/Resources.resx) — `icons8_close_50`

---

### Open About Dialog

Trigger: User clicks info icon (`btnInfo`).

Flow: `btnInfo_Click` → `new About().ShowDialog()` → user closes → `About.Dispose`

Files:
- [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)
- [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs)
- [SecureFileShredder/About.cs](SecureFileShredder/About.cs)
- [SecureFileShredder/About.Designer.cs](SecureFileShredder/About.Designer.cs)

---

### Release Build and Publish

Trigger: GitHub release `published` event.

Flow: Checkout → NuGet restore → MSBuild Release → zip artifacts → Inno Setup compile → upload installer to release

Files:
- [.github/workflows/build.yml](.github/workflows/build.yml)
- [SetupInstaller.iss](SetupInstaller.iss)
- [SecureFileShredder/SecureFileShredder.csproj](SecureFileShredder/SecureFileShredder.csproj)
- [SecureFileShredder.sln](SecureFileShredder.sln)

---

## File Responsibility Map

| Responsibility | File |
|----------------|------|
| Application entry, mutex, IPC send | [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs) |
| UI logic, queue, worker, deletion | [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs) |
| Main form layout (619×464, logo header, info/close) | [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs) |
| Main form embedded icon | [SecureFileShredder/Mainmenu.resx](SecureFileShredder/Mainmenu.resx) |
| About dialog logic | [SecureFileShredder/About.cs](SecureFileShredder/About.cs) |
| About dialog layout | [SecureFileShredder/About.Designer.cs](SecureFileShredder/About.Designer.cs) |
| About form resources | [SecureFileShredder/About.resx](SecureFileShredder/About.resx) |
| Overwrite algorithm | [SecureFileShredder/Controllers/ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs) |
| License stub (unused) | [SecureFileShredder/Controllers/LicenseController.cs](SecureFileShredder/Controllers/LicenseController.cs) |
| Project/build config, `Logo.ico` | [SecureFileShredder/SecureFileShredder.csproj](SecureFileShredder/SecureFileShredder.csproj) |
| Solution | [SecureFileShredder.sln](SecureFileShredder.sln) |
| Shared UI bitmaps (embedded) | [SecureFileShredder/Properties/Resources.resx](SecureFileShredder/Properties/Resources.resx) |
| Generated resource accessor | [SecureFileShredder/Properties/Resources.Designer.cs](SecureFileShredder/Properties/Resources.Designer.cs) |
| Source image files | `SecureFileShredder/Resources/` — `LogoPng.png`, `icons8-close-50.png`, `icons8-information-100.png`, `information.png` |
| App icon + shell context-menu icon | [SecureFileShredder/Logo.ico](SecureFileShredder/Logo.ico) (`.csproj` + installer `[Files]`) |
| Windows installer, registry, `MyAppVersion` 1.5 | [SetupInstaller.iss](SetupInstaller.iss) |
| Release CI/CD | [.github/workflows/build.yml](.github/workflows/build.yml) |
| Version history | [SecureFileShredder/ChangeLog.txt](SecureFileShredder/ChangeLog.txt) |
| AI context index | [context.md](context.md) |
| Human-facing overview | [README.md](README.md) |
| License text | [LICENSE.txt](LICENSE.txt) |
| Git ignore rules | [.gitignore](.gitignore) |
| Git attributes | [.gitattributes](.gitattributes) |

---

## Data Flow Map

### Primary shred pipeline

```
User input (drag / CLI / context menu / IPC)
  → Mainmenu.updateListWithFiles
  → listofPaths + listOfDirectories
  → btnStartDeleting (sets PASSES, Buffer_Size)
  → BackgroundWorker.DoWork
  → ShredderController.ShreddFile (RNG overwrite, N passes)
  → BackgroundWorker.RunWorkerCompleted
  → File.Delete + Directory.Delete
```

### Secondary IPC pipeline

```
Second process (context menu / CLI)
  → Program.SendFilesToExistingInstance
  → WM_COPYDATA (pipe-delimited paths)
  → Mainmenu.WndProc
  → updateListWithFiles
```

### Release pipeline

```
GitHub release published
  → build.yml (MSBuild Release)
  → SetupInstaller.iss (Inno Setup)
  → GitHub release asset upload
```

---

## Integration Registry

### Win32 user32.dll

Purpose: Window lookup, borderless drag, inter-process `WM_COPYDATA`.

Files: [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs), [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

Authentication: N/A (local P/Invoke)

Entry Points: `FindWindow`, `SendMessage`, `ReleaseCapture`, `WndProc`

---

### RNGCryptoServiceProvider

Purpose: Cryptographically random bytes for file overwrite.

Files: [SecureFileShredder/Controllers/ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs)

Authentication: N/A

Entry Points: `ShredderController.ShreddFile`

---

### Windows Registry (Shell)

Purpose: "Shred Securely" context menu for files (`*`) and directories; icon `{app}\Logo.ico`.

Files: [SetupInstaller.iss](SetupInstaller.iss), [SecureFileShredder/Logo.ico](SecureFileShredder/Logo.ico)

Authentication: N/A (installer writes HKCR)

Entry Points: `[Registry]` `*\shell\SecureFileShredder`, `Directory\shell\SecureFileShredder` (default label, `Icon`, `command`)

---

### Inno Setup

Purpose: Package Release build into Windows installer.

Files: [SetupInstaller.iss](SetupInstaller.iss), [.github/workflows/build.yml](.github/workflows/build.yml)

Authentication: N/A

Entry Points: CI `iscc` step; local compile of `SetupInstaller.iss`

---

### GitHub Releases

Purpose: Attach compiled installer to published releases.

Files: [.github/workflows/build.yml](.github/workflows/build.yml)

Authentication: `secrets.RELEASE_TOKEN` for `softprops/action-gh-release`

Entry Points: Workflow on `release: types: [published]`

---

## Dependency Impact Map

### ShredderController

Changing: [SecureFileShredder/Controllers/ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs)

May impact: Overwrite behavior, progress granularity, pass/buffer effectiveness, shred duration

---

### Mainmenu (worker and completion)

Changing: [SecureFileShredder/Mainmenu.cs](SecureFileShredder/Mainmenu.cs)

May impact: File queue, folder expansion, deletion timing, cancellation, UI state, combo parsing

---

### Mainmenu (designer)

Changing: [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs), [SecureFileShredder/Mainmenu.resx](SecureFileShredder/Mainmenu.resx)

May impact: Control bindings, drag-drop, layout, header logo, info button, form size, event wiring

---

### About form

Changing: [SecureFileShredder/About.cs](SecureFileShredder/About.cs), [SecureFileShredder/About.Designer.cs](SecureFileShredder/About.Designer.cs)

May impact: About dialog display, branding presentation, close behavior

---

### Properties/Resources

Changing: [SecureFileShredder/Properties/Resources.resx](SecureFileShredder/Properties/Resources.resx), files under `SecureFileShredder/Resources/`

May impact: Logo, close/info icons on `Mainmenu` and `About`

---

### Program (mutex and IPC)

Changing: [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)

May impact: Single-instance behavior, context-menu multi-launch, second-instance file handoff, window title lookup

---

### SetupInstaller.iss

Changing: [SetupInstaller.iss](SetupInstaller.iss)

May impact: Shell context menu label/icon/command, `MyAppVersion`, install paths, bundled files (`Logo.ico`), optional file association

---

### build.yml

Changing: [.github/workflows/build.yml](.github/workflows/build.yml)

May impact: Release artifacts only (no PR/push CI)

---

### SecureFileShredder.csproj

Changing: [SecureFileShredder/SecureFileShredder.csproj](SecureFileShredder/SecureFileShredder.csproj)

May impact: Target framework, output type, icon, content copy rules

---

## Known Conventions

- Namespace `SecureFileShredder`; controllers in `SecureFileShredder.Controllers`
- Form class `Mainmenu`; window title `"Mainmenu"` used by `FindWindow`
- `Controllers/` for extracted logic (refactor in progress; deletion still in form)
- Designer code in `*.Designer.cs`; strings/images in `*.resx`
- **UI typography**: title `Copperplate Gothic Bold` (maroon); config labels `Bahnschrift`; shred button `Bahnschrift SemiBold` (red/maroon)
- **UI colors**: form background `#E0E0E0`; primary accent maroon; CTA button red with maroon border
- **Main form size**: 619×464; header `pictureBox1` + `label1`; chrome buttons `btnInfo`, `btnClose` (top-right)
- **Bitmap resources**: `LogoPng`, `icons8_close_50`, `icons8_information_100` (plus legacy `icons8_close_48`, `information`)
- **Application icon**: `Logo.ico` in `.csproj` (`ApplicationIcon`, `CopyToOutputDirectory`); same file used for Explorer context-menu icon via installer `Icon` registry value
- **Installer version** (`MyAppVersion` in [SetupInstaller.iss](SetupInstaller.iss)): 1.5 — not synced from GitHub release tag or `.csproj` assembly version
- **About UI version label**: `Version 1.5` in [About.Designer.cs](SecureFileShredder/About.Designer.cs) (display only)
- Combo items encode numeric values; parsed via `Split` on shred start
- Progress max during overwrite: `fileCount * passes`
- Progress during deletion: `listofPaths.Count` (separate phase)
- Pass options: 1, 3, 7, 12, 35, 55 (label names only; same RNG algorithm)
- Buffer options: 1 KB–512 KB (default 4 KB)
- MIT license; installer publisher Softasium ([SetupInstaller.iss](SetupInstaller.iss))
- No NuGet dependencies; BCL only

---

## Maintenance Rules

Update this file whenever:

- A feature is added or removed
- A workflow changes
- Business logic moves between `Mainmenu` and controllers
- Files are renamed or relocated
- Architecture changes (new layers, DI, tests)
- New integrations are introduced
- Installer or registry behavior changes
- CI/release pipeline changes
- API or IPC contracts change (e.g. `WM_COPYDATA` format)

---

## Context Optimization Rules

Keep this file:

- Concise and high signal
- AI-readable and easy to search
- Easy to update (paths and flows, not prose)

Do NOT include:

- Code snippets
- Long explanations
- User documentation or installation steps
- Detailed implementation logic

Focus on:

- Navigation to correct files
- Architecture and layer boundaries
- Workflow chains
- Dependency and change-impact mapping
