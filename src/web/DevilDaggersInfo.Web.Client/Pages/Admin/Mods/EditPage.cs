using DevilDaggersInfo.Api.Admin.Mods;
using DevilDaggersInfo.Api.Admin.Players;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Client.Components.Admin;
using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Pages.Admin.Mods;

public partial class EditPage
{
	private Dictionary<int, string>? _playerNames;
	private EditMod _editMod = new();
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
			_editMod = new()
			{
				ModTypes = getMod.ModTypes.AsEnumerable().ToList(),
				HtmlDescription = getMod.HtmlDescription,
				IsHidden = getMod.IsHidden,
				Name = getMod.Name,
				PlayerIds = getMod.PlayerIds,
				TrailerUrl = getMod.TrailerUrl,
				Url = getMod.Url,
			};

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
		Dictionary<string, byte[]> files = await GetFiles(e, ModConstants.BinaryMaxFiles, ModConstants.BinaryMaxFileSize, _binaryFileSizeErrors);
		_editMod.Binaries = files.Select(kvp => new BinaryData { Name = kvp.Key, Data = kvp.Value }).ToList();
	}

	private async Task LoadScreenshots(InputFileChangeEventArgs e)
	{
		_editMod.Screenshots = await GetFiles(e, ModConstants.ScreenshotMaxFiles, ModConstants.ScreenshotMaxFileSize, _screenshotFileSizeErrors);
	}
}
