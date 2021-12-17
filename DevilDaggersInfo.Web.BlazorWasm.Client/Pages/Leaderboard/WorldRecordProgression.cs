using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.WorldRecords;
using Microsoft.JSInterop;
using System;
using Microsoft.AspNetCore.Components;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using System.Collections.Generic;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;

public partial class WorldRecordProgression
{
	private readonly LineChartOptions _lineChartOptions = new()
	{
		HighlighterTitle = "Date",
		HighlighterTitleValueNumberFormat = "0", // TODO: Date.
		HighlighterKeys = new()
		{
			"Time",
			"Player",
			"Gems",
			"Kills",
			"Accuracy",
			"Death type",
		},
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
	};

	private readonly List<LineDataSet> _lineDataSets = new();

	private DataOptions? _dataOptions;
	private GetWorldRecordDataContainer? _data;

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		_data = await Http.GetWorldRecordData();

		DateTime minX = new(2016, 1, 1);
		DateTime maxX = DateTime.Now;
		GetWorldRecord firstWr = _data.WorldRecords[0];
		GetWorldRecord lastWr = _data.WorldRecords[^1];
		double minY = Math.Floor(firstWr.Entry.Time / 100.0) * 100;
		double maxY = Math.Ceiling(lastWr.Entry.Time / 100.0) * 100;

		List<LineData> set = _data.WorldRecords.Select(wr => new LineData((wr.DateTime.Ticks - minX.Ticks), wr.Entry.Time)).ToList();

		_dataOptions = new(0, null, (maxX - minX).Ticks, minY, 100, maxY);

		_lineDataSets.Add(new("#f00", true, true, true, set, (ds, d) =>
		{
			List<MarkupString> list = new()
			{
				new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0.0000")}</span>"),
			};

			GetWorldRecord? wr = _data.WorldRecords.Find(wr => wr.Entry.Time == d.Y);
			if (wr != null)
			{
				GameVersion gameVersion = GameVersions.GetGameVersionFromDate(wr.DateTime) ?? GameVersion.V1_0;
				list.Add(new($"<span style='text-align: right;'>{wr.Entry.Username}</span>"));
				list.Add(new($"<span style='text-align: right;'>{wr.Entry.Gems}</span>"));
				list.Add(new($"<span style='text-align: right;'>{wr.Entry.Kills}</span>"));
				list.Add(new($"<span style='text-align: right;'>{(wr.Entry.DaggersFired == 0 ? 0 : wr.Entry.DaggersHit / (double)wr.Entry.DaggersFired).ToString("0.00%")}</span>"));
				list.Add(new($"<span style='text-align: right;'>{MarkupUtils.DeathString(wr.Entry.DeathType, gameVersion)}</span>"));
			}

			return list;
		}));
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
