using DevilDaggersInfo.Tool.GenerateClient.Generators;

new HttpClientSourceGenerator("Main", "MainApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "MainApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Admin", "AdminApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "AdminApiHttpClientGenerated.cs")).Execute();

new HttpClientSourceGenerator("Ddcl", "DdclApiHttpClient", Path.Combine(Constants.CoreClProjectPath, "HttpClients", "DdclApiHttpClientGenerated.cs")).Execute();
