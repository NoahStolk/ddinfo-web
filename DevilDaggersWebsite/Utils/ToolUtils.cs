using DevilDaggersWebsite.Models;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Utils
{
	public static class ToolUtils
	{
		public static List<Tool> Tools { get; set; } = new List<Tool>
		{
			new Tool
			{
				Name = "DevilDaggersSurvivalEditor",
				DisplayName = "Devil Daggers Survival Editor",
				Description = new HtmlString(@"Devil Daggers Survival Editor is a tool that lets you create, view, and edit spawnsets.<br /><br />

				Main features:<br />
				<ul>
					<li>Spawnset editor</li>
					<li>Arena editor with the ability to change tile heights</li>
					<li>Arena presets</li>
					<li>Shrink preview that shows a rough approximation of what the arena will be like at what time during the spawnset</li>
					<li>Easily replacing the currently active spawnset in the game</li>
					<li>Downloading and importing spawnsets directly from the website</li>
				</ul>"),
				Link = "https://bitbucket.org/NoahStolk/devildaggerssurvivaleditor/src",
				LinkText = "View source code and documentation",
				VersionNumber = "1.1.5.0"
			}
		};

		public static List<ExternalTool> ExternalTools { get; set; } = new List<ExternalTool>
		{
			new ExternalTool
			{
				Name = "ddstats",
				Author = "VHS",
				Link = "https://ddstats.com/"
			},
			new ExternalTool
			{
				Name = "Devil Daggers Asset Editor",
				Author = "Sojk",
				Link = "https://github.com/sojk/DevilDaggersAssetEditor"
			},
			new ExternalTool
			{
				Name = "Devil Daggers Extractor",
				Author = "pmcc",
				Link = "https://github.com/pmcc/devil-daggers-extractor"
			},
			new ExternalTool
			{
				Name = "Devil Daggers Spawnset Creator",
				Author = "Sojk",
				Link = "https://github.com/sojk/DevilDaggersSpawnsetCreator"
			},
			new ExternalTool
			{
				Name = "Devil Daggers Spawnset Editor",
				Author = "bowsr",
				Link = "https://github.com/bowsr/DDSE"
			},
			new ExternalTool
			{
				Name = "Devil Daggers Spawnset Selector",
				Author = "VHS",
				Link = "https://github.com/alexwilkerson/ddss"
			}
		};
	}
}