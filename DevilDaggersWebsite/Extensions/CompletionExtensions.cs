using DevilDaggersWebsite.LeaderboardHistory;

namespace DevilDaggersWebsite.Extensions
{
	public static class CompletionExtensions
	{
		public static string ToHtmlString(this CompletionEntry ce) => ce switch
		{
			CompletionEntry.Missing => "<span style='color: #f00'>(Missing)</span>",
			_ => string.Empty,
		};

		public static string ToHtmlString(this CompletionEntryCombined cec) => cec switch
		{
			CompletionEntryCombined.PartiallyMissing => "<span style='color: #f80'>(Partially missing)</span>",
			CompletionEntryCombined.Missing => "<span style='color: #f00'>(Missing)</span>",
			_ => string.Empty,
		};
	}
}
