using DevilDaggersInfo.Tool.GenerateClient.Generators;

new HttpClientSourceGenerator("Admin", "AdminApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "AdminApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Main", "MainApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "MainApiHttpClientGenerated.cs")).Execute();
