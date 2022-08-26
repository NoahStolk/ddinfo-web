open System
open System.Diagnostics
open System.IO
open DevilDaggersInfo.Tool.DistributeApp

[<EntryPoint>]
let main argv =
    let sourcePath = argv[0]
    let zipPath = argv[1]

    let tempReleaseDir = "_temp-release"
    let dotnetPublish = ProcessStartInfo(FileName = "dotnet", Arguments = $"publish {sourcePath} -p:PublishSingleFile=True -p:PublishTrimmed=True -p:EnableCompressionInSingleFile=True -p:PublishReadyToRun=False -p:PublishProtocol=FileSystem -p:SelfContained=true -p:TargetFramework=net6.0 -p:RuntimeIdentifier=win-x64 -p:Platform=x64 -p:Configuration=Release -p:PublishMethod=SELF_CONTAINED -p:PublishDir={tempReleaseDir}")
    let dotnetPublishProc = Process.Start(dotnetPublish)
    dotnetPublishProc.WaitForExit()
    
    match dotnetPublishProc.ExitCode with
    | 0 -> ()
    | _ -> Environment.Exit(dotnetPublishProc.ExitCode)
    
    let dirPath = Path.Combine(Path.GetDirectoryName(sourcePath), tempReleaseDir)
    ZipWriter.zip zipPath dirPath
    0
