using DevilDaggersInfo.Web.ApiSpec.Main.WorldRecords;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Test.Utils;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomainMain;

internal sealed class WorldRecordRepositoryTests
{
	private readonly WorldRecordRepository _repository;

	public WorldRecordRepositoryTests()
	{
		DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
			.UseInMemoryDatabase(databaseName: nameof(WorldRecordRepositoryTests))
			.Options;
		TestDbContext dbContext = new(options, Substitute.For<IHttpContextAccessor>(), Substitute.For<ILogContainerService>());
		TestData data = new();
		_repository = new WorldRecordRepository(dbContext, data, data);
	}

	[Test]
	public async Task GetWorldRecords_WithCheater()
	{
		GetWorldRecordDataContainer worldRecordData = await _repository.GetWorldRecordDataAsync();

		await Assert.That(worldRecordData.WorldRecordHolders.Count).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecordHolders[0].Id).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecordHolders[0].WorldRecordCount).IsEqualTo(3);

		const double delta = 0.00001;
		await Assert.That(worldRecordData.WorldRecords.Count).IsEqualTo(3);
		await Assert.That(worldRecordData.WorldRecords[0].Entry.Id).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecords[0].Entry.Time).IsEqualTo(0.0090).Within(delta);
		await Assert.That(worldRecordData.WorldRecords[1].Entry.Id).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecords[1].Entry.Time).IsEqualTo(0.0095).Within(delta);
		await Assert.That(worldRecordData.WorldRecords[2].Entry.Id).IsEqualTo(1);
		await Assert.That(worldRecordData.WorldRecords[2].Entry.Time).IsEqualTo(0.0098).Within(delta);
	}
}
