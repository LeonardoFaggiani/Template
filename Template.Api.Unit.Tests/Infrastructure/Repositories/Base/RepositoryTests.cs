using FluentAssertions;

using Moq;

using NetDevPack.Domain;

using Template.Api.Infrastructure.Data;
using Template.Api.Infrastructure.Repositories.Base;
using Template.Api.Unit.Tests.Base;

namespace Template.Api.Unit.Tests.Infrastructure.Repositories.Base
{
    public class EntityTestClass : Entity
    {
        public int Id { get; set; }
    }

    public class RepositoryTests : BaseTestClass<Repository<EntityTestClass>>
    {
        public TemplateContext DbContext { get; set; }
        public RepositoryTests()
        {

            this.DbContext = MockDbContext.Of<TemplateContext>();
            this.Sut = new Repository<EntityTestClass>(this.DbContext);
        }

        public class TheConstructor : RepositoryTests
        {
            [Fact]
            public void Should_throw_ArgumentNullException_when_dbContext_is_null()
            {
                // Arrange
                this.DbContext = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>("context", () => new Repository<EntityTestClass>(this.DbContext));
            }
        }

        public class TheMethod_All : RepositoryTests
        {
            [Fact]
            public void Should_return_an_queryable_from_the_entity()
            {
                // Arrange
                var repository = new List<EntityTestClass>() { };
                Mock.Get(this.DbContext).SetupDbSet(repository);

                // Act
                var result = Sut.All();

                // Assert
                result.Should().BeAssignableTo<IQueryable<EntityTestClass>>();
            }
        }

        public class TheMethod_Update : RepositoryTests
        {
            [Fact]
            public void Should_update_entity()
            {
                // Arrange
                var entityToUpdate = new EntityTestClass { Id = 1 };

                // Act
                Sut.Update(entityToUpdate);

                // Assert
                Mock.Get(this.DbContext).Verify(x => x.Update(It.IsAny<EntityTestClass>()), Times.Once);
            }
        }

        public class TheMethod_Add : RepositoryTests
        {
            [Fact]
            public void Should_insert_entity()
            {
                // Arrange
                var entityToInsert = new EntityTestClass { Id = 1 };

                // Act
                Sut.Add(entityToInsert);

                // Assert
                Mock.Get(this.DbContext).Verify(x => x.Add(It.IsAny<EntityTestClass>()), Times.Once);
            }
        }
    }
}
