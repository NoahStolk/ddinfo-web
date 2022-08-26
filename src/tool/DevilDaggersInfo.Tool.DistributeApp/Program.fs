open System.IO
open DevilDaggersInfo.Tool.DistributeApp

[<EntryPoint>]
let main argv =
    let projectFilePath = argv[0]
    let zipOutputDirectory = argv[1]

    let publishDirectoryName = "_temp-release"
    AppBuilder.build projectFilePath publishDirectoryName

    let proj = ProjectReader.readProjectFile projectFilePath
    
    let publishDirectoryPath = Path.Combine(Path.GetDirectoryName(projectFilePath), publishDirectoryName)
    let outputZipFilePath = Path.Combine(zipOutputDirectory, $"{proj.Name}-{proj.Version}-WindowsPhotino-SelfContained.zip")

    ZipWriter.zip outputZipFilePath publishDirectoryPath

    0
