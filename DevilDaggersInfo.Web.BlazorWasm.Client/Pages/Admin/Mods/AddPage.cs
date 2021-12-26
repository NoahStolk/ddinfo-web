using DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Constants;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Admin.Mods;

public partial class AddPage
{
	private Dictionary<int, string>? _playerNames;
	private AddMod _addMod = new();
	private AdminAdd<AddMod> _addComponent = null!;
	private Dictionary<int, string> _modTypes = Enum.GetValues<ModTypes>().ToDictionary(e => (int)e, e => e.ToString());
	private List<string> _binaryFileSizeErrors = new();
	private List<string> _screenshotFileSizeErrors = new();

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

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
		=> _addMod.Binaries = await GetFiles(e, ModConstants.BinaryMaxFiles, ModConstants.BinaryMaxFileSize, _binaryFileSizeErrors);

	private async Task LoadScreenshots(InputFileChangeEventArgs e)
		=> _addMod.Screenshots = await GetFiles(e, ModConstants.ScreenshotMaxFiles, ModConstants.ScreenshotMaxFileSize, _screenshotFileSizeErrors);

	private static async Task<Dictionary<string, byte[]>> GetFiles(InputFileChangeEventArgs e, int maximumFileCount, long maxAllowedSize, List<string> errorList)
	{
		errorList.Clear();
		IReadOnlyList<IBrowserFile> browserFiles = e.GetMultipleFiles(maximumFileCount);

		Dictionary<string, byte[]> files = new();
		foreach (IBrowserFile browserFile in browserFiles)
		{
			using MemoryStream ms = new();

			try
			{
				await browserFile.OpenReadStream(maxAllowedSize).CopyToAsync(ms);
				files.Add(browserFile.Name, ms.ToArray());
			}
			catch (IOException)
			{
				errorList.Add($"File {browserFile.Name} cannot be larger than {FileSizeUtils.Format(maxAllowedSize)}.");
			}
		}

		return files;
	}
}
