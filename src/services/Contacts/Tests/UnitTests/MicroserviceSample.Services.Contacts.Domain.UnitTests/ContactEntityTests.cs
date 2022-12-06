using AutoFixture.Xunit2;
using FluentAssertions;
using MicroserviceSample.Services.Contacts.Domain.Contact;

namespace Services.Contacts.Domain.UnitTests
{
    public class ContactEntityTests
    {
        [Theory, AutoData]
        public void CreateContact_IsSuccessful(string firstName,string lastName,string companyName)
        {
            //Arrange - Act
            ContactEntity entity = ContactEntity.Create(firstName, lastName, companyName);
              
            // Assert 
            entity.DomainEvents.Should().NotBeNull();
            entity.DomainEvents.Should().HaveCount(1);
        }

        [Theory, AutoData]
        public void AddCommunication_WhenCommunicationEmpty_IsSuccessful(string firstName,string lastName,string companyName)
        {
            //Arrange 
            ContactEntity entity = ContactEntity.Create(firstName, lastName, companyName);

            // Act
            entity.AddCommunication(CommunicationType.Email, "example@example.com");

            // Assert 
            entity.Communications.Should().NotBeNull();
            entity.Communications.Should().HaveCount(1);
        }

        [Theory, AutoData]
        public void DeleteCommunication_WhenCommunicationNotEmpty_IsSuccessful(string firstName,string lastName,string companyName)
        {
            //Arrange 
            ContactEntity entity = ContactEntity.Create(firstName, lastName, companyName);

            // Act
            entity.AddCommunication(CommunicationType.Email, "example@example.com");
            entity.DeleteCommunication(entity.Communications.FirstOrDefault()!);
            // Assert 
            entity.Communications.Should().BeEmpty();
            entity.Communications.Should().HaveCount(0);
        }

        [Theory, AutoData]
        public void DeleteCommunication_WhenCommunicationEmpty_IsPossible(string firstName,string lastName,string companyName)
        {
            //Arrange 
            ContactEntity entity = ContactEntity.Create(firstName, lastName, companyName);

            // Act
            Action result = () =>
            {
                entity.DeleteCommunication(entity.Communications.FirstOrDefault()!);
            }; 

            // Assert 
            result.Should().NotThrow<Exception>();
        }
    }
}