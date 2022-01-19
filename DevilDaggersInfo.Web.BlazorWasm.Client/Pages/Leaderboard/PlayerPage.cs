using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;

public partial class PlayerPage
{
	private readonly LineChartOptions _progressionScoreLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Time", "Player", "Rank", "Gems", "Kills", "Accuracy", "Death type" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 30,
		},
		DisplayXScaleAsDates = true,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _progressionRankLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Rank" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 30,
		},
		DisplayXScaleAsDates = true,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _activityDeathsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Avg deaths per day" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 30,
		},
		DisplayXScaleAsDates = true,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly List<LineDataSet> _progressionScoreData = new();
	private readonly List<LineDataSet> _progressionRankData = new();
	private readonly List<LineDataSet> _activityDeathsData = new();

	private LineChartDataOptions? _progressionScoreOptions;
	private LineChartDataOptions? _progressionRankOptions;
	private LineChartDataOptions? _activityDeathsOptions;

	private int _pageRankStart;
	private int _pageRankEnd;

	[Parameter, EditorRequired] public int Id { get; set; }

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	public GetEntry? GetEntry { get; set; }
	public GetPlayer? GetPlayer { get; set; }
	public GetPlayerHistory? GetPlayerHistory { get; set; }
	public List<GetCustomLeaderboardStatisticsForPlayer>? GetCustomLeaderboardStatistics { get; set; }
	public List<GetSpawnsetName>? GetSpawnsetNames { get; set; }
	public List<GetModName>? GetModNames { get; set; }
	public GetNumberOfCustomLeaderboards? GetNumberOfCustomLeaderboards { get; set; }

	public bool PlayerNotFound { get; set; }

	protected override async Task OnInitializedAsync()
	{
		GetEntry = await Http.GetEntryById(Id);

		_pageRankStart = (GetEntry.Rank - 1) / 100 * 100 + 1;
		_pageRankEnd = _pageRankStart + 99;

		try
		{
			GetPlayer = await Http.GetPlayerById(Id);
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			PlayerNotFound = true;
		}

		GetPlayerHistory = await Http.GetPlayerHistoryById(Id);
		GetCustomLeaderboardStatistics = await Http.GetCustomLeaderboardStatisticsByPlayerId(Id);
		GetSpawnsetNames = await Http.GetSpawnsetsByAuthorId(Id);
		GetModNames = await Http.GetModsByAuthorId(Id);
		GetNumberOfCustomLeaderboards = await Http.GetNumberOfCustomLeaderboards();

		if (GetPlayerHistory.ScoreHistory.Count > 0)
		{
			DateTime minX = GetPlayerHistory.ScoreHistory.Select(sh => sh.DateTime).Min();
			DateTime maxX = DateTime.UtcNow;

			IEnumerable<double> scores = GetPlayerHistory.ScoreHistory.Select(sh => sh.Time);
			const double scale = 50.0;
			double minY = Math.Floor(scores.Min() / scale) * scale;
			double maxY = Math.Ceiling(scores.Max() / scale) * scale;

			List<LineData> set = GetPlayerHistory.ScoreHistory.Select(sh => new LineData(sh.DateTime.Ticks, sh.Time, sh)).ToList();
			_progressionScoreOptions = new(minX.Ticks, null, maxX.Ticks, minY, scale, maxY);
			_progressionScoreData.Add(new("#f00", true, true, true, set, (ds, d) =>
			{
				GetPlayerHistoryScoreEntry? scoreEntry = GetPlayerHistory.ScoreHistory.Find(sh => sh == d.Reference);
				if (scoreEntry == null)
					return new();

				GameVersion gameVersion = GameVersions.GetGameVersionFromDate(scoreEntry.DateTime) ?? GameVersion.V1_0;
				Dagger dagger = Daggers.GetDaggerFromSeconds(gameVersion, scoreEntry.Time);
				return new()
				{
					new($"<span style='text-align: right;'>{scoreEntry.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='text-align: right;' class='{dagger.Name.ToLower()}'>{scoreEntry.Time.ToString(FormatUtils.TimeFormat)}</span>"),
					new($"<span style='text-align: right;' class='{dagger.Name.ToLower()}'>{scoreEntry.Username}</span>"),
					new($"<span style='text-align: right;'>{scoreEntry.Rank}</span>"),
					new($"<span style='text-align: right;'>{scoreEntry.Gems}</span>"),
					new($"<span style='text-align: right;'>{scoreEntry.Kills}</span>"),
					new($"<span style='text-align: right;'>{(scoreEntry.DaggersFired == 0 ? 0 : scoreEntry.DaggersHit / (double)scoreEntry.DaggersFired).ToString(FormatUtils.AccuracyFormat)}</span>"),
					new($"<span style='text-align: right;'>{MarkupUtils.DeathString(scoreEntry.DeathType, gameVersion)}</span>"),
				};
			}));
		}

		if (GetPlayerHistory.RankHistory.Count > 0)
		{
			DateTime minX = GetPlayerHistory.RankHistory.Select(rh => rh.DateTime).Min();
			DateTime maxX = DateTime.UtcNow;

			IEnumerable<int> rank = GetPlayerHistory.RankHistory.Select(rh => rh.Rank);
			const double scale = 10.0;
			double maxY = Math.Ceiling(rank.Max() / scale) * scale;

			List<LineData> set = GetPlayerHistory.RankHistory.Select(rh => new LineData(rh.DateTime.Ticks, rh.Rank, rh)).ToList();
			_progressionRankOptions = new(minX.Ticks, null, maxX.Ticks, 0, scale, maxY, false, true);
			_progressionRankData.Add(new("#ff0", false, true, true, set, (ds, d) =>
			{
				GetPlayerHistoryRankEntry? rankEntry = GetPlayerHistory.RankHistory.Find(rh => rh == d.Reference);
				return rankEntry == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{rankEntry.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='text-align: right;'>{rankEntry.Rank}</span>"),
				};
			}));
		}

		if (GetPlayerHistory.ActivityHistory.Count > 0)
		{
			DateTime minX = GetPlayerHistory.ActivityHistory.Select(ah => ah.DateTime).Min();
			DateTime maxX = DateTime.UtcNow;

			IEnumerable<double> deaths = GetPlayerHistory.ActivityHistory.Select(ah => ah.DeathsIncrement);
			const double scale = 20.0;
			double maxY = Math.Ceiling(deaths.Max() / scale) * scale;

			List<LineData> set = GetPlayerHistory.ActivityHistory.Select(ah => new LineData(ah.DateTime.Ticks, ah.DeathsIncrement, ah)).ToList();
			_activityDeathsOptions = new(minX.Ticks, null, maxX.Ticks, 0, scale, maxY);
			_activityDeathsData.Add(new("#0f0", false, true, true, set, (ds, d) =>
			{
				GetPlayerHistoryActivityEntry? activityEntry = GetPlayerHistory.ActivityHistory.Find(ah => ah == d.Reference);
				return activityEntry == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{activityEntry.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{activityEntry.DeathsIncrement.ToString("0.0")}</span>"),
				};
			}));
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
