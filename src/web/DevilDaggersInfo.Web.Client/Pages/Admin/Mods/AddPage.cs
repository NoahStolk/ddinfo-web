using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using DevilDaggersInfo.Web.ApiSpec.Admin.Players;
using DevilDaggersInfo.Web.Client.Components.Admin;
using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Client.StateObjects.Admin.Mods;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Pages.Admin.Mods;

public partial class AddPage
{
	private Dictionary<int, string>? _playerNames;
	private readonly AddModState _addModState = new();
	private readonly List<string> _binaryFileSizeErrors = new();
	private readonly List<string> _screenshotFileSizeErrors = new();

	private async Task PopulateAsync(AdminAdd<AddModState, AddMod> addComponent)
	{
		try
		{
			List<GetPlayerName> getPlayerNames = await Http.GetPlayerNames();
			_playerNames = getPlayerNames.ToDictionary(m => m.Id, m => m.PlayerName);

			addComponent.State = ErrorState.None;
		}
		catch (HttpRequestException ex)
		{
			addComponent.ErrorMessage = ex.StatusCode.HasValue ? $"Error {(int)ex.StatusCode}: {ex.StatusCode}" : "An error occurred while sending the request.";

			addComponent.State = ErrorState.FatalError;
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	private async Task LoadBinaries(InputFileChangeEventArgs e)
	{
		Dictionary<string, byte[]> files = await GetFiles(e, ModConstants.BinaryMaxFiles, ModConstants.BinaryMaxFileSize, _binaryFileSizeErrors);
		_addModState.Binaries = files.Select(kvp => new BinaryDataState { Name = kvp.Key, Data = kvp.Value }).ToList();
	}

	private async Task LoadScreenshots(InputFileChangeEventArgs e)
	{
		_addModState.Screenshots = await GetFiles(e, ModConstants.ScreenshotMaxFiles, ModConstants.ScreenshotMaxFileSize, _screenshotFileSizeErrors);
	}
}
