namespace DevilDaggersInfo.Test.Web.BlazorWasm.Server.Extensions;

public static class MockExtensions
{
	public static Mock<TDbContext> SetUpDbSet<TDbContext, TEntity>(this Mock<TDbContext> mockDbContext, Expression<Func<TDbContext, DbSet<TEntity>>> expression, Mock<DbSet<TEntity>> mockDbSet)
		where TDbContext : DbContext
		where TEntity : class
	{
		mockDbContext.Setup(expression).Returns(mockDbSet.Object);

		return mockDbContext;
	}
}
