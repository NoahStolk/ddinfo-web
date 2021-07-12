using DevilDaggersWebsite.BlazorWasm.Client.HttpClients;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Client
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddHttpClient<PublicApiHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
			builder.Services.AddHttpClient<AdminApiHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

			builder.Services.AddApiAuthorization();

			await builder.Build().RunAsync();
		}
	}
}
