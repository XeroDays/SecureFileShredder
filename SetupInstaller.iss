; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Secure Shredder"
#define MyAppVersion "1.0"
#define MyAppPublisher "Softasium Software Solutions"
#define MyAppURL "https://www.softasium.com"
#define MyAppExeName "SecureFileShredder.exe"
#define MyAppAssocName MyAppName + ""
#define MyAppAssocExt ".exe"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
AppId={{BA7DC219-9B3E-4628-83FD-D9ACDF4FAD7A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
ChangesAssociations=yes
DefaultGroupName={#MyAppName}
OutputBaseFilename=Secure Shredder
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "SecureFileShredder\bin\Release\net8.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "SecureFileShredder\bin\Release\net8.0-windows\SecureFileShredder.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "SecureFileShredder\bin\Release\net8.0-windows\SecureFileShredder.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "SecureFileShredder\bin\Release\net8.0-windows\SecureFileShredder.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "SecureFileShredder\bin\Release\net8.0-windows\SecureFileShredder.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "SecureFileShredder\bin\Release\net8.0-windows\icons8-demolition-96.ico"; DestDir: "{app}"; Flags: ignoreversion

[Registry]
; File association for .exe files (optional)
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

; Context menu for all files
Root: HKCR; Subkey: "*\shell\SecureFileShredder"; ValueType: string; ValueName: ""; ValueData: "Shred Securely"; Flags: uninsdeletekey
Root: HKCR; Subkey: "*\shell\SecureFileShredder\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Flags: uninsdeletekey
Root: HKCR; Subkey: "*\shell\SecureFileShredder"; ValueType: string; ValueName: "Icon"; ValueData: "{app}\icons8-demolition-96.ico"; Flags: uninsdeletekey

; Context menu for directories
Root: HKCR; Subkey: "Directory\shell\SecureFileShredder"; ValueType: string; ValueName: ""; ValueData: "Shred Securely"; Flags: uninsdeletekey
Root: HKCR; Subkey: "Directory\shell\SecureFileShredder\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Flags: uninsdeletekey
Root: HKCR; Subkey: "Directory\shell\SecureFileShredder"; ValueType: string; ValueName: "Icon"; ValueData: "{app}\icons8-demolition-96.ico"; Flags: uninsdeletekey

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
