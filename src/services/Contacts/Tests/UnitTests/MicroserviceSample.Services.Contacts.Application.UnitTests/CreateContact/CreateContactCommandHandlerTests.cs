

using MicroserviceSample.Services.Contacts.Application.Contacts.CreateContact;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace Services.Contacts.Application.UnitTests.CreateContact
{
    public class CreateContactCommandHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepository;

        public CreateContactCommandHandlerTests()
        {
            _contactRepository = new Mock<IContactRepository>();
        }

        [Theory, AutoData]
        public void CreateNewContact_WhenEmptyCommunication_IsPossible(string firstName, string lastName, string companyName)
        {
            // Arrange 
            _contactRepository.Setup(c => c.Add(It.IsAny<ContactEntity>())).Verifiable();
            CreateContactCommandHandler handler = new(_contactRepository.Object);

            //Act
            Task<Guid> id = handler.Handle(new CreateContactCommand(firstName, lastName, companyName, null), CancellationToken.None);

            //Assert
            _contactRepository.Verify(c => c.Add(It.IsAny<ContactEntity>()));
            id.Result.Should().NotBeEmpty();
        }

        [Theory, AutoData]
        public void CreateNewContact_WhenCommunication_IsPossible(string firstName, string lastName, string companyName, List<(string type, string value)>? communications)
        {
            // Arrange 
            _contactRepository.Setup(c => c.Add(It.IsAny<ContactEntity>())).Verifiable();
            CreateContactCommandHandler handler = new(_contactRepository.Object);

            //Act
            Task<Guid> id = handler.Handle(new CreateContactCommand(firstName, lastName, companyName, null), CancellationToken.None);

            //Assert
            _contactRepository.Verify(c => c.Add(It.IsAny<ContactEntity>()));
            id.Result.Should().NotBeEmpty();
        }
    }
}