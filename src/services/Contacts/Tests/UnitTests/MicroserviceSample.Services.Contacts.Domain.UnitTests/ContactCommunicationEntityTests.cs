using AutoFixture.Xunit2;
using FluentAssertions;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace Services.Contacts.Domain.UnitTests
{
    public class ContactCommunicationEntityTests
    {
        [Theory, AutoData]
        public void CreateContactCommunication_IsSuccessful(CommunicationType type,string value,Guid contactId)
        {
            //Arrange - Act
            ContactCommunicationEntity entity = ContactCommunicationEntity.Create(type, value, contactId);
              
            // Assert 
            entity.Id.Should().NotBeEmpty(); 
        } 
    }
}