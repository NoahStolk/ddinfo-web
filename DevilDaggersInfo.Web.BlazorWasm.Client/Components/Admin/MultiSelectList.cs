using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;

public partial class MultiSelectList
{
	private bool _show = false;

	private string SelectedDisplayValue => $"{CurrentValue?.Count ?? 0} selected";

	[Parameter, EditorRequired] public IReadOnlyList<string>? Values { get; set; }
	[Parameter] public Action? OnToggleAction { get; set; }

	private void Toggle(string value)
	{
		CurrentValue ??= new();

		if (CurrentValue.Contains(value))
			CurrentValue.Remove(value);
		else
			CurrentValue.Add(value);

		if (OnToggleAction != null)
			OnToggleAction.Invoke();
	}

	protected override bool TryParseValueFromString(string? value, out List<string>? result, out string validationMessage)
	{
		validationMessage = string.Empty;
		result = value?.Split(',').ToList();
		return true;
	}
}
