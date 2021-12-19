using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.BarChart;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;

public partial class StatisticsPage
{
	private GetLeaderboardStatistics? _statistics;

	//private const int _killGraphCount = 100;
	//private const int _gemGraphCount = 50;

	//private BarChartOptions _bco = new()
	//{
	//	MaintainAspectRatio = true,
	//	Responsive = true,
	//	AspectRatio = 3.5f,
	//};

	//private PieChartOptions _pco = new()
	//{
	//	MaintainAspectRatio = true,
	//	Responsive = true,
	//	AspectRatio = 3.5f,
	//};

	//private BarChart<int>? _sub500Chart;
	//private BarChart<int>? _sub1000Chart;
	//private BarChart<int>? _post1000Chart;
	//private BarChart<int>? _killsChart;
	//private BarChart<int>? _gemsChart;
	//private PieChart<int>? _levelsChart;
	//private BarChart<int>? _daggersChart;
	//private BarChart<int>? _deathsChart;
	//private BarChart<int>? _enemiesChart;

	//private readonly BarChartOptions _sub500ChartOptions = new()
	//{
	//	HighlighterKeys = new() { "Date", "Global Accuracy", "Global Daggers Hit", "Global Daggers Fired" },
	//	GridOptions = new()
	//	{
	//		MinimumRowHeightInPx = 50,
	//	},
	//	ScaleYOptions = new() { NumberFormat = "0%" },
	//	DisplayXScaleAsDates = true,
	//};

	private BarDataSet? _sub500Data;
	private BarDataSet? _sub1000Data;
	private BarDataSet? _post1000Data;
	private BarDataSet? _killsData;
	private BarDataSet? _gemsData;

	private BarChartDataOptions? _sub500DataOptions;
	private BarChartDataOptions? _sub1000DataOptions;
	private BarChartDataOptions? _post1000DataOptions;
	private BarChartDataOptions? _killsDataOptions;
	private BarChartDataOptions? _gemsDataOptions;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		_statistics = await Http.GetLeaderboardStatistics();

		if (!_statistics.IsFetched)
			return;

		IEnumerable<KeyValuePair<int, int>> sub500Scores = _statistics.TimeStatistics.Where(kvp => kvp.Key < 500);
		SetScoreChart(sub500Scores, 5000.0, ref _sub500Data, ref _sub500DataOptions);

		IEnumerable<KeyValuePair<int, int>> sub1000Scores = _statistics.TimeStatistics.Where(kvp => kvp.Key >= 500 && kvp.Key < 1000);
		SetScoreChart(sub1000Scores, 20.0, ref _sub1000Data, ref _sub1000DataOptions);

		IEnumerable<KeyValuePair<int, int>> post1000Scores = _statistics.TimeStatistics.Where(kvp => kvp.Key >= 1000);
		SetScoreChart(post1000Scores, 2.0, ref _post1000Data, ref _post1000DataOptions);

		static void SetScoreChart(IEnumerable<KeyValuePair<int, int>> data, double scale, ref BarDataSet? dataSet, ref BarChartDataOptions? dataOptions)
		{
			List<BarData> set = data.Select(kvp => new BarData(Daggers.GetDaggerFromSeconds(GameConstants.CurrentVersion, kvp.Key).Color.HexCode, kvp.Value, kvp)).ToList();
			dataOptions = new(0, scale, Math.Ceiling(data.Max(kvp => kvp.Value) / scale) * scale);
			dataSet = new(set, (ds, d) =>
			{
				return new();
				//GetLeaderboardHistoryStatistics? stats = _statistics.Find(hs => hs == d.Reference);
				//return stats == null ? new() : new()
				//{
				//	new($"<span style='text-align: right;'>{stats.DateTime.ToString(FormatUtils.DateFormat)}</span>"),
				//	new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0")}</span>"),
				//};
			});
		}

		List<BarData> killsSet = _statistics.KillStatistics.Where(kvp => kvp.Key < 500).Select(kvp => new BarData("#880", kvp.Value, kvp)).ToList();
		const double killsScale = 2500.0;
		_killsDataOptions = new(0, killsScale, Math.Ceiling(_statistics.KillStatistics.Max(kvp => kvp.Value) / killsScale) * killsScale);
		_killsData = new(killsSet, (ds, d) =>
		{
			return new();
		});

