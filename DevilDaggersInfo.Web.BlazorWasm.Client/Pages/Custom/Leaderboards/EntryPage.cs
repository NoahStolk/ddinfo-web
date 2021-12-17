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

	private readonly LineChartOptions _lineChartOptions = new()
	{
		HighlighterKeys = new()
		{
			"Value",
		},
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
	};

	private List<(string Name, DataOptions DataOptions, LineChartOptions ChartOptions, List<LineDataSet> Sets)> _lineCharts = new();

	[Parameter, EditorRequired] public int Id { get; set; }

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	public GetCustomEntryData? GetCustomEntryData { get; set; }

	private static void AddDataSet(List<LineDataSet> dataSets, ushort[]? data, string color)
		=> AddDataSet(dataSets, data?.Select(u => (int)u).ToArray(), color);

	private static void AddDataSet(List<LineDataSet> dataSets, int[]? data, string color)
	{
		if (data != null)
			dataSets.Add(new(color, false, false, false, data.Select((val, i) => new LineData(i, val)).ToList(), _highlightTransformation));
	}

	private void AddDataSets(List<LineDataSet> dataSets, double time, string name, params string[] keys)
	{
		if (dataSets.Count == 0)
			return;

		double maxData = dataSets.Select(ds => ds.Data.Select(d => d.Y).Max()).Max();
		DataOptions dataOptions = new(0, Math.Ceiling(time / 10), time, 0, maxData / 8, maxData);
		LineChartOptions chartOptions = new() { HighlighterKeys = keys.ToList(), GridOptions = new() { MinimumRowHeightInPx = 50 } };
		_lineCharts.Add((name, dataOptions, chartOptions, dataSets));
	}

	protected override async Task OnInitializedAsync()
	{
		GetCustomEntryData = await Http.GetCustomEntryDataById(Id);

		List<LineDataSet> gemsSets = new();
		AddDataSet(gemsSets, GetCustomEntryData.GemsTotalData, "#800");
		AddDataSet(gemsSets, GetCustomEntryData.GemsCollectedData, "#f00");
		AddDataSet(gemsSets, GetCustomEntryData.GemsDespawnedData, "#888");
		AddDataSet(gemsSets, GetCustomEntryData.GemsEatenData, "#0f0");
		AddDataSets(gemsSets, GetCustomEntryData.Time, "Gems", "Total Gems", "Collected Gems", "Despawned Gems", "Eaten Gems");

		List<LineDataSet> homingSets = new();
		AddDataSet(homingSets, GetCustomEntryData.HomingStoredData, "#f0f");
		AddDataSet(homingSets, GetCustomEntryData.HomingEatenData, "#c8a2c8");
		AddDataSets(homingSets, GetCustomEntryData.Time, "Homing", "Homing Stored", "Homing Eaten");

		List<LineDataSet> enemiesSets = new();
		AddDataSet(enemiesSets, GetCustomEntryData.EnemiesAliveData, "#840");
		AddDataSet(enemiesSets, GetCustomEntryData.EnemiesKilledData, "#f00");
		AddDataSets(enemiesSets, GetCustomEntryData.Time, "Enemies", "Enemies Alive", "Enemies Killed");

		List<LineDataSet> daggersSets = new();
		AddDataSet(daggersSets, GetCustomEntryData.DaggersHitData, "#ff0");
		AddDataSet(daggersSets, GetCustomEntryData.DaggersFiredData, "#f40");
		AddDataSets(daggersSets, GetCustomEntryData.Time, "Daggers", "Daggers Hit", "Daggers Fired");

		List<LineDataSet> skullsAliveSets = new();
		AddDataSet(skullsAliveSets, GetCustomEntryData.Skull1sAliveData, EnemyColors.Skull1.HexCode);
		AddDataSet(skullsAliveSets, GetCustomEntryData.Skull2sAliveData, EnemyColors.Skull2.HexCode);
		AddDataSet(skullsAliveSets, GetCustomEntryData.Skull3sAliveData, EnemyColors.Skull3.HexCode);
		AddDataSet(skullsAliveSets, GetCustomEntryData.Skull4sAliveData, EnemyColors.Skull4.HexCode);
		AddDataSets(skullsAliveSets, GetCustomEntryData.Time, "Skulls Alive", "Skull Is Alive", "Skull IIs Alive", "Skull IIIs Alive", "Skull IVs Alive");

		List<LineDataSet> squidsAliveSets = new();
		AddDataSet(squidsAliveSets, GetCustomEntryData.Squid1sAliveData, EnemyColors.Squid1.HexCode);
		AddDataSet(squidsAliveSets, GetCustomEntryData.Squid2sAliveData, EnemyColors.Squid2.HexCode);
		AddDataSet(squidsAliveSets, GetCustomEntryData.Squid3sAliveData, EnemyColors.Squid3.HexCode);
		AddDataSets(squidsAliveSets, GetCustomEntryData.Time, "Squids Alive", "Squid Is Alive", "Squid IIs Alive", "Squid IIIs Alive");

		List<LineDataSet> spidersAliveSets = new();
		AddDataSet(spidersAliveSets, GetCustomEntryData.Spider1sAliveData, EnemyColors.Spider1.HexCode);
		AddDataSet(spidersAliveSets, GetCustomEntryData.Spider2sAliveData, EnemyColors.Spider2.HexCode);
		AddDataSet(spidersAliveSets, GetCustomEntryData.SpiderEggsAliveData, EnemyColors.SpiderEgg1.HexCode);
		AddDataSet(spidersAliveSets, GetCustomEntryData.SpiderlingsAliveData, EnemyColors.Spiderling.HexCode);
		AddDataSets(spidersAliveSets, GetCustomEntryData.Time, "Spiders Alive", "Spider Is Alive", "Spider IIs Alive", "Spider Eggs Alive", "Spiderlings Alive");

		List<LineDataSet> pedesAliveSets = new();
		AddDataSet(pedesAliveSets, GetCustomEntryData.CentipedesAliveData, EnemyColors.Centipede.HexCode);
		AddDataSet(pedesAliveSets, GetCustomEntryData.GigapedesAliveData, EnemyColors.Gigapede.HexCode);
		AddDataSet(pedesAliveSets, GetCustomEntryData.GhostpedesAliveData, EnemyColors.Ghostpede.HexCode);
		AddDataSets(pedesAliveSets, GetCustomEntryData.Time, "Pedes Alive", "Centipedes Alive", "Gigapedes Alive", "Ghostpedes Alive");

		List<LineDataSet> otherEnemiesAliveSets = new();
		AddDataSet(otherEnemiesAliveSets, GetCustomEntryData.ThornsAliveData, EnemyColors.Thorn.HexCode);
		AddDataSet(otherEnemiesAliveSets, GetCustomEntryData.LeviathansAliveData, EnemyColors.Leviathan.HexCode);
		AddDataSet(otherEnemiesAliveSets, GetCustomEntryData.OrbsAliveData, EnemyColors.TheOrb.HexCode);
		AddDataSets(otherEnemiesAliveSets, GetCustomEntryData.Time, "Other Enemies Alive", "Thorns Alive", "Leviathans Alive", "Orbs Alive");

		List<LineDataSet> skullsKilledSets = new();
		AddDataSet(skullsKilledSets, GetCustomEntryData.Skull1sKilledData, EnemyColors.Skull1.HexCode);
		AddDataSet(skullsKilledSets, GetCustomEntryData.Skull2sKilledData, EnemyColors.Skull2.HexCode);
		AddDataSet(skullsKilledSets, GetCustomEntryData.Skull3sKilledData, EnemyColors.Skull3.HexCode);
		AddDataSet(skullsKilledSets, GetCustomEntryData.Skull4sKilledData, EnemyColors.Skull4.HexCode);
		AddDataSets(skullsKilledSets, GetCustomEntryData.Time, "Skulls Killed", "Skull Is Killed", "Skull IIs Killed", "Skull IIIs Killed", "Skull IVs Killed");

		List<LineDataSet> squidsKilledSets = new();
		AddDataSet(squidsKilledSets, GetCustomEntryData.Squid1sKilledData, EnemyColors.Squid1.HexCode);
		AddDataSet(squidsKilledSets, GetCustomEntryData.Squid2sKilledData, EnemyColors.Squid2.HexCode);
		AddDataSet(squidsKilledSets, GetCustomEntryData.Squid3sKilledData, EnemyColors.Squid3.HexCode);
		AddDataSets(squidsKilledSets, GetCustomEntryData.Time, "Squids Killed", "Squid Is Killed", "Squid IIs Killed", "Squid IIIs Killed");

		List<LineDataSet> spidersKilledSets = new();
		AddDataSet(spidersKilledSets, GetCustomEntryData.Spider1sKilledData, EnemyColors.Spider1.HexCode);
		AddDataSet(spidersKilledSets, GetCustomEntryData.Spider2sKilledData, EnemyColors.Spider2.HexCode);
		AddDataSet(spidersKilledSets, GetCustomEntryData.SpiderEggsKilledData, EnemyColors.SpiderEgg1.HexCode);
		AddDataSet(spidersKilledSets, GetCustomEntryData.SpiderlingsKilledData, EnemyColors.Spiderling.HexCode);
		AddDataSets(spidersKilledSets, GetCustomEntryData.Time, "Spiders Killed", "Spider Is Killed", "Spider IIs Killed", "Spider Eggs Killed", "Spiderlings Killed");

		List<LineDataSet> pedesKilledSets = new();
		AddDataSet(pedesKilledSets, GetCustomEntryData.CentipedesKilledData, EnemyColors.Centipede.HexCode);
		AddDataSet(pedesKilledSets, GetCustomEntryData.GigapedesKilledData, EnemyColors.Gigapede.HexCode);
		AddDataSet(pedesKilledSets, GetCustomEntryData.GhostpedesKilledData, EnemyColors.Ghostpede.HexCode);
		AddDataSets(pedesKilledSets, GetCustomEntryData.Time, "Pedes Killed", "Centipedes Killed", "Gigapedes Killed", "Ghostpedes Killed");

		List<LineDataSet> otherEnemiesKilledSets = new();
		AddDataSet(otherEnemiesKilledSets, GetCustomEntryData.ThornsKilledData, EnemyColors.Thorn.HexCode);
		AddDataSet(otherEnemiesKilledSets, GetCustomEntryData.LeviathansKilledData, EnemyColors.Leviathan.HexCode);
		AddDataSet(otherEnemiesKilledSets, GetCustomEntryData.OrbsKilledData, EnemyColors.TheOrb.HexCode);
		AddDataSets(otherEnemiesKilledSets, GetCustomEntryData.Time, "Other Enemies Killed", "Thorns Killed", "Leviathans Killed", "Orbs Killed");
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
