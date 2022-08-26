open System
open System.Diagnostics
open System.IO
open DevilDaggersInfo.Tool.DistributeApp

[<EntryPoint>]
let main argv =
    let projectFilePath = argv[0]
    let zipOutputDirectory = argv[1]

    let publishDirectoryName = "_temp-release"
    AppBuilder.build projectFilePath publishDirectoryName

    let publishDirectoryPath = Path.Combine(Path.GetDirectoryName(projectFilePath), publishDirectoryName)
    let outputZipFilePath = Path.Combine(zipOutputDirectory, "DevilDaggersReplayEditor.zip")
    ZipWriter.zip outputZipFilePath publishDirectoryPath
    0
