using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Collections.Concurrent;
using Testcontainers.MySql;
using TUnit.Core.Interfaces;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// Owns the single MySQL container for the whole test session, plus one database and one application per test class.
/// TUnit reference-counts shared data sources, so the container starts before the first test that asks for it and is
/// disposed after the last one finishes -- a filtered run that touches no integration test never starts Docker.
/// </summary>
internal sealed class MySqlFixture : IAsyncInitializer, IAsyncDisposable
{
	// Pinned deliberately. MySQL 8.4 and 9.x change the default authentication plugin and the default collation, neither
	// of which the Oracle connector this repository uses has been validated against here.
	private const string _image = "mysql:8.0.43";

	/// <summary>
	/// Admin.DatabaseController hard-codes this schema name in its information_schema query, so the container's
	/// bootstrap database uses it. Test classes that need that endpoint to see anything pass this as their key.
	/// </summary>
	internal const string BootstrapDatabase = "devildaggers";

	private readonly MySqlContainer _container = new MySqlBuilder(_image)
		.WithDatabase(BootstrapDatabase)
		.WithUsername("root")
		.WithPassword("root")
		.Build();

	private readonly ConcurrentDictionary<string, Lazy<Task<TestApplication>>> _applications = new(StringComparer.Ordinal);

	public Task InitializeAsync()
	{
		return _container.StartAsync();
	}

	/// <summary>
	/// Returns the application for <paramref name="key"/>, creating its database, schema and host on first use. Pass
	/// <see cref="BootstrapDatabase"/> to get the schema named <c>devildaggers</c>.
	/// </summary>
	internal async Task<TestApplication> GetApplicationAsync(string key)
	{
		// Lazy rather than GetOrAdd's factory directly, so that two test classes racing on the same key cannot both
		// start building an application. A Lazy caches its faulted task forever, though, so a transient failure while
		// creating the database or booting the host would otherwise fail every remaining test in the class rather than
		// just the one that hit it. The faulted entry is evicted and rebuilt once before giving up.
		for (int attempt = 0; ; attempt++)
		{
			Lazy<Task<TestApplication>> lazy = _applications.GetOrAdd(key, k => new Lazy<Task<TestApplication>>(() => CreateApplicationAsync(k)));
			try
			{
				return await lazy.Value;
			}
			catch (Exception) when (attempt == 0)
			{
				// Removes only if the entry is still this instance, so a rebuild started by another test is not evicted.
				_applications.TryRemove(new KeyValuePair<string, Lazy<Task<TestApplication>>>(key, lazy));
			}
		}
	}

	private async Task<TestApplication> CreateApplicationAsync(string key)
	{
		string database = key == BootstrapDatabase ? BootstrapDatabase : $"ddinfo_{key.ToLowerInvariant()}";
		if (database != BootstrapDatabase)
		{
			// Production runs MariaDB with utf8mb4_general_ci. MySQL 8 defaults to utf8mb4_0900_ai_ci, which orders and
			// compares strings differently, so the collation is set explicitly to match.
			await ExecuteOnServerAsync($"CREATE DATABASE `{database}` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;");
		}

		string dataRoot = Path.Combine(Path.GetTempPath(), "ddinfo-integration-tests", $"{key}-{Guid.NewGuid():N}");
		Directory.CreateDirectory(dataRoot);

		TestApplication application = new(BuildConnectionString(database), dataRoot);

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

	private async Task ExecuteOnServerAsync(string sql)
	{
		await using MySqlConnection connection = new(BuildConnectionString(BootstrapDatabase));
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
				await application.Value.Result.DisposeAsync();
		}

		await _container.DisposeAsync();
	}
}
