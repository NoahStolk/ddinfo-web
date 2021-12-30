using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;

public partial class MultiSearchDropdown
{
	private string? _searchValue;
	private bool _show = false;

	private string SelectedDisplayValue => $"{CurrentValue?.Count ?? 0} selected";

	[Parameter] public IReadOnlyDictionary<int, string>? Values { get; set; }

	public IReadOnlyDictionary<int, string> FilteredItems => Values == null ? new Dictionary<int, string>() : _searchValue == null ? Values : Values
		.Where(kvp =>
			kvp.Key.ToString()?.Contains(_searchValue, StringComparison.InvariantCultureIgnoreCase) == true ||
			kvp.Value.Contains(_searchValue, StringComparison.InvariantCultureIgnoreCase))
		.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

	private void Add(int i)
	{
		CurrentValue ??= new();
		if (!CurrentValue.Contains(i))
			CurrentValue.Add(i);
	}

	private void Remove(int i)
	{
		if (CurrentValue?.Contains(i) == true)
			CurrentValue.Remove(i);
	}

	protected override bool TryParseValueFromString(string? value, out List<int>? result, out string validationMessage)
	{
		validationMessage = string.Empty;
		result = value?.Split(',').Select(int.Parse).ToList();
		return true;
	}

	private string DisplayValue(int key)
	{
		string value = Values?.ContainsKey(key) == true ? Values[key] : "???";
		return $"{key} ({value})";
	}
}
