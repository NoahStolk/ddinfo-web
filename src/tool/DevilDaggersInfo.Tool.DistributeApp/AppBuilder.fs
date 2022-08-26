module DevilDaggersInfo.Tool.DistributeApp.AppBuilder

open System
open System.Diagnostics
open System.Text

let getPublishCommandProperties publishDirectoryName : Map<string, string> =
    Map.empty
        .Add("PublishSingleFile", "True")
        .Add("PublishTrimmed", "True")
        .Add("EnableCompressionInSingleFile", "True")
        .Add("PublishReadyToRun", "False")
        .Add("PublishProtocol", "FileSystem")
        .Add("SelfContained", "true")
        .Add("TargetFramework", "net6.0")
        .Add("RuntimeIdentifier", "win-x64")
        .Add("Platform", "x64")
        .Add("Configuration", "Release")
        .Add("PublishMethod", "SELF_CONTAINED")
        .Add("PublishDir", publishDirectoryName)

let getPublishCommand projectFilePath publishDirectoryName : string =
    let sb = StringBuilder()
    getPublishCommandProperties publishDirectoryName |> Map.iter (fun key value -> sb.Append($" -p:{key}={value}") |> ignore) 
    $"publish {projectFilePath}{sb.ToString()}"

let build projectFilePath publishDirectoryName =
    let dotnetPublishProc = Process.Start(ProcessStartInfo(FileName = "dotnet", Arguments = getPublishCommand projectFilePath publishDirectoryName))
    dotnetPublishProc.WaitForExit()

    match dotnetPublishProc.ExitCode with
    | 0 -> ()
    | _ -> failwith "Could not build the app"

// dotnet build -c Release -o bin\publish-win7\ --self-contained false
