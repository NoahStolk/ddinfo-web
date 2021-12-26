using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components.Forms;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Admin.Mods;

public abstract class BaseAdminModPage : BaseAdminPage
{
	protected IReadOnlyDictionary<int, string> ModTypes { get; } = Enum.GetValues<ModTypes>().ToDictionary(e => (int)e, e => e.ToString());

	protected static async Task<Dictionary<string, byte[]>> GetFiles(InputFileChangeEventArgs e, int maximumFileCount, long maxAllowedSize, List<string> errorList)
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
