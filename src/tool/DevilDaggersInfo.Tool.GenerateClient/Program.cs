using DevilDaggersInfo.Tool.GenerateClient.Generators;

new HttpClientSourceGenerator("Main", "DevilDaggersInfo.Web.Client.HttpClients", "MainApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "MainApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Admin", "DevilDaggersInfo.Web.Client.HttpClients", "AdminApiHttpClient", Path.Combine(Constants.ClientProjectPath, "HttpClients", "AdminApiHttpClientGenerated.cs")).Execute();

new HttpClientSourceGenerator("Ddiam", "DevilDaggersInfo.Razor.AppManager.HttpClients", "DdiamApiHttpClient", Path.Combine(Constants.RazorIamProjectPath, "HttpClients", "DdiamApiHttpClientGenerated.cs")).Execute();
new HttpClientSourceGenerator("Ddcl", "DevilDaggersInfo.Razor.CustomLeaderboard.HttpClients", "DdclApiHttpClient", Path.Combine(Constants.RazorClProjectPath, "HttpClients", "DdclApiHttpClientGenerated.cs")).Execute();
