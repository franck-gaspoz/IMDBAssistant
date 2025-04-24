#------------------------------------------------------
# make appx: copy publish files + make & register appx
#------------------------------------------------------

#Get-AppxPackage -publisher "CN=Franck Gaspoz Software, O=Franck Gaspoz Corporation, C=US"
#Remove-AppxPackage MovieDbAssistant_1.0.0.0_x64__xtrrbsjxvn07w

rmdir content -recurse
mkdir content
xcopy ..\MovieDbAssistant.App\bin\x64\Release\net8.0-windows10.0.22621.0\win-x64\ content /Q /E /Y

xcopy appxmanifest.xml content\ /Y
xcopy assets content\assets\ /Y

& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makepri.exe' createconfig /cf content\priconfig.xml /dq en-US
& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makepri.exe' new /pr "C:\Users\franc\source\repos\MovieDbAssistant\package\content" /cf "C:\Users\franc\source\repos\MovieDbAssistant\package\content\priconfig.xml" /in MovieDbAssistant
xcopy *.pri content\ /Y

# default to: SHA256
& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makeappx.exe' pack /d content /p MovieDbAssistant_1.0.0_x64.msix

#add-appxpackage –register AppxManifest.xml

# unpack (test)
#rmdir extract -recurse
#& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\makeappx.exe' unpack /d extract /p MovieDbAssistant_1.0.0_x64.msix

# sign
# method (AppxBlockMap.xml:BlockMap:HashMethod) : http://www.w3.org/2001/04/xmlenc#sha256

& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\SignTool.exe' sign /fd SHA256 /a /f franck-gaspoz-software-cert.pfx /p mypassword1234 MovieDbAssistant_1.0.0_x64.msix

## Done Adding Additional Store
## Successfully signed: MovieDbAssistant_1.0.0_x64_win.msix

Remove-AppPackage FranckGaspoz.Software.MovieDbAssistant_1.0.0.0_x64__xtrrbsjxvn07w
Add-AppxPackage -Path '.\MovieDbAssistant_1.0.0_x64.msix'
Get-AppxPackage -publisher "CN=Franck Gaspoz Software, O=Franck Gaspoz Corporation, C=US"
