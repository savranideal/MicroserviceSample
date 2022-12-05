using System.Xml.Linq;

using MicroserviceSample.BuildingBlocks.Domain;
using MicroserviceSample.Services.Contacts.Domain.Contact.Events;

namespace MicroserviceSample.Services.Contacts.Domain.Contact
{
    public class ContactEntity : Entity
    {
        private ContactEntity()
        {
            /// for ef core
            Communications = new List<ContactCommunicationEntity>();
        }

        private ContactEntity(string firstname, string lastname, string companyName)
        {
            Id = Guid.NewGuid();
            FirstName = firstname;
            LastName = lastname;
            CompanyName = companyName;
            Communications = new List<ContactCommunicationEntity>();
            AddDomainEvent(new ContactCreatedDomainEvent(Id));
        }

        public Guid Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string CompanyName { get; private set; }

        public virtual ICollection<ContactCommunicationEntity> Communications { get; private set; }

        public Guid AddCommunication(CommunicationType type, string value)
        {
            ContactCommunicationEntity communication = ContactCommunicationEntity.Create(type, value, Id);
            Communications.Add(communication);
            return communication.Id;
        }

        public void DeleteCommunication(ContactCommunicationEntity communicationEntity)
        {   
            Communications.Remove(communicationEntity);
        }

        public static ContactEntity Create(string firstname, string lastname, string companyName)
        {
            return new ContactEntity(firstname, lastname, companyName);
        }
    }
}
