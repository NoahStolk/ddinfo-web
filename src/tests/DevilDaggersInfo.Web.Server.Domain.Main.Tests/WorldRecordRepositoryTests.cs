using DevilDaggersInfo.Api.Main.WorldRecords;
using DevilDaggersInfo.Web.Server.Domain.Main.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Main.Tests.Utils;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Tests;

[TestClass]
public class WorldRecordRepositoryTests
{
	private readonly WorldRecordRepository _repository;

	public WorldRecordRepositoryTests()
	{
		DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
			.UseInMemoryDatabase(databaseName: nameof(WorldRecordRepositoryTests))
			.Options;
		TestDbContext dbContext = new(options, new Mock<IHttpContextAccessor>().Object, new Mock<ILogContainerService>().Object);
		TestData data = new();
		_repository = new(dbContext, data, data);
	}

	[TestMethod]
	public void GetWorldRecords_WithCheater()
	{
		GetWorldRecordDataContainer worldRecordData = _repository.GetWorldRecordData();

		Assert.AreEqual(1, worldRecordData.WorldRecordHolders.Count);
		Assert.AreEqual(1, worldRecordData.WorldRecordHolders[0].Id);
		Assert.AreEqual(3, worldRecordData.WorldRecordHolders[0].WorldRecordCount);

		const double delta = 0.00001;
		Assert.AreEqual(3, worldRecordData.WorldRecords.Count);
		Assert.AreEqual(1, worldRecordData.WorldRecords[0].Entry.Id);
		Assert.AreEqual(0.0090, worldRecordData.WorldRecords[0].Entry.Time, delta);
		Assert.AreEqual(1, worldRecordData.WorldRecords[1].Entry.Id);
		Assert.AreEqual(0.0095, worldRecordData.WorldRecords[1].Entry.Time, delta);
		Assert.AreEqual(1, worldRecordData.WorldRecords[2].Entry.Id);
		Assert.AreEqual(0.0098, worldRecordData.WorldRecords[2].Entry.Time, delta);
	}
}
