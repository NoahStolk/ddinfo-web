using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using Microsoft.AspNetCore.Components;
using System;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using Microsoft.JSInterop;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.WorldRecords;
using System.Xml.Linq;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;

public partial class PlayerPage
{
	private readonly LineChartOptions _progressionLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Time" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
	};

	private readonly LineChartOptions _activityLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Avg deaths per day" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
	};

	private readonly List<LineDataSet> _progressionData = new();
	private readonly List<LineDataSet> _activityData = new();

	private DataOptions? _progressionOptions;
	private DataOptions? _activityOptions;

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
			DateTime maxX = DateTime.Now;

			IEnumerable<double> scores = GetPlayerHistory.History.Select(eh => eh.Time);
			const double scale = 50.0;
			double minY = Math.Floor(scores.Min() / scale) * scale;
			double maxY = Math.Ceiling(scores.Max() / scale) * scale;

			List<LineData> set = GetPlayerHistory.History.Select(eh => new LineData((eh.DateTime.Ticks - minX.Ticks), eh.Time)).ToList();

			_progressionOptions = new(0, null, (maxX - minX).Ticks, minY, scale, maxY);

			_progressionData.Add(new("#f00", true, true, true, set, (ds, d) => new List<MarkupString> { new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0.0000")}</span>") }));
		}

		if (GetPlayerHistory.Activity.Count > 0)
		{
			DateTime minX = GetPlayerHistory.Activity.Select(pa => pa.DateTime).Min();
			DateTime maxX = DateTime.Now;

			IEnumerable<double> deaths = GetPlayerHistory.Activity.Select(pa => pa.DeathsIncrement);
			const double scale = 20.0;
			double maxY = Math.Ceiling(deaths.Max() / scale) * scale;

			List<LineData> set = GetPlayerHistory.Activity.Select(pa => new LineData((pa.DateTime.Ticks - minX.Ticks), pa.DeathsIncrement)).ToList();

			_activityOptions = new(0, null, (maxX - minX).Ticks, 0, scale, maxY);

			_activityData.Add(new("#f00", false, false, true, set, (ds, d) => new List<MarkupString> { new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0.0")}</span>") }));
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
