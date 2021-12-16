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

	protected override async Task OnInitializedAsync()
	{
		GetCustomEntryData = await Http.GetCustomEntryDataById(Id);

		Func<LineDataSet, LineData, List<MarkupString>> func = static (ds, d) => new List<MarkupString> { new($"<span style='color:{ds.Color};'>{d.Y.ToString("0")}</span>") };

		List<LineDataSet> gemsSets = new();
		if (GetCustomEntryData.GemsTotalData != null)
			gemsSets.Add(new("#800", false, false, false, GetCustomEntryData.GemsTotalData.Select((val, i) => new LineData(i, val)).ToList(), func));
		if (GetCustomEntryData.GemsCollectedData != null)
			gemsSets.Add(new("#f00", false, false, false, GetCustomEntryData.GemsCollectedData.Select((val, i) => new LineData(i, val)).ToList(), func));
		if (GetCustomEntryData.GemsDespawnedData != null)
			gemsSets.Add(new("#888", false, false, false, GetCustomEntryData.GemsDespawnedData.Select((val, i) => new LineData(i, val)).ToList(), func));
		if (GetCustomEntryData.GemsEatenData != null)
			gemsSets.Add(new("#0f0", false, false, false, GetCustomEntryData.GemsEatenData.Select((val, i) => new LineData(i, val)).ToList(), func));

		if (gemsSets.Count > 0)
		{
			DataOptions dataOptions = new(0, Math.Ceiling(GetCustomEntryData.Time / 10), GetCustomEntryData.Time, 0, 10, gemsSets.Select(ds => ds.Data.Select(d => d.Y).Max()).Max());
			LineChartOptions chartOptions = new()
			{
				HighlighterKeys = new()
				{
					"Total Gems",
					"Collected Gems",
					"Despawned Gems",
					"Eaten Gems",
				},
				GridOptions = new()
				{
					MinimumRowHeightInPx = 50,
				},
			};
			_lineCharts.Add(("Gems", dataOptions, chartOptions, gemsSets));
		}

		List<LineDataSet> homingSets = new();
		if (GetCustomEntryData.HomingStoredData != null)
			homingSets.Add(new("#f0f", false, false, false, GetCustomEntryData.HomingStoredData.Select((val, i) => new LineData(i, val)).ToList(), func));
		if (GetCustomEntryData.HomingEatenData != null)
			homingSets.Add(new("#C8A2C8", false, false, false, GetCustomEntryData.HomingEatenData.Select((val, i) => new LineData(i, val)).ToList(), func));

		if (homingSets.Count > 0)
		{
			DataOptions dataOptions = new(0, Math.Ceiling(GetCustomEntryData.Time / 10), GetCustomEntryData.Time, 0, 10, homingSets.Select(ds => ds.Data.Select(d => d.Y).Max()).Max());
			LineChartOptions chartOptions = new()
			{
				HighlighterKeys = new()
				{
					"Homing Stored",
					"Homing Eaten",
				},
				GridOptions = new()
				{
					MinimumRowHeightInPx = 50,
				},
			};
			_lineCharts.Add(("Homing", dataOptions, chartOptions, homingSets));
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
		//await AddLabelsDatasets(_enemiesChart, new("Enemies Alive", GetCustomEntryData.EnemiesAliveData, "#840", "#840"), new("Enemies Killed", GetCustomEntryData.EnemiesKilledData, "#f00", "#f00"));
		//await AddLabelsDatasets(_daggersChart, new("Daggers Hit", GetCustomEntryData.DaggersHitData, "#80f", "#80f"), new("Daggers Fired", GetCustomEntryData.DaggersFiredData, "#00f", "#00f"));

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
