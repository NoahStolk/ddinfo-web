using Blazored.LocalStorage;
using DevilDaggersInfo.Web.Client.Authentication;
using DevilDaggersInfo.Web.Client.Core.Canvas;
using DevilDaggersInfo.Web.Client.Core.CanvasArena;
using DevilDaggersInfo.Web.Client.HttpClients;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;

namespace DevilDaggersInfo.Web.Client;

public static class Program
{
	public static string? Version { get; private set; }
	public static string? VersionHash { get; private set; }
	public static string? BuildTime { get; private set; }

	public static async Task Main(string[] args)
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		BuildTime = executingAssembly.GetCustomAttribute<BuildTimeAttribute>()?.BuildTime;

		string? informationalVersion = executingAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
		string[]? split = informationalVersion?.Split('+');
		if (split is not { Length: 2 })
		{
			Version = informationalVersion;
		}
		else
		{
			Version = split[0];
			VersionHash = split[1];
		}

		WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

#if DEBUG
		builder.Logging.SetMinimumLevel(LogLevel.Information);
#else
		builder.Logging.SetMinimumLevel(LogLevel.Warning);
#endif

		builder.RootComponents.Add<App>("#app");
		builder.RootComponents.Add<HeadOutlet>("head::after");

		builder.Services.AddHttpClient<MainApiHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
		builder.Services.AddHttpClient<AdminApiHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

		builder.Services.AddBlazoredLocalStorage();

		builder.Services.AddAuthorizationCore();
		builder.Services.AddScoped<AdminAuthenticationStateProvider>();
		builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AdminAuthenticationStateProvider>());

		await JSHost.ImportAsync(nameof(WebAssemblyCanvas2d), "../_content/DevilDaggersInfo.Web.Client.Core.Canvas/js/webAssemblyCanvas2d.js");
		await JSHost.ImportAsync(nameof(WebAssemblyCanvasArena), "../_content/DevilDaggersInfo.Web.Client.Core.CanvasArena/js/webAssemblyArena.js");

		await builder.Build().RunAsync();
	}
}
