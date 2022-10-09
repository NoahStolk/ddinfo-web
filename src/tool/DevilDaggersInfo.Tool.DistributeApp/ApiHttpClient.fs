module DevilDaggersInfo.Tool.DistributeApp.ApiHttpClient

open System.IO
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Net.Http.Json
open System.Text.Json
open DevilDaggersInfo.Api.Admin.Tools
open DevilDaggersInfo.Api.Main.Authentication

let login =
    let client = new HttpClient()
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
    loginResponse

let upload projectName projectVersion toolBuildType toolPublishMethod outputZipFilePath loginToken =
    let distribution = AddDistribution(
        Name = projectName,
        Version = projectVersion,
        BuildType = toolBuildType,
        PublishMethod = toolPublishMethod,
        ZipFileContents = File.ReadAllBytes(outputZipFilePath),
        UpdateVersion = true,
        UpdateRequiredVersion = false)
    let jsonContent = JsonContent.Create(distribution)
    let mutable uploadRequest = new HttpRequestMessage(
        HttpMethod.Post,
        "https://devildaggers.info/api/admin/tools",
        Content = jsonContent)
    uploadRequest.Headers.Authorization <- AuthenticationHeaderValue("Bearer", loginToken)

    let client = new HttpClient()
    let uploadAsync = async {
        let! response = client.SendAsync(uploadRequest) |> Async.AwaitTask
        return response
    }
    let uploadResponse = uploadAsync |> Async.RunSynchronously

    match uploadResponse.StatusCode with
    | HttpStatusCode.OK -> ()
    | _ -> failwith $"Unsuccessful status code from upload '{uploadResponse.StatusCode}'"
