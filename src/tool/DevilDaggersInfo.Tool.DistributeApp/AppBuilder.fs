module DevilDaggersInfo.Tool.DistributeApp.AppBuilder

open System
open System.Diagnostics

let build projectFilePath publishDirectoryName =
    let dotnetPublish = ProcessStartInfo(FileName = "dotnet", Arguments = $"publish {projectFilePath} -p:PublishSingleFile=True -p:PublishTrimmed=True -p:EnableCompressionInSingleFile=True -p:PublishReadyToRun=False -p:PublishProtocol=FileSystem -p:SelfContained=true -p:TargetFramework=net6.0 -p:RuntimeIdentifier=win-x64 -p:Platform=x64 -p:Configuration=Release -p:PublishMethod=SELF_CONTAINED -p:PublishDir={publishDirectoryName}")
    let dotnetPublishProc = Process.Start(dotnetPublish)
    dotnetPublishProc.WaitForExit()
    
    match dotnetPublishProc.ExitCode with
    | 0 -> ()
    | _ -> Environment.Exit(dotnetPublishProc.ExitCode)

// dotnet build -c Release -o bin\publish-win7\ --self-contained false
