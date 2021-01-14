using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;

namespace SpawnsetHashes
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Open_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				InitialDirectory = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\spawnsets"
			};
			bool? result = dialog.ShowDialog();

			if (result.HasValue && result.Value)
			{
				using (MD5 md5 = MD5.Create())
				{
					using (FileStream stream = File.OpenRead(dialog.FileName))
					{
						byte[] hash = md5.ComputeHash(stream);
						Hash.Text = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
					}
				}
			}
		}
	}
}
