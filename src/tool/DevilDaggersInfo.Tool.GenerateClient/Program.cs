using DevilDaggersInfo.Tool.GenerateClient.Generators;

new HttpClientSourceGenerator("Admin", "AdminApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "AdminApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Public", "PublicApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "PublicApiHttpClientGenerated.cs")).Execute();
