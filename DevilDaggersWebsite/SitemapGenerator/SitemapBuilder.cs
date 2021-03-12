using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DevilDaggersWebsite.SitemapGenerator
{
	public class SitemapBuilder
	{
		private readonly XNamespace _ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

		private readonly List<SitemapUrl> _urls = new();

		public void AddUrl(string url, DateTime? modified = null, ChangeFrequency? changeFrequency = null, double? priority = null)
		{
			_urls.Add(new SitemapUrl
			{
				Url = url,
				Modified = modified,
				ChangeFrequency = changeFrequency,
				Priority = priority,
			});
		}

		private XElement CreateItemElement(SitemapUrl sitemapUrl)
		{
			XElement itemElement = new(_ns + "url", new XElement(_ns + "loc", sitemapUrl.Url.ToLower()));

			if (sitemapUrl.Modified.HasValue)
				itemElement.Add(new XElement(_ns + "lastmod", sitemapUrl.Modified.Value.ToString("yyyy-MM-ddTHH:mm:ss.fzzz")));

			if (sitemapUrl.ChangeFrequency.HasValue)
				itemElement.Add(new XElement(_ns + "changefreq", sitemapUrl.ChangeFrequency.Value.ToString().ToLower()));

			if (sitemapUrl.Priority.HasValue)
				itemElement.Add(new XElement(_ns + "priority", sitemapUrl.Priority.Value.ToString("N1")));

			return itemElement;
		}

		public override string ToString()
		{
			XDocument sitemap = new(new XDeclaration("1.0", "utf-8", "yes"), new XElement(_ns + "urlset", _urls.Select(url => CreateItemElement(url))));

			return $"{sitemap.Declaration}\n{sitemap}";
		}
	}
}
