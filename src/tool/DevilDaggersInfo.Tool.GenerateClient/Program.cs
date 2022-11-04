using DevilDaggersInfo.Tool.GenerateClient.Generators;

new HttpClientSourceGenerator("Main", "DevilDaggersInfo.Web.Client.HttpClients", "MainApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "MainApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Admin", "DevilDaggersInfo.Web.Client.HttpClients", "AdminApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "AdminApiHttpClientGenerated.cs")).Execute();

// TODO: Tools Swagger spec.
new HttpClientSourceGenerator("Main", "DevilDaggersInfo.App.Core.ApiClient.ApiClients", "ApiHttpClient", Path.Combine(Constants.ToolsProjectPath, "ApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Ddcl", "DevilDaggersInfo.App.Core.ApiClient.ApiClients", "DdclApiHttpClient", Path.Combine(Constants.ToolsProjectPath, "DdclApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Ddse", "DevilDaggersInfo.App.Core.ApiClient.ApiClients", "DdseApiHttpClient", Path.Combine(Constants.ToolsProjectPath, "DdseApiHttpClientGenerated.cs")).Execute();
