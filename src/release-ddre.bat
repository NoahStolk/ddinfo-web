@echo off
set root=%cd%\
set app_source_file=app\DevilDaggersInfo.App.ReplayEditor.Photino\DevilDaggersInfo.App.ReplayEditor.Photino.csproj
set distribute_tool_dir=tool\DevilDaggersInfo.Tool.DistributeApp\

rem Zip and clean publish directory
cd %root%
cd %distribute_tool_dir%
dotnet run -- %root%%app_source_file% %root%DevilDaggersReplayEditor.zip

cd %root%

rem TODO: Build and publish for Windows 7
rem dotnet build -c Release -o bin\publish-win7\ --self-contained false
