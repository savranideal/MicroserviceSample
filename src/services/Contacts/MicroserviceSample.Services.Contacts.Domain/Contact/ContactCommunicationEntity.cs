using MicroserviceSample.BuildingBlocks.Domain;

namespace MicroserviceSample.Services.Contacts.Domain.Contact
{
    public class ContactCommunicationEntity : Entity
    {
        private ContactCommunicationEntity()
        {
            // for EF core
        }

        private ContactCommunicationEntity(CommunicationType type, string value, Guid contactId)
        {
            Id = Guid.NewGuid();
            Type = type;
            Value = value;
            ContactId = contactId;
        }

        public Guid Id { get; private set; }
        public Guid ContactId { get; private set; }
        public CommunicationType Type { get; private set; }
        public string Value { get; private set; }
        public virtual ContactEntity Contact { get; private set; }

        public static ContactCommunicationEntity Create(CommunicationType type, string value, Guid ContactId)
        {
            return new ContactCommunicationEntity(type,value,ContactId);
        }
    }
}
