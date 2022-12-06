using FluentAssertions;
using MicroserviceSample.BuildingBlocks.Domain;
using Moq;

namespace MicroserviceSample.BuildingBlocks.UnitTests
{ 
    public class EntityTests
    {
        [Fact]
        public void AddDomainEvent_NullEvent_SetNull()
        {
            // Arrange
            Mock<Entity> entity = new();

            //Act
            entity.Object.AddDomainEvent(null);

            // Assert
            entity.Object.DomainEvents.Should().NotBeNull();
            entity.Object.DomainEvents.Should().HaveCount(1);
        }

        [Fact]
        public void AddDomainEvent_NotNullEventParam_SetEvent()
        {
            // Arrange
            Mock<Entity> entity = new();
            Mock<IDomainEvent> domainEvent = new();
            DateTime now = DateTime.UtcNow;
            domainEvent.SetupGet(v => v.OccurredOn).Returns(now);

            //Act
            entity.Object.AddDomainEvent(domainEvent.Object);

            //Assert
            entity.Object.DomainEvents.Should().NotBeNull();
            entity.Object.DomainEvents.Should().HaveCount(1);
            entity.Object.DomainEvents.First().OccurredOn.Should().Be(now);
        }

        [Fact]
        public void ClearDomainEvents_NotNullEventParam_SetEmpty()
        {
            // Arrange
            Mock<Entity> entity = new();
            Mock<IDomainEvent> domainEvent = new();
            domainEvent.SetupGet(v => v.OccurredOn).Returns(DateTime.UtcNow);

            //Act
            entity.Object.AddDomainEvent(domainEvent.Object);
            entity.Object.ClearDomainEvents();

            // Assert
            entity.Object.DomainEvents.Should().NotBeNull();
            entity.Object.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void ClearDomainEvents_NullEvent_SetEmpty()
        {
            // Arrange
            Mock<Entity> entity = new();
            
            //Act
            entity.Object.ClearDomainEvents();

            // Assert
            entity.Object.DomainEvents.Should().BeNull();
        }
    }
}