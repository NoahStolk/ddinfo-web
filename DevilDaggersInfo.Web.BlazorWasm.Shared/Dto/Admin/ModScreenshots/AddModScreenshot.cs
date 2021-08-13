using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.ModScreenshots
{
	public class AddModScreenshot
	{
		public string ModName { get; set; } = null!;

		[MaxLength(ModScreenshotConstants.MaxFileSize, ErrorMessage = ModScreenshotConstants.MaxFileSizeErrorMessage)]
		public byte[] FileContents { get; set; } = null!;
	}
}
