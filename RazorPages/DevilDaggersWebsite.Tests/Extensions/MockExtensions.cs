using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DevilDaggersWebsite.Tests.Extensions
{
	public static class MockExtensions
	{
		public static Mock<TDbContext> SetUpDbSet<TDbContext, TEntity>(this Mock<TDbContext> mockDbContext, Expression<Func<TDbContext, DbSet<TEntity>>> expression, Mock<DbSet<TEntity>> mockDbSet)
			where TDbContext : DbContext
			where TEntity : class
		{
			mockDbContext.Setup(expression).Returns(mockDbSet.Object);

			return mockDbContext;
		}

		public static Mock<DbSet<TEntity>> SetUpDbSet<TEntity>(this Mock<DbSet<TEntity>> mockDbSet, params TEntity[] values)
			where TEntity : class
		{
			IQueryable<TEntity>? dataAsQueryable = values.AsQueryable();
			mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(dataAsQueryable.Provider);
			mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(dataAsQueryable.Expression);
			mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(dataAsQueryable.ElementType);
			mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(dataAsQueryable.GetEnumerator);

			List<TEntity>? dataAsList = values.ToList();
			mockDbSet.Setup(dbs => dbs.Add(It.IsAny<TEntity>())).Callback<TEntity>((c) => dataAsList.Add(c));
			mockDbSet.Setup(dbs => dbs.Remove(It.IsAny<TEntity>())).Callback<TEntity>((c) => dataAsList.Remove(c));

			return mockDbSet;
		}
	}
}
