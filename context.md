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

- **WinForms partial class**: UI in [Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs) + [Mainmenu.resx](SecureFileShredder/Mainmenu.resx); behavior in [Mainmenu.cs](SecureFileShredder/Mainmenu.cs)
- **Shredding algorithm** in [ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs); **queue, config, progress, deletion** stay in `Mainmenu`
- **Static shared state**: `PASSES`, `Buffer_Size` on `Mainmenu`, set at shred start, consumed by `ShredderController`
- **Overwrite then delete**: `BackgroundWorker` overwrites; `File.Delete` / `Directory.Delete` in `RunWorkerCompleted` on UI thread
- **Single instance**: Mutex `"SecureShredder"` in [Program.cs](SecureFileShredder/Program.cs); second instance uses `WM_COPYDATA` IPC
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

Purpose: Windows context menu "Shred Securely" for files and folders.

Entry Points:
- [SetupInstaller.iss](SetupInstaller.iss) — `[Registry]` section

Primary Files:
- [SetupInstaller.iss](SetupInstaller.iss)

Related Files:
- [.github/workflows/build.yml](.github/workflows/build.yml)
- [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)

Dependencies: Inno Setup, HKCR registry keys

Workflow: Install → registry invokes `SecureFileShredder.exe "%1"` → Program/Mainmenu queue flow

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
- [SecureFileShredder/Properties/Resources.resx](SecureFileShredder/Properties/Resources.resx) — close icon

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
| WinForms controls layout | [SecureFileShredder/Mainmenu.Designer.cs](SecureFileShredder/Mainmenu.Designer.cs) |
| Form embedded resources (icon) | [SecureFileShredder/Mainmenu.resx](SecureFileShredder/Mainmenu.resx) |
| Overwrite algorithm | [SecureFileShredder/Controllers/ShredderController.cs](SecureFileShredder/Controllers/ShredderController.cs) |
| License stub (unused) | [SecureFileShredder/Controllers/LicenseController.cs](SecureFileShredder/Controllers/LicenseController.cs) |
| Project/build config | [SecureFileShredder/SecureFileShredder.csproj](SecureFileShredder/SecureFileShredder.csproj) |
| Solution | [SecureFileShredder.sln](SecureFileShredder.sln) |
| UI bitmap resources | [SecureFileShredder/Properties/Resources.resx](SecureFileShredder/Properties/Resources.resx) |
| Generated resource accessor | [SecureFileShredder/Properties/Resources.Designer.cs](SecureFileShredder/Properties/Resources.Designer.cs) |
| App icon (content, copy to output) | `SecureFileShredder/icons8-demolition-96.ico` |
| Windows installer + registry | [SetupInstaller.iss](SetupInstaller.iss) |
| Release CI/CD | [.github/workflows/build.yml](.github/workflows/build.yml) |
| Version history | [SecureFileShredder/ChangeLog.txt](SecureFileShredder/ChangeLog.txt) |
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

Purpose: "Shred Securely" context menu for files (`*`) and directories.

Files: [SetupInstaller.iss](SetupInstaller.iss)

Authentication: N/A (installer writes HKCR)

Entry Points: `[Registry]` keys under `*\shell\SecureFileShredder`, `Directory\shell\SecureFileShredder`

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

May impact: Control bindings, drag-drop, layout, event wiring

---

### Program (mutex and IPC)

Changing: [SecureFileShredder/Program.cs](SecureFileShredder/Program.cs)

May impact: Single-instance behavior, context-menu multi-launch, second-instance file handoff, window title lookup

---

### SetupInstaller.iss

Changing: [SetupInstaller.iss](SetupInstaller.iss)

May impact: Shell context menu, install paths, bundled files, optional file association

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
