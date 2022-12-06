using MicroserviceSample.BuildingBlocks.Application.Exception.Types; 
using MicroserviceSample.Services.Contacts.Application.Contacts.GetContacts;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace Services.Contacts.Application.UnitTests.GetContacts
{
    public class GetContactQueryHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepository;

        public GetContactQueryHandlerTests()
        {
            _contactRepository = new Mock<IContactRepository>();
        }

        [Fact]
        public void GetAllContact_IsSucces()
        {
            // Arrange  
            List<ContactEntity> database = GetContactList();
            _contactRepository.Setup(c => c.AllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(database)
                .Verifiable();
            GetContactQueryHandler handler = new(_contactRepository.Object);

            //Act
            Task<ContactDto[]> response = handler.Handle(new GetContactsQuery(), CancellationToken.None);

            //Assert
            _contactRepository.Verify(c => c.AllAsync(It.IsAny<CancellationToken>()));
            response.Result.Should().HaveCount(database.Count);
        }

        [Fact]
        public void GetContactById_WhenDbExists_IsSucces()
        {
            // Arrange  
            List<ContactEntity> database = GetContactList();
            _contactRepository.Setup(c => c.GetByIdAsync(database[0].Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(database[0])
                .Verifiable();
            GetContactQueryHandler handler = new(_contactRepository.Object);

            //Act
            Task<ContactDto> response = handler.Handle(new GetContactQuery(database[0].Id), CancellationToken.None);

            //Assert
            _contactRepository.Verify(c => c.GetByIdAsync(database[0].Id, It.IsAny<CancellationToken>()));
            response.Result.FirstName.Should().Be("ideal");
        }

        [Fact]
        public void GetContactById_WhenDbNotExists_ThrowException()
        {
            // Arrange   
            ContactEntity contact = null;
            _contactRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!
                .ReturnsAsync(contact)
                .Verifiable();
            GetContactQueryHandler handler = new(_contactRepository.Object);
             
            //Act
            Func<Task<ContactDto>> response = () =>
                handler.Handle(new GetContactQuery(It.IsAny<Guid>()), CancellationToken.None);

            //Assert
            response.Should().ThrowAsync<ResourceNotFoundException>().WithMessage("contact not found"); 
        }

        private static List<ContactEntity> GetContactList()
        {
            List<ContactEntity> database = new List<ContactEntity>
            {
                ContactEntity.Create("ideal", "şavran", "Savran A.Ş"),
                ContactEntity.Create("Tony", "Stark", "stark industries")
            };
            database[0].AddCommunication(CommunicationType.Location, "İstanubl");
            database[0].AddCommunication(CommunicationType.Phone, "507-598-96-85");
            database[0].AddCommunication(CommunicationType.Email, "ideal@savran.com");

            database[1].AddCommunication(CommunicationType.Location, "Miami");
            database[1].AddCommunication(CommunicationType.Phone, "9856-965-96-96");
            database[1].AddCommunication(CommunicationType.Email, "stark@stark.com");

            return database;
        }
    }
}
