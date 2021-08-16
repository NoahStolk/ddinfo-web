namespace DevilDaggersInfo.Core.Wiki
{
	public static class GameVersionInfo
	{
		private static readonly GameVersions[] _gameVersions = (GameVersions[])Enum.GetValues(typeof(GameVersions));

		public static GameVersions? GetGameVersionFromDate(DateTime dateTime)
		{
			for (int i = 0; i < _gameVersions.Length; i++)
			{
				if (dateTime >= GetReleaseDate(_gameVersions[i]) && (i == _gameVersions.Length - 1 || dateTime < GetReleaseDate(_gameVersions[i + 1])))
					return _gameVersions[i];
			}

			return null;
		}

		public static DateTime? GetReleaseDate(GameVersions gameVersion) => gameVersion switch
		{
			GameVersions.V1_0 => new(2016, 2, 18),
			GameVersions.V2_0 => new(2016, 7, 5),
			GameVersions.V3_0 => new(2016, 9, 19),
			GameVersions.V3_1 => new(2021, 2, 19),
			_ => null,
		};
	}
}
