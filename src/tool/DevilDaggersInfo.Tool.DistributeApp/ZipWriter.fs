module DevilDaggersInfo.Tool.DistributeApp.ZipWriter

open System.IO
open System.IO.Compression

let zip zipPath dirPath =
    printfn $"Deleting previous .zip file '{zipPath}' if present"
    File.Delete(zipPath)

    printfn $"Creating '{zipPath}' from temporary directory '{dirPath}'"
    ZipFile.CreateFromDirectory(dirPath, zipPath)

    printfn $"Deleting temporary directory '{dirPath}'"
    Directory.Delete(dirPath, true)
