using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Extensions
{
	public static class NavigationManagerExtensions
	{
		public static Dictionary<string, StringValues> GetQuery(this NavigationManager navigationManager)
			=> QueryHelpers.ParseQuery(navigationManager.ToAbsoluteUri(navigationManager.Uri).Query);

		public static void AddOrModifyQueryParameter(this NavigationManager navigationManager, string key, object value)
		{
			Dictionary<string, StringValues> query = navigationManager.GetQuery();

			if (!query.ContainsKey(key))
			{
				navigationManager.NavigateTo(QueryHelpers.AddQueryString(navigationManager.Uri, key, value.ToString()));
			}
			else
			{
				string newUrl = navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);
				foreach (KeyValuePair<string, StringValues> kvp in query)
				{
					if (kvp.Key == key)
						newUrl = QueryHelpers.AddQueryString(newUrl, key, value.ToString());
					else
						newUrl = QueryHelpers.AddQueryString(newUrl, kvp.Key, kvp.Value);
				}

				navigationManager.NavigateTo(newUrl);
			}
		}
	}
}
