using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.Networking;
using DevilDaggersInfo.App.Ui.Base.Networking.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Data;
using DevilDaggersInfo.App.Ui.Base.User.Cache;
using ImGuiNET;
using System.Diagnostics;
using System.Numerics;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards.LeaderboardList;

public static class LeaderboardListChild
{
	private const int _pageSize = 20;

	private static readonly string[] _categoryNames = Enum.GetValues<CustomLeaderboardCategory>().Select(et => et.ToString()).ToArray();

	private static int _categoryIndex;
	private static bool _featuredOnly;
	private static string _spawnsetFilter = string.Empty;
	private static string _authorFilter = string.Empty;
	private static LeaderboardListSorting _sorting;

	private static readonly List<GetCustomLeaderboardForOverview> _customLeaderboards = new();
	private static readonly Dictionary<LeaderboardListSorting, bool> _sortingDirections = Enum.GetValues<LeaderboardListSorting>().ToDictionary(s => s, _ => true);

	private static CustomLeaderboardCategory Category => Enum.Parse<CustomLeaderboardCategory>(_categoryNames[_categoryIndex]);

	public static bool IsLoading { get; private set; }
	public static int PageIndex { get; private set; }
	public static int TotalPages { get; private set; }
	public static List<GetCustomLeaderboardForOverview> PagedCustomLeaderboards { get; private set; } = new();

	public static void Render()
	{
		Vector2 iconSize = new(16);

		ImGui.BeginChild("LeaderboardList");

		if (ImGui.ImageButton((IntPtr)Root.InternalResources.ReloadTexture.Handle, iconSize))
			LoadAll();

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.ArrowStartTexture.Handle, iconSize))
			SetPageIndex(0);

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.ArrowLeftTexture.Handle, iconSize))
			SetPageIndex(PageIndex - 1);

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.ArrowRightTexture.Handle, iconSize))
			SetPageIndex(PageIndex + 1);

		ImGui.SameLine();
		if (ImGui.ImageButton((IntPtr)Root.InternalResources.ArrowEndTexture.Handle, iconSize))
			SetPageIndex(TotalPages - 1);

		ImGui.SameLine();
		ImGui.BeginChild("ComboCategory", new(170, 20));
		ImGui.Combo("Category", ref _categoryIndex, _categoryNames, _categoryNames.Length);
		ImGui.EndChild();

		ImGui.SameLine();
		ImGui.Checkbox("Featured only", ref _featuredOnly);

		ImGui.SameLine();
		ImGui.BeginChild("InputSpawnset", new(170, 20));
		ImGui.InputText("Spawnset filter", ref _spawnsetFilter, 32);
		ImGui.EndChild();

		ImGui.SameLine();
		ImGui.BeginChild("InputAuthor", new(170, 20));
		ImGui.InputText("Author filter", ref _authorFilter, 32);
		ImGui.EndChild();

		ImGui.EndChild();
	}

	public static void LoadAll()
	{
		IsLoading = true;
		AsyncHandler.Run(
			p =>
			{
				IsLoading = false;
				_customLeaderboards.Clear();

				if (p == null)
				{
					PageIndex = 0;
				}
				else
				{
					_customLeaderboards.AddRange(p);
					TotalPages = GetTotalPages();
					PageIndex = Math.Clamp(PageIndex, 0, GetMaxPageIndex());
				}

				PagedCustomLeaderboards = GetPagedCustomLeaderboards();
			},
			() => FetchCustomLeaderboards.HandleAsync(UserCache.Model.PlayerId));
	}

	private static void SetPageIndex(int pageIndex)
	{
		PageIndex = Math.Clamp(pageIndex, 0, Math.Max(0, GetMaxPageIndex()));
		PagedCustomLeaderboards = GetPagedCustomLeaderboards();
	}

	public static int GetTotal()
	{
		return _customLeaderboards.Count(Predicate);
	}

	public static int GetTotalPages()
	{
		return (int)Math.Ceiling(GetTotal() / (float)_pageSize);
	}

	public static int GetMaxPageIndex()
	{
		return Math.Max(0, GetTotalPages() - 1);
	}

	private static List<GetCustomLeaderboardForOverview> GetPagedCustomLeaderboards()
	{
		_ = _sortingDirections.TryGetValue(_sorting, out bool isAscending);
		IEnumerable<GetCustomLeaderboardForOverview> sorted = _sorting switch
		{
			LeaderboardListSorting.Name => isAscending ? _customLeaderboards.OrderBy(cl => cl.SpawnsetName.ToLower()) : _customLeaderboards.OrderByDescending(cl => cl.SpawnsetName.ToLower()),
			LeaderboardListSorting.Author => isAscending ? _customLeaderboards.OrderBy(cl => cl.SpawnsetAuthorName.ToLower()) : _customLeaderboards.OrderByDescending(cl => cl.SpawnsetAuthorName.ToLower()),
			LeaderboardListSorting.Criteria => isAscending ? _customLeaderboards.OrderBy(cl => cl.Criteria.Count) : _customLeaderboards.OrderByDescending(cl => cl.Criteria.Count),
			LeaderboardListSorting.Score => isAscending ? _customLeaderboards.OrderBy(cl => cl.SelectedPlayerStats?.Time) : _customLeaderboards.OrderByDescending(cl => cl.SelectedPlayerStats?.Time),
			LeaderboardListSorting.NextDagger => isAscending ? _customLeaderboards.OrderBy(GetNextDaggerSortingKey) : _customLeaderboards.OrderByDescending(GetNextDaggerSortingKey),
			LeaderboardListSorting.Rank => isAscending ? _customLeaderboards.OrderBy(GetRankSortingKey) : _customLeaderboards.OrderByDescending(GetRankSortingKey),
			LeaderboardListSorting.Players => isAscending ? _customLeaderboards.OrderBy(cl => cl.PlayerCount) : _customLeaderboards.OrderByDescending(cl => cl.PlayerCount),
			LeaderboardListSorting.WorldRecord => isAscending ? _customLeaderboards.OrderBy(cl => cl.WorldRecord?.Time) : _customLeaderboards.OrderByDescending(cl => cl.WorldRecord?.Time),
			_ => throw new UnreachableException(),
		};

		return sorted
			.Where(Predicate)
			.Skip(PageIndex * _pageSize)
			.Take(_pageSize)
			.ToList();

		static int GetRankSortingKey(GetCustomLeaderboardForOverview customLeaderboard)
		{
			if (customLeaderboard.SelectedPlayerStats == null)
				return int.MaxValue;

			return customLeaderboard.SelectedPlayerStats.Rank;
		}

		static double GetNextDaggerSortingKey(GetCustomLeaderboardForOverview customLeaderboard)
		{
			if (customLeaderboard.Daggers == null || customLeaderboard.SelectedPlayerStats == null)
				return double.MinValue;

			if (customLeaderboard.SelectedPlayerStats.NextDagger == null)
				return double.MaxValue;

			return customLeaderboard.SelectedPlayerStats.NextDagger.Time;
		}
	}

	private static bool Predicate(GetCustomLeaderboardForOverview cl)
	{
		return
			cl.Category == Category &&
			(string.IsNullOrEmpty(_spawnsetFilter) || cl.SpawnsetName.Contains(_spawnsetFilter, StringComparison.OrdinalIgnoreCase)) &&
			(string.IsNullOrEmpty(_authorFilter) || cl.SpawnsetAuthorName.Contains(_authorFilter, StringComparison.OrdinalIgnoreCase)) &&
			(!_featuredOnly || cl.Daggers != null);
	}
}
