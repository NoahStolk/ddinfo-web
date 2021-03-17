using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.SitemapGenerator
{
	public static class SitemapUtils
	{
		private static readonly List<string> _pageNamesToExclude = new()
		{
			"GenerateSitemap",
			"Error",
		};

		private static readonly List<Type> _pageTypesToExclude = new();

		public static void ExcludePage(string page)
		{
			if (!_pageNamesToExclude.Contains(page))
				_pageNamesToExclude.Add(page);
		}

		public static void ExcludePage(Type pageType)
		{
			if (!_pageTypesToExclude.Contains(pageType))
				_pageTypesToExclude.Add(pageType);
		}

		public static string GetSitemap(IHttpContextAccessor httpContextAccessor, Type pageBaseType, bool recursive = false)
		{
			string baseUrl = httpContextAccessor.HttpContext?.Request.Host.Host ?? throw new("Could not get base URL for sitemap.");

			SitemapBuilder sitemapBuilder = new();

			foreach (Type type in Array.Find(AppDomain.CurrentDomain.GetAssemblies(), a => a.FullName?.Contains("Views") == true)?
				.GetTypes()
				.Where(t =>
					t.BaseType == pageBaseType &&
					t.Name.Contains("Pages_") &&
					!_pageTypesToExclude.Contains(t)) ?? Enumerable.Empty<Type>())
			{
				string pageFileName = type.Name.Replace("Pages_", string.Empty);
				if (!_pageNamesToExclude.Contains(pageFileName) && (recursive || !pageFileName.Contains('_')))
					sitemapBuilder.AddUrl($"{baseUrl}/{pageFileName.Replace('_', '/')}");
			}

			return sitemapBuilder.ToString();
		}
	}
}