		List<BarData> gemsSet = _statistics.GemStatistics.Where(kvp => kvp.Key < 500).Select(kvp => new BarData("#f00", kvp.Value, kvp)).ToList();
		const double gemsScale = 15000.0;
		_gemsDataOptions = new(0, gemsScale, Math.Ceiling(_statistics.GemStatistics.Max(kvp => kvp.Value) / gemsScale) * gemsScale);
		_gemsData = new(gemsSet, (ds, d) =>
		{
			return new();
		});
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}

	//protected override async Task OnAfterRenderAsync(bool firstRender)
	//{
	//	if (firstRender ||
	//		_statistics == null ||
	//		_sub500Chart == null ||
	//		_sub1000Chart == null ||
	//		_post1000Chart == null ||
	//		_killsChart == null ||
	//		_gemsChart == null ||
	//		_levelsChart == null ||
	//		_daggersChart == null ||
	//		_deathsChart == null ||
	//		_enemiesChart == null)
	//		return;

	//	int post1000BarCount = _statistics.TimeStatistics.Max(kvp => kvp.Key) / 10 - 100 + 1;

	//	int[] sub500Times = Enumerable.Range(0, 50).Select(i => i * 10).ToArray();
	//	int[] sub1000Times = Enumerable.Range(50, 50).Select(i => i * 10).ToArray();
	//	int[] post1000Times = Enumerable.Range(100, post1000BarCount).Select(i => i * 10).ToArray();

	//	await SetUpBarChart(
	//		barChart: _sub500Chart,
	//		labels: sub500Times.Select(i => $"{i} - {i + 9}").ToArray(),
	//		data: _statistics.TimeStatistics.Select(kvp => kvp.Value).Take(50).ToList(),
	//		backgroundColors: sub500Times.Select(t => Daggers.GetDaggerFromSeconds(GameConstants.CurrentVersion, t).Color.HexCode).ToList());
	//	await SetUpBarChart(
	//		barChart: _sub1000Chart,
	//		labels: sub1000Times.Select(i => $"{i} - {i + 9}").ToArray(),
	//		data: _statistics.TimeStatistics.Select(kvp => kvp.Value).Skip(50).Take(50).ToList(),
	//		backgroundColors: sub1000Times.Select(t => Daggers.GetDaggerFromSeconds(GameConstants.CurrentVersion, t).Color.HexCode).ToList());
	//	await SetUpBarChart(
	//		barChart: _post1000Chart,
	//		labels: post1000Times.Select(i => $"{i} - {i + 9}").ToArray(),
	//		data: _statistics.TimeStatistics.Select(kvp => kvp.Value).Skip(100).Take(post1000BarCount).ToList(),
	//		backgroundColors: post1000Times.Select(t => Daggers.GetDaggerFromSeconds(GameConstants.CurrentVersion, t).Color.HexCode).ToList());
	//	await SetUpBarChart(
	//		barChart: _killsChart,
	//		labels: _statistics.KillStatistics.Take(_killGraphCount).Select(kvp => $"{kvp.Key} - {kvp.Key + 9}").ToArray(),
	//		data: _statistics.KillStatistics.Take(_killGraphCount).Select(kvp => kvp.Value).ToList(),
	//		backgroundColors: Enumerable.Repeat("#dd8800", _killGraphCount).ToList());
	//	await SetUpBarChart(
	//		barChart: _gemsChart,
	//		labels: _statistics.GemStatistics.Take(_gemGraphCount).Select(kvp => $"{kvp.Key} - {kvp.Key + 9}").ToArray(),
	//		data: _statistics.GemStatistics.Take(_gemGraphCount).Select(kvp => kvp.Value).ToList(),
	//		backgroundColors: Enumerable.Repeat("#ff0000", _gemGraphCount).ToList());
	//	await SetUpPieChart(
	//		pieChart: _levelsChart,
	//		labels: new[] { "Level 1", "Level 2", "Level 3 or 4" },
	//		data: new() { _statistics.PlayersWithLevel1, _statistics.PlayersWithLevel2, _statistics.PlayersWithLevel3Or4 },
	//		backgroundColors: Upgrades.GetUpgrades(GameConstants.CurrentVersion).Where(u => u.Level != 4).Select(u => u.Color.HexCode).ToList());
	//	await SetUpBarChart(
	//		barChart: _daggersChart,
	//		labels: _statistics.DaggerStatistics.Select(kvp => kvp.Key).ToArray(),
	//		data: _statistics.DaggerStatistics.Select(kvp => kvp.Value).ToList(),
	//		backgroundColors: _statistics.DaggerStatistics.Select(kvp => Daggers.GetDaggerByName(GameConstants.CurrentVersion, kvp.Key)?.Color.HexCode ?? "#444").ToList());
	//	await SetUpBarChart(
	//		barChart: _deathsChart,
	//		labels: _statistics.DeathStatistics.Select(kvp => kvp.Key).ToArray(),
	//		data: _statistics.DeathStatistics.Select(kvp => kvp.Value).ToList(),
	//		backgroundColors: _statistics.DeathStatistics.Select(kvp => Deaths.GetDeathByName(GameConstants.CurrentVersion, kvp.Key)?.Color.HexCode ?? "#444").ToList());
	//	await SetUpBarChart(
	//		barChart: _enemiesChart,
	//		labels: _statistics.EnemyStatistics.Select(kvp => kvp.Key).ToArray(),
	//		data: _statistics.EnemyStatistics.Select(kvp => kvp.Value).ToList(),
	//		backgroundColors: _statistics.EnemyStatistics.Select(kvp => Enemies.GetEnemyByName(GameConstants.CurrentVersion, kvp.Key)?.Color.HexCode ?? "#444").ToList());
	//}

	//private async Task SetUpBarChart(BarChart<int> barChart, string[] labels, List<int> data, List<string> backgroundColors)
	//{
	//	await barChart.Clear();
	//	await barChart.AddLabelsDatasetsAndUpdate(labels, new BarChartDataset<int>
	//	{
	//		Label = "Players",
	//		Data = data,
	//		BackgroundColor = backgroundColors,
	//	});
	//}

	//private async Task SetUpPieChart(PieChart<int> pieChart, string[] labels, List<int> data, List<string> backgroundColors)
	//{
	//	await pieChart.Clear();
	//	await pieChart.AddLabelsDatasetsAndUpdate(labels, new PieChartDataset<int>
	//	{
	//		Label = "Players",
	//		Data = data,
	//		BackgroundColor = backgroundColors,
	//		BorderWidth = 1,
	//		BorderColor = new List<string> { "#aaa" },
	//		HoverBorderColor = new List<string> { "#ddd" },
	//	});
	//}
}
