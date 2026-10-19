using JetBrains.Annotations;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// Compares a JSON response with an approved snapshot under <c>Contracts/{api}/{name}.json</c> in the test project's
/// source directory. The external consumers of these APIs cannot be redeployed with the server, so the exact response --
/// property names and casing, value types, nulls, and the encoding of enums, dates and units -- is the contract. Keeping
/// it in an approved file makes every change to it show up as a diff in review.
/// <para>
/// A missing or different snapshot fails the test and writes the actual response next to it as <c>{name}.received.json</c>.
/// To accept a change, review that file and rename it over the approved one. Nothing is ever approved automatically.
/// </para>
/// </summary>
internal static class ContractSnapshot
{
	private static readonly JsonSerializerOptions _formatting = new()
	{
		WriteIndented = true,
		IndentCharacter = '\t',
		IndentSize = 1,

		// Keep characters such as '<' and '+' readable in the approved files. This only affects the formatting of the file,
		// not the comparison, which parses the response first.
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
	};

	private static string SnapshotDirectory { get; } = Path.Combine(TestProject.Directory, "Contracts");

	/// <summary>
	/// Asserts that the response is a successful JSON response whose body matches the approved snapshot.
	/// </summary>
	[AssertionMethod]
	internal static async Task AssertMatchesAsync(HttpResponseMessage response, string api, string name)
	{
		string body = await response.Content.ReadAsStringAsync();
		await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK).Because(body);
		await Assert.That(response.Content.Headers.ContentType?.MediaType).IsEqualTo("application/json");

		await AssertJsonMatchesAsync(body, api, name);
	}

	/// <summary>
	/// Asserts that the JSON matches the approved snapshot. For a test that reduces a response before comparing it.
	/// </summary>
	[AssertionMethod]
	internal static async Task AssertJsonMatchesAsync(string json, string api, string name)
	{
		string actual = Format(json);
		string approvedPath = Path.Combine(SnapshotDirectory, api, $"{name}.json");
		string receivedPath = Path.Combine(SnapshotDirectory, api, $"{name}.received.json");

		string? approved = File.Exists(approvedPath) ? (await File.ReadAllTextAsync(approvedPath)).ReplaceLineEndings("\n") : null;
		if (approved == actual)
		{
			File.Delete(receivedPath);
			return;
		}

		Directory.CreateDirectory(Path.GetDirectoryName(receivedPath) ?? SnapshotDirectory);
		await File.WriteAllTextAsync(receivedPath, actual);

		string reason = approved == null
			? $"There is no approved snapshot at {approvedPath}. Review {receivedPath} and rename it to approve it."
			: $"The response differs from the approved snapshot at {approvedPath}. Compare it with {receivedPath}, and rename that over the approved file if the change is intended.";
		await Assert.That(actual).IsEqualTo(approved).Because(reason);
	}

	private static string Format(string json)
	{
		JsonNode? node = JsonNode.Parse(json);
		return (node?.ToJsonString(_formatting) ?? "null") + "\n";
	}
}
