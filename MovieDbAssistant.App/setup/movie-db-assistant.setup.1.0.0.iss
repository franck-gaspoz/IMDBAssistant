; Inno Setup 6.3.3
; https://jrsoftware.org/isdl.php

[Setup]
SourceDir=..\bin\x64\Release\net8.0-windows10.0.22621.0\win-x64

AppName=Movie Db Assistant
AppVersion=1.0.0
VersionInfoVersion=1.0.0
VersionInfoProductVersion=1.0.0
AppComments=https://github.com/franck-gaspoz/MovieDbAssistant/blob/main/doc/manual.md
AppContact=franck.gaspoz@gmail.com
VersionInfoCompany=franck.gaspoz@gmail.com
AppCopyright=(c) 2024 GPL-V3 Franck Gaspoz
VersionInfoCopyright=(c) 2024 GPL-V3 Franck Gaspoz
AppPublisher=Franck Gaspoz Software

ArchitecturesAllowed=x64compatible
AllowCancelDuringInstall=no

WizardStyle=modern
WizardImageFile=setup\setup.bmp

DefaultDirName={autopf}\Movie Db Assistant
DefaultGroupName=Movie Db Assistant
UninstallDisplayIcon={app}\MovieDbAssistant.exe
Compression=lzma2
SolidCompression=yes
PrivilegesRequired=lowest
OutputDir=..\..\..\..\release
OutputBaseFilename=movie-db-assistant.setup.1.0.0
LicenseFile=setup\LICENSE
InfoBeforeFile=setup\readme.txt

DisableWelcomePage=no
UserInfoPage=no
DisableDirPage=no
DisableProgramGroupPage=yes
DisableReadyPage=no
DisableReadyMemo=no

//#define Password 'password'
//Password={#Password}

[Files]
Source: "MovieDbAssistant.exe"; DestDir: "{app}"
Source: "setup\readme.txt"; DestDir: "{app}";
Source: *; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\Movie Db Assistant"; Filename: "{app}\MovieDbAssistant.exe"

//[Components]
//Name: "component"; Description: "Component";

[Tasks]

//Name: "task"; Description: "Task";
//Name: StartAfterInstall; Description: Run application after install
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Run]
Filename: {app}\readme.txt; Description: View the README file; Flags: postinstall shellexec skipifsilent unchecked
Filename: {app}\MovieDbAssistant.exe; Description: Run Application; Flags: postinstall nowait skipifsilent

[Code]
var
  OutputProgressWizardPage: TOutputProgressWizardPage;
  OutputMarqueeProgressWizardPage: TOutputMarqueeProgressWizardPage;
  OutputProgressWizardPagesAfterID: Integer;

procedure InitializeWizard;
var
  //InputQueryWizardPage: TInputQueryWizardPage;
  //InputOptionWizardPage: TInputOptionWizardPage;
  //InputDirWizardPage: TInputDirWizardPage;
  //InputFileWizardPage: TInputFileWizardPage;
  //OutputMsgWizardPage: TOutputMsgWizardPage;
  //OutputMsgMemoWizardPage: TOutputMsgMemoWizardPage;
  AfterID: Integer;
begin
  WizardForm.LicenseAcceptedRadio.Checked := True;
  //WizardForm.PasswordEdit.Text := '{#Password}';
  //WizardForm.UserInfoNameEdit.Text := 'Username';

  //AfterID := wpSelectTasks;
  
  //AfterID := CreateCustomPage(AfterID, 'CreateCustomPage', 'ADescription').ID;
  
  //InputQueryWizardPage := CreateInputQueryPage(AfterID, 'CreateInputQueryPage', 'ADescription', 'ASubCaption');
  //InputQueryWizardPage.Add('&APrompt:', False);
  //AfterID := InputQueryWizardPage.ID;
  
  //InputOptionWizardPage := CreateInputOptionPage(AfterID, 'CreateInputOptionPage', 'ADescription', 'ASubCaption', False, False);
  //InputOptionWizardPage.Add('&AOption');
  //AfterID := InputOptionWizardPage.ID;

  //InputDirWizardPage := CreateInputDirPage(AfterID, 'CreateInputDirPage', 'ADescription', 'ASubCaption', False, 'ANewFolderName');
  //InputDirWizardPage.Add('&APrompt:');
  //InputDirWizardPage.Values[0] := 'C:\';
  //AfterID := InputDirWizardPage.ID;

  //InputFileWizardPage := CreateInputFilePage(AfterID, 'CreateInputFilePage', 'ADescription', 'ASubCaption');
  //InputFileWizardPage.Add('&APrompt:', 'Executable files|*.exe|All files|*.*', '.exe');
  //AfterID := InputFileWizardPage.ID;

  //OutputMsgWizardPage := CreateOutputMsgPage(AfterID, 'CreateOutputMsgPage', 'ADescription', 'AMsg');
  //AfterID := OutputMsgWizardPage.ID;

  //OutputMsgMemoWizardPage := CreateOutputMsgMemoPage(AfterID, 'CreateOutputMsgMemoPage', 'ADescription', 'ASubCaption', 'AMsg');
  //AfterID := OutputMsgMemoWizardPage.ID;

  //OutputProgressWizardPage := CreateOutputProgressPage('CreateOutputProgressPage', 'Installing files...');
  //OutputMarqueeProgressWizardPage := CreateOutputMarqueeProgressPage('CreateOutputMarqueeProgressPage', 'Updating system...');
  //OutputProgressWizardPagesAfterID := AfterID;

  { See CodeDownloadFiles.iss for a CreateDownloadPage example }
end;

//function ShouldSkipPage(PageID: Integer): Boolean;
//begin
//  Result := False;
//  if PageID = 6 then
//    Result := True;
//end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  I, Max: Integer;
begin
  if CurPageID = OutputProgressWizardPagesAfterID then begin
    try
      Max := 50;
      for I := 0 to Max do begin
        OutputProgressWizardPage.SetProgress(I, Max);
        if I = 0 then
          OutputProgressWizardPage.Show;
        Sleep(2000 div Max);
      end;
    finally
      OutputProgressWizardPage.Hide;
    end;
    try
      Max := 50;
      OutputMarqueeProgressWizardPage.Show;
      for I := 0 to Max do begin
        OutputMarqueeProgressWizardPage.Animate;
        Sleep(2000 div Max);
      end;
    finally
      OutputMarqueeProgressWizardPage.Hide;
    end;
  end;
  Result := True;
end;

//function PrepareToInstall(var NeedsRestart: Boolean): String;
//begin
//  if SuppressibleMsgBox('Do you want to stop Setup at the Preparing To Install wizard page?', mbConfirmation, MB_YESNO, IDNO) = IDYES then
//    Result := 'Stopped by user';
//end;
