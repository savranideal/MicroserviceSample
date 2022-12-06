using MediatR;
using MicroserviceSample.BuildingBlocks.Application.Exception.Types; 
using MicroserviceSample.Services.Contacts.Application.Contacts.DeleteContact;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace Services.Contacts.Application.UnitTests.DeleteContact
{
    public class DeleteContactCommandHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepository;

        public DeleteContactCommandHandlerTests()
        {
            _contactRepository = new Mock<IContactRepository>();
        }

        [Fact]
        public void DeleteContact_WhenExistsContact_IsPossible()
        {
            // Arrange 
            ContactEntity contact = ContactEntity.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            _contactRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(contact)
                .Verifiable();
            _contactRepository.Setup(c => c.Remove(contact))
                .Verifiable();
            DeleteContactCommandHandler handler = new(_contactRepository.Object);

            //Act
            handler.Handle(new DeleteContactCommand(contact.Id), CancellationToken.None);

            //Assert
            _contactRepository.Verify(c => c.GetByIdAsync(contact.Id, It.IsAny<CancellationToken>()));
            _contactRepository.Verify(c => c.Remove(contact)); 
        }

        [Fact]
        public void DeleteContact_WhenNotExistsContact_ThrowException()
        {
            // Arrange 
            ContactEntity? contact = null;
            _contactRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!
                .ReturnsAsync(contact)
                .Verifiable();
            DeleteContactCommandHandler handler = new(_contactRepository.Object);

            //Act
            Func<Task<Unit>> result = () => handler.Handle(new DeleteContactCommand(It.IsAny<Guid>()), CancellationToken.None);

            //Assert
            result.Should().ThrowAsync<ResourceNotFoundException>().WithMessage("contact not found");
        } 
    }
}
