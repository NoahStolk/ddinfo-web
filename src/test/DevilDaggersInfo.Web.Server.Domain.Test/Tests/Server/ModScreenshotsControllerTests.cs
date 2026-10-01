using DevilDaggersInfo.Web.Server.Controllers.Main;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.Server;

internal sealed class ModScreenshotsControllerTests : IDisposable
{
	private readonly string _rootDirectory = Path.Combine(Path.GetTempPath(), $"ddinfo-{Guid.NewGuid():N}");
	private readonly ModScreenshotsController _controller;

	public ModScreenshotsControllerTests()
	{
		string screenshotsDirectory = Path.Combine(_rootDirectory, "Data", "ModScreenshots");
		Directory.CreateDirectory(Path.Combine(screenshotsDirectory, "mod"));
		File.WriteAllBytes(Path.Combine(screenshotsDirectory, "mod", "00.png"), [1, 2, 3]);
		File.WriteAllText(Path.Combine(_rootDirectory, "Data", "secret.json"), "secret");
		File.WriteAllText(Path.Combine(_rootDirectory, "appsettings.json"), "secret");

		IFileSystemService fileSystemService = Substitute.For<IFileSystemService>();
		fileSystemService.GetPath(DataSubDirectory.ModScreenshots).Returns(screenshotsDirectory);
		_controller = new ModScreenshotsController(fileSystemService);
	}

	public void Dispose()
	{
		Directory.Delete(_rootDirectory, true);
	}

	[Test]
	public async Task ReturnsScreenshot()
	{
		IActionResult result = await _controller.GetScreenshotByFilePath("mod", "00.png");
		await Assert.That(result).IsTypeOf<FileContentResult>();
	}

	[Test]
	[Arguments("mod", "missing.png")]
	[Arguments("..", "../appsettings.json")]
	[Arguments("..", "secret.json")]
	[Arguments("mod", "../../secret.json")]
	[Arguments("mod", "..\\..\\secret.json")]
	[Arguments("..", "..")]
	[Arguments("mod", "/etc/hostname")]
	[Arguments("mod", "C:secret.json")]
	[Arguments("", "00.png")]
	public async Task RejectsPathsOutsideScreenshotsDirectory(string modName, string fileName)
	{
		IActionResult result = await _controller.GetScreenshotByFilePath(modName, fileName);
		await Assert.That(result).IsTypeOf<NotFoundResult>();
	}
}
