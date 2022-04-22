using DevilDaggersInfo.AssetEditor.Wpf.Services;
using DevilDaggersInfo.Razor.Core.AssetEditor.Services;
using DevilDaggersInfo.Razor.Core.AssetEditor.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DevilDaggersInfo.AssetEditor.Wpf;

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

		Title = $"Devil Daggers Asset Editor {AssemblyUtils.Version}";
	}
}
