using DevilDaggersInfo.Core.Wiki.Extensions;
using DevilDaggersInfo.Razor.Core.CanvasChart.Data;
using DevilDaggersInfo.Razor.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Client.Utils;
using DevilDaggersInfo.Web.Shared.Dto.Public.Leaderboards;
using DevilDaggersInfo.Web.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.Shared.Dto.Public.Spawnsets;
using DevilDaggersInfo.Web.Shared.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public partial class PlayerPage
{
	private readonly LineChartOptions _progressionScoreLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Time", "Player", "Rank", "Gems", "Kills", "Accuracy", "Death Type", "Game Version" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 30,
		},
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _progressionRankLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Rank", "Game Version" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 30,
		},
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _activityDeathsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Avg Deaths / Day" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 30,
		},
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
		HighlighterWidth = 320,
	};

	private readonly LineChartOptions _activityTimeLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Avg Total Time / Day" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 30,
		},
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
		HighlighterWidth = 320,
	};

	private readonly List<LineDataSet> _progressionScoreData = new();
	private readonly List<LineDataSet> _progressionRankData = new();
	private readonly List<LineDataSet> _activityDeathsData = new();
	private readonly List<LineDataSet> _activityTimeData = new();

	private LineChartDataOptions _progressionScoreOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _progressionRankOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _activityDeathsOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _activityTimeOptions = LineChartDataOptions.Default;

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
	public List<GetPlayerCustomLeaderboardStatistics>? GetCustomLeaderboardStatistics { get; set; }
	public List<GetSpawnsetName>? GetSpawnsetNames { get; set; }
	public List<GetModName>? GetModNames { get; set; }

	public bool PlayerNotFound { get; set; }

	protected override async Task OnParametersSetAsync()
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

		_progressionScoreData.Clear();
		_progressionRankData.Clear();
		_activityDeathsData.Clear();
		_activityTimeData.Clear();

		if (GetPlayerHistory.ScoreHistory.Count > 0)
		{
			DateTime minX = GetPlayerHistory.ScoreHistory.Select(sh => sh.DateTime).Min();
			DateTime maxX = DateTime.UtcNow;

			IEnumerable<double> scores = GetPlayerHistory.ScoreHistory.Select(sh => sh.Time);
			const double scale = 50.0;
			double minY = Math.Floor(scores.Min() / scale) * scale;
			double maxY = Math.Ceiling(scores.Max() / scale) * scale;

			List<LineData> set = GetPlayerHistory.ScoreHistory.Select((sh, i) => new LineData(sh.DateTime.Ticks, sh.Time, i)).ToList();
			_progressionScoreOptions = new(minX.Ticks, null, maxX.Ticks, minY, scale, maxY);
			_progressionScoreData.Add(new("#f00", true, true, true, set, (ds, d) =>
			{
				GetPlayerHistoryScoreEntry? scoreEntry = GetPlayerHistory.ScoreHistory.Count <= d.Index ? null : GetPlayerHistory.ScoreHistory[d.Index];
				if (scoreEntry == null)
					return new();

				GameVersion? gameVersion = GameVersions.GetGameVersionFromDate(scoreEntry.DateTime);
				Dagger dagger = Daggers.GetDaggerFromSeconds(gameVersion ?? GameVersion.V1_0, scoreEntry.Time);
				return new()
				{
					new($"<span style='text-align: right;'>{scoreEntry.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='text-align: right;' class='{dagger.Name.ToLower()}'>{scoreEntry.Time.ToString(StringFormats.TimeFormat)}</span>"),
					new($"<span style='text-align: right;' class='{dagger.Name.ToLower()}'>{scoreEntry.Username}</span>"),
					new($"<span style='text-align: right;'>{scoreEntry.Rank}</span>"),
					new($"<span style='text-align: right;'>{scoreEntry.Gems}</span>"),
					new($"<span style='text-align: right;'>{scoreEntry.Kills}</span>"),
					new($"<span style='text-align: right;'>{(scoreEntry.DaggersFired == 0 ? 0 : scoreEntry.DaggersHit / (double)scoreEntry.DaggersFired).ToString(StringFormats.AccuracyFormat)}</span>"),
					new($"<span style='text-align: right;'>{MarkupUtils.DeathString(scoreEntry.DeathType, gameVersion ?? GameVersion.V1_0)}</span>"),
					new($"<span style='text-align: right;'>{gameVersion.GetGameVersionString()}</span>"),
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

			List<LineData> set = GetPlayerHistory.RankHistory.Select((rh, i) => new LineData(rh.DateTime.Ticks, rh.Rank, i)).ToList();
			_progressionRankOptions = new(minX.Ticks, null, maxX.Ticks, 0, scale, maxY, false, true);
			_progressionRankData.Add(new("#ff0", false, true, true, set, (ds, d) =>
			{
				GetPlayerHistoryRankEntry? rankEntry = GetPlayerHistory.RankHistory.Count <= d.Index ? null : GetPlayerHistory.RankHistory[d.Index];
				return rankEntry == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{rankEntry.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='text-align: right;'>{rankEntry.Rank}</span>"),
					new($"<span style='text-align: right;'>{GameVersions.GetGameVersionFromDate(rankEntry.DateTime).GetGameVersionString()}</span>"),
				};
			}));
		}

		if (GetPlayerHistory.ActivityHistory.Count > 0)
		{
			DateTime minX = GetPlayerHistory.ActivityHistory.Select(ah => ah.DateTime).Min();
			DateTime maxX = DateTime.UtcNow;

			IEnumerable<double> deaths = GetPlayerHistory.ActivityHistory.Select(ah => ah.DeathsIncrement);
			const double deathsScale = 20.0;
			double deathsMaxY = Math.Ceiling(deaths.Max() / deathsScale) * deathsScale;

			IEnumerable<double> time = GetPlayerHistory.ActivityHistory.Select(ah => ah.TimeIncrement);
			const double timeScale = 2000.0;
			double timeMaxY = Math.Ceiling(time.Max() / timeScale) * timeScale;

			List<LineData> deathsSet = GetPlayerHistory.ActivityHistory.Select((ah, i) => new LineData(ah.DateTime.Ticks, ah.DeathsIncrement, i)).ToList();
			_activityDeathsOptions = new(minX.Ticks, null, maxX.Ticks, 0, deathsScale, deathsMaxY);
			_activityDeathsData.Add(new("#0f0", false, true, true, deathsSet, (ds, d) =>
			{
				GetPlayerHistoryActivityEntry? activityEntry = GetPlayerHistory.ActivityHistory.Count <= d.Index ? null : GetPlayerHistory.ActivityHistory[d.Index];
				return activityEntry == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{activityEntry.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{activityEntry.DeathsIncrement.ToString("0.0")}</span>"),
				};
			}));

			List<LineData> timeSet = GetPlayerHistory.ActivityHistory.Select((ah, i) => new LineData(ah.DateTime.Ticks, ah.TimeIncrement, i)).ToList();
			_activityTimeOptions = new(minX.Ticks, null, maxX.Ticks, 0, timeScale, timeMaxY);
			_activityTimeData.Add(new("#f80", false, true, true, timeSet, (ds, d) =>
			{
				GetPlayerHistoryActivityEntry? activityEntry = GetPlayerHistory.ActivityHistory.Count <= d.Index ? null : GetPlayerHistory.ActivityHistory[d.Index];
				return activityEntry == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{activityEntry.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{activityEntry.TimeIncrement.ToString("0.0000")}</span>"),
				};
			}));
		}
	}
}
