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
	private readonly LineChartOptions _progressionLineChartOptions = new()
	{
		HighlighterKeys = new()
		{
			"Date",
			"Time",
			"Player",
			"Rank",
			"Gems",
			"Kills",
			"Accuracy",
			"Death type",
		},
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
		DisplayXScaleAsDates = true,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _activityLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Avg deaths per day" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
		DisplayXScaleAsDates = true,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly List<LineDataSet> _progressionData = new();
	private readonly List<LineDataSet> _activityData = new();

	private LineChartDataOptions? _progressionOptions;
	private LineChartDataOptions? _activityOptions;

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

		if (GetPlayerHistory.History.Count > 0)
		{
			DateTime minX = GetPlayerHistory.History.Select(eh => eh.DateTime).Min();
			DateTime maxX = DateTime.UtcNow;

			IEnumerable<double> scores = GetPlayerHistory.History.Select(eh => eh.Time);
			const double scale = 50.0;
			double minY = Math.Floor(scores.Min() / scale) * scale;
			double maxY = Math.Ceiling(scores.Max() / scale) * scale;

			List<LineData> set = GetPlayerHistory.History.Select(eh => new LineData(eh.DateTime.Ticks, eh.Time, eh)).ToList();
			_progressionOptions = new(minX.Ticks, null, maxX.Ticks, minY, scale, maxY);
			_progressionData.Add(new("#f00", true, true, true, set, (ds, d) =>
			{
				GetPlayerHistoryScoreEntry? entry = GetPlayerHistory.History.Find(eh => eh == d.Reference);
				if (entry == null)
					return new();

				GameVersion gameVersion = GameVersions.GetGameVersionFromDate(entry.DateTime) ?? GameVersion.V1_0;
				Dagger dagger = Daggers.GetDaggerFromSeconds(gameVersion, entry.Time);
				return new()
				{
					new($"<span style='text-align: right;'>{entry.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='text-align: right;' class='{dagger.Name.ToLower()}'>{entry.Time.ToString(FormatUtils.TimeFormat)}</span>"),
					new($"<span style='text-align: right;' class='{dagger.Name.ToLower()}'>{entry.Username}</span>"),
					new($"<span style='text-align: right;'>{entry.Rank}</span>"),
					new($"<span style='text-align: right;'>{entry.Gems}</span>"),
					new($"<span style='text-align: right;'>{entry.Kills}</span>"),
					new($"<span style='text-align: right;'>{(entry.DaggersFired == 0 ? 0 : entry.DaggersHit / (double)entry.DaggersFired).ToString(FormatUtils.AccuracyFormat)}</span>"),
					new($"<span style='text-align: right;'>{MarkupUtils.DeathString(entry.DeathType, gameVersion)}</span>"),
				};
			}));
		}

		if (GetPlayerHistory.Activity.Count > 0)
		{
			DateTime minX = GetPlayerHistory.Activity.Select(pa => pa.DateTime).Min();
			DateTime maxX = DateTime.UtcNow;

			IEnumerable<double> deaths = GetPlayerHistory.Activity.Select(pa => pa.DeathsIncrement);
			const double scale = 20.0;
			double maxY = Math.Ceiling(deaths.Max() / scale) * scale;

			List<LineData> set = GetPlayerHistory.Activity.Select(pa => new LineData(pa.DateTime.Ticks, pa.DeathsIncrement, pa)).ToList();
			_activityOptions = new(minX.Ticks, null, maxX.Ticks, 0, scale, maxY);
			_activityData.Add(new("#f00", false, true, true, set, (ds, d) =>
			{
				GetPlayerHistoryActivityEntry? activity = GetPlayerHistory.Activity.Find(pa => pa == d.Reference);
				return activity == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{activity.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0.0")}</span>"),
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
