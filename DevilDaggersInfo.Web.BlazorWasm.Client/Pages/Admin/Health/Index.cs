using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Health;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Admin.Health;

public partial class Index
{
	private List<GetResponseTimeEntry>? _entries;

	private Dictionary<string, bool> _sortings = new();

	private DateTime _dateTime = DateTime.UtcNow;

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
		_entries = null;
		_entries = await Http.GetResponseTimes(_dateTime);
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

	private void Sort<TKey>(Func<GetResponseTimeEntry, TKey> sorting, [CallerArgumentExpression("sorting")] string sortingExpression = "")
	{
		if (_entries == null)
			return;

		bool sortDirection = false;
		if (_sortings.ContainsKey(sortingExpression))
			sortDirection = _sortings[sortingExpression];
		else
			_sortings.Add(sortingExpression, false);

		_entries = _entries.OrderBy(sorting, sortDirection).ToList();

		_sortings[sortingExpression] = !sortDirection;
	}
}
