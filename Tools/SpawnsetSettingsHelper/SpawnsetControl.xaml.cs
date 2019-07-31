using DevilDaggersCore.Spawnsets.Web;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SpawnsetSettingsHelper
{
	public partial class SpawnsetControl : UserControl
	{
		public KeyValuePair<string, SpawnsetFileSettings> Data { get; }

		public SpawnsetControl(KeyValuePair<string, SpawnsetFileSettings> data)
		{
			InitializeComponent();

			Data = data;

			Main.DataContext = Data;
		}
	}
}