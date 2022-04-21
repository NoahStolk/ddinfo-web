using DevilDaggersInfo.Native.Editor.Asset.Wpf.Services;
using DevilDaggersInfo.Web.BlazorWasm.Client.Editor.Asset.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DevilDaggersInfo.Native.Editor.Asset.Wpf;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		ServiceCollection serviceCollection = new();
		serviceCollection.AddWpfBlazorWebView();
		serviceCollection.AddSingleton<IErrorReporter, ErrorReporter>();
		serviceCollection.AddSingleton<IFileSystemService, FileSystemService>();
		Resources.Add("services", serviceCollection.BuildServiceProvider());

		InitializeComponent();
	}
}
