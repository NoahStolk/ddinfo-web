using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Testcontainers.MariaDb;
using TUnit.Core.Interfaces;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// Owns the single database container for the whole test session, plus one database and one application per test class.
/// TUnit reference-counts shared data sources, so the container starts before the first test that asks for it and is
/// disposed after the last one finishes -- a filtered run that touches no database test never starts Docker.
/// </summary>
internal sealed class DatabaseFixture : IAsyncInitializer, IAsyncDisposable
{
	// Production runs MariaDB 10.3.39, so the container is pinned to exactly that rather than to MySQL, which differs in
	// SQL dialect, default collation and authentication plugin.
	private const string _image = "mariadb:10.3.39";

	private const string _bootstrapDatabase = "ddinfo_bootstrap";

	// The module's default readiness check runs the "mariadb" client, which images before 10.4.6 do not ship, so it would
	// wait forever. A custom wait strategy replaces the default one, and runs the "mysql" client instead.
	private readonly MariaDbContainer _container = new MariaDbBuilder(_image)
		.WithDatabase(_bootstrapDatabase)
		.WithUsername("root")
		.WithPassword("root")
		.WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("mysql", "--host=localhost", "--user=root", "--password=root", "--execute=SELECT 1;"))
		.Build();

	private readonly ConcurrentDictionary<string, Lazy<Task<TestApplication>>> _applications = new(StringComparer.Ordinal);

	public Task InitializeAsync()
	{
		return _container.StartAsync();
	}

	/// <summary>
	/// Returns the application for <paramref name="key"/>, creating its database, schema and host on first use.
	/// <paramref name="configureServices"/> runs after the harness's own service replacements, and only when the
	/// application is created, so every caller using the same key must pass the same configuration.
	/// </summary>
	internal async Task<TestApplication> GetApplicationAsync(string key, Action<IServiceCollection>? configureServices = null)
	{
		// Lazy rather than GetOrAdd's factory directly, so that two test classes racing on the same key cannot both
		// start building an application. A Lazy caches its faulted task forever, though, so a transient failure while
		// creating the database or booting the host would otherwise fail every remaining test in the class rather than
		// just the one that hit it. The faulted entry is evicted and rebuilt once before giving up.
		Lazy<Task<TestApplication>> lazy = GetOrAddLazy(key, configureServices);
		await ((Task)lazy.Value).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
		if (!lazy.Value.IsCompletedSuccessfully)
		{
			// Removes only if the entry is still this instance, so a rebuild started by another test is not evicted.
			_applications.TryRemove(new KeyValuePair<string, Lazy<Task<TestApplication>>>(key, lazy));
			lazy = GetOrAddLazy(key, configureServices);
		}

		return await lazy.Value;
	}

	private Lazy<Task<TestApplication>> GetOrAddLazy(string key, Action<IServiceCollection>? configureServices)
	{
		return _applications.GetOrAdd(key, k => new Lazy<Task<TestApplication>>(() => CreateApplicationAsync(k, configureServices)));
	}

	private async Task<TestApplication> CreateApplicationAsync(string key, Action<IServiceCollection>? configureServices)
	{
		// Production tables use the case-insensitive utf8_general_ci and utf8mb4_general_ci collations. Set it explicitly
		// rather than relying on the server default, which string filters and ORDER BY depend on.
		string database = $"ddinfo_{key.ToLowerInvariant()}";
		await ExecuteOnServerAsync($"CREATE DATABASE `{database}` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;");

		string scratchRoot = Path.Combine(Path.GetTempPath(), "ddinfo-integration-tests", $"{key}-{Guid.NewGuid():N}");
		Directory.CreateDirectory(scratchRoot);

		TestApplication application = new(BuildConnectionString(database), scratchRoot, configureServices);

		await using (AsyncServiceScope scope = application.CreateScope())
		{
			ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			await dbContext.Database.EnsureCreatedAsync();
		}

		await application.ResetAsync();
		return application;
	}

	private string BuildConnectionString(string database)
	{
		// Testcontainers emits Server/Port/Database/Uid/Pwd, all of which Oracle's Connector/NET accepts. The database is
		// swapped through the builder rather than by appending a second Database= key.
		return new MySqlConnectionStringBuilder(_container.GetConnectionString())
		{
			Database = database,
			ConvertZeroDateTime = true,
		}.ConnectionString;
	}

	[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Only called with DDL built from test class names; identifiers cannot be parameterized.")]
	private async Task ExecuteOnServerAsync(string sql)
	{
		await using MySqlConnection connection = new(BuildConnectionString(_bootstrapDatabase));
		await connection.OpenAsync();
		await using MySqlCommand command = connection.CreateCommand();
		command.CommandText = sql;
		await command.ExecuteNonQueryAsync();
	}

	public async ValueTask DisposeAsync()
	{
		foreach (Lazy<Task<TestApplication>> application in _applications.Values)
		{
			if (application is { IsValueCreated: true, Value.IsCompletedSuccessfully: true })
				await (await application.Value).DisposeAsync();
		}

		await _container.DisposeAsync();
	}
}
