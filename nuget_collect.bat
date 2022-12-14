@echo off
mkdir Nuget

copy .\NIdentity.Core\bin\Release\*.nupkg .\Nuget
copy .\NIdentity.Core.X509\bin\Release\*.nupkg .\Nuget
copy .\NIdentity.Connector\bin\Release\*.nupkg .\Nuget
copy .\NIdentity.Connector.AspNetCore\bin\Release\*.nupkg .\Nuget

pause