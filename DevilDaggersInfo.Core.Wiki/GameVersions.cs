namespace DevilDaggersInfo.Core.Wiki
{
	public static class GameVersions
	{
		private static readonly GameVersionFlags[] _gameVersions = (GameVersionFlags[])Enum.GetValues(typeof(GameVersionFlags));

		public static GameVersionFlags? GetGameVersionFromDate(DateTime dateTime)
		{
			for (int i = 0; i < _gameVersions.Length; i++)
			{
				if (dateTime >= GetReleaseDate(_gameVersions[i]) && (i == _gameVersions.Length - 1 || dateTime < GetReleaseDate(_gameVersions[i + 1])))
					return _gameVersions[i];
			}

			return null;
		}

		public static DateTime? GetReleaseDate(GameVersionFlags gameVersion) => gameVersion switch
		{
			GameVersionFlags.V1_0 => new(2016, 2, 18),
			GameVersionFlags.V2_0 => new(2016, 7, 5),
			GameVersionFlags.V3_0 => new(2016, 9, 19),
			GameVersionFlags.V3_1 => new(2021, 2, 19),
			_ => null,
		};
	}
}
