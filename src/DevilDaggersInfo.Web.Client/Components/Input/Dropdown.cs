using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Components.Input;

public partial class Dropdown<TKey>
	where TKey : notnull
{
	private bool _show;

	private string SelectedDisplayValue => CurrentValue == null || Values?.ContainsKey(CurrentValue) != true ? "<None selected>" : DisplayValue(CurrentValue);

	[Parameter]
	[EditorRequired]
	public required Dictionary<TKey, string> Values { get; set; }

	[Parameter]
	[EditorRequired]
	public required Func<string, TKey> Converter { get; set; }

	[Parameter]
	public bool ShowDisplayValue { get; set; } = true;

	public void HandleSelect(TKey? item)
	{
		CurrentValue = item;
		_show = false;
		StateHasChanged();
	}

	protected override bool TryParseValueFromString(string? value, out TKey result, out string validationErrorMessage)
	{
		validationErrorMessage = string.Empty;
		result = Converter(value ?? string.Empty);
		return true;
	}

	private string DisplayValue(TKey? key)
	{
		if (key == null || Values == null)
			return string.Empty;

		if (!ShowDisplayValue)
			return key.ToString() ?? string.Empty;

		return $"{key} ({Values[key]})";
	}

	private void OnClickOutside()
	{
		_show = false;
		StateHasChanged();
	}
}
