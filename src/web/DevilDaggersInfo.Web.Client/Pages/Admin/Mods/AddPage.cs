using DevilDaggersInfo.Web.Client.Components.Admin;
using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Shared.Constants;
using DevilDaggersInfo.Web.Shared.Dto.Admin.Mods;
using DevilDaggersInfo.Web.Shared.Dto.Admin.Players;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Pages.Admin.Mods;

public partial class AddPage
{
	private Dictionary<int, string>? _playerNames;
	private AddMod _addMod = new();
	private AdminAdd<AddMod>? _addComponent;
	private List<string> _binaryFileSizeErrors = new();
	private List<string> _screenshotFileSizeErrors = new();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (_addComponent == null)
			return;

		try
		{
			List<GetPlayerName> getPlayerNames = await Http.GetPlayerNames();
			_playerNames = getPlayerNames.ToDictionary(m => m.Id, m => m.PlayerName);

			_addComponent.State = ErrorState.None;
		}
		catch (HttpRequestException ex)
		{
			if (ex.StatusCode.HasValue)
				_addComponent.ErrorMessage = $"Error {(int)ex.StatusCode}: {ex.StatusCode}";
			else
				_addComponent.ErrorMessage = "An error occurred while sending the request.";

			_addComponent.State = ErrorState.FatalError;
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	private async Task LoadBinaries(InputFileChangeEventArgs e)
	{
		_addMod.Binaries = await GetFiles(e, ModConstants.BinaryMaxFiles, ModConstants.BinaryMaxFileSize, _binaryFileSizeErrors);
	}

	private async Task LoadScreenshots(InputFileChangeEventArgs e)
	{
		_addMod.Screenshots = await GetFiles(e, ModConstants.ScreenshotMaxFiles, ModConstants.ScreenshotMaxFileSize, _screenshotFileSizeErrors);
	}
}
