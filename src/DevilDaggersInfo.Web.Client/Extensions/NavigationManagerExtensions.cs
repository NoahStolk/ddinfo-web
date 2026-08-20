using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Extensions;

internal static class NavigationManagerExtensions
{
	extension(NavigationManager navigationManager)
	{
		public void AddOrModifyQueryParameter(string key, object? value)
		{
			navigationManager.AddOrModifyQueryParameters(new Dictionary<string, object?> { { key, value } });
		}

		public void AddOrModifyQueryParameters(KeyValuePair<string, object?>[] parameters)
		{
			navigationManager.AddOrModifyQueryParameters(parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
		}

		private void AddOrModifyQueryParameters(IReadOnlyDictionary<string, object?> parameters)
		{
			navigationManager.NavigateTo(navigationManager.GetUriWithQueryParameters(parameters));
		}
	}
}
