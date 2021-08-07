using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Client.Extensions
{
	public static class NavigationManagerExtensions
	{
		public static Dictionary<string, StringValues> GetQuery(this NavigationManager navigationManager)
			=> QueryHelpers.ParseQuery(navigationManager.ToAbsoluteUri(navigationManager.Uri).Query);
	}
}
