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

		DataOptions dataOptions = new(0, Math.Ceiling(time / 10), time, 0, 10, dataSets.Select(ds => ds.Data.Select(d => d.Y).Max()).Max());
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
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
	//await AddLabelsDatasets(_skullsAliveChart,
	//	new("Skull Is Alive", GetCustomEntryData.Skull1sAliveData, EnemyColors.Skull1.HexCode, EnemyColors.Skull1.HexCode),
	//	new("Skull IIs Alive", GetCustomEntryData.Skull2sAliveData, EnemyColors.Skull2.HexCode, EnemyColors.Skull2.HexCode),
	//	new("Skull IIIs Alive", GetCustomEntryData.Skull3sAliveData, EnemyColors.Skull3.HexCode, EnemyColors.Skull3.HexCode),
	//	new("Skull IVs Alive", GetCustomEntryData.Skull4sAliveData, EnemyColors.Skull4.HexCode, EnemyColors.Skull4.HexCode));
	//await AddLabelsDatasets(_squidsAliveChart,
	//	new("Squid Is Alive", GetCustomEntryData.Squid1sAliveData, EnemyColors.Squid1.HexCode, EnemyColors.Squid1.HexCode),
	//	new("Squid IIs Alive", GetCustomEntryData.Squid2sAliveData, EnemyColors.Squid2.HexCode, EnemyColors.Squid2.HexCode),
	//	new("Squid IIIs Alive", GetCustomEntryData.Squid3sAliveData, EnemyColors.Squid3.HexCode, EnemyColors.Squid3.HexCode));
	//await AddLabelsDatasets(_spidersAliveChart,
	//	new("Spider Is Alive", GetCustomEntryData.Spider1sAliveData, EnemyColors.Spider1.HexCode, EnemyColors.Spider1.HexCode),
	//	new("Spider IIs Alive", GetCustomEntryData.Spider2sAliveData, EnemyColors.Spider2.HexCode, EnemyColors.Spider2.HexCode),
	//	new("Spider Eggs Alive", GetCustomEntryData.SpiderEggsAliveData, EnemyColors.SpiderEgg1.HexCode, EnemyColors.SpiderEgg1.HexCode),
	//	new("Spiderlings Alive", GetCustomEntryData.SpiderlingsAliveData, EnemyColors.Spiderling.HexCode, EnemyColors.Spiderling.HexCode));
	//await AddLabelsDatasets(_pedesAliveChart,
	//	new("Centipedes Alive", GetCustomEntryData.CentipedesAliveData, EnemyColors.Centipede.HexCode, EnemyColors.Centipede.HexCode),
	//	new("Gigapedes Alive", GetCustomEntryData.GigapedesAliveData, EnemyColors.Gigapede.HexCode, EnemyColors.Gigapede.HexCode),
	//	new("Ghostpedes Alive", GetCustomEntryData.GhostpedesAliveData, EnemyColors.Ghostpede.HexCode, EnemyColors.Ghostpede.HexCode));
	//await AddLabelsDatasets(_otherEnemiesAliveChart,
	//	new("Thorns Alive", GetCustomEntryData.ThornsAliveData, EnemyColors.Thorn.HexCode, EnemyColors.Thorn.HexCode),
	//	new("Leviathans Alive", GetCustomEntryData.LeviathansAliveData, EnemyColors.Leviathan.HexCode, EnemyColors.Leviathan.HexCode),
	//	new("Orbs Alive", GetCustomEntryData.OrbsAliveData, EnemyColors.TheOrb.HexCode, EnemyColors.TheOrb.HexCode));

	//await AddLabelsDatasets(_skullsKilledChart,
	//	new("Skull Is Killed", GetCustomEntryData.Skull1sKilledData, EnemyColors.Skull1.HexCode, EnemyColors.Skull1.HexCode),
	//	new("Skull IIs Killed", GetCustomEntryData.Skull2sKilledData, EnemyColors.Skull2.HexCode, EnemyColors.Skull2.HexCode),
	//	new("Skull IIIs Killed", GetCustomEntryData.Skull3sKilledData, EnemyColors.Skull3.HexCode, EnemyColors.Skull3.HexCode),
	//	new("Skull IVs Killed", GetCustomEntryData.Skull4sKilledData, EnemyColors.Skull4.HexCode, EnemyColors.Skull4.HexCode));
	//await AddLabelsDatasets(_squidsKilledChart,
	//	new("Squid Is Killed", GetCustomEntryData.Squid1sKilledData, EnemyColors.Squid1.HexCode, EnemyColors.Squid1.HexCode),
	//	new("Squid IIs Killed", GetCustomEntryData.Squid2sKilledData, EnemyColors.Squid2.HexCode, EnemyColors.Squid2.HexCode),
	//	new("Squid IIIs Killed", GetCustomEntryData.Squid3sKilledData, EnemyColors.Squid3.HexCode, EnemyColors.Squid3.HexCode));
	//await AddLabelsDatasets(_spidersKilledChart,
	//	new("Spider Is Killed", GetCustomEntryData.Spider1sKilledData, EnemyColors.Spider1.HexCode, EnemyColors.Spider1.HexCode),
	//	new("Spider IIs Killed", GetCustomEntryData.Spider2sKilledData, EnemyColors.Spider2.HexCode, EnemyColors.Spider2.HexCode),
	//	new("Spider Eggs Killed", GetCustomEntryData.SpiderEggsKilledData, EnemyColors.SpiderEgg1.HexCode, EnemyColors.SpiderEgg1.HexCode),
	//	new("Spiderlings Killed", GetCustomEntryData.SpiderlingsKilledData, EnemyColors.Spiderling.HexCode, EnemyColors.Spiderling.HexCode));
	//await AddLabelsDatasets(_pedesKilledChart,
	//	new("Centipedes Killed", GetCustomEntryData.CentipedesKilledData, EnemyColors.Centipede.HexCode, EnemyColors.Centipede.HexCode),
	//	new("Gigapedes Killed", GetCustomEntryData.GigapedesKilledData, EnemyColors.Gigapede.HexCode, EnemyColors.Gigapede.HexCode),
	//	new("Ghostpedes Killed", GetCustomEntryData.GhostpedesKilledData, EnemyColors.Ghostpede.HexCode, EnemyColors.Ghostpede.HexCode));
	//await AddLabelsDatasets(_otherEnemiesKilledChart,
	//	new("Thorns Killed", GetCustomEntryData.ThornsKilledData, EnemyColors.Thorn.HexCode, EnemyColors.Thorn.HexCode),
	//	new("Leviathans Killed", GetCustomEntryData.LeviathansKilledData, EnemyColors.Leviathan.HexCode, EnemyColors.Leviathan.HexCode),
	//	new("Orbs Killed", GetCustomEntryData.OrbsKilledData, EnemyColors.TheOrb.HexCode, EnemyColors.TheOrb.HexCode));
}
