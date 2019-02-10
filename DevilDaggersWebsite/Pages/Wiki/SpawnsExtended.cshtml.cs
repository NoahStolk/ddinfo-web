using DevilDaggersWebsite.Models.PageModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class SpawnsExtendedModel : WikiPageModel
	{
		public string SpawnsetPath { get; set; }

		private readonly IHostingEnvironment _env;

		public SpawnsExtendedModel(IHostingEnvironment env)
		{
			_env = env;
		}

		public void OnGet(string gameVersion)
		{
			GetGameVersion(gameVersion);

			SpawnsetPath = Path.Combine(_env.WebRootPath, "spawnsets", $"{this.gameVersion}_Sorath");
		}
	}
}