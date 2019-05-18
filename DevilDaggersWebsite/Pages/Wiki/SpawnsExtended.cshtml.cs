using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Spawnsets;
using Microsoft.AspNetCore.Hosting;
using NetBase.Utils;
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
			this.squidGushCount = MathUtils.Clamp(squidGushCount, 1, 25);
			this.leviathanBeckonCount = MathUtils.Clamp(leviathanBeckonCount, 0, 25);

			SpawnEventSettings = new SpawnEventSettings
			{
				SpawnsetPath = Path.Combine(_env.WebRootPath, "spawnsets", $"{this.gameVersion}_Sorath"),
				SquidGushCount = this.squidGushCount,
				LeviathanBeckonCount = this.leviathanBeckonCount
			};
		}
	}
}