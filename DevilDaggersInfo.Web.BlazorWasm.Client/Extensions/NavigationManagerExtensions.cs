using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;

public static class NavigationManagerExtensions
{
	public static void AddOrModifyQueryParameter(this NavigationManager navigationManager, string key, object? value)
		=> navigationManager.AddOrModifyQueryParameters(new Dictionary<string, object?>() { { key, value } });

	public static void AddOrModifyQueryParameters(this NavigationManager navigationManager, params KeyValuePair<string, object?>[] parameters)
		=> navigationManager.AddOrModifyQueryParameters(parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));

	public static void AddOrModifyQueryParameters(this NavigationManager navigationManager, IReadOnlyDictionary<string, object?> parameters)
		=> navigationManager.NavigateTo(navigationManager.GetUriWithQueryParameters(parameters));
}
