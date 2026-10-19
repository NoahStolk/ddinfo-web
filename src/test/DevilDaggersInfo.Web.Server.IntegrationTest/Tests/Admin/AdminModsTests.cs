using DevilDaggersInfo.Core.Mod.Builders;
using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Tests.Admin;

/// <summary>
/// Mods uploaded through the Admin API. Unlike the other entities, a mod's binaries and screenshots live in the data
/// directory rather than the database, so these tests also assert on the files, and read them back through Main (the
/// website) and Ddae (Devil Daggers Asset Editor).
/// </summary>
[NotInParallel(nameof(AdminModsTests))]
internal sealed class AdminModsTests : ApplicationTest
{
	private static readonly byte[] _png = File.ReadAllBytes(Path.Combine("Resources", "Textures", "green.png"));

	// ! Assigned by the before-test hook, which TUnit runs before every test method.
	private HttpClient _client = null!;

	[Before(HookType.Test)]
	public async Task CreateAdminClientAsync()
	{
		await App.SeedAsync(dbContext => dbContext.Players.Add(EntityFixtures.Player(1, "Modder")));
		_client = App.CreateApiClient(await App.CreateJwtAsync("mod-admin", Roles.Mods));
	}

	[After(HookType.Test)]
	public void DisposeAdminClient()
	{
		_client.Dispose();
	}

	/// <summary>
	/// A minimal valid DD mod binary: one object binding, which is enough for the archive validation to accept it.
	/// </summary>
	private static BinaryData Binary(string name)
	{
		DdModBinaryBuilder builder = new();
		builder.AddObjectBinding($"{name}-binding", [.. "shader = \"boid\""u8]);
		return new BinaryData { Name = name, Data = builder.Compile() };
	}

	private static AddMod CreateAddMod(string name, List<BinaryData>? binaries = null, Dictionary<string, byte[]>? screenshots = null)
	{
		return new AddMod
		{
			Name = name,
			IsHidden = false,
			TrailerUrl = null,
			HtmlDescription = null,
			ModTypes = null,
			Url = null,
			PlayerIds = [1],
			Binaries = binaries ?? [],
			Screenshots = screenshots ?? [],
		};
	}

	private static EditMod CreateEditMod(string name, List<string>? binariesToDelete = null, List<BinaryData>? binaries = null, List<string>? screenshotsToDelete = null)
	{
		return new EditMod
		{
			Name = name,
			IsHidden = false,
			TrailerUrl = null,
			HtmlDescription = null,
			ModTypes = null,
			Url = null,
			PlayerIds = [1],
			BinariesToDelete = binariesToDelete ?? [],
			Binaries = binaries ?? [],
			ScreenshotsToDelete = screenshotsToDelete ?? [],
			Screenshots = [],
		};
	}

