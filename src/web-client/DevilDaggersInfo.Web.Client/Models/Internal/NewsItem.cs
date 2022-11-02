namespace DevilDaggersInfo.Web.Client.Models.Internal;

public class NewsItem
{
	public string Title { get; set; } = null!;

	public DateTime DateTime { get; set; }

	public string GetHtmlPath() => Path.Combine("news", $"{Title}.html");

	public string ToDisplayTitle() => $"{DateTime.ToString(StringFormats.DateFormat)} - {Title}";
}
