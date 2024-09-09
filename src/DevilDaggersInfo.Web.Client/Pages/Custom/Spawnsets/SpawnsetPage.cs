using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Core.Spawnset.View;
using DevilDaggersInfo.Core.Wiki;
using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;
using DevilDaggersInfo.Web.Client.HttpClients;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace DevilDaggersInfo.Web.Client.Pages.Custom.Spawnsets;

public partial class SpawnsetPage
{
	private const int _defaultWaveCount = 40;

	private bool _notFound;

	[Inject]
	public required MainApiHttpClient Http { get; set; }

	[Inject]
	public required NavigationManager NavigationManager { get; set; }

	[Parameter]
	[EditorRequired]
	public int Id { get; set; }

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
		SpawnsView = new SpawnsView(spawnsetBinary, GameConstants.CurrentVersion, GetSpawnset.MaxDisplayWaves ?? _defaultWaveCount);
		EffectivePlayerSettings = spawnsetBinary.GetEffectivePlayerSettings();
	}
}
