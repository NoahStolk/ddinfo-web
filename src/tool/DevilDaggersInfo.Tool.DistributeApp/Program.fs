open System.IO
open DevilDaggersInfo.Tool.DistributeApp
open DevilDaggersInfo.Types.Web

[<Literal>]
let publishDirectoryName = "_temp-release"

[<Literal>]
let toolName = "ddinfo-tools"

[<Literal>]
let contentFileName = "ddinfo"
let contentFileNameUi = "ddinfo-ui"

let buildAndUpload projectFilePath zipOutputDirectory toolBuildType toolPublishMethod =
    AppBuilder.build projectFilePath publishDirectoryName toolBuildType toolPublishMethod

    let publishDirectoryPath = Path.Combine(Path.GetDirectoryName(projectFilePath:string), publishDirectoryName)

    // Copy content files.
    let contentFilePath = Path.Combine(projectFilePath, "..", "bin", "Debug", "net7.0", contentFileName)
    let contentFilePathUi = Path.Combine(projectFilePath, "..", "bin", "Debug", "net7.0", contentFileNameUi)
    File.Copy(contentFilePath, Path.Combine(publishDirectoryPath, contentFileName))
    File.Copy(contentFilePathUi, Path.Combine(publishDirectoryPath, contentFileNameUi))
    
    // Zip build and content file.
    let version = ProjectReader.readVersionFromProjectFile projectFilePath
    let outputZipFilePath = Path.Combine(zipOutputDirectory, $"{toolName}-{version}-{toolBuildType}-{toolPublishMethod}.zip")
    ZipWriter.zip outputZipFilePath publishDirectoryPath

    ApiHttpClient.upload toolName version toolBuildType toolPublishMethod outputZipFilePath ApiHttpClient.login.Token

    printfn $"Deleting zip file '{outputZipFilePath}'"
    File.Delete(outputZipFilePath)

[<EntryPoint>]
let main _ =
    let projectFilePath = """C:\Users\NOAH\source\repos\DevilDaggersInfo\src\app\DevilDaggersInfo.App\DevilDaggersInfo.App.csproj"""
    let zipOutputDirectory = """C:\Users\NOAH\source\repos\DevilDaggersInfo\src\app\DevilDaggersInfo.App\bin"""

    buildAndUpload projectFilePath zipOutputDirectory ToolBuildType.WindowsWarp ToolPublishMethod.SelfContained
    buildAndUpload projectFilePath zipOutputDirectory ToolBuildType.LinuxWarp ToolPublishMethod.SelfContained

    0
