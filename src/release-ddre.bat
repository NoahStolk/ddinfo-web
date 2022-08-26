@echo off
set root=%cd%\
set app_project_file=app\DevilDaggersInfo.App.ReplayEditor.Photino\DevilDaggersInfo.App.ReplayEditor.Photino.csproj

cd tool\DevilDaggersInfo.Tool.DistributeApp\
dotnet run -- %root%%app_project_file% %root%
