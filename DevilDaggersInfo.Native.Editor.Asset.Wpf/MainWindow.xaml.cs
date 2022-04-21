using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DevilDaggersInfo.Native.Editor.Asset.Wpf;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		ServiceCollection serviceCollection = new();
		serviceCollection.AddWpfBlazorWebView();
		Resources.Add("services", serviceCollection.BuildServiceProvider());

		InitializeComponent();
	}
}
