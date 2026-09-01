using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Clients.Clubber;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Utils;
using DevilDaggersInfo.Web.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System.Net.Http.Headers;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Fixtures;

/// <summary>
/// One in-process instance of the real application, bound to one database and one scratch data directory.
/// </summary>
internal sealed class TestApplication : WebApplicationFactory<Program>
{
	// ConfigureJwtBearerOptions builds a SymmetricSecurityKey from the ASCII bytes of this value, and HS256 rejects
	// anything shorter than 256 bits.
	private const string _jwtKey = "integration-test-only-key-0123456789";

	// AesBase32Wrapper requires exactly 16 characters for each of the initialization vector, password and salt.
	private const string _customLeaderboardsSecret = "0123456789abcdef";

	private readonly string _connectionString;
	private readonly string _dataRoot;

	internal TestApplication(string connectionString, string dataRoot)
	{
		_connectionString = connectionString;
		_dataRoot = dataRoot;
	}

	/// <summary>
	/// The substitute the host resolves for <see cref="IDdLeaderboardService"/>. Configure it before issuing requests;
	/// the real implementation talks to the official Devil Daggers leaderboard over the network.
	/// </summary>
	internal IDdLeaderboardService DdLeaderboardService { get; } = Substitute.For<IDdLeaderboardService>();

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		// Program.cs registers DiscordUserIdFetchBackgroundService, LeaderboardHistoryBackgroundService and
		// PlayerNameFetchBackgroundService only outside Development, so three of the six hosted services -- all of which
		// poll the network on a timer -- never get registered in the first place.
		builder.UseEnvironment(Environments.Development);
		builder.UseSetting(WebHostDefaults.ContentRootKey, ServerProject.Directory);

		builder.ConfigureAppConfiguration(configuration =>
		{
			// AddValidatedOptions binds these sections with ErrorOnUnknownConfiguration = true, so every key here must
			// be a real property of the corresponding options record. Development user secrets are also loaded for this
			// environment, so these values are added last to win over anything configured on the machine.
			configuration.AddInMemoryCollection(new Dictionary<string, string?>(StringComparer.Ordinal)
			{
				["Authentication:JwtKey"] = _jwtKey,
				["CustomLeaderboards:InitializationVector"] = _customLeaderboardsSecret,
				["CustomLeaderboards:Password"] = _customLeaderboardsSecret,
				["CustomLeaderboards:Salt"] = _customLeaderboardsSecret,
				["Discord:BotToken"] = string.Empty,
				["MySql:ConnectionString"] = _connectionString,
				["Sentry:Dsn"] = string.Empty,
			});
		});

