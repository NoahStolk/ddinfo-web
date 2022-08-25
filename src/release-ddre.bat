@echo off
set root=%cd%\
set app_source_dir=app\DevilDaggersInfo.App.ReplayEditor.Photino\
set app_publish_dir_name=_temp-release\
set distribute_tool_dir=tool\DevilDaggersInfo.Tool.DistributeApp\

rem Build and publish for Windows 8+
cd %app_source_dir%
dotnet publish ^
-p:PublishSingleFile=True ^
-p:PublishTrimmed=True ^
-p:EnableCompressionInSingleFile=True ^
-p:PublishReadyToRun=False ^
-p:PublishProtocol=FileSystem ^
-p:SelfContained=true ^
-p:TargetFramework=net6.0 ^
-p:RuntimeIdentifier=win-x64 ^
-p:Platform=x64 ^
-p:Configuration=Release ^
-p:PublishMethod=SELF_CONTAINED ^
-p:PublishDir=%app_publish_dir_name%

rem Zip and clean publish directory
cd %root%
cd %distribute_tool_dir%
dotnet run -- %root%%app_source_dir%%app_publish_dir_name% %root%DevilDaggersReplayEditor.zip

cd %root%

rem TODO: Build and publish for Windows 7
rem dotnet build -c Release -o bin\publish-win7\ --self-contained false
