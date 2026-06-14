[Setup]
AppName=RemoteApp Tool v2
AppVersion=2.0.0
DefaultDirName={autopf}\RemoteApp Tool v2
DefaultGroupName=RemoteApp Tool
UninstallDisplayIcon={app}\RemoteAppTool.App.exe
Compression=lzma2
SolidCompression=yes
OutputDir=.
OutputBaseFilename=RemoteAppTool-v2-Setup

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop shortcut"; GroupDescription: "Additional icons:"; Flags: unchecked

[Files]
Source: "RemoteAppTool-v2-Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\RemoteApp Tool"; Filename: "{app}\RemoteAppTool.App.exe"
Name: "{autodesktop}\RemoteApp Tool"; Filename: "{app}\RemoteAppTool.App.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\RemoteAppTool.App.exe"; Description: "Launch RemoteApp Tool"; Flags: nowait postinstall skipifsilent
