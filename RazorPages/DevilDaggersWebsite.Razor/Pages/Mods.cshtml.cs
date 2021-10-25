using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Razor.Pagination;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class ModsModel : PageModel, IPaginationModel
	{
		private const string _ascendingSuffix = "_asc";

		public const string NameDesc = nameof(Name);
		public const string AuthorDesc = nameof(Author);
		public const string LastUpdatedDesc = nameof(LastUpdated);
		public const string TypeDesc = nameof(Type);
		public const string HostedDesc = nameof(Hosted);
		public const string ProhibitedDesc = nameof(Prohibited);
		public const string NameAsc = nameof(Name) + _ascendingSuffix;
		public const string AuthorAsc = nameof(Author) + _ascendingSuffix;
		public const string LastUpdatedAsc = nameof(LastUpdated) + _ascendingSuffix;
		public const string TypeAsc = nameof(Type) + _ascendingSuffix;
		public const string HostedAsc = nameof(Hosted) + _ascendingSuffix;
		public const string ProhibitedAsc = nameof(Prohibited) + _ascendingSuffix;

		private readonly ModHelper _modHelper;

		public ModsModel(ModHelper modHelper)
		{
			_modHelper = modHelper;
		}

		public PaginatedList<Mod> PaginatedList { get; private set; } = null!;

		public string? SearchAuthor { get; set; }
		public string? SearchName { get; set; }

		public string? Name { get; set; }
		public string? Author { get; set; }
		public string? LastUpdated { get; set; }
		public string? Type { get; set; }
		public string? Hosted { get; set; }
		public string? Prohibited { get; set; }

		public string? SortOrder { get; set; }

		public int PageSize { get; set; } = 20;
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }
		public int TotalResults { get; private set; }

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			Name = sortOrder == NameAsc ? NameDesc : NameAsc;
			Author = sortOrder == AuthorAsc ? AuthorDesc : AuthorAsc;
			LastUpdated = sortOrder == LastUpdatedAsc ? LastUpdatedDesc : LastUpdatedAsc;
			Type = sortOrder == TypeAsc ? TypeDesc : TypeAsc;
			Hosted = sortOrder == HostedDesc ? HostedAsc : HostedDesc;
			Prohibited = sortOrder == ProhibitedDesc ? ProhibitedAsc : ProhibitedDesc;

			List<Mod> mods = _modHelper.GetMods(searchAuthor, searchName);
			mods = (sortOrder switch
			{
				NameAsc => mods.OrderBy(m => m.Name).ThenByDescending(m => m.Authors[0]),
				NameDesc => mods.OrderByDescending(m => m.Name).ThenBy(m => m.Authors[0]),
				AuthorAsc => mods.OrderBy(m => m.Authors[0]).ThenByDescending(m => m.Name),
				AuthorDesc => mods.OrderByDescending(m => m.Authors[0]).ThenBy(m => m.Name),
				LastUpdatedAsc => mods.OrderBy(s => s.LastUpdated).ThenByDescending(s => s.Name),
				TypeAsc => mods.OrderBy(m => m.AssetModTypes).ThenByDescending(m => m.Name),
				TypeDesc => mods.OrderByDescending(m => m.AssetModTypes).ThenBy(m => m.Name),
				HostedAsc => mods.OrderBy(m => m.IsHostedOnDdInfo).ThenByDescending(m => m.Name),
				HostedDesc => mods.OrderByDescending(m => m.IsHostedOnDdInfo).ThenBy(m => m.Name),
				ProhibitedAsc => mods.OrderBy(m => m.ContainsProhibitedAssets).ThenByDescending(m => m.Name),
				ProhibitedDesc => mods.OrderByDescending(m => m.ContainsProhibitedAssets).ThenBy(m => m.Name),
				_ => mods.OrderByDescending(s => s.LastUpdated).ThenBy(s => s.Name),
			}).ToList();
			TotalResults = mods.Count;
			TotalPages = Math.Max(1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PageIndex = Math.Clamp(PageIndex, 1, TotalPages);
			PaginatedList = new(mods, PageIndex, PageSize);
		}
	}
}
