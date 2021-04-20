using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Razor.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages.CustomLeaderboards
{
	public class IndexModel : PageModel, IPaginationModel
	{
		private const string _ascendingSuffix = "_asc";

		public const string NameDesc = nameof(Name);
		public const string AuthorDesc = nameof(Author);
		public const string CategoryDesc = nameof(Category);
		public const string LastPlayedDesc = nameof(LastPlayed);
		public const string CreatedDesc = nameof(Created);
		public const string SubmitsDesc = nameof(Submits);
		public const string PlayersDesc = nameof(Players);
		public const string BronzeDesc = nameof(Bronze);
		public const string SilverDesc = nameof(Silver);
		public const string GoldenDesc = nameof(Golden);
		public const string DevilDesc = nameof(Devil);
		public const string LeviathanDesc = nameof(Leviathan);
		public const string WorldRecordDesc = nameof(WorldRecord);
		public const string WorldRecordHolderDesc = nameof(WorldRecordHolder);
		public const string NameAsc = nameof(Name) + _ascendingSuffix;
		public const string AuthorAsc = nameof(Author) + _ascendingSuffix;
		public const string CategoryAsc = nameof(Category) + _ascendingSuffix;
		public const string LastPlayedAsc = nameof(LastPlayed) + _ascendingSuffix;
		public const string CreatedAsc = nameof(Created) + _ascendingSuffix;
		public const string SubmitsAsc = nameof(Submits) + _ascendingSuffix;
		public const string PlayersAsc = nameof(Players) + _ascendingSuffix;
		public const string BronzeAsc = nameof(Bronze) + _ascendingSuffix;
		public const string SilverAsc = nameof(Silver) + _ascendingSuffix;
		public const string GoldenAsc = nameof(Golden) + _ascendingSuffix;
		public const string DevilAsc = nameof(Devil) + _ascendingSuffix;
		public const string LeviathanAsc = nameof(Leviathan) + _ascendingSuffix;
		public const string WorldRecordAsc = nameof(WorldRecord) + _ascendingSuffix;
		public const string WorldRecordHolderAsc = nameof(WorldRecordHolder) + _ascendingSuffix;

		private readonly Entities.ApplicationDbContext _dbContext;

		public IndexModel(Entities.ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public PaginatedList<CustomLeaderboard> PaginatedList { get; private set; } = null!;

		public string? SearchAuthor { get; set; }
		public string? SearchName { get; set; }

		public string? Name { get; set; }
		public string? Author { get; set; }
		public string? Category { get; set; }
		public string? LastPlayed { get; set; }
		public string? Created { get; set; }
		public string? Submits { get; set; }
		public string? Players { get; set; }
		public string? Bronze { get; set; }
		public string? Silver { get; set; }
		public string? Golden { get; set; }
		public string? Devil { get; set; }
		public string? Leviathan { get; set; }
		public string? WorldRecord { get; set; }
		public string? WorldRecordHolder { get; set; }

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
			Category = sortOrder == CategoryAsc ? CategoryDesc : CategoryAsc;
			LastPlayed = sortOrder == LastPlayedAsc ? LastPlayedDesc : LastPlayedAsc;
			Created = sortOrder == CreatedDesc ? CreatedAsc : CreatedDesc;
			Submits = sortOrder == SubmitsDesc ? SubmitsAsc : SubmitsDesc;
			Players = sortOrder == PlayersDesc ? PlayersAsc : PlayersDesc;
			Bronze = sortOrder == BronzeDesc ? BronzeAsc : BronzeDesc;
			Silver = sortOrder == SilverDesc ? SilverAsc : SilverDesc;
			Golden = sortOrder == GoldenDesc ? GoldenAsc : GoldenDesc;
			Devil = sortOrder == DevilDesc ? DevilAsc : DevilDesc;
			Leviathan = sortOrder == LeviathanDesc ? LeviathanAsc : LeviathanDesc;
			WorldRecord = sortOrder == WorldRecordDesc ? WorldRecordAsc : WorldRecordDesc;
			WorldRecordHolder = sortOrder == WorldRecordHolderAsc ? WorldRecordHolderDesc : WorldRecordHolderAsc;

			IIncludableQueryable<Entities.CustomLeaderboard, Entities.Player> customLeaderboardQuery = _dbContext.CustomLeaderboards
				.Where(cl => cl.Category != CustomLeaderboardCategory.Challenge && !cl.IsArchived)
				.Include(cl => cl.SpawnsetFile)
					.ThenInclude(sf => sf.Player);

			List<CustomLeaderboard> customLeaderboards = new();
			foreach (Entities.CustomLeaderboard cl in customLeaderboardQuery)
			{
				if (!string.IsNullOrWhiteSpace(SearchAuthor) && !cl.SpawnsetFile.Player.PlayerName.Contains(SearchAuthor, StringComparison.InvariantCultureIgnoreCase))
					continue;

				if (!string.IsNullOrWhiteSpace(SearchName) && !cl.SpawnsetFile.Name.Contains(SearchName, StringComparison.InvariantCultureIgnoreCase))
					continue;

				List<Entities.CustomEntry> entries = _dbContext.CustomEntries
					.Include(ce => ce.Player)
					.Where(ce => ce.CustomLeaderboard == cl)
					.OrderByMember(nameof(Entities.CustomEntry.Time), cl.Category.IsAscending())
					.ThenByMember(nameof(Entities.CustomEntry.SubmitDate), true)
					.ToList();

				Entities.CustomEntry? worldRecord = entries.Count == 0 ? null : entries[0];
				string? worldRecordDaggerName = worldRecord == null ? null : cl.GetDagger(worldRecord.Time);
				customLeaderboards.Add(new(cl.Category, cl.SpawnsetFile.Name, cl.SpawnsetFile.Player.PlayerName, cl.TimeBronze, cl.TimeSilver, cl.TimeGolden, cl.TimeDevil, cl.TimeLeviathan, worldRecord, worldRecordDaggerName, cl.DateLastPlayed, cl.DateCreated, cl.TotalRunsSubmitted, entries.Count));
			}

			customLeaderboards = (sortOrder switch
			{
				NameAsc => customLeaderboards.OrderBy(cl => cl.SpawnsetName).ThenByDescending(cl => cl.AuthorName),
				NameDesc => customLeaderboards.OrderByDescending(cl => cl.SpawnsetName).ThenBy(cl => cl.AuthorName),
				AuthorAsc => customLeaderboards.OrderBy(cl => cl.AuthorName).ThenByDescending(cl => cl.SpawnsetName),
				AuthorDesc => customLeaderboards.OrderByDescending(cl => cl.AuthorName).ThenBy(cl => cl.SpawnsetName),
				CategoryAsc => customLeaderboards.OrderBy(cl => cl.Category).ThenByDescending(cl => cl.SpawnsetName),
				CategoryDesc => customLeaderboards.OrderByDescending(cl => cl.Category).ThenBy(cl => cl.SpawnsetName),
				LastPlayedAsc => customLeaderboards.OrderBy(cl => cl.DateLastPlayed).ThenByDescending(cl => cl.SpawnsetName),
				CreatedAsc => customLeaderboards.OrderBy(cl => cl.DateCreated).ThenByDescending(cl => cl.SpawnsetName),
				CreatedDesc => customLeaderboards.OrderByDescending(cl => cl.DateCreated).ThenBy(cl => cl.SpawnsetName),
				SubmitsAsc => customLeaderboards.OrderBy(cl => cl.TotalRunsSubmitted).ThenByDescending(cl => cl.SpawnsetName),
				SubmitsDesc => customLeaderboards.OrderByDescending(cl => cl.TotalRunsSubmitted).ThenBy(cl => cl.SpawnsetName),
				PlayersAsc => customLeaderboards.OrderBy(cl => cl.TotalPlayers).ThenByDescending(cl => cl.SpawnsetName),
				PlayersDesc => customLeaderboards.OrderByDescending(cl => cl.TotalPlayers).ThenBy(cl => cl.SpawnsetName),
				BronzeAsc => customLeaderboards.OrderBy(cl => cl.TimeBronze).ThenByDescending(cl => cl.SpawnsetName),
				BronzeDesc => customLeaderboards.OrderByDescending(cl => cl.TimeBronze).ThenBy(cl => cl.SpawnsetName),
				SilverAsc => customLeaderboards.OrderBy(cl => cl.TimeSilver).ThenByDescending(cl => cl.SpawnsetName),
				SilverDesc => customLeaderboards.OrderByDescending(cl => cl.TimeSilver).ThenBy(cl => cl.SpawnsetName),
				GoldenAsc => customLeaderboards.OrderBy(cl => cl.TimeGolden).ThenByDescending(cl => cl.SpawnsetName),
				GoldenDesc => customLeaderboards.OrderByDescending(cl => cl.TimeGolden).ThenBy(cl => cl.SpawnsetName),
				DevilAsc => customLeaderboards.OrderBy(cl => cl.TimeDevil).ThenByDescending(cl => cl.SpawnsetName),
				DevilDesc => customLeaderboards.OrderByDescending(cl => cl.TimeDevil).ThenBy(cl => cl.SpawnsetName),
				LeviathanAsc => customLeaderboards.OrderBy(cl => cl.TimeLeviathan).ThenByDescending(cl => cl.SpawnsetName),
				LeviathanDesc => customLeaderboards.OrderByDescending(cl => cl.TimeLeviathan).ThenBy(cl => cl.SpawnsetName),
				WorldRecordAsc => customLeaderboards.OrderBy(cl => cl.WorldRecord?.Time).ThenByDescending(cl => cl.SpawnsetName),
				WorldRecordDesc => customLeaderboards.OrderByDescending(cl => cl.WorldRecord?.Time).ThenBy(cl => cl.SpawnsetName),
				WorldRecordHolderAsc => customLeaderboards.OrderBy(cl => cl.WorldRecord?.Player.PlayerName).ThenByDescending(cl => cl.SpawnsetName),
				WorldRecordHolderDesc => customLeaderboards.OrderByDescending(cl => cl.WorldRecord?.Player.PlayerName).ThenBy(cl => cl.SpawnsetName),
				_ => customLeaderboards.OrderByDescending(cl => cl.DateLastPlayed).ThenBy(cl => cl.SpawnsetName),
			}).ToList();

			TotalResults = customLeaderboards.Count;
			TotalPages = Math.Max(1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PageIndex = Math.Clamp(PageIndex, 1, TotalPages);
			PaginatedList = new(customLeaderboards, PageIndex, PageSize);
		}
	}
}
