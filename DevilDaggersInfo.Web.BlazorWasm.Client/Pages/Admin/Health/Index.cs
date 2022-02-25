using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Data;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options;
using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Web.BlazorWasm.Client.Utils;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
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
		XScaleDisplayUnit = ScaleDisplayUnit.MinutesAsTime,
		GridOptions = new()
		{
			MinimumColumnWidthInPx = 30,
		},
	};

	private readonly LineChartOptions _customEntrySubmitLineChartOptions = new()
	{
		HighlighterKeys = new() { "Time", "Min Response Time", "Avg Response Time", "Max Response Time" },
		XScaleDisplayUnit = ScaleDisplayUnit.MinutesAsTime,
		YScaleDisplayUnit = ScaleDisplayUnit.TicksAsSeconds,
		GridOptions = new()
		{
			MinimumColumnWidthInPx = 30,
		},
		HighlighterWidth = 360,
		ChartMarginXInPx = 80,
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

		RegisterTotalRequests();
		void RegisterTotalRequests()
		{
			_totalTrafficData.Clear();

			Dictionary<int, int> totalRequests = new();
			foreach (KeyValuePair<int, GetRequestPathEntry> kvp in _response.ResponseTimesByTimeByRequestPath.SelectMany(kvp => kvp.Value))
			{
				if (totalRequests.ContainsKey(kvp.Key))
					totalRequests[kvp.Key] += kvp.Value.RequestCount;
				else
					totalRequests.Add(kvp.Key, kvp.Value.RequestCount);
			}

			if (totalRequests.Count == 0)
				return;

			const double scale = 100;
			double minY = Math.Floor(totalRequests.Values.Min() / scale) * scale;
			double maxY = Math.Ceiling(totalRequests.Values.Max() / scale) * scale;

			_totalTrafficOptions = new(0, 60, 24 * 60, minY, scale, maxY);

			List<LineData> set = totalRequests.Select((kvp, i) => new LineData(kvp.Key, kvp.Value, i)).ToList();
			_totalTrafficData.Add(new("#f00", false, true, false, set, (ds, d) =>
			{
				KeyValuePair<int, int>? stats = totalRequests.Count <= d.Index ? null : totalRequests.ElementAt(d.Index);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{TimeUtils.MinutesToTimeString(stats.Value.Key)} - {TimeUtils.MinutesToTimeString(stats.Value.Key + _response.MinuteInterval)}</span>"),
					new($"<span style='color: {ds.Color}; text-align: right;'>{d.Y.ToString("0")}</span>"),
				};
			}));
		}

		RegisterTimesForSpecificRoute("/api/custom-entries/submit");
		void RegisterTimesForSpecificRoute(string route)
		{
			_customEntrySubmitData.Clear();

			Dictionary<int, GetRequestPathEntry>? clRequests = _response.ResponseTimesByTimeByRequestPath.ContainsKey(route) ? _response.ResponseTimesByTimeByRequestPath[route] : null;
			if (clRequests == null || clRequests.Count == 0)
				return;

			const double scale = TimeSpan.TicksPerSecond / 5;
			double minY = Math.Floor(clRequests.Values.Min(e => e.MinResponseTimeTicks) / scale) * scale;
			double maxY = Math.Ceiling(clRequests.Values.Max(e => e.MaxResponseTimeTicks) / scale) * scale;

			_customEntrySubmitOptions = new(0, 60, 24 * 60, minY, scale, maxY);

			_customEntrySubmitData.Add(new("#0f0", false, true, false, clRequests.Select((kvp, i) => new LineData(kvp.Key, kvp.Value.MinResponseTimeTicks, i)).ToList(), (ds, d) =>
			{
				KeyValuePair<int, GetRequestPathEntry>? stats = clRequests.Count <= d.Index ? null : clRequests.ElementAt(d.Index);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{TimeUtils.MinutesToTimeString(stats.Value.Key)} - {TimeUtils.MinutesToTimeString(stats.Value.Key + _response.MinuteInterval)}</span>"),
					new($"<span style='color: #f00; text-align: right;'>{TimeUtils.TicksToTimeString(stats.Value.Value.MaxResponseTimeTicks)}</span>"),
					new($"<span style='color: #ff0; text-align: right;'>{TimeUtils.TicksToTimeString(stats.Value.Value.AverageResponseTimeTicks)}</span>"),
					new($"<span style='color: #0f0; text-align: right;'>{TimeUtils.TicksToTimeString(stats.Value.Value.MinResponseTimeTicks)}</span>"),
				};
			}));
			_customEntrySubmitData.Add(new("#ff0", false, true, false, clRequests.Select((kvp, i) => new LineData(kvp.Key, kvp.Value.AverageResponseTimeTicks, i)).ToList(), null));
			_customEntrySubmitData.Add(new("#f00", false, true, false, clRequests.Select((kvp, i) => new LineData(kvp.Key, kvp.Value.MaxResponseTimeTicks, i)).ToList(), null));
		}
	}

	private async Task ForceDump()
	{
		await Http.ForceDump(null);

		await FetchEntries();
	}

	private void Sort<TKey>(Func<KeyValuePair<string, GetRequestPathEntry>, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		if (_response == null)
			return;

		bool sortDirection = false;
		if (_sortings.ContainsKey(sortingExpression))
			sortDirection = _sortings[sortingExpression];
		else
			_sortings.Add(sortingExpression, false);

		_response.ResponseTimeSummaryByRequestPath = _response.ResponseTimeSummaryByRequestPath.OrderBy(sorting, sortDirection).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		_sortings[sortingExpression] = !sortDirection;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			await JsRuntime.InvokeAsync<object>("init");
	}
}
