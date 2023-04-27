using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Common;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.Extensions;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Razor.Core.CanvasChart.Data;
using DevilDaggersInfo.Razor.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.Client.Extensions;
using DevilDaggersInfo.Web.Client.HttpClients;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;

namespace DevilDaggersInfo.Web.Client.Pages.Custom.Leaderboards;

public partial class EntryPage
{
	private static readonly Func<LineDataSet, LineData, List<MarkupString>> _initialHighlightTransformation = static (ds, d) => new List<MarkupString>
	{
		new($"<span style='text-align: right;'>{d.X.ToString(StringFormats.TimeFormat)}</span>"),
		new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y:0}</span>"),
	};
	private static readonly Func<LineDataSet, LineData, List<MarkupString>> _highlightTransformation = static (ds, d) => new List<MarkupString>
	{
		new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y:0}</span>"),
	};

	private bool _notFound;

	private readonly List<(string Name, LineChartDataOptions DataOptions, LineChartOptions ChartOptions, List<LineDataSet> Sets)> _lineCharts = new();
	private int _time;

	private readonly List<LineChartBackground> _backgrounds = new();

	[Parameter]
	[EditorRequired]
	public int Id { get; set; }

	[Inject]
	public required MainApiHttpClient Http { get; set; }

	[Inject]
	public required IJSRuntime JsRuntime { get; set; }

	public GetCustomEntryData? GetCustomEntryData { get; set; }

	protected override async Task OnInitializedAsync()
	{
		try
		{
			GetCustomEntryData = await Http.GetCustomEntryDataById(Id);
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			_notFound = true;
			return;
		}

		_time = (int)Math.Ceiling(GetCustomEntryData.Time);

		HandLevel handLevel = GetCustomEntryData.StartingLevel.ToCore();
		for (HandLevel i = handLevel; i <= HandLevel.Level4; i++)
		{
			double nextLevelUp = i switch
			{
				HandLevel.Level1 => GetCustomEntryData.LevelUpTime2,
				HandLevel.Level2 => GetCustomEntryData.LevelUpTime3,
				HandLevel.Level3 => GetCustomEntryData.LevelUpTime4,
				_ => 0,
			};

			Upgrade? upgrade = i.GetUpgradeByHandLevel();
			string color = !upgrade.HasValue ? "#fff2" : $"{upgrade.Value.Color.HexCode}08";
			_backgrounds.Add(new() { Color = color, ChartEndXValue = nextLevelUp == 0 ? _time : nextLevelUp });
			if (nextLevelUp == 0)
				break;
		}

		AddLineChart(
			"Gems",
			(GetCustomEntryData.GemsTotalData, "Total Gems", "#800"),
			(GetCustomEntryData.GemsCollectedData, "Gems Collected", "#f00"),
			(GetCustomEntryData.GemsDespawnedData, "Gems Despawned", "#888"),
			(GetCustomEntryData.GemsEatenData, "Gems Eaten", "#0f0"));

		AddLineChart(
			"Homing",
			(GetCustomEntryData.HomingStoredData, "Homing Stored", "#f0f"),
			(GetCustomEntryData.HomingEatenData, "Homing Eaten", "#c8a2c8"));

		AddLineChart(
			"Enemies",
			(GetCustomEntryData.EnemiesAliveData, "Enemies Alive", "#840"),
			(GetCustomEntryData.EnemiesKilledData, "Enemies Killed", "#f00"));

		if (GetCustomEntryData.DaggersHitData != null && GetCustomEntryData.DaggersFiredData != null)
		{
			int min = new[] { GetCustomEntryData.DaggersHitData.Length, GetCustomEntryData.DaggersFiredData.Length, _time }.Min();
			Accuracy[] stats = new Accuracy[min];
			for (int i = 0; i < min; i++)
			{
				int hit = GetCustomEntryData.DaggersHitData[i];
				int fired = GetCustomEntryData.DaggersFiredData[i];
				stats[i] = new(hit, fired, fired == 0 ? 0 : hit / (double)fired);
			}

			List<MarkupString> AccuracyHighlighter(LineDataSet ds, LineData d)
			{
				Accuracy? stat = stats.Length <= d.Index ? null : stats[d.Index];
				return stat == null ? new() : new List<MarkupString>
				{
					new($"<span style='text-align: right;'>{d.X:0.0000}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{stat.Acc:0.00%}</span>"),
					new($"<span style='text-align: right;'>{stat.Hit}</span>"),
					new($"<span style='text-align: right;'>{stat.Fired}</span>"),
				};
			}

			double minAcc = stats.Select(t => t.Acc).Min();
			double maxAcc = stats.Select(t => t.Acc).Max();
			LineChartDataOptions dataOptions = new(0, _time / 10, _time, Math.Floor(minAcc * 10) / 10, 0.1, Math.Ceiling(maxAcc * 10) / 10, true);
			LineChartOptions chartOptions = new()
			{
				HighlighterKeys = new() { "Time", "Accuracy", "Daggers Hit", "Daggers Fired" },
				GridOptions = new() { MinimumRowHeightInPx = 50 },
				ScaleYOptions = new() { NumberFormat = "0%" },
				Backgrounds = _backgrounds,
			};
			_lineCharts.Add(("Accuracy", dataOptions, chartOptions, new() { new("#f80", false, true, false, stats.Select((t, i) => new LineData(i, t.Acc, i)).ToList(), AccuracyHighlighter) }));
		}

		AddLineChart(
			"Skulls Alive",
			(GetCustomEntryData.Skull1sAliveData, "Skull Is Alive", EnemyColors.Skull1.HexCode),
			(GetCustomEntryData.Skull2sAliveData, "Skull IIs Alive", EnemyColors.Skull2.HexCode),
			(GetCustomEntryData.Skull3sAliveData, "Skull IIIs Alive", EnemyColors.Skull3.HexCode),
			(GetCustomEntryData.Skull4sAliveData, "Skull IVs Alive", EnemyColors.Skull4.HexCode));

		AddLineChart(
			"Squids Alive",
			(GetCustomEntryData.Squid1sAliveData, "Squid Is Alive", EnemyColors.Squid1.HexCode),
			(GetCustomEntryData.Squid2sAliveData, "Squid IIs Alive", EnemyColors.Squid2.HexCode),
			(GetCustomEntryData.Squid3sAliveData, "Squid IIIs Alive", EnemyColors.Squid3.HexCode));

		AddLineChart(
			"Spiders Alive",
			(GetCustomEntryData.Spider1sAliveData, "Spider Is Alive", EnemyColors.Spider1.HexCode),
			(GetCustomEntryData.Spider2sAliveData, "Spider IIs Alive", EnemyColors.Spider2.HexCode),
			(GetCustomEntryData.SpiderEggsAliveData, "Spider Eggs Alive", EnemyColors.SpiderEgg1.HexCode),
			(GetCustomEntryData.SpiderlingsAliveData, "Spiderlings Alive", EnemyColors.Spiderling.HexCode));

		AddLineChart(
			"Pedes Alive",
			(GetCustomEntryData.CentipedesAliveData, "Centipedes Alive", EnemyColors.Centipede.HexCode),
			(GetCustomEntryData.GigapedesAliveData, "Gigapedes Alive", EnemyColors.Gigapede.HexCode),
			(GetCustomEntryData.GhostpedesAliveData, "Ghostpedes Alive", EnemyColors.Ghostpede.HexCode));

		AddLineChart(
			"Other Enemies Alive",
			(GetCustomEntryData.ThornsAliveData, "Thorns Alive", EnemyColors.Thorn.HexCode),
			(GetCustomEntryData.LeviathansAliveData, "Leviathans Alive", EnemyColors.Leviathan.HexCode),
			(GetCustomEntryData.OrbsAliveData, "Orbs Alive", EnemyColors.TheOrb.HexCode));

		AddLineChart(
			"Skulls Killed",
			(GetCustomEntryData.Skull1sKilledData, "Skull Is Killed", EnemyColors.Skull1.HexCode),
			(GetCustomEntryData.Skull2sKilledData, "Skull IIs Killed", EnemyColors.Skull2.HexCode),
			(GetCustomEntryData.Skull3sKilledData, "Skull IIIs Killed", EnemyColors.Skull3.HexCode),
			(GetCustomEntryData.Skull4sKilledData, "Skull IVs Killed", EnemyColors.Skull4.HexCode));

		AddLineChart(
			"Squids Killed",
			(GetCustomEntryData.Squid1sKilledData, "Squid Is Killed", EnemyColors.Squid1.HexCode),
			(GetCustomEntryData.Squid2sKilledData, "Squid IIs Killed", EnemyColors.Squid2.HexCode),
			(GetCustomEntryData.Squid3sKilledData, "Squid IIIs Killed", EnemyColors.Squid3.HexCode));

		AddLineChart(
			"Spiders Killed",
			(GetCustomEntryData.Spider1sKilledData, "Spider Is Killed", EnemyColors.Spider1.HexCode),
			(GetCustomEntryData.Spider2sKilledData, "Spider IIs Killed", EnemyColors.Spider2.HexCode),
			(GetCustomEntryData.SpiderEggsKilledData, "Spider Eggs Killed", EnemyColors.SpiderEgg1.HexCode),
			(GetCustomEntryData.SpiderlingsKilledData, "Spiderlings Killed", EnemyColors.Spiderling.HexCode));

		AddLineChart(
			"Pedes Killed",
			(GetCustomEntryData.CentipedesKilledData, "Centipedes Killed", EnemyColors.Centipede.HexCode),
			(GetCustomEntryData.GigapedesKilledData, "Gigapedes Killed", EnemyColors.Gigapede.HexCode),
			(GetCustomEntryData.GhostpedesKilledData, "Ghostpedes Killed", EnemyColors.Ghostpede.HexCode));

		AddLineChart(
			"Other Enemies Killed",
			(GetCustomEntryData.ThornsKilledData, "Thorns Killed", EnemyColors.Thorn.HexCode),
			(GetCustomEntryData.LeviathansKilledData, "Leviathans Killed", EnemyColors.Leviathan.HexCode),
			(GetCustomEntryData.OrbsKilledData, "Orbs Killed", EnemyColors.TheOrb.HexCode));
	}

	// TODO: Use INumber in .NET 7.
	private void AddLineChart(string chartName, params (ushort[]? Data, string Name, string HexColor)[] dataSets)
	{
		List<(LineDataSet Set, string Name)> sets = new();
		foreach ((ushort[]? Data, string Name, string HexColor) dataSet in dataSets)
			AddDataSet(sets, dataSet.Data, dataSet.Name, dataSet.HexColor);

		AddDataSets(sets, chartName);
	}

	// TODO: Use INumber in .NET 7.
	private void AddLineChart(string chartName, params (int[]? Data, string Name, string HexColor)[] dataSets)
	{
		List<(LineDataSet Set, string Name)> sets = new();
		foreach ((int[]? Data, string Name, string HexColor) dataSet in dataSets)
			AddDataSet(sets, dataSet.Data, dataSet.Name, dataSet.HexColor);

		AddDataSets(sets, chartName);
	}

	// TODO: Use INumber in .NET 7.
	private void AddDataSet(List<(LineDataSet Set, string Name)> dataSets, ushort[]? data, string name, string color)
		=> AddDataSet(dataSets, data?.Select(u => (int)u).ToArray(), name, color);

	// TODO: Use INumber in .NET 7.
	private void AddDataSet(List<(LineDataSet Set, string Name)> dataSets, int[]? data, string name, string color)
	{
		if (data != null)
			dataSets.Add((new(color, false, true, false, data.Take(_time).Select((val, i) => new LineData(i, val, val)).ToList(), dataSets.Count == 0 ? _initialHighlightTransformation : _highlightTransformation), name));
	}

	private void AddDataSets(List<(LineDataSet Set, string Name)> dataSets, string name)
	{
		if (dataSets.Count == 0)
			return;

		double maxData = dataSets.Select(ds => ds.Set.Data.Select(d => d.Y).Max()).Max();
		int digits = ((int)Math.Round(maxData)).ToString().Length;
		int roundingPoint = (int)Math.Pow(10, digits - 1);
		maxData = Math.Ceiling(maxData / roundingPoint) * roundingPoint;
		LineChartDataOptions dataOptions = new(0, _time / 10, _time, 0, maxData / 8, maxData);
		LineChartOptions chartOptions = new()
		{
			HighlighterKeys = dataSets.ConvertAll(ds => ds.Name).Prepend("Time").ToList(), GridOptions = new() {MinimumRowHeightInPx = 50,}, Backgrounds = _backgrounds, HighlighterWidth = 320,
		};
		_lineCharts.Add((name, dataOptions, chartOptions, dataSets.ConvertAll(ds => ds.Set)));
	}

	private sealed record Accuracy(int Hit, int Fired, double Acc);
}
