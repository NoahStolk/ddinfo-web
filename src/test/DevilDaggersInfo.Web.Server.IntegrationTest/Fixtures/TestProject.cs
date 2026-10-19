namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

internal static class TestProject
{
	private const string _projectFileName = "DevilDaggersInfo.Web.Server.IntegrationTest.csproj";

	/// <summary>
	/// This test project's source directory, found by walking up from the test output directory. Approved contract
	/// snapshots are read from here rather than copied to the output, so that a received snapshot lands next to them.
	/// </summary>
	internal static string Directory { get; } = Resolve();

	private static string Resolve()
	{
		DirectoryInfo? directory = new(AppContext.BaseDirectory);
		while (directory != null)
		{
			if (File.Exists(Path.Combine(directory.FullName, _projectFileName)))
				return directory.FullName;

			directory = directory.Parent;
		}

		throw new InvalidOperationException($"Could not locate {_projectFileName} above '{AppContext.BaseDirectory}'.");
	}
}
