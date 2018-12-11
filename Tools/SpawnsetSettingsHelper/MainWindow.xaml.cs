using Microsoft.Win32;
using NetBase.Utils;
using System.Windows;

namespace DescriptionHelper
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			string description = $"{{\n\t\"Description\": \"{HTMLUtils.ConvertToHTML(Description.Text)}\"\n}}";

			SaveFileDialog dialog = new SaveFileDialog
			{
				Filter = "JSON|*.json",
				Title = "Save the spawnset description file",
				InitialDirectory = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\spawnsets\Settings"
			};
			dialog.ShowDialog();
			if (!string.IsNullOrEmpty(dialog.FileName))
			{
				FileUtils.CreateText(dialog.FileName, description);
			}
		}
	}
}