using DevilDaggersInfo.Api.Main.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Core.Wiki.Extensions;
using DevilDaggersInfo.Razor.Core.CanvasChart.Data;
using DevilDaggersInfo.Razor.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.Client.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public partial class HistoryStatisticsPage
{
	private readonly LineChartOptions _playersLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Players", "Game Version" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ScaleYOptions = new() { NumberFormat = StringFormats.LeaderboardIntFormat },
		ChartMarginXInPx = 60,
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _entrancesLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Top 1 Score", "Top 2 Score", "Top 3 Score", "Top 10 Score", "Top 100 Score", "Game Version" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _timeLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Time" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ScaleYOptions = new() { NumberFormat = StringFormats.LeaderboardIntFormat },
		ChartMarginXInPx = 100,
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		HighlighterWidth = 320,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _deathsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Deaths" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ScaleYOptions = new() { NumberFormat = StringFormats.LeaderboardIntFormat },
		ChartMarginXInPx = 100,
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _gemsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Gems" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ScaleYOptions = new() { NumberFormat = StringFormats.LeaderboardIntFormat },
		ChartMarginXInPx = 100,
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _killsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Kills" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ScaleYOptions = new() { NumberFormat = StringFormats.LeaderboardIntFormat },
		ChartMarginXInPx = 100,
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly LineChartOptions _accuracyLineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Global Accuracy", "Global Daggers Hit", "Global Daggers Fired" },
		GridOptions = new() { MinimumRowHeightInPx = 50 },
		ScaleYOptions = new() { NumberFormat = "0%" },
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		HighlighterWidth = 320,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly List<LineDataSet> _playersData = new();
	private readonly List<LineDataSet> _entrancesData = new();
	private readonly List<LineDataSet> _timeData = new();
	private readonly List<LineDataSet> _deathsData = new();
	private readonly List<LineDataSet> _gemsData = new();
	private readonly List<LineDataSet> _killsData = new();
	private readonly List<LineDataSet> _accuracyData = new();

	private LineChartDataOptions _playersOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _entrancesOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _timeOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _deathsOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _gemsOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _killsOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _accuracyOptions = LineChartDataOptions.Default;

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
			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => hs.TotalPlayers > 0 && hs.TotalPlayersUpdated);
			IEnumerable<int> totalPlayers = relevantData.Select(hs => hs.TotalPlayers);
			const double scale = 50_000;
			double minY = Math.Floor(totalPlayers.Min() / scale) * scale;
			double maxY = Math.Ceiling(totalPlayers.Max() / scale) * scale;

			List<LineData> set = relevantData.Select((hs, i) => new LineData(hs.DateTime.Ticks, hs.TotalPlayers, i)).ToList();
			_playersOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, minY, scale, maxY);
			_playersData.Add(new("#f00", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stats = relevantData.Count() <= d.Index ? null : relevantData.ElementAt(d.Index);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{stats.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString(StringFormats.LeaderboardIntFormat)}</span>"),
					new($"<span style='text-align: right;'>{GameVersions.GetGameVersionFromDate(stats.DateTime).GetGameVersionString()}</span>"),
				};
			}));
		}

		RegisterEntrances();
		void RegisterEntrances()
		{
			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => hs.Top1Entrance > 0 || hs.Top2Entrance > 0 || hs.Top3Entrance > 0 || hs.Top10Entrance > 0 || hs.Top100Entrance > 0);
			IEnumerable<double> top1Entrances = relevantData.Select(hs => hs.Top1Entrance);
			IEnumerable<double> top100Entrances = relevantData.Select(hs => hs.Top100Entrance);
			const double scale = 200;
			double minY = Math.Floor(top100Entrances.Min() / scale) * scale;
			double maxY = Math.Ceiling(top1Entrances.Max() / scale) * scale;
			_entrancesOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, minY, scale, maxY);

			const string top1 = "#fc0";
			const string top2 = "#bbb";
			const string top3 = "#a42";
			const string top10 = "#0aa";
			const string top100 = "#07a";
			_entrancesData.Add(new(top1, false, false, false, relevantData.Where(hs => hs.Top1EntranceUpdated).Select((hs, i) => new LineData(hs.DateTime.Ticks, hs.Top1Entrance, i)).ToList(), (_, d) =>
			{
				GetLeaderboardHistoryStatistics? stats = relevantData.Count() <= d.Index ? null : relevantData.ElementAt(d.Index);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{stats.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='color: {top1}; text-align: right;'>{stats.Top1Entrance.ToString(StringFormats.TimeFormat)}</span>"),
					new($"<span style='color: {top2}; text-align: right;'>{stats.Top2Entrance.ToString(StringFormats.TimeFormat)}</span>"),
					new($"<span style='color: {top3}; text-align: right;'>{stats.Top3Entrance.ToString(StringFormats.TimeFormat)}</span>"),
					new($"<span style='color: {top10}; text-align: right;'>{stats.Top10Entrance.ToString(StringFormats.TimeFormat)}</span>"),
					new($"<span style='color: {top100}; text-align: right;'>{stats.Top100Entrance.ToString(StringFormats.TimeFormat)}</span>"),
					new($"<span style='text-align: right;'>{GameVersions.GetGameVersionFromDate(stats.DateTime).GetGameVersionString()}</span>"),
				};
			}));

			_entrancesData.Add(new(top2, false, false, false, relevantData.Where(hs => hs.Top2EntranceUpdated).Select((hs, i) => new LineData(hs.DateTime.Ticks, hs.Top2Entrance, i)).ToList(), null));
			_entrancesData.Add(new(top3, false, false, false, relevantData.Where(hs => hs.Top3EntranceUpdated).Select((hs, i) => new LineData(hs.DateTime.Ticks, hs.Top3Entrance, i)).ToList(), null));
			_entrancesData.Add(new(top10, false, false, false, relevantData.Where(hs => hs.Top10EntranceUpdated).Select((hs, i) => new LineData(hs.DateTime.Ticks, hs.Top10Entrance, i)).ToList(), null));
			_entrancesData.Add(new(top100, false, false, false, relevantData.Where(hs => hs.Top100EntranceUpdated).Select((hs, i) => new LineData(hs.DateTime.Ticks, hs.Top100Entrance, i)).ToList(), null));
		}

		RegisterTime();
		void RegisterTime()
		{
			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => hs.TimeGlobal > 0 && hs.TimeGlobalUpdated);
			IEnumerable<double> stats = relevantData.Select(hs => hs.TimeGlobal);
			const double scale = 300_000_000;
			double minY = Math.Floor(stats.Min() / scale) * scale;
			double maxY = Math.Ceiling(stats.Max() / scale) * scale;
			_timeOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, minY, scale, maxY);

			List<LineData> set = relevantData.Select((hs, i) => new LineData(hs.DateTime.Ticks, hs.TimeGlobal, i)).ToList();
			_timeData.Add(new("#f00", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stat = relevantData.Count() <= d.Index ? null : relevantData.ElementAt(d.Index);
				return stat == null ? new() : new List<MarkupString>
				{
					new($"<span style='text-align: right;'>{stat.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{stat.TimeGlobal.ToString(StringFormats.LeaderboardGlobalTimeFormat)}</span>"),
				};
			}));
		}

		Register(hs => hs.DeathsGlobal, hs => hs.DeathsGlobalUpdated, ref _deathsOptions, _deathsData, 5_000_000);
		Register(hs => hs.GemsGlobal, hs => hs.GemsGlobalUpdated, ref _gemsOptions, _gemsData, 100_000_000);
		Register(hs => hs.KillsGlobal, hs => hs.KillsGlobalUpdated, ref _killsOptions, _killsData, 1_000_000_000);
		void Register(Func<GetLeaderboardHistoryStatistics, ulong> valueSelector, Func<GetLeaderboardHistoryStatistics, bool> valueUpdatedSelector, ref LineChartDataOptions lineChartDataOptions, List<LineDataSet> dataSets, double scale)
		{
			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => valueSelector(hs) > 0 && valueUpdatedSelector(hs));
			IEnumerable<ulong> stats = relevantData.Select(valueSelector);
			double minY = Math.Floor(stats.Min() / scale) * scale;
			double maxY = Math.Ceiling(stats.Max() / scale) * scale;
			lineChartDataOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, minY, scale, maxY);

			List<LineData> set = relevantData.Select((hs, i) => new LineData(hs.DateTime.Ticks, valueSelector(hs), i)).ToList();
			dataSets.Add(new("#f00", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stat = relevantData.Count() <= d.Index ? null : relevantData.ElementAt(d.Index);
				return stat == null ? new() : new List<MarkupString>
				{
					new($"<span style='text-align: right;'>{stat.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{valueSelector(stat).ToString(StringFormats.LeaderboardIntFormat)}</span>"),
				};
			}));
		}

		RegisterAccuracy();
		void RegisterAccuracy()
		{
			Func<ulong, ulong, double> converter = static (hit, fired) => fired == 0 ? 0 : hit / (double)fired;

			IEnumerable<GetLeaderboardHistoryStatistics> relevantData = _statistics.Where(hs => hs.DaggersFiredGlobal > 0 && hs.DaggersFiredGlobalUpdated);
			IEnumerable<double> accuracy = relevantData.Select(hs => converter(hs.DaggersHitGlobal, hs.DaggersFiredGlobal));
			double max = (int)Math.Ceiling((int)(accuracy.Max() * 100) / 5.0) * 5 / 100.0 + 0.0001;
			_accuracyOptions = new(relevantData.Min(hs => hs.DateTime.Ticks), null, maxX.Ticks, 0.2, 0.01, max, true);

			List<LineData> set = relevantData.Select((hs, i) => new LineData(hs.DateTime.Ticks, converter(hs.DaggersHitGlobal, hs.DaggersFiredGlobal), i)).ToList();
			_accuracyData.Add(new("#f80", false, false, false, set, (ds, d) =>
			{
				GetLeaderboardHistoryStatistics? stat = relevantData.Count() <= d.Index ? null : relevantData.ElementAt(d.Index);
				return stat == null ? new() : new List<MarkupString>
				{
					new($"<span style='text-align: right;'>{stat.DateTime.ToString(StringFormats.DateFormat)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{converter(stat.DaggersHitGlobal, stat.DaggersFiredGlobal).ToString(StringFormats.AccuracyFormat)}</span>"),
					new($"<span style='text-align: right;'>{(stat.DaggersFiredGlobal == 10_000 ? "?" : stat.DaggersHitGlobal.ToString(StringFormats.LeaderboardIntFormat))}</span>"),
					new($"<span style='text-align: right;'>{(stat.DaggersFiredGlobal == 10_000 ? "?" : stat.DaggersFiredGlobal.ToString(StringFormats.LeaderboardIntFormat))}</span>"),
				};
			}));
		}
	}
}
