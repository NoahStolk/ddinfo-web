module DevilDaggersInfo.Tool.DistributeApp.AppBuilder

open System.Diagnostics
open System.Text
open DevilDaggersInfo.Types.Web

let getPublishCommandProperties (publishDirectoryName, runtimeIdentifier, publishMethod, singleFile) : Map<string, string> =
    let singleFileStr = singleFile.ToString()
    Map.empty
        .Add("PublishSingleFile", singleFileStr)
        .Add("SelfContained", singleFileStr)
        .Add("PublishTrimmed", "True")
        .Add("EnableCompressionInSingleFile", "True")
        .Add("PublishReadyToRun", "False")
        .Add("PublishProtocol", "FileSystem")
        .Add("TargetFramework", "net7.0")
        .Add("RuntimeIdentifier", runtimeIdentifier)
        .Add("Platform", "x64")
        .Add("Configuration", "Release")
        .Add("PublishMethod", publishMethod)
        .Add("PublishDir", publishDirectoryName)

let getPublishCommand projectFilePath publishDirectoryName toolBuildType toolPublishMethod : string =
    let runtimeIdentifier = match toolBuildType with
                            | ToolBuildType.LinuxWarp -> "linux-x64"
                            | ToolBuildType.WindowsWarp -> "win-x64"
                            | _ -> failwith "Build type not supported"
    let publishMethod = match toolPublishMethod with
                        | ToolPublishMethod.SelfContained -> "SELF_CONTAINED"
                        | _ -> "" // TODO: OK?

    let sb = StringBuilder()
    getPublishCommandProperties(publishDirectoryName, runtimeIdentifier, publishMethod, toolPublishMethod = ToolPublishMethod.SelfContained) |> Map.iter (fun key value -> sb.Append($" -p:{key}={value}") |> ignore) 
    $"publish {projectFilePath}{sb.ToString()}"

let build projectFilePath publishDirectoryName toolBuildType toolPublishMethod =
    let dotnetPublishProc = Process.Start(ProcessStartInfo(FileName = "dotnet", Arguments = getPublishCommand projectFilePath publishDirectoryName toolBuildType toolPublishMethod))
    dotnetPublishProc.WaitForExit()

    match dotnetPublishProc.ExitCode with
    | 0 -> ()
    | _ -> failwith "Could not build the app"
