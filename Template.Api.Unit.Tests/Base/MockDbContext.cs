using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

using NetDevPack.Mediator;

namespace Template.Api.Unit.Tests.Base
{
    public static class MockDbContext
    {
        public static TContext Of<TContext>() where TContext : DbContext
        {
            return new Mock<TContext>(new DbContextOptionsBuilder<TContext>().Options, Mock.Of<IMediatorHandler>()).Object;
        }

        public static void SetupDbSet<TContext, TSet>(this Mock<TContext> mockContext, IEnumerable<TSet> setData)
            where TContext : DbContext
            where TSet : class
        {
            mockContext.Setup(c => c.Set<TSet>()).Returns(setData.AsQueryable().BuildMockDbSet().Object);
        }

        public static DbSet<TSet> MockSet<TSet>(this IEnumerable<TSet> setData)
            where TSet : class
        {
            return setData.AsQueryable().BuildMockDbSet().Object;
        }
    }
}
