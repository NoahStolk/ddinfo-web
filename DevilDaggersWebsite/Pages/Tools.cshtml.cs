using DevilDaggersWebsite.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Pages
{
	public class ToolsModel : PageModel
	{
		public List<Tool> Tools { get; set; }

		public void OnGet()
		{
			Tools = new List<Tool>
			{
				new Tool
				{
					Name = "DevilDaggersSurvivalEditor",
					DisplayName = "Devil Daggers Survival Editor",
					Description = new HtmlString(@"Devil Daggers Survival Editor is a tool that lets you create, view, and edit spawnsets.<br><br>

					Main features:<br>
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
					VersionNumber = "1.1.4.0"
				}
			};
		}
	}
}