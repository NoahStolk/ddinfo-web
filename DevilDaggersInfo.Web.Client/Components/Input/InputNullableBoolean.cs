using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Components.Input;

public partial class InputNullableBoolean
{
	private string SelectedDisplayValue => CurrentValue switch
	{
		true => True,
		false => False,
		null => "Unknown",
	};

	private bool _show = false;

	[Parameter] public bool ShowDisplayValue { get; set; } = true;

	[Parameter] public string False { get; set; } = "False";
	[Parameter] public string True { get; set; } = "True";

	public void HandleSelect(bool? item)
	{
		CurrentValue = item;
		_show = false;
		StateHasChanged();
	}

	protected override bool TryParseValueFromString(string? value, out bool? result, out string validationMessage)
	{
		validationMessage = string.Empty;
		result = value == False ? false : value == True ? true : null;
		return true;
	}

	private void OnClickOutside()
	{
		_show = false;
		StateHasChanged();
	}
}
