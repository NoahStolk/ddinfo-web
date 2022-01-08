using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;

public partial class HistoryStatisticsPage
{
	private readonly LineChartOptions _playersLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Players" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
		ChartMarginXInPx = 60,
		DisplayXScaleAsDates = true,
	};

	private readonly LineChartOptions _entrancesLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Top 10 Score", "Top 100 Score" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
		DisplayXScaleAsDates = true,
	};

	private readonly LineChartOptions _accuracyLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Accuracy", "Global Daggers Hit", "Global Daggers Fired" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
		ScaleYOptions = new() { NumberFormat = "0%" },
		DisplayXScaleAsDates = true,
	};

	private readonly List<LineDataSet> _playersData = new();
	private readonly List<LineDataSet> _entrancesData = new();
	private readonly List<LineDataSet> _accuracyData = new();

	private LineChartDataOptions? _playersOptions;
	private LineChartDataOptions? _entrancesOptions;
	private LineChartDataOptions? _accuracyOptions;

	private List<GetLeaderboardHistoryStatistics>? _statistics;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		_statistics = await Http.GetLeaderboardHistoryStatistics();

		if (_statistics.Count == 0)
			return;

		DateTime minX = _statistics.Select(hs => hs.DateTime).Min();
		DateTime maxX = DateTime.UtcNow;

		RegisterTotalPlayers();
		void RegisterTotalPlayers()
		{
			IEnumerable<int> totalPlayers = _statistics.Select(hs => hs.TotalPlayers);
			const double scale = 50000.0;
			double minY = Math.Floor(totalPlayers.Min() / scale) * scale;
			double maxY = Math.Ceiling(totalPlayers.Max() / scale) * scale;

			List<LineData> set = _statistics.Select(hs => new LineData(hs.DateTime.Ticks, hs.TotalPlayers, hs)).ToList();
			_playersOptions = new(minX.Ticks, null, maxX.Ticks, minY, scale, maxY);
			_playersData.Add(new("#f00", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stats = _statistics.Find(hs => hs == d.Reference);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{stats.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0")}</span>"),
				};
			}));
		}

		RegisterEntrances();
		void RegisterEntrances()
		{
			IEnumerable<double> top10Entrances = _statistics.Select(hs => hs.Top10Entrance);
			IEnumerable<double> top100Entrances = _statistics.Select(hs => hs.Top100Entrance);
			const double scale = 100.0;
			double minY = Math.Floor(top100Entrances.Min() / scale) * scale;
			double maxY = Math.Ceiling(top10Entrances.Max() / scale) * scale;
			_entrancesOptions = new(minX.Ticks, null, maxX.Ticks, minY, scale, maxY);

			List<LineData> top10Set = _statistics.Select(hs => new LineData(hs.DateTime.Ticks, hs.Top10Entrance, hs)).ToList();
			_entrancesData.Add(new("#800", false, false, false, top10Set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stats = _statistics.Find(hs => hs == d.Reference);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{stats.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0.0000")}</span>"),
				};
			}));

			List<LineData> top100Set = _statistics.Select(hs => new LineData(hs.DateTime.Ticks, hs.Top100Entrance, hs)).ToList();
			_entrancesData.Add(new("#f00", false, false, false, top100Set, (ds, d) => new List<MarkupString> { new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0.0000")}</span>") }));
		}

		RegisterAccuracy();
		void RegisterAccuracy()
		{
			Func<ulong, ulong, double> accuracyConverter = static (hit, fired) => fired == 0 ? 0 : hit / (double)fired;
			Func<LineDataSet, LineData, List<MarkupString>> accuracyHighlighter = (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stat = _statistics.Find(hs => hs == d.Reference);
				return stat == null ? new() : new List<MarkupString>
				{
					new($"<span style='text-align: right;'>{stat.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{accuracyConverter(stat.DaggersHitGlobal, stat.DaggersFiredGlobal).ToString("0.00%")}</span>"),
					new($"<span style='text-align: right;'>{(stat.DaggersFiredGlobal == 10000 ? 0 : stat.DaggersHitGlobal)}</span>"),
					new($"<span style='text-align: right;'>{(stat.DaggersFiredGlobal == 10000 ? 0 : stat.DaggersFiredGlobal)}</span>"),
				};
			};

			IEnumerable<double> accuracy = _statistics.Select(hs => accuracyConverter(hs.DaggersHitGlobal, hs.DaggersFiredGlobal));
			List<LineData> set = _statistics.Select(hs => new LineData(hs.DateTime.Ticks, accuracyConverter(hs.DaggersHitGlobal, hs.DaggersFiredGlobal), hs)).ToList();
			_accuracyOptions = new(minX.Ticks, null, maxX.Ticks, Math.Floor(accuracy.Min() * 10) / 10, 0.05, Math.Ceiling(accuracy.Max() * 10) / 10, true);
			_accuracyData.Add(new("#f80", false, false, false, set, accuracyHighlighter));
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
