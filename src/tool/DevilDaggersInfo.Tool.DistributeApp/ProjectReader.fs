module DevilDaggersInfo.Tool.DistributeApp.ProjectReader

open System.IO
open System.Xml

let readVersionFromProjectFile projectFilePath =
    let doc = XmlDocument() in
    doc.LoadXml(File.ReadAllText(projectFilePath))

    let versionList = doc.SelectNodes "/Project/PropertyGroup/Version/text()"
    match versionList.Count with
    | 0 -> failwith "Version not found in project file"
    | _ -> versionList.Item(0).Value
