using DevilDaggersCore.Spawnsets.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace SpawnsetSettingsHelper
{
	public partial class MainWindow : Window
	{
		private const string SettingsFilePath = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\spawnsets\Settings\Settings.json";

		private Dictionary<string, SpawnsetFileSettings> dict;

		public MainWindow()
		{
			InitializeComponent();

			CultureInfo culture = new CultureInfo("en-GB");
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

			LoadSettingsFile();
		}

		private void LoadSettingsFile()
		{
			using (StreamReader sr = new StreamReader(SettingsFilePath))
			{
				dict = JsonConvert.DeserializeObject<Dictionary<string, SpawnsetFileSettings>>(sr.ReadToEnd());

				foreach (KeyValuePair<string, SpawnsetFileSettings> kvp in dict)
					Spawnsets.Children.Add(new SpawnsetControl(kvp));
			}
		}

		private void SaveSettingsFile()
		{
			JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate });
			using (StreamWriter sw = new StreamWriter(File.Create(SettingsFilePath)))
			using (JsonTextWriter jtw = new JsonTextWriter(sw) { Formatting = Formatting.Indented, IndentChar = '\t', Indentation = 1 })
				serializer.Serialize(jtw, dict);
		}

		private void Reload_Click(object sender, RoutedEventArgs e)
		{
			LoadSettingsFile();
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			SaveSettingsFile();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (Keyboard.Modifiers == ModifierKeys.Control)
			{
				switch (e.Key)
				{
					case Key.S:
						SaveSettingsFile();
						break;
					case Key.R:
						LoadSettingsFile();
						break;
				}
			}
		}
	}
}