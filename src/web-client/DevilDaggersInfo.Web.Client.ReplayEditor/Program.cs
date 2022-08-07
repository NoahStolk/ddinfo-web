using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Core.NativeInterface.Services;
using DevilDaggersInfo.App.Core.NativeInterface.Services.Windows;
using DevilDaggersInfo.Razor.ReplayEditor.Services;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<DevilDaggersInfo.Razor.ReplayEditor.App>("app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// TODO: Register dummy web services instead of Windows stuff.
builder.Services.AddSingleton<INativeErrorReporter, WindowsErrorReporter>();
builder.Services.AddSingleton<INativeFileSystemService, WindowsFileSystemService>();
builder.Services.AddSingleton<INativeMemoryService, WindowsMemoryService>();
builder.Services.AddSingleton<GameMemoryReaderService>();

builder.Services.AddSingleton<NetworkService>();
builder.Services.AddScoped<StateFacade>();

builder.Services.AddFluxor(options => options.ScanAssemblies(typeof(Program).Assembly, typeof(DevilDaggersInfo.Razor.ReplayEditor.App).Assembly));

await builder.Build().RunAsync();
