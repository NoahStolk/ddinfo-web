using CoreBase;
using CoreBase.Services;
using DevilDaggersWebsite.Models.Spawnset;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
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
		public string LastUpdated { get; set; }
		public string NonLoopLength { get; set; }
		public string NonLoopSpawns { get; set; }
		public string LoopLength { get; set; }
		public string LoopSpawns { get; set; }

		public string SortOrder { get; set; }

		public int PageSize { get; set; } = 15;
		public int PageIndex { get; private set; }
		public int TotalResults { get; private set; }

		public SpawnsetsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet(string sortOrder, int? pageIndex)
		{
			SortOrder = sortOrder;

			NameSort = sortOrder == "Name" ? "Name_asc" : "Name";
			AuthorSort = sortOrder == "Author_asc" ? "Author" : "Author_asc";
			LastUpdated = sortOrder == "LastUpdated_asc" ? "LastUpdated" : "LastUpdated_asc";
			NonLoopLength = sortOrder == "NonLoopLength_asc" ? "NonLoopLength" : "NonLoopLength_asc";
			NonLoopSpawns = sortOrder == "NonLoopSpawns_asc" ? "NonLoopSpawns" : "NonLoopSpawns_asc";
			LoopLength = sortOrder == "LoopLength_asc" ? "LoopLength" : "LoopLength_asc";
			LoopSpawns = sortOrder == "LoopSpawns_asc" ? "LoopSpawns" : "LoopSpawns_asc";

			List<string> spawnsetPaths = Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets")).ToList();

			List<SpawnsetFile> spawnsetFiles = new List<SpawnsetFile>();
			foreach (string spawnsetPath in spawnsetPaths)
				spawnsetFiles.Add(new SpawnsetFile(spawnsetPath));

			switch (sortOrder)
			{
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
				case "LastUpdated_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.LastUpdated).ThenByDescending(s => s.Name).ToList();
					break;
				default:
				case "LastUpdated":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.LastUpdated).ThenBy(s => s.Name).ToList();
					break;
				case "NonLoopLength_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.SpawnData.NonLoopSeconds).ThenBy(s => s.SpawnData.NonLoopSpawns).ToList();
					break;
				case "NonLoopLength":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.SpawnData.NonLoopSeconds).ThenByDescending(s => s.SpawnData.NonLoopSpawns).ToList();
					break;
				case "NonLoopSpawns_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.SpawnData.NonLoopSpawns).ToList();
					break;
				case "NonLoopSpawns":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.SpawnData.NonLoopSpawns).ToList();
					break;
				case "LoopLength_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.SpawnData.LoopStart).ThenBy(s => s.SpawnData.LoopSpawns).ToList();
					break;
				case "LoopLength":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.SpawnData.LoopStart).ThenByDescending(s => s.SpawnData.LoopSpawns).ToList();
					break;
				case "LoopSpawns_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.SpawnData.LoopSpawns).ToList();
					break;
				case "LoopSpawns":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.SpawnData.LoopSpawns).ToList();
					break;
			}

			PageIndex = pageIndex ?? 1;
			PageIndex = Math.Max(PageIndex, 1);
			PageIndex = Math.Min(PageIndex, (int)Math.Ceiling(spawnsetFiles.Count / (double)PageSize));

			TotalResults = spawnsetFiles.Count;
			SpawnsetFiles = PaginatedList<SpawnsetFile>.Create(spawnsetFiles, PageIndex, PageSize);
		}
	}
}