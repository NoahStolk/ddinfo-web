namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

internal static class ServerProject
{
	private const string _projectFileName = "DevilDaggersInfo.Web.Server.csproj";

	/// <summary>
	/// The server project directory, which the test host uses as its content root. Microsoft.AspNetCore.Mvc.Testing can
	/// normally infer this from a generated attribute, but that machinery keys off the solution path and this repository
	/// uses the .slnx format, so the directory is resolved by walking up from the test output directory instead.
	/// </summary>
	internal static string Directory { get; } = Resolve();

	private static string Resolve()
	{
		DirectoryInfo? directory = new(AppContext.BaseDirectory);
		while (directory != null)
		{
			string candidate = Path.Combine(directory.FullName, "DevilDaggersInfo.Web.Server");
			if (File.Exists(Path.Combine(candidate, _projectFileName)))
				return candidate;

			directory = directory.Parent;
		}

		throw new InvalidOperationException($"Could not locate {_projectFileName} above '{AppContext.BaseDirectory}'.");
	}
}
