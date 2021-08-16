namespace DevilDaggersInfo.Core.Wiki;

public static class GameVersions
{
	private static readonly GameVersion[] _gameVersions = (GameVersion[])Enum.GetValues(typeof(GameVersion));

	public static GameVersion? GetGameVersionFromDate(DateTime dateTime)
	{
		for (int i = 0; i < _gameVersions.Length; i++)
		{
			if (dateTime >= GetReleaseDate(_gameVersions[i]) && (i == _gameVersions.Length - 1 || dateTime < GetReleaseDate(_gameVersions[i + 1])))
				return _gameVersions[i];
		}

		return null;
	}

	public static DateTime? GetReleaseDate(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => new(2016, 2, 18),
		GameVersion.V2_0 => new(2016, 7, 5),
		GameVersion.V3_0 => new(2016, 9, 19),
		GameVersion.V3_1 => new(2021, 2, 19),
		_ => null,
	};
}
