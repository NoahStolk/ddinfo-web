open System.IO
open System.IO.Compression

[<EntryPoint>]
let main argv =
    let dirPath = argv[0]
    let zipPath = argv[1]

    File.Delete(zipPath)
    printfn $"Deleted {zipPath}"

    ZipFile.CreateFromDirectory(dirPath, zipPath)
    printfn $"Created {zipPath} from directory {dirPath}"

    Directory.Delete(dirPath, true)
    printfn $"Deleted {dirPath}"
    0
