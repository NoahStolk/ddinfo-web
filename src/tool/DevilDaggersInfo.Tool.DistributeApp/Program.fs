open System.IO
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Net.Http.Json
open System.Text.Json
open DevilDaggersInfo.Tool.DistributeApp
open DevilDaggersInfo.Api.Admin.Tools
open DevilDaggersInfo.Api.Main.Authentication
open DevilDaggersInfo.Types.Web

[<EntryPoint>]
let main argv =
    let projectFilePath = argv[0]
    let zipOutputDirectory = argv[1]

    let publishDirectoryName = "_temp-release"
    AppBuilder.build projectFilePath publishDirectoryName

    let publishDirectoryPath = Path.Combine(Path.GetDirectoryName(projectFilePath), publishDirectoryName)
    let proj = ProjectReader.readProjectFile projectFilePath
    let outputZipFilePath = Path.Combine(zipOutputDirectory, $"{proj.Name}-{proj.Version}-WindowsPhotino-SelfContained.zip")

    ZipWriter.zip outputZipFilePath publishDirectoryPath

    let client = new HttpClient() 

    // Authenticate
    let loginRequest = new HttpRequestMessage(
        HttpMethod.Post,
        "https://devildaggers.info/api/authentication/login",
        Content = JsonContent.Create(JsonSerializer.Deserialize<LoginRequest>(File.ReadAllText("appsettings.json"))))
    let loginAsync = async {
        let! response = client.SendAsync(loginRequest) |> Async.AwaitTask
        match response.StatusCode with
        | HttpStatusCode.OK -> ()
        | _ -> failwith $"Unsuccessful status code from login '{response.StatusCode}'"

        let! loginResponse = response.Content.ReadFromJsonAsync<LoginResponse>() |> Async.AwaitTask
        return loginResponse
    }
    let loginResponse = loginAsync |> Async.RunSynchronously

    // Upload
    let distribution = AddDistribution(
        Name = proj.Name,
        Version = proj.Version,
        BuildType = ToolBuildType.WindowsPhotino,
        PublishMethod = ToolPublishMethod.SelfContained,
        ZipFileContents = File.ReadAllBytes(outputZipFilePath),
        UpdateVersion = true,
        UpdateRequiredVersion = false)
    let jsonContent = JsonContent.Create(distribution)
    let mutable uploadRequest = new HttpRequestMessage(
        HttpMethod.Post,
        "https://devildaggers.info/api/admin/tools",
        Content = jsonContent)
    uploadRequest.Headers.Authorization <- AuthenticationHeaderValue("Bearer", loginResponse.Token)

    let uploadAsync = async {
        let! response = client.SendAsync(uploadRequest) |> Async.AwaitTask
        return response
    }
    let uploadResponse = uploadAsync |> Async.RunSynchronously

    match uploadResponse.StatusCode with
    | HttpStatusCode.OK -> ()
    | _ -> failwith $"Unsuccessful status code from upload '{uploadResponse.StatusCode}'"

    printfn $"Deleting zip file '{outputZipFilePath}'"
    File.Delete(outputZipFilePath)

    0