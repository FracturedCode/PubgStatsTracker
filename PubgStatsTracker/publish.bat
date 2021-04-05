@echo off
cls
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:PublishReadyToRun=true --self-contained false
pause