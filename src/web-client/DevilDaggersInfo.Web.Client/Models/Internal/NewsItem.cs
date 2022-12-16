namespace DevilDaggersInfo.Web.Client.Models.Internal;

public class NewsItem
{
	public required string Title { get; set; }

	public DateTime DateTime { get; set; }

	public string GetHtmlPath() => Path.Combine("news", $"{Title}.html");

	public string ToDisplayTitle() => $"{DateTime.ToString(StringFormats.DateFormat)} - {Title}";
}
