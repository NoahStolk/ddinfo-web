using DevilDaggersInfo.Api.Admin.Mods;
using DevilDaggersInfo.Api.Admin.Players;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Client.Components.Admin;
using DevilDaggersInfo.Web.Client.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Pages.Admin.Mods;

public partial class EditPage
{
	private Dictionary<int, string>? _playerNames;
	private readonly EditMod _editMod = new();
	private List<string> _binaryFileSizeErrors = new();
	private List<string> _screenshotFileSizeErrors = new();

	[Parameter, EditorRequired] public int Id { get; set; }

	private IReadOnlyList<string>? _binaryNames;
	private IReadOnlyList<string>? _screenshotNames;

	private async Task PopulateAsync(AdminEdit<EditMod> editComponent)
	{
		try
		{
			List<GetPlayerName> getPlayerNames = await Http.GetPlayerNames();
			_playerNames = getPlayerNames.ToDictionary(m => m.Id, m => m.PlayerName);

			GetMod getMod = await Http.GetModById(Id);
			_editMod.ModTypes = getMod.ModTypes.AsEnumerable().ToList();
			_editMod.HtmlDescription = getMod.HtmlDescription;
			_editMod.IsHidden = getMod.IsHidden;
			_editMod.Name = getMod.Name;
			_editMod.PlayerIds = getMod.PlayerIds;
			_editMod.TrailerUrl = getMod.TrailerUrl;
			_editMod.Url = getMod.Url;

			_binaryNames = getMod.BinaryNames;
			_screenshotNames = getMod.ScreenshotNames;

			editComponent.State = ErrorState.None;
		}
		catch (HttpRequestException ex)
		{
			if (ex.StatusCode.HasValue)
				editComponent.ErrorMessage = $"Error {(int)ex.StatusCode}: {ex.StatusCode}";
			else
				editComponent.ErrorMessage = "An error occurred while sending the request.";

			editComponent.State = ErrorState.FatalError;
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	private async Task LoadBinaries(InputFileChangeEventArgs e)
	{
		Dictionary<string, byte[]> files = await GetFiles(e, Constants.BinaryMaxFiles, Constants.BinaryMaxFileSize, _binaryFileSizeErrors);
		_editMod.Binaries = files.Select(kvp => new BinaryData { Name = kvp.Key, Data = kvp.Value }).ToList();
	}

	private async Task LoadScreenshots(InputFileChangeEventArgs e)
	{
		_editMod.Screenshots = await GetFiles(e, Constants.ScreenshotMaxFiles, Constants.ScreenshotMaxFileSize, _screenshotFileSizeErrors);
	}
}
