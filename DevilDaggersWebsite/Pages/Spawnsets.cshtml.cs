using CoreBase;
using CoreBase.Services;
using DevilDaggersWebsite.Models.Spawnset;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetsModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public PaginatedList<SpawnsetFile> SpawnsetFiles;

		public string NameSort { get; set; }
		public string AuthorSort { get; set; }
		public string SortOrder { get; set; }
		public int PageSize { get; set; } = 10;
		public int PageIndex { get; private set; }

		public SpawnsetsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet(string sortOrder, int? pageIndex)
		{
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			NameSort = sortOrder == "Name" ? "Name_asc" : "Name";
			AuthorSort = sortOrder == "Author_asc" ? "Author" : "Author_asc";

			List<string> spawnsetPaths = Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets/")).ToList();

			List<SpawnsetFile> spawnsetFiles = new List<SpawnsetFile>();
			foreach (string spawnsetPath in spawnsetPaths)
			{
				string fileName = Path.GetFileName(spawnsetPath);

				string name = fileName.Substring(0, fileName.LastIndexOf('_'));
				string author = fileName.Substring(fileName.LastIndexOf('_') + 1);
				spawnsetFiles.Add(new SpawnsetFile(name, author, fileName));
			}

			switch (sortOrder)
			{
				default:
				case "Name_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.Name).ThenByDescending(s => s.Author).ToList();
					break;
				case "Name":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.Name).ThenBy(s => s.Author).ToList();
					break;
				case "Author_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.Author).ThenByDescending(s => s.Name).ToList();
					break;
				case "Author":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.Author).ThenBy(s => s.Name).ToList();
					break;
			}

			SpawnsetFiles = PaginatedList<SpawnsetFile>.Create(spawnsetFiles, PageIndex, PageSize);
		}
	}
}