using DevilDaggersInfo.Core.Common;

namespace DevilDaggersInfo.Web.Client.Models.Internal;

public class NewsItem
{
	public required string Title { get; init; }

	public DateOnly DateTime { get; init; }

	public string GetHtmlPath()
	{
		return Path.Combine("news", $"{Title}.html");
	}

	public string ToDisplayTitle()
	{
		return $"{DateTime.ToString(StringFormats.DateFormat)} - {Title}";
	}
}
