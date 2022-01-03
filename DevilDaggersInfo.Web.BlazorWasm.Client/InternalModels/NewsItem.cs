namespace DevilDaggersInfo.Web.BlazorWasm.Client.InternalModels;

public class NewsItem
{
	public string Title { get; set; } = null!;

	public DateTime DateTime { get; set; }

	public string GetHtmlPath() => Path.Combine("news", $"{Title}.html");
}
