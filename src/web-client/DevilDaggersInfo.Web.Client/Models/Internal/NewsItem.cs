using DevilDaggersInfo.Common;

namespace DevilDaggersInfo.Web.Client.Models.Internal;

public class NewsItem
{
	public required string Title { get; init; }

	public DateTime DateTime { get; init; }

	public string GetHtmlPath() => Path.Combine("news", $"{Title}.html");

	public string ToDisplayTitle() => $"{DateTime.ToString(StringFormats.DateFormat)} - {Title}";
}
