using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.BarChart;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardStatistics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;

public partial class StatisticsPage
{
	private static readonly List<string> _daggers = Daggers.GetDaggers(GameConstants.CurrentVersion).ConvertAll(d => d.Name);
	private static readonly List<string> _deathTypes = Deaths.GetDeaths(GameConstants.CurrentVersion).ConvertAll(d => d.Name);
	private static readonly List<string> _enemies = Enemies.GetEnemies(GameConstants.CurrentVersion).Where(e => e.FirstSpawnSecond.HasValue).OrderBy(e => e.FirstSpawnSecond).Select(e => e.Name).ToList();
	private static readonly List<string> _upgrades = new() { "Level 1", "Level 2", "Level 3 / 4" };

	private GetLeaderboardStatistics? _statistics;

	private BarDataSet? _sub500Data;
	private BarDataSet? _sub1000Data;
	private BarDataSet? _post1000Data;
	private BarDataSet? _killsData;
	private BarDataSet? _gemsData;
	private BarDataSet? _upgradesData;
	private BarDataSet? _daggersData;
	private BarDataSet? _deathsData;
	private BarDataSet? _enemiesData;

	private BarChartDataOptions? _sub500DataOptions;
	private BarChartDataOptions? _sub1000DataOptions;
	private BarChartDataOptions? _post1000DataOptions;
	private BarChartDataOptions? _killsDataOptions;
	private BarChartDataOptions? _gemsDataOptions;
	private BarChartDataOptions? _upgradesDataOptions;
	private BarChartDataOptions? _daggersDataOptions;
	private BarChartDataOptions? _deathsDataOptions;
	private BarChartDataOptions? _enemiesDataOptions;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		_statistics = await Http.GetLeaderboardStatistics();

		if (!_statistics.IsFetched)
			return;

		IEnumerable<KeyValuePair<int, int>> sub500Scores = _statistics.TimesStatistics.Where(kvp => kvp.Key < 500);
		SetScoreChart(sub500Scores, 5000.0, ref _sub500Data, ref _sub500DataOptions);

		IEnumerable<KeyValuePair<int, int>> sub1000Scores = _statistics.TimesStatistics.Where(kvp => kvp.Key >= 500 && kvp.Key < 1000);
		SetScoreChart(sub1000Scores, 20.0, ref _sub1000Data, ref _sub1000DataOptions);

		IEnumerable<KeyValuePair<int, int>> post1000Scores = _statistics.TimesStatistics.Where(kvp => kvp.Key >= 1000);
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

		List<BarData> killsSet = _statistics.KillsStatistics.Where(kvp => kvp.Key < 1000).Select(kvp => new BarData("#880", kvp.Value, kvp)).ToList();
		const double killsScale = 2500.0;
		_killsDataOptions = new(0, killsScale, Math.Ceiling(_statistics.KillsStatistics.Max(kvp => kvp.Value) / killsScale) * killsScale);
		_killsData = new(killsSet, (ds, d) =>
		{
			return new();
		});

		List<BarData> gemsSet = _statistics.GemsStatistics.Where(kvp => kvp.Key < 500).Select(kvp => new BarData("#f00", kvp.Value, kvp)).ToList();
		const double gemsScale = 20000.0;
		_gemsDataOptions = new(0, gemsScale, Math.Ceiling(_statistics.GemsStatistics.Max(kvp => kvp.Value) / gemsScale) * gemsScale);
		_gemsData = new(gemsSet, (ds, d) =>
		{
			return new();
		});

		List<BarData> upgradesSet = new()
		{
			new(UpgradesV3_2.Level1.Color.HexCode, _statistics.PlayersWithLevel1, _statistics.PlayersWithLevel1),
			new(UpgradesV3_2.Level2.Color.HexCode, _statistics.PlayersWithLevel2, _statistics.PlayersWithLevel2),
			new(UpgradesV3_2.Level3.Color.HexCode, _statistics.PlayersWithLevel3Or4, _statistics.PlayersWithLevel3Or4),
		};
		const double upgradesScale = 50000.0;
		_upgradesDataOptions = new(0, upgradesScale, Math.Ceiling(new int[] { _statistics.PlayersWithLevel1, _statistics.PlayersWithLevel2, _statistics.PlayersWithLevel3Or4 }.Max() / upgradesScale) * upgradesScale);
		_upgradesData = new(upgradesSet, (ds, d) =>
		{
			return new();
		});

		List<BarData> daggersSet = _statistics.DaggersStatistics.Select(kvp => new BarData(Daggers.GetDaggerByName(GameConstants.CurrentVersion, kvp.Key)?.Color.HexCode ?? "#444", kvp.Value, kvp)).ToList();
		const double daggersScale = 20000.0;
		_daggersDataOptions = new(0, daggersScale, Math.Ceiling(_statistics.DaggersStatistics.Max(kvp => kvp.Value) / daggersScale) * daggersScale);
		_daggersData = new(daggersSet, (ds, d) =>
		{
			return new();
		});

		List<BarData> deathsSet = _statistics.DeathsStatistics.Select(kvp => new BarData(Deaths.GetDeathByName(GameConstants.CurrentVersion, kvp.Key)?.Color.HexCode ?? "#444", kvp.Value, kvp)).ToList();
		const double deathsScale = 20000.0;
		_deathsDataOptions = new(0, deathsScale, Math.Ceiling(_statistics.DeathsStatistics.Max(kvp => kvp.Value) / deathsScale) * deathsScale);
		_deathsData = new(deathsSet, (ds, d) =>
		{
			return new();
		});

		List<BarData> enemiesSet = _statistics.EnemiesStatistics.Select(kvp => new BarData(Enemies.GetEnemyByName(GameConstants.CurrentVersion, kvp.Key)?.Color.HexCode ?? "#444", kvp.Value, kvp)).ToList();
		const double enemiesScale = 50000.0;
		_enemiesDataOptions = new(0, enemiesScale, Math.Ceiling(_statistics.EnemiesStatistics.Max(kvp => kvp.Value) / enemiesScale) * enemiesScale);
		_enemiesData = new(enemiesSet, (ds, d) =>
		{
			return new();
		});
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
