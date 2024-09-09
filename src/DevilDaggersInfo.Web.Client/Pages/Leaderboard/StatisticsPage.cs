using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardStatistics;
using DevilDaggersInfo.Web.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.Client.Core.CanvasChart.Options.BarChart;
using DevilDaggersInfo.Web.Client.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public partial class StatisticsPage
{
	private const string _percentageFormat = "0.000%";

	private static readonly BarChartOptions _scoreBarChartOptions = new() { ChartMarginXInPx = 60, HighlighterKeys = ["Score Range", "Players", "% Of All"], HighlighterWidth = 360 };
	private static readonly BarChartOptions _killsBarChartOptions = new() { ChartMarginXInPx = 60, HighlighterKeys = ["Kills Range", "Players", "% Of All"], HighlighterWidth = 320 };
	private static readonly BarChartOptions _gemsBarChartOptions = new() { ChartMarginXInPx = 60, HighlighterKeys = ["Gems Range", "Players", "% Of All"], HighlighterWidth = 320 };
	private static readonly BarChartOptions _upgradesBarChartOptions = new() { ChartMarginXInPx = 60, ChartMarginYInPx = 80, HighlighterKeys = ["Upgrade", "Players", "% Of All"], HighlighterWidth = 320 };
	private static readonly BarChartOptions _daggersBarChartOptions = new() { ChartMarginXInPx = 60, ChartMarginYInPx = 80, HighlighterKeys = ["Dagger", "Players", "% Of All"], HighlighterWidth = 320 };
	private static readonly BarChartOptions _deathsBarChartOptions = new() { ChartMarginXInPx = 60, ChartMarginYInPx = 80, HighlighterKeys = ["Death Type", "Players", "% Of All"], HighlighterWidth = 320 };
	private static readonly BarChartOptions _enemiesBarChartOptions = new() { ChartMarginXInPx = 60, ChartMarginYInPx = 80, HighlighterKeys = ["Enemy", "Players", "% Of All"], HighlighterWidth = 320 };

	private static readonly List<string> _daggers = Daggers.All.Select(d => d.Name).ToList();
	private static readonly List<string> _deathTypes = Deaths.GetDeaths(GameConstants.CurrentVersion).Select(d => d.Name).ToList();
	private static readonly List<string> _enemies = Enemies.GetEnemies(GameConstants.CurrentVersion).Where(e => e.FirstSpawnSecond.HasValue).OrderByDescending(e => e.FirstSpawnSecond).Select(e => e.Name).Reverse().ToList();
	private static readonly List<string> _upgrades = ["Level 1", "Level 2", "Level 3 / 4"];

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

	private BarChartDataOptions _sub500DataOptions = BarChartDataOptions.Default;
	private BarChartDataOptions _sub1000DataOptions = BarChartDataOptions.Default;
	private BarChartDataOptions _post1000DataOptions = BarChartDataOptions.Default;
	private BarChartDataOptions _killsDataOptions = BarChartDataOptions.Default;
	private BarChartDataOptions _gemsDataOptions = BarChartDataOptions.Default;
	private BarChartDataOptions _upgradesDataOptions = BarChartDataOptions.Default;
	private BarChartDataOptions _daggersDataOptions = BarChartDataOptions.Default;
	private BarChartDataOptions _deathsDataOptions = BarChartDataOptions.Default;
	private BarChartDataOptions _enemiesDataOptions = BarChartDataOptions.Default;

	[Inject]
	public required IJSRuntime JsRuntime { get; set; }

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

		void SetScoreChart(IEnumerable<KeyValuePair<int, int>> data, double scale, ref BarDataSet? dataSet, ref BarChartDataOptions dataOptions)
		{
			List<BarData> set = data.Select((kvp, i) => new BarData(Daggers.GetDaggerFromSeconds(GameConstants.CurrentVersion, kvp.Key).Color.HexCode, kvp.Value, i)).ToList();
			dataOptions = new BarChartDataOptions(0, scale, Math.Ceiling(data.Max(kvp => kvp.Value) / scale) * scale);
			dataSet = new BarDataSet(set, (ds, i) =>
			{
				BarData barData = ds.Data[i];
				int start = data.ElementAt(i).Key;
				Dagger dagger = Daggers.GetDaggerFromSeconds(GameConstants.CurrentVersion, start);
				return
				[
					new MarkupString($"<span class='{dagger.Name.ToLower()}' style='text-align: right;'>{start.ToString(StringFormats.TimeFormat)} - {(start + 9.9999).ToString(StringFormats.TimeFormat)}</span>"),
					new MarkupString($"<span style='text-align: right;'>{barData.Y:0}</span>"),
					new MarkupString($"<span style='text-align: right;'>{(barData.Y / _statistics.TotalEntries).ToString(_percentageFormat)}</span>"),
				];
			});
		}

		List<BarData> killsSet = _statistics.KillsStatistics.Where(kvp => kvp.Key < 1000).Select((kvp, i) => new BarData("#880", kvp.Value, i)).ToList();
		const double killsScale = 2500.0;
		_killsDataOptions = new BarChartDataOptions(0, killsScale, Math.Ceiling(_statistics.KillsStatistics.Max(kvp => kvp.Value) / killsScale) * killsScale);
		_killsData = new BarDataSet(killsSet, (ds, i) =>
		{
			BarData barData = ds.Data[i];
			int start = _statistics.KillsStatistics.ElementAt(i).Key;
			return
			[
				new MarkupString($"<span style='text-align: right;'>{start:0} - {start + 9:0}</span>"),
				new MarkupString($"<span style='text-align: right;'>{barData.Y:0}</span>"),
				new MarkupString($"<span style='text-align: right;'>{(barData.Y / _statistics.TotalEntries).ToString(_percentageFormat)}</span>"),
			];
		});

		List<BarData> gemsSet = _statistics.GemsStatistics.Where(kvp => kvp.Key < 500).Select((kvp, i) => new BarData("#f00", kvp.Value, i)).ToList();
		const double gemsScale = 20000.0;
		_gemsDataOptions = new BarChartDataOptions(0, gemsScale, Math.Ceiling(_statistics.GemsStatistics.Max(kvp => kvp.Value) / gemsScale) * gemsScale);
		_gemsData = new BarDataSet(gemsSet, (ds, i) =>
		{
			BarData barData = ds.Data[i];
			int start = _statistics.GemsStatistics.ElementAt(i).Key;
			return
			[
				new MarkupString($"<span style='text-align: right;'>{start:0} - {start + 9:0}</span>"),
				new MarkupString($"<span style='text-align: right;'>{barData.Y:0}</span>"),
				new MarkupString($"<span style='text-align: right;'>{(barData.Y / _statistics.TotalEntries).ToString(_percentageFormat)}</span>"),
			];
		});

		List<BarData> upgradesSet =
		[
			new(UpgradesV3_2.Level1.Color.HexCode, _statistics.PlayersWithLevel1, _statistics.PlayersWithLevel1),
			new(UpgradesV3_2.Level2.Color.HexCode, _statistics.PlayersWithLevel2, _statistics.PlayersWithLevel2),
			new(UpgradesV3_2.Level3.Color.HexCode, _statistics.PlayersWithLevel3Or4, _statistics.PlayersWithLevel3Or4),
		];
		const double upgradesScale = 50000.0;
		_upgradesDataOptions = new BarChartDataOptions(0, upgradesScale, Math.Ceiling(new[] { _statistics.PlayersWithLevel1, _statistics.PlayersWithLevel2, _statistics.PlayersWithLevel3Or4 }.Max() / upgradesScale) * upgradesScale);
		_upgradesData = new BarDataSet(upgradesSet, (ds, i) =>
		{
			BarData barData = ds.Data[i];
			return
			[
				new MarkupString($"<span style='color: {barData.Color}; text-align: right;'>{_upgrades[i]}</span>"),
				new MarkupString($"<span style='text-align: right;'>{barData.Y:0}</span>"),
				new MarkupString($"<span style='text-align: right;'>{(barData.Y / _statistics.TotalEntries).ToString(_percentageFormat)}</span>"),
			];
		});

		List<BarData> daggersSet = _statistics.DaggersStatistics.Select((kvp, i) => new BarData(Daggers.GetDaggerByName(kvp.Key)?.Color.HexCode ?? MarkupStrings.NoDataColor, kvp.Value, i)).ToList();
		const double daggersScale = 20000.0;
		_daggersDataOptions = new BarChartDataOptions(0, daggersScale, Math.Ceiling(_statistics.DaggersStatistics.Max(kvp => kvp.Value) / daggersScale) * daggersScale);
		_daggersData = new BarDataSet(daggersSet, (ds, i) =>
		{
			BarData barData = ds.Data[i];
			string dagger = _daggers[i];
			return
			[
				new MarkupString($"<span class='{dagger.ToLower()}' style='text-align: right;'>{dagger}</span>"),
				new MarkupString($"<span style='text-align: right;'>{barData.Y:0}</span>"),
				new MarkupString($"<span style='text-align: right;'>{(barData.Y / _statistics.TotalEntries).ToString(_percentageFormat)}</span>"),
			];
		});

		List<BarData> deathsSet = _statistics.DeathsStatistics.Select((kvp, i) => new BarData(Deaths.GetDeathByName(GameConstants.CurrentVersion, kvp.Key)?.Color.HexCode ?? MarkupStrings.NoDataColor, kvp.Value, i)).ToList();
		const double deathsScale = 20000.0;
		_deathsDataOptions = new BarChartDataOptions(0, deathsScale, Math.Ceiling(_statistics.DeathsStatistics.Max(kvp => kvp.Value) / deathsScale) * deathsScale);
		_deathsData = new BarDataSet(deathsSet, (ds, i) =>
		{
			BarData barData = ds.Data[i];
			return
			[
				new MarkupString($"<span style='color: {barData.Color}; text-align: right;'>{_deathTypes[i]}</span>"),
				new MarkupString($"<span style='text-align: right;'>{barData.Y:0}</span>"),
				new MarkupString($"<span style='text-align: right;'>{(barData.Y / _statistics.TotalEntries).ToString(_percentageFormat)}</span>"),
			];
		});

		List<BarData> enemiesSet = _statistics.EnemiesStatistics.Select((kvp, i) => new BarData(Enemies.GetEnemyByName(GameConstants.CurrentVersion, kvp.Key)?.Color.HexCode ?? MarkupStrings.NoDataColor, kvp.Value, i)).ToList();
		const double enemiesScale = 50000.0;
		_enemiesDataOptions = new BarChartDataOptions(0, enemiesScale, Math.Ceiling(_statistics.EnemiesStatistics.Max(kvp => kvp.Value) / enemiesScale) * enemiesScale);
		_enemiesData = new BarDataSet(enemiesSet, (ds, i) =>
		{
			BarData barData = ds.Data[i];
			return
			[
				new MarkupString($"<span style='color: {barData.Color}; text-align: right;'>{_enemies[i]}</span>"),
				new MarkupString($"<span style='text-align: right;'>{barData.Y:0}</span>"),
				new MarkupString($"<span style='text-align: right;'>{(barData.Y / _statistics.TotalEntries).ToString(_percentageFormat)}</span>"),
			];
		});
	}
}
