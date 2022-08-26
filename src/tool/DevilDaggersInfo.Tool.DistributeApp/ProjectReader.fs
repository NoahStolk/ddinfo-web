module DevilDaggersInfo.Tool.DistributeApp.ProjectReader

open System.IO
open System.Xml

type readResult(name: string, version: string) =
    member this.Name = name
    member this.Version = version

let readProjectFile projectFilePath =
    let doc = XmlDocument() in
    doc.LoadXml(File.ReadAllText(projectFilePath))

    let nameList = doc.SelectNodes "/Project/PropertyGroup/AssemblyName/text()"
    let name =
        match nameList.Count with
        | 0 -> failwith "Name not found in project file"
        | _ -> nameList.Item(0).Value

    let versionList = doc.SelectNodes "/Project/PropertyGroup/Version/text()"
    let version =
        match versionList.Count with
        | 0 -> failwith "Version not found in project file"
        | _ -> versionList.Item(0).Value

    let result = readResult (name, version)
    result
