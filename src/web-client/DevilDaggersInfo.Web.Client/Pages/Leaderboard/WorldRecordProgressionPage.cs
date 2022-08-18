using DevilDaggersInfo.Api.Main.WorldRecords;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Wiki.Extensions;
using DevilDaggersInfo.Razor.Core.CanvasChart.Data;
using DevilDaggersInfo.Razor.Core.CanvasChart.Options.LineChart;
using DevilDaggersInfo.Types.Core.Wiki;
using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Client.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public partial class WorldRecordProgressionPage
{
	private readonly LineChartOptions _lineChartOptions = new()
	{
		HighlighterKeys = new() { "Date", "Time", "Player", "Gems", "Kills", "Accuracy", "Death Type", "Game Version" },
		GridOptions = new()
		{
			MinimumRowHeightInPx = 50,
		},
		XScaleDisplayUnit = ScaleDisplayUnit.TicksAsDate,
		Backgrounds = LineChartUtils.GameVersionBackgrounds,
	};

	private readonly List<LineDataSet> _lineDataSets = new();

	private double _currentWorldRecord;
	private int _currentWorldRecordHolderId;
	private TimeSpan _totalTimeSinceFirstRecord;

	private LineChartDataOptions _dataOptions = LineChartDataOptions.Default;

	private List<GetWorldRecordHolder>? _worldRecordHolders;
	private List<GetWorldRecord>? _worldRecords;

	private readonly Dictionary<string, bool> _worldRecordHoldersSortings = new();
	private readonly Dictionary<string, bool> _worldRecordsSortings = new();

	[Inject]
	public MainApiHttpClient Http { get; set; } = null!;

	[Inject]
	public IJSRuntime JsRuntime { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		GetWorldRecordDataContainer data = await Http.GetWorldRecordData();
		GetWorldRecord currentWr = data.WorldRecords.MaxBy(wr => wr.Entry.Time)!;
		_currentWorldRecord = currentWr.Entry.Time;
		_currentWorldRecordHolderId = currentWr.Entry.Id;

		_totalTimeSinceFirstRecord = DateTime.UtcNow - data.WorldRecords.OrderBy(wr => wr.DateTime).First().DateTime;

		DateTime minX = new(2016, 1, 1);
		DateTime maxX = DateTime.UtcNow;
		GetWorldRecord firstWr = data.WorldRecords[0];
		GetWorldRecord lastWr = data.WorldRecords[^1];
		double minY = Math.Floor(firstWr.Entry.Time / 100.0) * 100;
		double maxY = Math.Ceiling(lastWr.Entry.Time / 100.0) * 100;

		List<LineData> set = data.WorldRecords.Select((wr, i) => new LineData(wr.DateTime.Ticks, wr.Entry.Time, i)).ToList();
		_dataOptions = new(minX.Ticks, null, maxX.Ticks, minY, 100, maxY);
		_lineDataSets.Add(new("#f00", true, true, true, set, (_, d) =>
		{
			GetWorldRecord? wr = data.WorldRecords.Count <= d.Index ? null : data.WorldRecords[d.Index];
			if (wr == null)
				return new();

			GameVersion? gameVersion = GameVersions.GetGameVersionFromDate(wr.DateTime);
			Dagger dagger = Daggers.GetDaggerFromSeconds(gameVersion ?? GameVersion.V1_0, wr.Entry.Time);
			return new()
			{
				new($"<span style='text-align: right;'>{wr.DateTime.ToString(StringFormats.DateFormat)}</span>"),
				new($"<span style='text-align: right;' class='{dagger.Name.ToLower()}'>{wr.Entry.Time.ToString(StringFormats.TimeFormat)}</span>"),
				new($"<span style='text-align: right;' class='{dagger.Name.ToLower()}'>{wr.Entry.Username}</span>"),
				new($"<span style='text-align: right;'>{wr.Entry.Gems}</span>"),
				new($"<span style='text-align: right;'>{wr.Entry.Kills}</span>"),
				new($"<span style='text-align: right;'>{(wr.Entry.DaggersFired == 0 ? 0 : wr.Entry.DaggersHit / (double)wr.Entry.DaggersFired).ToString(StringFormats.AccuracyFormat)}</span>"),
				new($"<span style='text-align: right;'>{MarkupUtils.DeathString(wr.Entry.DeathType, gameVersion ?? GameVersion.V1_0)}</span>"),
				new($"<span style='text-align: right;'>{gameVersion.GetGameVersionString()}</span>"),
			};
		}));

		_worldRecordHolders = data.WorldRecordHolders.OrderByDescending(wrh => wrh.LastHeld).ToList();
		_worldRecords = data.WorldRecords.OrderByDescending(wr => wr.DateTime).ToList();
	}

	private string GetHistoryDateString(DateTime dateTime)
	{
		int daysAgo = (int)Math.Round((DateTime.UtcNow - dateTime).TotalDays);
		return $"{dateTime:MMM dd} '{dateTime:yy} ({daysAgo:N0} day{S(daysAgo)} ago)";
	}

	private static string S(int value)
		=> value == 1 ? string.Empty : "s";

	private void Sort<TSource, TKey>(ref List<TSource> source, Dictionary<string, bool> sortings, Func<TSource, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		bool sortDirection = false;
		if (sortings.ContainsKey(sortingExpression))
			sortDirection = sortings[sortingExpression];
		else
			sortings.Add(sortingExpression, false);

		source = source.OrderBy(sorting, sortDirection).ToList();

		sortings[sortingExpression] = !sortDirection;
	}
}
