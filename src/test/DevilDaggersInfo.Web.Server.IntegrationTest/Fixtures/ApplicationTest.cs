using Microsoft.Extensions.DependencyInjection;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// Base class for tests that need the application. Each derived class gets its own database and scratch directory,
/// keyed by its type name, so classes run in parallel with each other. Mark derived classes
/// <c>[NotInParallel(nameof(TheClass))]</c> so their own cases do not race on that shared state.
/// </summary>
[RequiresDocker]
internal abstract class ApplicationTest
{
	[ClassDataSource<DatabaseFixture>(Shared = SharedType.PerTestSession)]
	public required DatabaseFixture Database { get; init; }

	// ! Assigned by the before-test hook, which TUnit runs before every test method.
	protected TestApplication App { get; private set; } = null!;

	/// <summary>
	/// Set to false by classes that never read or write the database or the data root, to skip the reset before every case.
	/// </summary>
	protected virtual bool ResetBeforeEachTest => true;

	/// <summary>
	/// Overridden by classes that need to replace a service in the host, for example to wrap the file system. Runs once,
	/// when the class's application is created.
	/// </summary>
	protected virtual void ConfigureServices(IServiceCollection services)
	{
	}

	[Before(HookType.Test)]
	public async Task InitializeApplicationAsync()
	{
		App = await Database.GetApplicationAsync(GetType().Name, ConfigureServices);
		if (ResetBeforeEachTest)
			await App.ResetAsync();
	}
}
