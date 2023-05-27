using DevilDaggersInfo.DevUtil.GenerateClient.Generators;

new HttpClientSourceGenerator("Main", "DevilDaggersInfo.Web.Client.HttpClients", "MainApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "MainApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Admin", "DevilDaggersInfo.Web.Client.HttpClients", "AdminApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "AdminApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("App", "DevilDaggersInfo.App.Networking", "AppApiHttpClient", Path.Combine(Constants.ToolsProjectPath, "Networking", "AppApiHttpClientGenerated.cs")).Execute();
