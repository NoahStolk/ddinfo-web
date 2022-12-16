using DevilDaggersInfo.Api.Admin.Health;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Razor.Core.CanvasChart.Data;
using DevilDaggersInfo.Razor.Core.CanvasChart.Options.LineChart;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Client.Pages.Admin.Health;

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
		HighlighterKeys = new() { "Time", "Max Response Time", "Avg Response Time", "Min Response Time" },
		XScaleDisplayUnit = ScaleDisplayUnit.MinutesAsTime,
		YScaleDisplayUnit = ScaleDisplayUnit.TicksAsSeconds,
		GridOptions = new()
		{
			MinimumColumnWidthInPx = 30,
		},
		HighlighterWidth = 360,
		ChartMarginXInPx = 80,
	};

	private readonly LineChartOptions _customLeaderboardExistsLineChartOptions = new()
	{
		HighlighterKeys = new() { "Time", "Max Response Time", "Avg Response Time", "Min Response Time" },
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
	private readonly List<LineDataSet> _customLeaderboardExistsData = new();

	private LineChartDataOptions _totalTrafficOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _customEntrySubmitOptions = LineChartDataOptions.Default;
	private LineChartDataOptions _customLeaderboardExistsOptions = LineChartDataOptions.Default;

	[Inject]
	public required IJSRuntime JsRuntime { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

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

		RegisterTimesForSpecificRoute(_customEntrySubmitData, ref _customEntrySubmitOptions, "POST /api/custom-entries/submit");
		RegisterTimesForSpecificRoute(_customLeaderboardExistsData, ref _customLeaderboardExistsOptions, "HEAD /api/custom-leaderboards");
		void RegisterTimesForSpecificRoute(List<LineDataSet> dataSet, ref LineChartDataOptions dataOptions, string route)
		{
			dataSet.Clear();

			Dictionary<int, GetRequestPathEntry>? requests = _response.ResponseTimesByTimeByRequestPath.ContainsKey(route) ? _response.ResponseTimesByTimeByRequestPath[route] : null;
			if (requests == null || requests.Count == 0)
				return;

			double maxTicks = requests.Values.Max(e => e.MaxResponseTimeTicks);
			double scale = maxTicks switch
			{
				< TimeSpan.TicksPerMillisecond => TimeSpan.TicksPerMillisecond / 10,
				< TimeSpan.TicksPerMillisecond * 10 => TimeSpan.TicksPerMillisecond,
				< TimeSpan.TicksPerMillisecond * 100 => TimeSpan.TicksPerMillisecond * 10,
				< TimeSpan.TicksPerSecond => TimeSpan.TicksPerSecond / 10,
				< TimeSpan.TicksPerSecond * 5 => TimeSpan.TicksPerSecond / 2,
				_ => TimeSpan.TicksPerSecond,
			};

			double minY = Math.Floor(requests.Values.Min(e => e.MinResponseTimeTicks) / scale) * scale;
			double maxY = Math.Ceiling(maxTicks / scale) * scale;

			dataOptions = new(0, 60, 24 * 60, minY, scale, maxY);

			dataSet.Add(new("#0f0", false, true, false, requests.Select((kvp, i) => new LineData(kvp.Key, kvp.Value.MinResponseTimeTicks, i)).ToList(), (_, d) =>
			{
				KeyValuePair<int, GetRequestPathEntry>? stats = requests.Count <= d.Index ? null : requests.ElementAt(d.Index);
				return stats == null ? new() : new()
				{
					new($"<span style='text-align: right;'>{TimeUtils.MinutesToTimeString(stats.Value.Key)} - {TimeUtils.MinutesToTimeString(stats.Value.Key + _response.MinuteInterval)}</span>"),
					new($"<span style='color: #f00; text-align: right;'>{TimeUtils.TicksToTimeString(stats.Value.Value.MaxResponseTimeTicks)}</span>"),
					new($"<span style='color: #ff0; text-align: right;'>{TimeUtils.TicksToTimeString(stats.Value.Value.AverageResponseTimeTicks)}</span>"),
					new($"<span style='color: #0f0; text-align: right;'>{TimeUtils.TicksToTimeString(stats.Value.Value.MinResponseTimeTicks)}</span>"),
				};
			}));
			dataSet.Add(new("#ff0", false, true, false, requests.Select((kvp, i) => new LineData(kvp.Key, kvp.Value.AverageResponseTimeTicks, i)).ToList(), null));
			dataSet.Add(new("#f00", false, true, false, requests.Select((kvp, i) => new LineData(kvp.Key, kvp.Value.MaxResponseTimeTicks, i)).ToList(), null));
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
}
