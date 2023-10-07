using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using DevilDaggersInfo.Web.ApiSpec.Admin.Players;
using DevilDaggersInfo.Web.Client.Components.Admin;
using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Client.StateObjects.Admin.Mods;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Pages.Admin.Mods;

public partial class EditPage
{
	private Dictionary<int, string>? _playerNames;
	private readonly EditModState _editModState = new();
	private readonly List<string> _binaryFileSizeErrors = new();
	private readonly List<string> _screenshotFileSizeErrors = new();

	[Parameter, EditorRequired] public int Id { get; set; }

	private IReadOnlyList<string>? _binaryNames;
	private IReadOnlyList<string>? _screenshotNames;

	private async Task PopulateAsync(AdminEdit<EditModState, EditMod> editComponent)
	{
		try
		{
			List<GetPlayerName> getPlayerNames = await Http.GetPlayerNames();
			_playerNames = getPlayerNames.ToDictionary(m => m.Id, m => m.PlayerName);

			GetMod getMod = await Http.GetModById(Id);
			_editModState.ModTypes = getMod.ModTypes.AsEnumerable().ToList();
			_editModState.HtmlDescription = getMod.HtmlDescription;
			_editModState.IsHidden = getMod.IsHidden;
			_editModState.Name = getMod.Name;
			_editModState.PlayerIds = getMod.PlayerIds;
			_editModState.TrailerUrl = getMod.TrailerUrl;
			_editModState.Url = getMod.Url;

			_binaryNames = getMod.BinaryNames;
			_screenshotNames = getMod.ScreenshotNames;

			editComponent.State = ErrorState.None;
		}
		catch (HttpRequestException ex)
		{
			editComponent.ErrorMessage = ex.StatusCode.HasValue ? $"Error {(int)ex.StatusCode}: {ex.StatusCode}" : "An error occurred while sending the request.";

			editComponent.State = ErrorState.FatalError;
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	private async Task LoadBinaries(InputFileChangeEventArgs e)
	{
		Dictionary<string, byte[]> files = await GetFiles(e, ModConstants.BinaryMaxFiles, ModConstants.BinaryMaxFileSize, _binaryFileSizeErrors);
		_editModState.Binaries = files.Select(kvp => new BinaryDataState { Name = kvp.Key, Data = kvp.Value }).ToList();
	}

	private async Task LoadScreenshots(InputFileChangeEventArgs e)
	{
		_editModState.Screenshots = await GetFiles(e, ModConstants.ScreenshotMaxFiles, ModConstants.ScreenshotMaxFileSize, _screenshotFileSizeErrors);
	}
}
