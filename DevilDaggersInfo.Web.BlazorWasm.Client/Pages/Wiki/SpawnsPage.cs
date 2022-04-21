using DevilDaggersInfo.Core.Spawnset.View;
using DevilDaggersInfo.Core.Spawnset;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Wiki;

public partial class SpawnsPage
{
	private const int _defaultWaveCount = 40;

	private readonly List<GameVersion> _allowedGameVersions = new() { GameVersion.V1_0, GameVersion.V2_0, GameVersion.V3_0 };

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

		byte[] spawnsetBytes = await Http.GetDefaultSpawnset(GameVersion);
		if (!SpawnsetBinary.TryParse(spawnsetBytes, out _spawnset))
			return;

		BuildSpawnsView(_spawnset);
	}

	private void BuildSpawnsView(SpawnsetBinary spawnset)
	{
		_spawnsView = new(spawnset, GameVersion, _waveCount);
	}

	private void IncreaseWaveCount(int amount)
	{
		if (_spawnset == null)
			return;

		_waveCount += amount;
		BuildSpawnsView(_spawnset);
	}
}
