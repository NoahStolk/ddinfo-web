using DevilDaggersWebsite.Caches;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Razor.Pagination;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Io = System.IO;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class ModsModel : PageModel, IPaginationModel
	{
		private const string _ascendingSuffix = "_asc";

		public const string NameDesc = nameof(Name);
		public const string AuthorDesc = nameof(Author);
		public const string TypeDesc = nameof(Type);
		public const string HostedDesc = nameof(Hosted);
		public const string ProhibitedDesc = nameof(Prohibited);
		public const string NameAsc = nameof(Name) + _ascendingSuffix;
		public const string AuthorAsc = nameof(Author) + _ascendingSuffix;
		public const string TypeAsc = nameof(Type) + _ascendingSuffix;
		public const string HostedAsc = nameof(Hosted) + _ascendingSuffix;
		public const string ProhibitedAsc = nameof(Prohibited) + _ascendingSuffix;

		private readonly ApplicationDbContext _dbContext;
		private readonly IWebHostEnvironment _env;

		public ModsModel(ApplicationDbContext dbContext, IWebHostEnvironment env)
		{
			_dbContext = dbContext;
			_env = env;
		}

		public PaginatedList<Mod> PaginatedList { get; private set; } = null!;

		public string? SearchAuthor { get; set; }
		public string? SearchName { get; set; }

		public string? Name { get; set; }
		public string? Author { get; set; }
		public string? Type { get; set; }
		public string? Hosted { get; set; }
		public string? Prohibited { get; set; }

		public string? SortOrder { get; set; }

		public int PageSize { get; set; } = 20;
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }
		public int TotalResults { get; private set; }

		public string PageName => "Mods";

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			Name = sortOrder == NameDesc ? NameAsc : NameDesc;
			Author = sortOrder == AuthorAsc ? AuthorDesc : AuthorAsc;
			Type = sortOrder == TypeAsc ? TypeDesc : TypeAsc;
			Hosted = sortOrder == HostedAsc ? HostedDesc : HostedAsc;
			Prohibited = sortOrder == ProhibitedAsc ? ProhibitedDesc : ProhibitedAsc;

			List<Mod> mods = new();
			foreach (AssetMod assetMod in _dbContext.AssetMods.Include(am => am.PlayerAssetMods).ThenInclude(pam => pam.Player).Where(am => !am.IsHidden))
			{
				string filePath = Path.Combine(_env.WebRootPath, "mods", $"{assetMod.Name}.zip");
				bool isHostedOnDdInfo = Io.File.Exists(filePath);
				bool? containsAnyProhibitedAssets = null;
				if (isHostedOnDdInfo)
					containsAnyProhibitedAssets = ModDataCache.Instance.GetModDataByFilePath(filePath).Any(md => md.ModAssetData.Any(mad => mad.IsProhibited));
				mods.Add(new(assetMod.AssetModTypes, assetMod.Name, assetMod.PlayerAssetMods.Select(pam => pam.Player.PlayerName).OrderBy(s => s).ToList(), isHostedOnDdInfo, containsAnyProhibitedAssets));
			}

			if (!string.IsNullOrWhiteSpace(SearchAuthor))
				mods = mods.Where(am => am.Authors.Any(a => a.Contains(SearchAuthor, StringComparison.InvariantCultureIgnoreCase))).ToList();

			if (!string.IsNullOrWhiteSpace(SearchName))
				mods = mods.Where(am => am.Name.Contains(SearchName, StringComparison.InvariantCultureIgnoreCase)).ToList();

			mods = sortOrder switch
			{
				NameDesc => mods.OrderByDescending(m => m.Name).ThenBy(m => m.Authors[0]).ToList(),
				AuthorAsc => mods.OrderBy(m => m.Authors[0]).ThenByDescending(m => m.Name).ToList(),
				AuthorDesc => mods.OrderByDescending(m => m.Authors[0]).ThenBy(m => m.Name).ToList(),
				TypeAsc => mods.OrderBy(m => m.AssetModTypes).ThenByDescending(m => m.Name).ToList(),
				TypeDesc => mods.OrderByDescending(m => m.AssetModTypes).ThenBy(m => m.Name).ToList(),
				HostedAsc => mods.OrderBy(m => m.IsHostedOnDdInfo).ThenByDescending(m => m.Name).ToList(),
				HostedDesc => mods.OrderByDescending(m => m.IsHostedOnDdInfo).ThenBy(m => m.Name).ToList(),
				ProhibitedAsc => mods.OrderBy(m => m.ContainsAnyProhibitedAssets).ThenByDescending(m => m.Name).ToList(),
				ProhibitedDesc => mods.OrderByDescending(m => m.ContainsAnyProhibitedAssets).ThenBy(m => m.Name).ToList(),
				_ => mods.OrderBy(m => m.Name).ThenByDescending(m => m.Authors[0]).ToList(),
			};
			TotalResults = mods.Count;
			TotalPages = Math.Max(1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PageIndex = Math.Clamp(PageIndex, 1, TotalPages);
			PaginatedList = new(mods, PageIndex, PageSize);
		}
	}
}
