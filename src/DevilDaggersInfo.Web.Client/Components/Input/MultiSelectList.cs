using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Components.Input;

public partial class MultiSelectList
{
	private bool _show;

	private string SelectedDisplayValue => $"{CurrentValue?.Count ?? 0} selected";

	[Parameter]
	[EditorRequired]
	public IReadOnlyList<string>? Values { get; set; }

	[Parameter]
	public Action? OnToggleAction { get; set; }

	private void Toggle(string value)
	{
		CurrentValue ??= [];

		if (CurrentValue.Contains(value))
			CurrentValue.Remove(value);
		else
			CurrentValue.Add(value);

		OnToggleAction?.Invoke();
	}

	protected override bool TryParseValueFromString(string? value, out List<string> result, out string validationErrorMessage)
	{
		validationErrorMessage = string.Empty;
		result = value?.Split(',').ToList() ?? [];
		return true;
	}
}
