#define MyAppName "WinMin"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "AGG Productions"
#define MyAppExeName "WinMin.exe"

[Setup]
AppId={{WinMin}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName=C:\Users\Public\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=C:\Users\User\Desktop
OutputBaseFilename=WinMinSetup
SetupIconFile=C:\Users\User\source\repos\WinMin\WinMin\Images\WinMin.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce

[Files]
Source: "C:\Users\User\source\repos\WinMin\WinMin\bin\Debug\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\User\source\repos\WinMin\WinMin\bin\Debug\WinMin.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\User\source\repos\WinMin\WinMin\bin\Debug\WinMin.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\User\source\repos\WinMin\WinMin\bin\Debug\WinMin.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\WinMin.xml"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "schtasks.exe"; \
    Parameters: "/create /XML ""{app}\WinMin.xml"" /TN WinMin"