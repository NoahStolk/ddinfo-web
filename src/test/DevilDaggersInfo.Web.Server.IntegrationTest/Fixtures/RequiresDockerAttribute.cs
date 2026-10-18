using System.ComponentModel;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// Skips rather than fails when no Docker daemon is reachable, so that contributors without Docker can still run the
/// rest of the suite. In CI (the <c>CI</c> environment variable, which GitHub Actions sets) a missing daemon fails the
/// test instead, so a broken runner cannot go green by skipping every database test.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
internal sealed class RequiresDockerAttribute() : SkipAttribute("Docker is not available.")
{
	private static readonly Lazy<Task<bool>> _isAvailable = new(IsDockerAvailableAsync);

	public override async Task<bool> ShouldSkip(TestRegisteredContext context)
	{
		if (await _isAvailable.Value)
			return false;

		if (string.Equals(Environment.GetEnvironmentVariable("CI"), "true", StringComparison.OrdinalIgnoreCase))
			throw new InvalidOperationException("Docker is not available, but database tests must not be skipped in CI.");

		return true;
	}

	private static async Task<bool> IsDockerAvailableAsync()
	{
		try
		{
			using Process process = new()
			{
				StartInfo = new ProcessStartInfo("docker", "info")
				{
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
				},
			};

			process.Start();
			await process.WaitForExitAsync();
			return process.ExitCode == 0;
		}
		catch (Win32Exception)
		{
			// The docker executable is not installed or not on the PATH.
			return false;
		}
	}
}