		builder.ConfigureTestServices(services =>
		{
			RemoveApplicationHostedServices(services);

			// AddDbContext registers its options with TryAdd, so the existing registrations have to go before the
			// container's connection string can take their place. Configuration alone would be enough today, but this
			// keeps the test independent of when AddValidatedOptions reads the configuration.
			services.RemoveAll<ApplicationDbContext>();
			services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
			services.RemoveAll<DbContextOptions>();
			services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(_connectionString, providerOptions => providerOptions.EnableRetryOnFailure(5)));

			// FileSystemService is a singleton rooted at a path relative to the process working directory, which every
			// test in the assembly shares. The real implementation is kept -- its file ordering behaviour is the subject
			// of a regression test -- and only the root is redirected.
			services.RemoveAll<IFileSystemService>();
			services.AddSingleton<IFileSystemService>(new FileSystemService(_dataRoot));

			// PersistKeysToFileSystem(new DirectoryInfo("keys")) is likewise working-directory relative.
			services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(_dataRoot, "keys")));

			services.RemoveAll<IDdLeaderboardService>();
			services.AddSingleton(DdLeaderboardService);

			// ClubberClient is concrete, so its primary handler is the only seam. Its sole consumer is a hosted service
			// that has just been removed, so this is a guard rather than a stub that anything depends on.
			services.AddHttpClient(nameof(ClubberClient)).ConfigurePrimaryHttpMessageHandler(() => new StubHttpMessageHandler());
		});
	}

	/// <summary>
	/// Removes the hosted services declared by the server assembly. A blanket removal of <see cref="IHostedService"/>
	/// would also take out GenericWebHostService -- the web server itself. Filtering by assembly means a hosted service
	/// added to Program.cs later is excluded automatically.
	/// </summary>
	private static void RemoveApplicationHostedServices(IServiceCollection services)
	{
		List<ServiceDescriptor> descriptors =
		[
			.. services.Where(sd => sd.ServiceType == typeof(IHostedService) && sd.ImplementationType?.Assembly == typeof(Program).Assembly),
		];

		foreach (ServiceDescriptor descriptor in descriptors)
			services.Remove(descriptor);
	}

	/// <summary>
	/// Truncates every table, re-seeds the roles, and empties the scratch data directory. Truncation resets
	/// AUTO_INCREMENT, which matters because replay files are named after the custom entry ID that produced them.
	/// </summary>
	internal async Task ResetAsync()
	{
		await using AsyncServiceScope scope = Services.CreateAsyncScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		// Derived from the model so that a newly added entity cannot be forgotten. Owned types share their owner's
		// table, hence the distinct.
		List<string> tables =
		[
			.. dbContext.Model.GetEntityTypes()
				.Select(e => e.GetTableName())
				.Where(t => !string.IsNullOrEmpty(t))
				.Select(t => t ?? string.Empty)
				.Distinct(StringComparer.Ordinal)
				.Order(StringComparer.Ordinal),
		];

		await dbContext.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 0;");
		foreach (string table in tables)
			await dbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE `{table}`;");

		await dbContext.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 1;");

		// UserRoles.RoleName is a foreign key to Roles.Name, so the roles have to exist before any user can be given one.
		dbContext.Roles.AddRange(
			new RoleEntity { Name = Roles.Admin },
			new RoleEntity { Name = Roles.CustomLeaderboards },
			new RoleEntity { Name = Roles.Mods },
			new RoleEntity { Name = Roles.Players },
			new RoleEntity { Name = Roles.Spawnsets });
		await dbContext.SaveChangesAsync();

		if (Directory.Exists(_dataRoot))
			Directory.Delete(_dataRoot, recursive: true);

		// FileSystemService creates these in its constructor only, and it is a singleton that outlives the reset.
		Directory.CreateDirectory(_dataRoot);
		foreach (DataSubDirectory subDirectory in Enum.GetValues<DataSubDirectory>())
			Directory.CreateDirectory(Path.Combine(_dataRoot, subDirectory.ToString()));
	}

	/// <summary>
	/// Runs <paramref name="seed"/> against a fresh scope and saves.
	/// </summary>
	internal async Task SeedAsync(Action<ApplicationDbContext> seed)
	{
		await using AsyncServiceScope scope = Services.CreateAsyncScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		seed(dbContext);
		await dbContext.SaveChangesAsync();
	}

	/// <summary>
	/// Resolves a scoped service, for tests that exercise the domain layer directly rather than over HTTP. The scope is
	/// returned alongside it and must be disposed by the caller.
	/// </summary>
	internal AsyncServiceScope CreateScope()
	{
		return Services.CreateAsyncScope();
	}

	internal HttpClient CreateApiClient(string? jwt = null)
	{
		HttpClient client = CreateClient(new WebApplicationFactoryClientOptions
		{
			// An https base address makes UseHttpsRedirection short-circuit on Request.IsHttps rather than attempt a
			// redirect to a port it cannot determine inside the test host.
			BaseAddress = new Uri("https://localhost/"),
			AllowAutoRedirect = false,
		});

		if (jwt != null)
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwt);

		return client;
	}

	/// <summary>
	/// Creates a user with the given roles and returns a JWT for it, minted by the production <see cref="UserManager"/>.
	/// </summary>
	internal async Task<string> CreateJwtAsync(string name, params string[] roles)
	{
		await using AsyncServiceScope scope = Services.CreateAsyncScope();
		ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		PasswordValidator.CreatePasswordHash("Integration-test-1", out byte[] hash, out byte[] salt);
		UserEntity user = new()
		{
			Name = name,
			PasswordHash = hash,
			PasswordSalt = salt,
			DateRegistered = DateTime.UtcNow,
		};
		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync();

		dbContext.UserRoles.AddRange(roles.Select(role => new UserRoleEntity { UserId = user.Id, RoleName = role }));
		await dbContext.SaveChangesAsync();

		// GenerateJwt reads the Role navigation to build the role claims, so it has to be loaded.
		// ! Navigation property.
		UserEntity loaded = await dbContext.Users
			.Include(u => u.UserRoles)!
			.ThenInclude(ur => ur.Role)
			.FirstAsync(u => u.Id == user.Id);

		return scope.ServiceProvider.GetRequiredService<UserManager>().GenerateJwt(loaded);
	}
}
