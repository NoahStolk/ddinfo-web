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
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ChartMarginXInPx = 60,
		DisplayXScaleAsDates = true,
	};

	private readonly LineChartOptions _entrancesLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Top 10 Score", "Top 100 Score" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		DisplayXScaleAsDates = true,
	};

	private readonly LineChartOptions _timeLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Time" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ChartMarginXInPx = 80,
		DisplayXScaleAsDates = true,
		HighlighterWidth = 320,
	};

	private readonly LineChartOptions _deathsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Deaths" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ChartMarginXInPx = 60,
		DisplayXScaleAsDates = true,
	};

	private readonly LineChartOptions _gemsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Gems" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ChartMarginXInPx = 80,
		DisplayXScaleAsDates = true,
	};

	private readonly LineChartOptions _killsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Kills" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ChartMarginXInPx = 80,
		DisplayXScaleAsDates = true,
	};

	private readonly LineChartOptions _accuracyLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Accuracy", "Global Daggers Hit", "Global Daggers Fired" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ScaleYOptions = new() { NumberFormat = "0%" },
		DisplayXScaleAsDates = true,
		HighlighterWidth = 320,
	};

	private readonly List<LineDataSet> _playersData = new();
	private readonly List<LineDataSet> _entrancesData = new();
	private readonly List<LineDataSet> _timeData = new();
	private readonly List<LineDataSet> _deathsData = new();
	private readonly List<LineDataSet> _gemsData = new();
	private readonly List<LineDataSet> _killsData = new();
	private readonly List<LineDataSet> _accuracyData = new();

	private LineChartDataOptions? _playersOptions;
	private LineChartDataOptions? _entrancesOptions;
	private LineChartDataOptions? _timeOptions;
	private LineChartDataOptions? _deathsOptions;
	private LineChartDataOptions? _gemsOptions;
	private LineChartDataOptions? _killsOptions;
	private LineChartDataOptions? _accuracyOptions;

	private List<GetLeaderboardHistoryStatistics>? _statistics;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		_statistics = await Http.GetLeaderboardHistoryStatistics();

		if (_statistics.Count == 0)
			return;

		DateTime maxX = DateTime.UtcNow;

		RegisterTotalPlayers();
		void RegisterTotalPlayers()
		{
			IEnumerable<int> totalPlayers = _statistics.Select(hs => hs.TotalPlayers);
			const double scale = 50000.0;
			double minY = Math.Floor(totalPlayers.Min() / scale) * scale;
			double maxY = Math.Ceiling(totalPlayers.Max() / scale) * scale;

			List<LineData> set = _statistics.Select(hs => new LineData(hs.DateTime.Ticks, hs.TotalPlayers, hs)).ToList();
			_playersOptions = new(_statistics.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, minY, scale, maxY);
			_playersData.Add(new("#f00", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stats = _statistics.Find(hs => hs == d.Reference);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{stats.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString(FormatUtils.LeaderboardIntFormat)}</span>"),
				};
			}));
		}

		RegisterEntrances();
		void RegisterEntrances()
		{
			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => hs.Top10Entrance > 0);
			IEnumerable<double> top10Entrances = relevantData.Select(hs => hs.Top10Entrance);
			IEnumerable<double> top100Entrances = relevantData.Select(hs => hs.Top100Entrance);
			const double scale = 100.0;
			double minY = Math.Floor(top100Entrances.Min() / scale) * scale;
			double maxY = Math.Ceiling(top10Entrances.Max() / scale) * scale;
			_entrancesOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, minY, scale, maxY);

			List<LineData> top10Set = relevantData.Select(hs => new LineData(hs.DateTime.Ticks, hs.Top10Entrance, hs)).ToList();
			_entrancesData.Add(new("#800", false, false, false, top10Set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stats = relevantData.FirstOrDefault(hs => hs == d.Reference);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{stats.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString(FormatUtils.TimeFormat)}</span>"),
				};
			}));

			List<LineData> top100Set = relevantData.Select(hs => new LineData(hs.DateTime.Ticks, hs.Top100Entrance, hs)).ToList();
			_entrancesData.Add(new("#f00", false, false, false, top100Set, (ds, d) => new List<MarkupString> { new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString(FormatUtils.TimeFormat)}</span>") }));
		}

		RegisterTime();
		void RegisterTime()
		{
			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => hs.TimeGlobal > 0);
			IEnumerable<double> stats = relevantData.Select(hs => hs.TimeGlobal);
			const double scale = 300_000_000.0;
			double minY = Math.Floor(stats.Min() / scale) * scale;
			double maxY = Math.Ceiling(stats.Max() / scale) * scale;
			_timeOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, minY, scale, maxY);

			List<LineData> set = relevantData.Select(hs => new LineData(hs.DateTime.Ticks, hs.TimeGlobal, hs)).ToList();
			_timeData.Add(new("#f00", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stat = relevantData.FirstOrDefault(hs => hs == d.Reference);
				return stat == null ? new() : new List<MarkupString>
				{
					new($"<span style='text-align: right;'>{stat.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='text-align: right;'>{stat.TimeGlobal.ToString(FormatUtils.LeaderboardGlobalTimeFormat)}</span>"),
				};
			}));
		}

		Register((hs) => hs.DeathsGlobal, ref _deathsOptions, _deathsData, 2_500_000);
		Register((hs) => hs.GemsGlobal, ref _gemsOptions, _gemsData, 100_000_000);
		Register((hs) => hs.KillsGlobal, ref _killsOptions, _killsData, 1_000_000_000);
		void Register(Func<GetLeaderboardHistoryStatistics, ulong> selector, ref LineChartDataOptions? lineChartDataOptions, List<LineDataSet> dataSets, double scale)
		{
			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => selector(hs) > 0);
			IEnumerable<ulong> stats = relevantData.Select(hs => selector(hs));
			double minY = Math.Floor(stats.Min() / scale) * scale;
			double maxY = Math.Ceiling(stats.Max() / scale) * scale;
			lineChartDataOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, minY, scale, maxY);

			List<LineData> set = relevantData.Select(hs => new LineData(hs.DateTime.Ticks, selector(hs), hs)).ToList();
			dataSets.Add(new("#f00", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stat = relevantData.FirstOrDefault(hs => hs == d.Reference);
				return stat == null ? new() : new List<MarkupString>
				{
					new($"<span style='text-align: right;'>{stat.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='text-align: right;'>{selector(stat).ToString(FormatUtils.LeaderboardIntFormat)}</span>"),
				};
			}));
		}

		RegisterAccuracy();
		void RegisterAccuracy()
		{
			Func<ulong, ulong, double> converter = static (hit, fired) => fired == 0 ? 0 : hit / (double)fired;

			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => hs.DaggersFiredGlobal > 0);
			IEnumerable<double> accuracy = relevantData.Select(hs => converter(hs.DaggersHitGlobal, hs.DaggersFiredGlobal));
			double max = ((int)Math.Ceiling((int)(accuracy.Max() * 100) / 5.0) * 5) / 100.0 + 0.0001;
			_accuracyOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, 0.2, 0.01, max, true);

			List<LineData> set = relevantData.Select(hs => new LineData(hs.DateTime.Ticks, converter(hs.DaggersHitGlobal, hs.DaggersFiredGlobal), hs)).ToList();
			_accuracyData.Add(new("#f80", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stat = relevantData.FirstOrDefault(hs => hs == d.Reference);
				return stat == null ? new() : new List<MarkupString>
				{
					new($"<span style='text-align: right;'>{stat.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{converter(stat.DaggersHitGlobal, stat.DaggersFiredGlobal).ToString(FormatUtils.AccuracyFormat)}</span>"),
					new($"<span style='text-align: right;'>{(stat.DaggersFiredGlobal == 10000 ? "?" : stat.DaggersHitGlobal.ToString(FormatUtils.LeaderboardIntFormat))}</span>"),
					new($"<span style='text-align: right;'>{(stat.DaggersFiredGlobal == 10000 ? "?" : stat.DaggersFiredGlobal.ToString(FormatUtils.LeaderboardIntFormat))}</span>"),
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
