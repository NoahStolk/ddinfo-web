namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// Base class for tests that need the application. Each derived class gets its own database and scratch data directory,
/// keyed by its type name, so classes run in parallel with each other. Mark derived classes
/// <c>[NotInParallel(nameof(TheClass))]</c> so their own cases do not race on that shared state.
/// </summary>
internal abstract class ApplicationTest
{
	[ClassDataSource<MySqlFixture>(Shared = SharedType.PerTestSession)]
	public required MySqlFixture MySql { get; init; }

	protected TestApplication App { get; private set; } = null!;

	/// <summary>
	/// Overridden by classes that need the schema to be named <c>devildaggers</c>, which
	/// <c>Admin.DatabaseController</c> reports on.
	/// </summary>
	protected virtual string DatabaseKey => GetType().Name;

	/// <summary>
	/// Set to false by classes that never read or write the database, to skip the truncate before every case.
	/// </summary>
	protected virtual bool ResetBeforeEachTest => true;

	[Before(HookType.Test)]
	public async Task InitializeApplicationAsync()
	{
		App = await MySql.GetApplicationAsync(DatabaseKey);
		if (ResetBeforeEachTest)
			await App.ResetAsync();
	}
}
