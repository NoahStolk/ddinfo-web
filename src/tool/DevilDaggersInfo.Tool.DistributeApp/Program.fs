open System.IO
open DevilDaggersInfo.Tool.DistributeApp
open DevilDaggersInfo.Types.Web

[<Literal>]
let publishDirectoryName = "_temp-release"

let buildAndUpload projectFilePath zipOutputDirectory toolBuildType toolPublishMethod =
    AppBuilder.build projectFilePath publishDirectoryName toolBuildType toolPublishMethod

    let publishDirectoryPath = Path.Combine(Path.GetDirectoryName(projectFilePath:string), publishDirectoryName)
    let proj = ProjectReader.readProjectFile projectFilePath
    let outputZipFilePath = Path.Combine(zipOutputDirectory, $"{proj.Name}-{proj.Version}-{toolBuildType}-{toolPublishMethod}.zip")

    ZipWriter.zip outputZipFilePath publishDirectoryPath

    ApiHttpClient.upload "ddinfo-tools" proj.Version toolBuildType toolPublishMethod outputZipFilePath ApiHttpClient.login.Token

    printfn $"Deleting zip file '{outputZipFilePath}'"
    File.Delete(outputZipFilePath)

[<EntryPoint>]
let main argv =
    let projectFilePath = argv[0]
    let zipOutputDirectory = argv[1]

    buildAndUpload projectFilePath zipOutputDirectory ToolBuildType.WindowsWarp ToolPublishMethod.SelfContained
    buildAndUpload projectFilePath zipOutputDirectory ToolBuildType.LinuxWarp ToolPublishMethod.SelfContained

    0
