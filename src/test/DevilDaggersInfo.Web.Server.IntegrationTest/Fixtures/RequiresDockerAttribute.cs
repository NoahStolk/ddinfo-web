using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// Skips rather than fails when no Docker daemon is reachable, so that contributors without Docker can still run the
/// rest of the suite. CI runs on ubuntu-latest, where Docker is preinstalled, and therefore never skips.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
internal sealed class RequiresDockerAttribute : SkipAttribute
{
	private static readonly Lazy<Task<bool>> _isAvailable = new(IsDockerAvailableAsync);

	public RequiresDockerAttribute()
		: base("Docker is not available.")
	{
	}

	public override async Task<bool> ShouldSkip(TestRegisteredContext context)
	{
		return !await _isAvailable.Value;
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
		catch (Exception)
		{
			return false;
		}
	}
}
