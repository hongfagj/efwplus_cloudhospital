; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "efwplusClient"
#define MyAppVersion "2.0"
#define MyAppPublisher "曾浩, Inc."
#define MyAppURL "http://www.efwplus.cn"
#define efwplusClientExeName "efwplusClient.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{6BA7AF33-3F41-40D3-9335-B3B9F76245C1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=D:\工作台\efwplus产品目录\efwplus_cloudhospital\发布打包
OutputBaseFilename=setup_efwplusClient
;SetupIconFile=D:\工作台\efwplus_wcfframe\发布打包\client.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "D:\工作台\efwplus产品目录\efwplus_cloudhospital\发布打包\efwplusClient\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\efwplusClient客户端"; Filename: "{app}\{#efwplusClientExeName}"
Name: "{group}\Uninstall"; Filename: "{uninstallexe}"
