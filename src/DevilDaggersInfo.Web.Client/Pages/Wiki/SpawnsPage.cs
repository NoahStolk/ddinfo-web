using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.View;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Web.Client.Extensions;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages.Wiki;

public partial class SpawnsPage
{
	private const int _defaultWaveCount = 40;

	private readonly IReadOnlyList<GameVersion> _allowedGameVersions = new List<GameVersion> { GameVersion.V1_0, GameVersion.V2_0, GameVersion.V3_0 };

	private int _waveCount = _defaultWaveCount;
	private SpawnsetBinary? _spawnset;
	private SpawnsView? _spawnsView;

	[Parameter]
	[EditorRequired]
	public GameVersion GameVersion { get; set; } = GameVersion.V3_0;

	protected override async Task OnParametersSetAsync()
	{
		await Fetch(GameVersion);
	}

	private async Task Fetch(GameVersion gameVersion)
	{
		_waveCount = _defaultWaveCount;
		GameVersion = gameVersion;

		byte[] spawnsetBytes = await Http.GetDefaultSpawnset(GameVersion.ToMainApi());
		if (!SpawnsetBinary.TryParse(spawnsetBytes, out _spawnset))
			return;

		_spawnsView = new SpawnsView(_spawnset, GameVersion, _waveCount);
	}

	private void IncreaseWaveCount(int amount)
	{
		if (_spawnset == null)
			return;

		_waveCount += amount;
		_spawnsView = new SpawnsView(_spawnset, GameVersion, _waveCount);
	}

	private static string ReplaceLastOccurrenceOf(string source, string find, string replace)
	{
		int position = source.LastIndexOf(find);
		if (position == -1)
			return source;

		return source.Remove(position, find.Length).Insert(position, replace);
	}
}
