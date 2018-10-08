namespace DevilDaggersWebsite.Utils
{
	public static class LeaderboardHistoryUtils
	{
		public static string HistoryJsonFileNameToDate(string fileNameWithoutExtension)
		{
			string year = fileNameWithoutExtension.Substring(0, 4);
			string month = fileNameWithoutExtension.Substring(4, 2);
			string day = fileNameWithoutExtension.Substring(6, 2);
			string hour = fileNameWithoutExtension.Substring(8, 2);
			string minute = fileNameWithoutExtension.Substring(10, 2);

			return $"{year}-{month}-{day} {hour}:{minute}";
		}
	}
}