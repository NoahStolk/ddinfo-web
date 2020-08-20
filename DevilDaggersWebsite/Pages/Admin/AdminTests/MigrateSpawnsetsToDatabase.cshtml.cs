using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.DataTransferObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Io = System.IO;

namespace DevilDaggersWebsite.Pages.Admin.AdminTests
{
	public class MigrateSpawnsetsToDatabaseModel : PageModel
	{
		private readonly Dictionary<string, int> authorIds = new Dictionary<string, int>
		{
			{ "xvlv", 21854 },
			{ "ThePassiveDada", 21059 },
			{ "Stop", 148951 },
			{ "Stephanstein", 10098 },
			{ "Sorath", 1 },
			{ "sjorsbw", 10210 },
			{ "purpleposeidon", 194431 },
			{ "Pritster", 115431 },
			{ "pagedMov", 65617 },
			{ "Nullifier", 10782 },
			{ "lsaille", 86805 },
			{ "LoIVeR", 65193 },
			{ "eidolon", 193721 },
			{ "Cookie", 93991 },
			{ "cake", 180168 },
			{ "Brain", 0 },
			{ "Braden", 193671 },
			{ "Bintr", 148788 },
			{ "beflos", 193721 },
		};

		private readonly IWebHostEnvironment env;
		private readonly ApplicationDbContext dbContext;

		public MigrateSpawnsetsToDatabaseModel(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			this.env = env;
			this.dbContext = dbContext;
		}

		public void OnGet()
		{
			Dictionary<string, SpawnsetFileSettings> dict = JsonConvert.DeserializeObject<Dictionary<string, SpawnsetFileSettings>>(Io.File.ReadAllText(Path.Combine(env.WebRootPath, "spawnsets", "Settings", "Settings.json")));

			List<Code.Database.SpawnsetFile> spawnsetFiles = new List<Code.Database.SpawnsetFile>();
			int i = 1;
			foreach (KeyValuePair<string, SpawnsetFileSettings> kvp in dict.OrderBy(kvp => kvp.Value.LastUpdated))
			{
				string name = kvp.Key.Substring(0, kvp.Key.LastIndexOf('_'));
				string author = kvp.Key.Substring(kvp.Key.LastIndexOf('_') + 1);

				spawnsetFiles.Add(new Code.Database.SpawnsetFile
				{
					Id = i++,
					Name = name,
					PlayerId = authorIds[author],
					HtmlDescription = kvp.Value.Description,
					LastUpdated = kvp.Value.LastUpdated,
					MaxDisplayWaves = kvp.Value.MaxWaves,
				});
			}

			dbContext.SpawnsetFiles.AddRange(spawnsetFiles);
			dbContext.SaveChanges();
		}
	}
}