	private async Task AddAsync(AddMod addMod)
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/mods", addMod);
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await response.Content.ReadAsStringAsync());
	}

	private async Task<(string[] BinaryNames, string[] ScreenshotNames)> GetAdminModAsync(int id)
	{
		using HttpResponseMessage response = await _client.GetAsync($"api/admin/mods/{id}");
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);

		using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
		return (ReadStrings(document.RootElement.GetProperty("binaryNames")), ReadStrings(document.RootElement.GetProperty("screenshotNames")));

		static string[] ReadStrings(JsonElement element)
		{
			return element.ValueKind == JsonValueKind.Null ? [] : [.. element.EnumerateArray().Select(e => e.GetString() ?? string.Empty).Order(StringComparer.Ordinal)];
		}
	}

	private async Task<bool> FileExistsAsync(DataSubDirectory directory, string name)
	{
		await using AsyncServiceScope scope = App.CreateScope();
		return await scope.ServiceProvider.GetRequiredService<IFileSystem>().ExistsAsync(directory, name);
	}

	[Test]
	public async Task Add_WritesTheArchiveAndScreenshots_AndIsServedByMainAndDdae()
	{
		await AddAsync(CreateAddMod("AdminMod", [Binary("main")], new Dictionary<string, byte[]>(StringComparer.Ordinal) { ["first"] = _png }));

		await Assert.That(await FileExistsAsync(DataSubDirectory.Mods, "AdminMod.zip")).IsTrue();
		await Assert.That(await FileExistsAsync(DataSubDirectory.ModScreenshots, "AdminMod/00.png")).IsTrue();

		(string[] binaryNames, string[] screenshotNames) = await GetAdminModAsync(1);
		await Assert.That(binaryNames).IsEquivalentTo(["dd-AdminMod-main"], CollectionOrdering.Matching);
		await Assert.That(screenshotNames).IsEquivalentTo(["00.png"], CollectionOrdering.Matching);

		using HttpClient anonymous = App.CreateApiClient();

		using (HttpResponseMessage file = await anonymous.GetAsync("api/mods/AdminMod/file"))
		{
			await Assert.That(file.StatusCode).IsEqualTo(HttpStatusCode.OK);
			await using Stream zipStream = await file.Content.ReadAsStreamAsync();
			await using ZipArchive archive = new(zipStream, ZipArchiveMode.Read);
			await Assert.That(archive.Entries.Select(e => e.Name)).IsEquivalentTo(["dd-AdminMod-main"], CollectionOrdering.Matching);
		}

		using (HttpResponseMessage screenshot = await anonymous.GetAsync("api/mod-screenshots?modName=AdminMod&fileName=00.png"))
		{
			await Assert.That(screenshot.StatusCode).IsEqualTo(HttpStatusCode.OK);
			await Assert.That(await screenshot.Content.ReadAsByteArrayAsync()).IsEquivalentTo(_png, CollectionOrdering.Matching);
		}

		using HttpResponseMessage ddae = await anonymous.GetAsync("api/ddae/mods");
		await Assert.That(await ddae.Content.ReadAsStringAsync()).Contains("AdminMod");
	}

	[Test]
	public async Task Add_SkipsScreenshotsThatAreNotPng()
	{
		await AddAsync(CreateAddMod("AdminMod", screenshots: new Dictionary<string, byte[]>(StringComparer.Ordinal) { ["text"] = "not an image"u8.ToArray(), ["image"] = _png }));

		(_, string[] screenshotNames) = await GetAdminModAsync(1);
		await Assert.That(screenshotNames).IsEquivalentTo(["00.png"], CollectionOrdering.Matching);
	}

	[Test]
	public async Task Add_RejectsAModWithoutAuthors()
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/mods", CreateAddMod("AdminMod") with { PlayerIds = [] });

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Mod must have at least one author.");
	}

	[Test]
	public async Task Add_RejectsANameThatIsTaken()
	{
		await AddAsync(CreateAddMod("AdminMod"));

		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/mods", CreateAddMod("adminmod"));
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "already exists");
	}

	[Test]
	public async Task Add_RejectsDuplicateBinaryNames()
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/mods", CreateAddMod("AdminMod", [Binary("main"), Binary("main")]));

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Binary names must all be unique.");
		await Assert.That(await FileExistsAsync(DataSubDirectory.Mods, "AdminMod.zip")).IsFalse();
	}

	[Test]
	public async Task Add_RejectsABinaryThatIsNotAModBinary()
	{
		using HttpResponseMessage response = await _client.PostAsJsonAsync("api/admin/mods", CreateAddMod("AdminMod", [new BinaryData { Name = "broken", Data = [1, 2, 3, 4] }]));

		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest).Because(await response.Content.ReadAsStringAsync());
		await Assert.That(await FileExistsAsync(DataSubDirectory.Mods, "AdminMod.zip")).IsFalse();
	}

	[Test]
	public async Task Edit_RenameMovesTheArchiveAndScreenshots()
	{
		await AddAsync(CreateAddMod("Before", [Binary("main")], new Dictionary<string, byte[]>(StringComparer.Ordinal) { ["first"] = _png }));

		using (HttpResponseMessage edited = await _client.PutAsJsonAsync("api/admin/mods/1", CreateEditMod("After")))
			await Assert.That(edited.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await edited.Content.ReadAsStringAsync());

		await Assert.That(await FileExistsAsync(DataSubDirectory.Mods, "Before.zip")).IsFalse();
		await Assert.That(await FileExistsAsync(DataSubDirectory.Mods, "After.zip")).IsTrue();
		await Assert.That(await FileExistsAsync(DataSubDirectory.ModScreenshots, "Before/00.png")).IsFalse();
		await Assert.That(await FileExistsAsync(DataSubDirectory.ModScreenshots, "After/00.png")).IsTrue();

		// The binary names carry the mod name, so they are rewritten too.
		(string[] binaryNames, _) = await GetAdminModAsync(1);
		await Assert.That(binaryNames).IsEquivalentTo(["dd-After-main"], CollectionOrdering.Matching);
	}

	[Test]
	public async Task Edit_AddsAndDeletesBinariesAndScreenshots()
	{
		await AddAsync(CreateAddMod("AdminMod", [Binary("main")], new Dictionary<string, byte[]>(StringComparer.Ordinal) { ["first"] = _png }));

		EditMod editMod = CreateEditMod("AdminMod", binariesToDelete: ["dd-AdminMod-main"], binaries: [Binary("second")], screenshotsToDelete: ["00.png"]);
		using (HttpResponseMessage edited = await _client.PutAsJsonAsync("api/admin/mods/1", editMod))
			await Assert.That(edited.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await edited.Content.ReadAsStringAsync());

		(string[] binaryNames, string[] screenshotNames) = await GetAdminModAsync(1);
		await Assert.That(binaryNames).IsEquivalentTo(["dd-AdminMod-second"], CollectionOrdering.Matching);
		await Assert.That(screenshotNames).IsEmpty();
	}

	[Test]
	public async Task Edit_RejectsAddingABinaryThatAlreadyExists()
	{
		await AddAsync(CreateAddMod("AdminMod", [Binary("main")]));

		using HttpResponseMessage response = await _client.PutAsJsonAsync("api/admin/mods/1", CreateEditMod("AdminMod", binaries: [Binary("main")]));
		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "exact same names");
	}

	/// <summary>
	/// The new archive is validated before the original one is removed. An audio binary whose loudness asset is cut off
	/// passes the early binary type check and is only rejected while the archive is validated, which used to happen after
	/// the original archive had already been deleted.
	/// </summary>
	[Test]
	public async Task Edit_RejectedEdit_KeepsTheOriginalArchive()
	{
		await AddAsync(CreateAddMod("AdminMod", [Binary("main")]));

		AudioModBinaryBuilder audio = new();
		audio.AddAudio("boid", new byte[64], 1.0f);
		byte[] truncated = audio.Compile()[..^3];

		using (HttpResponseMessage response = await _client.PutAsJsonAsync("api/admin/mods/1", CreateEditMod("AdminMod", binaries: [new BinaryData { Name = "truncated", Data = truncated }])))
			await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.BadRequest, "Processing the mod archive failed.");

		using HttpClient anonymous = App.CreateApiClient();
		using HttpResponseMessage file = await anonymous.GetAsync("api/mods/AdminMod/file");
		await Assert.That(file.StatusCode).IsEqualTo(HttpStatusCode.OK);

		(string[] binaryNames, _) = await GetAdminModAsync(1);
		await Assert.That(binaryNames).IsEquivalentTo(["dd-AdminMod-main"], CollectionOrdering.Matching);
	}

	/// <summary>
	/// A mod can be created without binaries, so removing all of them should leave it without an archive rather than
	/// be refused.
	/// </summary>
	[Test]
	public async Task Edit_DeletingEveryBinary_LeavesTheModWithoutAnArchive()
	{
		await AddAsync(CreateAddMod("AdminMod", [Binary("main")]));

		using (HttpResponseMessage edited = await _client.PutAsJsonAsync("api/admin/mods/1", CreateEditMod("AdminMod", binariesToDelete: ["dd-AdminMod-main"])))
			await Assert.That(edited.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await edited.Content.ReadAsStringAsync());

		(string[] binaryNames, _) = await GetAdminModAsync(1);
		await Assert.That(binaryNames).IsEmpty();
		await Assert.That(await FileExistsAsync(DataSubDirectory.Mods, "AdminMod.zip")).IsFalse();
	}

	[Test]
	public async Task Edit_RenamesAModThatHasNoArchive()
	{
		await AddAsync(CreateAddMod("Before"));

		using (HttpResponseMessage edited = await _client.PutAsJsonAsync("api/admin/mods/1", CreateEditMod("After")))
			await Assert.That(edited.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(await edited.Content.ReadAsStringAsync());

		await using AsyncServiceScope scope = App.CreateScope();
		await Assert.That(await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Mods.Select(m => m.Name).SingleAsync()).IsEqualTo("After");
	}

	[Test]
	public async Task Edit_RejectsABinaryToDeleteThatIsNotAFullBinaryName()
	{
		await AddAsync(CreateAddMod("AdminMod", [Binary("main")]));

		using HttpResponseMessage response = await _client.PutAsJsonAsync("api/admin/mods/1", CreateEditMod("AdminMod", binariesToDelete: ["main"]));
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest).Because(await response.Content.ReadAsStringAsync());
		await Assert.That(await FileExistsAsync(DataSubDirectory.Mods, "AdminMod.zip")).IsTrue();
	}

	[Test]
	public async Task Edit_IsNotFoundForAnUnknownId()
	{
		using HttpResponseMessage response = await _client.PutAsJsonAsync("api/admin/mods/999", CreateEditMod("Anything"));

		await ProblemAssertions.AssertProblemAsync(response, HttpStatusCode.NotFound, "Mod with ID '999' does not exist.");
	}

	[Test]
	public async Task Delete_RemovesTheArchiveScreenshotsAndAuthorLinks()
	{
		await AddAsync(CreateAddMod("AdminMod", [Binary("main")], new Dictionary<string, byte[]>(StringComparer.Ordinal) { ["first"] = _png }));

		using (HttpResponseMessage deleted = await _client.DeleteAsync("api/admin/mods/1"))
			await Assert.That(deleted.StatusCode).IsEqualTo(HttpStatusCode.OK);

		await Assert.That(await FileExistsAsync(DataSubDirectory.Mods, "AdminMod.zip")).IsFalse();
		await Assert.That(await FileExistsAsync(DataSubDirectory.ModScreenshots, "AdminMod/00.png")).IsFalse();

		await using AsyncServiceScope scope = App.CreateScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await Assert.That(await dbContext.Mods.CountAsync()).IsEqualTo(0);
		await Assert.That(await dbContext.PlayerMods.CountAsync()).IsEqualTo(0);
	}
}
