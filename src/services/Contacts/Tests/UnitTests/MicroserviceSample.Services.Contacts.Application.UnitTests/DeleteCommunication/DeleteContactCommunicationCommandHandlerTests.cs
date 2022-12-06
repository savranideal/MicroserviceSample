using MediatR;
using MicroserviceSample.BuildingBlocks.Application.Exception.Types; 
using MicroserviceSample.Services.Contacts.Application.Contacts.DeleteCommunication;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace Services.Contacts.Application.UnitTests.DeleteCommunication
{
    public class DeleteContactCommunicationCommandHandlerTests
    {
       private readonly Mock<IContactRepository> _contactRepository;

        public DeleteContactCommunicationCommandHandlerTests()
        {
            _contactRepository = new Mock<IContactRepository>();
        }

        [Fact]
        public void DeleteContactCommunication_WhenExistsContact_IsPossible()
        {
            // Arrange 
            ContactEntity contact = ContactEntity.Create(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>());
            contact.AddCommunication(CommunicationType.Location, "istanbul");
            _contactRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(contact)
                .Verifiable(); 
            DeleteContactCommunicationCommandHandler handler = new(_contactRepository.Object);

            //Act
            Task<Unit> id = handler.Handle(new DeleteContactCommunicationCommand(contact.Id, contact.Communications.First().Id), CancellationToken.None);

            //Assert
            _contactRepository.Verify(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())); 
        }

        [Fact]
        public void DeleteContactCommunication_WhenNotExistsContact_ThrowException()
        {
            // Arrange 
            ContactEntity? contact = null;
            _contactRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!
                .ReturnsAsync(contact)
                .Verifiable();
            DeleteContactCommunicationCommandHandler handler = new(_contactRepository.Object);

            //Act
            Func<Task<Unit>> result = () => handler.Handle(new DeleteContactCommunicationCommand(It.IsAny<Guid>(), It.IsAny<Guid>()), CancellationToken.None);
            
            //Assert
            result.Should().ThrowAsync<ResourceNotFoundException>().WithMessage("Contact not found");
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
            DeleteContactCommunicationCommandHandler handler = new(_contactRepository.Object);

            //Act
            Func<Task<Unit>> result = () => handler.Handle(new DeleteContactCommunicationCommand(contact.Id, It.IsAny<Guid>()), CancellationToken.None);

            //Assert
            result.Should().ThrowAsync<ResourceNotFoundException>().WithMessage("Communication not found");
        } 
    }
}