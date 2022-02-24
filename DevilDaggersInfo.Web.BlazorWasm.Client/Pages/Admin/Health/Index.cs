using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Admin.Health;

public partial class Index
{
	private GetResponseTimes? _response;
	private Dictionary<string, bool> _sortings = new();
	private DateTime _dateTime = DateTime.UtcNow;

	private readonly LineChartOptions _totalTrafficLineChartOptions = new()
	{
		HighlighterKeys = new() { "Time", "Requests" },
		DisplayXScaleAsDates = true,
	};

	private readonly LineChartOptions _customEntrySubmitLineChartOptions = new()
	{
		HighlighterKeys = new() { "Min Response Time", "Avg Response Time", "Max Response Time" },
		DisplayXScaleAsDates = true,
	};

	private readonly List<LineDataSet> _totalTrafficData = new();
	private readonly List<LineDataSet> _customEntrySubmitData = new();

	private LineChartDataOptions _totalTrafficOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _customEntrySubmitOptions = LineChartDataOptions.Default;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		await FetchEntries();
	}

	private async Task UpdateDateTime(DateTime dateTime)
	{
		_dateTime = dateTime;

		await FetchEntries();
	}

	private async Task FetchEntries()
	{
		_response = null;
		_response = await Http.GetResponseTimes(_dateTime);

		Dictionary<int, int> totalRequests = _response.ResponseTimesByTime.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Sum(e => e.RequestCount));
		const double scale = 100;
		double minY = Math.Floor(totalRequests.Values.Min() / scale) * scale;
		double maxY = Math.Ceiling(totalRequests.Values.Max() / scale) * scale;

		List<LineData> set = totalRequests.Select((kvp, i) => new LineData(kvp.Key, kvp.Value, i)).ToList();
		_totalTrafficOptions = new(0, null, 24 * 60 - 1, minY, scale, maxY);
		_totalTrafficData.Clear();
		_totalTrafficData.Add(new("#f00", false, true, false, set, (ds, d) =>
		{
			KeyValuePair<int, int>? stats = totalRequests.Count <= d.Index ? null : totalRequests.ElementAt(d.Index);
			return stats == null ? new() : new()
			{
				new($"<span style='text-align: right;'>{MinutesToTime(stats.Value.Key)} - {MinutesToTime(stats.Value.Key + _response.MinuteInterval)}</span>"),
				new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0")}</span>"),
			};
		}));

		static string MinutesToTime(int totalMinutes)
		{
			int hours = totalMinutes / 60;
			int minutes = totalMinutes % 60;
			return $"{hours:00}:{minutes:00}";
		}
	}

	private async Task ForceDump()
	{
		await Http.ForceDump(null);

		await FetchEntries();
	}

	private static string GetFormattedTime(double ticks)
	{
		if (ticks >= TimeSpan.TicksPerSecond)
			return $"{ticks / (float)TimeSpan.TicksPerSecond:0.00} s";

		if (ticks >= TimeSpan.TicksPerMillisecond)
			return $"{ticks / (float)TimeSpan.TicksPerMillisecond:0.0} ms";

		return $"{ticks / 10f:0} μs";
	}

	private void Sort<TKey>(Func<GetRequestPathEntry, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		if (_response == null)
			return;

		bool sortDirection = false;
		if (_sortings.ContainsKey(sortingExpression))
			sortDirection = _sortings[sortingExpression];
		else
			_sortings.Add(sortingExpression, false);

		_response.ResponseTimesByRequestPath = _response.ResponseTimesByRequestPath.OrderBy(sorting, sortDirection).ToList();

		_sortings[sortingExpression] = !sortDirection;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
