using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Mods
{
	public class AddModScreenshot
	{
		[MaxLength(ModScreenshotConstants.MaxFileSize, ErrorMessage = ModScreenshotConstants.MaxFileSizeErrorMessage)]
		public byte[] FileContents { get; set; } = null!;
	}
}
