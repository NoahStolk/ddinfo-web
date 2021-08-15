using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;

public static class NavigationManagerExtensions
{
	public static void AddOrModifyQueryParameter(this NavigationManager navigationManager, string key, object value)
		=> AddOrModifyQueryParameter(navigationManager, new KeyValuePair<string, object>(key, value));

	public static void AddOrModifyQueryParameter(this NavigationManager navigationManager, params KeyValuePair<string, object>[] parameters)
	{
		Dictionary<string, StringValues> query = QueryHelpers.ParseQuery(navigationManager.ToAbsoluteUri(navigationManager.Uri).Query);
		foreach (KeyValuePair<string, object> kvp in parameters)
			query[kvp.Key] = kvp.Value.ToString();

		string newUrl = navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);
		foreach (KeyValuePair<string, StringValues> kvp in query)
			newUrl = QueryHelpers.AddQueryString(newUrl, kvp.Key, kvp.Value);

		navigationManager.NavigateTo(newUrl);
	}
}
