using DevilDaggersInfo.Core.Wiki.Colors;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Custom.Leaderboards;

public partial class EntryPage
{
	private static readonly Func<LineDataSet, LineData, List<MarkupString>> _highlightTransformation = static (ds, d) => new List<MarkupString> { new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0")}</span>") };

	private List<(string Name, DataOptions DataOptions, LineChartOptions ChartOptions, List<LineDataSet> Sets)> _lineCharts = new();

	[Parameter, EditorRequired] public int Id { get; set; }

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	public GetCustomEntryData? GetCustomEntryData { get; set; }

	private static void AddDataSet(List<(LineDataSet Set, string Name)> dataSets, ushort[]? data, string name, string color)
		=> AddDataSet(dataSets, data?.Select(u => (int)u).ToArray(), name, color);

	private static void AddDataSet(List<(LineDataSet Set, string Name)> dataSets, int[]? data, string name, string color)
	{
		if (data != null)
			dataSets.Add((new(color, false, false, false, data.Select((val, i) => new LineData(i, val)).ToList(), _highlightTransformation), name));
	}

	private void AddDataSets(List<(LineDataSet Set, string Name)> dataSets, double time, string name)
	{
		if (dataSets.Count == 0)
			return;

		double maxData = dataSets.Select(ds => ds.Set.Data.Select(d => d.Y).Max()).Max();
		int digits = ((int)Math.Round(maxData)).ToString().Length;
		int roundingPoint = (int)Math.Pow(10, digits - 1);
		maxData = Math.Ceiling(maxData / roundingPoint) * roundingPoint;
		DataOptions dataOptions = new(0, Math.Ceiling(time / 10), Math.Floor(time), 0, maxData / 8, maxData);
		LineChartOptions chartOptions = new() { HighlighterKeys = dataSets.ConvertAll(ds => ds.Name), HighlighterTitle = "Time", HighlighterTitleValueNumberFormat = "0.0000", GridOptions = new() { MinimumRowHeightInPx = 50 } };
		_lineCharts.Add((name, dataOptions, chartOptions, dataSets.ConvertAll(ds => ds.Set)));
	}

	protected override async Task OnInitializedAsync()
	{
		GetCustomEntryData = await Http.GetCustomEntryDataById(Id);

		List<(LineDataSet Set, string Name)> gemsSets = new();
		AddDataSet(gemsSets, GetCustomEntryData.GemsTotalData, "Total Gems", "#800");
		AddDataSet(gemsSets, GetCustomEntryData.GemsCollectedData, "Gems Collected", "#f00");
		AddDataSet(gemsSets, GetCustomEntryData.GemsDespawnedData, "Gems Despawned", "#888");
		AddDataSet(gemsSets, GetCustomEntryData.GemsEatenData, "Gems Eaten", "#0f0");
		AddDataSets(gemsSets, GetCustomEntryData.Time, "Gems");

		List<(LineDataSet Set, string Name)> homingSets = new();
		AddDataSet(homingSets, GetCustomEntryData.HomingStoredData, "Homing Stored", "#f0f");
		AddDataSet(homingSets, GetCustomEntryData.HomingEatenData, "Homing Eaten", "#c8a2c8");
		AddDataSets(homingSets, GetCustomEntryData.Time, "Homing");

		List<(LineDataSet Set, string Name)> enemiesSets = new();
		AddDataSet(enemiesSets, GetCustomEntryData.EnemiesAliveData, "Enemies Alive", "#840");
		AddDataSet(enemiesSets, GetCustomEntryData.EnemiesKilledData, "Enemies Killed", "#f00");
		AddDataSets(enemiesSets, GetCustomEntryData.Time, "Enemies");

		List<(LineDataSet Set, string Name)> daggersSets = new();
		AddDataSet(daggersSets, GetCustomEntryData.DaggersHitData, "Daggers Hit", "#ff0");
		AddDataSet(daggersSets, GetCustomEntryData.DaggersFiredData, "Daggers Fired", "#f40");
		AddDataSets(daggersSets, GetCustomEntryData.Time, "Daggers");

		List<(LineDataSet Set, string Name)> skullsAliveSets = new();
		AddDataSet(skullsAliveSets, GetCustomEntryData.Skull1sAliveData, "Skull Is Alive", EnemyColors.Skull1.HexCode);
		AddDataSet(skullsAliveSets, GetCustomEntryData.Skull2sAliveData, "Skull IIs Alive", EnemyColors.Skull2.HexCode);
		AddDataSet(skullsAliveSets, GetCustomEntryData.Skull3sAliveData, "Skull IIIs Alive", EnemyColors.Skull3.HexCode);
		AddDataSet(skullsAliveSets, GetCustomEntryData.Skull4sAliveData, "Skull IVs Alive", EnemyColors.Skull4.HexCode);
		AddDataSets(skullsAliveSets, GetCustomEntryData.Time, "Skulls Alive");

		List<(LineDataSet Set, string Name)> squidsAliveSets = new();
		AddDataSet(squidsAliveSets, GetCustomEntryData.Squid1sAliveData, "Squid Is Alive", EnemyColors.Squid1.HexCode);
		AddDataSet(squidsAliveSets, GetCustomEntryData.Squid2sAliveData, "Squid IIs Alive", EnemyColors.Squid2.HexCode);
		AddDataSet(squidsAliveSets, GetCustomEntryData.Squid3sAliveData, "Squid IIIs Alive", EnemyColors.Squid3.HexCode);
		AddDataSets(squidsAliveSets, GetCustomEntryData.Time, "Squids Alive");

		List<(LineDataSet Set, string Name)> spidersAliveSets = new();
		AddDataSet(spidersAliveSets, GetCustomEntryData.Spider1sAliveData, "Spider Is Alive", EnemyColors.Spider1.HexCode);
		AddDataSet(spidersAliveSets, GetCustomEntryData.Spider2sAliveData, "Spider IIs Alive", EnemyColors.Spider2.HexCode);
		AddDataSet(spidersAliveSets, GetCustomEntryData.SpiderEggsAliveData, "Spider Eggs Alive", EnemyColors.SpiderEgg1.HexCode);
		AddDataSet(spidersAliveSets, GetCustomEntryData.SpiderlingsAliveData, "Spiderlings Alive", EnemyColors.Spiderling.HexCode);
		AddDataSets(spidersAliveSets, GetCustomEntryData.Time, "Spiders Alive");

		List<(LineDataSet Set, string Name)> pedesAliveSets = new();
		AddDataSet(pedesAliveSets, GetCustomEntryData.CentipedesAliveData, "Centipedes Alive", EnemyColors.Centipede.HexCode);
		AddDataSet(pedesAliveSets, GetCustomEntryData.GigapedesAliveData, "Gigapedes Alive", EnemyColors.Gigapede.HexCode);
		AddDataSet(pedesAliveSets, GetCustomEntryData.GhostpedesAliveData, "Ghostpedes Alive", EnemyColors.Ghostpede.HexCode);
		AddDataSets(pedesAliveSets, GetCustomEntryData.Time, "Pedes Alive");

		List<(LineDataSet Set, string Name)> otherEnemiesAliveSets = new();
		AddDataSet(otherEnemiesAliveSets, GetCustomEntryData.ThornsAliveData, "Thorns Alive", EnemyColors.Thorn.HexCode);
		AddDataSet(otherEnemiesAliveSets, GetCustomEntryData.LeviathansAliveData, "Leviathans Alive", EnemyColors.Leviathan.HexCode);
		AddDataSet(otherEnemiesAliveSets, GetCustomEntryData.OrbsAliveData, "Orbs Alive", EnemyColors.TheOrb.HexCode);
		AddDataSets(otherEnemiesAliveSets, GetCustomEntryData.Time, "Other Enemies Alive");

		List<(LineDataSet Set, string Name)> skullsKilledSets = new();
		AddDataSet(skullsKilledSets, GetCustomEntryData.Skull1sKilledData, "Skull Is Killed", EnemyColors.Skull1.HexCode);
		AddDataSet(skullsKilledSets, GetCustomEntryData.Skull2sKilledData, "Skull IIs Killed", EnemyColors.Skull2.HexCode);
		AddDataSet(skullsKilledSets, GetCustomEntryData.Skull3sKilledData, "Skull IIIs Killed", EnemyColors.Skull3.HexCode);
		AddDataSet(skullsKilledSets, GetCustomEntryData.Skull4sKilledData, "Skull IVs Killed", EnemyColors.Skull4.HexCode);
		AddDataSets(skullsKilledSets, GetCustomEntryData.Time, "Skulls Killed");

		List<(LineDataSet Set, string Name)> squidsKilledSets = new();
		AddDataSet(squidsKilledSets, GetCustomEntryData.Squid1sKilledData, "Squid Is Killed", EnemyColors.Squid1.HexCode);
		AddDataSet(squidsKilledSets, GetCustomEntryData.Squid2sKilledData, "Squid IIs Killed", EnemyColors.Squid2.HexCode);
		AddDataSet(squidsKilledSets, GetCustomEntryData.Squid3sKilledData, "Squid IIIs Killed", EnemyColors.Squid3.HexCode);
		AddDataSets(squidsKilledSets, GetCustomEntryData.Time, "Squids Killed");

		List<(LineDataSet Set, string Name)> spidersKilledSets = new();
		AddDataSet(spidersKilledSets, GetCustomEntryData.Spider1sKilledData, "Spider Is Killed", EnemyColors.Spider1.HexCode);
		AddDataSet(spidersKilledSets, GetCustomEntryData.Spider2sKilledData, "Spider IIs Killed", EnemyColors.Spider2.HexCode);
		AddDataSet(spidersKilledSets, GetCustomEntryData.SpiderEggsKilledData, "Spider Eggs Killed", EnemyColors.SpiderEgg1.HexCode);
		AddDataSet(spidersKilledSets, GetCustomEntryData.SpiderlingsKilledData, "Spiderlings Killed", EnemyColors.Spiderling.HexCode);
		AddDataSets(spidersKilledSets, GetCustomEntryData.Time, "Spiders Killed");

		List<(LineDataSet Set, string Name)> pedesKilledSets = new();
		AddDataSet(pedesKilledSets, GetCustomEntryData.CentipedesKilledData, "Centipedes Killed", EnemyColors.Centipede.HexCode);
		AddDataSet(pedesKilledSets, GetCustomEntryData.GigapedesKilledData, "Gigapedes Killed", EnemyColors.Gigapede.HexCode);
		AddDataSet(pedesKilledSets, GetCustomEntryData.GhostpedesKilledData, "Ghostpedes Killed", EnemyColors.Ghostpede.HexCode);
		AddDataSets(pedesKilledSets, GetCustomEntryData.Time, "Pedes Killed");

		List<(LineDataSet Set, string Name)> otherEnemiesKilledSets = new();
		AddDataSet(otherEnemiesKilledSets, GetCustomEntryData.ThornsKilledData, "Thorns Killed", EnemyColors.Thorn.HexCode);
		AddDataSet(otherEnemiesKilledSets, GetCustomEntryData.LeviathansKilledData, "Leviathans Killed", EnemyColors.Leviathan.HexCode);
		AddDataSet(otherEnemiesKilledSets, GetCustomEntryData.OrbsKilledData, "Orbs Killed", EnemyColors.TheOrb.HexCode);
		AddDataSets(otherEnemiesKilledSets, GetCustomEntryData.Time, "Other Enemies Killed");
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
