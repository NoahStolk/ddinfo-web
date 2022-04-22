using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.View;
using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Shared.Dto.Public.Spawnsets;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Pages.Custom.Spawnsets;

public partial class SpawnsetPage
{
	private const int _defaultWaveCount = 40;

	private bool _notFound;

	[Inject] public PublicApiHttpClient Http { get; set; } = null!;
	[Inject] public NavigationManager NavigationManager { get; set; } = null!;

	[Parameter, EditorRequired] public int Id { get; set; }

	public GetSpawnset? GetSpawnset { get; set; }

	public SpawnsetBinary? SpawnsetBinary { get; set; }
	public SpawnsView? SpawnsView { get; set; }
	public EffectivePlayerSettings EffectivePlayerSettings { get; set; }

	protected override async Task OnParametersSetAsync()
	{
		try
		{
			GetSpawnset = await Http.GetSpawnsetById(Id);
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			_notFound = true;
			return;
		}

		if (!SpawnsetBinary.TryParse(GetSpawnset.FileBytes, out SpawnsetBinary? spawnsetBinary))
		{
			// TODO: Log error.
			return;
		}

		SpawnsetBinary = spawnsetBinary;
		SpawnsView = new(spawnsetBinary, GameConstants.CurrentVersion, GetSpawnset.MaxDisplayWaves ?? _defaultWaveCount);
		EffectivePlayerSettings = spawnsetBinary.GetEffectivePlayerSettings();
	}
}
