using Blazorise;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace DevilDaggersInfo.Web.BlazorWasm.Client;

public static class Program
{
	public static string? Version { get; private set; }

	public static async Task Main(string[] args)
	{
		Version = Assembly.GetExecutingAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

		WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
		builder.RootComponents.Add<App>("#app");

		builder.Services.AddHttpClient<PublicApiHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
		builder.Services.AddHttpClient<AdminApiHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

		builder.Services.AddFluxor(options => options.ScanAssemblies(typeof(Program).Assembly));

		builder.Services
			.AddBlazorise(options => options.ChangeTextOnKeyPress = true)
			.AddEmptyProviders();

		await builder.Build().RunAsync();
	}
}
