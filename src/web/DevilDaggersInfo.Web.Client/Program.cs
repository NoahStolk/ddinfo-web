using Blazored.LocalStorage;
using DevilDaggersInfo.Razor.Core.Canvas.JSRuntime;
using DevilDaggersInfo.Web.Client.Authentication;
using DevilDaggersInfo.Web.Client.HttpClients;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.Client;

public static class Program
{
	public static string? Version { get; private set; }

	public static async Task Main(string[] args)
	{
		Version = Assembly.GetExecutingAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

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

		builder.Services.AddSingleton<IJSUnmarshalledRuntime>(serviceProvider => (IJSUnmarshalledRuntime)serviceProvider.GetRequiredService<IJSRuntime>());

		builder.Services.AddBlazoredLocalStorage();

		builder.Services.AddAuthorizationCore();
		builder.Services.AddScoped<AdminAuthenticationStateProvider>();
		builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AdminAuthenticationStateProvider>());

		await builder.Build().RunAsync();
	}
}
