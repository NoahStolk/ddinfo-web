using DevilDaggersWebsite.Models.PageModels;
using DevilDaggersWebsite.Models.Spawnset;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace DevilDaggersWebsite.Pages.Wiki
{
	public class SpawnsExtendedModel : WikiPageModel
	{
		public int squidGushCount;
		public int leviathanBeckonCount;

		public SpawnEventSettings SpawnEventSettings { get; set; }

		private readonly IHostingEnvironment _env;

		public SpawnsExtendedModel(IHostingEnvironment env)
		{
			_env = env;
		}

		public void OnGet(string gameVersion, int squidGushCount, int leviathanBeckonCount)
		{
			SetGameVersion(gameVersion);
			this.squidGushCount = Math.Min(squidGushCount, 25);
			this.leviathanBeckonCount = Math.Min(leviathanBeckonCount, 25);

			SpawnEventSettings = new SpawnEventSettings
			{
				SpawnsetPath = Path.Combine(_env.WebRootPath, "spawnsets", $"{this.gameVersion}_Sorath"),
				SquidGushCount = this.squidGushCount,
				LeviathanBeckonCount = this.leviathanBeckonCount
			};
		}
	}
}