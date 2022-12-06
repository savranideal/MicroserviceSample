using MicroserviceSample.BuildingBlocks.Application.Exception.Types;
using MicroserviceSample.Services.Contacts.Application.Contacts.CreateContactCommunication;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace Services.Contacts.Application.UnitTests.CreateContactCommunication
{ 
    public class CreateContactCommunicationCommandHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepository;

        public CreateContactCommunicationCommandHandlerTests()
        {
            _contactRepository = new Mock<IContactRepository>();
        }

        [Theory, AutoData]
        public void CreateContactCommunication_WhenExistsContact_IsPossible(string type, string value)
        {
            // Arrange 
            ContactEntity contact = ContactEntity.Create(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>());
            _contactRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(contact)
                .Verifiable();
            CreateContactCommunicationCommandHandler handler = new(_contactRepository.Object);

            //Act
            Task<Guid> id = handler.Handle(new CreateContactCommunicationCommand(contact.Id, type, value), CancellationToken.None);

            //Assert
            _contactRepository.Verify(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            id.Result.Should().NotBeEmpty();
        }

        [Theory, AutoData]
        public void CreateContactCommunication_WhenNotExistsContact_ThrowException(string type, string value)
        {
            // Arrange 
            ContactEntity? contact = null;
            _contactRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!
                .ReturnsAsync(contact)
                .Verifiable();
            CreateContactCommunicationCommandHandler handler = new(_contactRepository.Object);

            //Act
            Func<Task<Guid>> result = () => handler.Handle(new CreateContactCommunicationCommand(It.IsAny<Guid>(), type, value), CancellationToken.None);
            
            //Assert
            result.Should().ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public void CreateContactCommunication_WhenConflictCommucation_ThrowException()
        {
            // Arrange 
            ContactEntity contact = ContactEntity.Create(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>());
            contact.AddCommunication(CommunicationType.Location, "location");
            _contactRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(contact)
                .Verifiable();
            CreateContactCommunicationCommandHandler handler = new(_contactRepository.Object);

            //Act 
            Func<Task<Guid>> result = () => handler.Handle(new CreateContactCommunicationCommand(It.IsAny<Guid>(), "location", "location"), CancellationToken.None);
            
            //Assert
            result.Should().ThrowAsync<ConflictException>();
        }
    }
}
